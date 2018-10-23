using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/*
 Predpologaetsya:
 Vse, obsolyutno vse xozyaystvenniye operasii, otrajayutsya v provodkax v buxgalterii
 Yacheyki v finansovom otchete dojni kalkulirovatstya na osnovanii
 1) Oborotov po provodkam
 2) Ostatkov po schetam

    Inache ne mojet Bit, i ne doljno bit. Esli takoe 
    imeet mesto bit to eto ne sovershenstvo buxgaltenii Profsoyuza
 */
namespace KasabaKIAT.BusinessLogic.Buxgalteriya.FinansoviyOtchet
{
    /// <summary>
    /// 
    /// </summary>
    public enum OtchetPart
    {
        IspolneniyaByudjeta = 1,
        Balance,
        Svedeniya,
        Rasshifrovka
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FinOtchDataType
    {
        DebitOSTATOK,
        CreditOSTATOK,
        OBOROT,
        EXPRESSION,
        VIBORKA,
        DEFAULT0
    }

    /// <summary>
    /// 
    /// </summary>
    public class FinOtchetContainer
    {
        public string Code       { get; set; }

        public decimal PlanValue { get; set; }

        public decimal RealValue { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BalanceContainer
    {
        public string Code { get; set; }

        public decimal RealValue { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RasxodiContainer
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class TotalOutput
    {
        public string OrgName { get; set; }
        public string SelectedKvartalYear { get; set; }
        public FinOtchetOutput _finotchetoutput { get; set; }
        public BalanceOutput _balanceoutput { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FinOtchetOutput
    {
        public decimal? field010P { get; set; }
        public decimal? field020P { get; set; }
        public decimal? field021P { get; set; }
        public decimal? field022P { get; set; }
        public decimal? field023P { get; set; }
        public decimal? field024P { get; set; }
        public decimal? field030P { get; set; }
        public decimal? field040P { get; set; }
        public decimal? field050P { get; set; }
        public decimal? field060P { get; set; }
        public decimal? field070P { get; set; }
        public decimal? field071P { get; set; }
        public decimal? field072P { get; set; }
        public decimal? field080P { get; set; }
        public decimal? field090P { get; set; }
        public decimal? field100P { get; set; }
        public decimal? field110P { get; set; }
        public decimal? field120P { get; set; }
        public decimal? field130P { get; set; }
        public decimal? field140P { get; set; }
        public decimal? field150P { get; set; }
        public decimal? field160P { get; set; }
        public decimal? field170P { get; set; }
        public decimal? field180P { get; set; }
        public decimal? field190P { get; set; }
        public decimal? field200P { get; set; }
        public decimal? field210P { get; set; }
        public decimal? field220P { get; set; }
        public decimal? field230P { get; set; }
        public decimal? field240P { get; set; }
        public decimal? field250P { get; set; }
        public decimal? field260P { get; set; }
        public decimal? field270P { get; set; }
        public decimal? field280P { get; set; }
        public decimal? field290P { get; set; }
        public decimal? field300P { get; set; }
        public decimal? field310P { get; set; }
        public decimal? field320P { get; set; }
        public decimal? field330P { get; set; }
        public decimal? field340P { get; set; }

        public decimal? field010R { get; set; }
        public decimal? field020R { get; set; }
        public decimal? field021R { get; set; }
        public decimal? field022R { get; set; }
        public decimal? field023R { get; set; }
        public decimal? field024R { get; set; }
        public decimal? field030R { get; set; }
        public decimal? field040R { get; set; }
        public decimal? field050R { get; set; }
        public decimal? field060R { get; set; }
        public decimal? field070R { get; set; }
        public decimal? field071R { get; set; }
        public decimal? field072R { get; set; }
        public decimal? field080R { get; set; }
        public decimal? field090R { get; set; }
        public decimal? field100R { get; set; }
        public decimal? field110R { get; set; }
        public decimal? field120R { get; set; }
        public decimal? field130R { get; set; }
        public decimal? field140R { get; set; }
        public decimal? field150R { get; set; }
        public decimal? field160R { get; set; }
        public decimal? field170R { get; set; }
        public decimal? field180R { get; set; }
        public decimal? field190R { get; set; }
        public decimal? field200R { get; set; }
        public decimal? field210R { get; set; }
        public decimal? field220R { get; set; }
        public decimal? field230R { get; set; }
        public decimal? field240R { get; set; }
        public decimal? field250R { get; set; }
        public decimal? field260R { get; set; }
        public decimal? field270R { get; set; }
        public decimal? field280R { get; set; }
        public decimal? field290R { get; set; }
        public decimal? field300R { get; set; }
        public decimal? field310R { get; set; }
        public decimal? field320R { get; set; }
        public decimal? field330R { get; set; }
        public decimal? field340R { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BalanceOutput
    {
        public decimal? field010 { get; set; }
        public decimal? field020 { get; set; }
        public decimal? field030 { get; set; }
        public decimal? field040 { get; set; }
        public decimal? field050 { get; set; }
        public decimal? field060 { get; set; }
        public decimal? field070 { get; set; }
        public decimal? field080 { get; set; }
        public decimal? field090 { get; set; }
        public decimal? field100 { get; set; }
        public decimal? field110 { get; set; }
        public decimal? field120 { get; set; }
        public decimal? field130 { get; set; }
        public decimal? field140 { get; set; }
        public decimal? field150 { get; set; }
        public decimal? field160 { get; set; }
        public decimal? field170 { get; set; }
        public decimal? field180 { get; set; }
        public decimal? field190 { get; set; }
        public decimal? field200 { get; set; }
        public decimal? field210 { get; set; }
        public decimal? field220 { get; set; }
        public decimal? field230 { get; set; }
        public decimal? field240 { get; set; }
        public decimal? field250 { get; set; }
        public decimal? field260 { get; set; }
        public decimal? field270 { get; set; }
        public decimal? field280 { get; set; }
        public decimal? field290 { get; set; }
        public decimal? field300 { get; set; }
        public decimal? field310 { get; set; }
        public decimal? field320 { get; set; }
        public decimal? field330 { get; set; }
        public decimal? field340 { get; set; }
        public decimal? field350 { get; set; }
        public decimal? field360 { get; set; }
        public decimal? field370 { get; set; }

        //Compare
        public decimal? field250x           { get; set; }
        public decimal? field320x           { get; set; }
        public decimal? field060x           { get; set; }
        public decimal? field100x           { get; set; }
        public decimal? fieldActivePassive  { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class FinOtchetStrukture
    {
        //input parametrs if 9999 - svodniy po respubliki
        private string   departmentCode  { get; set; }

        private int      year            { get; set; }

        private int      kvartal         { get; set; }

        private long     usrId { get; set; }

        private Dictionary<string, string> planCodeFinCode { get; set; }

        //Out Parametrs
        private List<FinOtchetContainer> out_FinOtchet   { get;  set; }
        private List<BalanceContainer> out_Balance       { get;  set; }

        //Out Structures
        private FinOtchetOutput _outFinance { get;  set; }
        private BalanceOutput   _outBalance { get;  set; }
        public TotalOutput _totaloutput { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public FinOtchetStrukture()
        {
            planCodeFinCode = new Dictionary<string, string>();
            out_FinOtchet = new List<FinOtchetContainer>();
            out_Balance = new List<BalanceContainer>();
            MapPlanCodeFinCode();
            _totaloutput = new TotalOutput();
        }

        //planovaya
        //Should be mapped Planbyudgeta i Code 
        private void MapPlanCodeFinCode()
        {
            planCodeFinCode.Add("01"    , "020");
            planCodeFinCode.Add("02"    , "022");
            planCodeFinCode.Add("03"    , "021");
            planCodeFinCode.Add("04"    , "023");
            planCodeFinCode.Add("05"    , "");
            planCodeFinCode.Add("06"    , "030");
            planCodeFinCode.Add("07"    , "040");
            planCodeFinCode.Add("08"    , "060");
            planCodeFinCode.Add("09"    , "090");
            planCodeFinCode.Add("10"    , "120");
            planCodeFinCode.Add("11"    , "");
            planCodeFinCode.Add("12"    , "140");
            planCodeFinCode.Add("13"    , "");
            planCodeFinCode.Add("14"    , "");
            planCodeFinCode.Add("15"    , "");
            planCodeFinCode.Add("16"    , "170");
            planCodeFinCode.Add("161"   , "180");
            planCodeFinCode.Add("162"   , "");
            planCodeFinCode.Add("17"    , "190");
            planCodeFinCode.Add("18"    , "200");
            planCodeFinCode.Add("19"    , "230");
            planCodeFinCode.Add("20"    , "240");
            planCodeFinCode.Add("21"    , "250");
            planCodeFinCode.Add("22"    , "");
            planCodeFinCode.Add("23"    , "");
            planCodeFinCode.Add("24"    , "");
            planCodeFinCode.Add("25"    , "");
            planCodeFinCode.Add("26"    , "");
            planCodeFinCode.Add("27"    , "");
            planCodeFinCode.Add("28"    , "");
            planCodeFinCode.Add("29"    , "");
            planCodeFinCode.Add("30"    , "");
            planCodeFinCode.Add("31"    , "");
            planCodeFinCode.Add("32"    , "");
            planCodeFinCode.Add("33"    , "");
            planCodeFinCode.Add("34"    , "");
            planCodeFinCode.Add("35"    , "");
            planCodeFinCode.Add("36"    , "");
            planCodeFinCode.Add("37"    , "");
        }

        //fakticheskiye danniye

        /// <summary>
        /// 
        /// </summary>
        private void GetDataFromPlan()
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                if ((string.IsNullOrEmpty(departmentCode)) || (year ==0))
                {
                    throw new ApplicationException("departmentCode is invalid");
                }

                string soato_code = string.Empty;
                if (departmentCode.Equals("17"))
                {
                    soato_code = departmentCode;
                }
                else if (departmentCode.Length >= 5)
                {
                    soato_code = departmentCode.Remove(3, 1);
                }

                var plans = db.tbbu_planbyudjetregion.Where(z => z.SOATO_CODE == soato_code && z.YEAR == year);

                if (plans.Any())
                {
                    foreach(var plan  in plans)
                    {
                        var obj = new FinOtchetContainer();
                        string out_finCode = string.Empty;

                        if (planCodeFinCode.TryGetValue(plan.ITEM_CODE , out out_finCode))
                        {
                            if (string.IsNullOrEmpty(out_finCode))
                            {
                                continue;
                            }

                            obj.Code = out_finCode;

                            if (kvartal == 1)
                            {
                                obj.PlanValue = plan.QUARTER1.Value;
                            }
                            else if (kvartal == 2)
                            {
                                obj.PlanValue = plan.HALFYEAR.Value;
                            }
                            else if (kvartal == 3)
                            {
                                obj.PlanValue = plan.MONTH9.Value;
                            }
                            else if (kvartal == 4)
                            {
                                obj.PlanValue = plan.YEAR.Value;
                            }

                            out_FinOtchet.Add(obj);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void GetDataFromPlanSvodniy()
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                if ((string.IsNullOrEmpty(departmentCode)) || (year == 0))
                {
                    throw new ApplicationException("departmentCode is invalid");
                }

                string soato_code = string.Empty;
                if (departmentCode.Equals("17"))
                {
                    soato_code = departmentCode;
                }
                else if (departmentCode.Length >= 5)
                {
                    soato_code = departmentCode.Remove(3, 1);
                }

                var plans = db.tbbu_planbyudjetregion.Where(z => z.SOATO_CODE.Contains("17") && z.YEAR == year).GroupBy(z=>z.ITEM_CODE ,
                    (key, g) => new { ITEM_CODE = key,
                                                        QUARTER1 = g.Sum(k=>k.QUARTER1) ,
                                                        HALFYEAR = g.Sum(k => k.HALFYEAR) ,
                                                        MONTH9 = g.Sum(k => k.MONTH9) ,
                                                        YEAR = g.Sum(k => k.YEAR) });

                if (plans.Any())
                {
                    foreach (var plan in plans)
                    {
                        var obj = new FinOtchetContainer();
                        string out_finCode = string.Empty;
                        
                        if (planCodeFinCode.TryGetValue(plan.ITEM_CODE, out out_finCode))
                        {
                            if (string.IsNullOrEmpty(out_finCode))
                            {
                                continue;
                            }

                            obj.Code = out_finCode;

                            if (kvartal == 1)
                            {
                                obj.PlanValue = plan.QUARTER1.Value;
                            }
                            else if (kvartal == 2)
                            {
                                obj.PlanValue = plan.HALFYEAR.Value;
                            }
                            else if (kvartal == 3)
                            {
                                obj.PlanValue = plan.MONTH9.Value;
                            }
                            else if (kvartal == 4)
                            {
                                obj.PlanValue = plan.YEAR.Value;
                            }

                            out_FinOtchet.Add(obj);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <param name="debitAccount"></param>
        /// <param name="creditAccount"></param>
        /// <returns></returns>
        private decimal? GetValueProvodki(int year, int kvartal, string debitAccount, string creditAccount)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var provodkas = from prov in db.tbbu_provodki
                                where ((kvartal == 1 && (prov.TRDT.Value.Month <= 3)) || //prov.TRDT.Value.Month >= 1 && 
                                (kvartal == 2 && (prov.TRDT.Value.Month <= 6)) || //prov.TRDT.Value.Month >= 4 && 
                                (kvartal == 3 && (prov.TRDT.Value.Month <= 9)) || //prov.TRDT.Value.Month >= 7 && 
                                (kvartal == 4 && (prov.TRDT.Value.Month <= 12))) //prov.TRDT.Value.Month >= 10 && 
                                && prov.DEPARTMENT_BASE == departmentCode && prov.TRDT.Value.Year == year
                                && prov.PLATELRSCH == debitAccount && prov.POLUCHRSCH == creditAccount
                                select prov;

                if (provodkas.Any())
                {
                    var resSumma = provodkas.Sum(z => z.SUMMA);

                    return resSumma;
                }

            }

            return 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <param name="debitAccount"></param>
        /// <param name="creditAccount"></param>
        /// <returns></returns>
        private decimal? GetValueProvodkiSvodniy(int year, int kvartal, string debitAccount, string creditAccount)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var provodkas = from prov in db.tbbu_provodki
                                where ((kvartal == 1 && (prov.TRDT.Value.Month <= 3)) || //prov.TRDT.Value.Month >= 1 && 
                                (kvartal == 2 && (prov.TRDT.Value.Month <= 6)) || //prov.TRDT.Value.Month >= 4 && 
                                (kvartal == 3 && (prov.TRDT.Value.Month <= 9)) || //prov.TRDT.Value.Month >= 7 && 
                                (kvartal == 4 && (prov.TRDT.Value.Month <= 12))) //prov.TRDT.Value.Month >= 10 && 
                                && prov.DEPARTMENT_BASE.Contains(departmentCode) && prov.TRDT.Value.Year == year
                                select prov;

                if (provodkas.Any())
                {
                    var resSumma = provodkas.Sum(z => z.SUMMA);

                    return resSumma;
                }

            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <param name="accountCode"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        private decimal? GetBalanceDataFromValue(int year, int kvartal, string accountCode, FinOtchDataType tp)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var balances = db.tbbu_balance.Where(z => z.CUR_YEAR == year && 
                                                     z.CUR_KV == kvartal && 
                                                     z.ACCOUNT_CODE == accountCode && 
                                                     z.DEPARTMENT_BASE == departmentCode);

                if (balances.Any())
                {
                    if (tp == FinOtchDataType.DebitOSTATOK)
                        return balances.First().DEBIT_BAL;
                    else if (tp == FinOtchDataType.CreditOSTATOK)
                        return balances.First().CREDIT_BAL;
                }
            }

            return 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <param name="accountCode"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        private decimal? GetBalanceDataFromValueSvodniy(int year, int kvartal, string accountCode, FinOtchDataType tp)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var balances = db.tbbu_balance.Where(z => z.CUR_YEAR == year &&
                                                     z.CUR_KV == kvartal &&
                                                     z.ACCOUNT_CODE == accountCode &&
                                                     z.DEPARTMENT_BASE.Contains(departmentCode));

                if (balances.Any())
                {
                    if (tp == FinOtchDataType.DebitOSTATOK)
                        return balances.Sum(z=>z.DEBIT_BAL);
                    else if (tp == FinOtchDataType.CreditOSTATOK)
                        return balances.Sum(z => z.CREDIT_BAL);  
                }
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private decimal? CalcExpression(int part, string code)
        {
            if (part == (int)OtchetPart.IspolneniyaByudjeta)
            {
                switch (code)
                {
                    case "060":
                        var res060 = out_FinOtchet.Where(z => z.Code == "020" || z.Code == "030" || z.Code == "040" || z.Code == "050");

                        if (res060.Any())
                        {
                            var summa060 = res060.Sum(z => z.RealValue);

                            var objs1 = out_FinOtchet.Where(z => z.Code == "060");

                            if (objs1.Any())
                            {
                                var obj1 = objs1.First();
                                obj1.RealValue = summa060;

                            }
                            else
                            {
                                var obj1 = new FinOtchetContainer();
                                obj1.Code = "060";
                                obj1.RealValue = summa060;
                                out_FinOtchet.Add(obj1);
                            }

                            //var obj060 = new FinOtchetContainer
                            //{
                            //    Code = "060",
                            //    RealValue = summa060
                            //};
                            //out_FinOtchet.Add(obj060);
                        }

                        break;

                    case "100":

                        var res100 = out_FinOtchet.Where(z => z.Code == "070" || z.Code == "080" || z.Code == "090");

                        if (res100.Any())
                        {
                            var summa100 = res100.Sum(z => z.RealValue);

                            var objs1 = out_FinOtchet.Where(z => z.Code == "100");

                            if (objs1.Any())
                            {
                                var obj1 = objs1.First();
                                obj1.RealValue = summa100;

                            }
                            else
                            {
                                var obj1 = new FinOtchetContainer();
                                obj1.Code = "100";
                                obj1.RealValue = summa100;
                                out_FinOtchet.Add(obj1);
                            }


                            //var obj100 = new FinOtchetContainer
                            //{
                            //    Code = "100",
                            //    RealValue = summa100
                            //};
                            //out_FinOtchet.Add(obj100);
                        }

                        break;

                    case "110":

                        var res110 = out_FinOtchet.Where(z => z.Code == "010" || z.Code == "060" || z.Code == "100");

                        if (res110.Any())
                        {
                            var summa110 = res110.Sum(z => z.RealValue);

                            var objs1 = out_FinOtchet.Where(z => z.Code == "110");

                            if (objs1.Any())
                            {
                                var obj1 = objs1.First();
                                obj1.RealValue = summa110;

                            }
                            else
                            {
                                var obj1 = new FinOtchetContainer();
                                obj1.Code = "110";
                                obj1.RealValue = summa110;
                                out_FinOtchet.Add(obj1);
                            }


                            //var obj110 = new FinOtchetContainer
                            //{
                            //    Code = "110",
                            //    RealValue = summa110
                            //};
                            //out_FinOtchet.Add(obj110);
                        }

                        break;

                    case "160":

                        var res160 = out_FinOtchet.Where(z => z.Code == "120" || z.Code == "130" || z.Code == "140" || z.Code == "150");

                        if (res160.Any())
                        {
                            var summa160 = res160.Sum(z => z.RealValue);

                            var objs1 = out_FinOtchet.Where(z => z.Code == "160");

                            if (objs1.Any())
                            {
                                var obj1 = objs1.First();
                                obj1.RealValue = summa160;

                            }
                            else
                            {
                                var obj1 = new FinOtchetContainer();
                                obj1.Code = "160";
                                obj1.RealValue = summa160;
                                out_FinOtchet.Add(obj1);
                            }

                            //var obj160 = new FinOtchetContainer
                            //{
                            //    Code = "160",
                            //    RealValue = summa160
                            //};
                            //out_FinOtchet.Add(obj160);
                        }

                        break;

                    case "220":

                        var res220 = out_FinOtchet.Where(z => z.Code == "170" || z.Code == "180" || z.Code == "180" || z.Code == "200" || z.Code == "210");

                        if (res220.Any())
                        {
                            var summa220 = res220.Sum(z => z.RealValue);

                            var objs1 = out_FinOtchet.Where(z => z.Code == "220");

                            if (objs1.Any())
                            {
                                var obj1 = objs1.First();
                                obj1.RealValue = summa220;

                            }
                            else
                            {
                                var obj1 = new FinOtchetContainer();
                                obj1.Code = "220";
                                obj1.RealValue = summa220;
                                out_FinOtchet.Add(obj1);
                            }


                            //var obj220 = new FinOtchetContainer
                            //{
                            //    Code = "220",
                            //    RealValue = summa220
                            //};
                            //out_FinOtchet.Add(obj220);
                        }

                        break;
                    case "250":

                        var res250 = out_FinOtchet.Where(z => z.Code == "160" || z.Code == "220" || z.Code == "230" || z.Code == "240");

                        if (res250.Any())
                        {
                            var summa250 = res250.Sum(z => z.RealValue);

                            var objs1 = out_FinOtchet.Where(z => z.Code == "250");

                            if (objs1.Any())
                            {
                                var obj1 = objs1.First();
                                obj1.RealValue = summa250;

                            }
                            else
                            {
                                var obj1 = new FinOtchetContainer();
                                obj1.Code = "250";
                                obj1.RealValue = summa250;
                                out_FinOtchet.Add(obj1);
                            }


                            //var obj250 = new FinOtchetContainer
                            //{
                            //    Code = "250",
                            //    RealValue = summa250
                            //};
                            //out_FinOtchet.Add(obj250);
                        }

                        break;

                    case "320":

                        var res320 = out_FinOtchet.Where(z => z.Code == "260" || z.Code == "270" || z.Code == "280" || z.Code == "290"
                        || z.Code == "300" || z.Code == "310");

                        if (res320.Any())
                        {
                            var summa320 = res320.Sum(z => z.RealValue);

                            var objs1 = out_FinOtchet.Where(z => z.Code == "320");

                            if (objs1.Any())
                            {
                                var obj1 = objs1.First();
                                obj1.RealValue = summa320;

                            }
                            else
                            {
                                var obj1 = new FinOtchetContainer();
                                obj1.Code = "320";
                                obj1.RealValue = summa320;
                                out_FinOtchet.Add(obj1);
                            }


                            //var obj320 = new FinOtchetContainer
                            //{
                            //    Code = "320",
                            //    RealValue = summa320
                            //};
                            //out_FinOtchet.Add(obj320);
                        }

                        break;

                    case "330":

                        var res330 = out_FinOtchet.Where(z => z.Code == "250" || z.Code == "320");

                        if (res330.Any())
                        {
                            var summa330 = res330.Sum(z => z.RealValue);


                            var objs1 = out_FinOtchet.Where(z => z.Code == "330");

                            if (objs1.Any())
                            {
                                var obj1 = objs1.First();
                                obj1.RealValue = summa330;

                            }
                            else
                            {
                                var obj1 = new FinOtchetContainer();
                                obj1.Code = "330";
                                obj1.RealValue = summa330;
                                out_FinOtchet.Add(obj1);
                            }

                            //var obj330 = new FinOtchetContainer
                            //{
                            //    Code = "330",
                            //    RealValue = summa330
                            //};
                            //out_FinOtchet.Add(obj330);
                        }

                        break;

                    case "340":

                        decimal daromadlar = 0;
                        decimal xarajatlar = 0;

                        var res110x = out_FinOtchet.Where(z => z.Code == "110" );

                        if (res110x.Any())
                        {
                            daromadlar = res110x.First().RealValue;
                        }

                        var res330x = out_FinOtchet.Where(z => z.Code == "330");

                        if (res330x.Any())
                        {
                            xarajatlar = res330x.First().RealValue;
                        }

                        var obj = new FinOtchetContainer
                        {
                            Code = "340",
                            RealValue = daromadlar - xarajatlar
                        };
                        out_FinOtchet.Add(obj);

                        break;
                }
            }
            else if (part == (int)OtchetPart.Balance)
            {
                switch (code)
                {
                    case "220":
                        var res220 = out_Balance.Where(z => 
                           z.Code == "010" 
                        || z.Code == "020" 
                        || z.Code == "030" 
                        || z.Code == "040"
                        || z.Code == "050"
                        || z.Code == "060"
                        || z.Code == "070"
                        || z.Code == "080"
                        || z.Code == "090"
                        || z.Code == "100"
                        || z.Code == "110"
                        || z.Code == "120"
                        || z.Code == "130"
                        || z.Code == "140"
                        || z.Code == "150"
                        || z.Code == "160"
                        || z.Code == "170"
                        || z.Code == "180"
                        || z.Code == "190"
                        || z.Code == "200"
                        || z.Code == "210"
                        );

                        if (res220.Any())
                        {
                            var summa220 = res220.Sum(z => z.RealValue);
                            var obj220 = new BalanceContainer
                            {
                                Code = "220",
                                RealValue = summa220
                            };
                            out_Balance.Add(obj220);
                        }

                        break;

                    case "370":
                        var res370 = out_Balance.Where(z =>
                               z.Code == "230" 
                            || z.Code == "240" 
                            || z.Code == "250" 
                            || z.Code == "260"
                            || z.Code == "270"
                            || z.Code == "280"
                            || z.Code == "290"
                            || z.Code == "300"
                            || z.Code == "310"
                            || z.Code == "320"
                            || z.Code == "330"
                            || z.Code == "340"
                            || z.Code == "350"
                            || z.Code == "360"
                            );

                        if (res370.Any())
                        {
                            var summa370 = res370.Sum(z => z.RealValue);
                            var obj370 = new BalanceContainer
                            {
                                Code = "370",
                                RealValue = summa370
                            };
                            out_Balance.Add(obj370);
                        }

                        break;
                }
            }
            else if (part == (int)OtchetPart.Svedeniya)
            {

            }
            else if (part == (int)OtchetPart.Rasshifrovka)
            {

            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        private void FillFactData(int year, int kvartal, bool IsSvodniy = false)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var finSettings = db.tbbu_finOtchetSetting.Where(z => z.OTCHET_PART == (int)OtchetPart.IspolneniyaByudjeta && z.DEPARTMENT_BASE == departmentCode);

                if (finSettings.Any())
                {
                    foreach(var setting in finSettings)
                    {
                        decimal? Value = 0;

                        if (setting.DATA_TYPE == (int)FinOtchDataType.DebitOSTATOK)
                        {
                            if (IsSvodniy)
                            {
                                Value = GetBalanceDataFromValueSvodniy(year, kvartal, setting.ACCOUNT_CODE, FinOtchDataType.DebitOSTATOK);
                            }
                            else
                            {
                                Value = GetBalanceDataFromValue(year, kvartal, setting.ACCOUNT_CODE, FinOtchDataType.DebitOSTATOK);
                            }
                        }
                        else if (setting.DATA_TYPE == (int)FinOtchDataType.CreditOSTATOK)
                        {
                            if (IsSvodniy)
                            {
                                Value = GetBalanceDataFromValueSvodniy(year, kvartal, setting.ACCOUNT_CODE, FinOtchDataType.CreditOSTATOK);
                            }
                            else
                            {
                                Value = GetBalanceDataFromValue(year, kvartal, setting.ACCOUNT_CODE, FinOtchDataType.CreditOSTATOK);
                            }
                        }
                        else if (setting.DATA_TYPE == (int)FinOtchDataType.OBOROT)
                        {
                            if (IsSvodniy)
                            {
                                Value = GetValueProvodkiSvodniy(year, kvartal, setting.DEBIT_SIDE, setting.CREDIT_SIDE);
                            }
                            else
                            {
                                Value = GetValueProvodki(year, kvartal, setting.DEBIT_SIDE, setting.CREDIT_SIDE);
                            }
                        }
                        else if (setting.DATA_TYPE == (int)FinOtchDataType.DEFAULT0)
                        {
                            Value = 0;
                        }
                        else if (setting.DATA_TYPE == (int)FinOtchDataType.EXPRESSION)
                        {
                            CalcExpression((int)OtchetPart.IspolneniyaByudjeta, setting.CODE);
                            continue;
                        }

                        var objs = out_FinOtchet.Where(z => z.Code == setting.CODE);

                        if (objs.Any())
                        {
                            var obj = objs.First();
                            obj.RealValue = Value.Value;

                        }
                        else
                        {
                            var obj = new FinOtchetContainer();
                            obj.Code = setting.CODE;
                            obj.RealValue = Value.Value;
                            out_FinOtchet.Add(obj);
                        }

                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        private void FillFactDataBalance(int year, int kvartal, bool IsSvodniy = false)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var finSettings = db.tbbu_finOtchetSetting.Where(z => z.OTCHET_PART == (int)OtchetPart.Balance && z.DEPARTMENT_BASE == departmentCode);

                if (finSettings.Any())
                {
                    foreach (var setting in finSettings)
                    {
                        decimal? Value = 0;

                        if (setting.DATA_TYPE == (int)FinOtchDataType.DebitOSTATOK)
                        {
                            if (IsSvodniy)
                            {
                                Value = GetBalanceDataFromValueSvodniy(year, kvartal, setting.ACCOUNT_CODE, FinOtchDataType.DebitOSTATOK);
                            }
                            else
                            {
                                Value = GetBalanceDataFromValue(year, kvartal, setting.ACCOUNT_CODE, FinOtchDataType.DebitOSTATOK);
                            }
                        }
                        else if (setting.DATA_TYPE == (int)FinOtchDataType.CreditOSTATOK)
                        {
                            if (IsSvodniy)
                            {
                                Value = GetBalanceDataFromValueSvodniy(year, kvartal, setting.ACCOUNT_CODE, FinOtchDataType.CreditOSTATOK);
                            }
                            else
                            {
                                Value = GetBalanceDataFromValue(year, kvartal, setting.ACCOUNT_CODE, FinOtchDataType.CreditOSTATOK);
                            }
                        }
                        else if (setting.DATA_TYPE == (int)FinOtchDataType.OBOROT)
                        {
                            if (IsSvodniy)
                            {
                                Value = GetValueProvodkiSvodniy(year, kvartal, setting.DEBIT_SIDE, setting.CREDIT_SIDE);
                            }
                            else
                            {
                                Value = GetValueProvodki(year, kvartal, setting.DEBIT_SIDE, setting.CREDIT_SIDE);
                            }
                            
                        }
                        else if (setting.DATA_TYPE == (int)FinOtchDataType.EXPRESSION)
                        {
                            CalcExpression((int)OtchetPart.Balance, setting.CODE);
                            continue;
                        }

                        var obj = new BalanceContainer();
                         obj.Code = setting.CODE;
                         obj.RealValue = Value.Value;
                         out_Balance.Add(obj);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_departmentCode"></param>
        /// <param name="a_year"></param>
        /// <param name="a_kvartal"></param>
        public void MakeFinOtchet(string a_departmentCode , int a_year , int a_kvartal, long a_usrId , bool isSvodniy = false)
        {
            departmentCode = a_departmentCode;
            year = a_year;
            kvartal = a_kvartal;
            usrId = a_usrId;

            if (isSvodniy)
            {
                GetDataFromPlanSvodniy();
            }
            else
            {
                GetDataFromPlan();
            }
            
            FillFactData(a_year, a_kvartal , isSvodniy);
            FillFactDataBalance(a_year, a_kvartal , isSvodniy);
            PrepareForOutput();
        }

        private void PrepareForOutput()
        {
            if (out_FinOtchet != null)
            {
                if (_outFinance == null)
                {
                    _outFinance = new FinOtchetOutput();

                    var f010 = out_FinOtchet.Where(z => z.Code == "010").FirstOrDefault();
                    if (f010 != null)
                    {

                        _outFinance.field010P = Math.Round(f010.PlanValue / 1000 , 2);
                        _outFinance.field010R = Math.Round(f010.RealValue / 1000, 2);

                        SaveFinOtchet(usrId, 
                            departmentCode, 
                            year, 
                            kvartal, 
                            (int)OtchetPart.IspolneniyaByudjeta, 
                            "010", 
                            _outFinance.field010P, 
                            _outFinance.field010R);
                    }
                    else
                    {
                        _outFinance.field010P = null;
                        _outFinance.field010R = null;
                    }

                    var f020 = out_FinOtchet.Where(z => z.Code == "020").FirstOrDefault();
                    if (f020 != null)
                    {

                        _outFinance.field020P = Math.Round(f020.PlanValue / 1000, 2);
                        _outFinance.field020R = Math.Round(f020.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "020",
                                    _outFinance.field020P,
                                    _outFinance.field020R);
                    }
                    else
                    {
                        _outFinance.field020P = null;
                        _outFinance.field020R = null;
                    }

                    var f021 = out_FinOtchet.Where(z => z.Code == "021").FirstOrDefault();
                    if (f021 != null)
                    {

                        _outFinance.field021P = Math.Round(f021.PlanValue / 1000, 2);
                        _outFinance.field021R = Math.Round(f021.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "021",
                                    _outFinance.field021P,
                                    _outFinance.field021R);
                    }
                    else
                    {
                        _outFinance.field021P = null;
                        _outFinance.field021R = null;
                    }

                    var f022 = out_FinOtchet.Where(z => z.Code == "022").FirstOrDefault();
                    if (f022 != null)
                    {

                        _outFinance.field022P = Math.Round(f022.PlanValue / 1000, 2);
                        _outFinance.field022R = Math.Round(f022.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "022",
                                    _outFinance.field022P,
                                    _outFinance.field022R);
                    }
                    else
                    {
                        _outFinance.field022P = null;
                        _outFinance.field022R = null;
                    }

                    var f023 = out_FinOtchet.Where(z => z.Code == "023").FirstOrDefault();
                    if (f023 != null)
                    {

                        _outFinance.field023P = Math.Round(f023.PlanValue / 1000, 2);
                        _outFinance.field023R = Math.Round(f023.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "023",
                                    _outFinance.field023P,
                                    _outFinance.field023R);
                    }
                    else
                    {
                        _outFinance.field023P = null;
                        _outFinance.field023R = null;
                    }

                    var f024 = out_FinOtchet.Where(z => z.Code == "024").FirstOrDefault();
                    if (f024 != null)
                    {

                        _outFinance.field024P = Math.Round(f024.PlanValue / 1000, 2);
                        _outFinance.field024R = Math.Round(f024.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "024",
                                    _outFinance.field024P,
                                    _outFinance.field024R);
                    }
                    else
                    {
                        _outFinance.field024P = null;
                        _outFinance.field024R = null;
                    }

                    var f030 = out_FinOtchet.Where(z => z.Code == "030").FirstOrDefault();
                    if (f030 != null)
                    {

                        _outFinance.field030P = Math.Round(f030.PlanValue / 1000, 2);
                        _outFinance.field030R = Math.Round(f030.RealValue / 1000, 2);


                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "030",
                                    _outFinance.field030P,
                                    _outFinance.field030R);
                    }
                    else
                    {
                        _outFinance.field030P = null;
                        _outFinance.field030R = null;
                    }

                    var f040 = out_FinOtchet.Where(z => z.Code == "040").FirstOrDefault();
                    if (f040 != null)
                    {

                        _outFinance.field040P = Math.Round(f040.PlanValue / 1000, 2);
                        _outFinance.field040R = Math.Round(f040.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "040",
                                    _outFinance.field040P,
                                    _outFinance.field040R);
                    }
                    else
                    {
                        _outFinance.field040P = null;
                        _outFinance.field040R = null;
                    }

                    var f050 = out_FinOtchet.Where(z => z.Code == "050").FirstOrDefault();
                    if (f050 != null)
                    {

                        _outFinance.field050P = Math.Round(f050.PlanValue / 1000, 2);
                        _outFinance.field050R = Math.Round(f050.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "050",
                                    _outFinance.field050P,
                                    _outFinance.field050R);
                    }
                    else
                    {
                        _outFinance.field050P = null;
                        _outFinance.field050R = null;
                    }

                    var f060 = out_FinOtchet.Where(z => z.Code == "060").FirstOrDefault();
                    if (f060 != null)
                    {

                        _outFinance.field060P = Math.Round(f060.PlanValue / 1000, 2);
                        _outFinance.field060R = Math.Round(f060.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "060",
                                    _outFinance.field060P,
                                    _outFinance.field060R);
                    }
                    else
                    {
                        _outFinance.field060P = null;
                        _outFinance.field060R = null;
                    }

                    var f070 = out_FinOtchet.Where(z => z.Code == "070").FirstOrDefault();
                    if (f070 != null)
                    {

                        _outFinance.field070P = Math.Round(f070.PlanValue / 1000, 2);
                        _outFinance.field070R = Math.Round(f070.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "070",
                                    _outFinance.field070P,
                                    _outFinance.field070R);
                    }
                    else
                    {
                        _outFinance.field070P = null;
                        _outFinance.field070R = null;
                    }

                    var f071 = out_FinOtchet.Where(z => z.Code == "071").FirstOrDefault();
                    if (f071 != null)
                    {

                        _outFinance.field071P = Math.Round(f071.PlanValue / 1000, 2);
                        _outFinance.field071R = Math.Round(f071.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "071",
                                    _outFinance.field071P,
                                    _outFinance.field071R);
                    }
                    else
                    {
                        _outFinance.field071P = null;
                        _outFinance.field071R = null;
                    }

                    var f072 = out_FinOtchet.Where(z => z.Code == "072").FirstOrDefault();
                    if (f072 != null)
                    {

                        _outFinance.field072P = Math.Round(f072.PlanValue / 1000, 2);
                        _outFinance.field072R = Math.Round(f072.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "072",
                                    _outFinance.field072P,
                                    _outFinance.field072R);
                    }
                    else
                    {
                        _outFinance.field072P = null;
                        _outFinance.field072R = null;
                    }

                    var f080 = out_FinOtchet.Where(z => z.Code == "080").FirstOrDefault();
                    if (f080 != null)
                    {

                        _outFinance.field080P = Math.Round(f080.PlanValue / 1000, 2);
                        _outFinance.field080R = Math.Round(f080.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "080",
                                    _outFinance.field080P,
                                    _outFinance.field080R);
                    }
                    else
                    {
                        _outFinance.field080P = null;
                        _outFinance.field080R = null;
                    }

                    var f090 = out_FinOtchet.Where(z => z.Code == "090").FirstOrDefault();
                    if (f090 != null)
                    {

                        _outFinance.field090P = Math.Round(f090.PlanValue / 1000, 2);
                        _outFinance.field090R = Math.Round(f090.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "090",
                                    _outFinance.field090P,
                                    _outFinance.field090R);
                    }
                    else
                    {
                        _outFinance.field090P = null;
                        _outFinance.field090R = null;
                    }

                    var f100 = out_FinOtchet.Where(z => z.Code == "100").FirstOrDefault();
                    if (f100 != null)
                    {

                        _outFinance.field100P = Math.Round(f100.PlanValue / 1000, 2);
                        _outFinance.field100R = Math.Round(f100.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "100",
                                    _outFinance.field100P,
                                    _outFinance.field100R);
                    }
                    else
                    {
                        _outFinance.field100P = null;
                        _outFinance.field100R = null;
                    }

                    var f110 = out_FinOtchet.Where(z => z.Code == "110").FirstOrDefault();
                    if (f110 != null)
                    {

                        _outFinance.field110P = Math.Round(f110.PlanValue / 1000, 2);
                        _outFinance.field110R = Math.Round(f110.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "110",
                                    _outFinance.field110P,
                                    _outFinance.field110R);
                    }
                    else
                    {
                        _outFinance.field110P = null;
                        _outFinance.field110R = null;
                    }

                    var f120 = out_FinOtchet.Where(z => z.Code == "120").FirstOrDefault();
                    if (f120 != null)
                    {

                        _outFinance.field120P = Math.Round(f120.PlanValue / 1000, 2);
                        _outFinance.field120R = Math.Round(f120.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "120",
                                    _outFinance.field120P,
                                    _outFinance.field120R);
                    }
                    else
                    {
                        _outFinance.field120P = null;
                        _outFinance.field120R = null;
                    }

                    var f130 = out_FinOtchet.Where(z => z.Code == "130").FirstOrDefault();
                    if (f130 != null)
                    {

                        _outFinance.field130P = Math.Round(f130.PlanValue / 1000, 2);
                        _outFinance.field130R = Math.Round(f130.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "130",
                                    _outFinance.field130P,
                                    _outFinance.field130R);
                    }
                    else
                    {
                        _outFinance.field130P = null;
                        _outFinance.field130R = null;
                    }

                    var f140 = out_FinOtchet.Where(z => z.Code == "140").FirstOrDefault();
                    if (f140 != null)
                    {

                        _outFinance.field140P = Math.Round(f140.PlanValue / 1000, 2);
                        _outFinance.field140R = Math.Round(f140.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "140",
                                    _outFinance.field140P,
                                    _outFinance.field140R);
                    }
                    else
                    {
                        _outFinance.field140P = null;
                        _outFinance.field140R = null;
                    }

                    var f150 = out_FinOtchet.Where(z => z.Code == "150").FirstOrDefault();
                    if (f150 != null)
                    {

                        _outFinance.field150P = Math.Round(f150.PlanValue / 1000, 2);
                        _outFinance.field150R = Math.Round(f150.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "150",
                                    _outFinance.field150P,
                                    _outFinance.field150R);
                    }
                    else
                    {
                        _outFinance.field150P = null;
                        _outFinance.field150R = null;
                    }

                    var f160 = out_FinOtchet.Where(z => z.Code == "160").FirstOrDefault();
                    if (f160 != null)
                    {

                        _outFinance.field160P = Math.Round(f160.PlanValue / 1000, 2);
                        _outFinance.field160R = Math.Round(f160.RealValue / 1000, 2);


                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "160",
                                    _outFinance.field160P,
                                    _outFinance.field160R);
                    }
                    else
                    {
                        _outFinance.field160P = null;
                        _outFinance.field160R = null;
                    }

                    var f170 = out_FinOtchet.Where(z => z.Code == "170").FirstOrDefault();
                    if (f170 != null)
                    {

                        _outFinance.field170P = Math.Round(f170.PlanValue / 1000, 2);
                        _outFinance.field170R = Math.Round(f170.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "170",
                                    _outFinance.field170P,
                                    _outFinance.field170R);
                    }
                    else
                    {
                        _outFinance.field170P = null;
                        _outFinance.field170R = null;
                    }

                    var f180 = out_FinOtchet.Where(z => z.Code == "180").FirstOrDefault();
                    if (f180 != null)
                    {

                        _outFinance.field180P = Math.Round(f180.PlanValue / 1000, 2);
                        _outFinance.field180R = Math.Round(f180.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "180",
                                    _outFinance.field180P,
                                    _outFinance.field180R);
                    }
                    else
                    {
                        _outFinance.field180P = null;
                        _outFinance.field180R = null;
                    }

                    var f190 = out_FinOtchet.Where(z => z.Code == "190").FirstOrDefault();
                    if (f190 != null)
                    {

                        _outFinance.field190P = Math.Round(f190.PlanValue / 1000, 2);
                        _outFinance.field190R = Math.Round(f190.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "190",
                                    _outFinance.field190P,
                                    _outFinance.field190R);
                    }
                    else
                    {
                        _outFinance.field190P = null;
                        _outFinance.field190R = null;
                    }

                    var f200 = out_FinOtchet.Where(z => z.Code == "200").FirstOrDefault();
                    if (f200 != null)
                    {

                        _outFinance.field200P = Math.Round(f200.PlanValue / 1000, 2);
                        _outFinance.field200R = Math.Round(f200.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "200",
                                    _outFinance.field200P,
                                    _outFinance.field200R);
                    }
                    else
                    {
                        _outFinance.field200P = null;
                        _outFinance.field200R = null;
                    }

                    var f210 = out_FinOtchet.Where(z => z.Code == "210").FirstOrDefault();
                    if (f210 != null)
                    {

                        _outFinance.field210P = Math.Round(f210.PlanValue / 1000, 2);
                        _outFinance.field210R = Math.Round(f210.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "210",
                                    _outFinance.field210P,
                                    _outFinance.field210R);
                    }
                    else
                    {
                        _outFinance.field210P = null;
                        _outFinance.field210R = null;
                    }

                    var f220 = out_FinOtchet.Where(z => z.Code == "220").FirstOrDefault();
                    if (f220 != null)
                    {

                        _outFinance.field220P = Math.Round(f220.PlanValue / 1000, 2);
                        _outFinance.field220R = Math.Round(f220.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "220",
                                    _outFinance.field220P,
                                    _outFinance.field220R);
                    }
                    else
                    {
                        _outFinance.field220P = null;
                        _outFinance.field220R = null;
                    }

                    var f230 = out_FinOtchet.Where(z => z.Code == "230").FirstOrDefault();
                    if (f230 != null)
                    {

                        _outFinance.field230P = Math.Round(f230.PlanValue / 1000, 2);
                        _outFinance.field230R = Math.Round(f230.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "230",
                                    _outFinance.field230P,
                                    _outFinance.field230R);
                    }
                    else
                    {
                        _outFinance.field230P = null;
                        _outFinance.field230R = null;
                    }

                    var f240 = out_FinOtchet.Where(z => z.Code == "240").FirstOrDefault();
                    if (f240 != null)
                    {

                        _outFinance.field240P = Math.Round(f240.PlanValue / 1000, 2);
                        _outFinance.field240R = Math.Round(f240.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "240",
                                    _outFinance.field240P,
                                    _outFinance.field240R);
                    }
                    else
                    {
                        _outFinance.field240P = null;
                        _outFinance.field240R = null;
                    }

                    var f250 = out_FinOtchet.Where(z => z.Code == "250").FirstOrDefault();
                    if (f250 != null)
                    {

                        _outFinance.field250P = Math.Round(f250.PlanValue / 1000, 2);
                        _outFinance.field250R = Math.Round(f250.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "250",
                                    _outFinance.field250P,
                                    _outFinance.field250R);
                    }
                    else
                    {
                        _outFinance.field250P = null;
                        _outFinance.field250R = null;
                    }

                    var f260 = out_FinOtchet.Where(z => z.Code == "260").FirstOrDefault();
                    if (f260 != null)
                    {

                        _outFinance.field260P = Math.Round(f260.PlanValue / 1000, 2);
                        _outFinance.field260R = Math.Round(f260.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "260",
                                    _outFinance.field260P,
                                    _outFinance.field260R);
                    }
                    else
                    {
                        _outFinance.field260P = null;
                        _outFinance.field260R = null;
                    }

                    var f270 = out_FinOtchet.Where(z => z.Code == "270").FirstOrDefault();
                    if (f270 != null)
                    {

                        _outFinance.field270P = Math.Round(f270.PlanValue / 1000, 2);
                        _outFinance.field270R = Math.Round(f270.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "270",
                                    _outFinance.field270P,
                                    _outFinance.field270R);
                    }
                    else
                    {
                        _outFinance.field270P = null;
                        _outFinance.field270R = null;
                    }

                    var f280 = out_FinOtchet.Where(z => z.Code == "280").FirstOrDefault();
                    if (f280 != null)
                    {

                        _outFinance.field280P = Math.Round(f280.PlanValue / 1000, 2);
                        _outFinance.field280R = Math.Round(f280.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "280",
                                    _outFinance.field280P,
                                    _outFinance.field280R);
                    }
                    else
                    {
                        _outFinance.field280P = null;
                        _outFinance.field280R = null;
                    }

                    var f290 = out_FinOtchet.Where(z => z.Code == "290").FirstOrDefault();
                    if (f290 != null)
                    {

                        _outFinance.field290P = Math.Round(f290.PlanValue / 1000, 2);
                        _outFinance.field290R = Math.Round(f290.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "290",
                                    _outFinance.field290P,
                                    _outFinance.field290R);
                    }
                    else
                    {
                        _outFinance.field290P = null;
                        _outFinance.field290R = null;
                    }

                    var f300 = out_FinOtchet.Where(z => z.Code == "300").FirstOrDefault();
                    if (f300 != null)
                    {

                        _outFinance.field300P = Math.Round(f300.PlanValue / 1000, 2);
                        _outFinance.field300R = Math.Round(f300.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "300",
                                    _outFinance.field300P,
                                    _outFinance.field300R);
                    }
                    else
                    {
                        _outFinance.field300P = null;
                        _outFinance.field300R = null;
                    }

                    var f310 = out_FinOtchet.Where(z => z.Code == "310").FirstOrDefault();
                    if (f310 != null)
                    {

                        _outFinance.field310P = Math.Round(f310.PlanValue / 1000, 2);
                        _outFinance.field310R = Math.Round(f310.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "310",
                                    _outFinance.field310P,
                                    _outFinance.field310R);
                    }
                    else
                    {
                        _outFinance.field310P = null;
                        _outFinance.field310R = null;
                    }

                    var f320 = out_FinOtchet.Where(z => z.Code == "320").FirstOrDefault();
                    if (f320 != null)
                    {

                        _outFinance.field320P = Math.Round(f320.PlanValue / 1000, 2);
                        _outFinance.field320R = Math.Round(f320.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "320",
                                    _outFinance.field320P,
                                    _outFinance.field320R);
                    }
                    else
                    {
                        _outFinance.field320P = null;
                        _outFinance.field320R = null;
                    }

                    var f330 = out_FinOtchet.Where(z => z.Code == "330").FirstOrDefault();
                    if (f330 != null)
                    {

                        _outFinance.field330P = Math.Round(f330.PlanValue / 1000, 2);
                        _outFinance.field330R = Math.Round(f330.RealValue / 1000, 2);


                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "330",
                                    _outFinance.field330P,
                                    _outFinance.field330R);
                    }
                    else
                    {
                        _outFinance.field330P = null;
                        _outFinance.field330R = null;
                    }


                    var f340 = out_FinOtchet.Where(z => z.Code == "340").FirstOrDefault();
                    if (f340 != null)
                    {

                        _outFinance.field340P = Math.Round(f340.PlanValue / 1000, 2);
                        _outFinance.field340R = Math.Round(f340.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.IspolneniyaByudjeta,
                                    "340",
                                    _outFinance.field340P,
                                    _outFinance.field340R);
                    }
                    else
                    {
                        _outFinance.field340P = null;
                        _outFinance.field340R = null;
                    }

                    _totaloutput._finotchetoutput = _outFinance;
                }
            }

            if (out_Balance != null)
            {
                if (_outBalance == null)
                {
                    _outBalance = new BalanceOutput();

                    var f010 = out_Balance.Where(z => z.Code == "010").FirstOrDefault();
                    if (f010 != null)
                    {
                        _outBalance.field010 = Math.Round(f010.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "010",
                                    null,
                                    _outBalance.field010);
                    }
                    else
                    {
                        _outBalance.field010 = null;
                    }

                    var f020 = out_Balance.Where(z => z.Code == "020").FirstOrDefault();
                    if (f020 != null)
                    {
                        _outBalance.field020 = Math.Round(f020.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "020",
                                    null,
                                    _outBalance.field020);
                    }
                    else
                    {
                        _outBalance.field020 = null;
                    }

                    var f030 = out_Balance.Where(z => z.Code == "030").FirstOrDefault();
                    if (f030 != null)
                    {
                        _outBalance.field030 = Math.Round(f030.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "030",
                                    null,
                                    _outBalance.field030);
                    }
                    else
                    {
                        _outBalance.field030 = null;
                    }

                    var f040 = out_Balance.Where(z => z.Code == "040").FirstOrDefault();
                    if (f040 != null)
                    {
                        _outBalance.field040 = Math.Round(f040.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "040",
                                    null,
                                    _outBalance.field040);
                    }
                    else
                    {
                        _outBalance.field040 = null;
                    }

                    var f050 = out_Balance.Where(z => z.Code == "050").FirstOrDefault();
                    if (f050 != null)
                    {
                        _outBalance.field050 = Math.Round(f050.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "050",
                                    null,
                                    _outBalance.field050);
                    }
                    else
                    {
                        _outBalance.field050 = null;
                    }

                    var f060 = out_Balance.Where(z => z.Code == "060").FirstOrDefault();
                    if (f060 != null)
                    {
                        _outBalance.field060 = Math.Round(f060.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "060",
                                    null,
                                    _outBalance.field060);
                    }
                    else
                    {
                        _outBalance.field060 = null;
                    }

                    var f070 = out_Balance.Where(z => z.Code == "070").FirstOrDefault();
                    if (f070 != null)
                    {
                        _outBalance.field070 = Math.Round(f070.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "070",
                                    null,
                                    _outBalance.field070);
                    }
                    else
                    {
                        _outBalance.field070 = null;
                    }

                    var f080 = out_Balance.Where(z => z.Code == "080").FirstOrDefault();
                    if (f080 != null)
                    {
                        _outBalance.field080 = Math.Round(f080.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "080",
                                    null,
                                    _outBalance.field080);
                    }
                    else
                    {
                        _outBalance.field080 = null;
                    }

                    var f090 = out_Balance.Where(z => z.Code == "090").FirstOrDefault();
                    if (f090 != null)
                    {
                        _outBalance.field090 = Math.Round(f090.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "090",
                                    null,
                                    _outBalance.field090);
                    }
                    else
                    {
                        _outBalance.field090 = null;
                    }

                    var f100 = out_Balance.Where(z => z.Code == "100").FirstOrDefault();
                    if (f100 != null)
                    {
                        _outBalance.field100 = Math.Round(f100.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "100",
                                    null,
                                    _outBalance.field100);
                    }
                    else
                    {
                        _outBalance.field100 = null;
                    }

                    var f110 = out_Balance.Where(z => z.Code == "110").FirstOrDefault();
                    if (f110 != null)
                    {
                        _outBalance.field110 = Math.Round(f110.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "110",
                                    null,
                                    _outBalance.field110);
                    }
                    else
                    {
                        _outBalance.field110 = null;
                    }

                    var f120 = out_Balance.Where(z => z.Code == "120").FirstOrDefault();
                    if (f120 != null)
                    {
                        _outBalance.field120 = Math.Round(f120.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "120",
                                    null,
                                    _outBalance.field120);
                    }
                    else
                    {
                        _outBalance.field120 = null;
                    }

                    var f130 = out_Balance.Where(z => z.Code == "130").FirstOrDefault();
                    if (f130 != null)
                    {
                        _outBalance.field130 = Math.Round(f130.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "130",
                                    null,
                                    _outBalance.field130);
                    }
                    else
                    {
                        _outBalance.field130 = null;
                    }

                    var f140 = out_Balance.Where(z => z.Code == "140").FirstOrDefault();
                    if (f140 != null)
                    {
                        _outBalance.field140 = Math.Round(f140.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "140",
                                    null,
                                    _outBalance.field140);
                    }
                    else
                    {
                        _outBalance.field140 = null;
                    }

                    var f150 = out_Balance.Where(z => z.Code == "150").FirstOrDefault();
                    if (f150 != null)
                    {
                        _outBalance.field150 = Math.Round(f150.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "150",
                                    null,
                                    _outBalance.field150);
                    }
                    else
                    {
                        _outBalance.field150 = null;
                    }

                    var f160 = out_Balance.Where(z => z.Code == "160").FirstOrDefault();
                    if (f160 != null)
                    {
                        _outBalance.field160 = Math.Round(f160.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "160",
                                    null,
                                    _outBalance.field160);
                    }
                    else
                    {
                        _outBalance.field160 = null;
                    }

                    var f170 = out_Balance.Where(z => z.Code == "170").FirstOrDefault();
                    if (f170 != null)
                    {
                        _outBalance.field170 = Math.Round(f170.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "170",
                                    null,
                                    _outBalance.field170);
                    }
                    else
                    {
                        _outBalance.field170 = null;
                    }

                    var f180 = out_Balance.Where(z => z.Code == "180").FirstOrDefault();
                    if (f180 != null)
                    {
                        _outBalance.field180 = Math.Round(f180.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "180",
                                    null,
                                    _outBalance.field180);
                    }
                    else
                    {
                        _outBalance.field180 = null;
                    }

                    var f190 = out_Balance.Where(z => z.Code == "190").FirstOrDefault();
                    if (f190 != null)
                    {
                        _outBalance.field190 = Math.Round(f190.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "190",
                                    null,
                                    _outBalance.field190);
                    }
                    else
                    {
                        _outBalance.field190 = null;
                    }

                    var f200 = out_Balance.Where(z => z.Code == "200").FirstOrDefault();
                    if (f200 != null)
                    {
                        _outBalance.field200 = Math.Round(f200.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "200",
                                    null,
                                    _outBalance.field200);
                    }
                    else
                    {
                        _outBalance.field200 = null;
                    }

                    var f210 = out_Balance.Where(z => z.Code == "210").FirstOrDefault();
                    if (f210 != null)
                    {
                        _outBalance.field210 = Math.Round(f210.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "210",
                                    null,
                                    _outBalance.field210);
                    }
                    else
                    {
                        _outBalance.field210 = null;
                    }

                    var f220 = out_Balance.Where(z => z.Code == "220").FirstOrDefault();
                    if (f220 != null)
                    {
                        _outBalance.field220 = Math.Round(f220.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "220",
                                    null,
                                    _outBalance.field220);
                    }
                    else
                    {
                        _outBalance.field220 = null;
                    }

                    var f230 = out_Balance.Where(z => z.Code == "230").FirstOrDefault();
                    if (f230 != null)
                    {
                        _outBalance.field230 = Math.Round(f230.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "230",
                                    null,
                                    _outBalance.field230);
                    }
                    else
                    {
                        _outBalance.field230 = null;
                    }

                    var f240 = out_Balance.Where(z => z.Code == "240").FirstOrDefault();
                    if (f240 != null)
                    {
                        _outBalance.field240 = Math.Round(f240.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "240",
                                    null,
                                    _outBalance.field240);
                    }
                    else
                    {
                        _outBalance.field240 = null;
                    }

                    var f250 = out_Balance.Where(z => z.Code == "250").FirstOrDefault();
                    if (f250 != null)
                    {
                        _outBalance.field250 = Math.Round(f250.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "250",
                                    null,
                                    _outBalance.field250);
                    }
                    else
                    {
                        _outBalance.field250 = null;
                    }

                    var f260 = out_Balance.Where(z => z.Code == "260").FirstOrDefault();
                    if (f260 != null)
                    {
                        _outBalance.field260 = Math.Round(f260.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "260",
                                    null,
                                    _outBalance.field260);
                    }
                    else
                    {
                        _outBalance.field260 = null;
                    }

                    var f270 = out_Balance.Where(z => z.Code == "270").FirstOrDefault();
                    if (f270 != null)
                    {
                        _outBalance.field270 = Math.Round(f270.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "270",
                                    null,
                                    _outBalance.field270);
                    }
                    else
                    {
                        _outBalance.field270 = null;
                    }

                    var f280 = out_Balance.Where(z => z.Code == "280").FirstOrDefault();
                    if (f280 != null)
                    {
                        _outBalance.field280 = Math.Round(f280.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "280",
                                    null,
                                    _outBalance.field280);
                    }
                    else
                    {
                        _outBalance.field280 = null;
                    }

                    var f290 = out_Balance.Where(z => z.Code == "290").FirstOrDefault();
                    if (f290 != null)
                    {
                        _outBalance.field290 = Math.Round(f290.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "290",
                                    null,
                                    _outBalance.field290);
                    }
                    else
                    {
                        _outBalance.field290 = null;
                    }

                    var f300 = out_Balance.Where(z => z.Code == "300").FirstOrDefault();
                    if (f300 != null)
                    {
                        _outBalance.field300 = Math.Round(f300.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "300",
                                    null,
                                    _outBalance.field300);
                    }
                    else
                    {
                        _outBalance.field300 = null;
                    }

                    var f310 = out_Balance.Where(z => z.Code == "310").FirstOrDefault();
                    if (f310 != null)
                    {
                        _outBalance.field310 = Math.Round(f310.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "310",
                                    null,
                                    _outBalance.field310);
                    }
                    else
                    {
                        _outBalance.field310 = null;
                    }

                    var f320 = out_Balance.Where(z => z.Code == "320").FirstOrDefault();
                    if (f320 != null)
                    {
                        _outBalance.field320 = Math.Round(f320.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "320",
                                    null,
                                    _outBalance.field320);
                    }
                    else
                    {
                        _outBalance.field320 = null;
                    }

                    var f330 = out_Balance.Where(z => z.Code == "330").FirstOrDefault();
                    if (f330 != null)
                    {
                        _outBalance.field330 = Math.Round(f330.RealValue / 1000, 2);


                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "330",
                                    null,
                                    _outBalance.field330);
                    }
                    else
                    {
                        _outBalance.field330 = null;
                    }

                    var f340 = out_Balance.Where(z => z.Code == "340").FirstOrDefault();
                    if (f340 != null)
                    {
                        _outBalance.field340 = Math.Round(f340.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "340",
                                    null,
                                    _outBalance.field340);
                    }
                    else
                    {
                        _outBalance.field340 = null;
                    }

                    var f350 = out_Balance.Where(z => z.Code == "350").FirstOrDefault();
                    if (f350 != null)
                    {
                        _outBalance.field350 = Math.Round(f350.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "350",
                                    null,
                                    _outBalance.field350);
                    }
                    else
                    {
                        _outBalance.field350 = null;
                    }

                    var f360 = out_Balance.Where(z => z.Code == "360").FirstOrDefault();
                    if (f360 != null)
                    {
                        _outBalance.field360 = Math.Round(f360.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "360",
                                    null,
                                    _outBalance.field360);
                    }
                    else
                    {
                        _outBalance.field360 = null;
                    }

                    var f370 = out_Balance.Where(z => z.Code == "370").FirstOrDefault();
                    if (f370 != null)
                    {
                        _outBalance.field370 = Math.Round(f370.RealValue / 1000, 2);

                        SaveFinOtchet(usrId,
                                    departmentCode,
                                    year,
                                    kvartal,
                                    (int)OtchetPart.Balance,
                                    "370",
                                    null,
                                    _outBalance.field370);
                    }
                    else
                    {
                        _outBalance.field370 = null;
                    }

                    _totaloutput._balanceoutput = _outBalance;

                    _totaloutput._balanceoutput.field250x           = _totaloutput._balanceoutput.field200 - _totaloutput._finotchetoutput.field250R;

                    SaveFinOtchet(usrId,
                                departmentCode,
                                year,
                                kvartal,
                                (int)OtchetPart.Balance,
                                "250x",
                                null,
                                _totaloutput._balanceoutput.field250x);


                    _totaloutput._balanceoutput.field320x           = _totaloutput._balanceoutput.field210 - _totaloutput._finotchetoutput.field320R;

                    SaveFinOtchet(usrId,
                                departmentCode,
                                year,
                                kvartal,
                                (int)OtchetPart.Balance,
                                "320x",
                                null,
                                _totaloutput._balanceoutput.field320x);

                    _totaloutput._balanceoutput.field060x           = _totaloutput._balanceoutput.field330 - _totaloutput._finotchetoutput.field060R;

                    SaveFinOtchet(usrId,
                                departmentCode,
                                year,
                                kvartal,
                                (int)OtchetPart.Balance,
                                "060x",
                                null,
                                _totaloutput._balanceoutput.field060x);


                    _totaloutput._balanceoutput.field100x           = _totaloutput._balanceoutput.field340 - _totaloutput._finotchetoutput.field100R;

                    SaveFinOtchet(usrId,
                                departmentCode,
                                year,
                                kvartal,
                                (int)OtchetPart.Balance,
                                "100x",
                                null,
                                _totaloutput._balanceoutput.field100x);

                    _totaloutput._balanceoutput.fieldActivePassive  = _totaloutput._balanceoutput.field370 - _totaloutput._balanceoutput.field220;

                    SaveFinOtchet(usrId,
                                departmentCode,
                                year,
                                kvartal,
                                (int)OtchetPart.Balance,
                                "999",
                                null,
                                _totaloutput._balanceoutput.fieldActivePassive);
                }
            }

            
        }

        private void SaveFinOtchet(long usrId,
                                   string a_departmentCode,
                                   int a_year,
                                   int a_kvartal,
                                   int part,
                                   string code,
                                   decimal? plan_amount,
                                   decimal? fact_amount)
        {
            //Validation
            if (usrId <= 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(a_departmentCode))
            {
                return;
            }

            if (a_year <= 0)
            {
                return;
            }

            if (a_kvartal <= 0)
            {
                return;
            }

            if (part <= 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(code))
            {
                return;
            }

            if (plan_amount == null && plan_amount <= 0)
            {
                return;
            }

            if (fact_amount == null && fact_amount <= 0)
            {
                return;
            }

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var finAlreadyExists = db.tbbu_FinOtchetSave.Where(z => z.DEPARTMENT_CODE == a_departmentCode &&
                                                                   z.YEAR == a_year &&
                                                                   z.KVARTAL == a_kvartal &&
                                                                   z.PART == part &&
                                                                   z.CODE == code);

                if (finAlreadyExists.Any())
                {
                    foreach (var finOt in finAlreadyExists)
                    {
                        db.tbbu_FinOtchetSave.Remove(finOt);
                    }
                }

                var obj = new tbbu_FinOtchetSave();
                obj.YEAR = a_year;
                obj.KVARTAL = a_kvartal;
                obj.PART = part;
                obj.CODE = code;
                obj.PLAN_AMOUNT = plan_amount;
                obj.FACT_AMOUNT = fact_amount;
                obj.USER_ID = usrId;
                obj.DEPARTMENT_CODE = departmentCode;

                db.tbbu_FinOtchetSave.Add(obj);

                db.SaveChanges();
            }
        }
    }
}