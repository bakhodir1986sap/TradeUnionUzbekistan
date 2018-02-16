using KasabaKiat.ViewModel.Putyovki;
using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Bron
{
    public class BronDataManager
    {
        public static void Create(BronViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblBron br = new tblBron();

                br.ID_San = model.ID_San;
                br.Num_Put = model.Num_Put;
                br.Prem = model.Prem;
                br.Uchet = true;
                br.ExpireDate = model.ExpireDate;
                br.UserId = model.UserId;
                br.DepartmentBase = model.DepartmentBase;

                var bb = db.tblBron.Add(br);

                model.Id = bb.ID;

                db.SaveChanges();
            }
       }

        public static void Update(BronViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var bron = from tblbr in db.tblBron
                           where tblbr.ID == model.Id
                           select tblbr;

                if (bron.Any())
                {
                    var bronP = bron.FirstOrDefault();

                    if (bronP == null)
                    {
                        bronP.ID_San = model.ID_San;
                        bronP.Num_Put = model.Num_Put;
                        bronP.Prem = model.Prem;
                        bronP.ExpireDate = model.ExpireDate;

                    }
                }

                db.SaveChanges();
            }
        }

        public static void EnableExistingBron(long Id)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var bron = from tblbr in db.tblBron
                           where tblbr.ID == Id
                           select tblbr;

                if (bron.Any())
                {
                    var bronP = bron.FirstOrDefault();

                    if (bronP == null)
                    {
                        bronP.Uchet = true;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void DisableExistingBron(long Id)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var bron = from tblbr in db.tblBron
                           where tblbr.ID == Id
                           select tblbr;

                if (bron.Any())
                {
                    var bronP = bron.FirstOrDefault();

                    if (bronP == null)
                    {
                        bronP.Uchet = false;
                    }
                }

                db.SaveChanges();
            }
        }


        public static void ChangeExpiredDate(BronViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var bron = from tblbr in db.tblBron
                           where tblbr.ID == model.Id
                           select tblbr;

                if (bron.Any())
                {
                    var bronP = bron.FirstOrDefault();

                    if (bronP == null)
                    {
                        bronP.ExpireDate = model.ExpireDate;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(BronViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var bron = from tblbr in db.tblBron
                           where tblbr.ID == model.Id
                           select tblbr;

                if (bron.Any())
                {
                    var bronP = bron.FirstOrDefault();

                    if (bronP == null)
                    {
                        db.tblBron.Remove(bronP);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<BronViewModel> GetBronByPutyovka(string aNumPut, int aSanId)
        {
            List<BronViewModel> list= null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from bron in db.tblBron
                               where
                               bron.ID_San == aSanId && 
                               bron.Num_Put == aNumPut && 
                               bron.Uchet == true
                               select new BronViewModel
                               {
                                   Id = bron.ID,
                                   ID_San = bron.ID_San,
                                   SanatoryName = bron.tblSprav_San.Name_San,
                                   Num_Put = bron.Num_Put,
                                   Prem = bron.Prem,
                                   Uchet = bron.Uchet,
                                   ExpireDate = bron.ExpireDate,
                                   UserId = bron.UserId,
                                   UserName = bron.users.user_name,
                                   DepartmentBase = bron.DepartmentBase
                              };

                list = queryRes.ToList();
            }

            return list;
        }


        public static List<BronViewModel> GetBronByPutyovkaHistory(string aNumPut, int aSanId)
        {
            List<BronViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from bron in db.tblBron
                               where
                               bron.ID_San == aSanId &&
                               bron.Num_Put == aNumPut 
                               select new BronViewModel
                               {
                                   Id = bron.ID,
                                   ID_San = bron.ID_San,
                                   SanatoryName = bron.tblSprav_San.Name_San,
                                   Num_Put = bron.Num_Put,
                                   Prem = bron.Prem,
                                   Uchet = bron.Uchet,
                                   ExpireDate = bron.ExpireDate,
                                   UserId = bron.UserId,
                                   UserName = bron.users.user_name,
                                   DepartmentBase = bron.DepartmentBase
                               };

                list = queryRes.ToList();
            }

            return list;
        }


        public static List<BronViewModel> GetBronBySanatory(int aSanId)
        {
            List<BronViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from bron in db.tblBron
                               where bron.ID_San == aSanId && bron.Uchet == true
                               select new BronViewModel
                               {
                                   Id = bron.ID,
                                   ID_San = bron.ID_San,
                                   SanatoryName = bron.tblSprav_San.Name_San,
                                   Num_Put = bron.Num_Put,
                                   Prem = bron.Prem,
                                   Uchet = bron.Uchet,
                                   ExpireDate = bron.ExpireDate,
                                   UserId = bron.UserId,
                                   UserName = bron.users.user_name,
                                   DepartmentBase = bron.DepartmentBase
                               };

                list = queryRes.ToList();
            }

            return list;
        }


        public static List<BronViewModel> GetBronAllActive()
        {
            List<BronViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from bron in db.tblBron
                               where bron.Uchet == true
                               select new BronViewModel
                               {
                                   Id = bron.ID,
                                   ID_San = bron.ID_San,
                                   SanatoryName = bron.tblSprav_San.Name_San,
                                   Num_Put = bron.Num_Put,
                                   Prem = bron.Prem,
                                   Uchet = bron.Uchet,
                                   ExpireDate = bron.ExpireDate,
                                   UserId = bron.UserId,
                                   UserName = bron.users.user_name,
                                   DepartmentBase = bron.DepartmentBase
                               };

                list = queryRes.ToList();
            }

            return list;
        }


        public static void AutomaticDisableBronsByExpiredDate()
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryList = db.tblBron.Where(z => z.Uchet == true);

                if (queryList.Any())
                {
                    foreach(var bron in queryList)
                    {
                        if (bron.ExpireDate <= DateTime.Now)
                        {
                            DisableExistingBron(bron.ID);
                        }
                    }
                }
            }
        }
    }
}