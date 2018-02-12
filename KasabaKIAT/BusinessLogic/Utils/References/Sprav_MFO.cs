using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Utils.References
{
    public class Sprav_MFO
    {
        public static void Create(ref Sprav_MFOViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblSprav_MFO mfo = new tblSprav_MFO();

                mfo.Code_MFO = model.Code_MFO;
                mfo.Name_MFO = model.Name_MFO;
                mfo.Adres_MFO = model.Adres_MFO;
                mfo.Data_Act = model.Data_Act;
                mfo.Data_Cls = model.Data_Cls;
                mfo.UserId = model.UserId;                

                var mfoIns = db.tblSprav_MFO.Add(mfo);

                model.ID = mfoIns.ID;

                db.SaveChanges();
            }
        }

        public static void Modify(Sprav_MFOViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var mfo = db.tblSprav_MFO.Where(z => z.ID == model.ID);

                if (mfo.Any())
                {
                    var mfo1 = mfo.FirstOrDefault();

                    if (mfo1 != null)
                    {
                        mfo1.Code_MFO = model.Code_MFO;
                        mfo1.Name_MFO = model.Name_MFO;
                        mfo1.Adres_MFO = model.Adres_MFO;
                        mfo1.Data_Act = model.Data_Act;
                        mfo1.Data_Cls = model.Data_Cls;
                        mfo1.UserId = model.UserId;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(Sprav_MFOViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var mfo = db.tblSprav_MFO.Where(z => z.ID == model.ID);

                if (mfo.Any())
                {
                    var mfo1 = mfo.FirstOrDefault();

                    if (mfo1 != null)
                    {
                        db.tblSprav_MFO.Remove(mfo1);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<Sprav_MFOViewModel> Get(Sprav_MFOViewModel model)
        {
            List<Sprav_MFOViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from mfo in db.tblSprav_MFO
                            select new Sprav_MFOViewModel
                            {
                                Code_MFO =  mfo.Code_MFO,
                                Name_MFO =  mfo.Name_MFO,
                                Adres_MFO = mfo.Adres_MFO,
                                Data_Act =  mfo.Data_Act,
                                Data_Cls =  mfo.Data_Cls,
                                UserId =    mfo.UserId
                            };

                list = query.ToList();
            }

            return list;
        }

        public static string GetmfoById(long Id)
        {
            string res = string.Empty;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var mfo = db.tblSprav_MFO.Where(z => z.ID == Id);

                if (mfo.Any())
                {
                    res = mfo.FirstOrDefault().Name_MFO;
                }
            }

            return res;
        }


    }
}