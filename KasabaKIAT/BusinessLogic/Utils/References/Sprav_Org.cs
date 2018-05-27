using KasabaKIAT.BusinessLogic.Structure;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Utils.References
{
    /// <summary>
    /// 
    /// </summary>
    public class Sprav_Org
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
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
                org.PRAVORGLEVEL = model.PravSoyuzLevel;
                org.DIRECTOR = model.Director;
                org.BUXGALTER = model.Buxgalter;
                org.DOVERENNOE_LISO = model.Doverennoe_liso;
                org.ORIG_INN = model.orig_inn;
                org.OWNER_CODE = GenerateDepartmentCode(model.ID_Ot.Value
                                                        , model.Region.Value,
                                                        model.PravSoyuzLevel.Value,
                                                        0);

                if (!string.IsNullOrEmpty(org.OWNER_CODE))
                {
                    var isDepExists = db.t_departmentbase.Where(r => r.code == org.OWNER_CODE);

                    if (!isDepExists.Any())
                    {
                        if ((int)KIAT_STRUCT.TumanBranch == model.PravSoyuzLevel)
                        {
                            //skip
                        }
                        else if ((int)KIAT_STRUCT.PPO == model.PravSoyuzLevel)
                        {
                            t_departmentbase dd = new t_departmentbase
                            {
                                inn = model.INN,
                                le_inn = model.orig_inn,
                                parent_code = model.DepartmentBase,
                                code = org.OWNER_CODE,
                                name_ru = model.Name_Org,
                                name_uz = model.Name_Org,
                                dtype = "Bkut",
                                version = 0
                            };

                            db.t_departmentbase.Add(dd);
                        }
                    }
                }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otrasl"></param>
        /// <param name="region"></param>
        /// <param name="pravsoyuzLevel"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        private static string GenerateDepartmentCode(int otrasl = 0, int region = 0 , int pravsoyuzLevel = 0, long Id = 0)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var structureKiat = new StructureOfKiat();

                if (Id == 0)
                {
                    Id = db.tblSprav_Org.Max(z => z.ID) + 1;
                }

                var branches = db.tblSprav_Otras.Where(z => z.ID_Ot == otrasl);

                if (branches.Any())
                {
                    var branch = branches.First().BRANCH_CODE;

                    KIAT_STRUCT str = KIAT_STRUCT.PPO;

                    if (pravsoyuzLevel == (int)KIAT_STRUCT.TumanBranch)
                    {
                        str = KIAT_STRUCT.TumanBranch;
                    }
                    else if (pravsoyuzLevel == (int)KIAT_STRUCT.PPO)
                    {
                        str = KIAT_STRUCT.PPO;
                    }

                    var regions = db.tblSprav_Reg.Where(z => z.CodeReg == region);

                    string soatoCode = string.Empty;

                    if (regions.Any())
                    {
                        soatoCode = regions.First().SOATO_CODE;
                    }
                    else
                    {
                        throw new ApplicationException("Region is invalid");
                    }

                    return structureKiat.GenerateDepartmentCode(str, branch, soatoCode, Id);
                }
                else
                {
                    throw new ApplicationException("Otrasl info not found");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
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
                        org1.PRAVORGLEVEL = model.PravSoyuzLevel;
                        org1.DIRECTOR = model.Director;
                        org1.BUXGALTER = model.Buxgalter;
                        org1.DOVERENNOE_LISO = model.Doverennoe_liso;
                        org1.ORIG_INN = model.orig_inn;
                        org1.OWNER_CODE = GenerateDepartmentCode(model.ID_Ot.Value
                                        , model.Region.Value,
                                        model.PravSoyuzLevel.Value,
                                        model.ID);


                        if (!string.IsNullOrEmpty(org1.OWNER_CODE))
                        {
                            var isDepExists = db.t_departmentbase.Where(r => r.code == org1.OWNER_CODE);

                            if (!isDepExists.Any())
                            {
                                if ((int)KIAT_STRUCT.TumanBranch == model.PravSoyuzLevel)
                                {
                                    //skip
                                }
                                else if ((int)KIAT_STRUCT.PPO == model.PravSoyuzLevel)
                                {
                                    t_departmentbase dd = new t_departmentbase
                                    {
                                        inn = model.INN,
                                        le_inn = model.orig_inn,
                                        parent_code = model.DepartmentBase,
                                        code = org1.OWNER_CODE,
                                        name_ru = model.Name_Org,
                                        name_uz = model.Name_Org,
                                        dtype = "Bkut",
                                        version = 0
                                    };

                                    db.t_departmentbase.Add(dd);
                                }
                            }
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                                   ParentId = org.ParentID,
                                   PravSoyuzLevel = org.PRAVORGLEVEL,
                                   Director = org.DIRECTOR,
                                   Buxgalter = org.BUXGALTER,
                                   Doverennoe_liso = org.DOVERENNOE_LISO,
                                   orig_inn = org.ORIG_INN,
                                   owner_code = org.OWNER_CODE
                               }  ;

                list = queryRes.ToList();

            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
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
                                   ParentId = org.ParentID,
                                   PravSoyuzLevel = org.PRAVORGLEVEL,
                                   Director = org.DIRECTOR,
                                   Buxgalter = org.BUXGALTER,
                                   Doverennoe_liso = org.DOVERENNOE_LISO,
                                   orig_inn = org.ORIG_INN,
                                   owner_code = org.OWNER_CODE
                               };

                list = queryRes.ToList();

            }

            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <param name="Name"></param>
        /// <param name="Inn"></param>
        /// <param name="OtrasId"></param>
        /// <returns></returns>
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
                                   ParentId = org.ParentID,
                                   PravSoyuzLevel = org.PRAVORGLEVEL,
                                   Director = org.DIRECTOR,
                                   Buxgalter = org.BUXGALTER,
                                   Doverennoe_liso = org.DOVERENNOE_LISO,
                                   orig_inn = org.ORIG_INN,
                                   owner_code = org.OWNER_CODE
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
                                   ParentId = org.ParentID,
                                   PravSoyuzLevel = org.PRAVORGLEVEL,
                                   Director = org.DIRECTOR,
                                   Buxgalter = org.BUXGALTER,
                                   Doverennoe_liso = org.DOVERENNOE_LISO,
                                   orig_inn = org.ORIG_INN,
                                   owner_code = org.OWNER_CODE
                               };

                list = queryRes.ToList();

                foreach(var org in list)
                {
                    if (org.ParentId.HasValue)
                    {
                        var orgName = db.tblSprav_Org.Where(z => z.ID == org.ParentId);

                        if (orgName.Any())
                        {
                            org.ParentName = orgName.First().Name_Org;
                        }
                        
                    }
                }

            }

            return list;
        }
    }
}