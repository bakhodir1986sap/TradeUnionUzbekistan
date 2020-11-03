using KasabaKIAT.Models;
using System.Collections.Generic;
using System.Linq;

namespace KasabaKiat.BusinessLogic.Utils.References
{
    public class Sprav_Vil
    {
        public static void Create(ref Sprav_VilViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblSprav_Vil viloyat = new tblSprav_Vil();

                viloyat.FullName= model.FullName;
                viloyat.ID_Code = model.ID_Code ;
                viloyat.UserId = model.UserId;

                var viloyatIns = db.tblSprav_Vil.Add(viloyat);

                model.ID = viloyatIns.ID;

                db.SaveChanges();
            }
        }

        public static void Modify(Sprav_VilViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var viloyat = db.tblSprav_Vil.Where(z => z.ID == model.ID);

                if (viloyat.Any())
                {
                    var viloyat1 = viloyat.FirstOrDefault();

                    if (viloyat1 != null)
                    {
                        viloyat1.FullName = model.FullName;
                        viloyat1.ID_Code  = model.ID_Code;
                        viloyat1.UserId   = model.UserId;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(Sprav_VilViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var viloyat = db.tblSprav_Vil.Where(z => z.ID == model.ID);

                if (viloyat.Any())
                {
                    var viloyat1 = viloyat.FirstOrDefault();

                    if (viloyat1 != null)
                    {
                        db.tblSprav_Vil.Remove(viloyat1);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<Sprav_VilViewModel> Get(Sprav_VilViewModel model)
        {
            List<Sprav_VilViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from viloyat in db.tblSprav_Vil
                               select new Sprav_VilViewModel
                               {
                                    FullName = viloyat.FullName,
                                    ID_Code  = viloyat.ID_Code ,
                                    UserId   = viloyat.UserId  
                               };

                list = queryRes.ToList();

            }

            return list;
        }
    }
}