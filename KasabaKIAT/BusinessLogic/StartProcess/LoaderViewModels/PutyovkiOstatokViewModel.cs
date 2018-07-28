using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.StartProcess.LoaderViewModels
{
    public class PutyovkiOstatokViewModel
    {
        public int      SanatoryCode    { get; set; }
        public string   NumPut          { get; set; }
        public DateTime DataSan         { get; set; }
        public decimal  Summa           { get; set; }
        //public string   DepartmentBase  { get; set; }
        //public long     UserId          { get; set; }
    }
}