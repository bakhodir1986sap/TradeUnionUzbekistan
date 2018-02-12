using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel.Putyovki.GodovoyPlan
{
    public class GodovoyPlanOblastViewModel
    {
        /// <summary>
        /// 
        /// </summary>
      public long rec_id             {get; set;}

        /// <summary>
        /// 
        /// </summary>
      public string year               {get; set;}

        /// <summary>
        /// 
        /// </summary>
      public string kvartal            {get; set;}

        /// <summary>
        /// 
        /// </summary>
      public long? sanatorium_id      {get; set;}

        /// <summary>
        /// 
        /// </summary>
        public string sanatorium_name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int?  camp_id            {get; set;}

        /// <summary>
        /// 
        /// </summary>
      public long? otrasl_id          {get; set;}

        /// <summary>
        /// 
        /// </summary>
        public string otrasl_name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string region_id          {get; set;}

      public string region_name        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? price              {get; set;}

        /// <summary>
        /// 
        /// </summary>
      public int? quantity           {get; set;}

        /// <summary>
        /// 
        /// </summary>
      public string status             {get; set;}

        /// <summary>
        /// 
        /// </summary>
      public long? user_id            {get; set;}

        /// <summary>
        /// 
        /// </summary>
      public string departmant_base    {get; set;}

        /// <summary>
        /// 
        /// </summary>
      public DateTime? modify_date        {get; set;}
    }
}