using KasabaKiat.ViewModel.Buxgalteriya;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Buxgalteriya.Balance.Balance
{
    public class BalanceMasterDataManager
    {
        public static void Create(ref BalanceViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbgl_balance reg = new tbgl_balance();

                reg.TRDT                = model.trdt            ;
                reg.ACCOUNT             = model.account         ;
                reg.DEPARTMENT_CODE     = model.department_code ;
                reg.PRE_DEBIT_BAL       = model.pre_debit_bal   ;
                reg.PRE_CREDIT_BAL      = model.pre_credit_bal  ;
                reg.DEBIT_COUNT         = model.debit_count     ;
                reg.DEBIT_AMOUNT        = model.debit_amount    ;
                reg.CREDIT_COUNT        = model.credit_count    ;
                reg.CREDIT_AMOUNT       = model.credit_amount   ;
                reg.TDBAL_DEBIT         = model.tdbal_debit     ;
                reg.TD_BAL_CREDIT       = model.td_bal_credit   ;
                reg.CCY                 = model.ccy             ;
                reg.TIMESTAMP           = model.timestamp       ;
                reg.USER_ID             = model.user_id         ;
                //reg.REC_ID            = model.rec_id          ;
                reg.INN                 = model.inn;

                var planOtrasl = db.tbgl_balance.Add(reg);

                db.SaveChanges();

                model.rec_id = reg.REC_ID;
            }
        }

        public static void Modify(BalanceViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbgl_balance.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.TRDT = model.trdt;
                        reg.ACCOUNT = model.account;
                        reg.DEPARTMENT_CODE = model.department_code;
                        reg.PRE_DEBIT_BAL = model.pre_debit_bal;
                        reg.PRE_CREDIT_BAL = model.pre_credit_bal;
                        reg.DEBIT_COUNT = model.debit_count;
                        reg.DEBIT_AMOUNT = model.debit_amount;
                        reg.CREDIT_COUNT = model.credit_count;
                        reg.CREDIT_AMOUNT = model.credit_amount;
                        reg.TDBAL_DEBIT = model.tdbal_debit;
                        reg.TD_BAL_CREDIT = model.td_bal_credit;
                        reg.CCY = model.ccy;
                        reg.TIMESTAMP = model.timestamp;
                        reg.USER_ID = model.user_id;
                        //reg.REC_ID = model.rec_id;
                        reg.INN = model.inn;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(BalanceViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbgl_balance.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tbgl_balance.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<BalanceViewModel> Get(BalanceViewModel model)
        {
            List<BalanceViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tbgl_balance
                            where bank.DEPARTMENT_CODE == model.department_code
                            select new BalanceViewModel
                            {
                                trdt            = bank.TRDT             ,
                                account         = bank.ACCOUNT          , 
                                department_code = bank.DEPARTMENT_CODE  , 
                                pre_debit_bal   = bank.PRE_DEBIT_BAL    , 
                                pre_credit_bal  = bank.PRE_CREDIT_BAL   , 
                                debit_count     = bank.DEBIT_COUNT      , 
                                debit_amount    = bank.DEBIT_AMOUNT     , 
                                credit_count    = bank.CREDIT_COUNT     , 
                                credit_amount   = bank.CREDIT_AMOUNT    , 
                                tdbal_debit     = bank.TDBAL_DEBIT      , 
                                td_bal_credit   = bank.TD_BAL_CREDIT    , 
                                ccy             = bank.CCY              , 
                                timestamp       = bank.TIMESTAMP        , 
                                user_id         = bank.USER_ID          , 
                                rec_id          = bank.REC_ID           ,
                                inn = bank.INN
            };

                list = query.ToList();
            }

            return list;
        }


        public static List<BalanceViewModel> Get(string accountCode)
        {
            List<BalanceViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                //var query = from bank in db.tbgl_balance
                //            where bank.ACCOUNT_CODE == accountCode
                //            select new BalanceViewModel
                //            {
                //                rec_id = bank.REC_ID,
                //                account_name = bank.ACCOUNT_NAME,
                //                account_code = bank.ACCOUNT_CODE,
                //                account_type = bank.ACCOUNT_TYPE,
                //                status = bank.STATUS,
                //                userud = bank.UserUd,
                //                departmentbase = bank.DepartmentBase
                //            };

                //list = query.ToList();
            }

            return list;
        }

    }
}