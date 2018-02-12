using KasabaKiat.BusinessLogic.BankOplati.BankOplatQueries;
using KasabaKiat.BusinessLogic.GodovoyPlan.Oblast;
using KasabaKiat.BusinessLogic.Zayavki;
using KasabaKiat.ViewModel.Putyovki.GodovoyPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Putyovki.GodovoyPlan.Oblast
{
    public class GodovoyPlanOblast
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="soato"></param>
        /// <param name="branch_id"></param>
        /// <returns></returns>
        public decimal GetOtraslIncome(int Year , string soato, int branch_id )
        {
            var regionIncome = BankPaymentsQueriesN.GetIncomeForOtraslYearlyT2(Year, branch_id, soato);

            if (regionIncome.HasValue)
            {
                return regionIncome.Value;
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="regionCode"></param>
        /// <param name="branch_id"></param>
        /// <returns></returns>
        public int AvailablePutyovka(int Year, string regionCode, int branch_id)
        {
            int availablePutForRegion = 0;

            var regionIncome = BankPaymentsQueriesN.GetIncomeForOtraslYearlyT2(Year, branch_id, regionCode);

            if (regionIncome.HasValue)
            {
                var avaragePricePut = AvaragePriceManager.GetAvaragePriceForYear(Year.ToString());

                availablePutForRegion = (int)(regionIncome / avaragePricePut.Amount);
            }

            return availablePutForRegion;
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