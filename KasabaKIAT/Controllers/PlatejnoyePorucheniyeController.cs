using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Buxgalteriya;
using KasabaKiat.Helpers;
using KasabaKIAT.BusinessLogic.Buxgalteriya.Platejki;

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
            var list = from pp in KasabaDB.tbbu_platejka
                       select new
                       {
                           d0=pp.REC_ID,
                           d1=pp.PLAT_DATE,
                           d2=pp.PLAT_NUM,
                           d3=pp.PLATELNAME,
                           d4=pp.PLATELINN,
                           d5=pp.PLATELRASCHET,
                           d6=pp.PLATELBANKNAME,
                           d7=pp.PLATELBANKMFO,
                           d8=pp.POLUCHNAME,
                           d9=pp.POLUCHINN,
                           d10=pp.POLUCHRASCHET,
                           d11=pp.POLUCHBANKNAME,
                           d12=pp.POLUCHBANKMFO,
                           d13=pp.SUMMA,
                           d14=pp.COMMENT,
                           d15=pp.RUKOVODITEL,
                           d16=pp.GLAVBUX
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public string EnterPlatejnoyePorucheniye(PlatejkiViewModel d)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var uu = UserInfo.GetUserInfo(UserId);
            Platejki pl = new Platejki();
            d.department_code = uu.DepartmentBase;
            d.userid = UserId;

            pl.Enter(d);
            return "yes";
        }
    }
}