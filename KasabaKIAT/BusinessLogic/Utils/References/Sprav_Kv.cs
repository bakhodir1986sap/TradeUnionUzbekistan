using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Utils.References
{
    public class Sprav_Kv
    {
        public static void Create(ref Sprav_KvViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblSprav_Kv kvartal = new tblSprav_Kv();

                kvartal.Name_Kv = model.Name_Kv;
                kvartal.UserId = model.UserId;

                var kvartalIns = db.tblSprav_Kv.Add(kvartal);

                model.ID = kvartalIns.ID;

                db.SaveChanges();
            }
        }

        public static void Modify(Sprav_KvViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var kvartal = db.tblSprav_Kv.Where(z => z.ID == model.ID);

                if (kvartal.Any())
                {
                    var kvartal1 = kvartal.FirstOrDefault();

                    if (kvartal1 != null)
                    {
                        kvartal1.Name_Kv = model.Name_Kv;
                        kvartal1.UserId = model.UserId;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(Sprav_KvViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var kvartal = db.tblSprav_Kv.Where(z => z.ID == model.ID);

                if (kvartal.Any())
                {
                    var kvartal1 = kvartal.FirstOrDefault();

                    if (kvartal1 != null)
                    {
                        db.tblSprav_Kv.Remove(kvartal1);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<Sprav_KvViewModel> Get(Sprav_KvViewModel model)
        {
            List<Sprav_KvViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from kvartal in db.tblSprav_Kv
                            select new Sprav_KvViewModel
                            {
                                ID = kvartal.ID,
                                Name_Kv = kvartal.Name_Kv,
                                UserId = kvartal.UserId
                            };

                list = query.ToList();
            }

            return list;
        }

        public static string GetKvartalById(long Id)
        {
            string res = string.Empty;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var face = db.tblSprav_Kv.Where(z => z.ID == Id);

                if (face.Any())
                {
                    res = face.FirstOrDefault().Name_Kv;
                }
            }

            return res;
        }


    }
}