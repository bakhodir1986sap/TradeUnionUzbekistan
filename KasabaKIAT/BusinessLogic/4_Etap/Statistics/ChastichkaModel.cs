using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class ChastichkaModel
    {
        public string year { get; set; }

        public string quarter { get; set; }

        public string  ProfsoyzNaimenovanie { get; set; }

        public string match_departmentbase { get; set; }

        public decimal? amount { get; set; }
    }
}