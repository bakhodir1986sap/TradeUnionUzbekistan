using KasabaKIAT.ViewModel.Buxgalteriya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

            BankpaymentsDataManager.Create(ref model);
        }

        public List<BankpaymentsViewModel> GetData(string departmentBase, DateTime beginDate, DateTime endDate)
        {
            return BankpaymentsDataManager.Get(departmentBase, beginDate, endDate);
        }
    }
}