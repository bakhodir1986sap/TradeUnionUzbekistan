using KasabaKiat.BusinessLogic.Bron;
using KasabaKiat.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic
{
    public class BronirovaniyaPutyovki
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BronirovaniyaPutyovki()
        {

        }

        /// <summary>
        /// Make putyovka in pron
        /// </summary>
        /// <param name="model"></param>
        public void PostavitV_Bron(BronViewModel model)
        {
            var brExists = BronDataManager.GetBronByPutyovka(model.Num_Put, model.ID_San);

            if (brExists == null)
            {
                BronDataManager.Create(ref model);
            }
            else
            {
                BronDataManager.Update(model);

                BronDataManager.EnableExistingBron(model.Id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void SnyatBron(BronViewModel model)
        {
            var brExists = BronDataManager.GetBronByPutyovka(model.Num_Put, model.ID_San);

            if (brExists != null)
            {
                BronDataManager.DisableExistingBron(model.Id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AvtomaticheskoeSnatiyeBrona()
        {
            BronDataManager.AutomaticDisableBronsByExpiredDate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<BronViewModel> GetBronActiveList()
        {
            return BronDataManager.GetBronAllActive();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="put_number"></param>
        /// <returns></returns>
        public bool IsUnderBron(string put_number, int IdSan)
        {
            var list = BronDataManager.GetBronByPutyovka(put_number, IdSan);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="put_number"></param>
        /// <returns></returns>
        public List<BronViewModel> GetBronHistoryList(string put_number, int IdSan)
        {
            return BronDataManager.GetBronByPutyovkaHistory(put_number, IdSan);
        }
    }
}