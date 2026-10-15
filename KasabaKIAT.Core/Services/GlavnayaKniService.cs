using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.DTOs.Reports;
using Microsoft.EntityFrameworkCore;

namespace KasabaKIAT.Core.Services;

public class GlavnayaKniService(KasabaKIATContext db) : IGlavnayaKniService
{
    public async Task<GlavnayaKnigaResult> GetGlavnayaKnigaAsync(int year, int kvartal, string departmentCode, bool isSvodniy, CancellationToken ct = default)
    {
        // Load row and column labels from tbcs_dictionary
        var rowLabels = await db.TbcsDictionaries
            .Where(d => d.DICTIONARY_TYPE_ID == 14)
            .OrderBy(d => d.REC_ID)
            .Select(d => d.NAME_RU ?? string.Empty)
            .ToListAsync(ct);

        var colLabels = await db.TbcsDictionaries
            .Where(d => d.DICTIONARY_TYPE_ID == 15)
            .OrderBy(d => d.REC_ID)
            .Select(d => d.NAME_RU ?? string.Empty)
            .ToListAsync(ct);

        List<GkDebitRow> debitRows;
        List<GkCreditRow> creditRows;

        if (isSvodniy)
        {
            debitRows = await db.Database.SqlQueryRaw<GkDebitRow>(
                "EXEC sp_rt_GlavnayaKniga_Debit_Svodniy @p0, @p1, @p2",
                year, kvartal, departmentCode)
                .ToListAsync(ct);

            creditRows = await db.Database.SqlQueryRaw<GkCreditRow>(
                "EXEC sp_rt_GlavnayaKniga_Credit_Svodniy @p0, @p1, @p2",
                year, kvartal, departmentCode)
                .ToListAsync(ct);
        }
        else
        {
            debitRows = await db.Database.SqlQueryRaw<GkDebitRow>(
                "EXEC sp_rt_GlavnayaKniga_Debit @p0, @p1, @p2",
                year, kvartal, departmentCode)
                .ToListAsync(ct);

            creditRows = await db.Database.SqlQueryRaw<GkCreditRow>(
                "EXEC sp_rt_GlavnayaKniga_Credit @p0, @p1, @p2",
                year, kvartal, departmentCode)
                .ToListAsync(ct);
        }

        var tableRows = BuildGlavKnigaTable(debitRows, creditRows);

        return new GlavnayaKnigaResult
        {
            Rows = rowLabels,
            Cols = colLabels,
            Data = tableRows
        };
    }

    private static List<GlavnayaKnigaTable> BuildGlavKnigaTable(List<GkDebitRow> debitRows, List<GkCreditRow> creditRows)
    {
        // Collect all unique accounts
        var accounts = debitRows.Select(d => d.DebitAccount)
            .Concat(creditRows.Select(c => c.CreditAccount))
            .Where(a => a != null)
            .Select(a => a!)
            .Distinct()
            .OrderBy(a => a)
            .ToList();

        var joNames = new[] { "JO1", "JO2", "JO3", "JO4", "JO5", "JO6", "JO7", "JO8", "JO9", "JO10", "JO11" };

        var result = new List<GlavnayaKnigaTable>();

        decimal totalD_saldo = 0m, totalC_saldo = 0m;
        decimal[] totalD_jo = new decimal[11];
        decimal[] totalC_jo = new decimal[11];

        foreach (var account in accounts)
        {
            var row = new GlavnayaKnigaTable();
            row.field0010 = account;
            row.field0040 = account;
            row.field0024 = account;

            // Debit saldo
            decimal d_saldo = debitRows
                .Where(d => d.DebitAccount == account && d.JO == "saldo")
                .Sum(d => d.SUMMA ?? 0m);

            // Debit JO oboroti
            decimal[] d_jo = new decimal[11];
            for (int i = 0; i < 11; i++)
            {
                string joName = joNames[i];
                d_jo[i] = debitRows
                    .Where(d => d.DebitAccount == account && d.JO == joName)
                    .Sum(d => d.SUMMA ?? 0m);
            }

            decimal d_joSum = d_jo.Sum();
            decimal d_total = d_saldo + d_joSum;

            row.field0011 = d_saldo;
            row.field0012 = d_jo[0];
            row.field0013 = d_jo[1];
            row.field0014 = d_jo[2];
            row.field0015 = d_jo[3];
            row.field0016 = d_jo[4];
            row.field0017 = d_jo[5];
            row.field0018 = d_jo[6];
            row.field0019 = d_jo[7];
            row.field0020 = d_jo[8];
            row.field0021 = d_jo[9];
            // Note: field0021 is JO10, but old code has 11 JO fields mapped to field0012..field0021 (10 fields) + extra
            // Following old code: field0012-0021 = JO1-JO10, field0021 used for JO10, JO11 unused or field0021 overwritten
            // Old code: field0012=D.oboroti[JO1], ..., field0021=D.oboroti[JO11-1=JO10], field0022=sum
            // Let's map correctly: field0012..field0022 = 11 fields for JO1..JO11
            row.field0021 = d_jo[9];   // JO10
            // We need field0022 for JO11 sum — but field0022 in old code is "sum of oboroti"
            // Old GK_Structure.cs: field0012..field0022 indices per the balance layout in summary:
            // field0012=D.oboroti[JO1], field0013=JO2, ..., field0021=JO10, field0022=SUM (not JO11)
            // So JO11 is included in the sum but not individually displayed. Let's use that mapping:
            row.field0012 = d_jo[0];  // JO1
            row.field0013 = d_jo[1];  // JO2
            row.field0014 = d_jo[2];  // JO3
            row.field0015 = d_jo[3];  // JO4
            row.field0016 = d_jo[4];  // JO5
            row.field0017 = d_jo[5];  // JO6
            row.field0018 = d_jo[6];  // JO7
            row.field0019 = d_jo[7];  // JO8
            row.field0020 = d_jo[8];  // JO9
            row.field0021 = d_jo[9];  // JO10
            // JO11 (d_jo[10]) is included in d_joSum
            row.field0022 = d_joSum;
            row.field0023 = d_total;

            // Credit saldo
            decimal c_saldo = creditRows
                .Where(c => c.CreditAccount == account && c.JO == "saldo")
                .Sum(c => c.SUMMA ?? 0m);

            decimal[] c_jo = new decimal[11];
            for (int i = 0; i < 11; i++)
            {
                string joName = joNames[i];
                c_jo[i] = creditRows
                    .Where(c => c.CreditAccount == account && c.JO == joName)
                    .Sum(c => c.SUMMA ?? 0m);
            }

            decimal c_joSum = c_jo.Sum();
            decimal c_total = c_saldo + c_joSum;

            row.field0025 = c_saldo;
            row.field0026 = c_jo[0];
            row.field0027 = c_jo[1];
            row.field0028 = c_jo[2];
            row.field0029 = c_jo[3];
            row.field0030 = c_jo[4];
            row.field0031 = c_jo[5];
            row.field0032 = c_jo[6];
            row.field0033 = c_jo[7];
            row.field0034 = c_jo[8];
            row.field0035 = c_jo[9];
            row.field0036 = c_joSum;
            row.field0037 = c_total;

            // Aktiv / Passiv
            decimal diff = d_total - c_total;
            row.field0038 = diff > 0 ? diff : 0m;
            row.field0039 = diff < 0 ? -diff : 0m;

            // Skip rows where all decimal values are zero
            if (IsAllZero(row)) continue;

            result.Add(row);

            totalD_saldo += d_saldo;
            totalC_saldo += c_saldo;
            for (int i = 0; i < 11; i++) { totalD_jo[i] += d_jo[i]; totalC_jo[i] += c_jo[i]; }
        }

        // Append Итого row
        var itogo = new GlavnayaKnigaTable();
        itogo.field0010 = "Итого:";
        itogo.field0040 = "Итого:";
        itogo.field0024 = "Итого:";
        itogo.field0011 = totalD_saldo;
        itogo.field0012 = totalD_jo[0];
        itogo.field0013 = totalD_jo[1];
        itogo.field0014 = totalD_jo[2];
        itogo.field0015 = totalD_jo[3];
        itogo.field0016 = totalD_jo[4];
        itogo.field0017 = totalD_jo[5];
        itogo.field0018 = totalD_jo[6];
        itogo.field0019 = totalD_jo[7];
        itogo.field0020 = totalD_jo[8];
        itogo.field0021 = totalD_jo[9];
        decimal totalDJoSum = totalD_jo.Sum();
        itogo.field0022 = totalDJoSum;
        itogo.field0023 = totalD_saldo + totalDJoSum;
        itogo.field0025 = totalC_saldo;
        itogo.field0026 = totalC_jo[0];
        itogo.field0027 = totalC_jo[1];
        itogo.field0028 = totalC_jo[2];
        itogo.field0029 = totalC_jo[3];
        itogo.field0030 = totalC_jo[4];
        itogo.field0031 = totalC_jo[5];
        itogo.field0032 = totalC_jo[6];
        itogo.field0033 = totalC_jo[7];
        itogo.field0034 = totalC_jo[8];
        itogo.field0035 = totalC_jo[9];
        decimal totalCJoSum = totalC_jo.Sum();
        itogo.field0036 = totalCJoSum;
        itogo.field0037 = totalC_saldo + totalCJoSum;
        decimal totalDiff = itogo.field0023.GetValueOrDefault() - itogo.field0037.GetValueOrDefault();
        itogo.field0038 = totalDiff > 0 ? totalDiff : 0m;
        itogo.field0039 = totalDiff < 0 ? -totalDiff : 0m;

        result.Add(itogo);
        return result;
    }

    private static bool IsAllZero(GlavnayaKnigaTable row)
    {
        return (row.field0011 ?? 0m) == 0m &&
               (row.field0012 ?? 0m) == 0m &&
               (row.field0013 ?? 0m) == 0m &&
               (row.field0014 ?? 0m) == 0m &&
               (row.field0015 ?? 0m) == 0m &&
               (row.field0016 ?? 0m) == 0m &&
               (row.field0017 ?? 0m) == 0m &&
               (row.field0018 ?? 0m) == 0m &&
               (row.field0019 ?? 0m) == 0m &&
               (row.field0020 ?? 0m) == 0m &&
               (row.field0021 ?? 0m) == 0m &&
               (row.field0022 ?? 0m) == 0m &&
               (row.field0023 ?? 0m) == 0m &&
               (row.field0025 ?? 0m) == 0m &&
               (row.field0026 ?? 0m) == 0m &&
               (row.field0027 ?? 0m) == 0m &&
               (row.field0028 ?? 0m) == 0m &&
               (row.field0029 ?? 0m) == 0m &&
               (row.field0030 ?? 0m) == 0m &&
               (row.field0031 ?? 0m) == 0m &&
               (row.field0032 ?? 0m) == 0m &&
               (row.field0033 ?? 0m) == 0m &&
               (row.field0034 ?? 0m) == 0m &&
               (row.field0035 ?? 0m) == 0m &&
               (row.field0036 ?? 0m) == 0m &&
               (row.field0037 ?? 0m) == 0m &&
               (row.field0038 ?? 0m) == 0m &&
               (row.field0039 ?? 0m) == 0m;
    }
}
