using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Utils.References
{
    public class Sprav_Org
    {
        public static void Create(ref Sprav_OrgViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblSprav_Org org = new tblSprav_Org();

                org.ID_Ot       = model.ID_Ot       ;
                org.Name_Org    = model.Name_Org    ;
                org.INN         = model.INN         ;
                org.RS          = model.RS          ;
                org.MFO         = model.MFO         ;
                org.Bank        = model.Bank        ;
                org.OKONX       = model.OKONX       ;
                org.Adress      = model.Adress      ;
                org.Telefon     = model.Telefon     ;
                org.Region      = model.Region      ;
                org.DT_OST      = 0      ;
                org.KT_OST      = 0      ;
                org.E_Mail      = model.E_Mail      ;
                org.UserId = model.UserId;
                org.DepartmentBase = model.DepartmentBase;
                org.IS_PRAVSOYUZ = model.IsPravSoyuz;
                org.ParentID = model.ParentId;

                var orgIns = db.tblSprav_Org.Add(org);

                try
                {
                    db.SaveChanges();

                    model.ID = org.ID;
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

        public static void Modify(Sprav_OrgViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var org = db.tblSprav_Org.Where(z => z.ID == model.ID);

                if (org.Any())
                {
                    var org1 = org.FirstOrDefault();

                    if (org1 != null)
                    {
                        org1.ID_Ot = model.ID_Ot;
                        org1.Name_Org = model.Name_Org;
                        org1.INN = model.INN;
                        org1.RS = model.RS;
                        org1.MFO = model.MFO;
                        org1.Bank = model.Bank;
                        org1.OKONX = model.OKONX;
                        org1.Adress = model.Adress;
                        org1.Telefon = model.Telefon;
                        org1.Region = model.Region;
                        //org1.DT_OST = model.DT_OST;
                        //org1.KT_OST = model.KT_OST;
                        org1.E_Mail = model.E_Mail;
                        //org1.UserId = model.UserId;
                        org1.IS_PRAVSOYUZ = model.IsPravSoyuz;
                        org1.ParentID = model.ParentId;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(Sprav_OrgViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var org = db.tblSprav_Org.Where(z => z.ID == model.ID);

                if (org.Any())
                {
                    var org1 = org.FirstOrDefault();

                    if (org1 != null)
                    {
                        db.tblSprav_Org.Remove(org1);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<Sprav_OrgViewModel> Get(Sprav_OrgViewModel model)
        {
            List<Sprav_OrgViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from org in db.tblSprav_Org
                               orderby org.Name_Org
                               select new Sprav_OrgViewModel
                               {
                                   ID = org.ID,
                                   ID_Ot    = org.ID_Ot,
                                   Name_Org = org.Name_Org,
                                   INN      = org.INN,
                                   RS       = org.RS,
                                   MFO      = org.MFO,
                                   Bank     = org.Bank,
                                   OKONX    = org.OKONX,
                                   Adress   = org.Adress,
                                   Telefon  = org.Telefon,
                                   Region   = org.Region,
                                   DT_OST   = org.DT_OST,
                                   KT_OST   = org.KT_OST,
                                   E_Mail   = org.E_Mail,
                                   UserId   = org.UserId,
                                   IsPravSoyuz = org.IS_PRAVSOYUZ,
                                   ParentId = org.ParentID
                               }  ;

                list = queryRes.ToList();

            }

            return list;
        }


        public static List<Sprav_OrgViewModel> GetByDeraprtmentBase(string departmentBase)
        {
            List<Sprav_OrgViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from org in db.tblSprav_Org
                               join otras in db.tblSprav_Otras on org.ID_Ot equals otras.ID_Ot
                               join region in db.tblSprav_Reg on org.Region equals region.CodeReg
                               where org.DepartmentBase == departmentBase 
                               && otras.DepartmentBase == departmentBase
                               orderby org.Name_Org
                               select new Sprav_OrgViewModel
                               {
                                   ID = org.ID,
                                   ID_Ot = org.ID_Ot,
                                   OtraslName = otras.Name_Ot,
                                   Name_Org = org.Name_Org,
                                   INN      = org.INN,
                                   RS       = org.RS,
                                   MFO      = org.MFO,
                                   Bank     = org.Bank,
                                   OKONX    = org.OKONX,
                                   Adress   = org.Adress,
                                   Telefon  = org.Telefon,
                                   Region   = org.Region,
                                   RegionName = region.NameReg,
                                   DT_OST = org.DT_OST,
                                   KT_OST = org.KT_OST,
                                   E_Mail = org.E_Mail,
                                   UserId = org.UserId,
                                   IsPravSoyuz = org.IS_PRAVSOYUZ,
                                   ParentId = org.ParentID
                               };

                list = queryRes.ToList();

            }

            return list;
        }



        public static List<Sprav_OrgViewModel> GetByDeraprtmentBase(string departmentBase,
            string Name, string Inn, int? OtrasId)
        {
            List<Sprav_OrgViewModel> list = null;

            bool IsNameEmpty = false;
            bool IsInnEmpty = false;

            if ((string.IsNullOrEmpty(Name)) || (Name.ToLower().Contains("undefined")))
            {
                IsNameEmpty = true;
            }

            if ((string.IsNullOrEmpty(Inn)) || (Inn.ToLower().Contains("undefined")))
            {
                IsInnEmpty = true;
            }

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from org in db.tblSprav_Org
                               join otras in db.tblSprav_Otras on org.ID_Ot equals otras.ID_Ot
                               join region in db.tblSprav_Reg on org.Region equals region.CodeReg
                               where org.DepartmentBase == departmentBase
                               && otras.DepartmentBase == departmentBase
                               && (org.Name_Org.Contains((IsNameEmpty ? org.Name_Org: Name))) 
                               && (org.INN.Contains((IsInnEmpty ? org.INN : Inn)))
                               && (org.ID_Ot == (OtrasId.HasValue ? OtrasId: org.ID_Ot))
                               orderby org.Name_Org
                               select new Sprav_OrgViewModel
                               {
                                   ID = org.ID,
                                   ID_Ot = org.ID_Ot,
                                   OtraslName = otras.Name_Ot,
                                   Name_Org = org.Name_Org,
                                   INN = org.INN,
                                   RS = org.RS,
                                   MFO = org.MFO,
                                   Bank = org.Bank,
                                   OKONX = org.OKONX,
                                   Adress = org.Adress,
                                   Telefon = org.Telefon,
                                   Region = org.Region,
                                   RegionName = region.NameReg,
                                   DT_OST = org.DT_OST,
                                   KT_OST = org.KT_OST,
                                   E_Mail = org.E_Mail,
                                   UserId = org.UserId,
                                   IsPravSoyuz = org.IS_PRAVSOYUZ,
                                   ParentId = org.ParentID
                               };

                list = queryRes.ToList();

            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static List<Sprav_OrgViewModel> GetByDeraprtmentBase(string departmentBase, long Id)
        {
            List<Sprav_OrgViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from org in db.tblSprav_Org
                               join otras in db.tblSprav_Otras on org.ID_Ot equals otras.ID_Ot
                               join region in db.tblSprav_Reg on org.Region equals region.CodeReg
                               where org.DepartmentBase == departmentBase
                               && otras.DepartmentBase == departmentBase
                               && org.ID == Id
                               orderby org.Name_Org
                               select new Sprav_OrgViewModel
                               {
                                   ID = org.ID,
                                   ID_Ot = org.ID_Ot,
                                   OtraslName = otras.Name_Ot,
                                   Name_Org = org.Name_Org,
                                   INN = org.INN,
                                   RS = org.RS,
                                   MFO = org.MFO,
                                   Bank = org.Bank,
                                   OKONX = org.OKONX,
                                   Adress = org.Adress,
                                   Telefon = org.Telefon,
                                   Region = org.Region,
                                   RegionName = region.NameReg,
                                   DT_OST = org.DT_OST,
                                   KT_OST = org.KT_OST,
                                   E_Mail = org.E_Mail,
                                   UserId = org.UserId,
                                   IsPravSoyuz = org.IS_PRAVSOYUZ,
                                   ParentId = org.ParentID
                               };

                list = queryRes.ToList();

            }

            return list;
        }
    }
}