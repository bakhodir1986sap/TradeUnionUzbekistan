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
    public enum OtchetPart
    {
        IspolneniyaByudjeta = 1,
        Balance,
        Svedeniya,
        Rasshifrovka
    }

    public enum FinOtchDataType
    {
        DebitOSTATOK,
        CreditOSTATOK,
        OBOROT,
        EXPRESSION,
        VIBORKA,
        DEFAULT0
    }

    public class FinOtchetContainer
    {
        public string Code       { get; set; }

        public decimal PlanValue { get; set; }

        public decimal RealValue { get; set; }
    }


    public class BalanceContainer
    {
        public string Code { get; set; }

        public decimal RealValue { get; set; }
    }

    public class RasxodiContainer
    {

    }

    public class FinOtchetStrukture
    {
        //input parametrs if 9999 - svodniy po respubliki
        public string departmentCode { get; set; }

        public int year { get; set; }

        public int kvartal { get; set; }

        private Dictionary<string, string> planCodeFinCode { get; set; }

        //Out Parametrs
        public List<FinOtchetContainer> out_FinOtchet { get; set; }
        public List<BalanceContainer> out_Balance { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public FinOtchetStrukture()
        {
            planCodeFinCode = new Dictionary<string, string>();
            out_FinOtchet = new List<FinOtchetContainer>();
            out_Balance = new List<BalanceContainer>();
            MapPlanCodeFinCode();
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
        public void GetDataFromPlan()
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
                            var obj060 = new FinOtchetContainer
                            {
                                Code = "060",
                                RealValue = summa060
                            };
                            out_FinOtchet.Add(obj060);
                        }

                        break;

                    case "100":

                        var res100 = out_FinOtchet.Where(z => z.Code == "070" || z.Code == "080" || z.Code == "090");

                        if (res100.Any())
                        {
                            var summa100 = res100.Sum(z => z.RealValue);
                            var obj100 = new FinOtchetContainer
                            {
                                Code = "100",
                                RealValue = summa100
                            };
                            out_FinOtchet.Add(obj100);
                        }

                        break;

                    case "110":

                        var res110 = out_FinOtchet.Where(z => z.Code == "010" || z.Code == "060" || z.Code == "100");

                        if (res110.Any())
                        {
                            var summa110 = res110.Sum(z => z.RealValue);
                            var obj110 = new FinOtchetContainer
                            {
                                Code = "110",
                                RealValue = summa110
                            };
                            out_FinOtchet.Add(obj110);
                        }

                        break;

                    case "160":

                        var res160 = out_FinOtchet.Where(z => z.Code == "120" || z.Code == "130" || z.Code == "140" || z.Code == "150");

                        if (res160.Any())
                        {
                            var summa160 = res160.Sum(z => z.RealValue);
                            var obj160 = new FinOtchetContainer
                            {
                                Code = "160",
                                RealValue = summa160
                            };
                            out_FinOtchet.Add(obj160);
                        }

                        break;

                    case "220":

                        var res220 = out_FinOtchet.Where(z => z.Code == "170" || z.Code == "180" || z.Code == "180" || z.Code == "200" || z.Code == "210");

                        if (res220.Any())
                        {
                            var summa220 = res220.Sum(z => z.RealValue);
                            var obj220 = new FinOtchetContainer
                            {
                                Code = "220",
                                RealValue = summa220
                            };
                            out_FinOtchet.Add(obj220);
                        }

                        break;
                    case "250":

                        var res250 = out_FinOtchet.Where(z => z.Code == "160" || z.Code == "220" || z.Code == "230" || z.Code == "240");

                        if (res250.Any())
                        {
                            var summa250 = res250.Sum(z => z.RealValue);
                            var obj250 = new FinOtchetContainer
                            {
                                Code = "250",
                                RealValue = summa250
                            };
                            out_FinOtchet.Add(obj250);
                        }

                        break;

                    case "320":

                        var res320 = out_FinOtchet.Where(z => z.Code == "260" || z.Code == "270" || z.Code == "280" || z.Code == "290"
                        || z.Code == "300" || z.Code == "310");

                        if (res320.Any())
                        {
                            var summa320 = res320.Sum(z => z.RealValue);
                            var obj320 = new FinOtchetContainer
                            {
                                Code = "320",
                                RealValue = summa320
                            };
                            out_FinOtchet.Add(obj320);
                        }

                        break;

                    case "330":

                        var res330 = out_FinOtchet.Where(z => z.Code == "250" || z.Code == "320");

                        if (res330.Any())
                        {
                            var summa330 = res330.Sum(z => z.RealValue);
                            var obj330 = new FinOtchetContainer
                            {
                                Code = "330",
                                RealValue = summa330
                            };
                            out_FinOtchet.Add(obj330);
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
        public void FillFactData(int year, int kvartal)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var finSettings = db.tbbu_finOtchetSetting.Where(z => z.OTCHET_PART == (int)OtchetPart.IspolneniyaByudjeta);

                if (finSettings.Any())
                {
                    foreach(var setting in finSettings)
                    {
                        decimal? Value = 0;

                        if (setting.DATA_TYPE == (int)FinOtchDataType.DebitOSTATOK)
                        {
                            Value = GetBalanceDataFromValue(year, kvartal, setting.ACCOUNT_CODE, FinOtchDataType.DebitOSTATOK);
                        }
                        else if (setting.DATA_TYPE == (int)FinOtchDataType.CreditOSTATOK)
                        {
                            Value = GetBalanceDataFromValue(year, kvartal, setting.ACCOUNT_CODE, FinOtchDataType.CreditOSTATOK);
                        }
                        else if (setting.DATA_TYPE == (int)FinOtchDataType.OBOROT)
                        {
                            Value = GetValueProvodki(year, kvartal, setting.DEBIT_SIDE, setting.CREDIT_SIDE);
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
        public void FillFactDataBalance(int year, int kvartal)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var finSettings = db.tbbu_finOtchetSetting.Where(z => z.OTCHET_PART == (int)OtchetPart.Balance);

                if (finSettings.Any())
                {
                    foreach (var setting in finSettings)
                    {
                        decimal? Value = 0;

                        if (setting.DATA_TYPE == (int)FinOtchDataType.DebitOSTATOK)
                        {
                            Value = GetBalanceDataFromValue(year, kvartal, setting.ACCOUNT_CODE, FinOtchDataType.DebitOSTATOK);
                        }
                        else if (setting.DATA_TYPE == (int)FinOtchDataType.CreditOSTATOK)
                        {
                            Value = GetBalanceDataFromValue(year, kvartal, setting.ACCOUNT_CODE, FinOtchDataType.CreditOSTATOK);
                        }
                        else if (setting.DATA_TYPE == (int)FinOtchDataType.OBOROT)
                        {
                            Value = GetValueProvodki(year, kvartal, setting.DEBIT_SIDE, setting.CREDIT_SIDE);
                        }

                         var obj = new BalanceContainer();
                         obj.Code = setting.CODE;
                         obj.RealValue = Value.Value;
                         out_Balance.Add(obj);
                    }
                }
            }
        }
    }
}