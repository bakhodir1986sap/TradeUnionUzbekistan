using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.ClosePeriod.Holydays
{
    /// <summary>
    /// 
    /// </summary>
    public enum HolidayDataType
    {
        Holiday = 1,
        ChangedWeekEnd 
    }

    /// <summary>
    /// 
    /// </summary>
    public enum HolidayStatus
    {
        Aktive = 1,
        NotAktive
    }

    /// <summary>
    /// 
    /// </summary>
    public class HolidayViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public long rec_id    {get;set;}

        /// <summary>
        /// 
        /// </summary>
        public DateTime? set_date  {get;set;}

        /// <summary>
        /// 
        /// </summary>
        public HolidayDataType? date_type {get;set;}

        /// <summary>
        /// 
        /// </summary>
        public HolidayStatus? status    {get;set;}
    }
}