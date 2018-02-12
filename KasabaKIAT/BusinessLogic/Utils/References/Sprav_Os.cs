using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Utils.References
{
    public class Sprav_Os
    {
        public static void Create(ref Sprav_OsViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblSprav_Os os = new tblSprav_Os();

                os.Name_Os = model.Name_Os;
                os.UserId = model.UserId;

                var osIns = db.tblSprav_Os.Add(os);

                model.ID = osIns.ID;

                db.SaveChanges();
            }
        }

        public static void Modify(Sprav_OsViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var os = db.tblSprav_Os.Where(z => z.ID == model.ID);

                if (os.Any())
                {
                    var os1 = os.FirstOrDefault();

                    if (os1 != null)
                    {
                        os1.Name_Os = model.Name_Os;
                        os1.UserId = model.UserId;
                    }
                }

                db.SaveChanges();
            }
        }
        public static void Remove(Sprav_OsViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var os = db.tblSprav_Os.Where(z => z.ID == model.ID);

                if (os.Any())
                {
                    var os1 = os.FirstOrDefault();

                    if (os1 != null)
                    {
                        db.tblSprav_Os.Remove(os1);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<Sprav_OsViewModel> Get(Sprav_OsViewModel model)
        {
            List<Sprav_OsViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from os in db.tblSprav_Os
                               select new Sprav_OsViewModel
                               {
                                   Name_Os = os.Name_Os,
                                   UserId = os.UserId
                               };

                list = queryRes.ToList();

            }

            return list;
        }
    }
}