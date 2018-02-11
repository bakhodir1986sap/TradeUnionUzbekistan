using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.BankOplati.BankOplatQueries
{
    public class BankPaymentsQueries
    {
        #region Quaterly Total Value
        public static decimal GetIncomeForOblastQuaterly(int year, int kvartal, int oblastId)
        {
            return 0;
        }

        public static decimal GetIncomeForOtraslQuaterly(int year, int kvartal, int otraslId)
        {
            return 0;
        }

        public static decimal GetIncomeForCompanyQuaterly(int year, int kvartal, string Inn)
        {
            return 0;
        }
        #endregion

        #region Quaterly By each month
        public static List<BankPaymentsResM> GetIncomeForOblastQuaterlyMthly(int year, int kvartal, int oblastId)
        {
            return new List<BankPaymentsResM>();
        }

        public static List<BankPaymentsResM> GetIncomeForOtraslQuaterlyMthly(int year, int kvartal, int otraslId)
        {
            return new List<BankPaymentsResM>();
        }

        public static List<BankPaymentsResM> GetIncomeForCompanyQuaterlyMthly(int year, int kvartal, string Inn)
        {
            return new List<BankPaymentsResM>();
        }
        #endregion

        #region Yearly for each Quater
        public static List<BankPaymentsResQ> GetIncomeForOblastYearlyQ(int year, int oblastId)
        {
            return new List<BankPaymentsResQ>();
        }

        public static List<BankPaymentsResQ> GetIncomeForOtraslYearlyQ(int year, int otraslId)
        {
            return new List<BankPaymentsResQ>();
        }

        public static List<BankPaymentsResQ> GetIncomeForCompanyYearlyQ(int year, string Inn)
        {
            return new List<BankPaymentsResQ>();
        }
        #endregion

        #region Yearly for each Month
        public static List<BankPaymentsResM> GetIncomeForOblastYearlyMth(int year, int oblastId)
        {
            return new List<BankPaymentsResM>();
        }

        public static List<BankPaymentsResM> GetIncomeForOtraslYearlyMth(int year, int otraslId)
        {
            return new List<BankPaymentsResM>();
        }

        public static List<BankPaymentsResM> GetIncomeForCompanyYearlyMth(int year, string Inn)
        {
            return new List<BankPaymentsResM>();
        }
        #endregion

        #region Yearly Total
        public static decimal GetIncomeForOblastYearlyT(int year, int oblastId)
        {
            return 0;
        }

        public static decimal GetIncomeForOtraslYearlyT(int year, int otraslId)
        {
            return 0;
        }

        public static decimal GetIncomeForCompanyYearlyT(int year, string Inn)
        {
            return 0;
        }
        #endregion

        #region Monthly Total
        public static decimal GetIncomeForOblastMonthly(int year, int month ,int oblastId)
        {
            return 0;
        }

        public static decimal GetIncomeForOtraslMonthly(int year, int month, int otraslId)
        {
            return 0;
        }

        public static decimal GetIncomeForCompanyMonthly(int year, int month, string Inn)
        {
            return 0;
        }
        #endregion
    }


    public class BankPaymentsQueriesN
    {
        #region Quaterly Total Value
        public static decimal? GetIncomeForOblastQuaterly(int year, int kvartal, string soato)
        {
            decimal? res = 0;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                res = db.sp_get_BankPaymentSoatoYearQuaterSum(soato, year, kvartal).FirstOrDefault();
            }

            return res;
        }

        public static decimal? GetIncomeForOtraslQuaterly(int year, int kvartal, int branchId)
        {
            decimal? res = 0;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                res = db.sp_get_BankPaymentOtraslQuaterlySum(branchId, year, kvartal).FirstOrDefault();
            }

            return res;
        }

        public static decimal? GetIncomeForCompanyQuaterly(int year, int kvartal, string Inn)
        {
            decimal? res = 0;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                res = db.sp_get_BankPaymentCompanyYearQuaterSum(Inn, year, kvartal).FirstOrDefault();
            }

            return res;
        }
        #endregion

        #region Quaterly By each month
        public static List<BankPaymentsResM> GetIncomeForOblastQuaterlyMthly(int year, int kvartal, string soato)
        {
            List<BankPaymentsResM> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var resList = db.sp_get_BankPaymentSoatoYearQuaterMonthly(soato, year, kvartal);

                if (resList.Count() > 0)
                {
                    list = new List<BankPaymentsResM>();
                }

                foreach(var res in resList)
                {
                    var obj = new BankPaymentsResM();

                    obj.Month = res.MONTH_NUM;
                    obj.Summa = res.SUMMA;
                }
            }

            return list;
        }

        public static List<BankPaymentsResM> GetIncomeForOtraslQuaterlyMthly(int year, int kvartal, int branchId)
        {
            List<BankPaymentsResM> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var resList = db.sp_get_BankPaymentOtraslYearQuaterMonthly(branchId, year, kvartal);

                if (resList.Count() > 0)
                {
                    list = new List<BankPaymentsResM>();
                }

                foreach (var res in resList)
                {
                    var obj = new BankPaymentsResM();

                    obj.Month = res.MONTH_NUM;
                    obj.Summa = res.SUMMA;
                }
            }

            return list;
        }

        public static List<BankPaymentsResM> GetIncomeForCompanyQuaterlyMthly(int year, int kvartal, string Inn)
        {
            List<BankPaymentsResM> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var resList = db.sp_get_BankPaymentCompanyYearQuaterMonthly(Inn, year, kvartal);

                if (resList.Count() > 0)
                {
                    list = new List<BankPaymentsResM>();
                }

                foreach (var res in resList)
                {
                    var obj = new BankPaymentsResM();

                    obj.Month = res.MONTH_NUM;
                    obj.Summa = res.SUMMA;
                }
            }

            return list;
        }
        #endregion

        #region Yearly for each Quater
        public static List<BankPaymentsResQ> GetIncomeForOblastYearlyQ(int year, string soato)
        {
            List<BankPaymentsResQ> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var resList = db.sp_get_BankPaymentOblastYearlyQ(soato, year);

                if (resList.Count() > 0)
                {
                    list = new List<BankPaymentsResQ>();
                }

                foreach (var res in resList)
                {
                    var obj = new BankPaymentsResM();

                    obj.Month = res.QUATER_NUM;
                    obj.Summa = res.SUMMA;
                }
            }

            return list;
        }

        public static List<BankPaymentsResQ> GetIncomeForOtraslYearlyQ(int year, int branchId)
        {
            List<BankPaymentsResQ> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var resList = db.sp_get_BankPaymentOtraslYearlyQ(branchId, year);

                if (resList.Count() > 0)
                {
                    list = new List<BankPaymentsResQ>();
                }

                foreach (var res in resList)
                {
                    var obj = new BankPaymentsResM();

                    obj.Month = res.MONTH_NUM;
                    obj.Summa = res.SUMMA;
                }
            }

            return list;
        }

        public static List<BankPaymentsResQ> GetIncomeForCompanyYearlyQ(int year, string Inn)
        {
            List<BankPaymentsResQ> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var resList = db.sp_get_BankPaymentCompanyYearlyQ(Inn, year);

                if (resList.Count() > 0)
                {
                    list = new List<BankPaymentsResQ>();
                }

                foreach (var res in resList)
                {
                    var obj = new BankPaymentsResM();

                    obj.Month = res.MONTH_NUM;
                    obj.Summa = res.SUMMA;
                }
            }

            return list;
        }
        #endregion

        #region Yearly for each Month
        public static List<BankPaymentsResM> GetIncomeForOblastYearlyMth(int year, string soato)
        {
            List<BankPaymentsResM> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var resList = db.sp_get_BankPaymentSoatoYearMonthly(soato, year);

                if (resList.Count() > 0)
                {
                    list = new List<BankPaymentsResM>();
                }

                foreach (var res in resList)
                {
                    var obj = new BankPaymentsResM();

                    obj.Month = res.MONTH_NUM;
                    obj.Summa = res.SUMMA;
                }
            }

            return list;
        }

        public static List<BankPaymentsResM> GetIncomeForOtraslYearlyMth(int year, int branchId)
        {
            List<BankPaymentsResM> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var resList = db.sp_get_BankPaymentOtraslYearMonthly(branchId, year);

                if (resList.Count() > 0)
                {
                    list = new List<BankPaymentsResM>();
                }

                foreach (var res in resList)
                {
                    var obj = new BankPaymentsResM();

                    obj.Month = res.MONTH_NUM;
                    obj.Summa = res.SUMMA;
                }
            }

            return list;
        }

        public static List<BankPaymentsResM> GetIncomeForCompanyYearlyMth(int year, string Inn)
        {
            List<BankPaymentsResM> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var resList = db.sp_get_BankPaymentCompanyYearMonthly(Inn, year);

                if (resList.Count() > 0)
                {
                    list = new List<BankPaymentsResM>();
                }

                foreach (var res in resList)
                {
                    var obj = new BankPaymentsResM();

                    obj.Month = res.MONTH_NUM;
                    obj.Summa = res.SUMMA;
                }
            }

            return list;
        }
        #endregion

        #region Yearly Total
        public static decimal? GetIncomeForOblastYearlyT(int year, string soato)
        {
            decimal? res = 0;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                res = db.sp_get_BankPaymentSoatoYearT(soato, year).FirstOrDefault();
            }

            return res;
        }

        public static decimal? GetIncomeForOtraslYearlyT(int year, int branchId)
        {
            decimal? res = 0;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                res = db.sp_get_BankPaymentOtraslYearT(branchId, year).FirstOrDefault();
            }

            return res;
        }

        public static decimal? GetIncomeForOtraslYearlyT2(int year, int branchId, string soato)
        {
            decimal? res = 0;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                res = db.sp_get_BankPaymentOtraslYearT2(branchId, year, soato).FirstOrDefault();
            }

            return res;
        }

        public static decimal? GetIncomeForCompanyYearlyT(int year, string Inn)
        {
            decimal? res = 0;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                res = db.sp_get_BankPaymentCompanyYearT(Inn, year).FirstOrDefault();
            }

            return res;
        }
        #endregion

        #region Monthly Total
        public static decimal? GetIncomeForOblastMonthly(int year, int month, string soato)
        {
            decimal? res = 0;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                res = db.sp_get_BankPaymentOblastMonthly(soato, year,month).FirstOrDefault();
            }

            return res;
        }

        public static decimal? GetIncomeForOtraslMonthly(int year, int month, int branchId)
        {
            decimal? res = 0;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                res = db.sp_get_BankPaymentOtraslMonthly(branchId, year, month).FirstOrDefault();
            }

            return res;
        }

        public static decimal? GetIncomeForCompanyMonthly(int year, int month, string Inn)
        {
            decimal? res = 0;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                res = db.sp_get_BankPaymentCompanyMonthly(Inn, year, month).FirstOrDefault();
            }

            return res;
        }
        #endregion
    }
}