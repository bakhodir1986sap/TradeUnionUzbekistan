using KasabaKIAT.BusinessLogic.Utils;
using KaznaFilesImporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.GlavnayaKniga
{
    public class Balance
    {
        public static void CloseBalance(string depCode, int year, int kvartal, KasabaKIATEntities db)
        {
            var accounts = db.tbbu_balance.Where(z => z.DEPARTMENT_BASE == depCode &&
                                                z.CUR_YEAR == year &&
                                                z.CUR_KV == kvartal);

            if (accounts.Any())
            {
                int o_year, o_kvartal;

                DateOperations.GetNextQuarter(year, kvartal, out o_year, out o_kvartal);

                foreach (var account in accounts)
                {
                    var balance = new BalanceViewModel
                    {
                        department_base = depCode,
                        cur_year = o_year,
                        cur_kv = o_kvartal,
                        user_id = account.USER_ID,
                        account_code = account.ACCOUNT_CODE,
                        debit_bal = account.DEBIT_BAL,
                        credit_bal = account.CREDIT_BAL,
                        debit_count = 0,
                        credit_count = 0,
                        debit_trans = 0,
                        credit_trans = 0
                    };

                    BalanceDataManager.Create(balance, db);
                }
            }
        }
    }
}