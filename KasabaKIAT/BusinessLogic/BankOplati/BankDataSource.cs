using KasabaKIAT.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.BankOplati
{
    public abstract class BankDataSource
    {
        public abstract List<BankOplatViewModel> Process();
    }
}