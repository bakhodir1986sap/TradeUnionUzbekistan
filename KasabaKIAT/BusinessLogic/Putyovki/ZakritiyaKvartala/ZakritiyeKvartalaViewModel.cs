using KasabaKiat.ViewModel.Putyovki;
using System.Collections.Generic;

namespace KasabaKIAT.BusinessLogic.Putyovki.ZakritiyaKvartala
{
    public class ZakritiyeKvartalaViewModelTemp
    {
        public int      year    { set; get; }

        public int      id_kv   { set; get; }

        public long     IdOrg   { set; get; }
    }

    public class ZakritiyeKvartalaViewModel
    {
        public int year { set; get; }

        public int id_kv { set; get; }

        public long IdOrg { set; get; }

        public List<RealizasiyaViplPutViewModel> putyovkaList { set; get; }

        public ZakritiyeKvartalaViewModel()
        {
            putyovkaList = new List<RealizasiyaViplPutViewModel>();
        }
    }
}