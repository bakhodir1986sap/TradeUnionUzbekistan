using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.ClosePeriod
{
    public class CloseDayViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public long rec_id   {get;set;}

        /// <summary>
        /// 
        /// </summary>
        public string department_code  {get;set;}

        /// <summary>
        /// 
        /// </summary>
        public DateTime prevdate  {get;set;}

        /// <summary>
        /// 
        /// </summary>
        public DateTime curday   {get;set;}

        /// <summary>
        /// 
        /// </summary>
        public DateTime nextday   {get;set;}
    }
}