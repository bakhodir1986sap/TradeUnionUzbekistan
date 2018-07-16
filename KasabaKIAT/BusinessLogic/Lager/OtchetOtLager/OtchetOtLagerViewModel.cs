using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Lager.OtchetOtLager
{
    public class OtchetOtLagerViewModel
    {
      public long       rec_id              {get; set;}
      public long       id_org              {get; set;}
      public int        quantuty            {get; set;}
      public decimal    totalamount         {get; set;}
      public DateTime   timestamp           {get; set;}
      public int        status              {get; set;}
      public string     department_code     {get; set;}
      public long       userid              { get; set; }
    }
}