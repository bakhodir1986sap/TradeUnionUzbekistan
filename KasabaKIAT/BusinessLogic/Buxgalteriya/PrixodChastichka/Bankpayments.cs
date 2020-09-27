using KasabaKIAT.ViewModel.Buxgalteriya;
using System;
using System.Collections.Generic;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.PrixodChastichka
{
    public class Bankpayments
    {
        public void EnterData(BankpaymentsViewModel model)
        {
            if (model == null)
            {
                throw new ApplicationException("Input data is null");
            }
        }

        public List<BankpaymentsViewModel> GetData(string departmentBase, DateTime beginDate, DateTime endDate)
        {
            return BankpaymentsDataManager.Get(departmentBase, beginDate, endDate);
        }
    }
}