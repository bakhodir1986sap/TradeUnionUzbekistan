using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KasabaKIAT.Models;
using KurortUrvServiceTest.ServiceInterAction.ActionLogic;
using log4net;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data;
using KasabaKIAT.BusinessLogic.Putyovki.PrixodPutyovok;
using KasabaKiat.Helpers;
using KasabaKIAT.BusinessLogic.Security.Roles.RolePermissions;
using KasabaKIAT.BusinessLogic.Security.Roles.Roles;
using KasabaKiat.ViewModel;

namespace KasabaKIAT.Controllers
{
    /// <summary>
    /// Administration Controller,defines methods for User Management, Role management
    /// References management and others
    /// </summary>
    [Authorize]
    public class AdministirirovaniyeController : Controller
    {
        #region Properties
        /// <summary>
        /// Kurort Upravleniya External API 
        /// </summary>
        ServiceActionController service = new ServiceActionController();

        /// <summary>
        /// Logging 
        /// </summary>
        ILog log = log4net.LogManager.GetLogger(typeof(AdministirirovaniyeController));

        /// <summary>
        /// DataBase Model
        /// </summary>
        KasabaKIATEntities KasabaDB = new KasabaKIATEntities();

        #endregion

        #region Views
        /// <summary>
        /// StartUp Page for Administration Consist of Links to Users, Roles and References
        /// </summary>
        /// <returns>HTML Page</returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// User Management Page
        /// </summary>
        /// <returns>HTML</returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult UchetniyZapis()
        {
            return View();
        }

        /// <summary>
        /// Role Management Page
        /// </summary>
        /// <returns>HTML</returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult SozdaniyeRoley()
        {
            return View();
        }

        /// <summary>
        /// Transaction Make Reference 
        /// </summary>
        /// <returns>HTML</returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult SozdaniyeProvodok()
        {
            return View();
        }

        /// <summary>
        /// Transaction Link Reference
        /// </summary>
        /// <returns>HTML</returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult PrivyazkaProvodok()
        {
            return View();
        }

        /// <summary>
        /// References Management
        /// </summary>
        /// <returns>HTML</returns>
        public ActionResult Spravichniki()
        {
            return View();
        }

        /// <summary>
        /// Kurort Department References
        /// </summary>
        /// <returns>HTML</returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult SpravichnikiKurortnovoUpravleniya()
        {
            return View();
        }

        /// <summary>
        /// Sanatory Enterence Grafik Reference
        /// </summary>
        /// <returns>HTML</returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult GrafikSanatoryZaezdov()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult SredneyStoimostiPutevok()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult RazmesheniyaSanatorii()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult Otdixayushix()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult NaznacheniyaPlateja()
        {
            return View();
        }

        #endregion

        #region Methods and APIs

        #region RoleManagement
        /// <summary>
        /// Get Roles List
        /// </summary>
        /// <returns>HTML</returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetRoley()
        {
            log.Info("GetRoley");
            try
            {
                var O = from o in KasabaDB.roles
                        select new
                        {
                            d1 = o.id,
                            d2 = o.name,
                        };
                return Json(O.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// Add New Role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>Role Name</returns>
        public JsonResult AddRoley(string roleName)
        {
            log.Info("AddRoley");
            log.Info("roleName=" + roleName);
            try
            {
                var R = new Roles();
                R.EnterRole(roleName);
                return Json("yes", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// Get Role Permissions List
        /// </summary>
        /// <returns>JSON</returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetRoleys()
        {
            RolePermission vrp = new RolePermission();
            var data = vrp.GetRolePermitsAllList();
            log.Info("GetRoley");
            try
            {
                var O = from o in data
                        select new
                        {
                            d1 = o.id,
                            d2 = o.action,
                            d3 = o.role_id
                        };
                return Json(O.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// Get Role Permission List Filtered by Id
        /// </summary>
        /// <param name="id">Role Id</param>
        /// <returns>JSON</returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "id", VaryByCustom = "userId")]
        public JsonResult GetRoleys2(int id)
        {
            RolePermission vrp = new RolePermission();
            var data = vrp.GetRolePermitsByRoleList(id);
            log.Info("GetRoley");
            try
            {
                var O = from o in data
                        select new
                        {
                            d1 = o.id,
                            d2 = o.action,
                            d3 = o.role_id
                        };
                return Json(O.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return null;
        }

        /// <summary>
        /// Save Changes in Role Permission
        /// </summary>
        /// <param name="ids">Changed RolePermision Ids List</param>
        /// <param name="rolep_id">Owner Role ID</param>
        /// <returns>Result Message</returns>
        public string save(List<long> ids, long rolep_id)
        {
            RolePermission rp = new RolePermission();
            rp.EnterRolePermitN(ids, rolep_id);
            return "yes";
        }

        /// <summary>
        /// Remove Permission List
        /// </summary>
        /// <param name="ids">Changed RolePermision Ids List</param>
        /// <returns>Result Message</returns>
        public string Remove(List<long> ids)
        {
            RolePermission rp = new RolePermission();

            foreach (var id in ids)
            {
                rp.DeleteRolePermit(id);
            }

            return "yes";
        }

        #endregion
        
        #region Spravochniki SKU
        /// <summary>
        /// Refererence of Trade Union List
        /// </summary>
        /// <returns>HTML</returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult SpisokPravsoyuzov()
        {
            log.Info("SpisokPravsoyuzov");
            IList<Models.ViewModel> O = null;

            var UserId = Convert.ToInt32(User.Identity.Name);

            var userInfo = UserInfo.GetUserInfo(UserId);

            try
            {
                O = (from o in KasabaDB.tbku_StrokaProfsoyza
                     where o.MATCH_DEPARTMENTBASE != null  && o.MATCH_DEPARTMENTBASE.StartsWith(userInfo.DepartmentBase)
                                             select new Models.ViewModel
                                             {
                                                 Kod = o.ProfsoyzKod,
                                                 Name = o.ProfsoyzNaimenovanie
                                             }).ToList();

                log.Info("Count = " + O.ToList().Count); 
            }
            catch(Exception ex)
            {
                log.Info(ex);
            }

            return View(O);
        }

        /// <summary>
        /// Schet Faktura's List of Pravsoyus Organizations
        /// </summary>
        /// <returns>HTML</returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult SchetFaktura()
        {
            return View();
        }

        /// <summary>
        /// Enterence divition Grafik
        /// </summary>
        /// <returns>HTML</returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult RaspredeleniyeZaezdov()
        {
            return View();
        }


        /// <summary>
        /// Get Enterence Grafik
        /// </summary>
        /// <param name="yil">Year</param>
        /// <param name="kod">Sanatory Code</param>
        /// <returns></returns>
        [OutputCache(Duration = 3, Location = OutputCacheLocation.Client, VaryByParam = "yil;kod", VaryByCustom = "userId")]
        public JsonResult GetRaspredeleniyeZaezdov1(int yil,string kod)
        {

            log.Info("GetRaspredeleniyeZaezdov1");
            log.Info("yil = " + yil);
            log.Info("kod = " + kod);

            try
            {
                var O = from o in KasabaDB.tbku_GrafikZaezdovSanatoriev
                        where o.GodZaezdov == yil 
                        && o.SanatoryKod == kod
                        select new
                        {
                            d1 = o.SanatoryNaimenovanie,
                            d2 = o.SanatoryProfil,
                            d3 = o.SanatoryOblast,
                            d4 = o.SanatoryKoekVsego,
                            d5 = o.SanatoryKoekLetom,
                            d6 = o.SanatoryKoekZimoy,
                            d7 = o.SanatoryRazmeshenieNaimenovanie,
                            d8 = o.Putevok1kv,
                            d9 = o.Putevok2kv,
                            d10 = o.Putevok3kv,
                            d11 = o.Putevok4kv
                        };

                log.Info("Count = " + O.ToList().Count);

                return Json(O.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Info(ex);
            }

            return null;
        }

        /// <summary>
        /// Already Entered Divition
        /// </summary>
        /// <param name="yil">Year</param>
        /// <param name="kod">Sanatory Code</param>
        /// <returns></returns>
        [OutputCache(Duration = 3, Location = OutputCacheLocation.Client, VaryByParam = "yil;kod", VaryByCustom = "userId")]
        public JsonResult GetRaspredeleniyeZaezdov2(int yil, string kod)
        {
            log.Info("GetRaspredeleniyeZaezdov2");
            log.Info("yil = " + yil);
            log.Info("kod = " + kod);

            try
            {
                var O = from o in KasabaDB.tbku_SpisokRaspredeleniya
                        join pravsoyuz in KasabaDB.tbku_StrokaProfsoyza on
                        o.ProfsoyzKod equals pravsoyuz.ProfsoyzKod
                        where o.GodZaezdov == yil && o.SanatoryKod == kod
                        select new
                        {
                            ID = o.ID,
                            d1 = o.GodZaezdov,
                            d2 = o.SanatoryKod,
                            ProfKode = pravsoyuz.ProfsoyzKod,
                            d3 = pravsoyuz.ProfsoyzNaimenovanie,
                            RazmKod = KasabaDB.tbku_GrafikZaezdovSanatoriev.Where(z => z.SanatoryRazmeshenieKod == o.SanatoryRazmeshenieKod).FirstOrDefault().SanatoryRazmeshenieKod,
                            d4 = KasabaDB.tbku_GrafikZaezdovSanatoriev.Where(z=>z.SanatoryRazmeshenieKod == o.SanatoryRazmeshenieKod).FirstOrDefault().SanatoryRazmeshenieNaimenovanie,//zaezd.SanatoryRazmeshenieNaimenovanie,
                            d5 = o.Putevok1kv,
                            d6 = o.Putevok2kv,
                            d7 = o.Putevok3kv,
                            d8 = o.Putevok4kv
                        };

                var listRes = O.ToList();

                log.Info("Count = " + listRes.Count);

                return Json(listRes, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// Sanatory List from Enterence Grafik to Sanatories
        /// </summary>
        /// <param name="yil">Year</param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "yil", VaryByCustom = "userId")]
        public JsonResult GetSanatory(int yil)
        {
            log.Info("GetSanatory");
            log.Info("yil = " + yil);

            try
            {
                var O = from o in KasabaDB.tbku_GrafikZaezdovSanatoriev
                        where o.GodZaezdov == yil
                        select new
                        {
                            d1 = o.SanatoryKod,
                            d2 = o.SanatoryNaimenovanie,
                        };
                var Groups = from p in O
                             group p by new { p.d1, p.d2 } into g
                             select new { d1 = g.Key.d1, d2 = g.Key.d2 };
                return Json(Groups.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }
        
        /// <summary>
        /// Get Razmesheniya Type List
        /// </summary>
        /// <param name="yil">Year</param>
        /// <param name="kod">Sanatory Kod</param>
        /// <returns>JSON</returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "yil;kod", VaryByCustom = "userId")]
        public JsonResult GetKod(int yil,string kod)
        {

            log.Info("GetKod");
            log.Info("yil = " + yil);
            log.Info("kod = " + kod);

            try
            {
                var O = from o in KasabaDB.tbku_GrafikZaezdovSanatoriev
                        where o.GodZaezdov == yil && o.SanatoryKod == kod
                        select new
                        {
                            d1 = o.SanatoryRazmeshenieKod,
                            d2 = o.SanatoryRazmeshenieNaimenovanie,
                        };
                return Json(O.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;

        }

        /// <summary>
        /// Add Divition 
        /// </summary>
        /// <param name="PP"></param>
        /// <returns></returns>
        public string AddSpisokRaspredeleniya(tbku_SpisokRaspredeleniya PP)
        {
            log.Info("AddSpisokRaspredeleniya");

            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                if (PP != null)
                {
                    using (KasabaKIATEntities k = new KasabaKIATEntities())
                    {
                        tbku_SpisokRaspredeleniya p1 = new tbku_SpisokRaspredeleniya();
                        p1.GodZaezdov = PP.GodZaezdov;
                        p1.SanatoryKod = PP.SanatoryKod;
                        p1.ProfsoyzKod = PP.ProfsoyzKod;
                        p1.SanatoryRazmeshenieKod = PP.SanatoryRazmeshenieKod;
                        p1.Putevok1kv = PP.Putevok1kv;
                        p1.Putevok2kv = PP.Putevok2kv;
                        p1.Putevok3kv = PP.Putevok3kv;
                        p1.Putevok4kv = PP.Putevok4kv;
                        k.tbku_SpisokRaspredeleniya.Add(p1);
                        k.SaveChanges();
                        return "ok";
                    }
                }
                else
                {
                    return "error";
                }
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
        /// <param name="year"></param>
        public void SendData2Kurort(int year)
        {
            var service = new ServiceActionController();

            service.SendRaspredeleniya(year);
        }

        private class TotalRes
        {
            public decimal Postupleniya { get; set; }

            public decimal ZaplanirovannaNaSummu { get; set; }


        }

        public JsonResult GetProvData(int year, string profCode)
        {
            //Get total for this year by profCode => tbku_strokaProfsoyuz

            //Get Avarage price for this year
            var tot = new TotalRes();

            var profs = KasabaDB.tbku_StrokaProfsoyza.Where(z => z.ProfsoyzKod == profCode);

            if (!profs.Any()) return Json("invalid ProfCode", JsonRequestBehavior.AllowGet);

            var depCode = profs.First().MATCH_DEPARTMENTBASE;

            //ECP postupleniye
            var ecps = KasabaDB.tbbu_ecp_calc.Where(e => e.OBL_DEPARTMENT_BASE == depCode && e.OPERDATE.Value.Year == year);

            if (ecps.Any())
            {
                tot.Postupleniya = ecps.Sum(z => z.OBLSOVPROF_AMT.Value);
            }
            else
            {
                tot.Postupleniya = 0;
            }

            //Chastichka postupleniye
            var chastichkas = KasabaDB.tbgl_bankpayments.Where(z => z.DEPARTMENT_BASE == depCode && z.TRAN_DATE.Year == year);


            if (chastichkas.Any())
            {
                tot.Postupleniya += Decimal.Multiply(chastichkas.Sum(z => z.AMOUNT), 0.7M);
            }
            else
            {
                tot.Postupleniya += 0;
            }

            var plans = KasabaDB.tbku_SpisokRaspredeleniya.Where(p => p.GodZaezdov == year && p.ProfsoyzKod == profCode);


            var avarages = KasabaDB.tbpt_avarageprice.Where(y => y.SET_DATE.Value.Year == year);

            decimal avatagePutPrice = 0;

            if (avarages.Any())
            {
                avatagePutPrice = avarages.First().AMOUNT.Value;
            }

            if (plans.Any())
            {
                tot.ZaplanirovannaNaSummu = (plans.Sum(s => s.Putevok1kv.Value) + plans.Sum(s => s.Putevok2kv.Value)
                    + plans.Sum(s => s.Putevok3kv.Value) + plans.Sum(s => s.Putevok4kv.Value)) * avatagePutPrice;
            }
            else
            {
                tot.ZaplanirovannaNaSummu = 0;
            }
            
            return Json(tot, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Modify Divition
        /// </summary>
        /// <param name="PP"></param>
        /// <returns></returns>
        public string UpdateSpisokRaspredeleniya(tbku_SpisokRaspredeleniya PP)
        {
            log.Info("UpdateSpisokRaspredeleniya");

            try
            {
                if (PP != null)
                {
                    using (KasabaKIATEntities k = new KasabaKIATEntities())
                    {
                        tbku_SpisokRaspredeleniya p1 = k.tbku_SpisokRaspredeleniya.Where(z=>z.ID == PP.ID).FirstOrDefault();

                        if (p1 == null) return "error";

                        p1.GodZaezdov = PP.GodZaezdov;
                        p1.SanatoryKod = PP.SanatoryKod;
                        p1.ProfsoyzKod = PP.ProfsoyzKod;
                        p1.SanatoryRazmeshenieKod = PP.SanatoryRazmeshenieKod;
                        p1.Putevok1kv = PP.Putevok1kv;
                        p1.Putevok2kv = PP.Putevok2kv;
                        p1.Putevok3kv = PP.Putevok3kv;
                        p1.Putevok4kv = PP.Putevok4kv;

                        k.SaveChanges();

                        return "ok";
                    }
                }
                else
                {
                    return "error";
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return "";
        }

        /// <summary>
        /// Remove Divition
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteSpisokRaspredeleniya(string id)
        {
            log.Info("DeleteSpisokRaspredeleniya");

            try
            {
                if (!String.IsNullOrEmpty(id))
                {
                    try
                    {
                        int _bookId = Int32.Parse(id);
                        using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                        {
                            var _book = contextObj.tbku_SpisokRaspredeleniya.Find(_bookId);
                            contextObj.tbku_SpisokRaspredeleniya.Remove(_book);
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

        /// <summary>
        /// Load Pravsoyuz List from Kurort Department
        /// </summary>
        public void ServiceSpisokPravsoyuzov()
        {
            log.Info("ServiceSpisokPravsoyuzov");

            try
            {
                service.LoadSpiskaProfsoyzov();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        /// <summary>
        /// Get PravSoyuz List
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client)]
        public JsonResult GetSpisokPravsoyuzov()
        {
            log.Info("GetSpisokPravsoyuzov");

            var UserId = Convert.ToInt32(User.Identity.Name);

            var userInfo = UserInfo.GetUserInfo(UserId);

            try
            {
                var O = from o in KasabaDB.tbku_StrokaProfsoyza
                        where o.MATCH_DEPARTMENTBASE != null && o.MATCH_DEPARTMENTBASE.StartsWith(userInfo.DepartmentBase)
                        select new
                        {
                            d1 = o.ProfsoyzKod,
                            d2 = o.ProfsoyzNaimenovanie
                        };
                return Json(O.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;

        }

        /// <summary>
        /// External Api Load Grafik Sanatoriev
        /// </summary>
        /// <param name="yil">Year</param>
        public void ServiceGrafikSanatoryZaezdov(string yil)
        {
            log.Info("ServiceGrafikSanatoryZaezdov");
            log.Info("yil = " + yil);

            try
            {
                service.LoadZaezdovSanatoriev(yil);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        /// <summary>
        /// Get Sanatory Zaezdov List
        /// </summary>
        /// <param name="yil">Year</param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "yil", VaryByCustom = "userId")]
        public JsonResult GetGrafikSanatoryZaezdov(int yil)
        {

            log.Info("GetGrafikSanatoryZaezdov");
            log.Info("yil = " + yil);

            try
            {
                var O = from o in KasabaDB.tbku_GrafikZaezdovSanatoriev
                        where o.GodZaezdov == yil
                        select new
                        {
                            d1 = o.SanatoryNaimenovanie,
                            d2 = o.SanatoryProfil,
                            d3 = o.SanatoryOblast,
                            d4 = o.SanatoryKoekVsego,
                            d5 = o.SanatoryKoekLetom,
                            d6 = o.SanatoryKoekZimoy,
                            d7 = o.SanatoryRazmeshenieNaimenovanie,
                            d8 = o.Putevok1kv,
                            d9 = o.Putevok2kv,
                            d10 = o.Putevok3kv,
                            d11 = o.Putevok4kv
                        };
                return Json(O.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;

        }

        /// <summary>
        /// Kurort Department External API
        /// </summary>
        /// <param name="B">Begin date</param>
        /// <param name="E">End Date</param>
        /// <param name="p">PravSoyuz Kod</param>
        public void ServiceSchetFaktura(DateTime B,DateTime E, string p)
        {
            log.Info("ServiceSchetFaktura");
            log.Info("B = " + B);
            log.Info("E = " + E);
            log.Info("p = " + p);

            try
            {
                service.LoadSchetFactura(B, E, p);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        /// <summary>
        /// Oprixodovaniye Schet Fakturi
        /// </summary>
        /// <param name="B">Begin date</param>
        /// <param name="E">End Date</param>
        /// <param name="p">PravSoyuz Kod</param>
        public void OprixodovaniyaSchetFaktura(DateTime B, DateTime E, string p)
        {

            log.Info("OprixodovaniyaSchetFaktura");
            log.Info("B = " + B);
            log.Info("E = " + E);
            log.Info("p = " + p);

            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);

                var ustinfo = UserInfo.GetUserInfo(UserId);

                var op = new Oprixodovaniye();

                op.PrixodKurort(B, E, p, UserId, ustinfo.DepartmentBase);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        /// <summary>
        /// Get Main Row Of Schet Faktura 
        /// </summary>
        /// <param name="B">Begin date</param>
        /// <param name="E">End Date</param>
        /// <param name="p">PravSoyuz Kod</param>
        /// <returns>JSON</returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "B;E;p", VaryByCustom = "userId")]
        public JsonResult GetSchetFaktura(DateTime B,DateTime E,string p)
        {
            log.Info("GetSchetFaktura");
            log.Info("B = " + B);
            log.Info("E = " + E);
            log.Info("p = " + p);

            DateTime aperiodOkonchaniye = E;
            TimeSpan ts = new TimeSpan(23, 59, 59);
            aperiodOkonchaniye = aperiodOkonchaniye.Date + ts;

            try
            {
                var O = from o in KasabaDB.tbku_DocumentSchetFaktura
                        where o.DocData >= B && o.DocData <= aperiodOkonchaniye && o.ProfsoyzKod == p
                        select new
                        {
                            d0 = o.ID,
                            d1 = o.DocNomer,
                            d2 = o.DocData,
                            d3 = o.DocTypeRealVozv,
                            d4 = o.ProfsoyzDogovor,
                            d5 = o.DataKvartalaPutevok,
                            d6 = o.ProfsoyzDoverennost,
                            d7 = o.ProfsoyzDoverennostData,
                            d8 = o.ProfsoyzDoverennostMOL,
                            d9 = o.SummaDoc,
                            d10 = o.ProcentNacenki,
                            d11 = o.SummaNacenki,
                            d12 = o.KolichestvoPutevok,
                            d13 = o.TovarOtpustil,
                        };
                return Json(O.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;

        }

        /// <summary>
        /// Get Detail Schet Fakturi
        /// </summary>
        /// <param name="id">Main SchetFaktura Dokument Id</param>
        /// <returns>JSON</returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "id", VaryByCustom = "userId")]
        public JsonResult GetStrokaSchetFakturi(int id)
        {
            log.Info("GetStrokaSchetFakturi");
            log.Info("id = " + id);

            try
            {
                var O = from o in KasabaDB.tbku_StrokaSchetFakturi
                        where o.SchetFactura_ID == id
                        select new
                        {
                            d0 = o.ID,
                            d1 = o.SchetFactura_ID,
                            d2 = o.SanatoryNaimenovanie,
                            d3 = o.SanatoryRazmeshenieNaimenovanie,
                            d4 = o.SanatoryType,
                            d5 = o.DataZaezda,
                            d6 = o.Blank1,
                            d7 = o.Blank2,
                            d8 = o.Kolichestvo,
                            d9 = o.Sebestoimost,
                            d10 = o.SummaSebestoimosti,
                            d11 = o.ProcentNacenki,
                            d12 = o.Cena,
                            d13 = o.Summa,
                        };
                return Json(O.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        #endregion

        #region Reference Management

        /// <summary>
        /// Naznacheniya plateja
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetSNP()
        {
            log.Info("GetSNP");

            try
            {
                var camp = from dict in KasabaDB.tbcs_dictionary
                           join dictType in KasabaDB.tbcs_dictionarytype on
                           dict.DICTIONARY_TYPE_ID equals dictType.REC_ID
                           where dictType.CODE == "NaznacheniyaPlateja"
                           select new
                           {
                               REC_ID = dict.REC_ID,
                               NAME_UZ = dict.NAME_UZ
                           };

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
        public JsonResult GetSPP()
        {

            log.Info("GetSPP");


            try
            {
                var camp = from dict in KasabaDB.tbcs_dictionary
                           join dictType in KasabaDB.tbcs_dictionarytype on
                           dict.DICTIONARY_TYPE_ID equals dictType.REC_ID
                           where dictType.CODE == "order"
                           select new
                           {
                               REC_ID = dict.REC_ID,
                               NAME_UZ = dict.NAME_UZ
                           };

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
        public JsonResult GetSSPO()
        {
            log.Info("GetSSPO");

            try
            {
                var camp = from dict in KasabaDB.tbcs_dictionary
                           join dictType in KasabaDB.tbcs_dictionarytype on
                           dict.DICTIONARY_TYPE_ID equals dictType.REC_ID
                           where dictType.CODE == "Otdixayushix"
                           select new
                           {
                               REC_ID = dict.REC_ID,
                               NAME_UZ = dict.NAME_UZ
                           };
                return Json(camp, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;

        }


        public string AddSTRS(tbcs_dictionary pz)
        {
            log.Info("AddSTRS");

            try
            {
                if (pz != null)
                {
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        tbcs_dictionary z = new tbcs_dictionary();
                        tbcs_dictionarytype pti = contextObj.tbcs_dictionarytype.Where(b => b.CODE == "placetype").FirstOrDefault();
                        z.USER_ID = Convert.ToInt32(User.Identity.Name);
                        z.NAME_RU = pz.NAME_RU;
                        z.NAME_UZ = pz.NAME_UZ;
                        z.NAME_LT = pz.NAME_LT;
                        z.STATUS = true;
                        z.DICTIONARY_TYPE_ID = pti.REC_ID;
                        z.CODE = z.REC_ID.ToString();
                        contextObj.tbcs_dictionary.Add(z);
                        contextObj.SaveChanges();
                        return "tbcs_dictionary record added successfully";
                    }
                }
                else
                {
                    return "Invalid tbcs_dictionary record";
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        public string AddSNP(tbcs_dictionary pz)
        {
            log.Info("AddSNP");

            try
            {
                if (pz != null)
                {
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        tbcs_dictionary z = new tbcs_dictionary();
                        tbcs_dictionarytype pti = contextObj.tbcs_dictionarytype.Where(b => b.CODE == "NaznacheniyaPlateja").FirstOrDefault();
                        z.USER_ID = Convert.ToInt32(User.Identity.Name);
                        z.NAME_RU = pz.NAME_RU;
                        z.NAME_UZ = pz.NAME_UZ;
                        z.NAME_LT = pz.NAME_LT;
                        z.STATUS = true;
                        z.DICTIONARY_TYPE_ID = pti.REC_ID;
                        z.CODE = z.REC_ID.ToString();
                        contextObj.tbcs_dictionary.Add(z);
                        contextObj.SaveChanges();
                        return "tbcs_dictionary record added successfully";
                    }
                }
                else
                {
                    return "Invalid tbcs_dictionary record";
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        public string AddSSPO(tbcs_dictionary pz)
        {
            log.Info("AddSSPO");

            try
            {
                if (pz != null)
                {
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        tbcs_dictionary z = new tbcs_dictionary();
                        tbcs_dictionarytype pti = contextObj.tbcs_dictionarytype.Where(b => b.CODE == "Otdixayushix").FirstOrDefault();
                        z.USER_ID = Convert.ToInt32(User.Identity.Name);
                        z.NAME_RU = pz.NAME_RU;
                        z.NAME_UZ = pz.NAME_UZ;
                        z.NAME_LT = pz.NAME_LT;
                        z.STATUS = true;
                        z.DICTIONARY_TYPE_ID = pti.REC_ID;
                        z.CODE = z.REC_ID.ToString();
                        contextObj.tbcs_dictionary.Add(z);
                        contextObj.SaveChanges();
                        return "tbcs_dictionary record added successfully";
                    }
                }
                else
                {
                    return "Invalid tbcs_dictionary record";
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        public string UpdateSTRS(tbcs_dictionary pz)
        {

            log.Info("UpdateSTRS");

            try
            {
                if (pz != null)
                {
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        int bookId = Convert.ToInt32(pz.REC_ID);
                        tbcs_dictionary z = contextObj.tbcs_dictionary.Where(b => b.REC_ID == bookId).FirstOrDefault();
                        z.USER_ID = Convert.ToInt32(User.Identity.Name);
                        z.NAME_RU = pz.NAME_RU;
                        z.NAME_UZ = pz.NAME_UZ;
                        z.NAME_LT = pz.NAME_LT;
                        z.CODE = z.REC_ID.ToString();
                        z.STATUS = true;
                        contextObj.SaveChanges();
                        return "tbcs_dictionary record updated successfully";
                    }
                }
                else
                {
                    return "Invalid tbcs_dictionary record";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        public string DeleteSTRS(string id)
        {

            log.Info("DeleteSTRS");
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
                            var _book = contextObj.tbcs_dictionary.Find(_bookId);
                            contextObj.tbcs_dictionary.Remove(_book);
                            contextObj.SaveChanges();
                            return "sucessfully";
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        return "not found";
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

            return null;
 
        }

        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetSSSP()
        {
            log.Info("GetSSSP");

            try
            {
                var camp = from dict in KasabaDB.tbpt_avarageprice
                           select new
                           {
                               ID = dict.REC_ID,
                               ST = dict.SET_DATE,
                               A = dict.AMOUNT
                           };

                return Json(camp, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;

        }

        public string AddSSSP(tbpt_avarageprice pz)
        {
            log.Info("AddSSSP");

            try
            {
                if (pz != null)
                {
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        tbpt_avarageprice z = new tbpt_avarageprice();
                        z.SET_DATE = pz.SET_DATE;
                        z.AMOUNT = pz.AMOUNT;
                        z.USER_ID = Convert.ToInt32(User.Identity.Name);
                        contextObj.tbpt_avarageprice.Add(z);
                        contextObj.SaveChanges();
                        return "tbcs_dictionary record added successfully";
                    }
                }
                else
                {
                    return "Invalid tbcs_dictionary record";
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }
        public string UpdateSSSP(tbpt_avarageprice pz)
        {
            log.Info("UpdateSSSP");

            try
            {
                if (pz != null)
                {
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        int bookId = Convert.ToInt32(pz.REC_ID);
                        tbpt_avarageprice z = contextObj.tbpt_avarageprice.Where(b => b.REC_ID == bookId).FirstOrDefault();
                        z.SET_DATE = pz.SET_DATE;
                        z.AMOUNT = pz.AMOUNT;
                        z.USER_ID = Convert.ToInt32(User.Identity.Name);
                        contextObj.SaveChanges();
                        return "tbcs_dictionary record updated successfully";
                    }
                }
                else
                {
                    return "Invalid tbcs_dictionary record";
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }


        public string DeleteSSSP(string id)
        {
            log.Info("DeleteSSSP");
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
                            var _book = contextObj.tbpt_avarageprice.Find(_bookId);
                            contextObj.tbpt_avarageprice.Remove(_book);
                            contextObj.SaveChanges();
                            return "sucessfully";
                        }
                    }
                    catch (Exception)
                    {
                        return "not found";
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

            return null;
        }

        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetDB()
        {
            log.Info("GetDB");

            try
            {
                var data = from dp in KasabaDB.t_departmentbase
                           select new
                           {
                               id = dp.code,
                               value = dp.name_uz
                           };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetSP()
        {
            log.Info("GetSP");

            try
            {
                var camp = from dict in KasabaDB.provodok
                           join dp in KasabaDB.t_departmentbase on dict.DEPARTMENT_BASE equals dp.code
                           select new
                           {
                               ID = dict.id,
                               INN = dp.inn,
                               O = dp.name_uz,
                               D = dict.description,
                               SB = dict.score_begin,
                               SE = dict.score_end
                           };

                return Json(camp, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Info(ex);
            }

            return null;

        }
        public string AddSP(provodok pz)
        {
            log.Info("AddSP");

            try
            {
                if (pz != null)
                {
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        provodok z = new provodok();
                        z.DEPARTMENT_BASE = LOAD_DEPARTMENT_CODE(pz.DEPARTMENT_BASE);
                        z.description = pz.description;
                        z.score_begin = pz.score_begin;
                        z.score_end = pz.score_end;
                        z.USER_ID = Convert.ToInt32(User.Identity.Name);
                        contextObj.provodok.Add(z);
                        contextObj.SaveChanges();
                        return "tbcs_dictionary record added successfully";
                    }
                }
                else
                {
                    return "Invalid tbcs_dictionary record";
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }
        public string UpdateSP(provodok pz)
        {
            log.Info("UpdateSP");

            try
            {
                if (pz != null)
                {
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        int bookId = Convert.ToInt32(pz.id);
                        provodok z = contextObj.provodok.Where(b => b.id == bookId).FirstOrDefault();
                        z.DEPARTMENT_BASE = LOAD_DEPARTMENT_CODE(pz.DEPARTMENT_BASE);
                        z.description = pz.description;
                        z.score_begin = pz.score_begin;
                        z.score_end = pz.score_end;
                        z.USER_ID = Convert.ToInt32(User.Identity.Name);
                        contextObj.SaveChanges();
                        return "tbcs_dictionary record updated successfully";
                    }
                }
                else
                {
                    return "Invalid tbcs_dictionary record";
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }
        public string DeleteSP(string id)
        {
            log.Info("DeleteSP");

            try
            {
                if (!String.IsNullOrEmpty(id))
                {
                    try
                    {
                        int _bookId = Int32.Parse(id);
                        using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                        {
                            var _book = contextObj.provodok.Find(_bookId);
                            contextObj.provodok.Remove(_book);
                            contextObj.SaveChanges();
                            return "sucessfully";
                        }
                    }
                    catch (Exception)
                    {
                        return "not found";
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

            return null;
        }

        private class PrivyazkaProvodokL
        {
            public int ID            {get; set;}
            public string D          {get; set;}
            public string SB         {get; set;}
            public string SE         {get; set;}
            public bool PrivExists   {get; set;}
        }

        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "o_id;r_id", VaryByCustom = "userId")]
        public JsonResult GetPP(int o_id,string r_id)
        {
            log.Info("GetPP");
            log.Info("o_id = " + o_id);
            log.Info("r_id = " + r_id);

            var list = new List<PrivyazkaProvodokL>();

            try
            {
                var query = from dict in KasabaDB.provodok
                           select new PrivyazkaProvodokL
                           {
                               ID           = dict.id,
                               D            = dict.description,
                               SB           = dict.score_begin,
                               SE           = dict.score_end,
                               PrivExists   = false
                           };

                foreach(var provodka in query)
                {
                    var privyazka = KasabaDB.p_provodok.Where(z => z.order == o_id 
                    && z.REGION_ID == r_id 
                    && z.score_begin == provodka.SB
                    && z.score_end == provodka.SE);

                    if (privyazka.Any())
                    {
                        provodka.PrivExists = true;
                    }

                    list.Add(provodka);
                }

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }
        public string AddPP(p_provodok pz)
        {
            log.Info("AddPP");

            try
            {
                if (pz != null)
                {
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        p_provodok z = new p_provodok();
                        z.DEPARTMENT_BASE = LOAD_DEPARTMENT_CODE(pz.DEPARTMENT_BASE);
                        z.description = pz.description;
                        z.score_begin = pz.score_begin;
                        z.score_end = pz.score_end;
                        z.order = pz.order;
                        z.REGION_ID = pz.REGION_ID;
                        z.USER_ID = Convert.ToInt32(User.Identity.Name);
                        contextObj.p_provodok.Add(z);
                        contextObj.SaveChanges();
                        return "tbcs_dictionary record added successfully";
                    }
                }
                else
                {
                    return "Invalid tbcs_dictionary record";
                }
            }
            catch(Exception ex)
            {
                log.Info(ex);
            }

            return null;
        }
        public string UpdatePP(p_provodok pz)
        {
            log.Info("UpdatePP");

            try
            {
                if (pz != null)
                {
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        int bookId = Convert.ToInt32(pz.id);
                        p_provodok z = contextObj.p_provodok.Where(b => b.id == bookId).FirstOrDefault();
                        z.DEPARTMENT_BASE = LOAD_DEPARTMENT_CODE(pz.DEPARTMENT_BASE);
                        z.description = pz.description;
                        z.score_begin = pz.score_begin;
                        z.score_end = pz.score_end;
                        z.order = pz.order;
                        z.REGION_ID = pz.REGION_ID;
                        z.USER_ID = Convert.ToInt32(User.Identity.Name);
                        contextObj.SaveChanges();
                        return "tbcs_dictionary record updated successfully";
                    }
                }
                else
                {
                    return "Invalid tbcs_dictionary record";
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }
        public string DeletePP(string id)
        {
            log.Info("DeletePP");
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
                            var _book = contextObj.p_provodok.Find(_bookId);
                            contextObj.p_provodok.Remove(_book);
                            contextObj.SaveChanges();
                            return "sucessfully";
                        }
                    }
                    catch (Exception)
                    {
                        return "not found";
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

            return null;
        }

        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn", VaryByCustom = "userId")]
        public string LOAD_DEPARTMENT_CODE(string inn)
        {
            log.Info("LOAD_DEPARTMENT_CODE");
            log.Info("inn = " + inn);

            try
            {
                string name = "";
                var a = from d in KasabaDB.t_departmentbase orderby d.id select d;
                foreach (var v in a)
                {
                    if (v.inn == inn)
                    {
                        name = v.code;
                        break;
                    }
                }
                return name;
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;

        }

        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn", VaryByCustom = "userId")]
        public string LoadINN(int inn)
        {
            log.Info("LoadINN");
            log.Info("inn = " + inn);

            try
            {
                string strInn = inn.ToString();
                var query = from department in KasabaDB.t_departmentbase
                            where department.inn == strInn
                            select department.name_uz;
                if (query.Any())
                {
                    return query.First();
                }
                return "No Data Found";
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return null;

        }
        #endregion

        #region User Management
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetUZ()
        {
            log.Info("GetUZ");

            List<UserViewModel> list = new List<UserViewModel>();

            try
            {
                var Users = from u in KasabaDB.users
                        //join d in KasabaDB.t_departmentbase on u.bkut_id equals d.NATIVE_ID
                        join r in KasabaDB.roles on u.role_id equals r.id
                        //join rg in KasabaDB.sp_soato on u.REGION_ID equals rg.code
                        //join ot in KasabaDB.sp_branch on u.BRANCH_ID equals ot.id
                        where u.s_status == "ACTIVE"
                        select new
                        {
                            ID = u.id,
                            //O_id = d.id,
                            //O = d.name_uz,
                            Rl_id = r.id,
                            Rl = r.name,
                            L = u.email,
                            //Rg_id = rg.code,
                            //Rg = rg.name_uz,
                            //Ot_id = ot.id,
                            //Ot = ot.name_uz,
                            N = u.user_name,
                            D = u.created,
                            P = u.password,
                            bkut_id = u.bkut_id,
                            dep_id = u.dep_id,
                            bkuq_id = u.bkuq_id
                        };

                foreach(var user in Users)
                {
                    var uu = new UserViewModel
                    {
                        ID = user.ID,
                        Rl_id = user.Rl_id,
                        Rl = user.Rl,
                        L = user.L,
                        N = user.N,
                        D = user.D,
                        P = user.P
                    };

                    long departmentId = 0;

                    if (user.bkut_id.HasValue)
                    {
                        departmentId = user.bkut_id.Value;
                    }
                    else if (user.dep_id.HasValue)
                    {
                        departmentId = user.dep_id.Value;
                    }
                    else if (user.bkuq_id.HasValue)
                    {
                        departmentId = user.bkuq_id.Value;
                    }

                    var departments = KasabaDB.t_departmentbase.Where(z => z.id == departmentId);

                    if (departments.Any())
                    {
                        var dept = departments.First();

                        uu.O_id = dept.id;

                        if (!string.IsNullOrEmpty(dept.name_ru))
                        {
                            uu.O = dept.name_ru;
                        }
                        else if (!string.IsNullOrEmpty(dept.bkut_name))
                        {
                            uu.O = dept.bkut_name;
                        }
                        else if (!string.IsNullOrEmpty(dept.dep_name))
                        {
                            uu.O = dept.dep_name;
                        }
                    }

                    list.Add(uu);
                }


                return Json(list, JsonRequestBehavior.AllowGet);
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
        /// <param name="pz"></param>
        /// <returns></returns>
        public string AddUZ(users pz)
        {
            log.Info("AddUZ");

            try
            {
                if (pz != null)
                {
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        //users z = new users();
                        //z.version = 0;
                        //z.account_expired = false;
                        //z.account_locked = false;
                        //z.changed = DateTime.Now;
                        //z.created = DateTime.Now;
                        //z.email = pz.email;
                        //z.last_logged = DateTime.Now;
                        //z.last_logged_id = HttpContext.Server.ToString();
                        //z.state = "ACTIVE";
                        //z.password = MD5.MD5Hash(pz.password);
                        //z.salt = "";
                        //z.language = "RU";
                        //z.is_super_admin = false;
                        //z.user_name = pz.user_name;
                        //z.s_comment = "";
                        //z.s_create = DateTime.Now;
                        //z.s_status = "ACTIVE";
                        //z.s_user_id = 12;
                        //z.usertype = "BKUT";
                        //z.bkut_id = pz.bkut_id;
                        //z.role_id = pz.role_id;
                        //z.REGION_ID = pz.REGION_ID;
                        //z.BRANCH_ID = pz.BRANCH_ID;

                        //contextObj.users.Add(z);

                        contextObj.userAdd(pz.email, pz.password, pz.bkut_id, pz.role_id);

                        contextObj.SaveChanges();

                        return "User record added successfully";
                    }
                }
                else
                {
                    return "Invalid User record";
                }
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
        /// <param name="pz"></param>
        /// <returns></returns>
        public string UpdateUZ(users pz)
        {
            log.Info("UpdateUZ");

            try
            {
                if (pz != null)
                {
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        //int bookId = Convert.ToInt32(pz.id);
                        //users z = contextObj.users.Where(b => b.id == bookId).FirstOrDefault();
                        //z.version = 0;
                        //z.account_expired = false;
                        //z.account_locked = false;
                        //z.changed = DateTime.Now;
                        //z.created = DateTime.Now;
                        //z.email = pz.email;
                        //z.last_logged = DateTime.Now;
                        //z.last_logged_id = HttpContext.Server.ToString();
                        //z.state = "ACTIVE";
                        //z.password = MD5.MD5Hash(pz.password);
                        //z.salt = "";
                        //z.language = "UZ";
                        //z.is_super_admin = false;
                        //z.user_name = pz.user_name;
                        //z.s_comment = "";
                        //z.s_create = DateTime.Now;
                        //z.s_status = "ACTIVE";
                        //z.s_user_id = 12;
                        //z.usertype = "BKUT";
                        //z.bkut_id = pz.bkut_id;
                        //z.role_id = pz.role_id;
                        //z.REGION_ID = pz.REGION_ID;
                        //z.BRANCH_ID = pz.BRANCH_ID;

                        int userId = Convert.ToInt32(pz.id);
                        contextObj.userUpdate(userId, pz.email, pz.password, pz.bkut_id, pz.role_id);

                        contextObj.SaveChanges();
                        return "User record updated successfully";
                    }
                }
                else
                {
                    return "User tbcs_dictionary record";
                }
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
        public string DeleteUZ(string id)
        {
            log.Info("DeleteUZ");

            try
            {
                if (!String.IsNullOrEmpty(id))
                {
                    try
                    {
                        int UserId = Int32.Parse(id);
                        using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                        {
                            var user = contextObj.users.Find(UserId);

                            user.s_status = "INACTIVE";

                            contextObj.SaveChanges();

                            return "sucessfully";
                        }
                    }
                    catch (Exception)
                    {
                        return "not found";
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

            return null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GelOrganizatsiya()
        {
            var O = from department in KasabaDB.t_departmentbase
                    join org in KasabaDB.tblSprav_Org on department.code equals org.OWNER_CODE
                    where department.name_uz!=null
                    select new
                    {
                        id = department.id,
                        value = org.Name_Org
                    };

            return Json(O.ToList(), JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetRoles()
        {
            var R = from r in KasabaDB.roles
                    select new
                    {
                        id = r.id,
                        value = r.name
                    };
            return Json(R.ToList(), JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetOtrasl()
        {
            var O = from o in KasabaDB.sp_branch
                    select new
                    {
                        id = o.id,
                        value = o.name_uz
                    };
            return Json(O.ToList(), JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetRegion()
        {
            var UserId = Convert.ToInt32(User.Identity.Name);

            var userInfo = UserInfo.GetUserInfo(UserId);

            var R = from r in KasabaDB.sp_soato
                    where r.parent_code == "17"
                    select new
                    {
                        id = r.code,
                        value = r.name_uz
                    };
            return Json(R, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion
    }
}