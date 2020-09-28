using KasabaKiat.BusinessLogic.Bron;
using KasabaKiat.ViewModel.Putyovki;
using System.Collections.Generic;
using System.Linq;

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

            if (brExists.Count == 0)
            {
                BronDataManager.Create(model);
            }
            else
            {
                BronDataManager.Update(model);

                BronDataManager.EnableExistingBron(brExists.First().Id);
            }
        }


        /// <summary>
        /// Make putyovka in pron
        /// </summary>
        /// <param name="model"></param>
        public void PostavitV_Bron(List<BronViewModel> Lmodel)
        {
            foreach(var model in Lmodel)
            {
                var brExists = BronDataManager.GetBronByPutyovka(model.Num_Put, model.ID_San);

                if (brExists.Count == 0)
                {
                    BronDataManager.Create(model);
                }
                else
                {
                    BronDataManager.Update(model);

                    BronDataManager.EnableExistingBron(brExists.First().Id);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void SnyatBron(BronViewModel model)
        {
            var brExists = BronDataManager.GetBronByPutyovka(model.Num_Put, model.ID_San);

            if (brExists.Count != 0)
            {
                BronDataManager.DisableExistingBron(brExists.First().Id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<BronViewModel> GetBronActiveList(string departmentBase)
        {
            return BronDataManager.GetBronAllActive(departmentBase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="put_number"></param>
        /// <returns></returns>
        public bool IsUnderBron(string put_number, int IdSan)
        {
            var list = BronDataManager.GetBronByPutyovka(put_number, IdSan);

            if (list.Count != 0)
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