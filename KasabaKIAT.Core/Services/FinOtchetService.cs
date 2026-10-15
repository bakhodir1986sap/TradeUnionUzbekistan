using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.DTOs.Reports;
using Microsoft.EntityFrameworkCore;

namespace KasabaKIAT.Core.Services;

public class FinOtchetService(KasabaKIATContext db) : IFinOtchetService
{
    private enum FinOtchDataType { DebitOSTATOK = 0, CreditOSTATOK = 1, OBOROT = 2, VIBORKA = 3, EXPRESSION = 4, DEFAULT0 = 5 }
    private enum OtchetPart { IspolneniyaByudjeta = 1, Balance = 2, Svedeniya = 3, Rasshifrovka = 4 }

    private record FinOtchetRow(string Code, decimal RealValue, decimal PlanValue);

    private static (int prevYear, int prevKv) GetPrevQuarter(int year, int kvartal) =>
        kvartal == 1 ? (year - 1, 4) : (year, kvartal - 1);

    private static int GetMaxMonth(int kvartal) => kvartal switch
    {
        1 => 3,
        2 => 6,
        3 => 9,
        _ => 12
    };

    public async Task<TotalOutput> GetFinOtchetAsync(int year, int kvartal, string departmentCode, bool isSvodniy, CancellationToken ct = default)
    {
        var (prevYear, prevKv) = GetPrevQuarter(year, kvartal);
        int maxMonth = GetMaxMonth(kvartal);
        int prevMaxMonth = GetMaxMonth(prevKv);

        // Load all settings
        var settings = await db.TbbuFinOtchetSettings.ToListAsync(ct);

        // Load planCode→finCode mapping (DICTIONARY_TYPE_ID == 16)
        var planCodeMap = await db.TbcsDictionaries
            .Where(d => d.DICTIONARY_TYPE_ID == 16)
            .Select(d => new { d.CODE, d.NAME_RU })
            .ToListAsync(ct);
        var planCodeFinCode = planCodeMap
            .Where(x => x.CODE != null && x.NAME_RU != null)
            .ToDictionary(x => x.CODE!, x => x.NAME_RU!);

        // Load provodki for current period
        var provodki = await db.Database.SqlQueryRaw<TbbuProvodkiRow>(
            $"SELECT TRDT, DEPARTMENT_BASE, PLATELRSCH, POLUCHRSCH, SUMMA FROM tbbu_provodki WHERE YEAR(TRDT) = @p0 AND MONTH(TRDT) <= @p1",
            year, maxMonth)
            .ToListAsync(ct);

        // Load provodki for previous period (for compare)
        var provodkiPrev = await db.Database.SqlQueryRaw<TbbuProvodkiRow>(
            $"SELECT TRDT, DEPARTMENT_BASE, PLATELRSCH, POLUCHRSCH, SUMMA FROM tbbu_provodki WHERE YEAR(TRDT) = @p0 AND MONTH(TRDT) <= @p1",
            prevYear, prevMaxMonth)
            .ToListAsync(ct);

        // Load balance for current period
        var balances = await db.Database.SqlQueryRaw<TbbuBalanceRow>(
            "SELECT CUR_YEAR, CUR_KV, ACCOUNT_CODE, DEPARTMENT_BASE, DEBIT_BAL, CREDIT_BAL FROM tbbu_balance WHERE CUR_YEAR = @p0 AND CUR_KV = @p1",
            year, kvartal)
            .ToListAsync(ct);

        // Load balance for previous period
        var balancesPrev = await db.Database.SqlQueryRaw<TbbuBalanceRow>(
            "SELECT CUR_YEAR, CUR_KV, ACCOUNT_CODE, DEPARTMENT_BASE, DEBIT_BAL, CREDIT_BAL FROM tbbu_balance WHERE CUR_YEAR = @p0 AND CUR_KV = @p1",
            prevYear, prevKv)
            .ToListAsync(ct);

        // Load plan rows
        var planRows = await db.Database.SqlQueryRaw<TbbuPlanRow>(
            "SELECT SOATO_CODE, YEAR, ITEM_CODE, QUARTER1, HALFYEAR, MONTH9, GODOVOY FROM tbbu_planbyudjetregion WHERE YEAR = @p0",
            year)
            .ToListAsync(ct);

        // Compute IspolneniyaByudjeta section
        var ispolSettings = settings.Where(s => s.OTCHET_PART == (int)OtchetPart.IspolneniyaByudjeta).ToList();
        var finOtchetRows = FillFactData(ispolSettings, provodki, balances, planRows, year, kvartal, departmentCode, isSvodniy, planCodeFinCode);

        // Compute Balance section (current period)
        var balanceSettings = settings.Where(s => s.OTCHET_PART == (int)OtchetPart.Balance).ToList();
        var balanceRows = FillFactDataBalance(balanceSettings, balances, departmentCode, isSvodniy);

        // Compute Balance compare section (previous period)
        var balanceRowsPrev = FillFactDataBalance(balanceSettings, balancesPrev, departmentCode, isSvodniy);

        // Compute Svedeniya section (OtherOutput rasxod matrix)
        var svedeniyaSettings = settings.Where(s => s.OTCHET_PART == (int)OtchetPart.Svedeniya).ToList();
        var rasshifrovkaSettings = settings.Where(s => s.OTCHET_PART == (int)OtchetPart.Rasshifrovka).ToList();

        var result = new TotalOutput
        {
            FinOtchet = PrepareFinOtchetOutput(finOtchetRows),
            Balance = PrepareBalanceOutput(balanceRows, balanceRowsPrev),
            Other = PrepareOtherOutput(svedeniyaSettings, rasshifrovkaSettings, provodki, balances, departmentCode, isSvodniy)
        };

        return result;
    }

    private List<FinOtchetRow> FillFactData(
        List<Models.TbbuFinOtchetSetting> settings,
        List<TbbuProvodkiRow> provodki,
        List<TbbuBalanceRow> balances,
        List<TbbuPlanRow> planRows,
        int year, int kvartal, string departmentCode, bool isSvodniy,
        Dictionary<string, string> planCodeFinCode)
    {
        var rows = new List<FinOtchetRow>();
        var computed = new Dictionary<string, decimal>();

        foreach (var s in settings)
        {
            if (s.CODE == null) continue;

            decimal realValue = 0m;
            decimal planValue = 0m;

            var dataType = (FinOtchDataType)(s.DATA_TYPE ?? (int)FinOtchDataType.DEFAULT0);

            switch (dataType)
            {
                case FinOtchDataType.OBOROT:
                case FinOtchDataType.VIBORKA:
                    realValue = GetValueProvodki(s, provodki, departmentCode, isSvodniy);
                    planValue = GetDataFromPlan(s, planRows, kvartal, departmentCode, isSvodniy, planCodeFinCode);
                    break;
                case FinOtchDataType.DebitOSTATOK:
                case FinOtchDataType.CreditOSTATOK:
                    realValue = GetBalanceDataFromValue(s, balances, departmentCode, isSvodniy, dataType);
                    planValue = GetDataFromPlan(s, planRows, kvartal, departmentCode, isSvodniy, planCodeFinCode);
                    break;
                case FinOtchDataType.EXPRESSION:
                    if (s.DEBIT_SIDE != null)
                        realValue = CalcExpression(s.DEBIT_SIDE, computed);
                    if (s.CREDIT_SIDE != null)
                        planValue = CalcExpression(s.CREDIT_SIDE, computed);
                    break;
                case FinOtchDataType.DEFAULT0:
                    realValue = 0m;
                    planValue = 0m;
                    break;
            }

            computed[s.CODE] = realValue;
            rows.Add(new FinOtchetRow(s.CODE, realValue, planValue));
        }

        return rows;
    }

    private List<FinOtchetRow> FillFactDataBalance(
        List<Models.TbbuFinOtchetSetting> settings,
        List<TbbuBalanceRow> balances,
        string departmentCode, bool isSvodniy)
    {
        var rows = new List<FinOtchetRow>();
        var computed = new Dictionary<string, decimal>();

        foreach (var s in settings)
        {
            if (s.CODE == null) continue;

            decimal realValue = 0m;
            var dataType = (FinOtchDataType)(s.DATA_TYPE ?? (int)FinOtchDataType.DEFAULT0);

            switch (dataType)
            {
                case FinOtchDataType.DebitOSTATOK:
                case FinOtchDataType.CreditOSTATOK:
                    realValue = GetBalanceDataFromValue(s, balances, departmentCode, isSvodniy, dataType);
                    break;
                case FinOtchDataType.EXPRESSION:
                    if (s.DEBIT_SIDE != null)
                        realValue = CalcExpression(s.DEBIT_SIDE, computed);
                    break;
                case FinOtchDataType.DEFAULT0:
                    realValue = 0m;
                    break;
            }

            computed[s.CODE] = realValue;
            rows.Add(new FinOtchetRow(s.CODE, realValue, 0m));
        }

        return rows;
    }

    private static decimal GetValueProvodki(
        Models.TbbuFinOtchetSetting s,
        List<TbbuProvodkiRow> provodki,
        string departmentCode, bool isSvodniy)
    {
        var query = provodki.AsEnumerable();

        if (isSvodniy)
            query = query.Where(p => p.DEPARTMENT_BASE != null && p.DEPARTMENT_BASE.Contains(departmentCode));
        else
            query = query.Where(p => p.DEPARTMENT_BASE == departmentCode);

        decimal debitSum = 0m;
        decimal creditSum = 0m;

        if (!string.IsNullOrEmpty(s.DEBIT_SIDE))
        {
            var debitPrefix = s.DEBIT_SIDE;
            debitSum = query
                .Where(p => p.PLATELRSCH != null && p.PLATELRSCH.StartsWith(debitPrefix))
                .Sum(p => p.SUMMA ?? 0m);
        }

        if (!string.IsNullOrEmpty(s.CREDIT_SIDE))
        {
            var creditPrefix = s.CREDIT_SIDE;
            creditSum = query
                .Where(p => p.POLUCHRSCH != null && p.POLUCHRSCH.StartsWith(creditPrefix))
                .Sum(p => p.SUMMA ?? 0m);
        }

        return debitSum - creditSum;
    }

    private static decimal GetBalanceDataFromValue(
        Models.TbbuFinOtchetSetting s,
        List<TbbuBalanceRow> balances,
        string departmentCode, bool isSvodniy,
        FinOtchDataType dataType)
    {
        if (string.IsNullOrEmpty(s.ACCOUNT_CODE)) return 0m;

        var accountPrefix = s.ACCOUNT_CODE;
        var query = balances.AsEnumerable()
            .Where(b => b.ACCOUNT_CODE != null && b.ACCOUNT_CODE.StartsWith(accountPrefix));

        if (isSvodniy)
            query = query.Where(b => b.DEPARTMENT_BASE != null && b.DEPARTMENT_BASE.Contains(departmentCode));
        else
            query = query.Where(b => b.DEPARTMENT_BASE == departmentCode);

        return dataType == FinOtchDataType.DebitOSTATOK
            ? query.Sum(b => b.DEBIT_BAL ?? 0m)
            : query.Sum(b => b.CREDIT_BAL ?? 0m);
    }

    private static decimal GetDataFromPlan(
        Models.TbbuFinOtchetSetting s,
        List<TbbuPlanRow> planRows,
        int kvartal, string departmentCode, bool isSvodniy,
        Dictionary<string, string> planCodeFinCode)
    {
        if (s.CODE == null) return 0m;

        // Resolve plan item code via mapping
        if (!planCodeFinCode.TryGetValue(s.CODE, out var itemCode))
            itemCode = s.CODE;

        var query = planRows.AsEnumerable()
            .Where(p => p.ITEM_CODE == itemCode);

        if (isSvodniy)
            query = query.Where(p => p.SOATO_CODE != null && p.SOATO_CODE.Contains(departmentCode));
        else
            query = query.Where(p => p.SOATO_CODE == departmentCode);

        decimal value = kvartal switch
        {
            1 => query.Sum(p => p.QUARTER1 ?? 0m),
            2 => query.Sum(p => p.HALFYEAR ?? 0m),
            3 => query.Sum(p => p.MONTH9 ?? 0m),
            _ => query.Sum(p => p.GODOVOY ?? 0m)
        };

        return value;
    }

    private static decimal CalcExpression(string expression, Dictionary<string, decimal> computed)
    {
        // Parse simple arithmetic expressions like "010+020-030"
        var result = 0m;
        var tokens = System.Text.RegularExpressions.Regex.Split(expression.Trim(), @"(?=[+\-])");
        foreach (var token in tokens)
        {
            var t = token.Trim();
            if (string.IsNullOrEmpty(t)) continue;
            var sign = 1m;
            var code = t;
            if (t.StartsWith('+')) { sign = 1m; code = t[1..]; }
            else if (t.StartsWith('-')) { sign = -1m; code = t[1..]; }
            code = code.Trim();
            if (computed.TryGetValue(code, out var val))
                result += sign * val;
        }
        return result;
    }

    private static FinOtchetOutput PrepareFinOtchetOutput(List<FinOtchetRow> rows)
    {
        const int tochnost = 1;
        var o = new FinOtchetOutput();

        void Set(string code, Action<decimal?, decimal?> assign)
        {
            var r = rows.FirstOrDefault(x => x.Code == code);
            if (r != null)
                assign(Math.Round(r.PlanValue, tochnost), Math.Round(r.RealValue / 1000m, tochnost));
            else
                assign(null, null);
        }

        Set("010", (p, r) => { o.field010P = p; o.field010R = r; });
        Set("020", (p, r) => { o.field020P = p; o.field020R = r; });
        Set("030", (p, r) => { o.field030P = p; o.field030R = r; });
        Set("040", (p, r) => { o.field040P = p; o.field040R = r; });
        Set("050", (p, r) => { o.field050P = p; o.field050R = r; });
        Set("060", (p, r) => { o.field060P = p; o.field060R = r; });
        Set("070", (p, r) => { o.field070P = p; o.field070R = r; });
        Set("080", (p, r) => { o.field080P = p; o.field080R = r; });
        Set("090", (p, r) => { o.field090P = p; o.field090R = r; });
        Set("100", (p, r) => { o.field100P = p; o.field100R = r; });
        Set("110", (p, r) => { o.field110P = p; o.field110R = r; });
        Set("120", (p, r) => { o.field120P = p; o.field120R = r; });
        Set("130", (p, r) => { o.field130P = p; o.field130R = r; });
        Set("140", (p, r) => { o.field140P = p; o.field140R = r; });
        Set("150", (p, r) => { o.field150P = p; o.field150R = r; });
        Set("160", (p, r) => { o.field160P = p; o.field160R = r; });
        Set("170", (p, r) => { o.field170P = p; o.field170R = r; });
        Set("180", (p, r) => { o.field180P = p; o.field180R = r; });
        Set("190", (p, r) => { o.field190P = p; o.field190R = r; });
        Set("200", (p, r) => { o.field200P = p; o.field200R = r; });
        Set("210", (p, r) => { o.field210P = p; o.field210R = r; });
        Set("220", (p, r) => { o.field220P = p; o.field220R = r; });
        Set("230", (p, r) => { o.field230P = p; o.field230R = r; });
        Set("240", (p, r) => { o.field240P = p; o.field240R = r; });
        Set("250", (p, r) => { o.field250P = p; o.field250R = r; });
        Set("260", (p, r) => { o.field260P = p; o.field260R = r; });
        Set("270", (p, r) => { o.field270P = p; o.field270R = r; });
        Set("280", (p, r) => { o.field280P = p; o.field280R = r; });
        Set("290", (p, r) => { o.field290P = p; o.field290R = r; });
        Set("300", (p, r) => { o.field300P = p; o.field300R = r; });
        Set("310", (p, r) => { o.field310P = p; o.field310R = r; });
        Set("320", (p, r) => { o.field320P = p; o.field320R = r; });
        Set("330", (p, r) => { o.field330P = p; o.field330R = r; });
        Set("340", (p, r) => { o.field340P = p; o.field340R = r; });

        return o;
    }

    private static BalanceOutput PrepareBalanceOutput(List<FinOtchetRow> current, List<FinOtchetRow> prev)
    {
        const int tochnost = 1;
        var o = new BalanceOutput();

        void Set(string code, Action<decimal?> setCur, Action<decimal?> setPrev)
        {
            var r = current.FirstOrDefault(x => x.Code == code);
            setCur(r != null ? Math.Round(r.RealValue / 1000m, tochnost) : null);
            var rp = prev.FirstOrDefault(x => x.Code == code);
            setPrev(rp != null ? Math.Round(rp.RealValue / 1000m, tochnost) : null);
        }

        Set("010", v => o.field010 = v, v => o.field010c = v);
        Set("020", v => o.field020 = v, v => o.field020c = v);
        Set("030", v => o.field030 = v, v => o.field030c = v);
        Set("040", v => o.field040 = v, v => o.field040c = v);
        Set("050", v => o.field050 = v, v => o.field050c = v);
        Set("060", v => o.field060 = v, v => o.field060c = v);
        Set("070", v => o.field070 = v, v => o.field070c = v);
        Set("080", v => o.field080 = v, v => o.field080c = v);
        Set("090", v => o.field090 = v, v => o.field090c = v);
        Set("100", v => o.field100 = v, v => o.field100c = v);
        Set("110", v => o.field110 = v, v => o.field110c = v);
        Set("120", v => o.field120 = v, v => o.field120c = v);
        Set("130", v => o.field130 = v, v => o.field130c = v);
        Set("140", v => o.field140 = v, v => o.field140c = v);
        Set("150", v => o.field150 = v, v => o.field150c = v);
        Set("160", v => o.field160 = v, v => o.field160c = v);
        Set("170", v => o.field170 = v, v => o.field170c = v);
        Set("180", v => o.field180 = v, v => o.field180c = v);
        Set("190", v => o.field190 = v, v => o.field190c = v);
        Set("200", v => o.field200 = v, v => o.field200c = v);
        Set("210", v => o.field210 = v, v => o.field210c = v);
        Set("220", v => o.field220 = v, v => o.field220c = v);
        Set("230", v => o.field230 = v, v => o.field230c = v);
        Set("240", v => o.field240 = v, v => o.field240c = v);
        Set("250", v => o.field250 = v, v => o.field250c = v);
        Set("260", v => o.field260 = v, v => o.field260c = v);
        Set("270", v => o.field270 = v, v => o.field270c = v);
        Set("280", v => o.field280 = v, v => o.field280c = v);
        Set("290", v => o.field290 = v, v => o.field290c = v);
        Set("300", v => o.field300 = v, v => o.field300c = v);
        Set("310", v => o.field310 = v, v => o.field310c = v);
        Set("320", v => o.field320 = v, v => o.field320c = v);
        Set("330", v => o.field330 = v, v => o.field330c = v);
        Set("340", v => o.field340 = v, v => o.field340c = v);
        Set("350", v => o.field350 = v, v => o.field350c = v);
        Set("360", v => o.field360 = v, v => o.field360c = v);
        Set("370", v => o.field370 = v, v => o.field370c = v);

        return o;
    }

    private OtherOutput PrepareOtherOutput(
        List<Models.TbbuFinOtchetSetting> svedeniyaSettings,
        List<Models.TbbuFinOtchetSetting> rasshifrovkaSettings,
        List<TbbuProvodkiRow> provodki,
        List<TbbuBalanceRow> balances,
        string departmentCode, bool isSvodniy)
    {
        const int tochnost = 1;
        var o = new OtherOutput();

        // Rasxod matrix: settings with CODE like "X_Y" in Svedeniya
        // The CODE pattern is "row_col" where row and col are indices 3..12
        foreach (var s in svedeniyaSettings)
        {
            if (s.CODE == null) continue;
            var parts = s.CODE.Split('_');
            if (parts.Length != 2) continue;
            if (!int.TryParse(parts[0], out var row) || !int.TryParse(parts[1], out var col)) continue;

            decimal val = 0m;
            var dataType = (FinOtchDataType)(s.DATA_TYPE ?? (int)FinOtchDataType.DEFAULT0);
            if (dataType is FinOtchDataType.OBOROT or FinOtchDataType.VIBORKA)
                val = GetValueProvodki(s, provodki, departmentCode, isSvodniy);
            else if (dataType is FinOtchDataType.DebitOSTATOK or FinOtchDataType.CreditOSTATOK)
                val = GetBalanceDataFromValue(s, balances, departmentCode, isSvodniy, dataType);

            val = Math.Round(val / 1000m, tochnost);
            SetRasxodField(o, row, col, val);
        }

        // Rasshifrovka fields
        for (int i = 0; i < rasshifrovkaSettings.Count && i < 10; i++)
        {
            var s = rasshifrovkaSettings[i];
            decimal val = 0m;
            var dataType = (FinOtchDataType)(s.DATA_TYPE ?? (int)FinOtchDataType.DEFAULT0);
            if (dataType is FinOtchDataType.OBOROT or FinOtchDataType.VIBORKA)
                val = GetValueProvodki(s, provodki, departmentCode, isSvodniy);
            else if (dataType is FinOtchDataType.DebitOSTATOK or FinOtchDataType.CreditOSTATOK)
                val = GetBalanceDataFromValue(s, balances, departmentCode, isSvodniy, dataType);
            val = Math.Round(val / 1000m, tochnost);
            SetRasshifrovkaField(o, i + 1, val);
        }

        return o;
    }

    private static void SetRasxodField(OtherOutput o, int row, int col, decimal val)
    {
        switch ((row, col))
        {
            case (3, 3): o.rasxod_3_3 = val; break;
            case (3, 4): o.rasxod_3_4 = val; break;
            case (3, 5): o.rasxod_3_5 = val; break;
            case (3, 6): o.rasxod_3_6 = val; break;
            case (3, 7): o.rasxod_3_7 = val; break;
            case (3, 8): o.rasxod_3_8 = val; break;
            case (3, 9): o.rasxod_3_9 = val; break;
            case (3, 10): o.rasxod_3_10 = val; break;
            case (3, 11): o.rasxod_3_11 = val; break;
            case (3, 12): o.rasxod_3_12 = val; break;
            case (4, 3): o.rasxod_4_3 = val; break;
            case (4, 4): o.rasxod_4_4 = val; break;
            case (4, 5): o.rasxod_4_5 = val; break;
            case (4, 6): o.rasxod_4_6 = val; break;
            case (4, 7): o.rasxod_4_7 = val; break;
            case (4, 8): o.rasxod_4_8 = val; break;
            case (4, 9): o.rasxod_4_9 = val; break;
            case (4, 10): o.rasxod_4_10 = val; break;
            case (4, 11): o.rasxod_4_11 = val; break;
            case (4, 12): o.rasxod_4_12 = val; break;
            case (5, 3): o.rasxod_5_3 = val; break;
            case (5, 4): o.rasxod_5_4 = val; break;
            case (5, 5): o.rasxod_5_5 = val; break;
            case (5, 6): o.rasxod_5_6 = val; break;
            case (5, 7): o.rasxod_5_7 = val; break;
            case (5, 8): o.rasxod_5_8 = val; break;
            case (5, 9): o.rasxod_5_9 = val; break;
            case (5, 10): o.rasxod_5_10 = val; break;
            case (5, 11): o.rasxod_5_11 = val; break;
            case (5, 12): o.rasxod_5_12 = val; break;
            case (6, 3): o.rasxod_6_3 = val; break;
            case (6, 4): o.rasxod_6_4 = val; break;
            case (6, 5): o.rasxod_6_5 = val; break;
            case (6, 6): o.rasxod_6_6 = val; break;
            case (6, 7): o.rasxod_6_7 = val; break;
            case (6, 8): o.rasxod_6_8 = val; break;
            case (6, 9): o.rasxod_6_9 = val; break;
            case (6, 10): o.rasxod_6_10 = val; break;
            case (6, 11): o.rasxod_6_11 = val; break;
            case (6, 12): o.rasxod_6_12 = val; break;
            case (7, 3): o.rasxod_7_3 = val; break;
            case (7, 4): o.rasxod_7_4 = val; break;
            case (7, 5): o.rasxod_7_5 = val; break;
            case (7, 6): o.rasxod_7_6 = val; break;
            case (7, 7): o.rasxod_7_7 = val; break;
            case (7, 8): o.rasxod_7_8 = val; break;
            case (7, 9): o.rasxod_7_9 = val; break;
            case (7, 10): o.rasxod_7_10 = val; break;
            case (7, 11): o.rasxod_7_11 = val; break;
            case (7, 12): o.rasxod_7_12 = val; break;
            case (8, 3): o.rasxod_8_3 = val; break;
            case (8, 4): o.rasxod_8_4 = val; break;
            case (8, 5): o.rasxod_8_5 = val; break;
            case (8, 6): o.rasxod_8_6 = val; break;
            case (8, 7): o.rasxod_8_7 = val; break;
            case (8, 8): o.rasxod_8_8 = val; break;
            case (8, 9): o.rasxod_8_9 = val; break;
            case (8, 10): o.rasxod_8_10 = val; break;
            case (8, 11): o.rasxod_8_11 = val; break;
            case (8, 12): o.rasxod_8_12 = val; break;
            case (9, 3): o.rasxod_9_3 = val; break;
            case (9, 4): o.rasxod_9_4 = val; break;
            case (9, 5): o.rasxod_9_5 = val; break;
            case (9, 6): o.rasxod_9_6 = val; break;
            case (9, 7): o.rasxod_9_7 = val; break;
            case (9, 8): o.rasxod_9_8 = val; break;
            case (9, 9): o.rasxod_9_9 = val; break;
            case (9, 10): o.rasxod_9_10 = val; break;
            case (9, 11): o.rasxod_9_11 = val; break;
            case (9, 12): o.rasxod_9_12 = val; break;
            case (10, 3): o.rasxod_10_3 = val; break;
            case (10, 4): o.rasxod_10_4 = val; break;
            case (10, 5): o.rasxod_10_5 = val; break;
            case (10, 6): o.rasxod_10_6 = val; break;
            case (10, 7): o.rasxod_10_7 = val; break;
            case (10, 8): o.rasxod_10_8 = val; break;
            case (10, 9): o.rasxod_10_9 = val; break;
            case (10, 10): o.rasxod_10_10 = val; break;
            case (10, 11): o.rasxod_10_11 = val; break;
            case (10, 12): o.rasxod_10_12 = val; break;
            case (11, 3): o.rasxod_11_3 = val; break;
            case (11, 4): o.rasxod_11_4 = val; break;
            case (11, 5): o.rasxod_11_5 = val; break;
            case (11, 6): o.rasxod_11_6 = val; break;
            case (11, 7): o.rasxod_11_7 = val; break;
            case (11, 8): o.rasxod_11_8 = val; break;
            case (11, 9): o.rasxod_11_9 = val; break;
            case (11, 10): o.rasxod_11_10 = val; break;
            case (11, 11): o.rasxod_11_11 = val; break;
            case (11, 12): o.rasxod_11_12 = val; break;
            case (12, 3): o.rasxod_12_3 = val; break;
            case (12, 4): o.rasxod_12_4 = val; break;
            case (12, 5): o.rasxod_12_5 = val; break;
            case (12, 6): o.rasxod_12_6 = val; break;
            case (12, 7): o.rasxod_12_7 = val; break;
            case (12, 8): o.rasxod_12_8 = val; break;
            case (12, 9): o.rasxod_12_9 = val; break;
            case (12, 10): o.rasxod_12_10 = val; break;
            case (12, 11): o.rasxod_12_11 = val; break;
            case (12, 12): o.rasxod_12_12 = val; break;
        }
    }

    private static void SetRasshifrovkaField(OtherOutput o, int idx, decimal val)
    {
        switch (idx)
        {
            case 1: o.rasshifrovka_1 = val; break;
            case 2: o.rasshifrovka_2 = val; break;
            case 3: o.rasshifrovka_3 = val; break;
            case 4: o.rasshifrovka_4 = val; break;
            case 5: o.rasshifrovka_5 = val; break;
            case 6: o.rasshifrovka_6 = val; break;
            case 7: o.rasshifrovka_7 = val; break;
            case 8: o.rasshifrovka_8 = val; break;
            case 9: o.rasshifrovka_9 = val; break;
            case 10: o.rasshifrovka_10 = val; break;
        }
    }
}
