using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.BankOplati
{
    public class BankOplatiTotalManager
    {
        public void ImportData(BankDataSource source)
        {
            var list = source.Process();

            foreach(var oplata in list)
            {
                BankOplatiDataManager.CreateBankOplat(oplata);
            }
        }
    }
}