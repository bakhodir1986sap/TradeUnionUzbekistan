using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurortUrvServiceTest.ServiceInterAction.ViewModel
{
    public class StrokaSchetFakturiViewModel
    {
        public long ID { get; set; }
        public long?        SchetFactura_ID                 { get; set; }
        public string       SanatoryKod                     { get; set; }
        public string       SanatoryNaimenovanie            { get; set; }
        public string       SanatoryRazmeshenieKod          { get; set; }
        public string       SanatoryRazmeshenieNaimenovanie { get; set; }
        public string       SanatoryType                    { get; set; }
        public DateTime?    DataZaezda                      { get; set; }
        public string       Blank1                          { get; set; }
        public string       Blank2                          { get; set; }
        public int?         Kolichestvo                     { get; set; }
        public decimal?     Sebestoimost                    { get; set; }
        public decimal?     SummaSebestoimosti              { get; set; }
        public decimal?     ProcentNacenki                  { get; set; }
        public decimal?     Cena                            { get; set; }
        public decimal?     Summa                           { get; set; }
    }
}
