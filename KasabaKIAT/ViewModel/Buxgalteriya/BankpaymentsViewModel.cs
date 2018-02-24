using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Buxgalteriya
{
    public class BankpaymentsViewModel
    {
      public long?       rec_id             {get; set;}
      public string     num_doc            {get; set;}
      public string     payer_name         {get; set;}
      public string     payer_inn          {get; set;}
      public string     payer_mfo          {get; set;}
      public string     payer_account      {get; set;}
      public DateTime  tran_date          {get; set;}
      public string     payment_datail     {get; set;}
      public decimal    amount             {get; set;}
      public string     benef_name         {get; set;}
      public string     benef_inn          {get; set;}
      public string     benef_mfo          {get; set;}
      public string     benef_account      {get; set;}
      public string     dbcrflg            {get; set;}
      public long?      parent_id          {get; set;}
      public string     department_base    {get; set;}
      public long?      user_id            {get; set;}
    }
}