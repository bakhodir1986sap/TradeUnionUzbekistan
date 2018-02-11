using KasabaKIAT.BusinessLogic.GodovoyPlan.Federasiya;
using KasabaKIAT.ViewModel.Putyovki.GodovoyPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.GodovoyPlan.Federasiya
{
    public class GodovoyPlanFed
    {
        //Dlya prinyatiya resheniya
        public decimal GetRegionIncome(int regionId, int Year)
        {
            return 0;
        }

        public int AvailablePutyovka(long Id_Sanatory, int Year, int Kvartal)
        {
            return 0;
        }

        public static void Create(GdovoyPlanFedViewModel model)
        {
            GdovoyPlanFedDataManager.Create(ref model);
        }

        public static void Modify(GdovoyPlanFedViewModel model)
        {
            GdovoyPlanFedDataManager.Modify(model);
        }

        public static void Remove(GdovoyPlanFedViewModel model)
        {
            GdovoyPlanFedDataManager.Remove(model);
        }

        public List<GdovoyPlanFedViewModel> GetList(GdovoyPlanFedViewModel model)
        {
            return GdovoyPlanFedDataManager.Get(model);
        }
    }
}