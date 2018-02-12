using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel
{
    public enum OrganizationType
    {
        Private,
        Govern,
        Inertnational
    }

    public class BankOplatViewModel
    {
        public long id { get; set; }

        public DateTime? bankDay { get; set; }

        public string Inn { get; set; }

        public string Nibdd { get; set; }

        public string FirmName { get; set; }

        public byte? Otrasl { get; set; }

        public byte? Region { get; set; }

        public string Okonx { get; set; }

        public string Soato { get; set; }

        public OrganizationType Vid { get; set; }

        public string RaschetniySchet { get; set; }
    }
}