using KasabaKIAT.Models;
using KurortUrvServiceTest.ServiceInterAction.ActionLogic;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace KasabaKIAT.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class GrafikZaezdovController : Controller
    {
        /// <summary>
        /// Kurort Upravleniya External API 
        /// </summary>
        ServiceActionController service = new ServiceActionController();
        /// <summary>
        /// Logging 
        /// </summary>
        ILog log = log4net.LogManager.GetLogger(typeof(GrafikZaezdovController));
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
            int UserId = Convert.ToInt32(User.Identity.Name);
            if (UserId == 0)
            {
                return RedirectToAction("SignOut", "Login");
            }
            else
            {
                ViewBag.dataSource = null;

                return View();
            }
        }


        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult IndexV2()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            if (UserId == 0)
            {
                return RedirectToAction("SignOut", "Login");
            }
            else
            {


                ViewBag.dataSource = null;

                return View();
            }
        }

        /// <summary>
        /// External Api Load Grafik Sanatoriev
        /// </summary>
        /// <param name="yil">Year</param>
        public ActionResult ServiceGrafikSanatoryZaezdov(string yil)
        {
            log.Info("ServiceGrafikSanatoryZaezdov");
            log.Info("yil = " + yil);

            try
            {
                //service.LoadZaezdovSanatoriev(yil);

                var resultList = GetGrafikSanatoryZaezdov(int.Parse(yil));

                ViewBag.dataSource = resultList;
                ViewBag.Yil = yil;

                //return PartialView("GrafikZaezdovPV", resultList);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return View("IndexV2");
        }

        class GrafikZaezdovOut
        {
            public string SanatoryNaimenovanie { get; set; }

            public string SanatoryProfil { get; set; }

            public string SanatoryOblast { get; set; }

            public int? SanatoryKoekVsego { get; set; }

            public int? SanatoryKoekLetom { get; set; }

            public int? SanatoryKoekZimoy { get; set; }

            public string SanatoryRazmeshenieNaimenovanie { get; set; }

            public int? Putevok1kv { get; set; }

            public int? Putevok2kv { get; set; }

            public int? Putevok3kv { get; set; }

            public int? Putevok4kv { get; set; }
        }

        /// <summary>
        /// Get Sanatory Zaezdov List
        /// </summary>
        /// <param name="yil">Year</param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "yil", VaryByCustom = "userId")]
        private List<GrafikZaezdovOut> GetGrafikSanatoryZaezdov(int yil)
        {

            log.Info("GetGrafikSanatoryZaezdov");
            log.Info("yil = " + yil);

            try
            {
                var O = from o in KasabaDB.tbku_GrafikZaezdovSanatoriev
                        where o.GodZaezdov == yil
                        select new GrafikZaezdovOut
                        {
                            SanatoryNaimenovanie = o.SanatoryNaimenovanie,
                            SanatoryProfil = o.SanatoryProfil,
                            SanatoryOblast = o.SanatoryOblast,
                            SanatoryKoekVsego = o.SanatoryKoekVsego,
                            SanatoryKoekLetom = o.SanatoryKoekLetom,
                            SanatoryKoekZimoy = o.SanatoryKoekZimoy,
                            SanatoryRazmeshenieNaimenovanie = o.SanatoryRazmeshenieNaimenovanie,
                            Putevok1kv = o.Putevok1kv,
                            Putevok2kv = o.Putevok2kv,
                            Putevok3kv = o.Putevok3kv,
                            Putevok4kv = o.Putevok4kv
                        };

                return O.ToList();
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
        /// <param name="id"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "id;pt", VaryByCustom = "userId")]
        public JsonResult GetAllGrafikZaezdov_S(int id,int pt)
        {

            var list = (from e in KasabaDB.tbpt_zaezdi
                        join d in KasabaDB.tblSprav_San on e.SANATORY_ID equals d.ID
                        join a in KasabaDB.tbcs_dictionary on e.PLACE_TYPE_ID equals a.REC_ID
                        where e.SANATORY_ID==id &&e.PLACE_TYPE_ID==pt
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = d.Name_San,
                            SC_id = d.ID,
                            PTI = a.NAME_UZ,
                            PTI_ID = a.REC_ID,
                            P = e.PRICE,
                            Q = e.QUANTITY
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "id;pt", VaryByCustom = "userId")]
        public JsonResult GetAllGrafikZaezdov_C(int id,int pt)
        {
            var list = (from e in KasabaDB.tbpt_zaezdi
                        join d in KasabaDB.t_camp on e.CAMP_ID equals d.id
                        join a in KasabaDB.tbcs_dictionary on e.PLACE_TYPE_ID equals a.REC_ID
                        where e.CAMP_ID==id && e.PLACE_TYPE_ID==pt
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = d.camps_name,
                            SC_id=d.id,
                            PTI = a.NAME_UZ,
                            PTI_ID = a.REC_ID,
                            P = e.PRICE,
                            Q = e.QUANTITY
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gz"></param>
        /// <returns></returns>
        public string UpdateGrafikZaezdov(tbpt_zaezdi gz)
        {
            if (gz != null)
            {
                using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                {
                    int bookId = Convert.ToInt32(gz.REC_ID);
                    tbpt_zaezdi _gz = contextObj.tbpt_zaezdi.Where(b => b.REC_ID == bookId).FirstOrDefault();
                    _gz.ZAEZD_BEGIN = gz.ZAEZD_BEGIN;
                    _gz.ZAEZD_END = gz.ZAEZD_END;
                    _gz.USER_ID = Convert.ToInt32(Session["id"]);
                    _gz.PLACE_TYPE_ID = gz.PLACE_TYPE_ID;
                    _gz.QUANTITY = gz.QUANTITY;
                    _gz.PRICE = gz.PRICE;
                    _gz.SANATORY_ID = gz.SANATORY_ID;
                    _gz.CAMP_ID = gz.CAMP_ID;
                    contextObj.SaveChanges();
                    return "GZ record updated successfully";
                }
            }
            else
            {
                return "Invalid gz record";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gz"></param>
        /// <returns></returns>
        public string AddGrafikZaezdov(tbpt_zaezdi gz)
        {
            if (gz != null)
            {
                using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                {
                    tbpt_zaezdi z = new tbpt_zaezdi();
                    z.USER_ID = Convert.ToInt32(Session["id"]);
                    z.ZAEZD_BEGIN = gz.ZAEZD_BEGIN;
                    z.ZAEZD_END = gz.ZAEZD_END;
                    z.PLACE_TYPE_ID = gz.PLACE_TYPE_ID;
                    z.PRICE = gz.PRICE;
                    z.QUANTITY = gz.QUANTITY;
                    z.SANATORY_ID = gz.SANATORY_ID;
                    z.CAMP_ID = gz.CAMP_ID;
                    contextObj.tbpt_zaezdi.Add(z);
                    contextObj.SaveChanges();
                    return "Book record added successfully";
                }
            }
            else
            {
                return "Invalid book record";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public string DeleteGrafikZaezdov(string bookId)
        {

            if (!String.IsNullOrEmpty(bookId))
            {
                try
                {
                    int _bookId = Int32.Parse(bookId);
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        var _book = contextObj.tbpt_zaezdi.Find(_bookId);
                        contextObj.tbpt_zaezdi.Remove(_book);
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
    }
}