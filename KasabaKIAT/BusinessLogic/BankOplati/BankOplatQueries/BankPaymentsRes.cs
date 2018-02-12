using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.BankOplati.BankOplatQueries
{
    public class BankPaymentsResQ
    {
        public int? Kvartal { get; set; }

        public decimal? Summa { get; set; }
    }

    public class BankPaymentsResM
    {
        public int? Month { get; set; }

        public decimal? Summa { get; set; }
    }
}