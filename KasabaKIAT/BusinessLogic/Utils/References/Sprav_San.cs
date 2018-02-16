using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Utils.References
{
    public class Sprav_San
    {
        public static void Create(ref Sprav_SanViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblSprav_San sanatory = new tblSprav_San();

                int maxId = 0;

                try
                {
                    maxId = db.tblSprav_San.Max(z => z.ID);
                }
                catch(Exception)
                {
                    maxId = 0;
                }

                maxId += 1;

                sanatory.ID = maxId;
                sanatory.Name_San = model.Name_San;
                sanatory.Num_Kod =  model.Num_Kod ;
                sanatory.UserId = model.UserId;
                sanatory.KurortSanID = model.KurortSanId;

                var sanatoryIns = db.tblSprav_San.Add(sanatory);

                db.SaveChanges();

                model.ID = sanatory.ID;
            }
        }

        public static void Modify(Sprav_SanViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var sanatory = db.tblSprav_San.Where(z => z.ID == model.ID);

                if (sanatory.Any())
                {
                    var sanatory1 = sanatory.FirstOrDefault();

                    if (sanatory1 != null)
                    {
                        sanatory1.Name_San = model.Name_San;
                        sanatory1.Num_Kod = model.Num_Kod;
                        sanatory1.UserId = model.UserId;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(Sprav_SanViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var sanatory = db.tblSprav_San.Where(z => z.ID == model.ID);

                if (sanatory.Any())
                {
                    var sanatory1 = sanatory.FirstOrDefault();

                    if (sanatory1 != null)
                    {
                        db.tblSprav_San.Remove(sanatory1);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<Sprav_SanViewModel> Get(Sprav_SanViewModel model)
        {
            List<Sprav_SanViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from sanatory in db.tblSprav_San
                               select new Sprav_SanViewModel
                               {
                                   Name_San = sanatory.Name_San,
                                   Num_Kod = sanatory.Num_Kod ,
                                   UserId = sanatory.UserId
                               };

                list = queryRes.ToList();

            }

            return list;
        }
    }
}