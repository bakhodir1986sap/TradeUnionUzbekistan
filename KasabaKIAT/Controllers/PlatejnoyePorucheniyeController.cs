using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KasabaKIAT.Models;

namespace KasabaKIAT.Controllers
{
    //[Authorize(Roles = "ADMIN")]
    [Authorize]
    public class PlatejnoyePorucheniyeController : Controller
    {
        // GET: PlatejnoyePorucheniye
        KasabaKIATEntities KasabaDB = new KasabaKIATEntities();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetPlatejnoyePorucheniye()
        {
            var list = from pp in KasabaDB.tbgl_jnlmst
                       join p in KasabaDB.tbgl_jnldd on pp.DYSEQ equals p.DYSEQ
                       //join eg in KasabaDB.t_egrpo on p.INN equals eg.inn
                       //join ac in KasabaDB.t_egrpo on p.ACCOUNT equals ac.BANK_ACCOUNT
                       select new
                       {
                           ID=p.DYSEQ
                       };
            var Groups = from p in list
                         group p by p.ID into g
                         select new { DYSEQ=g.Key, count=g.Count() };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string AddPlatejnoyePorucheniye(tbpt_yearlyplanregion GP)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            if (GP != null)
            {
                using (KasabaKIATEntities k = new KasabaKIATEntities())
                {
                    tbpt_yearlyplanregion _GP = new tbpt_yearlyplanregion();
                    _GP.YEAR = DateTime.Now.Year.ToString();
                    _GP.KVARTAL = GP.KVARTAL;
                    _GP.SANATORIUM_ID = GP.SANATORIUM_ID;
                    _GP.CAMP_ID = GP.CAMP_ID;
                    _GP.TIP_MEST_ID = GP.TIP_MEST_ID;
                    _GP.PRICE = GP.PRICE;
                    _GP.QUANTITY = GP.QUANTITY;
                    _GP.STATUS = "1";
                    _GP.USER_ID = UserId;
                    _GP.REGION_ID = GP.REGION_ID;
                    _GP.MODIFY_DATE = DateTime.Now;
                    k.tbpt_yearlyplanregion.Add(_GP);
                    k.SaveChanges();
                    return "ok";
                }
            }
            else
            {
                return "error";
            }
        }
        [HttpPost]
        public string UpdatePlatejnoyePorucheniye(tbpt_yearlyplanregion GP)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            if (GP != null)
            {
                using (KasabaKIATEntities k = new KasabaKIATEntities())
                {
                    int id = Convert.ToInt32(GP.REC_ID);
                    tbpt_yearlyplanregion _GP = k.tbpt_yearlyplanregion.Where(i => i.REC_ID == id).FirstOrDefault();
                    _GP.YEAR = DateTime.Now.Year.ToString();
                    _GP.KVARTAL = GP.KVARTAL;
                    _GP.SANATORIUM_ID = GP.SANATORIUM_ID;
                    _GP.CAMP_ID = GP.CAMP_ID;
                    _GP.TIP_MEST_ID = GP.TIP_MEST_ID;
                    _GP.PRICE = GP.PRICE;
                    _GP.QUANTITY = GP.QUANTITY;
                    _GP.STATUS = "1";
                    _GP.USER_ID = UserId;
                    _GP.REGION_ID = GP.REGION_ID;
                    _GP.MODIFY_DATE = DateTime.Now;
                    k.SaveChanges();
                    return "ok";
                }
            }
            else
            {
                return "error";
            }
        }
        [HttpPost]
        public string DeletePlatejnoyePorucheniye(string id)
        {
            using (KasabaKIATEntities k = new KasabaKIATEntities())
            {
                int _id = Convert.ToInt32(id);
                tbpt_yearlyplanregion p1 = k.tbpt_yearlyplanregion.Where(i => i.REC_ID == _id).FirstOrDefault();
                p1.STATUS = "0";
                k.SaveChanges();
                return "ok";
            }
        }
    }
}