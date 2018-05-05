using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Utils.References
{
    public class tbcs_dictTypeViewModel
    {
      public long rec_id      {get; set;}
      public string code        {get; set;}
      public string name_uz     {get; set;}
      public string name_ru     {get; set;}
      public string name_lt     {get; set;}
      public long? user_id     { get; set; }
    }
}