using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurortUrvServiceTest.ServiceInterAction.ViewModel
{
    public class SpisokRaspredeleniyaViewModel
    {
        public long     ID { get; set; }
        public int?     GodZaezdov              { get; set; }
        public string   SanatoryKod             { get; set; }
        public string   ProfsoyzKod             { get; set; }
        public string   SanatoryRazmeshenieKod  { get; set; }
        public int?     Putevok1kv              { get; set; }
        public int?     Putevok2kv              { get; set; }
        public int?     Putevok3kv              { get; set; }
        public int?     Putevok4kv              { get; set; }
    }
}
