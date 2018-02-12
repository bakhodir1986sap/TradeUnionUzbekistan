using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel.Putyovki
{
    public class BronViewModel
    {
        /// <summary>
        /// 
        /// </summary>
      public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
      public int ID_San { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SanatoryName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Num_Put { get; set; }

        /// <summary>
        /// 
        /// </summary>
      public string  Prem { get; set; }

        /// <summary>
        /// 
        /// </summary>
      public bool? Uchet { get; set; }

        /// <summary>
        /// 
        /// </summary>
      public DateTime?  ExpireDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
      public long? UserId { get; set; }


        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string  DepartmentBase { get; set; }
    }
}