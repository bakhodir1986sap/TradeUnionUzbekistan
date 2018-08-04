using KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki;
using KasabaKIAT.BusinessLogic.ClosePeriod.CloseKvartal;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.GlavnayaKniga
{
    public class BuxTransaction
    {
        public int year { get; set; }

        public int kvartal { get; set; }

        public string departmentBase { get; set; }

        public BuxAccount debitAccount { get; set; }

        public BuxAccount creditAccount { get; set; }

        public decimal Amount { get; set; }

        public BuxTransaction(int cur_year, 
            int cur_kvartal, 
            string depCode , 
            string acctcdDebit, 
            string acctcdCredit, 
            decimal amount, KasabaKIATEntities db)
        {
            year = cur_year;
            kvartal = cur_kvartal;

            var curKvartalc = new CloseKvartal();
            var curKvartal = curKvartalc.GetCloseKvartalRow(depCode);

            //if (curKvartal != null)
            //{
            //    if ((curKvartal.curkv_year.Value != year) || (curKvartal.curkv.Value != kvartal))
            //    {
            //        throw new ApplicationException("Current Kvartal Info is invalid = " + year + "   "+ kvartal);
            //    }
            //}
            //else
            //{
            //    throw new ApplicationException("Current Kvartal Info not available = " + year + "   " + kvartal);
            //}

            departmentBase = depCode;
            debitAccount = new BuxAccount(cur_year , cur_kvartal , departmentBase, acctcdDebit, db);
            creditAccount = new BuxAccount(cur_year, cur_kvartal, departmentBase, acctcdCredit, db);

            Amount = amount;
        }

        public void MakeTransaction()
        {
            //Debit Balance
            debitAccount.Operation(TransactionOperation.Debit, Amount);

            //Creadit Balance
            creditAccount.Operation(TransactionOperation.Creadit, Amount);
        }
    }
}