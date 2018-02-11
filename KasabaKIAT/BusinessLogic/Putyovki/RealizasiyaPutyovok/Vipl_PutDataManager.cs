using KasabaKIAT.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.RealizasiyaPutyovok
{
    public class Vipl_PutDataManager
    {
        public static void Create(ref RealizasiyaViplPutViewModel model) { }
        public static void Modify(RealizasiyaViplPutViewModel model) { }
        public static void Remove(RealizasiyaViplPutViewModel model) { }

        public static List<RealizasiyaViplPutViewModel> Get(RealizasiyaViplPutViewModel model)
        {
            return new List<RealizasiyaViplPutViewModel>();
        }
    }
}