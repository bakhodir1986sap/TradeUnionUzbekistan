using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Utils.References
{
    public class Sprav_OrgViewModel
    {
      public long ID             { get; set; }

      public int?    ID_Ot          { get; set; }

        public string OtraslName { get; set; }

        public string Name_Org       { get; set; }

      public string INN            { get; set; }

      public string RS             { get; set; }

      public string MFO            { get; set; }

      public string Bank           { get; set; }

      public string OKONX          { get; set; }

      public string Adress         { get; set; }

      public string Telefon        { get; set; }

      public int?    Region         { get; set; }

        public string  RegionName { get; set; }

        public int?    DT_OST         { get; set; }

      public int?    KT_OST         { get; set; }

      public string E_Mail         { get; set; }

      public long?   UserId         { get; set; }

        public string DepartmentBase { get; set; }
    }
}