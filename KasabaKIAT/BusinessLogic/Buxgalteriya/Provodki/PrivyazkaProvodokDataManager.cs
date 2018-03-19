using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Buxgalteriya;
using System.Data.Entity.Validation;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki
{
    public class PrivyazkaProvodokDataManager
    {
        public static void Create(ref PrivyazkaProvodokViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                KasabaKIAT.Models.PrivyazkaProvodok reg = new KasabaKIAT.Models.PrivyazkaProvodok();

                reg.JO_REF = model.jo_ref;
                reg.USER_ID = model.user_id;
                reg.OWNER_DEPARTMENTBASE = model.owner_departmentbase;
                reg.PROVODOK_ID = model.provodok_id;

                var planOtrasl = db.PrivyazkaProvodoks.Add(reg);

                try
                {
                    db.SaveChanges();

                    model.id = reg.ID;
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

        public static void Modify(PrivyazkaProvodokViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.PrivyazkaProvodoks.Where(z => z.ID == model.id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.JO_REF = model.jo_ref;
                        reg.USER_ID = model.user_id;
                        reg.OWNER_DEPARTMENTBASE = model.owner_departmentbase;
                        reg.PROVODOK_ID = model.provodok_id;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(PrivyazkaProvodokViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.PrivyazkaProvodoks.Where(z => z.ID == model.id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.PrivyazkaProvodoks.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<PrivyazkaProvodokViewModel> Get(PrivyazkaProvodokViewModel model)
        {
            List<PrivyazkaProvodokViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.PrivyazkaProvodoks
                            select new PrivyazkaProvodokViewModel
                            {
                                jo_ref = bank.JO_REF,
                                user_id = bank.USER_ID,
                                owner_departmentbase = bank.OWNER_DEPARTMENTBASE,
                                provodok_id = bank.PROVODOK_ID,
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}