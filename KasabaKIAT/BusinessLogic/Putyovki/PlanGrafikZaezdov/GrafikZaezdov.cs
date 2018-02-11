using KasabaKIAT.BusinessLogic.PlanGrafikZaezdov;
using KasabaKIAT.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.PlanGrafikZaezdov
{
    public class GrafikZaezdov
    {
        public void Create(PlanGrafikZaezdovViewModel model)
        {
            GrafikZaezdovDataManager.Create(model);
        }

        public static void Update(PlanGrafikZaezdovViewModel model)
        {
            GrafikZaezdovDataManager.Update(model);
        }

        public static void Remove(PlanGrafikZaezdovViewModel model)
        {
            GrafikZaezdovDataManager.Remove(model);
        }

        public static List<PlanGrafikZaezdovViewModel> Get(PlanGrafikZaezdovViewModel model)
        {
            return GrafikZaezdovDataManager.Get(model);
        }
    }
}