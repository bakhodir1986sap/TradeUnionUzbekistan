using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Putyovki
{
    public class RealizasiyaOtraslViewModel
    {
        public long ID_otras { get; set; }

        public string OtrasName { get; set; }

        public string OtraslName { get; set; }

        public string PolojenoDlyaOtrasli { get; set; }

        public string IspolzovannoPoOtrasli { get; set; }

        public int Kvartal { get; set; }

        public int Year { get; set; }
    }
}