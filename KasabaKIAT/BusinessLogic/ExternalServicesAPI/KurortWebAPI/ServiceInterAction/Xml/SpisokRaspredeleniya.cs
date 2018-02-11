using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KurortUrvServiceTest.ServiceInterAction.Xml
{
    [DataContract]
    public class StrokiRaspredelenyaZaezdov
    {
        [DataMember]
        public string SanatoryKod { get; set; }

        [DataMember]
        public string ProfsoyzKod { get; set; }

        [DataMember]
        public string SanatoryRazmeshenieKod { get; set; }

        [DataMember]
        public int Putevok1kv { get; set; }

        [DataMember]
        public int Putevok2kv { get; set; }

        [DataMember]
        public int Putevok3kv { get; set; }

        [DataMember]
        public int Putevok4kv { get; set; }
    }
}
