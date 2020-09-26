using System;

namespace KasabaKIAT.BusinessLogic._3_Etap.Statistika.Stat_ViewModels
{
    public class Stat_PPOViewModel : IBaseStatViewModel
    {
        public string DepartmentBase { get; set; }
        public int Year { get; set; }
        public int Kvartal { get; set; }
        public DateTime LastChangeDate { get; set; }

        //Charts
        //2 Chart Zayavki
        public int? ApprovedZayavka { get; set; }
        public int? RejectedZayavka { get; set; }
        public int? InProgressZayavka { get; set; }
        public int? AktivUsers { get; set; }
    }
}