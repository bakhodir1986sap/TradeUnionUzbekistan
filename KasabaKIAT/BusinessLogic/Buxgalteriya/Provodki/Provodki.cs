using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Buxgalteriya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki
{
    public class Provodki
    {
        public void Enter(ProvodkiViewModel model, KasabaKIATEntities db, out tbbu_provodki provod)
        {
            ProvodkiDataManager.Create(ref model, db, out provod);
        }
    }
}