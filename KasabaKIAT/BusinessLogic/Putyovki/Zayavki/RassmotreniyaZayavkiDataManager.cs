using KasabaKiat.ViewModel;
using KasabaKiat.ViewModel.RassmotreniyaZayavok;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Zayavki
{
    public class RassmotreniyaZayavkiDataManager
    {
        /// <summary>
        /// For Rassmotreniya Zayavki Screen
        /// </summary>
        /// <param name="inn"></param>
        /// <param name="state">in general always new</param>
        /// <returns></returns>
        public static List<RequestPutyovkiViewModel> GetRequestPutyovki(string inn, ZayavkaStates state)
        {
            return new List<RequestPutyovkiViewModel>();
        }

        /// <summary>
        /// For Rassmotreniya Zayavki Screen
        /// </summary>
        /// <param name="inn"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static List<BankTranForOrgVewModel> GetBankTranForOrg(string inn, ZayavkaStates state)
        {
            return new List<BankTranForOrgVewModel>();
        }

        /// <summary>
        /// For Rassmotreniya Zayavki Screen
        /// </summary>
        /// <param name="inn"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static List<ConsolidPutyovkiViewModel> GetConsolidPutyovki(string inn, ZayavkaStates state)
        {
            return new List<ConsolidPutyovkiViewModel>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static List<ZayavkiChangeHistoryViewModel> GetZayavkiChangeHistory(string inn, ZayavkaStates state)
        {
            return new List<ZayavkiChangeHistoryViewModel>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static List<ResheniyeViewModel> GetResheniye(string inn, ZayavkaStates state)
        {
            return new List<ResheniyeViewModel>();
        }


        public static void ChangePossibleQuantity(string inn, ZayavkaStates state, List<PossibleQuantityViewModel> listModel)
        {
            //Update valueOrdered Update for each one every case 1 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void ChangeStateOfZayavka(ZayavkiViewModel model)
        {
            ///Update by recId
        }

    }
}