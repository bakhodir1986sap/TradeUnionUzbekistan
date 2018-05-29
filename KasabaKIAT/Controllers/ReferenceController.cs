using KasabaKiat.BusinessLogic.Utils.References;
using KasabaKiat.Helpers;
using KasabaKIAT.BusinessLogic.Utils.References;
using KasabaKIAT.Models;
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
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
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
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetKvartals()
        {
            var list = Sprav_Kv.Get(new Sprav_KvViewModel() { });

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
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
        /// <returns></returns>.
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
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
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetTipOplati()
        {
            var list = tbcs_dictionaryDataManager.Get("TipOplati");

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetVidOplati()
        {
            var list = tbcs_dictionaryDataManager.Get("VidOplati");

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
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
                Code = 1,
                Decode = "Мужчины"
            };

            list.Add(obj2);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetLoadSide()
        {
            var list = new List<SpravCodeDecode>();

            var obj1 = new SpravCodeDecode
            {
                Code = 0,
                Decode = "Санаторий"
            };

            list.Add(obj1);

            var obj2 = new SpravCodeDecode
            {
                Code = 1,
                Decode = "Профсоюзы"
            };

            list.Add(obj2);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Osnovaniye vidachi
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetOsnovaniyaVidachi()
        {
            var list = Sprav_Face.Get();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Tip Vidachi
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetTipVidachi()
        {
            var list = Sprav_Os.Get();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Procentnaya stavka
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetProcentZaPutyovki()
        {
            var list = Sprav_Pr.Get();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Rekomendovanniye sanatorii
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetRecommendedSanatory()
        {
            var list = Sprav_San.Get();

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// General sanatorii
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetSanatories()
        {
            var list = Sprav_San.Get();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetPravsoyuzList()
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var list = db.tbku_StrokaProfsoyza.Where(z=>z.MATCH_DEPARTMENTBASE != null).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetPravLevel()
        {
            var list = new List<SpravCodeDecode>();

            //var obj1 = new SpravCodeDecode
            //{
            //    Code = 0,
            //    Decode = "Федерация"
            //};

            //list.Add(obj1);

            //var obj2 = new SpravCodeDecode
            //{
            //    Code = 1,
            //    Decode = "Область"
            //};

            //list.Add(obj2);

            var obj3 = new SpravCodeDecode
            {
                Code = 2,
                Decode = "Район-Отрасль"
            };

            list.Add(obj3);

            var obj5 = new SpravCodeDecode
            {
                Code = 3,
                Decode = "ППО"
            };

            list.Add(obj5);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

    }
}