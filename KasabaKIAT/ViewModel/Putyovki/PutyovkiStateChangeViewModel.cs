using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Putyovki
{
    public class PutyovkiStateChangeViewModel
    {
        public long rec_id { get; set; }
        public int id_sanat { get; set; }
        public string num_put { get; set; }
        public DateTime change_dt { get; set; }
        public int state { get; set; }
        public string department_base { get; set; }
        public long user_id { get; set; }
    }
}