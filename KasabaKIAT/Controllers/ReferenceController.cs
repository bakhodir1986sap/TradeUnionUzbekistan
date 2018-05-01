using KasabaKiat.BusinessLogic.Utils.References;
using KasabaKiat.Helpers;
using KasabaKIAT.BusinessLogic.Utils.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace KasabaKIAT.Controllers
{
    public class ReferenceController : Controller
    {
        // GET: Reference
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 3000, Location = OutputCacheLocation.ServerAndClient)]
        public JsonResult GetYears()
        {
            var obj = new RefYars();

            var list = obj.GetYears();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 3000, Location = OutputCacheLocation.ServerAndClient)]
        public JsonResult GetKvartals()
        {
            var list = Sprav_Kv.Get(new Sprav_KvViewModel() { });

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOrganization()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = Sprav_Org.GetByDeraprtmentBase(ustinfo.DepartmentBase);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Inn"></param>
        /// <returns></returns>
        public JsonResult GetOrganization(string Name, string Inn)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = Sprav_Org.GetByDeraprtmentBase(ustinfo.DepartmentBase, Name, Inn, null);

            return Json(list, JsonRequestBehavior.AllowGet);
        }



    }
}