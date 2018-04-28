using KasabaKIAT.BusinessLogic.ClosePeriod.CloseHistory;
using KasabaKIAT.BusinessLogic.ClosePeriod.Holydays;
using System;
using System.Linq;

namespace KasabaKIAT.BusinessLogic.ClosePeriod.CloseDay
{
    public delegate void TimeEvent(CloseDayViewModel model);
    /// <summary>
    /// 
    /// </summary>
    public class CloseDay
    {
        public event TimeEvent OnDayClosing;
        public event TimeEvent OnDayClosed;

        public event TimeEvent OnMonthClosing;
        public event TimeEvent OnMonthClosed;

        public event TimeEvent OnQuoterClosing;
        public event TimeEvent OnQuoterClosed;

        public event TimeEvent OnYearClosing;
        public event TimeEvent OnYearClosed;

        private string _departmentBase;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="depBase"></param>
        public CloseDay(string depBase)
        {
            _departmentBase = depBase;
        }


        public bool ChangeCurrentDateAll(long userId)
        {
            var allList = CloseDayDataManager.GetAll();

            if (allList.Any())
            {
                foreach(var depart in allList)
                {
                    if (!ChangeCurrentDate(depart.department_code, userId)) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentCode"></param>
        /// <returns></returns>
        public bool ChangeCurrentDate(string departmentCode, long userId)
        {
            var row = GetBusinessDayRow(departmentCode);

            if (row == null) return false;

            int counter = 1;
            DateTime nextWorkingDay = DateTime.MinValue;

            while(true)
            {
                var nextCalendarDt = row.nextday.Value.AddDays(counter);

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

            var history = new CloseHistoryViewModel()
            {
                department_code = departmentCode,
                closed_date = row.curday,
                remark = "CLOSE",
                user_id = userId                
            };

            BeforeEOD(newBusiness);

            CloseHistoryDataManager.Create(ref history);

            CloseDayDataManager.Modify(newBusiness);

            AfterEOD(newBusiness);

            return true;
        }

        private void BeforeEOD(CloseDayViewModel model)
        {
            var prevDate = model.prevdate;
            var curDate = model.curday;

            var prevKvartal = GetCurQuarter(prevDate.Value);
            var curKvartal = GetCurQuarter(curDate.Value);

            if (prevDate.Value.Year != curDate.Value.Year)
            {
                OnYearClosing?.Invoke(model);
            }
            else if (prevKvartal != curKvartal)
            {
                OnQuoterClosing?.Invoke(model);
            }
            else if (prevDate.Value.Month != curDate.Value.Month)
            {
                OnMonthClosing?.Invoke(model);
            }
            else
            {
                OnDayClosing?.Invoke(model);
            }
        }


        private void AfterEOD(CloseDayViewModel model)
        {
            var prevDate = model.prevdate;
            var curDate = model.curday;

            var prevKvartal = GetCurQuarter(prevDate.Value);
            var curKvartal = GetCurQuarter(curDate.Value);

            if (prevDate.Value.Year != curDate.Value.Year)
            {
                OnYearClosed?.Invoke(model);
            }
            else if (prevKvartal != curKvartal)
            {
                OnQuoterClosed?.Invoke(model);
            }
            else if (prevDate.Value.Month != curDate.Value.Month)
            {
                OnMonthClosed?.Invoke(model);
            }
            else
            {
                OnDayClosed?.Invoke(model);
            }
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

    public class CloseDayTest
    {
        public void Process()
        {
            var closeDay = new CloseDay("");
            closeDay.OnDayClosing += CloseDay_OnDayClosing;

            if (!closeDay.ChangeCurrentDateAll(1))
            {
                throw new ApplicationException("");
            }


        }

        private void CloseDay_OnDayClosing(CloseDayViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}