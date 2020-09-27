using KasabaKIAT.BusinessLogic.Utils;
using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Buxgalteriya;
using System.Linq;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.GlavnayaKniga
{
    public class BuxControl
    {
        //Provodki Controller
        public static bool IsValidProvodka(ProvodkiViewModel model, KasabaKIATEntities db, out string remark)
        {
            remark = string.Empty;

            if (model == null)
            {
                remark = "provodki structure is empty";
                return false;
            }

            if (model.summa < 0 || model.summa == 0)
            {
                remark = "amount is 0 or minus";
                return false;
            }

            if (model.platelrsch.Equals(model.poluchrsch))
            {
                remark = "debit and credit accounts are the same";
                return false;
            }

            if (db == null)
            {
                remark = "DataBase item is null";
                return false;
            }

            //Check Account Existing
            //Debit Side
            var isDebitExists = db.tbbu_balance.Where(z => z.ACCOUNT_CODE == model.platelrsch && z.DEPARTMENT_BASE == model.department_base);

            if (!isDebitExists.Any())
            {
                remark = "Account Code Debit side does not exists";
                return false;
            }

            //Credit Side
            var isCreditExists = db.tbbu_balance.Where(z => z.ACCOUNT_CODE == model.poluchrsch && z.DEPARTMENT_BASE == model.department_base);

            if (!isCreditExists.Any())
            {
                remark = "Account Code Credit side does not exists";
                return false;
            }

            return true;
        }

        //Balance Controller
        public static bool ValidateBalance(KasabaKIATEntities db, string departmentBase, int year , int kvartal , out string remark)
        {
            remark = string.Empty;

            #region Input Parametrs validation
            if (db == null)
            {
                remark = "DataBase item is null";
                return false;
            }

            if (string.IsNullOrEmpty(departmentBase))
            {
                remark = "departmentBase is null or empty";
                return false;
            }

            if (year < 0)
            {
                remark = "invalid year";
                return false;
            }

            if (kvartal < 0)
            {
                remark = "invalid kvartal";
                return false;
            }
            #endregion

            #region Previous Kvartal
            int o_year, o_kv;

            DateOperations.GetPrevQuarter(year, kvartal, out o_year, out o_kv);

            #endregion

            //Compare Provodki with Balance Accounts
            var balances = db.tbbu_balance.Where(z => z.DEPARTMENT_BASE == departmentBase && z.CUR_YEAR == year && z.CUR_KV == kvartal);

            if (!balances.Any())
            {
                remark = string.Format("invalid departmentBase value {0}", departmentBase);
                return false;
            }

            foreach (var bal in balances)
            {
                //Debit Side
                #region Debit Side
                var provodkisD = db.tbbu_provodki.Where(z => z.PLATELRSCH == bal.ACCOUNT_CODE 
                && z.DEPARTMENT_BASE == departmentBase);

                decimal? debitTransactionSum = 0;
                int? debitTransactionCount = 0;

                if (provodkisD.Any())
                {
                    foreach (var debit in provodkisD)
                    {
                        if (debit.TRDT.HasValue)
                        {
                            if ((DateOperations.GetQuarter(debit.TRDT.Value) == kvartal) && (debit.TRDT.Value.Year == year))
                            {
                                debitTransactionSum += debit.SUMMA;
                                debitTransactionCount++;
                            }
                        }
                    }
                }

                if (bal.DEBIT_TRANS != debitTransactionSum)
                {
                    remark = string.Format("DEBIT_TRANS for Account = {0} year = {1} kvartal = {2} DEBIT_TRANS = {3} debitTransactionSum = {4}"
                        , bal.ACCOUNT_CODE , year , kvartal, bal.DEBIT_TRANS , debitTransactionSum);
                    return false;
                }

                if (bal.DEBIT_COUNT != debitTransactionCount)
                {
                    remark = string.Format("DEBIT_TRANS for Account = {0} year = {1} kvartal = {2} DEBIT_COUNT = {3} debitTransactionCount = {4}"
                        , bal.ACCOUNT_CODE, year, kvartal, bal.DEBIT_COUNT, debitTransactionCount);
                    return false;
                }

                #endregion

                //Credit Side
                #region Credit Side

                decimal? creditTransactionSum = 0;
                int? creditTransactionCount = 0;

                var provodkisC = db.tbbu_provodki.Where(z => z.POLUCHRSCH == bal.ACCOUNT_CODE
                && z.DEPARTMENT_BASE == departmentBase);

                if (provodkisC.Any())
                {
                    foreach (var credit in provodkisC)
                    {
                        if (credit.TRDT.HasValue)
                        {
                            if ((DateOperations.GetQuarter(credit.TRDT.Value) == kvartal) && (credit.TRDT.Value.Year == year))
                            {
                                creditTransactionSum += credit.SUMMA;
                                creditTransactionCount++;
                            }
                        }
                    }
                }

                if (bal.CREDIT_TRANS != creditTransactionSum)
                {
                    remark = string.Format("DEBIT_TRANS for Account = {0} year = {1} kvartal = {2} CREDIT_TRANS = {3} creditTransactionSum = {4}"
                        , bal.ACCOUNT_CODE, year, kvartal, bal.CREDIT_TRANS, creditTransactionSum);
                    return false;
                }

                if (bal.CREDIT_COUNT != creditTransactionCount)
                {
                    remark = string.Format("DEBIT_TRANS for Account = {0} year = {1} kvartal = {2} CREDIT_COUNT = {3} creditTransactionCount = {4}"
                        , bal.ACCOUNT_CODE, year, kvartal, bal.CREDIT_COUNT, creditTransactionCount);
                    return false;
                }

                #endregion

                #region Debit_bal and Credit_bal both have positive balance 
                if (bal.DEBIT_BAL > 0 && bal.CREDIT_BAL > 0)
                {
                    remark = string.Format("DEBIT_BAL > 0 and CREDIT_BAL > 0 {0}", departmentBase);
                    return false;
                }
                #endregion

                //Balance Arifmetics check
                #region Balance Arifmetics check
                var balancesPrev = db.tbbu_balance.Where(
                    z => z.DEPARTMENT_BASE == departmentBase && 
                    z.CUR_YEAR == o_year && 
                    z.CUR_KV == o_kv && 
                    z.ACCOUNT_CODE == bal.ACCOUNT_CODE);

                if (balancesPrev.Any())
                {
                    var prevAccount = balancesPrev.First();

                    if (prevAccount.DEBIT_BAL > 0)
                    {
                        var calcD = prevAccount.DEBIT_BAL + bal.DEBIT_TRANS - bal.CREDIT_TRANS;

                        if (calcD > 0)
                        {
                            if (bal.DEBIT_BAL != calcD)
                            {
                                remark = string.Format("111 bal.DEBIT_BAL != calcD {0} account = {1}", departmentBase , bal.ACCOUNT_CODE);
                                return false;
                            }
                        }
                        else if (calcD < 0)
                        {
                            if (bal.CREDIT_BAL != calcD)
                            {
                                remark = string.Format("222 bal.CREDIT_BAL != calcD {0} account = {1}", departmentBase, bal.ACCOUNT_CODE);
                                return false;
                            }
                        }
                    }
                    else if (prevAccount.CREDIT_BAL > 0)
                    {
                        var calcC = prevAccount.CREDIT_BAL + bal.CREDIT_TRANS - bal.DEBIT_TRANS;

                        if (calcC > 0)
                        {
                            if (bal.CREDIT_BAL != calcC)
                            {
                                remark = string.Format("333 bal.CREDIT_BAL != calcC {0} account = {1}", departmentBase, bal.ACCOUNT_CODE);
                                return false;
                            }
                        }
                        else if (calcC < 0)
                        {
                            if (bal.DEBIT_BAL != calcC)
                            {
                                remark = string.Format("444 bal.DEBIT_BAL != calcC {0} account = {1}", departmentBase, bal.ACCOUNT_CODE);
                                return false;
                            }
                        }
                    }
                    else
                    {

                    }
                }

                #endregion
            }

            //Aktiv and Passiv Equality
            #region Aktiv and Passiv Equality
            var debitSum = balances.Sum(z => z.DEBIT_BAL);
            var creditSum = balances.Sum(z => z.CREDIT_BAL);

            if (debitSum != creditSum)
            {
                remark = string.Format("Debit Total Balance not Equal to Credit Bal {0}", departmentBase);
                return false;
            }

            #endregion

            return true;
        }

        //GlavNayaKniga Controller

        //FinOtchet Controller

    }
}