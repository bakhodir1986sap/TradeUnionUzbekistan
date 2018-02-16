using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Utils.References
{
    public class Sprav_SanViewModel
    {
      public int ID         {get; set;}
      public string Name_San   {get; set;}
      public string Num_Kod    {get; set;}
      public long?   UserId     {get; set;}

      public int KurortSanId { get; set; }
    }
}