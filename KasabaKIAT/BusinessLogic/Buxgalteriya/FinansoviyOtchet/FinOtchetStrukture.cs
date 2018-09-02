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
        FinOtchet,
        Balance,
        Rasxod
    }

    public enum FinOtchDataType
    {
        DebitOSTATOK,
        CreditOSTATOK,
        OBOROT,
        EXPRESSION,
        VIBORKA
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
        //input parametrs
        public string departmentCode { get; set; }

        public int year { get; set; }

        public int kvartal { get; set; }

        private Dictionary<string, string> planCodeFinCode { get; set; }

        //Out Parametrs
        public List<FinOtchetContainer> out_FinOtchet { get; set; }
        public List<BalanceContainer> out_Balance { get; set; }
        


        public FinOtchetStrukture()
        {
            planCodeFinCode = new Dictionary<string, string>();
            out_FinOtchet = new List<FinOtchetContainer>();
            out_Balance = new List<BalanceContainer>();
        }

        //planovaya
        //Should be mapped Planbyudgeta i Code 
        private void MapPlanCodeFinCode()
        {
            planCodeFinCode.Add("01"    , "");
            planCodeFinCode.Add("02"    , "");
            planCodeFinCode.Add("03"    , "");
            planCodeFinCode.Add("04"    , "");
            planCodeFinCode.Add("05"    , "");
            planCodeFinCode.Add("06"    , "");
            planCodeFinCode.Add("07"    , "");
            planCodeFinCode.Add("08"    , "");
            planCodeFinCode.Add("09"    , "");
            planCodeFinCode.Add("10"    , "");
            planCodeFinCode.Add("11"    , "");
            planCodeFinCode.Add("12"    , "");
            planCodeFinCode.Add("13"    , "");
            planCodeFinCode.Add("14"    , "");
            planCodeFinCode.Add("15"    , "");
            planCodeFinCode.Add("16"    , "");
            planCodeFinCode.Add("161"   , "");
            planCodeFinCode.Add("162"   , "");
            planCodeFinCode.Add("17"    , "");
            planCodeFinCode.Add("18"    , "");
            planCodeFinCode.Add("19"    , "");
            planCodeFinCode.Add("20"    , "");
            planCodeFinCode.Add("21"    , "");
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

                string soato_code = departmentCode.Remove(3, 1);

                var plans = db.tbbu_planbyudjetregion.Where(z => z.SOATO_CODE == soato_code && z.YEAR == year);

                if (plans.Any())
                {
                    foreach(var plan  in plans)
                    {
                        var obj = new FinOtchetContainer();
                        string out_finCode = string.Empty;

                        if (planCodeFinCode.TryGetValue(plan.ITEM_CODE , out out_finCode))
                        {
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
                                where ((kvartal == 1 && (prov.TRDT.Value.Month >= 1 && prov.TRDT.Value.Month <= 3)) ||
                                (kvartal == 2 && (prov.TRDT.Value.Month >= 4 && prov.TRDT.Value.Month <= 6)) ||
                                (kvartal == 3 && (prov.TRDT.Value.Month >= 7 && prov.TRDT.Value.Month <= 9)) ||
                                (kvartal == 4 && (prov.TRDT.Value.Month >= 10 && prov.TRDT.Value.Month <= 12)))
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
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        private void FillFactData(int year, int kvartal)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var finSettings = db.tbbu_finOtchetSetting.Where(z => z.OTCHET_PART == 0);

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
        private void FillFactDataBalance(int year, int kvartal)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var finSettings = db.tbbu_finOtchetSetting.Where(z => z.OTCHET_PART == 0);

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