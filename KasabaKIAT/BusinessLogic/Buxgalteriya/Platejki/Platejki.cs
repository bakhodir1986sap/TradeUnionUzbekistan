using KasabaKIAT.ViewModel.Buxgalteriya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.Platejki
{
    public class Platejki
    {
        public void Enter(PlatejkiViewModel model)
        {
            PlatejkiDataManager.Create(ref model);
        }
    }
}