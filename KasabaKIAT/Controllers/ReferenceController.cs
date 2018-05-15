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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTipOplati()
        {
            var list = tbcs_dictionaryDataManager.Get("TipOplati");

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetVidOplati()
        {
            var list = tbcs_dictionaryDataManager.Get("VidOplati");

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPol()
        {
            var list = new List<SpravCodeDecode>();

            var obj1 = new SpravCodeDecode
            {
                Code = 0,
                Decode = "Женщины"
            };

            list.Add(obj1);

            var obj2 = new SpravCodeDecode
            {
                Code = 0,
                Decode = "Мужчины"
            };

            list.Add(obj2);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Osnovaniye vidachi
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOsnovaniyaVidachi()
        {
            var list = Sprav_Face.Get();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Tip Vidachi
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTipVidachi()
        {
            var list = Sprav_Os.Get();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Procentnaya stavka
        /// </summary>
        /// <returns></returns>
        public JsonResult GetProcentZaPutyovki()
        {
            var list = Sprav_Pr.Get();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Rekomendovanniye sanatorii
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRecommendedSanatory()
        {
            var list = Sprav_San.Get();

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// General sanatorii
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSanatories()
        {
            var list = Sprav_San.Get();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}