using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.ClosePeriod
{
    public class CloseDayDataManager
    {
        public static void Create(ref CloseDayViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblCloseDay reg = new tblCloseDay();

                reg.DEPARTMENT_CODE = model.department_code;
                reg.PREVDATE = model.prevdate.Value.Date;
                reg.CURDAY = model.curday.Value.Date;
                reg.NEXTDAY = model.nextday.Value.Date;

                var planOtrasl = db.tblCloseDays.Add(reg);

                try
                {
                    db.SaveChanges();

                    model.rec_id = reg.REC_ID;
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
        }

        public static void Modify(CloseDayViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tblCloseDays.Where(z => z.DEPARTMENT_CODE == model.department_code);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.PREVDATE = model.prevdate.Value.Date;
                        reg.CURDAY = model.curday.Value.Date;
                        reg.NEXTDAY = model.nextday.Value.Date;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(CloseDayViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tblCloseDays.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblCloseDays.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<CloseDayViewModel> Get(CloseDayViewModel model)
        {
            List<CloseDayViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tblCloseDays
                            where bank.DEPARTMENT_CODE == model.department_code
                            select new CloseDayViewModel
                            {
                                rec_id = bank.REC_ID,
                                department_code = bank.DEPARTMENT_CODE,
                                prevdate = bank.PREVDATE,
                                curday = bank.CURDAY,
                                nextday = bank.NEXTDAY
                            };

                list = query.ToList();
            }

            return list;
        }


        public static List<CloseDayViewModel> GetAll()
        {
            List<CloseDayViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tblCloseDays
                            select new CloseDayViewModel
                            {
                                rec_id = bank.REC_ID,
                                department_code = bank.DEPARTMENT_CODE,
                                prevdate = bank.PREVDATE,
                                curday = bank.CURDAY,
                                nextday = bank.NEXTDAY
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}