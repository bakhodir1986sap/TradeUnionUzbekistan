using System;

namespace KasabaKIAT.BusinessLogic.Putyovki.PutyovkiHistory
{
    public enum HistEventType
    {
        Prixod,
        Rasxod,
        Oplata,
        Vozvrat,
        UstanovVlad,
        IzmPutDate,
        BronPutyovka,
        SnyatBronPutyovka,
        UdalitPutIzPrixod,
        UdalitPutIzRasxod,
        UdalitPutIzVozvrat
    }


    public class PutHistViewModel
    {
        public long         id               { get; set;} 
        public int          event_tp         { get; set;}
        public string       event_name       { get; set; }
        public string       num_put          { get; set;}
        public int?         id_sanat         { get; set;}
        public string       sanatName        { get; set; }
        public string       remark           { get; set;}
        public long?        userid           { get; set;}
        public string       departmentbase   { get; set; }
        public DateTime?    timestamp_t      { get; set; }
        public string       timeString          { get; set; }
    }
}