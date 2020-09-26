using System;

namespace KasabaKIAT.BusinessLogic._4_Etap.Zayavki
{
    public class PodachaZayavokModel
    {
        public long rec_id { get; set; }
        public DateTime? zaezd_begin { get; set; }
        public DateTime? zaezd_end { get; set; }
        public string name_san { get; set; }
        public int sanid { get; set; }
        public string position { get; set; }
        public string full_name { get; set; }
        public string status { get; set; }
        public string inn { get; set; }
        public string organization { get; set; }
        public int? quantity { get; set; }
        public DateTime? timestamp { get; set; }
        public bool? medical_order_exists { get; set; }
        public string medical_order { get; set; }
        public string sotr_inn { get; set; }
        public string pnfl { get; set; }
    }
}