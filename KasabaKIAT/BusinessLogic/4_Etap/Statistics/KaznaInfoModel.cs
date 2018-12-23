using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class KaznaInfoModel
    {
        public string OblSovProf { get; set; }

        public string Tuman { get; set; }

        public string Tarmoq { get; set; }

        public string Year { get; set; }

        public string Quarter { get; set; }

        public decimal? TotalByRow { get; set; }

        public decimal? FederationAmount { get; set; }

        public decimal? OblSovProvAmount { get; set; }

        public DateTime? operdate { get; set; }

        public string inn { get; set; }
    }
}