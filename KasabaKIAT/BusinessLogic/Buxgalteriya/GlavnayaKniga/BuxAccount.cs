using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.GlavnayaKniga
{
    /// <summary>
    /// 
    /// </summary>
    public enum TransactionOperation
    {
        Debit,
        Creadit
    }

    /// <summary>
    /// 
    /// </summary>
    public class BuxAccount
    {
        /// <summary>
        /// 
        /// </summary>
        private  int    CurYear { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private int CurKvartal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private string accountCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private string description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private decimal CurBalDebit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private decimal CurBalCredit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private int?     debit_count { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private int?     credit_count { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private decimal? debit_trans { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private decimal? credit_trans { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private string departmentBase { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private KasabaKIATEntities _db { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cur_year"></param>
        /// <param name="cur_kvartal"></param>
        /// <param name="_depCode"></param>
        /// <param name="_acctcd"></param>
        /// <param name="db"></param>
        public BuxAccount(int cur_year,
            int cur_kvartal, string _depCode, string _acctcd, KasabaKIATEntities db)
        {
            if (string.IsNullOrEmpty(_depCode) || string.IsNullOrEmpty(_acctcd))
            {
                throw new ApplicationException("Invalid Account parametrs");
            }

            CurYear = cur_year;
            CurKvartal = cur_kvartal;
            accountCode = _acctcd;
            departmentBase = _depCode;

            _db = db;
            InitBalances(_db);
            //in future check if both exists
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        private void InitBalances(KasabaKIATEntities db)
        {
            //Get Balances from DB
            CurBalDebit = 0;
            CurBalCredit = 0;
            debit_count = 0;
            credit_count = 0;
            debit_trans = 0;
            credit_trans = 0;

            var account = db.tbbu_balance.Where(z => z.DEPARTMENT_BASE == departmentBase
                                                && z.ACCOUNT_CODE == accountCode
                                                && z.CUR_YEAR == CurYear
                                                && z.CUR_KV == CurKvartal
                                                );

            if (account.Any())
            {
                var acct = account.FirstOrDefault();

                CurBalDebit = acct.DEBIT_BAL;
                CurBalCredit = acct.CREDIT_BAL;
                debit_count = acct.DEBIT_COUNT;
                credit_count = acct.CREDIT_COUNT;
                debit_trans = acct.DEBIT_TRANS;
                credit_trans = acct.CREDIT_TRANS;
            }
            else
            {
                throw new ApplicationException("Account does not exists!!!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="amount"></param>
        public void Operation(TransactionOperation op, decimal amount)
        {
            if (amount < 0)
            {
                throw new ApplicationException("Invalid amount");
            }

            if (op == TransactionOperation.Debit)
            {
                if ((CurBalDebit + amount) >= CurBalCredit)
                {
                    CurBalDebit = (CurBalDebit + amount) - CurBalCredit;
                    CurBalCredit = 0;
                }
                else
                {
                    CurBalCredit = CurBalCredit - (CurBalDebit + amount);
                    CurBalDebit = 0;
                }

                debit_trans += amount;
                debit_count++;
            }
            else if (op == TransactionOperation.Creadit)
            {
                if ((CurBalCredit + amount) >= CurBalDebit)
                {
                    CurBalCredit = (CurBalCredit + amount) - CurBalDebit;
                    CurBalDebit = 0;
                }
                else
                {
                    CurBalDebit = CurBalDebit - (CurBalCredit + amount);
                    CurBalCredit = 0;
                }

                credit_trans+= amount;
                credit_count++;
            }
            else
            {
                throw new ApplicationException("Invalid operation");
            }

            //Update current balance
            var account = _db.tbbu_balance.Where(z => z.DEPARTMENT_BASE == departmentBase
                                    && z.ACCOUNT_CODE == accountCode
                                    && z.CUR_YEAR == CurYear
                                    && z.CUR_KV == CurKvartal
                                    );

            if (account.Any())
            {
                var acct = account.FirstOrDefault();

                acct.DEBIT_BAL = CurBalDebit;
                acct.CREDIT_BAL = CurBalCredit;
                acct.DEBIT_COUNT = debit_count;
                acct.CREDIT_COUNT = credit_count;
                acct.DEBIT_TRANS = debit_trans;
                acct.CREDIT_TRANS = credit_trans;
            }
        }

    }
}