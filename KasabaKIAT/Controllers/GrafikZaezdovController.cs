using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KasabaKIAT.Controllers
{
    [Authorize]
    public class GrafikZaezdovController : Controller
    {
        KasabaKIATEntities KasabaDB = new KasabaKIATEntities();
        public ActionResult Index()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            if (UserId == 0)
            {
                return RedirectToAction("SignOut", "Login");
            }
            else
            {
                return View();
            }
        }
        public JsonResult GetAllGrafikZaezdov_S(int id,int pt)
        {

            var list = (from e in KasabaDB.tbpt_zaezdi
                        join d in KasabaDB.t_sanatorium on e.SANATORY_ID equals d.id
                        join a in KasabaDB.tbcs_dictionary on e.PLACE_TYPE_ID equals a.REC_ID
                        where e.SANATORY_ID==id &&e.PLACE_TYPE_ID==pt
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = d.sanatorium_name,
                            SC_id = d.id,
                            PTI = a.NAME_UZ,
                            PTI_ID = a.REC_ID,
                            P = e.PRICE,
                            Q = e.QUANTITY
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
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