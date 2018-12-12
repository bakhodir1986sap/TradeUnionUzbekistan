using KasabaKiat.BusinessLogic.Zayavki;
using KasabaKiat.Helpers;
using KasabaKiat.ViewModel;
using KasabaKIAT.BusinessLogic._3_Etap.Statistika;
using KasabaKIAT.BusinessLogic.Putyovki.Rekviziti;
using KasabaKIAT.BusinessLogic.Putyovki.Zayavki;
using KasabaKIAT.BusinessLogic.Structure;
using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Putyovki;
using log4net;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Globalization;
using KasabaKiat.BusinessLogic.Utils.References;
using KasabaKIAT.BusinessLogic._3_Etap.Statistika.Stat_ViewModels;
using KasabaKIAT.BusinessLogic._4_Etap.Statistics;
using KasabaKIAT.BusinessLogic._4_Etap.Zayavki;
using System.Threading.Tasks;

namespace KasabaKIAT.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        ILog log = log4net.LogManager.GetLogger(typeof(HomeController));

        /// <summary>
        /// 
        /// </summary>
        KasabaKIATEntities KasabaDB = new KasabaKIATEntities();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult Index()
        {
            int UserId = 0;

            try
            {
                UserId = Convert.ToInt32(User.Identity.Name);

                log.Info("UserId = " + UserId);
                log.Info("User.Identity.Name = " + User.Identity.Name);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            if (UserId == 0)
            {
                log.Info("UserId = " + UserId);
                log.Info("Redirected SignOut => Login");
                return RedirectToAction("SignOut", "Login");
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult INN()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult izmParol()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public string TP(string p)
        {
            log.Info("TP");
            log.Info("p="+p);
            var UserId = Convert.ToInt32(User.Identity.Name);
            var d= UserInfo.GetUserInfo(UserId);
            try
            {
                var D = KasabaDB.uspLogin(d.username, p);
                var resList = D.ToList();
                if (resList.Any())
                {
                    if (resList.First() == UserId)
                    {
                        return "yes";
                    }
                    else
                    {
                        return "no";
                    }
                }
                else
                {
                    return "no";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public string NP(string p)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);
            log.Info("NP");
            log.Info("P="+p);
            try
            {
                KasabaDB.uspLoginChange(UserId, p);
                return "yes";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return "no";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult Getdictionary()
        {
            try
            {
                var camp = from dict in KasabaDB.tbcs_dictionary
                           join dictType in KasabaDB.tbcs_dictionarytype on
                           dict.DICTIONARY_TYPE_ID equals dictType.REC_ID
                           where dictType.CODE == "placetype"
                           select new
                           {
                               REC_ID = dict.REC_ID,
                               NAME_UZ = dict.NAME_UZ
                           };

                log.Info("Count = " + camp.ToList().Count);

                return Json(camp, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult Getdictionary_RZ()
        {
            try
            {
                var camp = from dict in KasabaDB.tbcs_dictionary
                       join dictType in KasabaDB.tbcs_dictionarytype on
                       dict.DICTIONARY_TYPE_ID equals dictType.REC_ID
                       where dictType.CODE == "rassmot_status"
                       select new
                       {
                           ID = dict.CODE,
                           NAME_UZ = dict.NAME_UZ
                       };


                log.Info("Count = " + camp.ToList().Count);

                return Json(camp, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult RassmotreniyeZayavok()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult Rekviziti()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetRekvizit()
        {
            log.Info("GetSanatory ");

            int UserId = Convert.ToInt32(User.Identity.Name);
            var uu = UserInfo.GetUserInfo(UserId);
            //d.department_base = uu.DepartmentBase;


            var rekvizit = from rek in KasabaDB.tblPreds
                           where rek.Departmentbase == uu.DepartmentBase
                           select new RekvizitiViewModel
                           {
                                ID_Name   = rek.ID_Name,
                                ID_Pred   = rek.ID_Pred,
                                ID_Pass   = rek.ID_Pass,
                                Adress    = rek.Adress,
                                RS        = rek.RS,
                                Name_bank = rek.Name_bank,
                                MFO       = rek.MFO,
                                INN       = rek.INN,
                                OKONX     = rek.OKONX,
                                Name_Dir  = rek.Name_Dir,
                                VIO       = rek.VIO
                           };

            if(!rekvizit.Any())
            {
                return Json( new RekvizitiViewModel()
                {
                    Adress ="",
                    ID_Name = "",
                    ID_Pass = "",
                    ID_Pred = "",
                    ID_Prof = "",
                    INN = "",
                    MFO = "",
                    Name_bank = "", 
                    Name_Dir = "",
                    OKONX = "",
                    PutDay = "",
                    RS = "",
                    VIO = false
                }, JsonRequestBehavior.AllowGet);
            } 

            return Json(rekvizit.ToList().First(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveRekvizit(RekvizitiViewModel model)
        {
            var rekvizit = new Rekviziti();

            int UserId = Convert.ToInt32(User.Identity.Name);
            var uu = UserInfo.GetUserInfo(UserId);

            model.DepartmentBase = uu.DepartmentBase;

            if (!rekvizit.SaveRekvizit(model)) return Json("fail", JsonRequestBehavior.AllowGet);

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult PodachaZayavok()
        {
           
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult PodachaZayavokV2()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);

            ViewBag.DataSouceZayavki = GetAllPZ_SV2(UserId, -1);

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetSanatory()
        {
            log.Info("GetSanatory ");

            try
            {
                var S = (from s in KasabaDB.tblSprav_San
                         select new
                         {
                             id = s.ID,
                             value = s.Name_San
                         });

                var resList = S.ToList();

                resList.Insert(0, new { id = -1, value = "Все" });

                log.Info("Count = " + resList.ToList().Count);

                return Json(resList, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetCamp()
        {
            log.Info("GetCamp");

            try
            {
                var C = (from s in KasabaDB.t_camp
                         orderby s.camps_name
                         select new
                         {
                             s.id,
                             value = s.camps_name + " - " + s.camps_organization_name
                         });

                log.Info("Count = " + C.ToList().Count);

                return Json(C, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetPosition()
        {
            log.Info("GetPosition");

            try
            {
                var P = (from s in KasabaDB.tbpt_zayavki
                         select new
                         {
                             id = s.REC_ID,
                             value = s.POSITION
                         });

                log.Info("Count = " + P.ToList().Count);

                return Json(P.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetAllPRZ_S()
        {
            log.Info("GetAllPRZ_S");

            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var list = (from e in KasabaDB.tbpt_zayavki
                            where e.USER_ID == UserId
                            where e.STATUS == "1"
                            join d in KasabaDB.tblSprav_San on e.SANATORY_ID equals d.ID
                            //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                            join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                            select new
                            {
                                ID = e.REC_ID,
                                ZB = e.ZAEZD_BEGIN,
                                ZE = e.ZAEZD_END,
                                SC = d.Name_San,//a.camps_name,
                                SC_id = d.ID,
                                P = e.POSITION,
                                FN = e.FULL_NAME,
                                S = e.STATUS,
                                INN = c.inn,
                                O = c.name_uz,
                                Q = e.QUANTITY,
                                TS = e.TIMESTAMP,
                                MOE = e.MEDICAL_ORDER_EXISTS,
                                MO = e.MEDICAL_ORDER
                            }).ToList();

                log.Info("Count = " + list.ToList().Count);

                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetAllPRZ_C()
        {
            log.Info("GetAllPRZ_C");

            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var list = (from e in KasabaDB.tbpt_zayavki
                            where e.USER_ID == UserId
                            where e.STATUS == "1"
                            //join d in KasabaDB.tblSprav_San on e.SANATORY_ID equals d.id
                            join a in KasabaDB.t_camp on e.CAMP_ID equals a.id
                            join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                            select new
                            {
                                ID = e.REC_ID,
                                ZB = e.ZAEZD_BEGIN,
                                ZE = e.ZAEZD_END,
                                SC = a.camps_name,
                                SC_id = a.id,
                                P = e.POSITION,
                                FN = e.FULL_NAME,
                                S = e.STATUS,
                                INN = c.inn,
                                O = c.name_uz,
                                Q = e.QUANTITY,
                                TS = e.TIMESTAMP,
                                MOE = e.MEDICAL_ORDER_EXISTS,
                                MO = e.MEDICAL_ORDER
                            });

                log.Info("Count = " + list.ToList().Count);

                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult Load_PRZ_S(int id)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            log.Info("Load_PRZ_S");
            log.Info("id = " + id);

            List<ZayavkiModel> list = new List<ZayavkiModel>();

            var kiat = new StructureOfKiat();

            try
            {
                var zayavkiOtp = KasabaDB.tbpt_zayavki.Where(z => z.STATUS != "0").ToList();

                if (zayavkiOtp.Any())
                {
                    foreach(var zayavka in zayavkiOtp)
                    {
                        if (id != -1)
                        {
                            if (zayavka.SANATORY_ID != id)
                            {
                                continue;
                            }
                        }

                        if (zayavka.USER_ID == UserId)
                        {
                            var zayavki1 = new ZayavkiModel();

                            zayavki1.ID = zayavka.REC_ID;
                            zayavki1.ZB = zayavka.ZAEZD_BEGIN;
                            zayavki1.ZE = zayavka.ZAEZD_END;
                            zayavki1.SC = KasabaDB.tblSprav_San.Where(z => z.ID == zayavka.SANATORY_ID).First().Name_San;
                            zayavki1.SC_id = zayavka.SANATORY_ID;
                            zayavki1.P  = zayavka.POSITION;
                            zayavki1.FN = zayavka.FULL_NAME;

                            if (zayavka.STATUS.Equals("1"))
                            {
                                zayavki1.S = "Отправленные";
                                zayavki1.isInitial = true;
                            }
                            else if (zayavka.STATUS.Equals("3"))
                            {
                                zayavki1.S = "Откланен";
                                zayavki1.isInitial = false;
                            }
                            else if (zayavka.STATUS.Equals("4"))
                            {
                                zayavki1.S = "Утвержден";
                                zayavki1.isInitial = false;
                            }
                            else if (zayavka.STATUS.Equals("5"))
                            {
                                zayavki1.S = "Выдана путевка";
                                zayavki1.isInitial = false;
                            }

                            var orgs = KasabaDB.tblSprav_Org.Where(z => z.OWNER_CODE == zayavka.DEPARTMENT_CODE);

                            if (orgs.Any())
                            {
                                var organization = orgs.First();

                                zayavki1.INN = organization.ORIG_INN;
                                zayavki1.O = organization.Name_Org;
                            }
                            else
                            {
                                var departments = KasabaDB.t_departmentbase.Where(z => z.code == zayavka.DEPARTMENT_CODE);

                                if (departments.Any())
                                {
                                    var department = departments.First();

                                    zayavki1.INN = department.le_inn;
                                    zayavki1.O = department.name_ru + department.bkut_name + department.dep_name;
                                }
                                else
                                {
                                    zayavki1.INN = string.Empty;
                                    zayavki1.O = string.Empty;
                                }
                            }

                            zayavki1.Q = zayavka.QUANTITY;
                            zayavki1.TS = zayavka.TIMESTAMP;
                            zayavki1.MOE = zayavka.MEDICAL_ORDER_EXISTS;
                            zayavki1.MO = zayavka.MEDICAL_ORDER;
                            zayavki1.isOwn = true;
                            zayavki1.sotrinn = zayavka.SOTR_INN;
                            zayavki1.pnfl = zayavka.PNFL;

                            list.Add(zayavki1);
                        }

                        var deptCode = kiat.GetParentOrganizationCode(zayavka.DEPARTMENT_CODE);

                        if (deptCode == ustinfo.DepartmentBase)
                        {
                            var zayavki1 = new ZayavkiModel();

                            zayavki1.ID = zayavka.REC_ID;
                            zayavki1.ZB = zayavka.ZAEZD_BEGIN;
                            zayavki1.ZE = zayavka.ZAEZD_END;
                            zayavki1.SC = KasabaDB.tblSprav_San.Where(z => z.ID == zayavka.SANATORY_ID).First().Name_San;
                            zayavki1.SC_id = zayavka.SANATORY_ID;
                            zayavki1.P = zayavka.POSITION;
                            zayavki1.FN = zayavka.FULL_NAME;

                            if (zayavka.STATUS.Equals("1"))
                            {
                                zayavki1.S = "Отправленные";
                                zayavki1.isInitial = true;
                            }
                            else if (zayavka.STATUS.Equals("3"))
                            {
                                zayavki1.S = "Откланен";
                                zayavki1.isInitial = false;
                            }
                            else if (zayavka.STATUS.Equals("4"))
                            {
                                zayavki1.S = "Утвержден";
                                zayavki1.isInitial = false;
                            }
                            else if (zayavka.STATUS.Equals("5"))
                            {
                                zayavki1.S = "Выдана путевка";
                                zayavki1.isInitial = false;
                            }

                            var orgs = KasabaDB.tblSprav_Org.Where(z => z.OWNER_CODE == zayavka.DEPARTMENT_CODE);

                            if (orgs.Any())
                            {
                                var organization = orgs.First();

                                zayavki1.INN = organization.ORIG_INN;
                                zayavki1.O = organization.Name_Org;
                            }
                            else
                            {
                                var departments = KasabaDB.t_departmentbase.Where(z => z.code == zayavka.DEPARTMENT_CODE);

                                if (departments.Any())
                                {
                                    var department = departments.First();

                                    zayavki1.INN = department.le_inn;
                                    zayavki1.O = department.name_ru + department.bkut_name + department.dep_name;
                                }
                                else
                                {
                                    zayavki1.INN = string.Empty;
                                    zayavki1.O = string.Empty;
                                }
                            }

                            zayavki1.Q = zayavka.QUANTITY;
                            zayavki1.TS = zayavka.TIMESTAMP;
                            zayavki1.MOE = zayavka.MEDICAL_ORDER_EXISTS;
                            zayavki1.MO = zayavka.MEDICAL_ORDER;
                            zayavki1.isOwn = false;
                            zayavki1.sotrinn = zayavka.SOTR_INN;
                            zayavki1.pnfl = zayavka.PNFL;

                            list.Add(zayavki1);
                        }
                    }
                }

                return Json(list, JsonRequestBehavior.AllowGet); 
            }
            catch(Exception ex)
            {
                log.Info(ex);
            }

            return null;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult Load_PRZ_SV2(int id = -1)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            log.Info("Load_PRZ_S");
            log.Info("id = " + id);

            List<ZayavkiModel> list = new List<ZayavkiModel>();

            var kiat = new StructureOfKiat();

            try
            {
                var zayavkiOtp = KasabaDB.tbpt_zayavki.Where(z => z.STATUS != "0").ToList();

                if (zayavkiOtp.Any())
                {
                    foreach (var zayavka in zayavkiOtp)
                    {
                        if (id != -1)
                        {
                            if (zayavka.SANATORY_ID != id)
                            {
                                continue;
                            }
                        }

                        if (zayavka.USER_ID == UserId)
                        {
                            var zayavki1 = new ZayavkiModel();

                            zayavki1.ID = zayavka.REC_ID;
                            zayavki1.ZB = zayavka.ZAEZD_BEGIN;
                            zayavki1.ZE = zayavka.ZAEZD_END;
                            zayavki1.SC = KasabaDB.tblSprav_San.Where(z => z.ID == zayavka.SANATORY_ID).First().Name_San;
                            zayavki1.SC_id = zayavka.SANATORY_ID;
                            zayavki1.P = zayavka.POSITION;
                            zayavki1.FN = zayavka.FULL_NAME;

                            if (zayavka.STATUS.Equals("1"))
                            {
                                zayavki1.S = "Отправленные";
                                zayavki1.isInitial = true;
                            }
                            else if (zayavka.STATUS.Equals("3"))
                            {
                                zayavki1.S = "Откланен";
                                zayavki1.isInitial = false;
                            }
                            else if (zayavka.STATUS.Equals("4"))
                            {
                                zayavki1.S = "Утвержден";
                                zayavki1.isInitial = false;
                            }
                            else if (zayavka.STATUS.Equals("5"))
                            {
                                zayavki1.S = "Выдана путевка";
                                zayavki1.isInitial = false;
                            }

                            var orgs = KasabaDB.tblSprav_Org.Where(z => z.OWNER_CODE == zayavka.DEPARTMENT_CODE);

                            if (orgs.Any())
                            {
                                var organization = orgs.First();

                                zayavki1.INN = organization.ORIG_INN;
                                zayavki1.O = organization.Name_Org;
                            }
                            else
                            {
                                var departments = KasabaDB.t_departmentbase.Where(z => z.code == zayavka.DEPARTMENT_CODE);

                                if (departments.Any())
                                {
                                    var department = departments.First();

                                    zayavki1.INN = department.le_inn;
                                    zayavki1.O = department.name_ru + department.bkut_name + department.dep_name;
                                }
                                else
                                {
                                    zayavki1.INN = string.Empty;
                                    zayavki1.O = string.Empty;
                                }
                            }

                            zayavki1.Q = zayavka.QUANTITY;
                            zayavki1.TS = zayavka.TIMESTAMP;
                            zayavki1.MOE = zayavka.MEDICAL_ORDER_EXISTS;
                            zayavki1.MO = zayavka.MEDICAL_ORDER;
                            zayavki1.isOwn = true;
                            zayavki1.sotrinn = zayavka.SOTR_INN;
                            zayavki1.pnfl = zayavka.PNFL;

                            list.Add(zayavki1);
                        }

                        var deptCode = kiat.GetParentOrganizationCode(zayavka.DEPARTMENT_CODE);

                        if ((deptCode == null) || (deptCode == ustinfo.DepartmentBase))
                        {
                            var zayavki1 = new ZayavkiModel();

                            zayavki1.ID = zayavka.REC_ID;
                            zayavki1.ZB = zayavka.ZAEZD_BEGIN;

                            //if (zayavka.ZAEZD_BEGIN.HasValue)
                            //{
                            //    zayavki1.ZB = zayavka.ZAEZD_BEGIN;
                            //}
                            //else
                            //{
                            //    zayavki1.ZB = DateTime.MinValue;
                            //}

                            zayavki1.ZE = zayavka.ZAEZD_END;
                            //if (zayavka.ZAEZD_END.HasValue)
                            //{
                            //    zayavki1.ZE = zayavka.ZAEZD_END;
                            //}
                            //else
                            //{
                            //    zayavki1.ZE = DateTime.MaxValue;
                            //}

                            var sanat = KasabaDB.tblSprav_San.Where(z => z.ID == zayavka.SANATORY_ID);

                            if (sanat.Any())
                            {
                                zayavki1.SC = sanat.First().Name_San;
                            }
                            else
                            {
                                zayavki1.SC = string.Empty;
                            }
                                                        
                            zayavki1.SC_id = zayavka.SANATORY_ID;
                            zayavki1.P = zayavka.POSITION;
                            zayavki1.FN = zayavka.FULL_NAME;

                            if (zayavka.STATUS.Equals("1"))
                            {
                                zayavki1.S = "Отправленные";
                                zayavki1.isInitial = true;
                            }
                            else if (zayavka.STATUS.Equals("3"))
                            {
                                zayavki1.S = "Откланен";
                                zayavki1.isInitial = false;
                            }
                            else if (zayavka.STATUS.Equals("4"))
                            {
                                zayavki1.S = "Утвержден";
                                zayavki1.isInitial = false;
                            }
                            else if (zayavka.STATUS.Equals("5"))
                            {
                                zayavki1.S = "Выдана путевка";
                                zayavki1.isInitial = false;
                            }

                            var orgs = KasabaDB.tblSprav_Org.Where(z => z.OWNER_CODE == zayavka.DEPARTMENT_CODE);

                            if (orgs.Any())
                            {
                                var organization = orgs.First();

                                zayavki1.INN = organization.ORIG_INN;
                                zayavki1.O = organization.Name_Org;
                            }
                            else
                            {
                                var departments = KasabaDB.t_departmentbase.Where(z => z.code == zayavka.DEPARTMENT_CODE);

                                if (departments.Any())
                                {
                                    var department = departments.First();

                                    zayavki1.INN = department.le_inn;
                                    zayavki1.O = department.name_ru + department.bkut_name + department.dep_name;
                                }
                                else
                                {
                                    zayavki1.INN = string.Empty;
                                    zayavki1.O = string.Empty;
                                }
                            }

                            zayavki1.Q = zayavka.QUANTITY;
                            zayavki1.TS = zayavka.TIMESTAMP;
                            zayavki1.MOE = zayavka.MEDICAL_ORDER_EXISTS;
                            zayavki1.MO = zayavka.MEDICAL_ORDER;
                            zayavki1.isOwn = false;
                            zayavki1.sotrinn = zayavka.SOTR_INN;
                            zayavki1.pnfl = zayavka.PNFL;

                            list.Add(zayavki1);
                        }
                    }
                }
                return Json(new { result = list, count = list.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }

            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public JsonResult UpdateState(long Id , string state)
        {
            var zayavki = KasabaDB.tbpt_zayavki.Where(z => z.REC_ID == Id);

            if (zayavki.Any())
            {
                var zayavka = zayavki.First();

                zayavka.STATUS = state;

                KasabaDB.SaveChanges();
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "id", VaryByCustom = "userId")]
        public JsonResult Load_PRZ_C(int id)
        {
            log.Info("Load_PRZ_C");
            log.Info("id = " + id);

            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var list = (from e in KasabaDB.tbpt_zayavki
                            where e.USER_ID == UserId
                            where e.STATUS == "1"
                            //join d in KasabaDB.tblSprav_San on e.SANATORY_ID equals d.id
                            join a in KasabaDB.t_camp on e.CAMP_ID equals a.id
                            where e.CAMP_ID == (id == -1 ? e.SANATORY_ID : id)
                            join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                            select new
                            {
                                ID = e.REC_ID,
                                ZB = e.ZAEZD_BEGIN,
                                ZE = e.ZAEZD_END,
                                SC = a.camps_name,
                                SC_id = a.id,
                                P = e.POSITION,
                                FN = e.FULL_NAME,
                                S = e.STATUS,
                                INN = c.inn,
                                O = c.name_uz,
                                Q = e.QUANTITY,
                                TS = e.TIMESTAMP,
                                MOE = e.MEDICAL_ORDER_EXISTS,
                                MO = e.MEDICAL_ORDER
                            });

                log.Info("Count = " + list.ToList().Count);

                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "date", VaryByCustom = "userId")]
        public JsonResult Load_PRZ_D0_S(DateTime date)
        {
            log.Info("Load_PRZ_D0_S");
            log.Info("date = " + date);

            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var list = (from e in KasabaDB.tbpt_zayavki
                            where e.USER_ID == UserId
                            where e.TIMESTAMP == date
                            where e.STATUS == "1"
                            join d in KasabaDB.tblSprav_San on e.SANATORY_ID equals d.ID
                            //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                            join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                            select new
                            {
                                ID = e.REC_ID,
                                ZB = e.ZAEZD_BEGIN,
                                ZE = e.ZAEZD_END,
                                SC = d.Name_San,//a.camps_name,
                                SC_id = d.ID,
                                P = e.POSITION,
                                FN = e.FULL_NAME,
                                S = e.STATUS,
                                INN = c.inn,
                                O = c.name_uz,
                                Q = e.QUANTITY,
                                TS = e.TIMESTAMP,
                                MOE = e.MEDICAL_ORDER_EXISTS,
                                MO = e.MEDICAL_ORDER
                            });

                log.Info("Count = " + list.ToList().Count);

                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "date", VaryByCustom = "userId")]
        public JsonResult Load_PRZ_D0_C(DateTime date)
        {
            log.Info("Load_PRZ_D0_C");
            log.Info("date = " + date);

            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var list = (from e in KasabaDB.tbpt_zayavki
                            where e.USER_ID == UserId
                            where e.TIMESTAMP == date
                            where e.STATUS == "1"
                            join d in KasabaDB.t_camp on e.CAMP_ID equals d.id
                            //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                            join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                            select new
                            {
                                ID = e.REC_ID,
                                ZB = e.ZAEZD_BEGIN,
                                ZE = e.ZAEZD_END,
                                SC = d.camps_name,//a.camps_name,
                                SC_id = d.id,
                                P = e.POSITION,
                                FN = e.FULL_NAME,
                                S = e.STATUS,
                                INN = c.inn,
                                O = c.name_uz,
                                Q = e.QUANTITY,
                                TS = e.TIMESTAMP,
                                MOE = e.MEDICAL_ORDER_EXISTS,
                                MO = e.MEDICAL_ORDER
                            });

                log.Info("Count = " + list.ToList().Count);

                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "date", VaryByCustom = "userId")]
        public JsonResult Load_PRZ_D1_S(DateTime date)
        {
            log.Info("Load_PRZ_D1_S");
            log.Info("date = " + date);


            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var list = (from e in KasabaDB.tbpt_zayavki
                            where e.USER_ID == UserId
                            where e.ZAEZD_BEGIN == date
                            where e.STATUS == "1"
                            join d in KasabaDB.tblSprav_San on e.SANATORY_ID equals d.ID
                            //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                            join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                            select new
                            {
                                ID = e.REC_ID,
                                ZB = e.ZAEZD_BEGIN,
                                ZE = e.ZAEZD_END,
                                SC = d.Name_San,//a.camps_name,
                                SC_id = d.ID,
                                P = e.POSITION,
                                FN = e.FULL_NAME,
                                S = e.STATUS,
                                INN = c.inn,
                                O = c.name_uz,
                                Q = e.QUANTITY,
                                TS = e.TIMESTAMP,
                                MOE = e.MEDICAL_ORDER_EXISTS,
                                MO = e.MEDICAL_ORDER
                            });

                log.Info("Count = " + list.ToList().Count);

                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "date", VaryByCustom = "userId")]
        public JsonResult Load_PRZ_D1_C(DateTime date)
        {
            log.Info("Load_PRZ_D1_C");
            log.Info("date = " + date);

            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var list = (from e in KasabaDB.tbpt_zayavki
                            where e.USER_ID == UserId
                            where e.ZAEZD_BEGIN == date
                            where e.STATUS == "1"
                            join d in KasabaDB.t_camp on e.CAMP_ID equals d.id
                            //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                            join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                            select new
                            {
                                ID = e.REC_ID,
                                ZB = e.ZAEZD_BEGIN,
                                ZE = e.ZAEZD_END,
                                SC = d.camps_name,//a.camps_name,
                                SC_id = d.id,
                                P = e.POSITION,
                                FN = e.FULL_NAME,
                                S = e.STATUS,
                                INN = c.inn,
                                O = c.name_uz,
                                Q = e.QUANTITY,
                                TS = e.TIMESTAMP,
                                MOE = e.MEDICAL_ORDER_EXISTS,
                                MO = e.MEDICAL_ORDER
                            });

                log.Info("Count = " + list.ToList().Count);

                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "id", VaryByCustom = "userId")]
        public JsonResult Load_PRZ_P_S(int id)
        {
            log.Info("Load_PRZ_P_S");
            log.Info("id = " + id);

            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var list = (from e in KasabaDB.tbpt_zayavki
                            where e.USER_ID == UserId
                            where e.REC_ID == id
                            where e.STATUS == "1"
                            join d in KasabaDB.tblSprav_San on e.SANATORY_ID equals d.ID
                            //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                            join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                            select new
                            {
                                ID = e.REC_ID,
                                ZB = e.ZAEZD_BEGIN,
                                ZE = e.ZAEZD_END,
                                SC = d.Name_San,//a.camps_name,
                                SC_id = d.ID,
                                P = e.POSITION,
                                FN = e.FULL_NAME,
                                S = e.STATUS,
                                INN = c.inn,
                                O = c.name_uz,
                                Q = e.QUANTITY,
                                TS = e.TIMESTAMP,
                                MOE = e.MEDICAL_ORDER_EXISTS,
                                MO = e.MEDICAL_ORDER
                            });

                log.Info("Count = " + list.ToList().Count);

                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "id", VaryByCustom = "userId")]
        public JsonResult Load_PRZ_P_C(int id)
        {
            log.Info("Load_PRZ_P_C");
            log.Info("id = " + id);

            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var list = (from e in KasabaDB.tbpt_zayavki
                            where e.USER_ID == UserId
                            where e.REC_ID == id
                            where e.STATUS == "1"
                            join d in KasabaDB.t_camp on e.CAMP_ID equals d.id
                            //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                            join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                            select new
                            {
                                ID = e.REC_ID,
                                ZB = e.ZAEZD_BEGIN,
                                ZE = e.ZAEZD_END,
                                SC = d.camps_name,//a.camps_name,
                                SC_id = d.id,
                                P = e.POSITION,
                                FN = e.FULL_NAME,
                                S = e.STATUS,
                                INN = c.inn,
                                O = c.name_uz,
                                Q = e.QUANTITY,
                                TS = e.TIMESTAMP,
                                MOE = e.MEDICAL_ORDER_EXISTS,
                                MO = e.MEDICAL_ORDER
                            });

                log.Info("Count = " + list.ToList().Count);

                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetAllPodachaZayavok()
        {
            log.Info("GetAllPodachaZayavok");

            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var list = (from e in KasabaDB.tbpt_zayavki
                            where e.USER_ID == UserId
                            where e.STATUS == "0"
                            join d in KasabaDB.tblSprav_San on e.SANATORY_ID equals d.ID
                            //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                            join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                            where e.SANATORY_ID != null || e.CAMP_ID != null
                            select new
                            {
                                ID = e.REC_ID,
                                ZB = e.ZAEZD_BEGIN,
                                ZE = e.ZAEZD_END,
                                SC = (e.CAMP_ID == null) ? d.Name_San : "",//a.camps_name,
                                SC_id = d.ID,
                                P = e.POSITION,
                                FN = e.FULL_NAME,
                                S = e.STATUS,
                                INN = c.inn,
                                O = c.name_uz,
                                Q = e.QUANTITY,
                                TS = e.TIMESTAMP,
                                MOE = e.MEDICAL_ORDER_EXISTS,
                                MO = e.MEDICAL_ORDER
                            });
                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc_id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "sc_id", VaryByCustom = "userId")]
        public JsonResult GetAllPZ_S(int sc_id)
        {
            log.Info("GetAllPZ_S");
            log.Info("sc_id = " + sc_id);

            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var list = (from e in KasabaDB.tbpt_zayavki
                            where e.USER_ID == UserId && e.STATUS == "0" && e.SANATORY_ID == (sc_id == -1 ? e.SANATORY_ID : sc_id)
                            join d in KasabaDB.tblSprav_San on e.SANATORY_ID equals d.ID
                            //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                            join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                            where e.SANATORY_ID != null || e.CAMP_ID != null
                            select new
                            {
                                ID = e.REC_ID,
                                ZB = e.ZAEZD_BEGIN,
                                ZE = e.ZAEZD_END,
                                SC = (e.CAMP_ID == null) ? d.Name_San : "",//a.camps_name,
                                SC_id = d.ID,
                                P = e.POSITION,
                                FN = e.FULL_NAME,
                                S = e.STATUS,
                                INN = c.inn,
                                O = c.name_uz + c.bkut_name + c.dep_name,
                                Q = e.QUANTITY,
                                TS = e.TIMESTAMP,
                                MOE = e.MEDICAL_ORDER_EXISTS,
                                MO = e.MEDICAL_ORDER,
                                InnFiz = e.SOTR_INN,
                                pnfl = e.PNFL
                            });
                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc_id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "sc_id", VaryByCustom = "userId")]
        public List<PodachaZayavokModel> GetAllPZ_SV2(int UserId, int sc_id = -1)
        {
            log.Info("GetAllPZ_S");
            log.Info("sc_id = " + sc_id);

            try
            {
                var list = (from e in KasabaDB.tbpt_zayavki
                            where e.USER_ID == UserId && e.STATUS == "0" 
                            && e.SANATORY_ID == (sc_id == -1 ? e.SANATORY_ID : sc_id)
                            join d in KasabaDB.tblSprav_San on e.SANATORY_ID equals d.ID
                            join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                            where e.SANATORY_ID != null || e.CAMP_ID != null
                            select new PodachaZayavokModel
                            {
                                rec_id = e.REC_ID,
                                zaezd_begin = e.ZAEZD_BEGIN,
                                zaezd_end = e.ZAEZD_END,
                                name_san = (e.CAMP_ID == null) ? d.Name_San : "",//a.camps_name,
                                sanid = d.ID,
                                position = e.POSITION,
                                full_name = e.FULL_NAME,
                                status = e.STATUS,
                                inn = c.inn,
                                organization = c.name_uz + c.bkut_name + c.dep_name,
                                quantity = e.QUANTITY,
                                timestamp = e.TIMESTAMP,
                                medical_order_exists = e.MEDICAL_ORDER_EXISTS,
                                medical_order = e.MEDICAL_ORDER,
                                sotr_inn = e.SOTR_INN,
                                pnfl = e.PNFL
                            });

                return list.ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc_id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "sc_id", VaryByCustom = "userId")]
        public JsonResult GetAllPZ_C(int sc_id)
        {

            log.Info("GetAllPZ_C");
            log.Info("sc_id = " + sc_id);

            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var list = (from e in KasabaDB.tbpt_zayavki
                        where e.USER_ID == UserId&& e.STATUS == "0"&&e.CAMP_ID==sc_id
                        //join d in KasabaDB.tblSprav_San on e.SANATORY_ID equals d.id
                        join a in KasabaDB.t_camp on e.CAMP_ID equals a.id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        where e.SANATORY_ID != null || e.CAMP_ID != null
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = (e.SANATORY_ID == null) ? a.camps_name : "",//d.sanatorium_name ,
                            SC_id = a.id,
                            P = e.POSITION,
                            FN = e.FULL_NAME,
                            S = e.STATUS,
                            INN = c.inn,
                            O = c.name_uz,
                            Q = e.QUANTITY,
                            TS = e.TIMESTAMP,
                            MOE = e.MEDICAL_ORDER_EXISTS,
                            MO = e.MEDICAL_ORDER,
                            InnFiz = e.SOTR_INN,
                            pnfl = e.PNFL
                        });

               return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc_id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "sc_id", VaryByCustom = "userId")]
        public JsonResult GetPodachaZayavok()
        {

            log.Info("GetAllPZ_C");

            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var list = (from e in KasabaDB.tbpt_zayavki
                            where e.USER_ID == UserId && e.STATUS == "0"
                            join d in KasabaDB.tblSprav_San on e.SANATORY_ID equals d.ID
                            join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                            where e.SANATORY_ID != null || e.CAMP_ID != null
                            select new PodachaZayavokModel
                            {
                                rec_id = e.REC_ID,
                                zaezd_begin = e.ZAEZD_BEGIN,
                                zaezd_end = e.ZAEZD_END,
                                name_san = (e.CAMP_ID == null) ? d.Name_San : "",//a.camps_name,
                                sanid = d.ID,
                                position = e.POSITION,
                                full_name = e.FULL_NAME,
                                status = e.STATUS,
                                inn = c.inn,
                                organization = c.name_uz + c.bkut_name + c.dep_name,
                                quantity = e.QUANTITY,
                                timestamp = e.TIMESTAMP,
                                medical_order_exists = e.MEDICAL_ORDER_EXISTS,
                                medical_order = e.MEDICAL_ORDER,
                                sotr_inn = e.SOTR_INN,
                                pnfl = e.PNFL
                            });

                return Json(new { result = list.ToList(), count = list.ToList().Count }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <param name="zb"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn;zb", VaryByCustom = "userId")]
        public JsonResult GetAllPodachaZayavok_S_LoadINN(string inn, int zb)
        {
            log.Info("GetAllPodachaZayavok_S_LoadINN");
            log.Info("inn = " + inn);
            log.Info("zb = " + zb);

            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                //DateTime b = new DateTime(, 'dd.MM.yyyy');
                var list = (from e in KasabaDB.tbpt_zayavki
                        //where e.USER_ID == UserId
                        where e.STATUS == "1"
                        join d in KasabaDB.tblSprav_San on e.SANATORY_ID equals d.ID
                        //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        where c.inn == inn && e.ZAEZD_BEGIN.Value.Year == zb && e.ZAEZD_BEGIN.Value.Day > 0 && e.ZAEZD_BEGIN.Value.Month > 0
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = (e.CAMP_ID == null) ? d.Name_San : "",//a.camps_name,
                            SC_id = d.ID,
                            P = e.POSITION,
                            FN = e.FULL_NAME,
                            S = e.STATUS,
                            INN = c.inn,
                            O = c.name_uz,
                            Q = e.QUANTITY,
                            TS = e.TIMESTAMP,
                            MOE = e.MEDICAL_ORDER_EXISTS,
                            MO = e.MEDICAL_ORDER
                        });
                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <param name="zb"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn;zb", VaryByCustom = "userId")]
        public JsonResult GetAllPodachaZayavok_C_LoadINN(string inn, int zb)
        {
            log.Info("GetAllPodachaZayavok_C_LoadINN");
            log.Info("inn = " + inn);
            log.Info("zb = " + zb);

            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var list = (from e in KasabaDB.tbpt_zayavki
                                //where e.USER_ID == UserId
                            where e.STATUS == "1"
                            //join d in KasabaDB.tblSprav_San on e.SANATORY_ID equals d.id
                            join a in KasabaDB.t_camp on e.CAMP_ID equals a.id
                            join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                            where c.inn == inn && e.ZAEZD_BEGIN.Value.Year == zb && e.ZAEZD_BEGIN.Value.Day > 0 && e.ZAEZD_BEGIN.Value.Month > 0
                            select new
                            {
                                ID = e.REC_ID,
                                ZB = e.ZAEZD_BEGIN,
                                ZE = e.ZAEZD_END,
                                SC = (e.SANATORY_ID == null) ? a.camps_name : "",//d.sanatorium_name,
                                SC_id = a.id,
                                P = e.POSITION,
                                FN = e.FULL_NAME,
                                S = e.STATUS,
                                INN = c.inn,
                                O = c.name_uz,
                                Q = e.QUANTITY,
                                TS = e.TIMESTAMP,
                                MOE = e.MEDICAL_ORDER_EXISTS,
                                MO = e.MEDICAL_ORDER
                            });
                return Json(list.ToList(), JsonRequestBehavior.AllowGet);

            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <param name="zb"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn;zb", VaryByCustom = "userId")]
        public JsonResult GroupRZ_S(string inn, int zb)
        {
            log.Info("GroupRZ_S");
            log.Info("inn = " + inn);
            log.Info("zb = " + zb);

            try
            {
                var list = (from e in KasabaDB.tbpt_zayavki
                            where e.STATUS == "1"
                            join d in KasabaDB.tblSprav_San on e.SANATORY_ID equals d.ID
                            join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                            where c.inn == inn && e.ZAEZD_BEGIN.Value.Year == zb
                            select new
                            {
                                ZB = e.ZAEZD_BEGIN,
                                SC = d.Name_San,
                                SC_id = d.ID,
                                Q = e.QUANTITY,
                                OQ = e.ORDER_QUANTITY
                            });
                var Groups = from p in list
                             group p by new { p.ZB.Value.Month, p.SC, p.SC_id } into g
                             select new { ZB_g = g.Key.Month, SC_g = g.Key.SC, SC_g_id = g.Key.SC_id, OQ = g.Sum(i => i.OQ), Count = g.Count(), Q_Sum = g.Sum(i => i.Q) };
                return Json(Groups.ToList(), JsonRequestBehavior.AllowGet);

            }
            catch(Exception ex)
            {
                log.Error(ex);
            }


            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <param name="zb"></param>
        /// <param name="zb_g"></param>
        /// <param name="SC_id"></param>
        /// <param name="oq"></param>
        /// <returns></returns>
        public string Add_RZ_S_OQ(string inn, int zb, int zb_g, int SC_id, int oq)
        {
            try
            {
                int i = 0;
                var list = from a in KasabaDB.tbpt_zayavki
                           orderby a.ZAEZD_BEGIN
                           //join d in KasabaDB.tblSprav_San on a.SANATORY_ID equals d.id
                           join c in KasabaDB.t_departmentbase on a.DEPARTMENT_CODE equals c.code
                           where a.STATUS == "1" && c.inn == inn && a.ZAEZD_BEGIN.Value.Year == zb && a.ZAEZD_BEGIN.Value.Month == zb_g && a.SANATORY_ID == SC_id
                           select a.REC_ID;
                foreach (var l in list)
                {
                    tbpt_zayavki tz = KasabaDB.tbpt_zayavki.FirstOrDefault(o => o.REC_ID == l);
                    i++;
                    if (i == oq)
                    {
                        tz.ORDER_QUANTITY = 1;
                        TryUpdateModel(tz);
                        break;
                    }
                    else
                    {
                        tz.ORDER_QUANTITY = 1;
                        TryUpdateModel(tz);
                    }
                }
                KasabaDB.SaveChanges();
                return "ok";
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <param name="zb"></param>
        /// <param name="zb_g"></param>
        /// <param name="SC_id"></param>
        /// <param name="oq"></param>
        /// <returns></returns>
        public string Add_RZ_C_OQ(string inn, int zb, int zb_g, int SC_id, int oq)
        {
            try
            {
                int i = 0;
                var list = from a in KasabaDB.tbpt_zayavki
                           orderby a.ZAEZD_BEGIN
                           //join d in KasabaDB.t_camp on a.CAMP_ID equals d.id
                           join c in KasabaDB.t_departmentbase on a.DEPARTMENT_CODE equals c.code
                           where a.STATUS == "1" && c.inn == inn && a.ZAEZD_BEGIN.Value.Year == zb && a.ZAEZD_BEGIN.Value.Month == zb_g && a.CAMP_ID == SC_id
                           select a.REC_ID;
                foreach (var l in list)
                {
                    tbpt_zayavki tz = KasabaDB.tbpt_zayavki.FirstOrDefault(o => o.REC_ID == l);
                    i++;
                    if (i == oq)
                    {
                        tz.ORDER_QUANTITY = 1;
                        TryUpdateModel(tz);
                        break;
                    }
                    else
                    {
                        tz.ORDER_QUANTITY = 1;
                        TryUpdateModel(tz);
                    }
                }
                KasabaDB.SaveChanges();
                return "ok";
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <param name="zb"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn;zb", VaryByCustom = "userId")]
        public JsonResult GroupRZ_C(string inn, int zb)
        {
            log.Info("GroupRZ_C");
            log.Info("inn = " + inn);
            log.Info("zb = " + zb);

            try
            {
                var list = (from e in KasabaDB.tbpt_zayavki
                            where e.STATUS == "1"
                            join d in KasabaDB.t_camp on e.CAMP_ID equals d.id
                            join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                            where c.inn == inn && e.ZAEZD_BEGIN.Value.Year == zb
                            select new
                            {
                                ZB = e.ZAEZD_BEGIN,
                                SC = d.camps_name,
                                SC_id = d.id,
                                Q = e.QUANTITY,
                                OQ = e.ORDER_QUANTITY
                            });
                var Groups = from p in list
                             group p by new { p.ZB.Value.Month, p.SC, p.SC_id } into g
                             select new { ZB_g = g.Key.Month, SC_g = g.Key.SC, SC_g_id = g.Key.SC_id, OQ = g.Sum(i => i.OQ), Count = g.Count(), Q_Sum = g.Sum(i => i.Q) };
                return Json(Groups.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public string UpdateStatusRZ(int id, string s)
        {
            log.Info("UpdateStatusRZ");
            log.Info("id = " + id);
            log.Info("s = " + s);


            try
            {
                tbpt_zayavki tz = KasabaDB.tbpt_zayavki.FirstOrDefault(o => o.REC_ID == id);
                tz.STATUS = s;
                TryUpdateModel(tz);
                KasabaDB.SaveChanges();
                return "ok";
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <param name="zb"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn;zb", VaryByCustom = "userId")]
        public JsonResult Getbankpayments(string inn, int zb)
        {
            log.Info("Getbankpayments");
            log.Info("inn = " + inn);
            log.Info("zb = " + zb);

            try
            {
                var list = from a in KasabaDB.tbgl_bankpayments
                           where a.TRAN_DATE.Year == zb && a.BENEF_INN == inn
                           select new
                           {
                               TD = a.TRAN_DATE,
                               A = a.AMOUNT
                           };
                var Groups = from p in list
                             group p by p.TD.Month into g
                             select new { ZB_g = g.Key, Count = g.Count(), A_Sum = g.Sum(i => i.A) };
                return Json(Groups.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yil"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "yil", VaryByCustom = "userId")]
        public JsonResult GetAvarageprice(int yil)
        {
            log.Info("GetAvarageprice");
            log.Info("yil = " + yil);

            try
            {
                var list = from a in KasabaDB.tbpt_avarageprice
                           where a.SET_DATE.Value.Year == yil
                           select new
                           {
                               A = a.AMOUNT
                           };
                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <param name="zb"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn;zb", VaryByCustom = "userId")]
        public JsonResult GetbankpaymentsYear(string inn, int zb)
        {

            log.Info("GetbankpaymentsYear");
            log.Info("inn = " + inn);
            log.Info("zb = " + zb);

            try
            {
                var list = from a in KasabaDB.tbgl_bankpayments
                           where a.TRAN_DATE.Year == zb && a.BENEF_INN == inn
                           select new
                           {
                               TD = a.TRAN_DATE,
                               A = a.AMOUNT
                           };
                var Groups = from p in list
                             group p by p.TD.Year into g
                             select new { ZB_g = g.Key, Count = g.Count(), A_Sum = g.Sum(i => i.A) };
                return Json(Groups.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string SplitValue1(string value)
        {
            return value.Split(new Char[] { '&' })[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string SplitValue2(string value)
        {
            return value.Split(new Char[] { '&' })[1];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pz"></param>
        /// <returns></returns>
        public string AddPodachaZayavok(tbpt_zayavki pz)
        {
            log.Info("AddPodachaZayavok ");

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);
            
            try
            {
                if (pz != null)
                {
                    //using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    //{
                        var x = MAX_GROUP_ID();
                        tbpt_zayavki z = new tbpt_zayavki();
                        z.USER_ID = Convert.ToInt32(User.Identity.Name);
                        z.ZAEZD_BEGIN = pz.ZAEZD_BEGIN;
                        z.ZAEZD_END = pz.ZAEZD_END;
                        z.GROUP_ID = x;
                        z.MEDICAL_ORDER = pz.MEDICAL_ORDER;
                        z.FULL_NAME = pz.FULL_NAME;
                        z.DEPARTMENT_CODE = ustinfo.DepartmentBase;//LOAD_DEPARTMENT_CODE(pz.DEPARTMENT_CODE);
                        z.POSITION = pz.POSITION;
                        z.QUANTITY = pz.QUANTITY;
                        z.SANATORY_ID = pz.SANATORY_ID;
                        z.CAMP_ID = pz.CAMP_ID;
                        z.STATUS = "0";
                        z.ORDER_QUANTITY = 0;
                        z.TIMESTAMP = DateTime.Now;
                        z.MEDICAL_ORDER_EXISTS = pz.MEDICAL_ORDER_EXISTS;
                        z.SOTR_INN = pz.SOTR_INN;
                        z.PNFL = pz.PNFL;

                        if (string.IsNullOrEmpty(pz.SOTR_INN) && string.IsNullOrEmpty(pz.PNFL))
                        {
                            return "Не заполненны поля ИНН или ПНФЛ";
                        }
                        else
                        {
                            DateTime minDate = DateTime.Now.AddDays(-730);

                            var takePutyovkasInn = KasabaDB.tblFase.Where(p => p.Data_Post >= minDate && p.INN == pz.SOTR_INN);

                            if (takePutyovkasInn.Any())
                            {
                                var pp1 = takePutyovkasInn.FirstOrDefault();

                                if (pp1 == null)
                                {
                                    throw new NullReferenceException();
                                }

                                var Sanatory1 = KasabaDB.tblSprav_San.Where(s => s.ID == pp1.ID_Sanat).First();
                                
                                var putyovka1 = pp1.Num_Put + " " + Sanatory1.Name_San;

                                return "Сотрудник уже ехал в санаторий последние 2 года " + putyovka1;
                            }

                            var takePutyovkasPnfl = KasabaDB.tblFase.Where(p => p.Data_Post >= minDate && p.INN == pz.PNFL);

                            if (takePutyovkasPnfl.Any())
                            {
                                var pp1 = takePutyovkasInn.FirstOrDefault();

                                if (pp1 == null)
                                {
                                    throw new NullReferenceException();
                                }

                                var Sanatory1 = KasabaDB.tblSprav_San.Where(s => s.ID == pp1.ID_Sanat).First();

                                var putyovka1 = pp1.Num_Put + " " + Sanatory1.Name_San;

                                return "Сотрудник уже ехал в санаторий последние 2 года " + putyovka1;
                            }
                        }

                        KasabaDB.tbpt_zayavki.Add(z);

                        //Save to BazaDannix Organizasiy
                        var mFizLiso = new Sprav_OrgViewModel();

                        mFizLiso.Name_Org = pz.FULL_NAME;
                        mFizLiso.INN = pz.SOTR_INN;
                        mFizLiso.Adress = string.Empty;
                        mFizLiso.Telefon = string.Empty;
                        mFizLiso.E_Mail = string.Empty;
                        mFizLiso.UserId = UserId;
                        mFizLiso.DepartmentBase = ustinfo.DepartmentBase;
                        mFizLiso.Pas_ser = string.Empty;
                        mFizLiso.Pas_Num = string.Empty;
                        mFizLiso.Pas_KD = string.Empty;
                        mFizLiso.Pas_Data = DateTime.Now;

                        Sprav_Org.CreateFizLiso(mFizLiso, KasabaDB);

                        try
                        {
                            KasabaDB.SaveChanges();
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

                        return "0";
                    //}
                }
                else
                {
                    return "Ошибка в данных";
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return "Ошибка в данных";

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn", VaryByCustom = "userId")]
        public string LOAD_DEPARTMENT_CODE(string inn)
        {
            log.Info("LOAD_DEPARTMENT_CODE ");

            string name = "";

            try
            {
                var a = from d in KasabaDB.t_departmentbase orderby d.id select d;
                foreach (var v in a)
                {
                    if (v.inn == inn)
                    {
                        name = v.code;
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn", VaryByCustom = "userId")]
        public string LoadINN(int inn)
        {
            log.Info("LoadINN");
            log.Info("Params = " + inn);

            try
            {
                string strInn = inn.ToString();

                var query = from department in KasabaDB.t_departmentbase
                            where department.le_inn == strInn
                            select department.name_uz;

                if (query.Any())
                {
                    return query.First();
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            
            return "No Data Found";

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int MAX_GROUP_ID()
        {
            log.Info("MAX_GROUP_ID");

            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                int x = 0;
                var g = from a in KasabaDB.tbpt_zayavki where (a.STATUS == "0" && a.USER_ID == UserId) orderby a.REC_ID select a;
                foreach (var v in g)
                {
                    x = Convert.ToInt32(v.GROUP_ID);
                }
                if (x == 0)
                {
                    var a = from b in KasabaDB.tbpt_zayavki where (b.STATUS == "1" && b.USER_ID == UserId) orderby b.REC_ID select b;
                    foreach (var v in a)
                    {
                        x = Convert.ToInt32(v.GROUP_ID);
                    }
                    x++;
                    return x;
                }
                return x;

            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "group_id", VaryByCustom = "userId")]
        public string STATUS(int group_id)
        {
            log.Info("STATUS");
            log.Info("group_id = "+ group_id);

            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                string d = "";
                var a = from b in KasabaDB.tbpt_zayavki where b.GROUP_ID == group_id where b.USER_ID == UserId orderby b.REC_ID select b;
                foreach (var s in a)
                {
                    d = s.STATUS;
                }
                return d;
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeletePodachaZayavok(string id)
        {
            log.Info("DeletePodachaZayavok");
            log.Info("id = " + id);


            try
            {
                if (!String.IsNullOrEmpty(id))
                {
                    try
                    {
                        int _bookId = Int32.Parse(id);
                        using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                        {
                            var _book = contextObj.tbpt_zayavki.Find(_bookId);
                            contextObj.tbpt_zayavki.Remove(_book);
                            contextObj.SaveChanges();
                            return "Selected book record deleted sucessfully";
                        }
                    }
                    catch (Exception)
                    {
                        return "Book details not found";
                    }
                }
                else
                {
                    return "Invalid operation";
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return "";
        }
        
        //Update PodachaZayavok
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pz"></param>
        /// <returns></returns>
        public string UpdatePodachaZayavok(tbpt_zayavki pz)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            log.Info("UpdatePodachaZayavok");
            log.Info("pz = " + pz.FULL_NAME);

            try
            {
                if (pz != null)
                {
                    //using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    //{
                        int bookId = Convert.ToInt32(pz.REC_ID);
                        tbpt_zayavki _gz = KasabaDB.tbpt_zayavki.Where(b => b.REC_ID == bookId).FirstOrDefault();
                        _gz.ZAEZD_BEGIN = pz.ZAEZD_BEGIN;
                        _gz.ZAEZD_END = pz.ZAEZD_END;
                        _gz.USER_ID = Convert.ToInt32(User.Identity.Name);
                        _gz.POSITION = pz.POSITION;
                        _gz.QUANTITY = pz.QUANTITY;
                        _gz.CAMP_ID = pz.CAMP_ID;
                        _gz.SANATORY_ID = pz.SANATORY_ID;
                        _gz.FULL_NAME = pz.FULL_NAME;
                        _gz.DEPARTMENT_CODE = ustinfo.DepartmentBase;//LOAD_DEPARTMENT_CODE(pz.DEPARTMENT_CODE);
                        _gz.MEDICAL_ORDER_EXISTS = pz.MEDICAL_ORDER_EXISTS;
                        _gz.MEDICAL_ORDER = pz.MEDICAL_ORDER;
                        _gz.TIMESTAMP = DateTime.Now;
                        _gz.SOTR_INN = pz.SOTR_INN;
                        _gz.PNFL = pz.PNFL;

                        try
                        {
                            KasabaDB.SaveChanges();
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
                    return "GZ record updated successfully";
                    //}
                }
                else
                {
                    return "Invalid gz record";
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return "Invalid gz record";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string OTPPodachaZayavok()
        {
            log.Info("OTPPodachaZayavok");


            try
            {
                using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                {
                    int UserId = Convert.ToInt32(User.Identity.Name);
                    //int bookId = Convert.ToInt32(pz.REC_ID);
                    var bb = from a in KasabaDB.tbpt_zayavki
                             where a.USER_ID == UserId && a.STATUS == "0"
                             select a;

                    foreach (var v in bb)
                    {
                        tbpt_zayavki _gz = contextObj.tbpt_zayavki.Where(b => b.REC_ID == v.REC_ID).FirstOrDefault();
                        _gz.STATUS = "1";
                    }
                    contextObj.SaveChanges();
                    return "OTP succes!!!";
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return "";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FileResult DownloadZayavkaTempl()
        {
            var ArxiveDirectory = Properties.Settings.Default.TemplateDir;

            var targetFile = Path.Combine(ArxiveDirectory, "zayavki.xlsx");

            if (!System.IO.File.Exists(targetFile)) return null;

            byte[] fileBytes = System.IO.File.ReadAllBytes(targetFile);
            string fileName = "zayavki.xlsx";
            return File(fileBytes, "application/vnd.ms-excel", fileName);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="formCollection"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReadFileZayavki(FormCollection formCollection)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            log.Info("ReadFileZayavki");
            try
            {
                if (Request != null)
                {
                    HttpPostedFileBase file = Request.Files["ZayavkiLoad"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                var zayavka = new ZayavkiViewModel();

                                zayavka.userid = UserId;
                                zayavka.departmentCode = ustinfo.DepartmentBase;

                                try
                                {
                                    var sanatName = workSheet.Cells[rowIterator, 1].Value.ToString();

                                    var sanats = KasabaDB.tblSprav_San.Where(ss => ss.Name_San == sanatName);

                                    if (sanats.Any())
                                    {
                                        zayavka.SanatoryId = sanats.First().ID;
                                    }
                                    else
                                    {
                                        zayavka.SanatoryId = 2;
                                    }

                                    string str = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    string[] format = { "dd.MM.yyyy" };
                                    DateTime date;

                                    if (!string.IsNullOrEmpty(str))
                                    {
                                        if (str.Length >= 10)
                                        {
                                            if (DateTime.TryParseExact(str.Substring(0, 10),
                                                                   format,
                                                                   System.Globalization.CultureInfo.InvariantCulture,
                                                                   System.Globalization.DateTimeStyles.None,
                                                                   out date))
                                            {
                                                zayavka.ZaezdBegin = date;
                                            }
                                            else
                                            {
                                                zayavka.ZaezdBegin = DateTime.Now;
                                            }
                                        }
                                        else
                                        {
                                            zayavka.ZaezdBegin = DateTime.Now;
                                        }
                                    }
                                    else
                                    {
                                        zayavka.ZaezdBegin = DateTime.Now;
                                    }


                                    string str2 = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    string[] format2 = { "dd.MM.yyyy" };
                                    DateTime date2;

                                    if (!string.IsNullOrEmpty(str2))
                                    {
                                        if (str.Length >= 10)
                                        {
                                            if (DateTime.TryParseExact(str2.Substring(0, 10),
                                                                   format2,
                                                                   System.Globalization.CultureInfo.InvariantCulture,
                                                                   System.Globalization.DateTimeStyles.None,
                                                                   out date2))
                                            {
                                                zayavka.ZaezdEnd = date2;
                                            }
                                            else
                                            {
                                                zayavka.ZaezdEnd = DateTime.Now;
                                            }
                                        }
                                        else
                                        {
                                            zayavka.ZaezdEnd = DateTime.Now;
                                        }
                                    }
                                    else
                                    {
                                        zayavka.ZaezdEnd = DateTime.Now;
                                    }

                                    zayavka.inn = workSheet.Cells[rowIterator, 4].Value.ToString();
                                    zayavka.pnfl = workSheet.Cells[rowIterator, 5].Value.ToString();
                                    zayavka.FullName = workSheet.Cells[rowIterator, 6].Value.ToString();
                                    zayavka.Position = workSheet.Cells[rowIterator, 7].Value.ToString();
                                    zayavka.MedicalOrderExists = true;
                                    zayavka.MedicalOrder = workSheet.Cells[rowIterator, 8].Value.ToString();
                                }
                                catch (NullReferenceException ex)
                                {
                                    log.Info(ex.ToString());
                                    continue;
                                }
                                catch(Exception ex)
                                {
                                    log.Info(ex.ToString());
                                    continue;
                                }

                                ZayavkiDataManager.EnterZayavka(ref zayavka);
                            }
                        }
                    }
                }
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult buxgalteriya()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult PoiskRedaktZayavok()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult PoiskRedaktZayavokV2()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult Putyovki(string lang = null)
        {
            if (lang != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            }
            else
            {
                lang = "ru";
            }

            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = lang;
            Response.Cookies.Add(cookie);

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);
            ustinfo.CurLang = lang;

            string viewName = "StatFederasiyaV2";

            switch (ustinfo.role_id.Value)
            {
                case 6:
                    viewName = "StatRayonOtrasl";
                    break;
                case 8:
                    viewName = "StatPPO";
                    break;
                case 13:
                    viewName = "StatOblSovProfV2";
                    var resultStat3 = StatDataContainer.GetStataData(ustinfo.DepartmentBase, ustinfo.CurYear);

                    ViewBag.LastChangeTime = StatDataContainer._modelChangedTime;
                    ViewBag.DataSource = ((FourStapViewModel)resultStat3).kaznaInfo;
                    ViewBag.DataUrgent = ((FourStapViewModel)resultStat3).urgentInfo;
                    ViewBag.DataPutyovkas = ((FourStapViewModel)resultStat3).putyovkaInfo;
                    ViewBag.RealizData = ((FourStapViewModel)resultStat3).realizInfo;
                    ViewBag.ZadoljennostData = ((FourStapViewModel)resultStat3).zadoljInfo;
                    ViewBag.BronData = ((FourStapViewModel)resultStat3).bronInfo;
                    ViewBag.ActiveUsersData = ((FourStapViewModel)resultStat3).activeUsersInfo;
                    ViewBag.PieChartDataSource = ((FourStapViewModel)resultStat3).organizationPieInfo;
                    ViewBag.PieChartZayavki = ((FourStapViewModel)resultStat3).zayavkiPieInfo;
                    ViewBag.PieChartPutyovki = ((FourStapViewModel)resultStat3).putyovkiPieInfo;
                    ViewBag.Chastichka = ((FourStapViewModel)resultStat3).chastichkaInfo;
                    break;
                case 14:
                    viewName = "StatFederasiyaV2";
                    var resultStat = StatDataContainer.GetStataData(ustinfo.DepartmentBase, ustinfo.CurYear);

                    ViewBag.LastChangeTime = StatDataContainer._modelChangedTime;
                    ViewBag.DataSource = ((FourStapViewModel)resultStat).kaznaInfo; 
                    ViewBag.DataUrgent       = ((FourStapViewModel)resultStat).urgentInfo;
                    ViewBag.DataPutyovkas    = ((FourStapViewModel)resultStat).putyovkaInfo;
                    ViewBag.RealizData       = ((FourStapViewModel)resultStat).realizInfo;
                    ViewBag.ZadoljennostData = ((FourStapViewModel)resultStat).zadoljInfo;
                    ViewBag.BronData = ((FourStapViewModel)resultStat).bronInfo;
                    ViewBag.ActiveUsersData = ((FourStapViewModel)resultStat).activeUsersInfo;
                    ViewBag.PieChartDataSource = ((FourStapViewModel)resultStat).organizationPieInfo;
                    ViewBag.PieChartZayavki = ((FourStapViewModel)resultStat).zayavkiPieInfo;
                    ViewBag.PieChartPutyovki = ((FourStapViewModel)resultStat).putyovkiPieInfo;
                    ViewBag.Chastichka = ((FourStapViewModel)resultStat).chastichkaInfo;
                    ViewBag.PostupleniyaLastDay = ((FourStapViewModel)resultStat).kaznaLastDayInfo;
                    ViewBag.Pererasxod = ((FourStapViewModel)resultStat).pererasxodInfo;
                    break;
                default:
                    viewName = "StatFederasiyaV2";
                    var resultStat2 = StatDataContainer.GetStataData(ustinfo.DepartmentBase, ustinfo.CurYear);

                    ViewBag.LastChangeTime = StatDataContainer._modelChangedTime;
                    ViewBag.DataSource = ((FourStapViewModel)resultStat2).kaznaInfo;
                    ViewBag.DataUrgent = ((FourStapViewModel)resultStat2).urgentInfo;
                    ViewBag.DataPutyovkas = ((FourStapViewModel)resultStat2).putyovkaInfo;
                    ViewBag.RealizData = ((FourStapViewModel)resultStat2).realizInfo;
                    ViewBag.ZadoljennostData = ((FourStapViewModel)resultStat2).zadoljInfo;
                    ViewBag.BronData = ((FourStapViewModel)resultStat2).bronInfo;
                    ViewBag.ActiveUsersData     = ((FourStapViewModel)resultStat2).activeUsersInfo;
                    ViewBag.PieChartDataSource = ((FourStapViewModel)resultStat2).organizationPieInfo;
                    ViewBag.PieChartZayavki = ((FourStapViewModel)resultStat2).zayavkiPieInfo;
                    ViewBag.PieChartPutyovki = ((FourStapViewModel)resultStat2).putyovkiPieInfo;
                    ViewBag.Chastichka = ((FourStapViewModel)resultStat2).chastichkaInfo;
                    ViewBag.PostupleniyaLastDay = ((FourStapViewModel)resultStat2).kaznaLastDayInfo;
                    ViewBag.Pererasxod = ((FourStapViewModel)resultStat2).pererasxodInfo;
                    break;
            }

            return View(viewName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetStatFederasiya()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var result = StatDataContainer.GetStataData(ustinfo.DepartmentBase, ustinfo.role_id.Value, ustinfo.CurYear, ustinfo.CurKvartal);

            var printable = (Stat_FederasiyaViewModel)result;

            log.Info("GetStatFederasiya==========================");
            log.Info(printable.VozvratPutyovkas);
            log.Info("GetStatFederasiya==========================");

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult StatOblSovProf()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var result = StatDataContainer.GetStataData(ustinfo.DepartmentBase, ustinfo.role_id.Value, ustinfo.CurYear, ustinfo.CurKvartal);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StatPPO()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var result = StatDataContainer.GetStataData(ustinfo.DepartmentBase, ustinfo.role_id.Value, ustinfo.CurYear, ustinfo.CurKvartal);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult StatRayonOtrasl()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var result = StatDataContainer.GetStataData(ustinfo.DepartmentBase, ustinfo.role_id.Value, ustinfo.CurYear, ustinfo.CurKvartal);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult Zayavok()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult Otchot()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult Admin()
        {
            return View();
        }


        #region ZayavkiNew

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUpperOrganization()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var kiat = new StructureOfKiat();

            var deptCode = kiat.GetParentOrganizationCode(ustinfo.DepartmentBase);

            var Name = kiat.GetParentOrganizationName(deptCode);

            return Json(Name, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        public JsonResult UploadZayavkaFiles(FormCollection formCollection)
        {
            log.Info("ReadFileZayavki");
            try
            {
                if (Request != null)
                {
                    long ZayavkaId = 0;

                    foreach (var key in formCollection.AllKeys)
                    {
                        if (key.Equals("ZayavkaID"))
                        {
                            ZayavkaId = int.Parse(formCollection[key]);
                        }
                    }

                    foreach (var fileNames in Request.Files)
                    {
                        string ffname = fileNames.ToString();

                        var dbFile = KasabaDB.tblFiles.Where(z => z.EXTERNAL_ID == ZayavkaId && z.OPNAME == ffname);

                        if (dbFile.Any()) continue;

                        HttpPostedFileBase file = Request.Files[fileNames.ToString()];

                        if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                        {
                            string fileName = file.FileName;
                            string fileContentType = file.ContentType;
                            byte[] fileBytes = new byte[file.ContentLength];
                            var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                            tblFile ffile = new tblFile
                            {
                                OP_TYPE = 1, //Zayavki
                                OPNAME = fileNames.ToString(),
                                NAME = fileName,
                                CONTENTTYPE = fileContentType,
                                DATA = fileBytes,
                                EXTERNAL_ID = ZayavkaId
                            };

                            KasabaDB.tblFiles.Add(ffile);
                        }
                    }

                    KasabaDB.SaveChanges();
                }
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zayavkaId"></param>
        /// <param name="OpName"></param>
        /// <returns></returns>
        public FileResult DownloadZayavkaFile(string zayavkaId, string OpName)
        {
            long CurrentFileID = Convert.ToInt64(zayavkaId);

            var CurrentFile = (from fls in KasabaDB.tblFiles
                                      where fls.EXTERNAL_ID == CurrentFileID && fls.OPNAME == OpName
                                      select fls).FirstOrDefault();

            return File(CurrentFile.DATA, CurrentFile.CONTENTTYPE, CurrentFile.NAME);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zayavkaId"></param>
        /// <returns></returns>
        public JsonResult GetFileInfoZayavka(string zayavkaId)
        {
            long CurrentFileID = Convert.ToInt64(zayavkaId);

            var ZayavkaFiles = from fls in KasabaDB.tblFiles
                               where fls.EXTERNAL_ID == CurrentFileID
                               select fls;

            var loadedFileInfo = new LoadedFilesZayavka();

            foreach (var file in ZayavkaFiles)
            {
                if (file.OPNAME.Equals("Passport"))
                {
                    loadedFileInfo.Passport = file.NAME;
                }
                if (file.OPNAME.Equals("LabourBook"))
                {
                    loadedFileInfo.LabourBook = file.NAME;
                }
                if (file.OPNAME.Equals("VocationOrder"))
                {
                    loadedFileInfo.VocationOrder = file.NAME;
                }
                if (file.OPNAME.Equals("PPO_Protokol"))
                {
                    loadedFileInfo.PPO_Protokol = file.NAME;
                }
            }

            return Json(loadedFileInfo, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}