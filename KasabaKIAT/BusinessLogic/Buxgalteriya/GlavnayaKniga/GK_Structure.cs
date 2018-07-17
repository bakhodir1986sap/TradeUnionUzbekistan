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

        public List<GlavnayaKnigaTable> MakeGlavKniga()
        {
            debitSide.FillDebit();
            creditSide.FillCredit();
            balanceSide.CalcBalance();

            var retObj = new GlavnayaKnigaOut();
            var retList = new List<GlavnayaKnigaTable>();

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

            var rowsCarcas = CarcasForGlavBook.GetCarcas(GlavBookSt.Rows);

            foreach(var row in rowsCarcas)
            {
                var tableRow = new GlavnayaKnigaTable();
                tableRow.field0010 = row;

                tableRow.field0011 = debitSide.saldo[row];
                tableRow.field0012=  (debitSide.oboroti["ЖО 1"])[row];
                tableRow.field0013=  (debitSide.oboroti["ЖО 2"])[row];
                tableRow.field0014=  (debitSide.oboroti["ЖО 3"])[row];
                tableRow.field0015=  (debitSide.oboroti["ЖО 4"])[row];
                tableRow.field0016=  0;//4a
                tableRow.field0017=  (debitSide.oboroti["ЖО 5"])[row];
                tableRow.field0018=  0; //5a
                tableRow.field0019=  (debitSide.oboroti["ЖО 6"])[row];
                tableRow.field0020=  (debitSide.oboroti["ЖО 7"])[row];
                tableRow.field0021=  (debitSide.oboroti["ЖО 11"])[row];
                tableRow.field0022 = debitSide.zaKvartal[row];
                tableRow.field0023 = debitSide.zaVsego[row];

                tableRow.field0024 = row;

                tableRow.field0025 = creditSide.saldo[row];
                tableRow.field0026 = (creditSide.oboroti["ЖО 1"])[row];
                tableRow.field0027 = (creditSide.oboroti["ЖО 2"])[row];
                tableRow.field0028 = (creditSide.oboroti["ЖО 3"])[row];
                tableRow.field0029 = (creditSide.oboroti["ЖО 4"])[row];
                tableRow.field0030 = 0;//4a
                tableRow.field0031 = (creditSide.oboroti["ЖО 5"])[row];
                tableRow.field0032 = 0; //5a
                tableRow.field0033 = (creditSide.oboroti["ЖО 6"])[row];
                tableRow.field0034 = (creditSide.oboroti["ЖО 7"])[row];
                tableRow.field0035 = (creditSide.oboroti["ЖО 11"])[row];
                tableRow.field0036 = creditSide.zaKvartal[row];
                tableRow.field0037 = creditSide.zaVsego[row];

                tableRow.field0038 = balanceSide.aktiv[row];
                tableRow.field0039 = balanceSide.passiv[row];

                tableRow.field0040 = row;

                retList.Add(tableRow);
            }

            return retList;
        }

    }

    public class GlavnayaKnigaTable
    {

        public string   field0010 { get; set; }
        public decimal? field0011 { get; set; }
        public decimal? field0012 { get; set; }
        public decimal? field0013 { get; set; }
        public decimal? field0014 { get; set; }
        public decimal? field0015 { get; set; }
        public decimal? field0016 { get; set; }
        public decimal? field0017 { get; set; }
        public decimal? field0018 { get; set; }
        public decimal? field0019 { get; set; }
        public decimal? field0020 { get; set; }
        public decimal? field0021 { get; set; }
        public decimal? field0022 { get; set; }
        public decimal? field0023 { get; set; }
        public string   field0024 { get; set; }
        public decimal? field0025 { get; set; }
        public decimal? field0026 { get; set; }
        public decimal? field0027 { get; set; }
        public decimal? field0028 { get; set; }
        public decimal? field0029 { get; set; }
        public decimal? field0030 { get; set; }
        public decimal? field0031 { get; set; }
        public decimal? field0032 { get; set; }
        public decimal? field0033 { get; set; }
        public decimal? field0034 { get; set; }
        public decimal? field0035 { get; set; }
        public decimal? field0036 { get; set; }
        public decimal? field0037 { get; set; }
        public decimal? field0038 { get; set; }
        public decimal? field0039 { get; set; }
        public string   field0040 { get; set; }
        /*
                    <th style="text-align:center">№ счетов Дебет</th>
                    <th style="text-align:center">Сальдо_на_начало</th>
                    <th style="text-align:center">Ж_О_№1</th>
                    <th style="text-align:center">Ж_О_№2</th>
                    <th style="text-align:center">Ж_О_№3</th>
                    <th style="text-align:center">Ж_О_№4</th>
                    <th style="text-align:center">Ж_О_№4а</th>
                    <th style="text-align:center">Ж_О_№5</th>
                    <th style="text-align:center">Ж_О_№5а</th>
                    <th style="text-align:center">Ж_О_№6</th>
                    <th style="text-align:center">Ж_О_№7</th>
                    <th style="text-align:center">Ж_О_№11</th>
                    <th style="text-align:center">Ж_О_№11</th>
                    <th style="text-align:center">Сумма_за_квартал</th>
                    <th style="text-align:center">Сумма_всего</th>
                    <th style="text-align:center">№ счетов Кредит</th>
                    <th style="text-align:center">Сальдо_на_начало</th>
                    <th style="text-align:center">Ж_О_№1</th>
                    <th style="text-align:center">Ж_О_№2</th>
                    <th style="text-align:center">Ж_О_№3</th>
                    <th style="text-align:center">Ж_О_№4</th>
                    <th style="text-align:center">Ж_О_№4а</th>
                    <th style="text-align:center">Ж_О_№5</th>
                    <th style="text-align:center">Ж_О_№5а</th>
                    <th style="text-align:center">Ж_О_№6</th>
                    <th style="text-align:center">Ж_О_№7</th>
                    <th style="text-align:center">Ж_О_№11</th>
                    <th style="text-align:center">Ж_О_№11</th>
                    <th style="text-align:center">Сумма_за_квартал</th>
                    <th style="text-align:center">Сумма_всего</th>
                    <th style="text-align:center">Актив</th>
                    <th style="text-align:center">Пассив</th>
                    <th style="text-align:center">№ счетов</th>
         */
    }
}