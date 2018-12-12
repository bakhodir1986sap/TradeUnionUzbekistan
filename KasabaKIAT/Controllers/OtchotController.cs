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
using KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki;
using KasabaKIAT.BusinessLogic.Utils;
using KasabaKIAT.BusinessLogic.Buxgalteriya.GlavnayaKniga;
using KasabaKIAT.BusinessLogic.Utils.References;

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
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OtchetTitul() { return View(); }
        public ActionResult OtchetZayavki           () {return View();}       
        public ActionResult OtchetOrganizasii       () {return View();}
        public ActionResult OtchetPostupleniya      () {return View();}
        public ActionResult OtchetPutevki           () {return View();}
        public ActionResult OtchetBuxgalteriya      () {return View();}
        public ActionResult OtchetJurnalniyeOrdera()   { return View(); }
        public ActionResult OtchetMonitoring() { return View(); }
        public ActionResult PutyovkaHistory() { return View(); }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult SvodniyOtchot()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult SvodniyOtchotV2()
        {
            var resQuery = KasabaDB.sp_rt_SvodniyOtchetV2().ToList();

            ViewBag.DateSource = resQuery;

            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult GlavnayaKnigaN()
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult GlavnayaKnigaVRazreze()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult GlavnayaKnigaSvodniy()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult GlavnayaKnigaNV2()
        {
            var obj = new RefYars();

            ViewBag.YearsList = obj.GetYears();

            ViewBag.KvartalsList = obj.GetKvartals();

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public ActionResult GlavnayaKnigaV2(string year, string kvartal)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var gvk = new GK_Structure(ustinfo.DepartmentBase, int.Parse(year), int.Parse(kvartal));
            const bool isSvodniyOtchet = false;
            var OutList = gvk.MakeGlavKniga(isSvodniyOtchet);

            var obj = new RefYars();

            ViewBag.YearsList = obj.GetYears();

            ViewBag.KvartalsList = obj.GetKvartals();

            ViewBag.Datasource = OutList;

            return View("GlavnayaKnigaNV2");
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Models.ViewModel> SpisokPravsoyuzovOrg()
        {
            log.Info("SpisokPravsoyuzov");
            IList<Models.ViewModel> O = null;

            var UserId = Convert.ToInt32(User.Identity.Name);

            var userInfo = UserInfo.GetUserInfo(UserId);

            try
            {
                O = (from o in KasabaDB.tbku_StrokaProfsoyza
                     where o.MATCH_DEPARTMENTBASE != null && o.MATCH_DEPARTMENTBASE != userInfo.DepartmentBase
                     select new Models.ViewModel
                     {
                         Kod = o.MATCH_DEPARTMENTBASE,
                         Name = o.ProfsoyzNaimenovanie
                     }).ToList();

                log.Info("Count = " + O.ToList().Count);
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }

            return O.ToList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult GlavnayaKnigaVRazrezeV2()
        {
            var obj = new RefYars();

            ViewBag.YearsList = obj.GetYears();

            ViewBag.KvartalsList = obj.GetKvartals();

            ViewBag.SpisokPravsoyuzovOrg = SpisokPravsoyuzovOrg();

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public ActionResult GlavnayaKnigaVRazrezeNV2(string year, string kvartal, string departmentBase)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var obj = new RefYars();

            ViewBag.YearsList = obj.GetYears();

            ViewBag.KvartalsList = obj.GetKvartals();

            ViewBag.SpisokPravsoyuzovOrg = SpisokPravsoyuzovOrg();

            var gvk = new GK_Structure(departmentBase, int.Parse(year), int.Parse(kvartal));
            const bool isSvodniyOtchet = false;
            var OutList = gvk.MakeGlavKniga(isSvodniyOtchet);

            ViewBag.Datasource = OutList;

            return View("GlavnayaKnigaVRazrezeV2");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult GlavnayaKnigaSvodniyV2()
        {
            var obj = new RefYars();

            ViewBag.YearsList = obj.GetYears();

            ViewBag.KvartalsList = obj.GetKvartals();

            return View();
        }
                              
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public ActionResult GlavnayaKnigaSvodniyNV2(string year, string kvartal)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var gvk = new GK_Structure("17", int.Parse(year), int.Parse(kvartal));
            const bool isSvodniyOtchet = true;
            var OutList = gvk.MakeGlavKniga(isSvodniyOtchet);

            var obj = new RefYars();

            ViewBag.YearsList = obj.GetYears();

            ViewBag.KvartalsList = obj.GetKvartals();

            ViewBag.Datasource = OutList;

            return View("GlavnayaKnigaSvodniyV2");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult Talabnomlar()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
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





        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult Forma14()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult TablitsaOtdixayushix()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult JurnalniyOrder()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult FinalniyOtchot()
        {
            return View();
        }

        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult FinansoviyOtchotKvartalniyN()
        {
            return View();
        }

        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult FinansoviyOtchotKvartalniyVRazreze()
        {
            return View();
        }

        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult FinansoviyOtchotKvartalniySvodniy()
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
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
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "Begindt;Enddt;Soato;Branch", VaryByCustom = "userId")]
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

        public ActionResult GetSvodniyOtchotV2()
        {
            var resQuery = KasabaDB.sp_rt_SvodniyOtchetV2().ToList();

            ViewBag.DateSource = resQuery;

            return View("SvodniyOtchotV2");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yil"></param>
        /// <param name="inn"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "yil;inn", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "yil;soato;branch", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "yil;soato", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult Postupleniya()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult PostupleniyaN()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult PostupleniyaNV2()
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn", VaryByCustom = "userId")]
        public JsonResult GetPostupleniyaN(string inn)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);
            var userInfo = UserInfo.GetUserInfo(UserId);

            log.Info("GetPostupleniyaN");
            log.Info("inn = " + inn);

            var data = KasabaDB.sp_rt_PostupleniyaN(inn, userInfo.DepartmentBase);

            var OutData = from outData in data
                    select new
                    {
                        operday = outData.OPERDAY,
                        inn = outData.INN,
                        clname = outData.CLNAME,
                        postupleniya = outData.POSTUPLENIYA,
                        obshiy01 = outData.Obshiy01,
                        federation_amt = outData.FEDERATION_AMT,
                        oblsovprof_amt = outData.OBLSOVPROF_AMT,
                        purpose = outData.PURPOSE
                    };

            return Json(OutData.ToList(), JsonRequestBehavior.AllowGet);
        }

        public class PostupleniyaNCL
        {
            public string operday           {get; set;}
            public string inn               {get; set;}
            public string clname            {get; set;}
            public decimal? postupleniya      {get; set;}
            public decimal? obshiy01          {get; set;}
            public decimal? federation_amt    {get; set;}
            public decimal? oblsovprof_amt    {get; set;}
            public string purpose { get; set; }
        }

        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn", VaryByCustom = "userId")]
        public ActionResult GetPostupleniyaNV2(string inn)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);
            var userInfo = UserInfo.GetUserInfo(UserId);

            log.Info("GetPostupleniyaN");
            log.Info("inn = " + inn);

            var data = KasabaDB.sp_rt_PostupleniyaN(inn, userInfo.DepartmentBase);

            var OutData = from outData in data
                          select new PostupleniyaNCL
                          {
                              operday           = outData.OPERDAY,
                              inn               = outData.INN,
                              clname            = outData.CLNAME,
                              postupleniya      = outData.POSTUPLENIYA,
                              obshiy01          = outData.Obshiy01,
                              federation_amt    = outData.FEDERATION_AMT,
                              oblsovprof_amt    = outData.OBLSOVPROF_AMT,
                              purpose           = outData.PURPOSE
                          };

            ViewBag.dataSource = OutData.ToList();

            return View("PostupleniyaNV2");
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult OrganizationSummaryV2()
        {
            var resQuery = KasabaDB.sp_rt_SvodniyOtchetOrganizationV2().ToList();

            ViewBag.DateSource = resQuery;

            return View();
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn", VaryByCustom = "userId")]
        public JsonResult GetOrganizationSummary()
        {
            var UserId = Convert.ToInt32(User.Identity.Name);
            var userInfo = UserInfo.GetUserInfo(UserId);

            log.Info("GetOrganizationSummary");

            var data = KasabaDB.sp_rt_Organization_Summary(userInfo.DepartmentBase);

            var OutData = from outData in data
                          select new
                          {
                              branch = outData.Branch,
                              count = outData.colVo
                          };

            return Json(OutData.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult ProvodkiDetail()
        {
            return View();
        }

        public JsonResult GetProvodkiDetail(DateTime? bg, DateTime? end)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);
            var userInfo = UserInfo.GetUserInfo(UserId);

            var list = KasabaDB.sp_rt_Provodka_Details(userInfo.DepartmentBase, bg, end);

            var OutData = from outData in list
                          select new
                          {
                              REC_ID = outData.REC_ID,
                              TRDT = outData.TRDT,
                              DEBIT_ACCOUNT = outData.DEBIT_ACCOUNT,
                              CREDIT_ACCOUNT = outData.CREDIT_ACCOUNT,
                              AMOUNT = outData.AMOUNT,
                              DETALI = outData.DETALI,
                              ADDITIONAL = outData.ADDITIONAL,
                              OPISANIYE = outData.OPISANIYE,
                              NAME_RU = outData.NAME_RU
                          };

            return Json(OutData.ToList(), JsonRequestBehavior.AllowGet);
        }


        public JsonResult CancelProvodki(long rec_id)
        {
            try
            {
                ProvodkiDataManager.RemoveTransaction(rec_id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult ProvodkiSummary()
        {
            return View();
        }

        public JsonResult GetProvodkiSummary(DateTime? bg, DateTime? end)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);
            var userInfo = UserInfo.GetUserInfo(UserId);

            var list = KasabaDB.sp_rt_Provodka_Summary(userInfo.DepartmentBase, bg, end);

            var OutData = from outData in list
                          select new
                          {
                              DEBIT_ACCOUNT = outData.DEBIT_ACCOUNT,
                              CREDIT_ACCOUNT = outData.CREDIT_ACCOUNT,
                              AMOUNT = outData.AMOUNT
                          };

            return Json(OutData.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult RazbivkaPostupleniya()
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult RazbivkaPostupleniyaNV2()
        {
            List<CodeDecode> typeList = new List<CodeDecode>();
            var code1 = new CodeDecode();
            code1.text = "15 %";
            code1.value = "700";
            typeList.Add(code1);

            var code2 = new CodeDecode();
            code2.text = "25 %";
            code2.value = "777";
            typeList.Add(code2);

            var code3 = new CodeDecode();
            code3.text = "15 % Фермер";
            code3.value = "600";
            typeList.Add(code3);

            ViewBag.TypeList = typeList;
            //{ id: '700', value: '15 %' },
            //{ id: '777', value: '25 %' },
            //{ id: '600', value: '15 % Фермер' }

            return View();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult RazbivkaPostupleniyaN()
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="payType"></param>
        /// <param name="bg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "payType;bg", VaryByCustom = "userId")]
        public JsonResult GetRazbivkaPostupleniyaN(string payType, DateTime? bg)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);
            var userInfo = UserInfo.GetUserInfo(UserId);

            var data = KasabaDB.sp_rt_Razbivka_PostupN(payType, userInfo.DepartmentBase, bg);

            var OutData = from outData in data
                          select new
                          {
                              tuman_shaxar      = outData.TUMAN_SHAXAR      ,
                              operday           = outData.OPERDAY           ,
                              clacc             = outData.CLACC             ,
                              inn               = outData.INN               ,
                              clmfo             = outData.CLMFO             ,
                              clname            = outData.CLNAME            ,
                              postupleniya      = outData.POSTUPLENIYA      ,
                              obshiy01          = outData.Obshiy01          ,
                              federation_amt    = outData.FEDERATION_AMT    ,
                              oblsovprof_amt    = outData.OBLSOVPROF_AMT    ,
                              purpose           = outData.PURPOSE           ,
                              okonx_name        = outData.OKONX_NAME        ,
                              branch_name       = outData.BRANCH_NAME   
                          };

            return Json(OutData.ToList(), JsonRequestBehavior.AllowGet);
        }

        public class GetRazbivkaPostupleniyaNV2CL
        {
            public string tuman_shaxar      {get; set;}
            public string operday           {get; set;}
            public string clacc             {get; set;}
            public string inn               {get; set;}
            public string clmfo             {get; set;}
            public string clname            {get; set;}
            public decimal? postupleniya      {get; set;}
            public decimal? obshiy01          {get; set;}
            public decimal? federation_amt    {get; set;}
            public decimal? oblsovprof_amt    {get; set;}
            public string purpose           {get; set;}
            public string okonx_name        {get; set;}
            public string branch_name       { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payType"></param>
        /// <param name="bg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "payType;bg", VaryByCustom = "userId")]
        public ActionResult GetRazbivkaPostupleniyaNV2(string payType, DateTime? bg)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);
            var userInfo = UserInfo.GetUserInfo(UserId);

            var data = KasabaDB.sp_rt_Razbivka_PostupN(payType, userInfo.DepartmentBase, bg);

            var OutData = from outData in data
                          select new GetRazbivkaPostupleniyaNV2CL
                          {
                              tuman_shaxar          = outData.TUMAN_SHAXAR,
                              operday               = outData.OPERDAY,
                              clacc                 = outData.CLACC,
                              inn                   = outData.INN,
                              clmfo                 = outData.CLMFO,
                              clname                = outData.CLNAME,
                              postupleniya          = outData.POSTUPLENIYA,
                              obshiy01              = outData.Obshiy01,
                              federation_amt        = outData.FEDERATION_AMT,
                              oblsovprof_amt        = outData.OBLSOVPROF_AMT,
                              purpose               = outData.PURPOSE,
                              okonx_name            = outData.OKONX_NAME,
                              branch_name           = outData.BRANCH_NAME
                          };

            List<CodeDecode> typeList = new List<CodeDecode>();
            var code1 = new CodeDecode();
            code1.text = "15 %";
            code1.value = "700";
            typeList.Add(code1);

            var code2 = new CodeDecode();
            code2.text = "25 %";
            code2.value = "777";
            typeList.Add(code2);

            var code3 = new CodeDecode();
            code3.text = "15 % Фермер";
            code3.value = "600";
            typeList.Add(code3);

            ViewBag.TypeList = typeList;

            ViewBag.dataSource = OutData.ToList();

            return View("RazbivkaPostupleniyaNV2");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="b"></param>
        /// <param name="bg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "s;b;bg;end", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult PostupleniyaOblastyam()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult PostupleniyaOblastyamV2()
        {
            return View();
        }

        class OblPostupleniya
        {
            public string d1  {get; set;}
            public int? d2  {get; set;}
            public decimal? d3  {get; set;}
            public decimal? d4 { get; set;}

            public string   oblastName        {get; set;}
            public string   otraslName        {get; set;}
            public int?     kolvoStrok        {get; set;}
            public decimal? obshiy01          {get; set;}
            public decimal? federation_amount {get; set;}
            public decimal? oblast_amount     {get; set;}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "bg;end", VaryByCustom = "userId")]
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
                        select new OblPostupleniya
                        {
                            d1 = o.OblastName   ,
                            d2 = o.Kolichestvo  ,
                            d3 = o.TOTALSUM     ,
                            d4 = o.OBLASTSUM    ,
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
        /// <param name="bg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "bg;end", VaryByCustom = "userId")]
        public ActionResult GetPostupleniyaOblastyamV2(DateTime? bg, DateTime? end)
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
                        select new OblPostupleniya
                        {
                            oblastName = o.OblastName,
                            kolvoStrok = o.Kolichestvo,
                            obshiy01 = o.TOTALSUM + o.OBLASTSUM,
                            federation_amount = o.TOTALSUM,
                            oblast_amount = o.OBLASTSUM,
                        };

                var resultList = O.ToList();

                ViewBag.dataSource = resultList;

                return View("PostupleniyaOblastyamV2");
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }

            return View("PostupleniyaOblastyamV2");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
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
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "bg;end", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
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
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "s;bg;end", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult PostupleniyaOtrasli()
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult PostupleniyaOtrasliV2()
        {
            var UserId = Convert.ToInt32(User.Identity.Name);

            var userInfo = UserInfo.GetUserInfo(UserId);

            ViewBag.TypeList = GetOtchetRegionList(userInfo.DepartmentBase);

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="bg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "s;bg;end", VaryByCustom = "userId")]
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
        /// <param name="s"></param>
        /// <param name="bg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "s;bg;end", VaryByCustom = "userId")]
        public ActionResult GetPostupleniyaOtrasliV2(string region, DateTime? bg, DateTime? end)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);

            var userInfo = UserInfo.GetUserInfo(UserId);

            ViewBag.TypeList = GetOtchetRegionList(userInfo.DepartmentBase);

            log.Info("GetPostupleniyaOtrasli");
            log.Info("SOATO = " + region);
            log.Info("BEGIN_DATE = " + bg);
            log.Info("END_DATE = " + end);
            try
            {
                var data = KasabaDB.sp_rt_Razbivka_Postup_OtraslGroup(region, bg.Value.Date, end.Value.Date, userInfo.DepartmentBase);
                var O = from o in data
                        select new OblPostupleniya
                        {
                            oblastName = o.OblastName,
                            otraslName = o.otraslName,
                            kolvoStrok = o.Kolichestvo,
                            obshiy01 = o.TOTALSUM + o.OBLASTSUM,
                            federation_amount = o.TOTALSUM,
                            oblast_amount = o.OBLASTSUM,
                        };

                var resultList = O.ToList();

                ViewBag.dataSource = resultList;

                return View("PostupleniyaOtrasliV2");
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }

            return View("PostupleniyaOtrasliV2");
        }

        #region OtraslPivod

        class OtraslPivod
        {
           public string  TumanName      { get; set; }
           public decimal TumanAmount   { get; set; }
           public decimal BranchCode1   {get; set;}   //А1
           public decimal BranchCode2   {get; set;}   //А2
           public decimal BranchCode3   {get; set;}   //А3
           public decimal BranchCode4   {get; set;}   //Д1
           public decimal BranchCode5   {get; set;} //И1
           public decimal BranchCode6   {get; set;} //К1
           public decimal BranchCode7   {get; set;} //М1
           public decimal BranchCode8   {get; set;} //Н1
           public decimal BranchCode9   {get; set;} //С1
           public decimal BranchCode10  {get; set;} //Т1
           public decimal BranchCode11  {get; set;} //Т2
           public decimal BranchCode12  {get; set;} //Т3
           public decimal BranchCode13  { get; set; } //Э1
           public decimal BranchCode14  { get; set; } //Неопределен
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult PostupleniyaOtrasliPivod()
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
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "s;bg;end", VaryByCustom = "userId")]
        public JsonResult GetPostupleniyaOtrasliPivod(string s, DateTime? bg, DateTime? end)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);

            var userInfo = UserInfo.GetUserInfo(UserId);

            log.Info("GetPostupleniyaOtrasliPivod");
            log.Info("SOATO = " + s);
            log.Info("BEGIN_DATE = " + bg);
            log.Info("END_DATE = " + end);
            try
            {
                var data = KasabaDB.sp_rt_Razbivka_Postup_OtraslGroup(s, bg.Value.Date, end.Value.Date, userInfo.DepartmentBase);

                var RetList = data.ToList();

                var TumanList = RetList.Where(z => z.otraslName.Contains("Итого") && !z.OblastName.Contains("Итого"));

                List<OtraslPivod> pivodList = new List<OtraslPivod>();

                if (TumanList.Any())
                {
                    var branches = KasabaDB.sp_branch.ToList();

                    

                    foreach(var tuman in TumanList)
                    {
                        var obj = new OtraslPivod();

                        obj.TumanName = tuman.OblastName;
                        obj.TumanAmount = tuman.OBLASTSUM.Value;

                        var tumanInternals = RetList.Where(z => z.OblastName.Equals(tuman.OblastName) && !z.otraslName.Contains("Итого"));

                        {
                            var bBranchCode1s = branches.Where(z => z.code.Equals("А1")).FirstOrDefault();

                            if (bBranchCode1s != null)
                            {
                                var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                                if (tumalInts.Any())
                                {
                                    obj.BranchCode1 = tumalInts.First().OBLASTSUM.Value;
                                }
                                else
                                {
                                    obj.BranchCode1 = 0;
                                }
                            }
                        }


                        {
                            var bBranchCode1s = branches.Where(z => z.code.Equals("А2")).FirstOrDefault();

                            if (bBranchCode1s != null)
                            {
                                var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                                if (tumalInts.Any())
                                {
                                    obj.BranchCode2 = tumalInts.First().OBLASTSUM.Value;
                                }
                                else
                                {
                                    obj.BranchCode2 = 0;
                                }
                            }
                        }


                        {
                            var bBranchCode1s = branches.Where(z => z.code.Equals("А3")).FirstOrDefault();

                            if (bBranchCode1s != null)
                            {
                                var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                                if (tumalInts.Any())
                                {
                                    obj.BranchCode3 = tumalInts.First().OBLASTSUM.Value;
                                }
                                else
                                {
                                    obj.BranchCode3 = 0;
                                }
                            }
                        }


                        {
                            var bBranchCode1s = branches.Where(z => z.code.Equals("Д1")).FirstOrDefault();

                            if (bBranchCode1s != null)
                            {
                                var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                                if (tumalInts.Any())
                                {
                                    obj.BranchCode4 = tumalInts.First().OBLASTSUM.Value;
                                }
                                else
                                {
                                    obj.BranchCode4 = 0;
                                }
                            }
                        }


                        {
                            var bBranchCode1s = branches.Where(z => z.code.Equals("И1")).FirstOrDefault();

                            if (bBranchCode1s != null)
                            {
                                var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                                if (tumalInts.Any())
                                {
                                    obj.BranchCode5 = tumalInts.First().OBLASTSUM.Value;
                                }
                                else
                                {
                                    obj.BranchCode5 = 0;
                                }
                            }
                        }



                        {
                            var bBranchCode1s = branches.Where(z => z.code.Equals("К1")).FirstOrDefault();

                            if (bBranchCode1s != null)
                            {
                                var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                                if (tumalInts.Any())
                                {
                                    obj.BranchCode6 = tumalInts.First().OBLASTSUM.Value;
                                }
                                else
                                {
                                    obj.BranchCode6 = 0;
                                }
                            }
                        }



                        {
                            var bBranchCode1s = branches.Where(z => z.code.Equals("М1")).FirstOrDefault();

                            if (bBranchCode1s != null)
                            {
                                var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                                if (tumalInts.Any())
                                {
                                    obj.BranchCode7 = tumalInts.First().OBLASTSUM.Value;
                                }
                                else
                                {
                                    obj.BranchCode7 = 0;
                                }
                            }
                        }


                        {
                            var bBranchCode1s = branches.Where(z => z.code.Equals("Н1")).FirstOrDefault();

                            if (bBranchCode1s != null)
                            {
                                var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                                if (tumalInts.Any())
                                {
                                    obj.BranchCode8 = tumalInts.First().OBLASTSUM.Value;
                                }
                                else
                                {
                                    obj.BranchCode8 = 0;
                                }
                            }
                        }


                        {
                            var bBranchCode1s = branches.Where(z => z.code.Equals("С1")).FirstOrDefault();

                            if (bBranchCode1s != null)
                            {
                                var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                                if (tumalInts.Any())
                                {
                                    obj.BranchCode9 = tumalInts.First().OBLASTSUM.Value;
                                }
                                else
                                {
                                    obj.BranchCode9 = 0;
                                }
                            }
                        }


                        {
                            var bBranchCode1s = branches.Where(z => z.code.Equals("Т1")).FirstOrDefault();

                            if (bBranchCode1s != null)
                            {
                                var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                                if (tumalInts.Any())
                                {
                                    obj.BranchCode10 = tumalInts.First().OBLASTSUM.Value;
                                }
                                else
                                {
                                    obj.BranchCode10 = 0;
                                }
                            }
                        }


                        {
                            var bBranchCode1s = branches.Where(z => z.code.Equals("Т2")).FirstOrDefault();

                            if (bBranchCode1s != null)
                            {
                                var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                                if (tumalInts.Any())
                                {
                                    obj.BranchCode11 = tumalInts.First().OBLASTSUM.Value;
                                }
                                else
                                {
                                    obj.BranchCode11 = 0;
                                }
                            }
                        }


                        {
                            var bBranchCode1s = branches.Where(z => z.code.Equals("Т3")).FirstOrDefault();

                            if (bBranchCode1s != null)
                            {
                                var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                                if (tumalInts.Any())
                                {
                                    obj.BranchCode12 = tumalInts.First().OBLASTSUM.Value;
                                }
                                else
                                {
                                    obj.BranchCode12 = 0;
                                }
                            }
                        }


                        {
                            var bBranchCode1s = branches.Where(z => z.code.Equals("Э1")).FirstOrDefault();

                            if (bBranchCode1s != null)
                            {
                                var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                                if (tumalInts.Any())
                                {
                                    obj.BranchCode13 = tumalInts.First().OBLASTSUM.Value;
                                }
                                else
                                {
                                    obj.BranchCode13 = 0;
                                }
                            }
                        }


                        {

                                var tumalInts = tumanInternals.Where(z => z.otraslName.Equals("Неопределен"));

                                if (tumalInts.Any())
                                {
                                    obj.BranchCode14 = tumalInts.First().OBLASTSUM.Value;
                                }
                                else
                                {
                                    obj.BranchCode14 = 0;
                                }
                        }

                        pivodList.Add(obj);
                    }
                }

                return Json(pivodList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }
            return null;
        }


        public void PivodTest()
        {
            var data = KasabaDB.sp_rt_Razbivka_Postup_OtraslGroup("1726",DateTime.Now.AddDays(-40), DateTime.Now, "17226");

            var RetList = data.ToList();

            var TumanList = RetList.Where(z => z.otraslName.Contains("Итого") && !z.OblastName.Contains("Итого"));

            List<OtraslPivod> pivodList = new List<OtraslPivod>();

            if (TumanList.Any())
            {
                var branches = KasabaDB.sp_branch.ToList();



                foreach (var tuman in TumanList)
                {
                    var obj = new OtraslPivod();

                    obj.TumanName = tuman.OblastName;
                    obj.TumanAmount = tuman.OBLASTSUM.Value;

                    var tumanInternals = RetList.Where(z => z.OblastName.Equals(tuman.OblastName) && !z.otraslName.Contains("Итого"));

                    {
                        var bBranchCode1s = branches.Where(z => z.code.Equals("А1")).FirstOrDefault();

                        if (bBranchCode1s != null)
                        {
                            var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                            if (tumalInts.Any())
                            {
                                obj.BranchCode1 = tumalInts.First().OBLASTSUM.Value;
                            }
                            else
                            {
                                obj.BranchCode1 = 0;
                            }
                        }
                    }


                    {
                        var bBranchCode1s = branches.Where(z => z.code.Equals("А2")).FirstOrDefault();

                        if (bBranchCode1s != null)
                        {
                            var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                            if (tumalInts.Any())
                            {
                                obj.BranchCode2 = tumalInts.First().OBLASTSUM.Value;
                            }
                            else
                            {
                                obj.BranchCode2 = 0;
                            }
                        }
                    }


                    {
                        var bBranchCode1s = branches.Where(z => z.code.Equals("А3")).FirstOrDefault();

                        if (bBranchCode1s != null)
                        {
                            var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                            if (tumalInts.Any())
                            {
                                obj.BranchCode3 = tumalInts.First().OBLASTSUM.Value;
                            }
                            else
                            {
                                obj.BranchCode3 = 0;
                            }
                        }
                    }


                    {
                        var bBranchCode1s = branches.Where(z => z.code.Equals("Д1")).FirstOrDefault();

                        if (bBranchCode1s != null)
                        {
                            var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                            if (tumalInts.Any())
                            {
                                obj.BranchCode4 = tumalInts.First().OBLASTSUM.Value;
                            }
                            else
                            {
                                obj.BranchCode4 = 0;
                            }
                        }
                    }


                    {
                        var bBranchCode1s = branches.Where(z => z.code.Equals("И1")).FirstOrDefault();

                        if (bBranchCode1s != null)
                        {
                            var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                            if (tumalInts.Any())
                            {
                                obj.BranchCode5 = tumalInts.First().OBLASTSUM.Value;
                            }
                            else
                            {
                                obj.BranchCode5 = 0;
                            }
                        }
                    }



                    {
                        var bBranchCode1s = branches.Where(z => z.code.Equals("К1")).FirstOrDefault();

                        if (bBranchCode1s != null)
                        {
                            var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                            if (tumalInts.Any())
                            {
                                obj.BranchCode6 = tumalInts.First().OBLASTSUM.Value;
                            }
                            else
                            {
                                obj.BranchCode6 = 0;
                            }
                        }
                    }



                    {
                        var bBranchCode1s = branches.Where(z => z.code.Equals("М1")).FirstOrDefault();

                        if (bBranchCode1s != null)
                        {
                            var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                            if (tumalInts.Any())
                            {
                                obj.BranchCode7 = tumalInts.First().OBLASTSUM.Value;
                            }
                            else
                            {
                                obj.BranchCode7 = 0;
                            }
                        }
                    }


                    {
                        var bBranchCode1s = branches.Where(z => z.code.Equals("Н1")).FirstOrDefault();

                        if (bBranchCode1s != null)
                        {
                            var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                            if (tumalInts.Any())
                            {
                                obj.BranchCode8 = tumalInts.First().OBLASTSUM.Value;
                            }
                            else
                            {
                                obj.BranchCode8 = 0;
                            }
                        }
                    }


                    {
                        var bBranchCode1s = branches.Where(z => z.code.Equals("С1")).FirstOrDefault();

                        if (bBranchCode1s != null)
                        {
                            var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                            if (tumalInts.Any())
                            {
                                obj.BranchCode9 = tumalInts.First().OBLASTSUM.Value;
                            }
                            else
                            {
                                obj.BranchCode9 = 0;
                            }
                        }
                    }


                    {
                        var bBranchCode1s = branches.Where(z => z.code.Equals("Т1")).FirstOrDefault();

                        if (bBranchCode1s != null)
                        {
                            var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                            if (tumalInts.Any())
                            {
                                obj.BranchCode10 = tumalInts.First().OBLASTSUM.Value;
                            }
                            else
                            {
                                obj.BranchCode10 = 0;
                            }
                        }
                    }


                    {
                        var bBranchCode1s = branches.Where(z => z.code.Equals("Т2")).FirstOrDefault();

                        if (bBranchCode1s != null)
                        {
                            var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                            if (tumalInts.Any())
                            {
                                obj.BranchCode11 = tumalInts.First().OBLASTSUM.Value;
                            }
                            else
                            {
                                obj.BranchCode11 = 0;
                            }
                        }
                    }


                    {
                        var bBranchCode1s = branches.Where(z => z.code.Equals("Т3")).FirstOrDefault();

                        if (bBranchCode1s != null)
                        {
                            var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                            if (tumalInts.Any())
                            {
                                obj.BranchCode12 = tumalInts.First().OBLASTSUM.Value;
                            }
                            else
                            {
                                obj.BranchCode12 = 0;
                            }
                        }
                    }


                    {
                        var bBranchCode1s = branches.Where(z => z.code.Equals("Э1")).FirstOrDefault();

                        if (bBranchCode1s != null)
                        {
                            var tumalInts = tumanInternals.Where(z => z.otraslName.Equals(bBranchCode1s.name_ru));

                            if (tumalInts.Any())
                            {
                                obj.BranchCode13 = tumalInts.First().OBLASTSUM.Value;
                            }
                            else
                            {
                                obj.BranchCode13 = 0;
                            }
                        }
                    }


                    {

                        var tumalInts = tumanInternals.Where(z => z.otraslName.Equals("Неопределен"));

                        if (tumalInts.Any())
                        {
                            obj.BranchCode14 = tumalInts.First().OBLASTSUM.Value;
                        }
                        else
                        {
                            obj.BranchCode14 = 0;
                        }
                    }

                    pivodList.Add(obj);
                }
            }
        }


        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
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
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "s;bg;end", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
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


        public List<CodeDecode> GetOtchetRegionList(string departmentBase)
        {
            const string FEDERATION_DEPCODE = "17";

            if (departmentBase.Equals(FEDERATION_DEPCODE))
            {
                var R = (from r in KasabaDB.sp_soato
                         where r.parent_code == FEDERATION_DEPCODE && r.code.StartsWith(departmentBase)
                         select new CodeDecode
                         {
                             value  = r.code,
                             text = r.name_uz
                         });
                var resList = R.ToList();

                resList.Insert(0, new CodeDecode { value = "0", text = "Все" });

                return resList;
            }
            else
            {
                var soatoCode = departmentBase.Length == 5 ?
                    departmentBase.Substring(0, 2) + departmentBase.Substring(3, 2) : departmentBase;

                var R = (from r in KasabaDB.sp_soato
                         where r.parent_code == FEDERATION_DEPCODE && r.code.StartsWith(soatoCode)
                         select new CodeDecode
                         {
                             value = r.code,
                             text = r.name_uz
                         });
                var resList = R.ToList();

                return resList;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
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