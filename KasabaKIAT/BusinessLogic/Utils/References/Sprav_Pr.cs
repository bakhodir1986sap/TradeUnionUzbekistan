using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Utils.References
{
    public class Sprav_Pr
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Create(ref Sprav_PrViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblSprav_Pr procent = new tblSprav_Pr();

                procent.Name_Proc= model.Name_Proc;
                procent.Num      = model.Num      ;
                procent.UserId = model.UserId     ;

                var procentIns = db.tblSprav_Pr.Add(procent);

                model.ID = procentIns.ID;

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Modify(Sprav_PrViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var procent = db.tblSprav_Pr.Where(z => z.ID == model.ID);

                if (procent.Any())
                {
                    var procent1 = procent.FirstOrDefault();

                    if (procent1 != null)
                    {
                        procent1.Name_Proc = model.Name_Proc;
                        procent1.Num = model.Num;
                        procent1.UserId = model.UserId;
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Remove(Sprav_PrViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var procent = db.tblSprav_Pr.Where(z => z.ID == model.ID);

                if (procent.Any())
                {
                    var procent1 = procent.FirstOrDefault();

                    if (procent1 != null)
                    {
                        db.tblSprav_Pr.Remove(procent1);
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<Sprav_PrViewModel> Get(Sprav_PrViewModel model)
        {
            List<Sprav_PrViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from procent in db.tblSprav_Pr
                               select new Sprav_PrViewModel
                               {
                                   ID = procent.ID,
                                   Name_Proc = procent.Name_Proc,
                                   Num = procent.Num,
                                   UserId = procent.UserId
                               };

                list = queryRes.ToList();

            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Sprav_PrViewModel> Get()
        {
            List<Sprav_PrViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from procent in db.tblSprav_Pr
                               select new Sprav_PrViewModel
                               {
                                   ID           = procent.ID,
                                   Name_Proc    = procent.Name_Proc
                                   //,Num = procent.Num,
                                   //UserId = procent.UserId
                               };

                list = queryRes.ToList();

            }

            return list;
        }
    }
}