using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class UrgentPutyovkas
    {
        public int?          Year_Op                 { get; set; }

        public int?          Quarter                 { get; set; }

        public string       ProfsoyzNaimenovanie    { get; set; }

        public string       Num_Put                 { get; set; }

        public string       Name_San                { get; set; }

        public DateTime?    DataSan                 { get; set; }

        public int          CounterU                { get; set; }

        public string       ComplexString { get; set; }
    }
}