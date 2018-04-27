using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Buxgalteriya
{
    public class PlanRegionViewModel
    {
      public long       rec_id             {get; set;}
      public string     department_base    {get; set;}
      public long?      userid             {get; set;}
      public int?        year               {get; set;}
      public string     soato_code         {get; set;}
      public string     item_code          {get; set;}
      public string     item_name          {get; set;}
      public decimal?   godovoy            {get; set;}
      public bool       IsModValueYearly   { get; set; }
      public decimal?   quarter1           {get; set;}
      public bool       IsModValue1Quater  { get; set; }
      public decimal?   halfyear           {get; set;}
      public bool       IsModValue1Half    { get; set; }
      public decimal?   month9             {get; set;}
      public bool       IsModValue9Month   { get; set; }
    }
}