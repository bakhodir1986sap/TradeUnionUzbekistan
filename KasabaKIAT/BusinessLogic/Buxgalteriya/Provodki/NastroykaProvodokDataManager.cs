using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Buxgalteriya;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki
{
    public class NastroykaProvodokDataManager
    {
        public static void Create(ref NastroykiProvodokViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                provodok reg = new provodok();

                reg.DEPARTMENT_BASE = model.department_base;
                reg.USER_ID = model.user_id;
                reg.description = model.description;
                reg.score_begin = model.score_begin;
                reg.score_end = model.score_end;

                var planOtrasl = db.provodok.Add(reg);

                try
                {
                    db.SaveChanges();

                    model.id = reg.id;
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

        public static void Modify(NastroykiProvodokViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.provodok.Where(z => z.id == model.id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.DEPARTMENT_BASE = model.department_base;
                        reg.USER_ID = model.user_id;
                        reg.description = model.description;
                        reg.score_begin = model.score_begin;
                        reg.score_end = model.score_end;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(NastroykiProvodokViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.provodok.Where(z => z.id == model.id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.provodok.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<NastroykiProvodokViewModel> Get(NastroykiProvodokViewModel model)
        {
            List<NastroykiProvodokViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.provodok
                            where bank.DEPARTMENT_BASE == model.department_base
                            select new NastroykiProvodokViewModel
                            {
                                department_base = bank.DEPARTMENT_BASE,
                                user_id = bank.USER_ID,
                                description = bank.description,
                                score_begin = bank.score_begin,
                                score_end = bank.score_end,
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}