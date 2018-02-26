using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel.Buxgalteriya
{
    public class BalanceViewModel
    {
        public DateTime         trdt               {get; set;}
        public string           account              {get; set;}
        public string           department_code    {get; set;}
        public decimal?          pre_debit_bal      {get; set;}
        public decimal?          pre_credit_bal     {get; set;}
        public int?              debit_count        {get; set;}
        public decimal?          debit_amount       {get; set;}
        public int?              credit_count       {get; set;}
        public decimal?          credit_amount      {get; set;}
        public decimal?          tdbal_debit        {get; set;}
        public decimal?          td_bal_credit      {get; set;}
        public string           ccy                {get; set;}
        public DateTime?         timestamp          {get; set;}
        public long?            user_id            {get; set;}
        public long             rec_id             {get; set;}
        public string           inn                {get; set;}
    }
}