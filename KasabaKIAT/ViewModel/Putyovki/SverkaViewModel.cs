using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel.Putyovki
{
    public class SverkaViewModel
    {
        public long rec_id { get; set; }
        public long? user_id { get; set; }
        public string serial_number { get; set; }
        public DateTime? rest_order_begin_dt { get; set; }
        public DateTime? rest_order_end_dt { get; set; }
        public DateTime? fact_rest_begin_dt { get; set; }
        public DateTime? fact_rest_end_dt { get; set; }
        public string full_name { get; set; }
        public string passport_number { get; set; }
        public string birth_year { get; set; }
        public string residence_place { get; set; }
        public string work_palce { get; set; }
        public string yullanma_bergan { get; set; }
        public string ijtimoiy_tarkib { get; set; }
        public string days_by_putyovka { get; set; }
        public string days_late { get; set; }
        public string days_early { get; set; }
        public string days_prolong { get; set; }
        public string days_moved { get; set; }
        public string days_all { get; set; }
        public string putyovka_price { get; set; }
        public string additional_payments { get; set; }
        public string building { get; set; }
        public string flat { get; set; }
        public string treating_doctor { get; set; }
        public string talon_back { get; set; }
        public string department_code { get; set; }
    }
}