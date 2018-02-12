using KasabaKiat.ViewModel;
using KasabaKiat.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok
{
    public class Postupleniya
    {
        private IPostupleniyaEnter enterfunc;

        public Postupleniya()
        {
            enterfunc = new PostupleniyaEnterTemp();
        }

        private  List<PostupleniyaViewModel> GetPotupleniyaMaster(PostupleniyaViewModel paramModel)
        {
            return PostMainDataManager.Get(paramModel);
        }

        private List<PostPutyovkaViewModel> GetPotupleniyaDetail(PostPutyovkaViewModel paramModel)
        {
            return PostPutDetailDataManager.Get(paramModel);
        }

        public List<PutPostTempViewModel> GetCurrentPostupleniya(PutPostTempViewModel filterModel)
        {
            //Use GetPotupleniyaMaster And GetPotupleniyaDetail and make one List for PutPostTempViewModel

            return new List<PutPostTempViewModel>();
        }

        public void CreatePutyovka(PutPostTempViewModel model)
        {
            enterfunc.EnterProcess(model);
        }

        public void ModifyPutyovka(PutPostTempViewModel model)
        {
            enterfunc.ModifyProcess(model);
        }

        public void RemovePutyovka(PutPostTempViewModel model)
        {
            enterfunc.DeleteProcess(model);
        }
    }
}