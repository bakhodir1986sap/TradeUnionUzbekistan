using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel
{
    public class AvaragePriceViewModel
    {
        public long RecId { get; set; }

        public DateTime? SetDate { get; set; }

        public decimal? Amount { get; set; }

        public long? UserId { get; set; }
    }
}