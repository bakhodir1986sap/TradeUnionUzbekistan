using System;

namespace KasabaKIAT.BusinessLogic.StartProcess.LoaderViewModels
{
    public class PutyovkiOstatokViewModel
    {
        public int      SanatoryCode    { get; set; }
        public string   NumPut          { get; set; }
        public DateTime DataSan         { get; set; }
        public decimal  Summa           { get; set; }
        public string   Inn             { get; set; }
    }
}