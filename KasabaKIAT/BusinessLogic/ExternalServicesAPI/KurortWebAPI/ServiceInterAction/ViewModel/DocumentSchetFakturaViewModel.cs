using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurortUrvServiceTest.ServiceInterAction.ViewModel
{
    public class DocumentSchetFakturaViewModel
    {
        public long ID { get; set; }
        public string       DocNomer                { get; set; }
        public DateTime?    DocData                 { get; set; }
        public string       DocTypeRealVozv         { get; set; }
        public string       ProfsoyzKod             { get; set; }
        public string       ProfsoyzDogovor         { get; set; }
        public DateTime?    DataKvartalaPutevok     { get; set; }
        public string       ProfsoyzDoverennost     { get; set; }
        public DateTime?    ProfsoyzDoverennostData { get; set; }
        public string       ProfsoyzDoverennostMOL  { get; set; }
        public decimal?     SummaDoc                { get; set; }
        public decimal?     ProcentNacenki          { get; set; }
        public decimal?     SummaNacenki            { get; set; }
        public int?         KolichestvoPutevok      { get; set; }
        public string       TovarOtpustil           { get; set; }
    }
}
