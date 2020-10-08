using KasabaKiat.ViewModel;
using KasabaKiat.ViewModel.Putyovki;
using System.Collections.Generic;


namespace KasabaKiat.BusinessLogic.RealizasiyaPutyovok
{
    public class Realizasiya
    {
        public List<PostupleniyaViewModel> GetMainList()
        {
            return new List<PostupleniyaViewModel>();
        }

        public List<PutPostTempViewModel> GetDetailView(PostupleniyaViewModel filterModel)
        {
            return new List<PutPostTempViewModel>();
        }

        public BronViewModel GetActiveBron(string Num_Putyovka)
        {
            return new BronViewModel();
        }

        public bool IsUrgentPutyovka(string Num_Putyovka)
        {
            return false;
        }

        public string GetOrganization(string inn)
        {
            return string.Empty;
        }

        public List<RealizasiyaOtraslViewModel> GetOtraslList(int year, int kvartal)
        {
            return new List<RealizasiyaOtraslViewModel>();
        }

        public List<ZayavkiViewModel> GetAppliedZayavkaList(string inn)
        {
            return new List<ZayavkiViewModel>();
        }

    }
}