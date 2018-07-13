using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.ClosePeriod.CloseKvartal
{
    public class CloseKvartalHistoryViewModel
    {
      public long       rec_id          {get; set;}
      public string     department_code {get; set;}
      public int?        closed_kvyear   {get; set;}
      public int?        closed_kv       {get; set;}
      public DateTime?   timestamp       {get; set;}
      public long?      user_id         {get; set;}
      public string     remark          { get; set; }
    }
}