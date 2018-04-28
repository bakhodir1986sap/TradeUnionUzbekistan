using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace KasabaKIAT.BusinessLogic.ClosePeriod.Holydays
{
    /// <summary>
    /// 
    /// </summary>
    public class Holydays
    {
        /// <summary>
        /// 
        /// </summary>
        public void EnterHoliday(DateTime setdate, int dateType)
        {
            var holiday = new HolidayViewModel();
            holiday.set_date = setdate;
            holiday.date_type = (HolidayDataType)dateType;
            holiday.status = HolidayStatus.Aktive;

            HolidayDataManager.Create(ref holiday);

        }

        /// <summary>
        /// 
        /// </summary>
        public void ModifyHoliday(DateTime oldSetdate, DateTime newSetDate)
        {
            var holiday = new HolidayViewModel();
            holiday.set_date = newSetDate;

            HolidayDataManager.ModifyByDate(oldSetdate, holiday);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setdate"></param>
        public void RemoveHoliday(DateTime setdate)
        {
            HolidayDataManager.RemoveByDate(setdate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsWorkDay(DateTime date)
        {
            return HolidayDataManager.IsWorkDay(date);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsHoliday(DateTime date)
        {
            return HolidayDataManager.IsHoliday(date);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="dateType"></param>
        /// <returns></returns>
        public List<HolidayViewModel> GetHolidaysByYear(int year, HolidayDataType dateType)
        {
            return HolidayDataManager.GetByYear(year, dateType);
        }

    }


    public class HolidaysTest
    {
        Holydays holi = new Holydays();

        public void Process()
        {
            try
            {
                holi.EnterHoliday(DateTime.ParseExact("20180101", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo), 1);
                holi.EnterHoliday(DateTime.ParseExact("20180308", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo), 1);
                holi.EnterHoliday(DateTime.ParseExact("20180321", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo), 1);
                holi.EnterHoliday(DateTime.ParseExact("20180509", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo), 1);
                holi.EnterHoliday(DateTime.ParseExact("20180901", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo), 1);
                holi.EnterHoliday(DateTime.ParseExact("20181001", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo), 1);
                holi.EnterHoliday(DateTime.ParseExact("20181208", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo), 1);
                holi.EnterHoliday(DateTime.ParseExact("20180610", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo), 1);
                holi.EnterHoliday(DateTime.ParseExact("20180815", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo), 1);
            }
            catch
            {
                //Dublicate enter
            }

            if (!holi.IsHoliday(DateTime.ParseExact("20180610", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo)))
            {
                throw new ApplicationException("Holiday not set");
            }


            try
            {
                holi.EnterHoliday(DateTime.ParseExact("20180428", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo), 2);
                holi.EnterHoliday(DateTime.ParseExact("20180429", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo), 2);
            }
            catch
            {
                //Dublicate enter
            }

            if (!holi.IsWorkDay(DateTime.ParseExact("20180428", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo)))
            {
                throw new ApplicationException("Workday not set");
            }

            holi.ModifyHoliday(DateTime.ParseExact("20180815", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo),
                DateTime.ParseExact("20180816", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo));


            if (holi.IsHoliday(DateTime.ParseExact("20180815", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo)))
            {
                throw new ApplicationException("Holiday not set");
            }

            if (!holi.IsHoliday(DateTime.ParseExact("20180816", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo)))
            {
                throw new ApplicationException("Holiday not set");
            }

            holi.RemoveHoliday(DateTime.ParseExact("20180816", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo));

            if (holi.IsHoliday(DateTime.ParseExact("20180816", "yyyyMMdd", DateTimeFormatInfo.InvariantInfo)))
            {
                throw new ApplicationException("Holiday not set");
            }

            var holidayList = holi.GetHolidaysByYear(2018, HolidayDataType.Holiday);

            var changedWorkdays = holi.GetHolidaysByYear(2018, HolidayDataType.ChangedWeekEnd);
        }
    }
}