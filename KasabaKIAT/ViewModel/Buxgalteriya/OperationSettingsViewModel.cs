using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Buxgalteriya
{
    public class OperationSettingsViewModel
    {
      public long       rec_id             {get; set;}
      public string     oper_name          {get; set;}
      public string     oper_type          {get; set;}
      public string     debit_account      {get; set;}
      public string     credit_account     {get; set;}
      public string     status             {get; set;}
      public string     department_code    {get; set;}
      public long?       user_id            {get; set;}
    }
}