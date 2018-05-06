using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel.Putyovki
{
    public class VozvratViewModel
    {
      /// <summary>
      /// Unique identifier
      /// </summary>
      public long   id               {get; set;}

      /// <summary>
      /// Data vozvrata
      /// </summary>
      public DateTime?  data_post        {get; set;}

      /// <summary>
      /// Nomer nakladnoy
      /// </summary>
      public string   num_doc          {get; set;}

        /// <summary>
        /// Reference to table tblSprav_San
        /// </summary>
        public int?  id_sanat         {get; set;}

        /// <summary>
        /// 
        /// </summary>
        public string sanatoriyName { get; set; }

        /// <summary>
        /// Nomer putyovki
        /// </summary>
        public string   num_put          {get; set;}

        /// <summary>
        /// Reference to table tblSprav_Otras
        /// </summary>
        public long?   id_ot            {get; set;}

      /// <summary>
      /// Doverennost
      /// </summary>
      public string  dover            {get; set;}

        /// <summary>
        /// Id poluchatelya reference to table tblSprav_Org
        /// </summary>
        public long?  id_pol           {get; set;}

      /// <summary>
      ///  Summa vozvrata
      /// </summary>
      public decimal?  symmap           {get; set;}

        /// <summary>
        /// Reference to table tblSprav_Kv
        /// </summary>
        public int?  id_kv            {get; set;}

      /// <summary>
      /// Year YYYY
      /// </summary>
      public int?  smfil            {get; set;}

      /// <summary>
      /// Tip vozvrata enumaration { 0 - Vidanniye putyovki, 1 - Spisanniye putyovki }
      /// </summary>
      public bool  tip_voz          {get; set;}

      /// <summary>
      /// Reference to table users
      /// </summary>
      public long?  userid           {get; set;}

      /// <summary>
      /// Reference to table t_departmentbase
      /// </summary>
      public string  departmentbase   {get; set;}
    }
}