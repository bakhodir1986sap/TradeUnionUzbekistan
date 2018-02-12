using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Utils.References
{
    public class Sprav_MFOViewModel
    {
      public long ID         { get; set;}

      public string Code_MFO   { get; set;}

      public string Name_MFO   { get; set;}

      public string Adres_MFO  { get; set;}

      public string Data_Act   { get; set;}

      public string Data_Cls   { get; set;}

      public long? UserId     { get; set;}
    }
}