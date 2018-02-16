using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.RealizasiyaPutyovok
{
    public class ViplNumGenDataManager
    {
        public static void Create(ref ViplNumGenViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblVipl_NumGEn gen = new tblVipl_NumGEn();

                gen.DEPARTMENT_BASE = model.department_base;
                gen.NUMDOC_GEN = model.numdoc_gen;
                gen.USER_ID = model.user_id;
                gen.YEAR = model.year;

                var pst = db.tblVipl_NumGEn.Add(gen);

                db.SaveChanges();

                model.id = gen.ID;
            }
        }

        public static void Modify(ViplNumGenViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var gen = db.tblVipl_NumGEn.Where(z => z.ID == model.id);

                if (gen.Any())
                {
                    var gen1 = gen.FirstOrDefault();

                    if (gen1 != null)
                    {
                        gen1.DEPARTMENT_BASE = model.department_base;
                        gen1.NUMDOC_GEN = model.numdoc_gen;
                        gen1.USER_ID = model.user_id;
                        gen1.YEAR = model.year;
                    }
                }

                db.SaveChanges();
            }
        }


        public static void Increase(ViplNumGenViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var gen = db.tblVipl_NumGEn.Where(z => z.DEPARTMENT_BASE == model.department_base && 
                z.YEAR == model.year);

                if (gen.Any())
                {
                    var gen1 = gen.FirstOrDefault();

                    if (gen1 != null)
                    {
                        gen1.NUMDOC_GEN = model.numdoc_gen;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(ViplNumGenViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var vipl = db.tblVipl_NumGEn.Where(z => z.ID == model.id);

                if (vipl.Any())
                {
                    var reg = vipl.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblVipl_NumGEn.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<ViplNumGenViewModel> Get(ViplNumGenViewModel model)
        {
            List<ViplNumGenViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from vipl in db.tblVipl_NumGEn
                            where vipl.DEPARTMENT_BASE == model.department_base &&
                            vipl.YEAR == model.year
                            select new ViplNumGenViewModel
                            {
                                department_base    = model.department_base,
                                numdoc_gen         = model.numdoc_gen,
                                user_id            = model.user_id,
                                year               = model.year
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}