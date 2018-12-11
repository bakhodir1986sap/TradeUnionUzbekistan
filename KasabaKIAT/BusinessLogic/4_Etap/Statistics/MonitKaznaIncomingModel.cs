using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class MonitKaznaIncomingModel
    {
        public string OblSovProf { get; set; }

        public int Year { get; set; }

        public decimal? FederationAmount { get; set; }

        public decimal? OblSovProvAmount { get; set; }

        public DateTime? operdate { get; set; }

    }
}