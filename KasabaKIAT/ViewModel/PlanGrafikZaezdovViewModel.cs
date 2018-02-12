using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel
{
    public class PlanGrafikZaezdovViewModel
    {
        /// <summary>
        /// 
        /// </summary>
      public long rec_id         {get; set;}

        /// <summary>
        /// 
        /// </summary>
      public DateTime? zaezd_begin    {get; set;}

        /// <summary>
        /// 
        /// </summary>
      public DateTime?  zaezd_end      {get; set;}

        /// <summary>
        /// 
        /// </summary>
      public long? place_type_id  {get; set;}

        public string place_type_name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? price          {get; set;}

        /// <summary>
        /// 
        /// </summary>
      public int? quantity       {get; set;}

        /// <summary>
        /// 
        /// </summary>
      public long? user_id        {get; set;}

        /// <summary>
        /// 
        /// </summary>
      public long? sanatory_id    {get; set;}

        public string sanatory_name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? camp_id        {get; set;}

        /// <summary>
        /// 
        /// </summary>
      public string status         {get; set;}

        /// <summary>
        /// 
        /// </summary>
        public string Department_base { get; set; }

    }
}