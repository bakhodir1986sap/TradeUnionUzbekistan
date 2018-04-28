using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.ClosePeriod.Holydays
{
    public class HolidayDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Create(ref HolidayViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblHoliday reg = new tblHoliday();

                reg.SET_DATE = model.set_date;
                reg.DATE_TYPE = (short)model.date_type;
                reg.STATUS = (short)model.status;

                var planOtrasl = db.tblHolidays.Add(reg);

                db.SaveChanges();

                model.rec_id = reg.REC_ID;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Modify(HolidayViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var holidays = db.tblHolidays.Where(z => z.REC_ID == model.rec_id);

                if (holidays.Any())
                {
                    var reg = holidays.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.SET_DATE = model.set_date;
                        reg.DATE_TYPE = (short)model.date_type;
                        reg.STATUS = (short)model.status;
                    }
                }

                db.SaveChanges();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void ModifyByDate(DateTime oldDate, HolidayViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var holidays = db.tblHolidays.Where(z => z.SET_DATE == oldDate);

                if (holidays.Any())
                {
                    var reg = holidays.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.SET_DATE = model.set_date;
                        //reg.DATE_TYPE = (short)model.date_type;
                        //reg.STATUS = (short)model.status;
                    }

                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Remove(HolidayViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tblHolidays.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblHolidays.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void RemoveByDate(DateTime remDate)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tblHolidays.Where(z => z.SET_DATE == remDate);

                if (planOblast.Any())
                {
                    foreach(var reg in planOblast)
                    {
                        if (reg != null)
                        {
                            db.tblHolidays.Remove(reg);
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<HolidayViewModel> Get(HolidayViewModel model)
        {
            List<HolidayViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tblHolidays
                            select new HolidayViewModel
                            {
                                rec_id = bank.REC_ID,
                                set_date = bank.SET_DATE,
                                date_type = (HolidayDataType)bank.DATE_TYPE,
                                status = (HolidayStatus)bank.STATUS
                            };

                list = query.ToList();
            }

            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<HolidayViewModel> GetByYear(int year, HolidayDataType dataType)
        {
            List<HolidayViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tblHolidays
                            where (bank.SET_DATE.Value.Year == year && bank.DATE_TYPE == (short)dataType)
                            select new HolidayViewModel
                            {
                                rec_id = bank.REC_ID,
                                set_date = bank.SET_DATE,
                                date_type = (HolidayDataType)bank.DATE_TYPE,
                                status = (HolidayStatus)bank.STATUS
                            };

                list = query.ToList();
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsHoliday(DateTime date)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tblHolidays
                            where bank.SET_DATE == date && bank.DATE_TYPE == (short)HolidayDataType.Holiday
                            select new HolidayViewModel
                            {
                                rec_id = bank.REC_ID,
                                set_date = bank.SET_DATE,
                                date_type = (HolidayDataType)bank.DATE_TYPE,
                                status = (HolidayStatus)bank.STATUS
                            };

                var list = query.ToList();

                if (list.Any())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsWorkDay(DateTime date)
        {

            if ((DateTime.MinValue >= date) || (date >= DateTime.MaxValue))
            {
                return false;
            }

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tblHolidays
                            where bank.SET_DATE == date 
                            select new HolidayViewModel
                            {
                                rec_id = bank.REC_ID,
                                set_date = bank.SET_DATE,
                                date_type = (HolidayDataType)bank.DATE_TYPE,
                                status = (HolidayStatus)bank.STATUS
                            };

                var list = query.ToList();

                if (list.Any())
                {
                    var holiday = list.FirstOrDefault();

                    if (holiday != null)
                    {
                        if (holiday.date_type == HolidayDataType.Holiday)
                        {
                            return false;
                        }
                        else if (holiday.date_type == HolidayDataType.ChangedWeekEnd)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    var dayWeek = date.DayOfWeek;

                    if ((dayWeek == DayOfWeek.Saturday) || (dayWeek == DayOfWeek.Sunday))
                    {
                        return false;
                    }
                    else if ((dayWeek == DayOfWeek.Monday) || (dayWeek == DayOfWeek.Tuesday)
                        || (dayWeek == DayOfWeek.Wednesday) || (dayWeek == DayOfWeek.Thursday) || (dayWeek == DayOfWeek.Friday))
                    {
                        return true;
                    }
                }

            }

            return false;
        }
    }
}