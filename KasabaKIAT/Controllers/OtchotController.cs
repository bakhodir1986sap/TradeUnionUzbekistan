using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KasabaKIAT.Models;

namespace KasabaKIAT.Controllers
{
    [Authorize]
    public class OtchotController : Controller
    {
        KasabaKIATEntities KasabaDB = new KasabaKIATEntities();
        // GET: Otchot
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SvodniyOtchot()
        {
            return View();
        }
        public ActionResult Talabnomlar()
        {
            return View();
        }
        public ActionResult Forma13()
        {
            return View();
        }
        public ActionResult Forma14()
        {
            return View();
        }
        public ActionResult TablitsaOtdixayushix()
        {
            return View();
        }
        public ActionResult JurnalniyOrder()
        {
            return View();
        }
        public ActionResult FinalniyOtchot()
        {
            return View();
        }
        public ActionResult GlavnayaKniga()
        {
            return View();
        }
        public JsonResult GetSvodniyOtchot(DateTime? Begindt,DateTime? Enddt,string Soato,string Branch)
        {
            var data = KasabaDB.sp_rt_SvodniyOtchet(Begindt, Enddt, Soato, Branch);
            var list = from d in data
                       select new
                       {
                           PN = d.ProfName,
                           FN = d.FullName,
                           PP=d.PlaceAndPos,
                           MSdt=d.MedSprevDate,
                           Zdt=d.ZayavkaDate,
                           SN=d.SanName,
                           ZB=d.zaezdBegin
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTalabnomlar_Korxona(int yil,string inn)
        {
            var data = KasabaDB.sp_rt_TalabnomaKorxona(yil,inn);
            var list = from d in data
                       select new
                       {
                           SN=d.sanName,
                           KV1=d.KV1,
                           KV2=d.KV2,
                           KV3=d.KV3,
                           KV4=d.KV4
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTalabnomlar_Tarmoq(int yil, string soato,string branch)
        {
            var data1 = KasabaDB.sp_rt_TalabnomaTarmoq(yil, soato, branch);
            var list = from d in data1
                       select new
                       {
                           SN = d.sanName,
                           KV1 = d.KV1,
                           KV2 = d.KV2,
                           KV3 = d.KV3,
                           KV4 = d.KV4
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTalabnomlar_Viloyat(int yil, string soato)
        {
            var data3 = KasabaDB.sp_rt_TalabnomaViloyat(yil, soato);
            var list = from d in data3
                       select new
                       {
                           SN = d.sanName,
                           KV1 = d.KV1,
                           KV2 = d.KV2,
                           KV3 = d.KV3,
                           KV4 = d.KV4
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOtrasl()
        {
            var O = from o in KasabaDB.sp_branch
                    select new
                    {
                        id = o.code,
                        value = o.name_uz
                    };
            return Json(O.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRegion()
        {
            var R = from r in KasabaDB.sp_soato
                    where r.parent_code == "17"
                    select new
                    {
                        id = r.code,
                        value = r.name_uz
                    };
            return Json(R, JsonRequestBehavior.AllowGet);
        }
    }
}