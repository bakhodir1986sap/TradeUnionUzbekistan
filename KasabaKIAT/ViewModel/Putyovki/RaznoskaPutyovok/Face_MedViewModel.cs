using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel.Putyovki.RaznoskaPutyovok
{
    public class Face_MedViewModel
    {
      /// <summary>
      /// Unique identifier
      /// </summary>
      public long                                           id                 {get; set;}

      /// <summary>
      /// Reference to Master Table 
      /// </summary>
      public long?                                          id_face            {get; set;}

      /// <summary>
      /// Medical Documentation
      /// </summary>
      public string                                         med_doc            {get; set;}

      /// <summary>
      /// Medical Document Date
      /// </summary>
      public DateTime?                                      med_data           {get; set;}
    
      /// <summary>
      /// Medikal sanatory 
      /// reference to Sanatory table 
      /// </summary>
      public int?                                           med_san            {get; set;}

      /// <summary>
      /// Document Oplati
      /// </summary>
      public string                                         opl_doc            {get; set;}

      /// <summary>
      /// Data oplati
      /// </summary>
      public DateTime?                                      opl_data           {get; set;}

      /// <summary>
      /// Summa oplati 
      /// </summary>
      public decimal?                                       opl_symma          {get; set;}

      /// <summary>
      /// Talon document
      /// </summary>
      public string                                         tal_doc            {get; set;}

      /// <summary>
      /// Talon Data
      /// </summary>
      public DateTime?                                      tal_data           {get; set;}

      /// <summary>
      /// Reference to users table 
      /// </summary>
      public long?                                          userid             {get; set;}

      /// <summary>
      /// Reference 
      /// t_departmentbase table 
      /// </summary>
      public string                                         departmentbase     {get; set;}
    }
}