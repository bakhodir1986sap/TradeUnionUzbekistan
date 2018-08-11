using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._3_Etap.Statistika.Stat_ViewModels
{
    public class Stat_PPOViewModel : IBaseStatViewModel
    {
        //Charts
        //2 Chart Zayavki
        public int? ApprovedZayavka { get; set; }
        public int? RejectedZayavka { get; set; }
        public int? InProgressZayavka { get; set; }
    }
}