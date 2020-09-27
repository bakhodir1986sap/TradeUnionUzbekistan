using KasabaKIAT.Models;
using System.Collections.Generic;
using System.Linq;


namespace KasabaKIAT.BusinessLogic.Buxgalteriya.GlavnayaKniga
{
    public class BalanceDataManager
    {
        public static void Create(BalanceViewModel model, KasabaKIATEntities db)
        {
                tbbu_balance balance = new tbbu_balance
                {
                    DEPARTMENT_BASE = model.department_base ,
                    CUR_YEAR = model.cur_year               ,
                    CUR_KV = model.cur_kv                   ,
                    USER_ID = model.user_id                 ,
                    ACCOUNT_CODE = model.account_code       ,
                    DEBIT_BAL = model.debit_bal             ,
                    CREDIT_BAL = model.credit_bal           ,
                    CREDIT_COUNT = model.credit_count,
                    CREDIT_TRANS = model.credit_trans,
                    DEBIT_COUNT = model.debit_count,
                    DEBIT_TRANS = model.debit_trans
                };

                var pst = db.tbbu_balance.Add(balance);
        }

        public static void Modify(BalanceViewModel model, KasabaKIATEntities db)
        {
                var balans = db.tbbu_balance.Where(z => z.DEPARTMENT_BASE == model.department_base 
                && z.ACCOUNT_CODE == model.account_code
                && z.CUR_YEAR == model.cur_year
                && z.CUR_KV == model.cur_kv);

                if (balans.Any())
                {
                    var balans1 = balans.FirstOrDefault();

                    if (balans1 != null)
                    {
                        balans1.DEBIT_BAL = model.debit_bal            ;
                        balans1.CREDIT_BAL = model.credit_bal;
                    }
                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<BalanceViewModel> Get(BalanceViewModel model)
        {
            List<BalanceViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from balance in db.tbbu_balance
                            where balance.DEPARTMENT_BASE == model.department_base
                            select new BalanceViewModel
                            {
                                rec_id              = balance.REC_ID            ,     
                                department_base     = balance.DEPARTMENT_BASE   ,
                                cur_year            = balance.CUR_YEAR          ,
                                cur_kv              = balance.CUR_KV            ,
                                user_id             = balance.USER_ID           ,
                                account_code        = balance.ACCOUNT_CODE      ,
                                debit_bal           = balance.DEBIT_BAL         ,
                                credit_bal          = balance.CREDIT_BAL
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}