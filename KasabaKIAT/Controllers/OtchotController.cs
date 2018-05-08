using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KasabaKIAT.Models;
using KurortUrvServiceTest.ServiceInterAction.ActionLogic;
using log4net;
using KasabaKiat.Helpers;
using System.Web.UI;

namespace KasabaKIAT.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class OtchotController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        ServiceActionController service = new ServiceActionController();

        /// <summary>
        /// 
        /// </summary>
        ILog log = log4net.LogManager.GetLogger(typeof(AdministirirovaniyeController));

        /// <summary>
        /// 
        /// </summary>
        KasabaKIATEntities KasabaDB = new KasabaKIATEntities();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 30000, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public ActionResult SvodniyOtchot()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public ActionResult Talabnomlar()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public ActionResult Forma13()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yil"></param>
        /// <param name="inn"></param>
        /// <returns></returns>
        public JsonResult GetForma13(int yil, string inn)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);

            var userInfo = UserInfo.GetUserInfo(UserId);

            log.Info("GetRaspredeleniyeZaezdov1");
            log.Info("yil = " + yil);
            log.Info("inn = " + inn);
            try
            {
                var data = KasabaDB.sp_rt_FormaN13(yil, inn, userInfo.DepartmentBase);
                var O = from o in data
                        select new
                        {
                            d1 = o.Data_Postupleniya,
                            d2 = o.Nomer_SchetFacturi,
                            d3 = o.NameimenovaniyeOrg,
                            d4 = o.NomerPutyovki,
                            d5 = o.DataZaezda,
                            d6 = o.SenaPutyovki,
                            d7 = o.DataPokupkiPutyovki,
                            d8 = o.NomerResheniyaOVidache,
                            d9 = o.FIO,
                            d10 = o.MestoRaboti,
                            d11 = o.DataPlateja,
                            d12 = o.SymmaPlateja,
                            d13 = o.NomerPlatejnogoPorucheniya,
                            d14 = o.NomerTalona,
                            d15 = o.DataTalona
                        };

                log.Info("Count = " + O.ToList().Count);

                return Json(O.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }
            return null;
        }

        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public ActionResult Forma14()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public ActionResult TablitsaOtdixayushix()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public ActionResult JurnalniyOrder()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public ActionResult FinalniyOtchot()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public ActionResult GlavnayaKniga()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Begindt"></param>
        /// <param name="Enddt"></param>
        /// <param name="Soato"></param>
        /// <param name="Branch"></param>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "Begindt;Enddt;Soato;Branch", VaryByCustom = "userId")]
        public JsonResult GetSvodniyOtchot(DateTime? Begindt,DateTime? Enddt,string Soato,string Branch)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);

            var userInfo = UserInfo.GetUserInfo(UserId);

            var data = KasabaDB.sp_rt_SvodniyOtchet(Begindt, Enddt, Soato, Branch, userInfo.DepartmentBase);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yil"></param>
        /// <param name="inn"></param>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "yil;inn", VaryByCustom = "userId")]
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yil"></param>
        /// <param name="soato"></param>
        /// <param name="branch"></param>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "yil;soato;branch", VaryByCustom = "userId")]
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yil"></param>
        /// <param name="soato"></param>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "yil;soato", VaryByCustom = "userId")]
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public ActionResult Postupleniya()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "inn", VaryByCustom = "userId")]
        public JsonResult GetPostupleniya(string inn)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);

            var userInfo = UserInfo.GetUserInfo(UserId);

            log.Info("GetPostupleniya");
            log.Info("inn = " + inn);
            try
            {
                var data = KasabaDB.sp_rt_Postupleniya(inn, userInfo.DepartmentBase);
                var O = from o in data
                        select new
                        {
                            d1 = o.K_TYPE,
                            d2 = o.OPERDAY,
                            d3 = o.ID,
                            d4 = o.BANKID,
                            d5 = o.KLSDOX,
                            d6 = o.DOCNUMB,
                            d7 = o.DOCDATE,
                            d8 = o.DOCTYPE,
                            d9 = o.SUMPAY,
                            d10 =o.CLMFO,
                            d11 =o.CLACC,
                            d12 =o.CLINN,
                            d13 =o.CLNAME,
                            d14 =o.COMFO,
                            d15 =o.COACC,
                            d16=o.COINN,
                            d17=o.CONAME,
                            d18=o.PURPOSE,
                            d19=o.CHAPTER,
                            d20=o.OKONH,
                            d21=o.FILE_ID,//
                            d22=o.OTCHISLEN,//
                            d23=o.FileName,//
                            d24=o.CreateDate// 
                        };

                return Json(O.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public ActionResult RazbivkaPostupleniya()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="b"></param>
        /// <param name="bg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "s;b;bg;end", VaryByCustom = "userId")]
        public JsonResult GetRazbivkaPostupleniya(string s,string b,DateTime? bg,DateTime? end)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);

            var userInfo = UserInfo.GetUserInfo(UserId);

            log.Info("GetPostupleniya");
            log.Info("SOATO = " + s);
            log.Info("BRANCH = " + b);
            log.Info("BEGIN_DATE = " + bg);
            log.Info("END_DATE = " + end);
            try
            {
                var data = KasabaDB.sp_rt_Razbivka_Postup(s, b, bg.Value.Date, end.Value.Date, userInfo.DepartmentBase);
                var O = from o in data
                        select new
                        {
                            d1 =  o.K_TYPE           ,
                            d2 =  o.OPERDAY          ,
                            d3 =  o.ID               ,
                            d4 =  o.BANKID           ,
                            d5 =  o.KLSDOX           ,
                            d6 =  o.DOCNUMB          ,
                            d7 =  o.DOCDATE          ,
                            d8 =  o.DOCTYPE          ,
                            d9 =  o.SUMPAY           ,
                            d10 = o.CLMFO           ,
                            d11 = o.CLACC           ,
                            d12 = o.CLINN           ,
                            d13 = o.CLNAME          ,
                            d14 = o.COMFO           ,
                            d15 = o.COACC           ,
                            d16 = o.COINN           ,
                            d17 = o.CONAME          ,
                            d18 = o.PURPOSE         ,
                            d19 = o.CHAPTER         ,
                            d20 = o.OKONH           ,
                            d21 = o.FILE_ID         ,//
                            d22 = o.OTCHISLEN       ,//
                            d23 = o.SUMMATOTAL      ,//
                            d24 = o.FEDERATIONSUM   ,// 
                            d25 = o.OBLASTSUM       ,
                            d26 = o.OblastName      ,
                            d27 = o.OkonxName       ,
                            d28 = o.otraslName
                        };

                var resultList = O.ToList();

                return Json(resultList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public ActionResult PostupleniyaOblastyam()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "bg;end", VaryByCustom = "userId")]
        public JsonResult GetPostupleniyaOblastyam(DateTime? bg, DateTime? end)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);

            var userInfo = UserInfo.GetUserInfo(UserId);

            log.Info("GetPostupleniyaOblastyam");
            log.Info("BEGIN_DATE = " + bg);
            log.Info("END_DATE = " + end);
            try
            {
                var data = KasabaDB.sp_rt_Razbivka_Postup_OblastGroup(bg.Value.Date, end.Value.Date, userInfo.DepartmentBase);
                var O = from o in data
                        select new
                        {
                            d1 = o.OblastName,
                            d2 = o.Kolichestvo,
                            d3 = o.TOTALSUM,
                            d4 = o.OBLASTSUM,
                        };

                var resultList = O.ToList();

                return Json(resultList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public ActionResult PostupleniyaTuman()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "bg;end", VaryByCustom = "userId")]
        public JsonResult GetPostupleniyaTuman(DateTime? bg, DateTime? end)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);

            var userInfo = UserInfo.GetUserInfo(UserId);

            log.Info("GetPostupleniyaTuman");
            log.Info("BEGIN_DATE = " + bg);
            log.Info("END_DATE = " + end);
            try
            {
                var data = KasabaDB.sp_rt_Razbivka_Postup_OblastTumanGroup(bg.Value.Date, end.Value.Date, userInfo.DepartmentBase);
                var O = from o in data
                        select new
                        {
                            d1 = o.OBLAST_NAME,
                            d2 = o.TUMAN_NAME,
                            d3 = o.KOLICHESTVO,
                            d4 = o.summa,
                        };

                var resultList = O.ToList();

                return Json(resultList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public ActionResult PostupleniyaOblastTumanOtraslGroup()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="bg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "s;bg;end", VaryByCustom = "userId")]
        public JsonResult GetPostupleniyaOblastTumanOtraslGroup(string s, DateTime? bg, DateTime? end)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);

            var userInfo = UserInfo.GetUserInfo(UserId);

            log.Info("GetPostupleniyaOblastTumanOtraslGroup");
            log.Info("SOATO = " + s);
            log.Info("BEGIN_DATE = " + bg);
            log.Info("END_DATE = " + end);
            try
            {
                var data = KasabaDB.sp_rt_Razbivka_Postup_OblastTumanOtraslGroup(bg.Value.Date, end.Value.Date, s, userInfo.DepartmentBase);
                var O = from o in data
                        select new
                        {
                            d1 = o.OBLAST_NAME,
                            d2 = o.TUMAN_NAME,
                            d3 = o.otraslName,
                            d4 = o.KOLICHESTVO,
                            d5 = o.summa,
                        };

                var resultList = O.ToList();

                return Json(resultList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public ActionResult PostupleniyaOtrasli()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="bg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "s;bg;end", VaryByCustom = "userId")]
        public JsonResult GetPostupleniyaOtrasli(string s, DateTime? bg, DateTime? end)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);

            var userInfo = UserInfo.GetUserInfo(UserId);

            log.Info("GetPostupleniyaOtrasli");
            log.Info("SOATO = " + s);
            log.Info("BEGIN_DATE = " + bg);
            log.Info("END_DATE = " + end);
            try
            {
                var data = KasabaDB.sp_rt_Razbivka_Postup_OtraslGroup(s, bg.Value.Date, end.Value.Date, userInfo.DepartmentBase);
                var O = from o in data
                        select new
                        {
                            d1 = o.OblastName,
                            d2 = o.otraslName,
                            d3 = o.Kolichestvo,
                            d4 = o.TOTALSUM,
                            d5 = o.OBLASTSUM,
                        };

                var resultList = O.ToList();

                return Json(resultList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public ActionResult PostupleniyaOkonxu()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="bg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "s;bg;end", VaryByCustom = "userId")]
        public JsonResult GetPostupleniyaOkonxu(string s, DateTime? bg, DateTime? end)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);

            var userInfo = UserInfo.GetUserInfo(UserId);

            log.Info("GetPostupleniyaOkonxu");
            log.Info("SOATO = " + s);
            log.Info("BEGIN_DATE = " + bg);
            log.Info("END_DATE = " + end);
            try
            {
                var data = KasabaDB.sp_rt_Razbivka_Postup_OkonxGroup(s, bg.Value.Date, end.Value.Date, userInfo.DepartmentBase);
                var O = from o in data
                        select new
                        {
                            d1=o.OblastName,
                            d2 = o.OkonxName,
                            d3 = o.Kolichestvo,
                            d4 = o.TOTALSUM,
                            d5 = o.OBLASTSUM,
                        };

                var resultList = O.ToList();

                return Json(resultList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 3000, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public JsonResult GetOtrasl()
        {
            var O = from o in KasabaDB.sp_branch
                    select new
                    {
                        id = o.code,
                        value = o.name_uz
                    };
            var resList = O.ToList();

            resList.Insert(0, new { id = "0", value = "Все" });
            return Json(resList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 3000, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public JsonResult GetRegion()
        {
            var UserId = Convert.ToInt32(User.Identity.Name);

            var userInfo = UserInfo.GetUserInfo(UserId);


            if (userInfo.DepartmentBase == "17")
            {
                var R = (from r in KasabaDB.sp_soato
                         where r.parent_code == "17" && r.code.StartsWith(userInfo.DepartmentBase)
                         select new
                         {
                             id = r.code,
                             value = r.name_uz
                         });
                var resList = R.ToList();

                resList.Insert(0, new { id = "0", value = "Все" });

                return Json(resList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var soatoCode = userInfo.DepartmentBase.Length == 5 ?
                    userInfo.DepartmentBase.Substring(0, 2) + userInfo.DepartmentBase.Substring(3, 2) : userInfo.DepartmentBase;

                var R = (from r in KasabaDB.sp_soato
                         where r.parent_code == "17" && r.code.StartsWith(soatoCode)
                         select new
                         {
                             id = r.code,
                             value = r.name_uz
                         });
                var resList = R.ToList();

                //resList.Insert(0, new { id = "0", value = "Все" });

                return Json(resList, JsonRequestBehavior.AllowGet);
            }

        }


        #region Ilxom Otchots

        public ActionResult ZaprosPoPutyovkam() { return View(); }
        public ActionResult OperativkaPoPutyovkam() { return View(); }
        public ActionResult SvodPoPutyovkam() { return View(); }
        public ActionResult Operativnayakartochka() { return View(); }
        public ActionResult BalancePredpriyatiy() { return View(); }
        public ActionResult UchetMedicinskoyReg() { return View(); }
        public ActionResult UchetPostupleniya() { return View(); }
        public ActionResult OtchetN13() { return View(); }
        public ActionResult OtchetN14() { return View(); }
        public ActionResult KnigaUchetaVladel() { return View(); }

        public ActionResult JurnalniyOrder4() { return View(); }
        public ActionResult JurnalniyOrder5() { return View(); }
        public ActionResult JurnalniyOrder6() { return View(); }


        #endregion
    }
}