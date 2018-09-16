using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._3_Etap.Statistika.Stat_ViewModels
{
    public class UrgentPutyovkaDetail
    {
        public string NumPut { get; set; }
        public string SanatoryName { get; set; }
        public string DataSan { get; set; } //dd.MM.yyyy
    }

    public class Stat_OblSovProfViewModel : IBaseStatViewModel
    {
        public string DepartmentBase { get; set; }
        public int Year { get; set; }
        public int Kvartal { get; set; }
        public DateTime LastChangeDate { get; set; }
        public string LastChangeDateStr
        {
            get
            {
                return LastChangeDate.ToLongDateString();
            }
        }

        #region Data

        public int BronCount { get; set; }
        //Doxodi
        public decimal? TotalIncome { get; set; }

        public decimal? EcpIncome { get; set; }

        public decimal? PartialIncome { get; set; }

        public decimal? OtherIncome { get; set; }

        public List<IncomeRegion> IncomePerRegionList { get; set; }

        //UrgentPutyovki

        public int? UrgentPutyovkas { get; set; }

        public List<UrgentPutyovkaDetail> UrgentPutyovkasDtlList { get; set; }

        //Putyovkas
        //Plan
        public int? PlanPutyovkaCount { get; set; }
        public decimal? PlanPutyovkaAmount { get; set; }

        //Realizovanno
        public int? RealizPutyovkaCount { get; set; }
        public decimal? RealizPutyovkaAmount { get; set; }

        //Ostatok
        public int? OstatokPutyovkaCount { get; set; }
        public decimal? OstatokPutyovkaAmount { get; set; }

        //Realizasiya
        //45%
        public int? S45PutyovkaCount { get; set; }
        public decimal? S45PutyovkaAmount { get; set; }

        //100%
        public int? S100PutyovkaCount { get; set; }
        public decimal? S100PutyovkaAmount { get; set; }

        //Besplatno
        public int? S0PutyovkaCount { get; set; }
        public decimal? S0PutyovkaAmount { get; set; }

        //Zadoljennost
        public decimal? TotalZadoljennostAmount { get; set; }

        public List<ZadoljennostRegion> ZadoljennostList { get; set; }

        //Charts
        //1 Chart Organizations
        public int? ProfComExists { get; set; }
        public int? ProfComNotExists { get; set; }

        //2 Chart Zayavki
        public int? ApprovedZayavka { get; set; }
        public int? RejectedZayavka { get; set; }
        public int? InProgressZayavka { get; set; }

        //3 Chart Putyovki
        public int? OstatokPutyovkas { get; set; }
        public int? RealizedPutyovkas { get; set; }
        public int? VozvratPutyovkas { get; set; }
        public int? AktivUsers { get; set; }

        #endregion
    }
}