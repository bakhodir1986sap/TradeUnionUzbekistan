using KasabaKIAT.BusinessLogic.Utils;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.GlavnayaKniga
{
    public enum GlavBookSt
    {
        Rows = 14,
        Cols = 15
    }

    public class GlavnayaKnigaOut
    {
        //Saldo
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, decimal?> Dsaldo { get; set; }

        //Oboroti
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, Dictionary<string, decimal?>> Doboroti { get; set; }

        //Summa Oborotov za kvartal
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, decimal?> DzaKvartal { get; set; }

        //Summa Oborotov vsego
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, decimal?> DzaVsego { get; set; }

        //Saldo
        public Dictionary<string, decimal?> Csaldo { get; set; }

        //Oboroti
        public Dictionary<string, Dictionary<string, decimal?>> Coboroti { get; set; }

        //Summa Oborotov za kvartal
        public Dictionary<string, decimal?> CzaKvartal { get; set; }

        //Summa Oborotov vsego
        public Dictionary<string, decimal?> CzaVsego { get; set; }

        //Aktiv
        public Dictionary<string, decimal?> aktiv { get; set; }

        //Passiv
        public Dictionary<string, decimal?> passiv { get; set; }


    }

    public class CarcasForGlavBook
    {
        public static List<string> GetCarcas(GlavBookSt glType)
        {
            List<string> retList = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                retList = db.tbcs_dictionary.Where(z => z.DICTIONARY_TYPE_ID == (int)glType).Select(z => z.NAME_RU).ToList();
            }

            return retList;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class GK_Debit
    {
        #region InputParametrs

        private string DepartmentBase { get; set; }

        private int Year { get; set; }

        private int Kvartal { get; set; }

        #endregion
        //Saldo
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, decimal?> saldo { get; set; }

        //Oboroti
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, Dictionary<string, decimal?>> oboroti { get; set; }

        //Summa Oborotov za kvartal
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, decimal?> zaKvartal { get; set; }

        //Summa Oborotov vsego
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, decimal?> zaVsego { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GK_Debit(string depCode, int year, int kvartal)
        {
            saldo = new Dictionary<string, decimal?>();
            oboroti = new Dictionary<string, Dictionary<string, decimal?>>();
            zaKvartal = new Dictionary<string, decimal?>();
            zaVsego = new Dictionary<string, decimal?>();

            DepartmentBase = depCode;
            Year = year;
            Kvartal = kvartal;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CalcSaldo()
        {
            if (saldo != null)
            {
                //Get info, Fill Collection
                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    var RowLists = CarcasForGlavBook.GetCarcas(GlavBookSt.Rows);

                    int o_year, o_kv;

                    DateOperations.GetPrevQuarter(Year, Kvartal, out o_year, out o_kv);

                    foreach (var row in RowLists)
                    {
                        var dbRes = db.tbbu_balance.Where(z => z.DEPARTMENT_BASE == DepartmentBase
                                                         && z.CUR_YEAR == o_year
                                                         && z.CUR_KV == o_kv
                                                         && z.ACCOUNT_CODE == row);

                        if (dbRes.Any())
                        {
                            var resAccount = dbRes.First();

                            saldo.Add(row, resAccount.DEBIT_BAL);
                        }
                        else
                        {
                            saldo.Add(row, 0);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CalcOboroti()
        {
            if (oboroti != null)
            {
                //Get info, Fill Collection
                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    var colsCarcas = CarcasForGlavBook.GetCarcas(GlavBookSt.Cols);
                    var rowsCarcas = CarcasForGlavBook.GetCarcas(GlavBookSt.Rows);
                    var DebitList = db.sp_rt_GlavnayaKniga_Debit(Year, Kvartal, DepartmentBase).ToList(); 

                    foreach(var col in colsCarcas)
                    {
                        var colDict = new Dictionary<string, decimal?>();

                        foreach (var row in rowsCarcas)
                        {
                            if (DebitList != null)
                            {
                                var debitRows = DebitList.Where(z => z.DebitAccount == row && z.JO == col);

                                if (debitRows.Any())
                                {
                                    var debitRow = debitRows.First();

                                    colDict.Add(row, debitRow.SUMMA);
                                }
                                else
                                {
                                    colDict.Add(row, 0);
                                }
                            }
                            else
                            {
                                colDict.Add(row, 0);
                            }
                        }

                        oboroti.Add(col, colDict);
                    }

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CalcZaKvartal()
        {
            if (zaKvartal != null)
            {
                //Get info, Fill Collection
                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    var RowLists = CarcasForGlavBook.GetCarcas(GlavBookSt.Rows);

                    foreach (var row in RowLists)
                    {
                        var dbRes = db.tbbu_balance.Where(z => z.DEPARTMENT_BASE == DepartmentBase
                                                         && z.CUR_YEAR == Year
                                                         && z.CUR_KV == Kvartal
                                                         && z.ACCOUNT_CODE == row);

                        if (dbRes.Any())
                        {
                            var resAccount = dbRes.First();

                            zaKvartal.Add(row, resAccount.DEBIT_BAL);
                        }
                        else
                        {
                            zaKvartal.Add(row, 0);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CalcZaVsego()
        {
            if (zaVsego != null)
            {
                //Get info, Fill Collection
                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    if ((saldo != null) && (zaKvartal != null))
                    {
                        var rowsCarcas = CarcasForGlavBook.GetCarcas(GlavBookSt.Rows);

                        foreach (var row in rowsCarcas)
                        {
                            decimal? saldoAmount;

                            if (!saldo.TryGetValue(row, out saldoAmount))
                            {
                                saldoAmount = 0;
                            }

                            decimal? zaKvartalAmount = 0;

                            if (!zaKvartal.TryGetValue(row, out zaKvartalAmount))
                            {
                                zaKvartalAmount = 0;
                            }

                            var Summa = saldoAmount + zaKvartalAmount;

                            zaVsego.Add(row, Summa);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void FillDebit()
        {
            CalcSaldo();
            CalcOboroti();
            CalcZaKvartal();
            CalcZaVsego();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GK_Credit
    {
        #region InputParametrs

        private string DepartmentBase { get; set; }

        private int Year { get; set; }

        private int Kvartal { get; set; }

        #endregion

        //Saldo
        public Dictionary<string, decimal?> saldo { get; set; }

        //Oboroti
        public Dictionary<string, Dictionary<string, decimal?>> oboroti { get; set; }

        //Summa Oborotov za kvartal
        public Dictionary<string, decimal?> zaKvartal { get; set; }

        //Summa Oborotov vsego
        public Dictionary<string, decimal?> zaVsego { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="depCode"></param>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        public GK_Credit(string depCode, int year, int kvartal)
        {
            saldo = new Dictionary<string, decimal?>();
            oboroti = new Dictionary<string, Dictionary<string, decimal?>>();
            zaKvartal = new Dictionary<string, decimal?>();
            zaVsego = new Dictionary<string, decimal?>();

            DepartmentBase = depCode;
            Year = year;
            Kvartal = kvartal;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CalcSaldo()
        {
            if (saldo != null)
            {
                //Get info, Fill Collection
                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    var RowLists = CarcasForGlavBook.GetCarcas(GlavBookSt.Rows);

                    int o_year, o_kv;

                    DateOperations.GetPrevQuarter(Year, Kvartal, out o_year, out o_kv);

                    foreach (var row in RowLists)
                    {
                        var dbRes = db.tbbu_balance.Where(z => z.DEPARTMENT_BASE == DepartmentBase
                                                         && z.CUR_YEAR == o_year
                                                         && z.CUR_KV == o_kv
                                                         && z.ACCOUNT_CODE == row);

                        if (dbRes.Any())
                        {
                            var resAccount = dbRes.First();

                            saldo.Add(row, resAccount.CREDIT_BAL );
                        }
                        else
                        {
                            saldo.Add(row, 0);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CalcOboroti()
        {
            if (oboroti != null)
            {
                //Get info, Fill Collection
                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    var colsCarcas = CarcasForGlavBook.GetCarcas(GlavBookSt.Cols);
                    var rowsCarcas = CarcasForGlavBook.GetCarcas(GlavBookSt.Rows);
                    var CreditList = db.sp_rt_GlavnayaKniga_Credit(Year, Kvartal, DepartmentBase).ToList(); 

                    foreach (var col in colsCarcas)
                    {
                        var colDict = new Dictionary<string, decimal?>();

                        foreach (var row in rowsCarcas)
                        {
                            if (CreditList != null)
                            {
                                var creditRows = CreditList.Where(z => z.CreditAccount == row && z.JO == col);

                                if (creditRows.Any())
                                {
                                    var creditRow = creditRows.First();

                                    colDict.Add(row, creditRow.SUMMA);
                                }
                                else
                                {
                                    colDict.Add(row, 0);
                                }
                            }
                            else
                            {
                                colDict.Add(row, 0);
                            }
                        }

                        oboroti.Add(col, colDict);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CalcZaKvartal()
        {
            if (zaKvartal != null)
            {
                //Get info, Fill Collection
                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    var RowLists = CarcasForGlavBook.GetCarcas(GlavBookSt.Rows);

                    foreach (var row in RowLists)
                    {
                        var dbRes = db.tbbu_balance.Where(z => z.DEPARTMENT_BASE == DepartmentBase
                                                         && z.CUR_YEAR == Year
                                                         && z.CUR_KV == Kvartal
                                                         && z.ACCOUNT_CODE == row);

                        if (dbRes.Any())
                        {
                            var resAccount = dbRes.First();

                            zaKvartal.Add(row, resAccount.CREDIT_BAL);
                        }
                        else
                        {
                            zaKvartal.Add(row, 0);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CalcZaVsego()
        {
            if (zaVsego != null)
            {
                //Get info, Fill Collection
                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    if ((saldo != null) && (zaKvartal != null))
                    {
                        var rowsCarcas = CarcasForGlavBook.GetCarcas(GlavBookSt.Rows);

                        foreach(var row in rowsCarcas)
                        {
                            decimal? saldoAmount;
                            
                            if (!saldo.TryGetValue(row, out saldoAmount))
                            {
                                saldoAmount = 0;
                            }

                            decimal? zaKvartalAmount = 0;

                            if (!zaKvartal.TryGetValue(row, out zaKvartalAmount))
                            {
                                zaKvartalAmount = 0;
                            }

                            var Summa = saldoAmount + zaKvartalAmount;

                            zaVsego.Add(row, Summa);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void FillCredit()
        {
            CalcSaldo();
            CalcOboroti();
            CalcZaKvartal();
            CalcZaVsego();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GK_Balance
    {
        #region InputParametrs

        private string DepartmentBase { get; set; }

        private int Year { get; set; }

        private int Kvartal { get; set; }

        #endregion
        //Aktiv
        public Dictionary<string, decimal?> aktiv { get; set; }

        //Passiv
        public Dictionary<string, decimal?> passiv { get; set; }

        public GK_Balance(string depCode, int year, int kvartal)
        {
            aktiv = new Dictionary<string, decimal?>();
            passiv = new Dictionary<string, decimal?>();

            DepartmentBase = depCode;
            Year = year;
            Kvartal = kvartal;
        }

        public void CalcBalance()
        {
            if (aktiv != null)
            {
                //Get info, Fill Collection
                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    var RowLists = CarcasForGlavBook.GetCarcas(GlavBookSt.Rows);

                    foreach (var row in RowLists)
                    {
                        var dbRes = db.tbbu_balance.Where(z => z.DEPARTMENT_BASE == DepartmentBase
                                                         && z.CUR_YEAR == Year
                                                         && z.CUR_KV == Kvartal
                                                         && z.ACCOUNT_CODE == row);

                        if (dbRes.Any())
                        {
                            var resAccount = dbRes.First();

                            aktiv.Add(row, resAccount.DEBIT_BAL);
                        }
                        else
                        {
                            aktiv.Add(row, 0);
                        }
                    }
                }
            }


            if (passiv != null)
            {
                //Get info, Fill Collection
                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    var RowLists = CarcasForGlavBook.GetCarcas(GlavBookSt.Rows);

                    foreach (var row in RowLists)
                    {
                        var dbRes = db.tbbu_balance.Where(z => z.DEPARTMENT_BASE == DepartmentBase
                                                         && z.CUR_YEAR == Year
                                                         && z.CUR_KV == Kvartal
                                                         && z.ACCOUNT_CODE == row);

                        if (dbRes.Any())
                        {
                            var resAccount = dbRes.First();

                            passiv.Add(row, resAccount.CREDIT_BAL);
                        }
                        else
                        {
                            passiv.Add(row, 0);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GK_Structure
    {
        #region InputParametrs

         public string DepartmentBase { get; set; }

         public int Year { get; set; }

         public int Kvartal { get; set; }

        #endregion

        public GK_Debit debitSide { get; set; }

        public GK_Credit creditSide { get; set; }

        public GK_Balance balanceSide { get; set; }

        public GK_Structure(string _departmentBase, int _year, int _kvartal)
        {
            DepartmentBase = _departmentBase;
            Year = _year;
            Kvartal = _kvartal;

            debitSide = new GK_Debit(DepartmentBase, Year, Kvartal);
            creditSide = new GK_Credit(DepartmentBase, Year, Kvartal);
            balanceSide = new GK_Balance(DepartmentBase, Year, Kvartal);
        }

        public GlavnayaKnigaOut MakeGlavKniga()
        {
            debitSide.FillDebit();
            creditSide.FillCredit();
            balanceSide.CalcBalance();

            var retObj = new GlavnayaKnigaOut();

            retObj.Dsaldo = debitSide.saldo;
            retObj.Doboroti = debitSide.oboroti;
            retObj.DzaKvartal = debitSide.zaKvartal;
            retObj.DzaVsego = debitSide.zaVsego;

            retObj.Csaldo = creditSide.saldo;
            retObj.Coboroti = creditSide.oboroti;
            retObj.CzaKvartal = creditSide.zaKvartal;
            retObj.CzaVsego = creditSide.zaVsego;

            retObj.aktiv = balanceSide.aktiv;
            retObj.passiv = balanceSide.passiv;

            return retObj;
        }

    }
}