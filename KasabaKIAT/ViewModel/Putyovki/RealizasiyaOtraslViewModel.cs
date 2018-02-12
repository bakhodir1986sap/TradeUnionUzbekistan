using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel.Putyovki
{
    public class RealizasiyaOtraslViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public long ID_otras { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OtrasName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OtraslName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PolojenoDlyaOtrasli { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IspolzovannoPoOtrasli { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Kvartal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Year { get; set; }
    }
}