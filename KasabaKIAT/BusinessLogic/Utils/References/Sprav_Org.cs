using KasabaKIAT.BusinessLogic.Buxgalteriya.GlavnayaKniga;
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
        private const int OthersBranch = -111;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Create(ref Sprav_OrgViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblSprav_Org org = new tblSprav_Org();

                org.ID_Ot = model.ID_Ot;
                org.Name_Org = model.Name_Org;
                org.INN = model.INN;
                org.RS = model.RS;
                org.MFO = model.MFO;
                org.Bank = model.Bank;
                org.OKONX = model.OKONX;
                org.Adress = model.Adress;
                org.Telefon = model.Telefon;
                org.Region = model.Region;
                org.DT_OST = 0;
                org.KT_OST = 0;
                org.E_Mail = model.E_Mail;
                org.UserId = model.UserId;
                org.DepartmentBase = model.DepartmentBase;
                org.IS_PRAVSOYUZ = model.IsPravSoyuz;
                org.ParentID = model.ParentId;
                org.PRAVORGLEVEL = model.PravSoyuzLevel;
                org.DIRECTOR = model.Director;
                org.BUXGALTER = model.Buxgalter;
                org.DOVERENNOE_LISO = model.Doverennoe_liso;
                org.ORIG_INN = model.orig_inn;
                org.DB_BAL = 0;
                org.CT_BAL = 0;
                org.FACE_TYPE = 0;
                org.OWNER_CODE = model.ID_Ot == OthersBranch ? null : GenerateDepartmentCode(model.ID_Ot.Value
                                , model.Region.Value,
                                model.PravSoyuzLevel.Value,
                                model.ID);
                org.TIMESTAMP_CH = DateTime.Now;
                org.TO_DEPCODE = model.to_depcode;

                if (!string.IsNullOrEmpty(org.OWNER_CODE))
                {
                    var isDepExists = db.t_departmentbase.Where(r => r.code == org.OWNER_CODE);

                    if (!isDepExists.Any())
                    {
                        if ((int)KIAT_STRUCT.TumanBranch == model.PravSoyuzLevel)
                        {
                            t_departmentbase dd = new t_departmentbase
                            {
                                inn = model.INN,
                                le_inn = model.orig_inn,
                                parent_code = model.DepartmentBase,
                                code = org.OWNER_CODE,
                                name_ru = model.Name_Org,
                                name_uz = model.Name_Org,
                                dtype = "Department",
                                version = 0
                            };

                            db.t_departmentbase.Add(dd);
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
        private static string GenerateDepartmentCode(int otrasl = 0, int region = 0, int pravsoyuzLevel = 0, long Id = 0)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                if (region == -1)
                {
                    //Pustoy
                    return null;
                }

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
                        org1.DB_BAL = 0;
                        org1.CT_BAL = 0;
                        org1.FACE_TYPE = 0;
                        org1.OWNER_CODE = model.ID_Ot == OthersBranch ? null : GenerateDepartmentCode(model.ID_Ot.Value
                                        , model.Region.Value,
                                        model.PravSoyuzLevel.Value,
                                        model.ID);
                        org1.TIMESTAMP_CH = DateTime.Now;
                        org1.TO_DEPCODE = model.to_depcode;

                        if (!string.IsNullOrEmpty(org1.OWNER_CODE))
                        {
                            var isDepExists = db.t_departmentbase.Where(r => r.code == org1.OWNER_CODE);

                            if (!isDepExists.Any())
                            {
                                if ((int)KIAT_STRUCT.TumanBranch == model.PravSoyuzLevel)
                                {
                                    t_departmentbase dd = new t_departmentbase
                                    {
                                        inn = model.INN,
                                        le_inn = model.orig_inn,
                                        parent_code = model.DepartmentBase,
                                        code = org1.OWNER_CODE,
                                        name_ru = model.Name_Org,
                                        name_uz = model.Name_Org,
                                        dtype = "Department",
                                        version = 0
                                    };

                                    db.t_departmentbase.Add(dd);
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


        public static void ModifyByTemplate(Sprav_OrgViewModel model, KasabaKIATEntities db)
        {
            var orgs = db.tblSprav_Org.Where(z => z.INN == model.INN);

            if (orgs.Any())
            {
                foreach(var org in orgs)
                {
                    org.ID_Ot = model.ID_Ot;
                    org.TIMESTAMP_CH = DateTime.Now;
                }
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
                               where org.DepartmentBase == model.DepartmentBase && org.FACE_TYPE == 0
                               orderby org.Name_Org
                               select new Sprav_OrgViewModel
                               {
                                   ID = org.ID,
                                   ID_Ot = org.ID_Ot,
                                   Name_Org = org.Name_Org,
                                   INN = org.INN,
                                   RS = org.RS,
                                   MFO = org.MFO,
                                   Bank = org.Bank,
                                   OKONX = org.OKONX,
                                   Adress = org.Adress,
                                   Telefon = org.Telefon,
                                   Region = org.Region,
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
                                   owner_code = org.OWNER_CODE,
                                   db_bal = org.DB_BAL,
                                   ct_bal = org.CT_BAL,
                                   to_depcode = org.TO_DEPCODE
                               };

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
                                   //join otras in db.tblSprav_Otras on org.ID_Ot equals otras.ID_Ot
                                   //join region in db.tblSprav_Reg on org.Region equals region.CodeReg
                               where org.DepartmentBase == (departmentBase.Equals("17") ? org.DepartmentBase : departmentBase) && org.FACE_TYPE == 0
                               //&& otras.DepartmentBase == departmentBase
                               orderby org.Name_Org
                               select new Sprav_OrgViewModel
                               {
                                   ID = org.ID,
                                   ID_Ot = org.ID_Ot,
                                   OtraslName = "",//otras.Name_Ot,
                                   Name_Org = org.Name_Org,
                                   INN = org.INN,
                                   RS = org.RS,
                                   MFO = org.MFO,
                                   Bank = org.Bank,
                                   OKONX = org.OKONX,
                                   Adress = org.Adress,
                                   Telefon = org.Telefon,
                                   Region = org.Region,
                                   RegionName = "",
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
                                   owner_code = org.OWNER_CODE,
                                   db_bal = org.DB_BAL,
                                   ct_bal = org.CT_BAL,
                                   to_depcode = org.TO_DEPCODE
                               };

                list = queryRes.Take(30).ToList();

                var otrasList = db.tblSprav_Otras.Where(s => s.DepartmentBase == departmentBase).ToList();

                var regionList = db.tblSprav_Reg.ToList();

                foreach (var org in list)
                {
                    var isOtrasExists = otrasList.Where(d => d.ID_Ot == org.ID_Ot);

                    if (isOtrasExists.Any())
                    {
                        org.OtraslName = isOtrasExists.First().Name_Ot;
                    }

                    var isRegionExists = regionList.Where(r => r.CodeReg == org.Region);

                    if (isRegionExists.Any())
                    {
                        org.RegionName = isRegionExists.First().NameReg;
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public static List<Sprav_OrgViewModel> GetByDeraprtmentBaseV2(string departmentBase)
        {
            List<Sprav_OrgViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from org in db.tblSprav_Org
                                   //join otras in db.tblSprav_Otras on org.ID_Ot equals otras.ID_Ot
                                   //join region in db.tblSprav_Reg on org.Region equals region.CodeReg
                               where org.DepartmentBase == (departmentBase.Equals("17") ? org.DepartmentBase : departmentBase) && org.FACE_TYPE == 0
                               //&& otras.DepartmentBase == departmentBase
                               orderby org.Name_Org
                               select new Sprav_OrgViewModel
                               {
                                   ID = org.ID,
                                   ID_Ot = org.ID_Ot,
                                   OtraslName = "",//otras.Name_Ot,
                                   Name_Org = org.Name_Org,
                                   INN = org.INN,
                                   RS = org.RS,
                                   MFO = org.MFO,
                                   Bank = org.Bank,
                                   OKONX = org.OKONX,
                                   Adress = org.Adress,
                                   Telefon = org.Telefon,
                                   Region = org.Region,
                                   RegionName = "",
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
                                   owner_code = org.OWNER_CODE,
                                   db_bal = org.DB_BAL,
                                   ct_bal = org.CT_BAL,
                                   to_depcode = org.TO_DEPCODE
                               };

                list = queryRes.Take(50000).ToList();

                var otrasList = db.tblSprav_Otras.Where(s => s.DepartmentBase == departmentBase).ToList();

                var regionList = db.tblSprav_Reg.ToList();

                foreach (var org in list)
                {
                    var isOtrasExists = otrasList.Where(d => d.ID_Ot == org.ID_Ot);

                    if (isOtrasExists.Any())
                    {
                        org.OtraslName = isOtrasExists.First().Name_Ot;
                    }

                    var isRegionExists = regionList.Where(r => r.CodeReg == org.Region);

                    if (isRegionExists.Any())
                    {
                        org.RegionName = isRegionExists.First().NameReg;
                    }
                }
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
                                   //join otras in db.tblSprav_Otras on org.ID_Ot equals otras.ID_Ot
                                   //join region in db.tblSprav_Reg on org.Region equals region.CodeReg
                               where org.DepartmentBase == (departmentBase.Equals("17") ? org.DepartmentBase : departmentBase)
                               //&& otras.DepartmentBase == departmentBase
                               && (org.Name_Org.Contains((IsNameEmpty ? org.Name_Org : Name)))
                               && (org.INN.Contains((IsInnEmpty ? org.INN : Inn)))
                               && (org.ID_Ot == (OtrasId.HasValue ? OtrasId == -1 ? null : OtrasId : org.ID_Ot))
                               && org.FACE_TYPE == 0
                               orderby org.Name_Org
                               select new Sprav_OrgViewModel
                               {
                                   ID = org.ID,
                                   ID_Ot = org.ID_Ot,
                                   OtraslName = "",//otras.Name_Ot,
                                   Name_Org = org.Name_Org,
                                   INN = org.INN,
                                   RS = org.RS,
                                   MFO = org.MFO,
                                   Bank = org.Bank,
                                   OKONX = org.OKONX,
                                   Adress = org.Adress,
                                   Telefon = org.Telefon,
                                   Region = org.Region,
                                   RegionName = "",//region.NameReg,
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
                                   owner_code = org.OWNER_CODE,
                                   db_bal = org.DB_BAL,
                                   ct_bal = org.CT_BAL,
                                   to_depcode = org.TO_DEPCODE
                               };

                list = queryRes.ToList();

                var otrasList = db.tblSprav_Otras.Where(s => s.DepartmentBase == departmentBase).ToList();

                var regionList = db.tblSprav_Reg.ToList();

                foreach (var org in list)
                {
                    var isOtrasExists = otrasList.Where(d => d.ID_Ot == org.ID_Ot);

                    if (isOtrasExists.Any())
                    {
                        org.OtraslName = isOtrasExists.First().Name_Ot;
                    }

                    var isRegionExists = regionList.Where(r => r.CodeReg == org.Region);

                    if (isRegionExists.Any())
                    {
                        org.RegionName = isRegionExists.First().NameReg;
                    }
                }

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
        public static List<Sprav_OrgViewModel> GetByDeraprtmentBaseM(string departmentBase,
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
                                   //join otras in db.tblSprav_Otras on org.ID_Ot equals otras.ID_Ot
                                   //join region in db.tblSprav_Reg on org.Region equals region.CodeReg
                               where org.DepartmentBase.Contains("17")
                               //&& otras.DepartmentBase == departmentBase
                               && (org.Name_Org.Contains((IsNameEmpty ? org.Name_Org : Name)))
                               && (org.INN.Contains((IsInnEmpty ? org.INN : Inn)))
                               && (org.ID_Ot == (OtrasId.HasValue ? OtrasId == -1 ? null : OtrasId : org.ID_Ot))
                               && org.FACE_TYPE == 0
                               orderby org.Name_Org
                               select new Sprav_OrgViewModel
                               {
                                   ID = org.ID,
                                   ID_Ot = org.ID_Ot,
                                   OtraslName = "",//otras.Name_Ot,
                                   Name_Org = org.Name_Org,
                                   INN = org.INN,
                                   RS = org.RS,
                                   MFO = org.MFO,
                                   Bank = org.Bank,
                                   OKONX = org.OKONX,
                                   Adress = org.Adress,
                                   Telefon = org.Telefon,
                                   Region = org.Region,
                                   RegionName = "",//region.NameReg,
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
                                   owner_code = org.OWNER_CODE,
                                   db_bal = org.DB_BAL,
                                   ct_bal = org.CT_BAL,
                                   to_depcode = org.TO_DEPCODE
                               };

                list = queryRes.ToList();

                var otrasList = db.tblSprav_Otras.Where(s => s.DepartmentBase == departmentBase).ToList();

                var regionList = db.tblSprav_Reg.ToList();

                foreach (var org in list)
                {
                    var isOtrasExists = otrasList.Where(d => d.ID_Ot == org.ID_Ot);

                    if (isOtrasExists.Any())
                    {
                        org.OtraslName = isOtrasExists.First().Name_Ot;
                    }

                    var isRegionExists = regionList.Where(r => r.CodeReg == org.Region);

                    if (isRegionExists.Any())
                    {
                        org.RegionName = isRegionExists.First().NameReg;
                    }
                }

            }

            return list;
        }
        #region OraslCounts

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public static long TotalCountOfNeotOrganization(string departmentBase)
        {
            long res = 0;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                res = (from org in db.tblSprav_Org
                       where org.DepartmentBase.Contains(departmentBase) && org.ID_Ot == null && org.FACE_TYPE == 0
                       select org).Count();
            }

            return res;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public static long TotalCountOfOrganization(string departmentBase)
        {
            long res = 0;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                res = (from org in db.tblSprav_Org
                       where org.DepartmentBase.Contains(departmentBase) && org.FACE_TYPE == 0
                       select org).Count();
            }

            return res;
        }

        #endregion

        /// <summary>
        /// For modify purpose
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
                                   //join otras in db.tblSprav_Otras on org.ID_Ot equals otras.ID_Ot
                                   //join region in db.tblSprav_Reg on org.Region equals region.CodeReg
                               where org.DepartmentBase == (departmentBase.Equals("17")? org.DepartmentBase : departmentBase) && org.FACE_TYPE == 0
                               //&& otras.DepartmentBase == departmentBase
                               && org.ID == Id
                               orderby org.Name_Org
                               select new Sprav_OrgViewModel
                               {
                                   ID = org.ID,
                                   ID_Ot = org.ID_Ot,
                                   OtraslName = "",//otras.Name_Ot,
                                   Name_Org = org.Name_Org,
                                   INN = org.INN,
                                   RS = org.RS,
                                   MFO = org.MFO,
                                   Bank = org.Bank,
                                   OKONX = org.OKONX,
                                   Adress = org.Adress,
                                   Telefon = org.Telefon,
                                   Region = org.Region,
                                   RegionName = "",
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
                                   owner_code = org.OWNER_CODE,
                                   db_bal = org.DB_BAL,
                                   ct_bal = org.CT_BAL,
                                   to_depcode = org.TO_DEPCODE
                               };

                list = queryRes.ToList();

                var otrasList = db.tblSprav_Otras.Where(s => s.DepartmentBase == departmentBase).ToList();

                var regionList = db.tblSprav_Reg.ToList();

                foreach (var org in list)
                {
                    var isOtrasExists = otrasList.Where(d => d.ID_Ot == org.ID_Ot);

                    if (isOtrasExists.Any())
                    {
                        org.OtraslName = isOtrasExists.First().Name_Ot;
                    }

                    if (org.ParentId.HasValue)
                    {
                        var orgName = db.tblSprav_Org.Where(z => z.ID == org.ParentId);

                        if (orgName.Any())
                        {
                            org.ParentName = orgName.First().Name_Org;
                        }

                    }

                    var isRegionExists = regionList.Where(r => r.CodeReg == org.Region);

                    if (isRegionExists.Any())
                    {
                        org.RegionName = isRegionExists.First().NameReg;
                    }
                }

            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="amount"></param>
        public static void ChangeOstatok(TransactionOperation oper, string inn, string departmentCode ,decimal? amount, KasabaKIATEntities db)
        {
            var orgs = db.tblSprav_Org.Where(z => z.INN == inn && z.DepartmentBase == departmentCode && z.FACE_TYPE == 0);

            if (orgs.Any())
            {
                var org = orgs.First();
                decimal? DB_BAL = 0;
                if (org.DB_BAL.HasValue)
                {
                    DB_BAL = org.DB_BAL;
                }

                decimal? CT_BAL = 0;
                if (org.CT_BAL.HasValue)
                {
                    CT_BAL = org.CT_BAL;
                }


                if (oper == TransactionOperation.Debit)
                {
                    DB_BAL += amount;
                }
                else if (oper == TransactionOperation.Creadit)
                {
                    CT_BAL += amount;
                }

                if (DB_BAL >= CT_BAL)
                {
                    DB_BAL = DB_BAL - CT_BAL;
                    CT_BAL = 0;
                }
                else
                {
                    CT_BAL = CT_BAL - DB_BAL;
                    DB_BAL = 0;
                }

                org.DB_BAL = DB_BAL;
                org.CT_BAL = CT_BAL;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="amount"></param>
        public static void ChangeOstatok(TransactionOperation oper, long idOrg, decimal? amount, KasabaKIATEntities db)
        {

            var orgs = db.tblSprav_Org.Where(z => z.ID == idOrg && z.FACE_TYPE == 0);

            if (orgs.Any())
            {
                var org = orgs.First();

                decimal? DB_BAL = 0;
                if (org.DB_BAL.HasValue)
                {
                    DB_BAL = org.DB_BAL;
                }

                decimal? CT_BAL = 0;
                if (org.CT_BAL.HasValue)
                {
                    CT_BAL = org.CT_BAL;
                }

                if (oper == TransactionOperation.Debit)
                {
                    DB_BAL += amount;
                }
                else if (oper == TransactionOperation.Creadit)
                {
                    CT_BAL += amount;
                }

                if (DB_BAL >= CT_BAL)
                {
                    DB_BAL = DB_BAL - CT_BAL;
                    CT_BAL = 0;
                }
                else
                {
                    CT_BAL = CT_BAL - DB_BAL;
                    DB_BAL = 0;
                }

                org.DB_BAL = DB_BAL;
                org.CT_BAL = CT_BAL;
            }
        }


        #region FizLiso

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void CreateFizLiso(Sprav_OrgViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblSprav_Org org = new tblSprav_Org();

                org.ID_Ot = -1;
                org.Name_Org = model.Name_Org;
                org.INN = model.INN;
                org.Adress = model.Adress;
                org.Telefon = model.Telefon;
                org.Region = -1;
                org.DT_OST = 0;
                org.KT_OST = 0;
                org.E_Mail = model.E_Mail;
                org.UserId = model.UserId;
                org.DepartmentBase = model.DepartmentBase;
                org.IS_PRAVSOYUZ = false;
                org.ParentID = null;
                org.PRAVORGLEVEL = 4;
                org.DB_BAL = 0;
                org.CT_BAL = 0;
                org.FACE_TYPE = 1;
                org.Pas_ser = model.Pas_ser;
                org.Pas_Num = model.Pas_Num;
                org.Pas_KD = model.Pas_KD;
                org.Pas_Data = model.Pas_Data;


                db.tblSprav_Org.Add(org);

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="db"></param>
        public static void CreateFizLiso(Sprav_OrgViewModel model, KasabaKIATEntities db)
        {
            var org2 = db.tblSprav_Org.Where(z => z.DepartmentBase == model.DepartmentBase && z.INN == model.INN);

            if (org2.Any())
            {
                var org1 = org2.FirstOrDefault();

                org1.ID_Ot = -1;
                org1.Name_Org = model.Name_Org;
                org1.INN = model.INN;
                org1.Adress = model.Adress;
                org1.Telefon = model.Telefon;
                org1.Region = -1;
                org1.DT_OST = 0;
                org1.KT_OST = 0;
                org1.E_Mail = model.E_Mail;
                org1.UserId = model.UserId;
                org1.DepartmentBase = model.DepartmentBase;
                org1.IS_PRAVSOYUZ = false;
                org1.ParentID = null;
                org1.PRAVORGLEVEL = 4;
                org1.DB_BAL = 0;
                org1.CT_BAL = 0;
                org1.FACE_TYPE = 1;
                org1.Pas_ser = model.Pas_ser;
                org1.Pas_Num = model.Pas_Num;
                org1.Pas_KD = model.Pas_KD;
                org1.Pas_Data = model.Pas_Data;
            }
            else
            {
                tblSprav_Org org = new tblSprav_Org
                {
                    ID_Ot = -1,
                    Name_Org = model.Name_Org,
                    INN = model.INN,
                    Adress = model.Adress,
                    Telefon = model.Telefon,
                    Region = -1,
                    DT_OST = 0,
                    KT_OST = 0,
                    E_Mail = model.E_Mail,
                    UserId = model.UserId,
                    DepartmentBase = model.DepartmentBase,
                    IS_PRAVSOYUZ = false,
                    ParentID = null,
                    PRAVORGLEVEL = 4,
                    DB_BAL = 0,
                    CT_BAL = 0,
                    FACE_TYPE = 1,
                    Pas_ser = model.Pas_ser,
                    Pas_Num = model.Pas_Num,
                    Pas_KD = model.Pas_KD,
                    Pas_Data = model.Pas_Data
                };

                db.tblSprav_Org.Add(org);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void ModifyFizLiso(Sprav_OrgViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var org = db.tblSprav_Org.Where(z => z.ID == model.ID);

                if (org.Any())
                {
                    var org1 = org.FirstOrDefault();

                    if (org1 != null)
                    {
                        org1.ID_Ot = -1;
                        org1.Name_Org = model.Name_Org;
                        org1.INN = model.INN;
                        org1.Adress = model.Adress;
                        org1.Telefon = model.Telefon;
                        org1.Region = -1;
                        org1.DT_OST = 0;
                        org1.KT_OST = 0;
                        org1.E_Mail = model.E_Mail;
                        org1.UserId = model.UserId;
                        org1.DepartmentBase = model.DepartmentBase;
                        org1.IS_PRAVSOYUZ = false;
                        org1.ParentID = null;
                        org1.PRAVORGLEVEL = 4;
                        org1.DB_BAL = 0;
                        org1.CT_BAL = 0;
                        org1.FACE_TYPE = 1;
                        org1.Pas_ser = model.Pas_ser;
                        org1.Pas_Num = model.Pas_Num;
                        org1.Pas_KD = model.Pas_KD;
                        org1.Pas_Data = model.Pas_Data;

                        db.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void ModifyFizLiso(Sprav_OrgViewModel model, KasabaKIATEntities db)
        {
                var org = db.tblSprav_Org.Where(z => z.ID == model.ID);

                if (org.Any())
                {
                    var org1 = org.FirstOrDefault();

                    if (org1 != null)
                    {
                        org1.ID_Ot = -1;
                        org1.Name_Org = model.Name_Org;
                        org1.INN = model.INN;
                        org1.Adress = model.Adress;
                        org1.Telefon = model.Telefon;
                        org1.Region = -1;
                        org1.DT_OST = 0;
                        org1.KT_OST = 0;
                        org1.E_Mail = model.E_Mail;
                        org1.UserId = model.UserId;
                        org1.DepartmentBase = model.DepartmentBase;
                        org1.IS_PRAVSOYUZ = false;
                        org1.ParentID = null;
                        org1.PRAVORGLEVEL = 4;
                        org1.DB_BAL = 0;
                        org1.CT_BAL = 0;
                        org1.FACE_TYPE = 1;
                        org1.Pas_ser = model.Pas_ser;
                        org1.Pas_Num = model.Pas_Num;
                        org1.Pas_KD = model.Pas_KD;
                        org1.Pas_Data = model.Pas_Data;

                        db.SaveChanges();
                    }
                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<Sprav_OrgViewModel> GetFizLisoList(Sprav_OrgViewModel model)
        {
            List<Sprav_OrgViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from org in db.tblSprav_Org
                               where (org.Name_Org.Contains(model.Name_Org)
                               || org.Pas_ser.Contains(model.Pas_ser)
                               || org.Pas_Num.Contains(model.Pas_Num))
                               && org.FACE_TYPE == 1
                               && org.DepartmentBase == model.DepartmentBase
                               select new Sprav_OrgViewModel
                               {
                                   ID = org.ID,
                                   Name_Org = org.Name_Org,
                                   INN = org.INN,
                                   Adress = org.Adress,
                                   Telefon = org.Telefon,
                                   E_Mail = org.E_Mail,
                                   UserId = org.UserId,
                                   Pas_ser = org.Pas_ser,
                                   Pas_Num = org.Pas_Num,
                                   Pas_KD = org.Pas_KD,
                                   Pas_Data = org.Pas_Data
                               };

                list = queryRes.ToList();
            }

            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<Sprav_OrgViewModel> GetFizLisoList(string departmentBase, long Id)
        {
            List<Sprav_OrgViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from org in db.tblSprav_Org
                               where org.FACE_TYPE == 1
                               && org.DepartmentBase == departmentBase
                               && org.ID == Id
                               select new Sprav_OrgViewModel
                               {
                                   ID = org.ID,
                                   Name_Org = org.Name_Org,
                                   INN = org.INN,
                                   Adress = org.Adress,
                                   Telefon = org.Telefon,
                                   E_Mail = org.E_Mail,
                                   UserId = org.UserId,
                                   Pas_ser = org.Pas_ser,
                                   Pas_Num = org.Pas_Num,
                                   Pas_KD = org.Pas_KD,
                                   Pas_Data = org.Pas_Data
                               };

                list = queryRes.ToList();

            }

            return list;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<Sprav_OrgViewModel> GetFizLisoList(string departmentBase)
        {
            List<Sprav_OrgViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from org in db.tblSprav_Org
                               where org.FACE_TYPE == 1
                               && org.DepartmentBase.Contains(departmentBase)
                               select new Sprav_OrgViewModel
                               {
                                   ID = org.ID,
                                   Name_Org = org.Name_Org,
                                   INN = org.INN,
                                   Adress = org.Adress,
                                   Telefon = org.Telefon,
                                   E_Mail = org.E_Mail,
                                   UserId = org.UserId,
                                   Pas_ser = org.Pas_ser,
                                   Pas_Num = org.Pas_Num,
                                   Pas_KD = org.Pas_KD,
                                   Pas_Data = org.Pas_Data
                               };

                list = queryRes.ToList();

            }

            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<Sprav_OrgViewModel> GetFizLisoList(string Name, string Inn, string Pas_ser, string Pas_Number, string departmentBase)
        {
            List<Sprav_OrgViewModel> list = null;

            bool IsNameEmpty = false;
            bool IsInnEmpty = false;

            bool IsPasNumEmpty = false;
            bool IsPasSerEmpty = false;

            if ((string.IsNullOrEmpty(Name)) || (Name.ToLower().Contains("undefined")))
            {
                IsNameEmpty = true;
            }

            if ((string.IsNullOrEmpty(Inn)) || (Inn.ToLower().Contains("undefined")))
            {
                IsInnEmpty = true;
            }

            if ((string.IsNullOrEmpty(Pas_ser)) || (Pas_ser.ToLower().Contains("undefined")))
            {
                IsPasSerEmpty = true;
            }

            if ((string.IsNullOrEmpty(Pas_Number)) || (Pas_Number.ToLower().Contains("undefined")))
            {
                IsPasNumEmpty = true;
            }

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from org in db.tblSprav_Org
                               where 
                               (org.Name_Org.Contains((IsNameEmpty ? org.Name_Org : Name)))
                               && (org.INN.Contains((IsInnEmpty ? org.INN : Inn)))
                               && (org.Pas_ser.Contains((IsPasSerEmpty ? org.Pas_ser : Pas_ser)))
                               && (org.Pas_Num.Contains((IsPasNumEmpty ? org.Pas_Num : Pas_Number)))
                               && org.FACE_TYPE == 1
                               && org.DepartmentBase.Contains(departmentBase)
                               select new Sprav_OrgViewModel
                               {
                                   ID = org.ID,
                                   Name_Org = org.Name_Org,
                                   INN = org.INN,
                                   Adress = org.Adress,
                                   Telefon = org.Telefon,
                                   E_Mail = org.E_Mail,
                                   UserId = org.UserId,
                                   Pas_ser = org.Pas_ser,
                                   Pas_Num = org.Pas_Num,
                                   Pas_KD = org.Pas_KD,
                                   Pas_Data = org.Pas_Data
                               };

                list = queryRes.ToList();
            }

            return list;
        }
        #endregion

    }


}