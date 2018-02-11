using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel
{
    public class PostupleniyaViewModel
    {

        public long Id { get; set; }

        public int? Id_Kv { get; set; }

        public string Kvartal { get; set; }

        public DateTime? Data_Post { get; set; }

        public string Num_Doc { get; set; }

        public long? ID_Pos { get; set; }

        public int Num_Kol { get; set; }

        public decimal Symma { get; set; }

        public string Name_Dov { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Vid { get; set; }

        /// <summary>
        /// Year YYYY
        /// </summary>
        public int SmFil { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// Which Branch entered
        /// </summary>
        public string DepartmentBase { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BeginNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int EndNumber { get; set; }

    }
}