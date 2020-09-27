using KasabaKiat.ViewModel.Buxgalteriya;
using System.Linq;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.PlanProvodok
{
    public class AccountType
    {
        public void Enter(PlanSchetovViewModel model)
        {
            AccountTypeDataManager.Create(ref model);
        }

        public PlanSchetovViewModel GetAccount(string accountCode)
        {
            var acPlan = AccountTypeDataManager.Get(accountCode);

            return acPlan.FirstOrDefault();
        }


        public string AccountName(string accountCode)
        {
            var account = GetAccount(accountCode);

            return account.account_name;
        }
    }
}