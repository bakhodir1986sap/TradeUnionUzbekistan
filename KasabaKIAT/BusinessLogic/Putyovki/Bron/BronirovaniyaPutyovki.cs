using KasabaKIAT.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic
{
    public class BronirovaniyaPutyovki
    {
        /// <summary>
        /// 
        /// </summary>
        public BronirovaniyaPutyovki()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void PostavitV_Bron(BronViewModel model)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void SnyatBron(BronViewModel model)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private void AvtomaticheskoeSnatiyeBrona()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<BronViewModel> GetBronActiveList()
        {
            return new List<BronViewModel>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="put_number"></param>
        /// <returns></returns>
        public bool IsUnderBron(string put_number)
        {
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="put_number"></param>
        /// <returns></returns>
        public List<BronViewModel> GetBronHistoryList(string put_number)
        {
            return new List<BronViewModel>();
        }
    }
}