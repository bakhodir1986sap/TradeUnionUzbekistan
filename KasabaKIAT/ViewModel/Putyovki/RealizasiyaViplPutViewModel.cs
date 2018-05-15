using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel.Putyovki
{
    public class RealizasiyaViplPutViewModel
    {
      /// <summary>
      /// Unique identifier
      /// </summary>
      public long                                           id                              {get; set;}

      /// <summary>
      /// Reference to master table
      /// </summary>
      public long?                                          id_vipl                         {get; set;}

      /// <summary>
      /// Nomer po poryadku
      /// </summary>
      public long?                                          id_pok                          {get; set;}

      /// <summary>
      /// Reference to Sanatory 
      /// </summary>
      public int?                                           id_sanat                        {get; set;}

        /// <summary>
        /// 
        /// </summary>
      public string                                         sanatName                       { get; set; }

        /// <summary>
        /// Nomer putyovki
        /// </summary>
        public string                                       num_put                         {get; set;}

      /// <summary>
      /// Nomer procenta
      /// </summary>
      public int?                                           num_proc                        {get; set;}

      /// <summary>
      /// summa 
      /// </summary>
      public decimal?                                       symma                           {get; set;}

      /// <summary>
      /// Polzovanniy
      /// </summary>
      public bool                                           id_polvoz                       {get; set;}

      /// <summary>
      /// Data postuplaniya
      /// </summary>
      public DateTime?                                      data_post                       {get; set;}

      /// <summary>
      /// User Id
      /// </summary>
      public long?                                          userid                          {get; set;}

      /// <summary>
      /// reference to tbpt_zayavka
      /// </summary>
      public long?                                          zayavkaid                       {get; set;}

      /// <summary>
      /// Department
      /// </summary>
      public string                                         departmentbase                  {get; set;}

        /// <summary>
        /// 
        /// </summary>
        public bool                                         putyovkaChecked                 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string                                       primechaniye                    { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string                                       oplata                              { get; set; }
    }
}