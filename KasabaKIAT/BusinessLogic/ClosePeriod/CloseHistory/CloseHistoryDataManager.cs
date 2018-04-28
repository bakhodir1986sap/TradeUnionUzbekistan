using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.ClosePeriod.CloseHistory
{
    public class CloseHistoryDataManager
    {
        public static void Create(ref CloseHistoryViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblCloseDayHistory reg = new tblCloseDayHistory();

                reg.DEPARTMENT_CODE = model.department_code;
                reg.CLOSED_DATE = model.closed_date;
                reg.TIMESTAMP = DateTime.Now;
                reg.USER_ID = model.user_id;
                reg.REMARK = model.remark;

                var planOtrasl = db.tblCloseDayHistories.Add(reg);

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

        public static void Modify(CloseHistoryViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tblCloseDayHistories.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.DEPARTMENT_CODE = model.department_code;
                        reg.CLOSED_DATE = model.closed_date;
                        reg.TIMESTAMP = DateTime.Now;
                        reg.USER_ID = model.user_id; ;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(CloseHistoryViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tblCloseDayHistories.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblCloseDayHistories.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<CloseHistoryViewModel> Get(CloseHistoryViewModel model)
        {
            List<CloseHistoryViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tblCloseDayHistories
                            where bank.DEPARTMENT_CODE == model.department_code
                            select new CloseHistoryViewModel
                            {
                                rec_id = bank.REC_ID,
                                department_code = bank.DEPARTMENT_CODE,
                                timestamp = bank.TIMESTAMP,
                                closed_date = bank.CLOSED_DATE,
                                remark = bank.REMARK,
                                user_id = bank.USER_ID
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}