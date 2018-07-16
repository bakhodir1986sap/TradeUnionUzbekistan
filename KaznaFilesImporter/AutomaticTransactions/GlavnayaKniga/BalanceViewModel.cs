using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.GlavnayaKniga
{
    public class BalanceViewModel
    {
      public long       rec_id          { get; set;}
      public string     department_base { get; set;}
      public int        cur_year        { get; set;}
      public int        cur_kv          { get; set;}
      public long       user_id         { get; set;}
      public string     account_code    { get; set;}
      public decimal    debit_bal       { get; set;}
      public decimal    credit_bal      { get; set; }
      public int        debit_count     { get; set; }
      public int        credit_count    { get; set; }
      public decimal    debit_trans     { get; set; }
      public decimal    credit_trans    { get; set; }
    }
}