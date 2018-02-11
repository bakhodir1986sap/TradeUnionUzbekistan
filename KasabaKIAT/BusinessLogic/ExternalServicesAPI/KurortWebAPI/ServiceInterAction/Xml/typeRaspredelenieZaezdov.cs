using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KurortUrvServiceTest.ServiceInterAction.Xml
{
    [DataContract]
    public class typeRaspredelenieZaezdov
    {
        public typeRaspredelenieZaezdov()
        {
            StrokiRaspredelenyaZaezdov = new List<StrokiRaspredelenyaZaezdov>();
        }

        [DataMember]
        public List<StrokiRaspredelenyaZaezdov> StrokiRaspredelenyaZaezdov { get; set; }

        [DataMember]
        public string GodZaezdov { get; set; }
    }
}
