using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel.Putyovki
{
    public class OplataRasxodiViewModel
    {
      /// <summary>
      /// Unique identifier
      /// </summary>
      public long ID         { get; set;}

       /// <summary>
       /// Reference to master table tblOplata
       /// </summary>
       public long? ID_Opl     { get; set;}

        /// <summary>
        /// Reference to table tblSprav_San
        /// </summary>
        public int? ID_Sanat   { get; set;}

        /// <summary>
        /// 
        /// </summary>
        public string SanatName { get; set; }

        /// <summary>
        /// Numer putyovki
        /// </summary>
        public string Num_Put    { get; set;}

      /// <summary>
      /// Amount
      /// </summary>
      public decimal? Symma      { get; set;}

        /// <summary>
        /// Reference to tblSprav_Pr
        /// </summary>
        public long? ID_Pr      { get; set;}

        /// <summary>
        /// 
        /// </summary>
        public string Name_Proc { get; set; }

        /// <summary>
        /// enumaration 0 - Vzaimorascheti, 1 - OplataPutyovok
        /// </summary>
        public short? Priz       { get; set;}

      /// <summary>
      /// Reference to table users
      /// </summary>
      public long? UserId     { get; set;}

      /// <summary>
      /// Reference to table t_departmentbase
      /// </summary>
      public string Departmentbase { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool PutyovkaChecked { get; set; }


    }
}