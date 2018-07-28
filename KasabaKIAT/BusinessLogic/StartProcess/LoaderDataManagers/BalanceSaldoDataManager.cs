using KasabaKIAT.BusinessLogic.StartProcess.LoaderViewModels;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.StartProcess.LoaderDataManagers
{
    public class BalanceSaldoDataManager
    {
        public static void Enter(BalanceSaldoViewModel model, KasabaKIATEntities db)
        {
            var balances = db.tbbu_balance.Where(z => z.DEPARTMENT_BASE == model.department_base
                                                && z.ACCOUNT_CODE == model.account_code
                                                && z.CUR_YEAR == model.cur_year
                                                && z.CUR_KV == model.cur_kv);

            if (balances.Any())
            {
                var balance = balances.First();

                balance.DEBIT_BAL  = model.debit_bal;
                balance.CREDIT_BAL = model.credit_bal;
                balance.DEBIT_COUNT = 0;
                balance.CREDIT_COUNT = 0;
                balance.DEBIT_TRANS = 0;
                balance.CREDIT_TRANS = 0;
            }
            else
            {
                var balance = new tbbu_balance
                {
                    DEPARTMENT_BASE = model.department_base,
                    ACCOUNT_CODE = model.account_code,
                    CUR_YEAR = model.cur_year,
                    CUR_KV = model.cur_kv,
                    DEBIT_BAL = model.debit_bal,
                    CREDIT_BAL = model.credit_bal,
                    DEBIT_COUNT = 0,
                    CREDIT_COUNT = 0,
                    DEBIT_TRANS = 0,
                    CREDIT_TRANS = 0
                };

                db.tbbu_balance.Add(balance);
            }
        }
    }
}