using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KasabaKIAT.Controllers
{
    //[Authorize(Roles = "ADMIN")]
    [Authorize]
    public class BuxgalterController : Controller
    {
        // GET: Buxgalter
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BU_RuchnayaPravodka()
        {
            return View();
        }
        public ActionResult BU_PrasmotrDannix()
        {
            return View();
        }
        public ActionResult VvodDannix()
        {
            return View();
        }
        public ActionResult OsushestvleniyeProvodki()
        {
            return View();
        }
        public ActionResult Nakladniye()
        {
            return View();
        }
        public ActionResult RaspredleniyeByudjeta()
        {
            return View();
        }
        public ActionResult PlanByudjetaOblastey()
        {
            return View();
        }
        public ActionResult PlanByudjetaOblasteyNoviy()
        {
            return View();
        }
        public ActionResult PlanByudjetaOblasteyIzmenit()
        {
            return View();
        }
    }
}