using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.PrixodPutyovok
{
    public class PrixodDetailViewModel
    {
        public int      Year            { get; set; }

        public int      Kvartal         { get; set; }

        public int      SanatId         { get; set; }

        public string   SanatName       { get; set; }

        public int      beginNumber     { get; set; }

        public int      endNumber       { get; set; }

        public DateTime SanatDate       { get; set; }

        public decimal  Price           { get; set; }

        public string   DepartmentBase  { get; set; }

        public long?    UserId          { get; set; }

        public long?    IdPost          { get; set; }
    }
}