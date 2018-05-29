using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel
{
    public class ZayavkiViewModel
    {
        public long rec_id { get; set; }

        public long groupId { get; set; }

        public DateTime? ZaezdBegin { get; set; }

        public DateTime? ZaezdEnd { get; set; }

        public long? SanatoryId { get; set; }

        public string SanatoryName { get; set; }

        public long? CampId { get; set; }

        public string CampName { get; set; }

        public string departmentCode { get; set; }

        public string OrdanizationName { get; set; }

        public string Inn { get; set; }

        public string FullName { get; set; }

        public string Position { get; set; }

        public int Quantity { get; set; }

        public int OrderQuantity { get; set; }

        public DateTime? TimeStamp { get; set; }

        public bool MedicalOrderExists { get; set; }

        public string MedicalOrder { get; set; }

        public string Kuk_qarori { get; set; }

        public long userid { get; set; }
    }

    public class ZayavkiModel
    {
        public long         ID       {get; set;}
        public DateTime?    ZB       {get; set;}
        public DateTime?    ZE       {get; set;}
        public string       SC       {get; set;}
        public long?         SC_id    {get; set;}
        public string       P        {get; set;}
        public string       FN       {get; set;}
        public string       S        {get; set;}
        public string       INN      {get; set;}
        public string       O        {get; set;}
        public int?         Q        {get; set;}
        public DateTime?    TS       {get; set;}
        public bool?        MOE      {get; set;}
        public string       MO       {get; set;}
        public bool?        isOwn    { get; set; }
    }
}