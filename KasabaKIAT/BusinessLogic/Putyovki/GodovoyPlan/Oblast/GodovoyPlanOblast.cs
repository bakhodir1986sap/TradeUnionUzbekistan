using KasabaKIAT.BusinessLogic.GodovoyPlan.Oblast;
using KasabaKIAT.ViewModel.Putyovki.GodovoyPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.GodovoyPlan.Oblast
{
    public class GodovoyPlanOblast
    {
        public decimal GetOtraslIncome(int regionId, int Year)
        {
            return 0;
        }

        public int AvailablePutyovka(long Id_Sanatory, int Year, int Kvartal)
        {
            return 0;
        }

        public static void Create(GodovoyPlanOblastViewModel model)
        {
            GodovoyPlanOblastDataManager.Create(ref model);
        }

        public static void Modify(GodovoyPlanOblastViewModel model)
        {
            GodovoyPlanOblastDataManager.Modify(model);
        }

        public static void Remove(GodovoyPlanOblastViewModel model)
        {
            GodovoyPlanOblastDataManager.Remove(model);
        }

        public List<GodovoyPlanOblastViewModel> GetList(GodovoyPlanOblastViewModel model)
        {
            return GodovoyPlanOblastDataManager.Get(model);
        }
    }
}