using KasabaKIAT.BusinessLogic.ClosePeriod.Holydays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.ClosePeriod.CloseDay
{
    /// <summary>
    /// 
    /// </summary>
    public class CloseDay
    {
        private string _departmentBase;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="depBase"></param>
        public CloseDay(string depBase)
        {
            _departmentBase = depBase;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentCode"></param>
        /// <returns></returns>
        private bool ChangeCurrentDate(string departmentCode)
        {
            var row = GetBusinessDayRow(_departmentBase);

            if (row == null) return false;

            int counter = 1;
            DateTime nextWorkingDay = DateTime.MinValue;

            while(true)
            {
                var nextCalendarDt = row.nextday.AddDays(counter);

                if (HolidayDataManager.IsWorkDay(nextCalendarDt))
                {
                    nextWorkingDay = nextCalendarDt;
                    break;
                }

                counter++;
            }

            if (nextWorkingDay == DateTime.MinValue)
            {
                return false;
            }

            var newBusiness = new CloseDayViewModel();

            newBusiness.department_code = departmentCode;
            newBusiness.prevdate = row.curday;
            newBusiness.curday = row.nextday;
            newBusiness.nextday = nextWorkingDay;

            CloseDayDataManager.Modify(newBusiness);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentCode"></param>
        /// <returns></returns>
        private CloseDayViewModel GetBusinessDayRow(string departmentCode)
        {
            var businessDates = CloseDayDataManager.Get(new CloseDayViewModel() { department_code = departmentCode });

            if (businessDates != null)
            {
                var businessDate = businessDates.FirstOrDefault();

                if (businessDate != null)
                {
                    return businessDate;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public DateTime? GetCurrentBusinessDay(string departmentBase)
        {
            var row = GetBusinessDayRow(_departmentBase);

            if (row == null) return null;

            return row.curday;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public DateTime? GetPrevBusinessDay(string departmentBase)
        {
            var row = GetBusinessDayRow(_departmentBase);

            if (row == null) return null;

            return row.prevdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public DateTime? GetNextBusinessDay(string departmentBase)
        {
            var row = GetBusinessDayRow(_departmentBase);

            if (row == null) return null;

            return row.nextday;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public int? GetCurrentMonth(string departmentBase)
        {
            var curday = GetCurrentBusinessDay(departmentBase);

            if (curday.HasValue)
            {
                return curday.Value.Month;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public int? GetPrevMonth(string departmentBase)
        {
            var curday = GetCurrentBusinessDay(departmentBase);

            if (curday.HasValue)
            {
                return curday.Value.AddMonths(-1).Month;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public int? GetNextMonth(string departmentBase)
        {
            var curday = GetCurrentBusinessDay(departmentBase);

            if (curday.HasValue)
            {
                return curday.Value.AddMonths(1).Month;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public int? GetCurrentKvartal(string departmentBase)
        {
            var curday = GetCurrentBusinessDay(departmentBase);

            if (curday.HasValue)
            {
                return GetCurQuarter(curday.Value);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public int? GetPrevKvartal(string departmentBase)
        {
            var curday = GetCurrentBusinessDay(departmentBase);

            if (curday.HasValue)
            {
                return GetPrevQuarter(curday.Value);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public int? GetNextKvartal(string departmentBase)
        {
            var curday = GetCurrentBusinessDay(departmentBase);

            if (curday.HasValue)
            {
                return GetNextQuarter(curday.Value);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public int? GetCurrentYear(string departmentBase)
        {
            var curday = GetCurrentBusinessDay(departmentBase);

            if (curday.HasValue)
            {
                return curday.Value.Year;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public int? GetPrevYear(string departmentBase)
        {
            var curday = GetCurrentBusinessDay(departmentBase);

            if (curday.HasValue)
            {
                return curday.Value.AddYears(-1).Year;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public int? GetNextYear(string departmentBase)
        {
            var curday = GetCurrentBusinessDay(departmentBase);

            if (curday.HasValue)
            {
                return curday.Value.AddYears(1).Month;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private int GetCurQuarter(DateTime dt)
        {
            return (dt.Month - 1) / 3 + 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private int GetPrevQuarter(DateTime dt)
        {
            var res = (dt.Month - 1) / 3 + 1;

            if (res == 1)
            {
                return 4;
            }

            return res - 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private int GetNextQuarter(DateTime dt)
        {
            var res = (dt.Month - 1) / 3 + 1;

            if (res == 4)
            {
                return 1;
            }

            return res + 1;
        }

    }
}