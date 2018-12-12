using KasabaKIAT.BusinessLogic._3_Etap.Statistika.Stat_ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class FourStapViewModel : IBaseStatViewModel
    {
        public List<KaznaInfoModel> kaznaInfo { get; set; }

        public List<PutyovkiModel> putyovkaInfo { get; set; }

        public List<RealizModel> realizInfo { get; set; }

        public List<UrgentPutyovkas> urgentInfo { get; set; }

        public List<ZadoljennostModel> zadoljInfo { get; set; }

        public List<BronModel> bronInfo { get; set; }

        public List<ActiveUsersModel> activeUsersInfo { get; set; }

        public List<PieChartData> organizationPieInfo { get; set; }

        public List<PieChartData> zayavkiPieInfo { get; set; }

        public List<PieChartData> putyovkiPieInfo { get; set; }

        public List<ChastichkaModel> chastichkaInfo { get; set; }

        public List<KaznaInfoModel> kaznaLastDayInfo { get; set; }

        public List<PererasxodPoPutevkamModel> pererasxodInfo { get; set; }


        public string DepartmentBase { get; set; }

        public int Year { get; set; }
        public int Kvartal { get; set; }
        public DateTime LastChangeDate { get; set; }
        public int? AktivUsers { get; set; }
    }
}