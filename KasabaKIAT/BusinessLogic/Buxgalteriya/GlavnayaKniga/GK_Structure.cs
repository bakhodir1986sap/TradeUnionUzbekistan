using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.GlavnayaKniga
{

    /// <summary>
    /// 
    /// </summary>
    public class GK_Debit
    {
        
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
        public GK_Debit()
        {
            saldo = new Dictionary<string, decimal?>();
            oboroti = new Dictionary<string, Dictionary<string, decimal?>>();
            zaKvartal = new Dictionary<string, decimal?>();
            zaVsego = new Dictionary<string, decimal?>();
        }

        public void CalcSaldo()
        {
            if (saldo != null)
            {
                //Get info, Fill Collection
            }
        }

        public void CalcOboroti()
        {
            if (zaKvartal != null)
            {
                //Get info, Fill Collection
            }
        }


        public void CalcZaKvartal()
        {
            if (oboroti != null)
            {
                //Get info, Fill Collection
            }
        }

        public void CalcZaVsego()
        {
            if (zaVsego != null)
            {
                //Get info, Fill Collection
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GK_Credit
    {
        //Saldo
        public Dictionary<string, decimal?> saldo { get; set; }

        //Oboroti
        public Dictionary<string, Dictionary<string, decimal?>> oboroti { get; set; }

        //Summa Oborotov za kvartal
        public Dictionary<string, decimal?> zaKvartal { get; set; }

        //Summa Oborotov vsego
        public Dictionary<string, decimal?> zaVsego { get; set; }

        public GK_Credit()
        {
            saldo = new Dictionary<string, decimal?>();
            oboroti = new Dictionary<string, Dictionary<string, decimal?>>();
            zaKvartal = new Dictionary<string, decimal?>();
            zaVsego = new Dictionary<string, decimal?>();
        }

        public void CalcSaldo()
        {
            if (saldo != null)
            {
                //Get info, Fill Collection
            }
        }

        public void CalcOboroti()
        {
            if (zaKvartal != null)
            {
                //Get info, Fill Collection
            }
        }


        public void CalcZaKvartal()
        {
            if (oboroti != null)
            {
                //Get info, Fill Collection
            }
        }

        public void CalcZaVsego()
        {
            if (zaVsego != null)
            {
                //Get info, Fill Collection
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GK_Balance
    {
        //Aktiv
        public Dictionary<string, decimal?> aktiv { get; set; }

        //Passiv
        public Dictionary<string, decimal?> passiv { get; set; }

        public GK_Balance()
        {
            aktiv = new Dictionary<string, decimal?>();
            passiv = new Dictionary<string, decimal?>();
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

        public GK_Structure()
        {
            debitSide = new GK_Debit();
            creditSide = new GK_Credit();
            balanceSide = new GK_Balance();
        }

        public GK_Structure(string _departmentBase, int _year, int _kvartal)
        {
            DepartmentBase = _departmentBase;
            Year = _year;
            Kvartal = _kvartal;

            debitSide = new GK_Debit();
            creditSide = new GK_Credit();
            balanceSide = new GK_Balance();
        }



    }
}