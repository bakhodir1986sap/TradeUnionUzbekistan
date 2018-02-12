using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel.Putyovki.RaznoskaPutyovok
{
    public class FaceViewModel
    {
      /// <summary>
      /// Unique identifier
      /// </summary>
      public long id                   {get; set;}

        /// <summary>
        /// Reference to tblPos_Put
        /// </summary>
        public long? id_put               {get; set;}

        /// <summary>
        /// Reference to table tblSprav_San 
        /// </summary>
        public int? id_sanat              {get; set;}

      /// <summary>
      /// Nomer putyovki
      /// </summary>
      public string num_put            {get; set;}

      /// <summary>
      /// LastName
      /// </summary>
      public string lastname           {get; set;}

      /// <summary>
      /// Name
      /// </summary>
      public string name               {get; set;}

      /// <summary>
      /// FirstName
      /// </summary>
      public string firstname          {get; set;}

      /// <summary>
      /// Adress
      /// </summary>
      public string adress             {get; set;}

      /// <summary>
      /// Work
      /// </summary>
      public string rabota             {get; set;}

      /// <summary>
      /// Doljnost
      /// </summary>
      public string dolj               {get; set;}

        /// <summary>
        /// Reference to table tblSprav_Os
        /// </summary>
        public long? stav_vid             {get; set;}

        /// <summary>
        /// tblSprav_Face name field
        /// </summary>
        public string stav_vidNm { get; set; }

      /// <summary>
      /// Passport Seriya
      /// </summary>
      public string pas_ser            {get; set;}

      /// <summary>
      /// Passport number
      /// </summary>
      public string pas_num            {get; set;}

      /// <summary>
      /// Passport kem vidan
      /// </summary>
      public string pas_kd             {get; set;}

      /// <summary>
      /// Passport data
      /// </summary>
      public DateTime? pas_data         {get; set;}

        /// <summary>
        /// Reference to table tblSprav_Pr 
        /// </summary>
        public long? gost_pr            {get; set;}

        /// <summary>
        /// tblSprav_Pr name
        /// </summary>
        public string gost_prNm { get; set; }

        /// <summary>
        /// Reference to table tblSprav_Face 
        /// </summary>
        public long? id_osnov           {get; set;}

        /// <summary>
        /// tblSprav_Face name
        /// </summary>
        public string id_osnovNm { get; set; }

      /// <summary>
      /// Data spzdaniya zapisi
      /// </summary>
      public DateTime? data_post          {get; set;}

      /// <summary>
      /// Pol Fiz lisa
      /// </summary>
      public bool? pol                {get; set;}

      /// <summary>
      /// Reference to table usrs
      /// </summary>
      public long? userid             {get; set;}
      
      /// <summary>
      /// Reference to table t_departmentbase
      /// </summary>
      public string departmentbase     {get; set;}
    }
}