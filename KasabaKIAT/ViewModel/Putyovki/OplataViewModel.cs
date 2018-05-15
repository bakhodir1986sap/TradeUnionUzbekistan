using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel.Putyovki
{
    public class OplataViewModel
    {
      /// <summary>
      /// Unique identifier
      /// </summary>
      public long ID         {get; set;}

      /// <summary>
      /// Data oplati
      /// </summary>
      public DateTime? Data_Post  {get; set;}

      /// <summary>
      /// Nomer Nakladnoy
      /// </summary>
      public string Num_Doc    {get; set;}

        /// <summary>
        /// Reference to tblSprav_Otras
        /// </summary>
        public long? ID_Otras   {get; set;}

        public string OrgName { get; set; }

        /// <summary>
        /// Debit Summa
        /// </summary>
        public decimal? Symma_DT   {get; set;}

      /// <summary>
      /// Kredit Summa
      /// </summary>
      public decimal? Symma_KT   {get; set;}

      /// <summary>
      /// Osnovaniya detail
      /// </summary>
      public string Osnov      {get; set;}

        /// <summary>
        /// Reference to tblSprav_Kv
        /// </summary>
        public int? ID_KV      {get; set;}

      /// <summary>
      /// Year YYYY
      /// </summary>
      public int? SmFil      {get; set;}

      /// <summary>
      /// Organization Name
      /// </summary>
      public string Name_org   {get; set;}

      /// <summary>
      /// enumaration 0 - Opredelen, 1 - Ne opredelen
      /// </summary>
      public bool? Tip_Org    {get; set;}

      /// <summary>
      /// enumaration 0 - Vzaimorascheti, 1 - OplataPutyovok
      /// </summary>
      public int? PrizOpl    {get; set;}

        /// <summary>
        /// User Identifier Reference to table dbo.users
        /// </summary>
        public long? UserId     {get; set;}

        /// <summary>
        /// Reference to table t_departmentbase
        /// </summary>
        public string Departmentbase { get; set; }
    }
}