using KasabaKiat.BusinessLogic.Putyovki.RealizasiyaPutyovok;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.ZakritiyaKvartala
{
    public class ZakritKvartal4Putyovka
    {
        public void Process(ZakritiyeKvartalaViewModel model)
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
                Vipl_PutDataManager.ZakritPutyovki(new KasabaKiat.ViewModel.Putyovki.RealizasiyaViplPutViewModel()
                {
                    id = mmzakr.id
                });
            }

        }
    }
}