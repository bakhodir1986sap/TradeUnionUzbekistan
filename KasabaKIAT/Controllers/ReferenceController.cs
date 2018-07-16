using KasabaKiat.BusinessLogic.Utils.References;
using KasabaKiat.Helpers;
using KasabaKIAT.BusinessLogic.Utils;
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

        private class Organization
        {
            public string Inn { get; set; }

            public string Name_Full { get; set; }

            public string Okonx { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn", VaryByCustom = "userId")]
        public JsonResult GetOrgDataByInn(string inn)
        {
            if (string.IsNullOrEmpty(inn))
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var orgs = db.t_egrpo.Where(e => e.inn == inn);

                if (orgs.Any())
                {
                    var org = orgs.First();

                    var orgn = new Organization
                    {
                        Inn = inn,
                        Name_Full = org.name_full,
                        Okonx = org.okonx_id
                    };

                    return Json(orgn, JsonRequestBehavior.AllowGet);
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        private class BankData
        {
            public string BankCode { get; set; }

            public string BankName { get; set; }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mfo"></param>
        /// <returns></returns>
        public JsonResult GetBankNameByMFO(string mfo)
        {
            if (string.IsNullOrEmpty(mfo))
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var orgs = db.tblSprav_MFO.Where(e => e.Code_MFO == mfo);

                if (orgs.Any())
                {
                    var org = orgs.First();

                    var bankName = string.Empty;

                    if (!string.IsNullOrEmpty(org.Name_MFO))
                    {
                        if (org.Name_MFO.Length > 20)
                        {
                            bankName = org.Name_MFO.Substring(0, 20);
                        }
                        else
                        {
                            bankName = org.Name_MFO;
                        }
                    }

                    var bank = new BankData
                    {
                        BankCode = org.Code_MFO,
                        BankName = org.Name_MFO
                    };

                    return Json(bank, JsonRequestBehavior.AllowGet);
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        private class CurBusinessTime
        {
            public int CurYear { get; set; }
            public int CurKvartal { get; set; }
        }

        public JsonResult GetBusinessTime()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);
            var obj = new CurBusinessTime();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var curBusiness = db.tblCloseKvartals.Where(z => z.DEPARTMENT_CODE == ustinfo.DepartmentBase);

                if (curBusiness.Any())
                {

                    var cuuuu = curBusiness.First();
                    obj.CurYear = cuuuu.CURKV_YEAR.Value;
                    obj.CurKvartal = cuuuu.CURKV.Value;
                }
                else
                {
                    obj.CurYear = DateTime.Now.Year;
                    obj.CurKvartal = DateOperations.GetQuarter(DateTime.Now);
                }
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

    }
}