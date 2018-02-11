using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurortUrvServiceTest.ServiceInterAction.ViewModel
{
    public class GrafikZaezdovSanatorievViewModel
    {
        public long ID { get; set; }
        public string   SanatoryKod                       { get; set; }
        public string   SanatoryNaimenovanie              { get; set; }
        public string   SanatoryProfil                    { get; set; }
        public string   SanatoryKodOblasti                { get; set; }
        public string   SanatoryOblast                    { get; set; }
        public int?     SanatoryKoekVsego                   { get; set; }
        public int?     SanatoryKoekLetom                   { get; set; }
        public int?     SanatoryKoekZimoy                   { get; set; }
        public string   SanatoryRazmeshenieKod            { get; set; }
        public string   SanatoryRazmeshenieNaimenovanie   { get; set; }
        public int?     Putevok1kv                          { get; set; }
        public int?     Putevok2kv                          { get; set; }
        public int?     Putevok3kv                          { get; set; }
        public int?     Putevok4kv                          { get; set; }
        public int      GodZaezdov                           { get; set; }
    }
}
