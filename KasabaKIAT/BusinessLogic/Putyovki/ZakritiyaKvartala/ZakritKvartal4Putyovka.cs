using KasabaKiat.BusinessLogic.Putyovki.RealizasiyaPutyovok;
using System;
using System.Threading.Tasks;

namespace KasabaKIAT.BusinessLogic.Putyovki.ZakritiyaKvartala
{
    public class ZakritKvartal4Putyovka
    {
        public async Task Process(ZakritiyeKvartalaViewModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("model is null");
            }

            if (model.putyovkaList == null)
            {
                throw new NullReferenceException("putyovkaList is null");
            }

            if (model.putyovkaList.Count == 0) return;

            foreach(var mmzakr in model.putyovkaList)
            {
                await Vipl_PutDataManager.ZakritPutyovki(new KasabaKiat.ViewModel.Putyovki.RealizasiyaViplPutViewModel()
                {
                    id = mmzakr.id
                });
            }

        }
    }
}