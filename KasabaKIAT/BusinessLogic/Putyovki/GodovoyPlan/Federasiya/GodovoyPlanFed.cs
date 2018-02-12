using KasabaKiat.BusinessLogic.BankOplati.BankOplatQueries;
using KasabaKiat.BusinessLogic.GodovoyPlan.Federasiya;
using KasabaKiat.BusinessLogic.Zayavki;
using KasabaKiat.ViewModel.Putyovki.GodovoyPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Putyovki.GodovoyPlan.Federasiya
{
    /// <summary>
    /// 
    /// </summary>
    public class GodovoyPlanFed
    {
        //Dlya prinyatiya resheniya
        public decimal GetRegionIncome(string soato, int Year)
        {
            var regionIncome = BankPaymentsQueriesN.GetIncomeForOblastYearlyT(Year, soato);

            if (regionIncome.HasValue)
            {
                return regionIncome.Value;
            }

            return 0;
        }

        /// <summary>
        /// Dostupno putyovok : N kolichestvo
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="regionCode"></param>
        /// <returns></returns>
        public int AvailablePutyovka(int Year, string regionCode)
        {
            int availablePutForRegion = 0;

            var regionIncome = BankPaymentsQueriesN.GetIncomeForOblastYearlyT(Year, regionCode);

            if (regionIncome.HasValue)
            {
                var avaragePricePut = AvaragePriceManager.GetAvaragePriceForYear(Year.ToString());

                availablePutForRegion = (int)(regionIncome / avaragePricePut.Amount);
            }

            return availablePutForRegion;
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