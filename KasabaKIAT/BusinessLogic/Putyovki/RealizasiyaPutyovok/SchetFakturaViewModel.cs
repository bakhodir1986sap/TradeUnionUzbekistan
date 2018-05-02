using KasabaKiat.BusinessLogic.Utils.References;
using KasabaKiat.ViewModel.Putyovki;
using KasabaKIAT.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.RealizasiyaPutyovok
{
    public class SchetFakturaViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public RekvizitiViewModel rekvizit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Sprav_OrgViewModel clientData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RealizasiyaViplMainViewModel nakladnaya { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<RealizasiyaViplPutViewModel> nakladDtl { get; set; }

    }
}