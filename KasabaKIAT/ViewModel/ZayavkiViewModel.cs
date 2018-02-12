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


}