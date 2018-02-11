using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurortUrvServiceTest.ServiceInterAction.ViewModel
{
    public class ZaezdyClientovViewModel
    {
        public long ID { get; set; }
        public string       SanatoryKod             { get; set; }
        public string       SanatoryNaimenovanie    { get; set; }
        public string       NomerPutevki            { get; set; }
        public DateTime?    DataZaezdaPlan          { get; set; }
        public DateTime?    DataZaezdaFakt          { get; set; }
        public decimal?     StoimostPutevki         { get; set; }
        public decimal?     OplachenaStoimost       { get; set; }
        public string       ClientFIO               { get; set; }
        public string       ClientPasport           { get; set; }
    }
}
