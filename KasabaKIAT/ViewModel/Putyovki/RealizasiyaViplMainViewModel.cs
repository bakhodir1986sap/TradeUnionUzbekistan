using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel.Putyovki
{
    public class RealizasiyaViplMainViewModel
    {
      /// <summary>
      /// Unical Identifier
      /// </summary>
      public long       id                 {get; set;}

      /// <summary>
      /// Organization link
      /// </summary>
      public long?       id_org             {get; set;}

      /// <summary>
      /// Data postupleniya
      /// </summary>
      public DateTime? data_post      {get; set;}

      /// <summary>
      /// Nomer documenta
      /// </summary>
      public string num_doc          {get; set;}

      /// <summary>
      /// Nomer po poryadku
      /// </summary>
      public long? id_pok             {get; set;}

      /// <summary>
      /// Kolichestvo putyovok
      /// </summary>
      public int? num_kol             {get; set;}

      /// <summary>
      /// Summa obshaya
      /// </summary>
      public decimal? symma           {get; set;}

      /// <summary>
      /// Nazvaniye doverennosti
      /// </summary>
      public string name_dov         {get; set;}

      /// <summary>
      /// Id Kvartala
      /// </summary>
      public int? id_kv               {get; set;}

      /// <summary>
      /// Flag vidannosti
      /// </summary>
      public bool vid                {get; set;}

      /// <summary>
      /// God zapolneniya
      /// </summary>
      public int? smfil               {get; set;}

      /// <summary>
      /// Polzovatel
      /// </summary>
      public long? userid             {get; set;}

      /// <summary>
      /// Department
      /// </summary>
      public string departmentbase   {get; set;}
    }
}