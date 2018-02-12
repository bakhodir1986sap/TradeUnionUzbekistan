using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel.Buxgalteriya
{
    public class PlanSchetovViewModel
    {
      public long            rec_id         {get; set;}
      public string          account_name   {get; set;}
      public string          account_code   {get; set;}
      public string          account_type   {get; set;}
      public string          status         {get; set;}
      public long?           userud         {get; set;}
      public string          departmentbase {get; set;}
    }
}