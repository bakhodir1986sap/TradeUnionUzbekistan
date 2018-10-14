using KasabaKiat.BusinessLogic;
using KasabaKiat.BusinessLogic.Putyovki.RealizasiyaPutyovok;
using KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok;
using KasabaKiat.BusinessLogic.Utils.References;
using KasabaKiat.Helpers;
using KasabaKiat.ViewModel;
using KasabaKiat.ViewModel.Putyovki;
using KasabaKIAT.BusinessLogic.Putyovki.PrixodPutyovok;
using KasabaKIAT.BusinessLogic.Putyovki.RealizasiyaPutyovok.RealizasitaPoTz;
using KasabaKIAT.BusinessLogic.Putyovki.VozvratPutyovki.VozvratPoTz;
using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Putyovki;
using log4net;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KasabaKiat.BusinessLogic.Putyovki.OplataPutyovok;
using KasabaKiat.BusinessLogic.VozvratPutyovki;
using KasabaKIAT.BusinessLogic.Putyovki.VozvratPutyovki;
using KasabaKiat.BusinessLogic.Putyovki.VozvratPutyovki;
using KasabaKIAT.BusinessLogic.Putyovki.ZakritiyaKvartala;
using KasabaKIAT.BusinessLogic.Putyovki.RaznoskaPoPutyovki;
using KasabaKiat.BusinessLogic.Putyovki.RaznoskaPoPutyovki;
using KasabaKiat.ViewModel.Putyovki.RaznoskaPutyovok;
using KasabaKIAT.BusinessLogic.Putyovki.SverkaDannix.Ruchnoy;
using OfficeOpenXml;
using System.Threading.Tasks;
using KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki;
using KasabaKIAT.ViewModel.Buxgalteriya;
using KasabaKIAT.BusinessLogic.Buxgalteriya.GlavnayaKniga;
using System.Globalization;
using KasabaKIAT.BusinessLogic.StartProcess.LoaderViewModels;
using KasabaKIAT.BusinessLogic.StartProcess.LoaderDataManagers;
using KasabaKIAT.BusinessLogic.Putyovki.PutyovkiHistory;
using KasabaKIAT.BusinessLogic.Putyovki.OplataPutyovok.Template;
using KasabaKIAT.BusinessLogic.ClosePeriod.CloseKvartal;
using KasabaKIAT.BusinessLogic._3_Etap.Statistika;
using System.Data.Entity.Validation;

namespace KasabaKIAT.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class PrixodPutevokController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        ILog log = log4net.LogManager.GetLogger(typeof(HomeController));

        /// <summary>
        /// 
        /// </summary>
        KasabaKIATEntities KasabaDB = new KasabaKIATEntities();

        /// <summary>
        /// 
        /// </summary>
        BronirovaniyaPutyovki BP = new BronirovaniyaPutyovki();

        /// <summary>
        /// 
        /// </summary>
        RealizasiyaPoTzCl OR = new RealizasiyaPoTzCl();

        /// <summary>
        /// 
        /// </summary>
        VozvratPutyovki VP = new VozvratPutyovki();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult Index()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            if (UserId == 0)
            {
                return RedirectToAction("SignOut", "Login");
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult ProsmotrRealzPutevok()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult ProsmotrOstatPutevok()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            if (UserId == 0)
            {
                return RedirectToAction("SignOut", "Login");
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult RealizatsiyaPutevok()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            if (UserId == 0)
            {
                return RedirectToAction("SignOut", "Login");
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult SverkaDannix()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn", VaryByCustom = "userId")]
        public JsonResult GetZList(int inn)
        {
            var data = KasabaDB.sp_rt_ZayavkiList(inn);
            var list = from d in data
                       where d.STATUS_ZAYAVKI == "4" //Ma'qullangan
                       select new
                       {
                           d0=d.REC_ID,
                           d1=d.FULL_NAME,
                           d2=d.Name_San,
                           d3=d.monthDate,
                           d4=d.STATUS_ZAYAVKI
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        
        public JsonResult GetOformleniyaRealizatsiyaPutevok(SanPutTemp s)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var uu = UserInfo.GetUserInfo(UserId);
            var data = OR.GetOformRealizList(s.b, s.inn, uu.DepartmentBase, uu.userid);
            var list = from d in data
                       select new
                       {
                           d0 = d.rec_id,
                           d1 = d.SanatoryName,
                           d2 = d.serail_number,
                           d3 = d.rest_begin_date,
                           d4 = d.full_name,
                           d5 = d.price,
                           d6=d.payment_persent,
                           d7=d.org_inn,
                           d8=d.nakladnoy_nomer,
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="foiz"></param>
        /// <returns></returns>
        public string Save(int id,int foiz)
        {
            RealizasiyaDataManager.Modify(new RealizasiyaByTzViewModel { rec_id=id,payment_persent= foiz });
            return "yes";
        }


        public class SanPutTemp
        {
            public List<SanAndPutViewModel> b { get; set; }

            public string inn { get; set; }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ORP(List<SanAndPutViewModel> b, string inn)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var uu = UserInfo.GetUserInfo(UserId);

            return View(OR.GetOformRealizList(b, inn, uu.DepartmentBase, uu.userid));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetSanatory()
        {
            var S = (from s in KasabaDB.tblSprav_San
                     select new
                     {
                         id = s.ID,
                         value = s.Name_San
                     });
            return Json(S.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetCamp()
        {
            var C = (from s in KasabaDB.t_camp
                     select new
                     {
                         id = s.id,
                         value = s.camps_name
                     });
            return Json(C, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetOR()
        {
            var C = (from s in KasabaDB.t_departmentbase
                     select new
                     {
                         id = s.id,
                         value = s.bkut_name
                     });
            return Json(C, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc_id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "sc_id",VaryByCustom = "userId")]
        public JsonResult GetPP(int sc_id)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);
            var list = (from a in KasabaDB.tblPos_Put
                        where a.ID_Sanat == sc_id&&a.ID_PolVoz==false &&a.Departmentbase== ustinfo.DepartmentBase
                        join sc in KasabaDB.tblSprav_San on a.ID_Sanat equals sc.ID
                        select new
                        { 
                            ID = a.ID,
                            d1 = sc.Name_San,
                            d1_id = sc.ID,
                            d2 = a.Num_Put,
                            d3 = a.DataSan,
                            d4 = a.Symma_Put,
                            d5 = a.Data_Post
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc_id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "sc_id", VaryByCustom = "userId")]
        public JsonResult GetPP_SUM(int sc_id)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);
            var list = (from a in KasabaDB.tblPos_Put
                        where a.ID_Sanat == sc_id && a.ID_PolVoz == false && a.Departmentbase == ustinfo.DepartmentBase
                        join sc in KasabaDB.tblSprav_San on a.ID_Sanat equals sc.ID
                        select new
                        {
                            ID = a.ID,
                            d1 = sc.Name_San,
                            d1_id = sc.ID,
                            d2 = a.Num_Put,
                            d3 = a.DataSan,
                            d4 = a.Symma_Put,
                            d5 = a.Data_Post
                        });
            var Groups = from p in list
                         group p by p.d1_id into g
                         select new { P_SUM = g.Sum(i => i.d4) };
            return Json(Groups.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn;name", VaryByCustom = "userId")]
        public JsonResult Getegrpo(string inn,string name)
        {
            log.Info("Getegrpo");
            log.Info("inn="+inn);
            log.Info("name="+name);
            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var ustinfo = UserInfo.GetUserInfo(UserId);
                string str = ustinfo.DepartmentBase.Substring(0, 2) + ustinfo.DepartmentBase.Substring(3, 2);

                if ((string.IsNullOrEmpty(name)) || (name.ToLower().Contains("undefined")))
                {
                    name = string.Empty;
                }

                var data = KasabaDB.sp_rt_GetOrganization(ustinfo.DepartmentBase, inn, name);
                var list = (from a in data //KasabaDB.t_egrpo
                            //where a.soato_id.Contains(str) && (a.inn == inn || a.inn.Contains(inn) || a.name_full.Trim().Contains(name.Trim()) || a.name_short.Trim().Contains(name.Trim()))
                            select new
                            {
                                d0 = a.id,
                                d1 = a.address,
                                d2 = a.name_full,
                                d3 = a.name_short,
                                d4 = a.inn,
                            });

                var resList = list.ToList();
                log.Info("Count = " + resList.Count);

                return Json(resList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn;name", VaryByCustom = "userId")]
        public JsonResult Getegrpo1(string inn, string name)
        {
            log.Info("Getegrpo");
            log.Info("inn=" + inn);
            log.Info("name=" + name);
            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var ustinfo = UserInfo.GetUserInfo(UserId);
                string str = ustinfo.DepartmentBase.Substring(0, 2) + ustinfo.DepartmentBase.Substring(3, 2);

                if (string.IsNullOrEmpty(name))
                {
                    name = string.Empty;
                }

                var data = KasabaDB.sp_rt_GetOrganization(ustinfo.DepartmentBase, inn, name);
                var list = (from a in data //KasabaDB.t_egrpo
                                           //where a.soato_id.Contains(str) && (a.inn == inn || a.inn.Contains(inn) || a.name_full.Trim().Contains(name.Trim()) || a.name_short.Trim().Contains(name.Trim()))
                            select new
                            {
                                d0 = a.id,
                                d1 = a.address,
                                d2 = a.name_full,
                                d3 = a.name_short,
                                d4 = a.inn,
                            });

                var resList = list.ToList();
                log.Info("Count = " + resList.Count);

                return Json(resList.First(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn", VaryByCustom = "userId")]
        public string LoadINN(int inn)
        {
            string strInn = inn.ToString();
            var query = from department in KasabaDB.t_egrpo
                        where department.inn == strInn
                        select department.name_full;
            if (query.Any())
            {
                return query.First();
            }
            return "No Data Found";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult SD()
        {
            var list = from d in KasabaDB.tbpt_sanatory_info
                       select new
                       {
                           d1 = d.SERIAL_NUMBER,
                           d2 = d.SANATORY_NAME,
                           d3 = d.FACT_REST_BEGIN_DT,
                           d4 = d.FULL_NAME,
                           d5 = d.INN,
                           d6 = d.PASSPORT_NUMBER,
                           d7 = d.WORK_PALCE,
                           d8 = d.PUTYOVKA_PRICE,
                           d9=d.PUTYOVKA_PRICE+d.ADDITIONAL_PAYMENTS
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult RP()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);
            var data = KasabaDB.sp_get_RealizasiyaPutyovokGroup(ustinfo.DepartmentBase);
            var list = from d in data
                       select new
                       {
                           d1=d.SANID,
                           d2=d.SAN_NAME,
                           d3=d.MIN_NUM,
                           d4=d.MAX_NUM,
                           d5=d.MIN_DATE,
                           d6=d.MAX_DATE,
                           d7=d.BRON,
                           d8=d.URGENT,
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "id", VaryByCustom = "userId")]
        public JsonResult RP2(int id)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);
            var data = KasabaDB.sp_get_RealizasiyaPutyovokDetail(ustinfo.DepartmentBase,id);
            var list = from d in data
                       select new
                       {
                           d0=d.ID,
                           d1 = d.ID_SANAT,
                           d2 = d.Name_San,
                           d3 = d.NUM_PUT,
                           d4 = d.DATASAN,
                           d5 = d.BRON_FLG,
                           d6 = d.URGENT_FLG,
                           green = d.BRON_FLG == 1 ? "green" : "",
                           red = d.URGENT_FLG == 1 ? "red" : "",
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult PRP()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);
            var data = KasabaDB.sp_get_ProsmotrRealizovannixPutyovok(ustinfo.DepartmentBase);
            var list = from d in data
                       select new
                       {
                           d0=d.REC_ID,
                           d1 = d.SERAIL_NUMBER,
                           d2 = d.Name_San,
                           d3 = d.REST_BEGIN_DATE,
                           d4 = d.REST_END_DATE,
                           d5 = d.PRICE,
                           d6 = d.QUANTITY,
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "id", VaryByCustom = "userId")]
        public JsonResult PRP2(int id)
        {
            var data = KasabaDB.sp_get_ProsmotrRealizovannixPutyovokDetali(id);
            var list = from d in data
                       select new
                       {
                           d0 = d.REC_ID,
                           d1 = d.SERAIL_NUMBER,
                           d2 = d.Name_San,
                           d3 = d.REST_BEGIN_DATE,
                           d4 = d.REST_END_DATE,
                           d5 = d.PLATEJ_DATE,
                           d6 = d.MakeOrgName,
                           d7=d.PRICE,
                           d8=d.PAYMENT_PERSENT,
                           d9=d.NAKLADNOY_NOMER,
                           d10= d.PRICE * d.PAYMENT_PERSENT/100,
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "id", VaryByCustom = "userId")]
        public JsonResult PRP3(int id)
        {
            var data = KasabaDB.sp_get_RealizasiyaPutyovokDetailN(id);
            var list = from d in data
                       select new
                       {
                           d0 = d.REC_ID,
                           d1 = d.KOD,
                           d2 = d.INN,
                           d3 = d.OKONX_NAME,
                           d4 = d.name_full,
                           d5 = d.FULL_NAME,
                           d6 = d.POSITION,
                           d7 = d.SOTR_INN,
                           d8 = d.REMARK,
                           d9 = d.TALON_NUMBER,
                           d10 = d.RECIEVER,
                           d11=d.DOVER_ISSUE_DATE,
                           d12=d.DOVER_EXPIRE_DATE,
                           d13=d.DOVERENNOST_NUMBER
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public string vozvrat(long id,string remark)
        {
            VP.VozvratPutyovkiProcess(id, remark);
            return "yes";
        }
        public class BronTemp
        {
            public int ID_San { get; set; }
            public string Num_Put { get; set; }
        }

        public string RPBron(List<BronTemp> b)
        {
            foreach (var bb in b)
            {
                var br = new BronViewModel();
                br.ID_San = bb.ID_San;
                br.Num_Put = bb.Num_Put;

                BP.PostavitV_Bron(br);
            }
            //BP.PostavitV_Bron(b);
            return "yes";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public string RPSnyat(List<BronTemp> b)
        {
            foreach (var bb in b)
            {
                var br = new BronViewModel();
                br.ID_San = bb.ID_San;
                br.Num_Put = bb.Num_Put;

                BP.SnyatBron(br);
            }
            //BP.PostavitV_Bron(b);
            return "yes";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc_id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "sc_id", VaryByCustom = "userId")]
        public JsonResult GetPrixodPutevok_S(int sc_id)
        {
            var list = (from a in KasabaDB.tbpt_putyovki
                        where a.STATUS == "1" && a.SANATORIUM_ID==sc_id
                        //join u in KasabaDB.users on a.USER_ID equals u.id
                        join sc in KasabaDB.tblSprav_San on a.SANATORIUM_ID equals sc.ID
                        //join d in KasabaDB.t_departmentbase on a.DEPARTMENT_CODE equals d.code
                        select new
                        {
                            ID = a.REC_ID,
                            SB = a.SERIAL_BEGIN,
                            SE = a.SERIAL_END,
                            SC = sc.Name_San,
                            SC_id = sc.ID,
                            RBD = a.REST_BEGIN_DATE,
                            RED = a.REST_END_DATE,
                            P = a.PRICE,
                            RB = a.REMAIN_BAL,
                            Q_IN = a.QUANTITY_IN,
                            Q_OUT = a.QUANTITY_OUT,
                            QR = a.QUANTITY_REMAIN,
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc_id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "sc_id", VaryByCustom = "userId")]
        public JsonResult GetPrixodPutevok_C(int sc_id)
        {
            var list = (from a in KasabaDB.tbpt_putyovki
                        where a.STATUS == "1" && a.CAMP_ID== sc_id
                        //join u in KasabaDB.users on a.USER_ID equals u.id
                        join sc in KasabaDB.t_camp on a.CAMP_ID equals sc.id
                        //join d in KasabaDB.t_departmentbase on a.DEPARTMENT_CODE equals d.code
                        select new
                        {
                            ID = a.REC_ID,
                            SB = a.SERIAL_BEGIN,
                            SE = a.SERIAL_END,
                            SC = sc.camps_name,
                            SC_id = sc.id,
                            RBD = a.REST_BEGIN_DATE,
                            RED = a.REST_END_DATE,
                            P = a.PRICE,
                            RB = a.REMAIN_BAL,
                            Q_IN = a.QUANTITY_IN,
                            Q_OUT = a.QUANTITY_OUT,
                            QR = a.QUANTITY_REMAIN,
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc_id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "sc_id", VaryByCustom = "userId")]
        public JsonResult GetPP_SUM_S(int sc_id)
        {
            var list = (from a in KasabaDB.tbpt_putyovki
                        where a.STATUS == "1" && a.SANATORIUM_ID == sc_id
                        //join u in KasabaDB.users on a.USER_ID equals u.id
                        join sc in KasabaDB.tblSprav_San on a.SANATORIUM_ID equals sc.ID
                        //join d in KasabaDB.t_departmentbase on a.DEPARTMENT_CODE equals d.code
                        select new
                        {
                            ID = a.USER_ID,
                            P = a.PRICE,
                            RB = a.REMAIN_BAL,
                            QR = a.QUANTITY_REMAIN,
                        });
            var Groups = from p in list
                         group p by p.ID into g
                         select new { P_SUM = g.Sum(i => i.P), QR_SUM = g.Sum(i => i.QR), RB_SUM = g.Sum(i => i.RB) };
            return Json(Groups.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc_id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "sc_id", VaryByCustom = "userId")]
        public JsonResult GetPP_SUM_C(int sc_id)
        {
            var list = (from a in KasabaDB.tbpt_putyovki
                        where a.STATUS == "1" && a.CAMP_ID == sc_id
                        //join u in KasabaDB.users on a.USER_ID equals u.id
                        join sc in KasabaDB.t_camp on a.CAMP_ID equals sc.id
                        //join d in KasabaDB.t_departmentbase on a.DEPARTMENT_CODE equals d.code
                        select new
                        {
                            ID = a.USER_ID,
                            P = a.PRICE,
                            RB = a.REMAIN_BAL,
                            QR = a.QUANTITY_REMAIN,
                        });
            var Groups = from p in list
                         group p by p.ID into g
                         select new { P_SUM = g.Sum(i => i.P), QR_SUM = g.Sum(i => i.QR), RB_SUM = g.Sum(i => i.RB) };
            return Json(Groups.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn", VaryByCustom = "userId")]
        public JsonResult GetPrixodPutevok_S_LoadINN(string inn)
        {
            var list = (from a in KasabaDB.tbpt_putyovki
                        where a.STATUS == "1" //&& a.SANATORIUM_ID == sc_id
                        //join u in KasabaDB.users on a.USER_ID equals u.id
                        join sc in KasabaDB.tblSprav_San on a.SANATORIUM_ID equals sc.ID
                        join d in KasabaDB.t_departmentbase on a.DEPARTMENT_CODE equals d.code
                        where d.inn==inn
                        select new
                        {
                            ID = a.REC_ID,
                            SB = a.SERIAL_BEGIN,
                            SE = a.SERIAL_END,
                            SC = sc.Name_San,
                            SC_id = sc.ID,
                            RBD = a.REST_BEGIN_DATE,
                            RED = a.REST_END_DATE,
                            P = a.PRICE,
                            RB = a.REMAIN_BAL,
                            Q_IN = a.QUANTITY_IN,
                            Q_OUT = a.QUANTITY_OUT,
                            QR = a.QUANTITY_REMAIN,
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn", VaryByCustom = "userId")]
        public JsonResult GetPrixodPutevok_C_LoadINN(string inn)
        {
            var list = (from a in KasabaDB.tbpt_putyovki
                        where a.STATUS == "1"// && a.CAMP_ID == sc_id
                        //join u in KasabaDB.users on a.USER_ID equals u.id
                        join sc in KasabaDB.t_camp on a.CAMP_ID equals sc.id
                        join d in KasabaDB.t_departmentbase on a.DEPARTMENT_CODE equals d.code
                        where d.inn==inn
                        select new
                        {
                            ID = a.REC_ID,
                            SB = a.SERIAL_BEGIN,
                            SE = a.SERIAL_END,
                            SC = sc.camps_name,
                            SC_id = sc.id,
                            RBD = a.REST_BEGIN_DATE,
                            RED = a.REST_END_DATE,
                            P = a.PRICE,
                            RB = a.REMAIN_BAL,
                            Q_IN = a.QUANTITY_IN,
                            Q_OUT = a.QUANTITY_OUT,
                            QR = a.QUANTITY_REMAIN,
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PP"></param>
        /// <returns></returns>
        public string AddPrixodPutevok(tbpt_putyovki PP)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            if (PP != null)
            {
                using (KasabaKIATEntities k = new KasabaKIATEntities())
                {
                    string DEPARTMENT = "";
                    long? DEP = null;
                    var Usr = from u in KasabaDB.users where u.id == UserId select u;
                    if (Usr.Any())
                    {
                        var d = Usr.FirstOrDefault();
                        if (d.bkut_id != null)
                        {
                            DEP = Convert.ToInt32(d.bkut_id);
                        }
                        else
                        {
                            if (d.dep_id != null)
                            {
                                DEP = Convert.ToInt32(d.dep_id);
                            }
                            else
                            {
                                if (d.bkuq_id != null)
                                {
                                    DEP = Convert.ToInt32(d.bkuq_id);
                                }
                            }
                        }
                    }
                    var Dpr = KasabaDB.t_departmentbase.Where(i => i.id == DEP).FirstOrDefault();
                    DEPARTMENT = Dpr.code;
                    tbpt_putyovki p1 = new tbpt_putyovki();
                    p1.DEPARTMENT_CODE = DEPARTMENT;
                    p1.SANATORIUM_ID = PP.SANATORIUM_ID;
                    p1.CAMP_ID = PP.CAMP_ID;
                    p1.USER_ID =Convert.ToInt32(User.Identity.Name);
                    p1.SERIAL_BEGIN = PP.SERIAL_BEGIN;
                    p1.SERIAL_END = PP.SERIAL_END;
                    p1.REST_BEGIN_DATE = PP.REST_BEGIN_DATE;
                    p1.REST_END_DATE = PP.REST_END_DATE;
                    p1.PRICE = PP.PRICE;
                    p1.STATUS = "1";
                    p1.MODIFY_DATE = DateTime.Now;
                    p1.QUANTITY_IN = PP.QUANTITY_IN;
                    p1.QUANTITY_OUT = PP.QUANTITY_OUT;
                    p1.QUANTITY_REMAIN = PP.QUANTITY_IN - PP.QUANTITY_OUT;
                    p1.REMAIN_BAL = p1.QUANTITY_REMAIN * PP.PRICE;
                    k.tbpt_putyovki.Add(p1);
                    k.SaveChanges();
                    return "ok";
                }
            }
            else
            {
                return "error";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByParam = "inn", VaryByCustom = "userId")]
        public string LOAD_DEPARTMENT_CODE(string inn)
        {
            string name = "";
            var a = from d in KasabaDB.t_departmentbase orderby d.id select d;
            foreach (var v in a)
            {
                if (v.code == inn)
                {
                    name = v.code;
                    break;
                }
            }
            return name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PP"></param>
        /// <returns></returns>
        public string UpdatePrixodPutevok(tbpt_putyovki PP)
        {
            if (PP != null)
            {
                using (KasabaKIATEntities k = new KasabaKIATEntities())
                {
                    int id = Convert.ToInt32(PP.REC_ID);
                    tbpt_putyovki p1 = k.tbpt_putyovki.Where(i => i.REC_ID == id).FirstOrDefault();
                    p1.SANATORIUM_ID = PP.SANATORIUM_ID;
                    p1.CAMP_ID = PP.CAMP_ID;
                    p1.USER_ID =Convert.ToInt32(User.Identity.Name);
                    p1.SERIAL_BEGIN = PP.SERIAL_BEGIN;
                    p1.SERIAL_END = PP.SERIAL_END;
                    p1.REST_BEGIN_DATE = PP.REST_BEGIN_DATE;
                    p1.REST_END_DATE = PP.REST_END_DATE;
                    p1.PRICE = PP.PRICE;
                    p1.STATUS = "1";
                    p1.MODIFY_DATE = DateTime.Now;
                    p1.QUANTITY_IN = PP.QUANTITY_IN;
                    p1.QUANTITY_OUT = PP.QUANTITY_OUT;
                    p1.QUANTITY_REMAIN = PP.QUANTITY_IN - PP.QUANTITY_OUT;
                    p1.REMAIN_BAL = p1.QUANTITY_REMAIN * PP.PRICE;
                    k.SaveChanges();
                    return "ok";
                }
            }
            else
            {
                return "error";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeletePrixodPutevok(string id)
        {
            using (KasabaKIATEntities k = new KasabaKIATEntities())
            {
                int _id = Convert.ToInt32(id);
                tbpt_putyovki p1 = k.tbpt_putyovki.Where(i => i.REC_ID == _id).FirstOrDefault();
                p1.STATUS = "0";
                k.SaveChanges();
                return "ok";
            }
        }

        #region Organizasii
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Organizasii()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOrganization()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);



            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 500, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetOtraslForOrganization()
        {
            log.Info("GetOtraslForOrganization");

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var otrasList = Sprav_Otras.GetByDepartmentBase(ustinfo.DepartmentBase);

            log.Info(otrasList);

            return Json(otrasList.ToList(), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 500, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetOtraslForOrganizationRasxod()
        {
            log.Info("GetOtraslForOrganizationRasxod");

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var otrasList = Sprav_Otras.GetByDepartmentBase(ustinfo.DepartmentBase);

            var obj = new Sprav_OtrasViewModel();
            obj.ID_Ot = -999;
            obj.Name_Ot = "Все отрасли";
            otrasList.Add(obj);

            log.Info(otrasList);

            return Json(otrasList.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetRegionForOrganization()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var regionList = Sprav_Reg.GetByDepartmentCode(ustinfo.DepartmentBase, ustinfo.IsPrivOrg);

            return Json(regionList.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveOrganization(Sprav_OrgViewModel model)
        {
            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var ustinfo = UserInfo.GetUserInfo(UserId);

                model.DepartmentBase = ustinfo.DepartmentBase;
                model.UserId = UserId;

                if (model.ID != 0)
                {
                    Sprav_Org.Modify(model);
                }
                else
                {
                    Sprav_Org.Create(ref model);
                }
                
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 3, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetEmptyCarcas(long Id)
        {
            var model = new Sprav_OrgViewModel();

            if (Id != 0)
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var ustinfo = UserInfo.GetUserInfo(UserId);

                var list = Sprav_Org.GetByDeraprtmentBase(ustinfo.DepartmentBase, Id);

                if (list.Any())
                {
                    return Json(list.First(), JsonRequestBehavior.AllowGet);
                }
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetOrganizationList()
        {
            log.Info("GetOrganizationList");

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = Sprav_Org.GetByDeraprtmentBase(ustinfo.DepartmentBase);

            log.Info(list);

            return Json(list.ToList().Take(30), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Inn"></param>
        /// <param name="OtrasId"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetOrganizationFilter(string Name, string Inn, int? OtrasId)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = Sprav_Org.GetByDeraprtmentBase(ustinfo.DepartmentBase, Name, Inn, OtrasId);

            return Json(list.ToList().Take(30), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Inn"></param>
        /// <param name="OtrasId"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetOrganizationFilterM(string Name, string Inn, int? OtrasId)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = Sprav_Org.GetByDeraprtmentBaseM(ustinfo.DepartmentBase, Name, Inn, OtrasId);

            return Json(list.ToList().Take(30), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        public async Task<JsonResult> OrganizationOtrasl(FormCollection formCollection)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            log.Info("Save");
            try
            {
                if (Request != null)
                {
                    HttpPostedFileBase file = Request.Files["OtraslFile"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;
                            var listModel = new List<PutyovkiOstatokViewModel>();

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                try
                                {
                                    var orgModel = new Sprav_OrgViewModel();

                                    orgModel.INN = workSheet.Cells[rowIterator, 1].Value.ToString();

                                    int OtraslId = 0;
                                    if (int.TryParse(workSheet.Cells[rowIterator, 2].Value.ToString(), out OtraslId))
                                    {
                                        orgModel.ID_Ot = OtraslId;
                                    }
                                    else
                                    {
                                        orgModel.ID_Ot =null;
                                    }

                                    Sprav_Org.ModifyByTemplate(orgModel, KasabaDB);
                                }
                                catch (Exception ex)
                                {
                                    log.Info(ex);
                                }
                            }

                            await KasabaDB.SaveChangesAsync();
                        }
                    }
                }
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
                return Json("error", JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region FizLisoBaza

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult FizLiso()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveFizLiso(Sprav_OrgViewModel model)
        {
            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var ustinfo = UserInfo.GetUserInfo(UserId);

                model.DepartmentBase = ustinfo.DepartmentBase;
                model.UserId = UserId;

                if (model.ID != 0)
                {
                    Sprav_Org.ModifyFizLiso(model);
                }
                else
                {
                    Sprav_Org.CreateFizLiso(model);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 3, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetEmptyCarcasFizLiso(long Id)
        {
            var model = new Sprav_OrgViewModel();

            if (Id != 0)
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var ustinfo = UserInfo.GetUserInfo(UserId);

                var list = Sprav_Org.GetFizLisoList(ustinfo.DepartmentBase, Id);

                if (list.Any())
                {
                    return Json(list.First(), JsonRequestBehavior.AllowGet);
                }
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetFizLisoList()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = Sprav_Org.GetFizLisoList(ustinfo.DepartmentBase);

            return Json(list.ToList().Take(30), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Inn"></param>
        /// <param name="OtrasId"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetFizLisoFilter(string Name, string Inn, string Pas_ser, string Pas_Number)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = Sprav_Org.GetFizLisoList(Name, Inn, Pas_ser, Pas_Number, ustinfo.DepartmentBase);

            return Json(list.ToList().Take(30), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FileResult DownloadTemplateNeopr()
        {
            var ArxiveDirectory = Properties.Settings.Default.TemplateDir;

            var targetFile = Path.Combine(ArxiveDirectory, "NeoprZagruska.xlsx");

            if (!System.IO.File.Exists(targetFile)) return null;

            byte[] fileBytes = System.IO.File.ReadAllBytes(targetFile);
            string fileName = "NeoprZagruska.xlsx";
            return File(fileBytes, "application/vnd.ms-excel", fileName);
        }


        #endregion

        #region Ilxom

        #region PrixodPutyovki

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult PrixodPutyovokRuchnoy()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetMainList(int year, int kvartal)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = await PostMainDataManager.Get(new PostupleniyaViewModel()
            {

                Id_Kv = kvartal,
                SmFil = year,
                DepartmentBase = ustinfo.DepartmentBase

            });

            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdPost"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetDetailList(long IdPost)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = await PostPutDetailDataManager.GetFiltered(new PostPutyovkaViewModel()
            {
                IdPost = IdPost,
                Departmentbase = ustinfo.DepartmentBase
            });

            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdPost"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetDetailListRes(long IdPost)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = await PostPutDetailDataManager.GetFiltered(new PostPutyovkaViewModel()
            {
                IdPost = IdPost,
                Departmentbase = ustinfo.DepartmentBase
            });

            var res = new TotalResults
            {
                PutyovkaCount = list.Count(),
                SummaNakladnix = list.Sum(z => z.Symma_Put)
            };

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> AddMain(PostupleniyaViewModel model)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            model.UserId = UserId;
            model.DepartmentBase = ustinfo.DepartmentBase;

            try
            {
                tblPost pp = null;

                PostMainDataManager.Create(ref model, KasabaDB, out pp);

                await KasabaDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> ModifyMain(PostupleniyaViewModel model)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            model.UserId = UserId;
            model.DepartmentBase = ustinfo.DepartmentBase;

            try
            {
                await PostMainDataManager.Modify(model);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> RemoveAllWithLinked(PostupleniyaViewModel model)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            model.UserId = UserId;
            model.DepartmentBase = ustinfo.DepartmentBase;

            try
            {
                var  res = await PostMainDataManager.Remove(new PostupleniyaViewModel()
                {
                    Id = model.Id
                });

                if (!res) return Json("Нельзя удалить закрытую накладную", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("Накладная удачно удалена", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<JsonResult> RemovePutyovkaDetail(List<long> putyovkaL)
        {
            if (putyovkaL == null) return Json("fail", JsonRequestBehavior.AllowGet);

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            try
            {
                foreach(var putyovkaId in putyovkaL)
                {
                    var model = new PostPutyovkaViewModel
                    {
                        UserId = UserId,
                        Departmentbase = ustinfo.DepartmentBase,
                        Id = putyovkaId
                    };

                    var putyovka = await PostPutDetailDataManager.Get2(model);

                    if (!putyovka.Any()) continue;

                    var putObj = putyovka.First();

                    await PostPutDetailDataManager.Remove(new PostPutyovkaViewModel()
                    {
                        Id = putyovkaId
                    });

                    //History making
                    var histModel = new PutHistViewModel
                    {
                        event_tp = (int)HistEventType.UdalitPutIzPrixod,
                        departmentbase = model.Departmentbase,
                        id_sanat = putObj.IdSanat,
                        num_put = putObj.Num_Put,
                        remark = string.Format("Сумма {0} - Дата заезда {1}", putObj.Symma_Put, putObj.DataSan),
                        userid = model.UserId
                    };

                    PutHistDataManager.Create(histModel);
                }
            }
            catch(Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetTotalResults(int year, int kvartal)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var totalres = await PostMainDataManager.GetTotals(year, kvartal, ustinfo.DepartmentBase);

            return Json(totalres, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult ParsePrixodPutyovka(PrixodDetailViewModel model)
        {
            if ((model.IdPost == 0) || (model.IdPost == null))
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            model.UserId = UserId;
            model.DepartmentBase = ustinfo.DepartmentBase;

            var list = PostPutDetailDataManager.ParsePrixod(model);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public class ExceptionList
        {
            public string Num_Put_SanId { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lModel"></param>
        /// <returns></returns>
        public async Task<JsonResult> AddDetail(List<PostPutyovkaViewModel> lModel, List<string> expLst)
        {
            try
            {
                foreach (var mm in lModel)
                {
                    var put_san = string.Format("{0}_{1}", mm.Num_Put, mm.IdSanat);

                    if (expLst != null)
                    {
                        var exp = expLst.Where(z => z.Equals(put_san));

                        if (exp.Any())
                        {
                            continue;
                        }
                    }

                    PostPutyovkaViewModel mmModel = mm;

                    var pp = KasabaDB.tblPost.Where(z => z.ID == mmModel.IdPost).First();

                    PostPutDetailDataManager.Create(ref mmModel,KasabaDB, pp);

                    await KasabaDB.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult>  CloseNakladnoy(PostupleniyaViewModel model)
        {
            try
            {
                await PostMainDataManager.CloseNakladnoy(model);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdSan"></param>
        /// <param name="NumPut"></param>
        /// <returns></returns>
        public JsonResult PostavitVBron(int IdSan , string NumPut)
        {
            try
            {
                var bron = new BronirovaniyaPutyovki();

                bron.PostavitV_Bron(new BronViewModel()
                {
                    ID_San = IdSan,
                    Num_Put = NumPut
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="putyovkaL"></param>
        /// <returns></returns>
        public async Task<JsonResult> PostavitVBron2(List<long> putyovkaL, DateTime expireDate, string Primechaniye)
        {
            if (putyovkaL == null) return Json("fail", JsonRequestBehavior.AllowGet);

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            foreach (var puId in putyovkaL)
            {
                var model = new PostPutyovkaViewModel
                {
                    UserId = UserId,
                    Departmentbase = ustinfo.DepartmentBase,
                    Id = puId
                };

                var putyovka = await PostPutDetailDataManager.Get2(model);

                if (putyovka.Any())
                {
                    var bron = new BronirovaniyaPutyovki();
                    var putObj = putyovka.First();

                    if (putObj.ID_PolVoz) continue;

                    bron.PostavitV_Bron(new BronViewModel()
                    {
                        ID_San = putObj.IdSanat.Value,
                        Num_Put = putObj.Num_Put,
                        DepartmentBase = ustinfo.DepartmentBase,
                        ExpireDate = expireDate,
                        UserId = UserId,
                        Prem = Primechaniye
                    });

                    //History making
                    var histModel = new PutHistViewModel
                    {
                        event_tp = (int)HistEventType.BronPutyovka,
                        departmentbase = model.Departmentbase,
                        id_sanat = putObj.IdSanat,
                        num_put = putObj.Num_Put,
                        remark = string.Format("Сумма {0} - Дата Заезда {1}", putObj.Symma_Put, putObj.DataSan),
                        userid = model.UserId
                    };

                    PutHistDataManager.Create(histModel);
                }
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdSan"></param>
        /// <param name="NumPut"></param>
        /// <returns></returns>
        public JsonResult SnyatsBron(int IdSan, string NumPut)
        {
            try
            {
                var bron = new BronirovaniyaPutyovki();

                bron.SnyatBron(new BronViewModel()
                {
                    ID_San = IdSan,
                    Num_Put = NumPut
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="putyovkaL"></param>
        /// <returns></returns>
        public async Task<JsonResult> SnyatsBron2(List<long> putyovkaL)
        {
            if (putyovkaL == null) return Json("fail", JsonRequestBehavior.AllowGet);

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            foreach (var puId in putyovkaL)
            {
                var model = new PostPutyovkaViewModel
                {
                    UserId = UserId,
                    Departmentbase = ustinfo.DepartmentBase,
                    Id = puId
                };

                var putyovka = await PostPutDetailDataManager.Get2(model);

                if (putyovka.Any())
                {
                    var bron = new BronirovaniyaPutyovki();
                    var putObj = putyovka.First();

                    if (putObj.ID_PolVoz) continue;

                    bron.SnyatBron(new BronViewModel()
                    {
                        ID_San = putObj.IdSanat.Value,
                        Num_Put = putObj.Num_Put
                    });

                    //History making
                    var histModel = new PutHistViewModel
                    {
                        event_tp = (int)HistEventType.SnyatBronPutyovka,
                        departmentbase = model.Departmentbase,
                        id_sanat = putObj.IdSanat,
                        num_put = putObj.Num_Put,
                        remark = string.Format("Сумма {0} - Дата Заезда {1}", putObj.Symma_Put, putObj.DataSan),
                        userid = model.UserId
                    };

                    PutHistDataManager.Create(histModel);
                }
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="putyovkaL"></param>
        /// <returns></returns>
        public async Task<JsonResult> IzmData(List<long> putyovkaL, DateTime IzmDate)
        {
            if (putyovkaL == null) return Json("fail", JsonRequestBehavior.AllowGet);

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            foreach (var puId in putyovkaL)
            {
                var model = new PostPutyovkaViewModel
                {
                    UserId = UserId,
                    Departmentbase = ustinfo.DepartmentBase,
                    Id = puId,
                    DataSan = IzmDate

                };

                var putyovka = await PostPutDetailDataManager.Get2(model);

                if (putyovka.Any())
                {
                    //var bron = new BronirovaniyaPutyovki();
                    var putObj = putyovka.First();
                    var oldDate = putObj.DataSan;
                    putObj.DataSan = IzmDate;

                    await PostPutDetailDataManager.ModifyIzmData(putObj);

                    //History making
                    var histModel = new PutHistViewModel
                    {
                        event_tp = (int)HistEventType.IzmPutDate,
                        departmentbase = model.Departmentbase,
                        id_sanat = putObj.IdSanat,
                        num_put = putObj.Num_Put,
                        remark = string.Format("Старая дата {0} - Новая дата {1}", oldDate , IzmDate),
                        userid = model.UserId
                    };

                    PutHistDataManager.Create(histModel);
                }
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public JsonResult PechatPrixodPutyovok(int year, int kvartal)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            //Reporting Service

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public JsonResult GetBronList(int year, int kvartal)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var bron = new BronirovaniyaPutyovki();

            var list = bron.GetBronActiveList(ustinfo.DepartmentBase);

            return Json(list, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region RasxodPutyovok

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult RasxodPutyovok()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult RasxodPutyovokFizLiso()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetViplMain(int year, int kvartal, int ID_Ot)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = await ViplMainDataManager.Get(new RealizasiyaViplMainViewModel()
            {
                smfil = year,
                id_kv = kvartal,
                departmentbase = ustinfo.DepartmentBase,
                ID_Ot = ID_Ot
            });

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetViplMainF(int year, int kvartal, int ID_Ot)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = await ViplMainDataManager.GetFizLiso(new RealizasiyaViplMainViewModel()
            {
                smfil = year,
                id_kv = kvartal,
                departmentbase = ustinfo.DepartmentBase,
                ID_Ot = ID_Ot
            });

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdVipl"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetViplDtl(long IdVipl)
        {
            var list = await Vipl_PutDataManager.Get(IdVipl);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> AddViplMain(RealizasiyaViplMainViewModel model)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            model.userid = UserId;
            model.departmentbase = ustinfo.DepartmentBase;

            try
            {
                tblVipl pp = null;

                ViplMainDataManager.Create(ref model, KasabaDB , out pp);

                await KasabaDB.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());

                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> ModifyViplMain(RealizasiyaViplMainViewModel model)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            model.userid = UserId;
            model.departmentbase = ustinfo.DepartmentBase;

            try
            {
                await ViplMainDataManager.Modify(model);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());

                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> RemoveViplMain(RealizasiyaViplMainViewModel model)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            model.userid = UserId;
            model.departmentbase = ustinfo.DepartmentBase;

            try
            {
                await ViplMainDataManager.RemoveWithChild(model);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());

                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetNewPutyovkaList(string NumPut, long? IdVipl)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = await PostPutDetailDataManager.Get(new PostPutyovkaViewModel()
            {
                Departmentbase = ustinfo.DepartmentBase,
                Num_Put = NumPut
            }, IdVipl);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modModel"></param>
        /// <param name="IdViplMain"></param>
        /// <returns></returns>
        public async Task<JsonResult> AddViplPut(List<PostPutyovkaViewModel> modModel, long? IdViplMain)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            try
            {
                if (modModel != null)
                {
                    foreach (var mm in modModel)
                    {
                        if (mm.Checked)
                        {
                            var viplPut = new RealizasiyaViplPutViewModel();

                            viplPut.id_vipl = IdViplMain;
                            viplPut.id_pok = 0; //I do not know for what it is used
                            viplPut.id_sanat = mm.IdSanat;
                            viplPut.num_put = mm.Num_Put;
                            viplPut.num_proc = 100; //May be not 100
                            viplPut.symma = mm.Symma_Put;
                            viplPut.id_polvoz = false;
                            viplPut.data_post = DateTime.Now;
                            viplPut.userid = UserId;
                            viplPut.departmentbase = ustinfo.DepartmentBase;

                            //Enter to Rasxod
                            Vipl_PutDataManager.Create(viplPut, KasabaDB);

                            //Change Status of Putyovka in Prixod
                            PostPutDetailDataManager.Polzov(mm.IdSanat.Value, mm.Num_Put, KasabaDB);

                            await KasabaDB.SaveChangesAsync();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());

                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modModel"></param>
        /// <param name="IdViplMain"></param>
        /// <returns></returns>
        public async Task<JsonResult> AddViplPut2(List<long> modModel, long IdViplMain)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            try
            {
                if (modModel != null)
                {
                    foreach (var putId in modModel)
                    {
                        var prixodPut = await PostPutDetailDataManager.Get2(new PostPutyovkaViewModel() {

                            Id = putId,
                            Departmentbase = ustinfo.DepartmentBase
                        });

                        if (!prixodPut.Any()) continue;

                        var mm = prixodPut.First();

                        //if (mm.Checked)
                        //
                            var viplPut = new RealizasiyaViplPutViewModel();

                            viplPut.id_vipl = IdViplMain;
                            viplPut.id_pok = 0; //I do not know for what it is used
                            viplPut.id_sanat = mm.IdSanat;
                            viplPut.num_put = mm.Num_Put;
                            viplPut.num_proc = 100; //May be not 100
                            viplPut.symma = mm.Symma_Put;
                            viplPut.id_polvoz = false;
                            viplPut.data_post = DateTime.Now;
                            viplPut.userid = UserId;
                            viplPut.departmentbase = ustinfo.DepartmentBase;

                            //Enter to Rasxod
                            Vipl_PutDataManager.Create(viplPut, KasabaDB);

                            //Change Status of Putyovka in Prixod
                            PostPutDetailDataManager.Polzov(mm.IdSanat.Value, mm.Num_Put, KasabaDB);

                            await KasabaDB.SaveChangesAsync();
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());

                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetViplTotalResults(int year, int kvartal, int ID_Ot)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var totalres = await ViplMainDataManager.GetTotals(year, kvartal, ustinfo.DepartmentBase, ID_Ot);

            return Json(totalres, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetViplTotalResultsF(int year, int kvartal, int ID_Ot)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var totalres = await ViplMainDataManager.GetTotalsF(year, kvartal, ustinfo.DepartmentBase, ID_Ot);

            return Json(totalres, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdVipl"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetViplDetailListRes(long IdVipl)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = await Vipl_PutDataManager.Get(IdVipl);

            var res = new TotalResults();
            res.PutyovkaCount = list.Count();
            res.SummaNakladnix = list.Sum(z => z.symma);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdViplMain"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetSchetFaktura(long IdViplMain)
        {
            var schetFaktura = await ViplMainDataManager.GetSchetFakturaData(IdViplMain);

            return Json(schetFaktura, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> CloseViplNakladnoy(RealizasiyaViplMainViewModel model)
        {
            try
            {
                await ViplMainDataManager.CloseNakladnoy(model);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<JsonResult> RemoveViplPutyovkaDetail(List<long> putyovkaL)
        {
            if (putyovkaL == null) return Json("fail", JsonRequestBehavior.AllowGet);

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            try
            {
                foreach (var putyovkaId in putyovkaL)
                {
                    var rasxm = await Vipl_PutDataManager.Get2(putyovkaId);

                    if (!rasxm.Any()) continue;

                    var rmodel = rasxm.First();


                    Vipl_PutDataManager.Remove(new RealizasiyaViplPutViewModel()
                    {
                        id = putyovkaId
                    }, KasabaDB);

                    //Otmen Polzov
                    PostPutDetailDataManager.OtmenPolzov(rmodel.id_sanat.Value, rmodel.num_put, KasabaDB);

                    //History making
                    var histModel = new PutHistViewModel
                    {
                        event_tp = (int)HistEventType.UdalitPutIzRasxod,
                        departmentbase = ustinfo.DepartmentBase,
                        id_sanat = rmodel.id_sanat,
                        num_put = rmodel.num_put,
                        remark = string.Format("Сумма {0} - Санаторий {1}", rmodel.symma, rmodel.sanatName),
                        userid = UserId
                    };

                    PutHistDataManager.CreateL(histModel, KasabaDB);

                    await KasabaDB.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdViplMain"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetOrgPaymentsLastYear(long IdViplMain)
        {
            var paymentsAmount = await ViplMainDataManager.GetOrgPaymentsLastYear(IdViplMain);

            return Json(paymentsAmount, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  Bank-Oplati i Viplati 

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult OplataViplatiZaPutyovki()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetOplataMain(int year, int kvartal, int tipOrg, int Priz)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list =await OplataDataManager.GetFiltered(new OplataViewModel()
            {
                SmFil = year,
                ID_KV = kvartal,
                Departmentbase = ustinfo.DepartmentBase,
                Tip_Org = tipOrg == 1 ? true: false,
                PrizOpl = Priz
            });

            //StatController.Process(ustinfo.DepartmentBase, ustinfo.role_id.Value);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetOplataMainRes(int year, int kvartal, int tipOrg, int Priz)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = await OplataDataManager.GetFiltered(new OplataViewModel()
            {
                SmFil = year,
                ID_KV = kvartal,
                Departmentbase = ustinfo.DepartmentBase,
                Tip_Org = tipOrg == 1 ? true : false,
                PrizOpl = Priz
            });

            var totres = new TotalOplataResults()
            {
                SummaDebit = list.Sum(z=>z.Symma_DT),
                SummaCredit = list.Sum(z=>z.Symma_KT),
                Saldo = list.Sum(z => z.Symma_DT) - list.Sum(z => z.Symma_KT)
            };

            return Json(totres, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdMain"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetOplataDetail(long IdMain)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = await OplataRasxodDataManager.GetDetail(new OplataRasxodiViewModel()
            {
                Departmentbase = ustinfo.DepartmentBase,
                ID_Opl = IdMain
            });

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdMain"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetOplataDetailRes(long IdMain)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = await OplataRasxodDataManager.GetDetail(new OplataRasxodiViewModel()
            {
                Departmentbase = ustinfo.DepartmentBase,
                ID_Opl = IdMain
            });

            var total = new TotalOplataDtlResults()
            {
                RaznostSummi = 0,//list.Sum(z => z.Symma ) * 45 / 100,
                Summa = list.Sum(z => z.Symma)
            };

            return Json(total, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> AddOplataMain(OplataViewModel model)
        {
            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var ustinfo = UserInfo.GetUserInfo(UserId);

                model.Departmentbase = ustinfo.DepartmentBase;
                model.UserId = UserId;

                tblOplata oplMain = null;

                OplataDataManager.Create(ref model,KasabaDB, out oplMain);

                Sprav_Org.ChangeOstatok(TransactionOperation.Creadit, model.ID_Otras.Value, oplMain.Symma_KT, KasabaDB);

                var trans = new ProvodkiViewModel
                {
                    comment = "Оплата и Выплата Общий ",
                    department_base = oplMain.Departmentbase,
                    inoutflg = false,
                    platelrsch = "21",
                    poluchrsch = "04",
                    summa = oplMain.Symma_KT,
                    user_id = oplMain.UserId,
                    trdt = oplMain.Data_Post,
                    trref = "OOV",
                    trrefno = string.Format("{0}|{1}", oplMain.Num_Doc, oplMain.Data_Post.Value.ToShortDateString())
                };

                tbbu_provodki provodka = null;
                ProvodkiDataManager.Create(ref trans, KasabaDB, out provodka);

                oplMain.tbbu_provodki = provodka;

                await KasabaDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> AddOplataMainOplataPutyovok(OplataViewModel model)
        {
            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var ustinfo = UserInfo.GetUserInfo(UserId);

                model.PrizOpl = (int)OplataRasxPriz.OrganizasiyaOplata;
                model.Departmentbase = ustinfo.DepartmentBase;
                model.UserId = UserId;
                model.Symma_DT = 0;

                tblOplata oplMain = null;

                OplataDataManager.Create(ref model,KasabaDB, out oplMain);

                Sprav_Org.ChangeOstatok(TransactionOperation.Creadit, model.ID_Otras.Value, oplMain.Symma_KT, KasabaDB);

                var trans = new ProvodkiViewModel
                {
                    comment = "Оплата путевок ",
                    department_base = oplMain.Departmentbase,
                    inoutflg = false,
                    platelrsch = "21",
                    poluchrsch = "04",
                    summa = oplMain.Symma_KT,
                    user_id = oplMain.UserId,
                    trdt = oplMain.Data_Post,
                    trref = "OPV",
                    trrefno = string.Format("{0}|{1}", oplMain.Num_Doc, oplMain.Data_Post.Value.ToShortDateString())
                };

                tbbu_provodki provodka = null;
                ProvodkiDataManager.Create(ref trans, KasabaDB, out provodka);

                oplMain.tbbu_provodki = provodka;

                await KasabaDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> AddOplataMainOplataKurortniy(OplataViewModel model)
        {
            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var ustinfo = UserInfo.GetUserInfo(UserId);

                model.PrizOpl = (int)OplataRasxPriz.KurortOplata;
                model.Departmentbase = ustinfo.DepartmentBase;
                model.UserId = UserId;
                model.Symma_KT = 0;

                tblOplata oplMain = null;

                OplataDataManager.Create(ref model,KasabaDB, out oplMain);

                Sprav_Org.ChangeOstatok(TransactionOperation.Creadit, model.ID_Otras.Value, oplMain.Symma_KT, KasabaDB);

                var trans = new ProvodkiViewModel
                {
                    comment = "Оплата курортному ",
                    department_base = oplMain.Departmentbase,
                    inoutflg = false,
                    platelrsch = "21",
                    poluchrsch = "04",
                    summa = oplMain.Symma_DT,
                    user_id = oplMain.UserId,
                    trdt = model.Data_Post,
                    trref = "OKV",
                    trrefno = string.Format("{0}|{1}", oplMain.Num_Doc, oplMain.Data_Post.Value.ToShortDateString())
                };

                tbbu_provodki provodka = null;
                ProvodkiDataManager.Create(ref trans, KasabaDB, out provodka);

                oplMain.tbbu_provodki = provodka;

                await KasabaDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> AddOplataMainOplataVozvrat(OplataViewModel model)
        {
            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var ustinfo = UserInfo.GetUserInfo(UserId);

                model.PrizOpl = (int)OplataRasxPriz.VozvratViplata;
                model.Departmentbase = ustinfo.DepartmentBase;
                model.UserId = UserId;
                model.Symma_KT = 0;

                tblOplata oplMain = null;

                OplataDataManager.Create(ref model,KasabaDB, out oplMain);

                Sprav_Org.ChangeOstatok(TransactionOperation.Creadit, model.ID_Otras.Value, oplMain.Symma_KT, KasabaDB);

                var trans = new ProvodkiViewModel
                {
                    comment = "Возврат путевок ",
                    department_base = oplMain.Departmentbase,
                    inoutflg = false,
                    platelrsch = "03",
                    poluchrsch = "04",
                    summa = oplMain.Symma_KT,
                    user_id = oplMain.UserId,
                    trdt = oplMain.Data_Post,
                    trref = "OVV",
                    trrefno = string.Format("{0}|{1}", oplMain.Num_Doc, oplMain.Data_Post.Value.ToShortDateString())
                };

                tbbu_provodki provodka = null;
                ProvodkiDataManager.Create(ref trans, KasabaDB, out provodka);

                await KasabaDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> ModifyOplataMainOplata(OplataViewModel model)
        {
            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var ustinfo = UserInfo.GetUserInfo(UserId);

                model.Departmentbase = ustinfo.DepartmentBase;
                model.UserId = UserId;

                await OplataDataManager.Modify(model);

                //Should be rethinked
                Sprav_Org.ChangeOstatok(TransactionOperation.Creadit, model.ID_Otras.Value, model.Symma_KT, KasabaDB);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> ModifyOplataMainOplataPutyovok(OplataViewModel model)
        {
            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var ustinfo = UserInfo.GetUserInfo(UserId);

                model.PrizOpl = (int)OplataRasxPriz.OrganizasiyaOplata;
                model.Departmentbase = ustinfo.DepartmentBase;
                model.UserId = UserId;
                model.Symma_DT = 0;

                await OplataDataManager.Modify(model);

                Sprav_Org.ChangeOstatok(TransactionOperation.Creadit, model.ID_Otras.Value, model.Symma_KT, KasabaDB);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> ModifyOplataMainOplataKurortniy(OplataViewModel model)
        {
            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var ustinfo = UserInfo.GetUserInfo(UserId);

                model.PrizOpl = (int)OplataRasxPriz.KurortOplata;
                model.Departmentbase = ustinfo.DepartmentBase;
                model.UserId = UserId;
                model.Symma_KT = 0;

                await OplataDataManager.Modify(model);

                Sprav_Org.ChangeOstatok(TransactionOperation.Creadit, model.ID_Otras.Value, model.Symma_KT, KasabaDB);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FileResult DownloadTemplateOp()
        {
            var ArxiveDirectory = Properties.Settings.Default.TemplateDir;

            var targetFile = Path.Combine(ArxiveDirectory, "OplataPutyovok.xlsx");

            if (!System.IO.File.Exists(targetFile)) return null;

            byte[] fileBytes = System.IO.File.ReadAllBytes(targetFile);
            string fileName = "OplataPutyovok.xlsx";
            return File(fileBytes, "application/vnd.ms-excel", fileName);
        }

        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        public async Task<JsonResult> LoadOplataViplata(FormCollection formCollection)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            log.Info("Save");
            try
            {
                if (Request != null)
                {
                    HttpPostedFileBase file = Request.Files["OplataFile"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;
                            var listModel = new List<OplataViplataTemplateViewModel>();

                            var curKvartalc = new CloseKvartal();
                            var curKvartal = curKvartalc.GetCloseKvartalRow(ustinfo.DepartmentBase);

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                try
                                {
                                    var oplModel = new OplataViplataTemplateViewModel();

                                    oplModel.inn = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    oplModel.naimenovanie = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    oplModel.num_scetfak = workSheet.Cells[rowIterator, 3].Value.ToString();

                                    int sanatId = 0;
                                    if (int.TryParse(workSheet.Cells[rowIterator, 4].Value.ToString(), out sanatId))
                                    {
                                        oplModel.sanatoriy_code = sanatId;
                                    }
                                    else
                                    {
                                        oplModel.sanatoriy_code = null;
                                    }

                                    oplModel.nomer_putyovki = workSheet.Cells[rowIterator, 5].Value.ToString();

                                    decimal summa = 0;
                                    if (decimal.TryParse(workSheet.Cells[rowIterator, 6].Value.ToString(), out summa))
                                    {
                                        oplModel.summa_puyovki = summa;
                                    }
                                    else
                                    {
                                        oplModel.summa_puyovki = null;
                                    }

                                    int procent = 0;
                                    if (int.TryParse(workSheet.Cells[rowIterator, 7].Value.ToString(), out procent))
                                    {
                                        oplModel.procent = procent;
                                    }
                                    else
                                    {
                                        oplModel.procent = null;
                                    }


                                    decimal oplacheno = 0;
                                    if (decimal.TryParse(workSheet.Cells[rowIterator, 6].Value.ToString(), out oplacheno))
                                    {
                                        oplModel.oplacheno = oplacheno;
                                    }
                                    else
                                    {
                                        oplModel.oplacheno = null;
                                    }

                                    DateTime trandate;
                                    string DateOfPayment = workSheet.Cells[rowIterator, 7].Value.ToString();

                                    if (DateOfPayment.Length > 10)
                                    {
                                        DateOfPayment = DateOfPayment.Substring(0, 10);
                                    }
                                    if (DateTime.TryParseExact(DateOfPayment, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out trandate))
                                    {
                                        oplModel.data_oplati = trandate;
                                    }
                                    else
                                    {
                                        oplModel.data_oplati = DateTime.Now;

                                    }

                                    listModel.Add(oplModel);


                                    //Sprav_Org.ModifyByTemplate(orgModel, KasabaDB);
                                }
                                catch (Exception ex)
                                {
                                    log.Info(ex);
                                }
                            }

                            OplataImporter.EnterOplata(ustinfo.DepartmentBase, 
                                                        UserId, 
                                                        curKvartal.curkv_year.Value, 
                                                        curKvartal.curkv.Value, 
                                                        listModel, 
                                                        KasabaDB);

                            await KasabaDB.SaveChangesAsync();
                        }
                    }
                }
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
                return Json("error", JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> ModifyOplataMainOplataVozvrat(OplataViewModel model)
        {
            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var ustinfo = UserInfo.GetUserInfo(UserId);

                model.PrizOpl = (int)OplataRasxPriz.VozvratViplata;
                model.Departmentbase = ustinfo.DepartmentBase;
                model.UserId = UserId;
                model.Symma_KT = 0;

                await OplataDataManager.Modify(model);

                Sprav_Org.ChangeOstatok(TransactionOperation.Creadit, model.ID_Otras.Value, model.Symma_KT, KasabaDB);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }


        //Raznoska putyovok
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdOrg"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetNotCoveredPutyovka(long IdOrg)
        {
            var _opl = new Oplata();

            try
            {
                var list = await _opl.GetRaznoskaList(IdOrg);

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdOrg"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetGeneralRemainingByOrg(long IdOrg)
        {
            var _opl = new Oplata();

            try
            {
                var remain = await _opl.GetGeneralRemainingByOrg(IdOrg);

                return Json(remain, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdOrg"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetTotalDebtByOrg(long IdOrg)
        {
            var _opl = new Oplata();

            try
            {
                var remain = await _opl.GetTotalDebtByOrg(IdOrg);

                return Json(remain, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdOpl"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetOplataSaldo(long IdOpl)
        {
            var _opl = new Oplata();

            try
            {
                var remain = await _opl.GetOplataSaldo(IdOpl);

                return Json(remain, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> AddOplataDetail(OplataRasxodiViewModel model)
        {
            if (model == null)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);
            model.Departmentbase = ustinfo.DepartmentBase;
            model.UserId = UserId;

            try
            {
                tblOplata opMain = KasabaDB.tblOplatas.Where(z=>z.ID == model.ID_Opl).First();
                OplataRasxodDataManager.Create(ref model,KasabaDB, opMain);

                await KasabaDB.SaveChangesAsync();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lmodel"></param>
        /// <returns></returns>
        public async  Task<JsonResult> AddOplataPutyovok(List<OplataRasxodiViewModel> lmodel)
        {
            if (lmodel == null) return Json("fail", JsonRequestBehavior.AllowGet);

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            try
            {
                foreach(var model in lmodel)
                {
                    OplataRasxodiViewModel mmodel = model;

                    if (model.PutyovkaChecked)
                    {
                        tblOplata opMain = KasabaDB.tblOplatas.Where(z => z.ID == model.ID_Opl).First();

                        OplataRasxodDataManager.Create(ref mmodel, KasabaDB, opMain);

                        await KasabaDB.SaveChangesAsync();
                    }
                }

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lmodel"></param>
        /// <param name="IdOpl"></param>
        /// <param name="IdPr"></param>
        /// <returns></returns>
        public async Task<JsonResult> AddOplataPutyovok2(List<long> lmodel, long IdOpl, long IdPr)
        {
            if (lmodel == null) return Json("fail", JsonRequestBehavior.AllowGet);

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            try
            {
                long debitOplataModel = 0;

                foreach (var rasxodId in lmodel)
                {
                    var rasxm = OplataRasxodDataManager.Get(new OplataRasxodiViewModel() {
                        ID = rasxodId

                    });

                    if (!rasxm.Any()) continue;

                    var model = rasxm.First();

                    decimal puyovka_amount = 0;

                    if (IdPr == 1)
                    {
                        //it by default 45%
                        puyovka_amount = model.Symma.Value;
                    }
                    else if (IdPr == 2)
                    {
                        puyovka_amount = Decimal.Multiply(model.Symma.Value, 100 / 45);
                    }
                    else if (IdPr == 3)
                    {
                        puyovka_amount = 0;
                    }


                    //Debit side Rasxod putyovka change

                    var putyovka = KasabaDB.tblOplata_Ras.Where(z => z.ID_Sanat == model.ID_Sanat
                                                                && z.Num_Put == model.Num_Put
                                                                && z.Priz == (int)OplataRasxPriz.OrganizasiyaRasxod).FirstOrDefault();

                    if (putyovka != null)
                    {
                        putyovka.Symma = puyovka_amount;
                        putyovka.ID_Pr = IdPr;
                        debitOplataModel = putyovka.ID_Opl.Value;
                    }
                    

                    //////////////////////////////////////

                    var mmodel = new OplataRasxodiViewModel
                    {
                        ID_Opl = IdOpl,
                        ID_Pr = IdPr,
                        ID_Sanat = model.ID_Sanat.Value,
                        Num_Put = model.Num_Put,
                        Symma = puyovka_amount,
                        Departmentbase = ustinfo.DepartmentBase,
                        UserId = UserId,
                        Priz = (int)OplataRasxPriz.OrganizasiyaOplata
                    };

                    tblOplata opMain = KasabaDB.tblOplatas.Where(z => z.ID == mmodel.ID_Opl).First();

                    OplataRasxodDataManager.Create(ref mmodel, KasabaDB, opMain);

                    await KasabaDB.SaveChangesAsync();
                }

                var putyovkasDebit = KasabaDB.tblOplata_Ras.Where(z => z.ID_Opl == debitOplataModel);

                if (putyovkasDebit.Any())
                {
                    var totalNew = putyovkasDebit.Sum(z => z.Symma);

                    var putyovkasDebitMain = KasabaDB.tblOplatas.Where(z => z.ID == debitOplataModel).FirstOrDefault();

                    if (putyovkasDebitMain != null)
                    {
                        putyovkasDebitMain.Symma_DT = totalNew;
                    }
                }

                await KasabaDB.SaveChangesAsync();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lmodel"></param>
        /// <param name="IdOpl"></param>
        /// <param name="IdPr"></param>
        /// <returns></returns>
        public async Task<JsonResult> AddOplataPutyovok3(List<OplataRasxodiViewModel> lmodel, long IdOpl, long IdPr)
        {
            if (lmodel == null) return Json("fail", JsonRequestBehavior.AllowGet);

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            try
            {
                long debitOplataModel = 0;

                foreach (var rasxodId in lmodel)
                {
                    var rasxm = OplataRasxodDataManager.Get(new OplataRasxodiViewModel()
                    {
                        ID = rasxodId.ID

                    });

                    if (!rasxm.Any()) continue;

                    var puty = rasxm.First();

                    var model = rasxodId;

                    decimal puyovka_amount = 0;

                    if (IdPr == 1)
                    {
                        //it by default 45%
                        puyovka_amount = model.Symma.Value;
                    }
                    else if (IdPr == 2)
                    {
                        puyovka_amount = Decimal.Multiply(model.Symma.Value, 100 / 45);
                    }
                    else if (IdPr == 3)
                    {
                        puyovka_amount = 0;
                    }


                    //Debit side Rasxod putyovka change

                    var putyovka = KasabaDB.tblOplata_Ras.Where(z => z.ID_Sanat == model.ID_Sanat
                                                                && z.Num_Put == model.Num_Put
                                                                && z.Priz == (int)OplataRasxPriz.OrganizasiyaRasxod).FirstOrDefault();

                    if (putyovka != null)
                    {
                        if (IdPr != 3)
                        {
                            if ((puyovka_amount != puty.Symma) && (Decimal.Multiply(puyovka_amount, 45 / 100) != puty.Symma))
                            {
                                //do not change
                            }
                            else
                            {
                                putyovka.Symma = puyovka_amount;
                            }
                        }
                        else
                        {
                            putyovka.Symma = puyovka_amount;
                        }
                        
                        putyovka.ID_Pr = IdPr;
                        debitOplataModel = putyovka.ID_Opl.Value;
                    }


                    //////////////////////////////////////

                    var mmodel = new OplataRasxodiViewModel
                    {
                        ID_Opl = IdOpl,
                        ID_Pr = IdPr,
                        ID_Sanat = model.ID_Sanat.Value,
                        Num_Put = model.Num_Put,
                        Symma = puyovka_amount,
                        Departmentbase = ustinfo.DepartmentBase,
                        UserId = UserId,
                        Priz = (int)OplataRasxPriz.OrganizasiyaOplata
                    };

                    tblOplata opMain = KasabaDB.tblOplatas.Where(z => z.ID == mmodel.ID_Opl).First();

                    OplataRasxodDataManager.Create(ref mmodel, KasabaDB, opMain);

                    await KasabaDB.SaveChangesAsync();
                }

                var putyovkasDebit = KasabaDB.tblOplata_Ras.Where(z => z.ID_Opl == debitOplataModel);

                if (putyovkasDebit.Any())
                {
                    var totalNew = putyovkasDebit.Sum(z => z.Symma);

                    var putyovkasDebitMain = KasabaDB.tblOplatas.Where(z => z.ID == debitOplataModel).FirstOrDefault();

                    if (putyovkasDebitMain != null)
                    {
                        putyovkasDebitMain.Symma_DT = totalNew;
                    }
                }

                await KasabaDB.SaveChangesAsync();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lmodel"></param>
        /// <returns></returns>
        public async Task<JsonResult> RemoveOplataRasxod(List<long> lmodel)
        {
            if (lmodel == null) return Json("fail", JsonRequestBehavior.AllowGet);

            try
            {
                foreach (var opsPutId in lmodel)
                {
                    await OplataRasxodDataManager.Remove(new OplataRasxodiViewModel()
                    {
                        ID = opsPutId
                    });
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> RemoveOplataMain(OplataViewModel model)
        {
            try
            {
                    await OplataDataManager.Remove(new OplataViewModel()
                    {
                        ID = model.ID
                    });
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return Json("fail", JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region AktVozvrata

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetVozvratList(int year, int kvartal)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = await VozvratDataManager.GetFilter(new VozvratViewModel()
            {
                departmentbase = ustinfo.DepartmentBase,
                smfil = year,
                id_kv = kvartal
            });

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetVozvratRes(int year, int kvartal)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = await VozvratDataManager.GetFilter(new VozvratViewModel()
            {
                departmentbase = ustinfo.DepartmentBase,
                smfil = year,
                id_kv = kvartal
            });

            var total = new TotalResults()
            {
                PutyovkaCount = list.Count,
                SummaNakladnix = list.Sum(z => z.symmap)
            };

            return Json(total, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AktVozvrataPutyovok()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public async Task<JsonResult> ShowTotalForVozvrat(int year, int kvartal)
        {

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = await VozvratDataManager.GetTotals(year, kvartal, ustinfo.DepartmentBase);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrg"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetGivenPutyovkaList(long idOrg)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = await VozvratDataManager.GetPutyovkas(idOrg, ustinfo.DepartmentBase);

            return Json(list, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetEmptyForAdd()
        {
            var model = new VozvratEnter();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> AddVozvtars(VozvratEnter model)
        {
            if (model == null ) return Json("fail", JsonRequestBehavior.AllowGet);

            try
            {
                var vv = new Vozvrat();

                int UserId = Convert.ToInt32(User.Identity.Name);
                var ustinfo = UserInfo.GetUserInfo(UserId);

                model.departmentbase = ustinfo.DepartmentBase;
                model.userid = UserId;

                //All logic realized
                await vv.EnterVozvrat(model);

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainMdl"></param>
        /// <param name="lrasxm"></param>
        /// <returns></returns>
        public async Task<JsonResult> AddVozvtars2(VozvratEnterTemp mainMdl , List<long> lrasxm)
        {
            if (mainMdl == null) return Json("fail", JsonRequestBehavior.AllowGet);

            if (lrasxm == null) return Json("fail", JsonRequestBehavior.AllowGet);

            try
            {
                var model = new VozvratEnter
                {
                    data_post = mainMdl.data_post,
                    num_doc = mainMdl.num_doc,
                    id_ot = mainMdl.id_ot,
                    dover = mainMdl.dover,
                    symmap = mainMdl.symmap,
                    id_kv = mainMdl.id_kv,
                    smfil = mainMdl.smfil,
                    tip_voz = mainMdl.tip_voz
                };

                foreach (var rasxodId in lrasxm)
                {
                    var rasxm = await Vipl_PutDataManager.Get2(rasxodId);

                    if (!rasxm.Any()) continue;

                    var rmodel = rasxm.First();

                    model.detailPutyovka.Add(rmodel);
                }

                 var vv = new Vozvrat();

                int UserId = Convert.ToInt32(User.Identity.Name);
                var ustinfo = UserInfo.GetUserInfo(UserId);

                model.departmentbase = ustinfo.DepartmentBase;
                model.userid = UserId;

                log.Info("Before EnterVozvrat = =============================");
                //All logic realized
                await vv.EnterVozvrat(model);

                log.Info("After EnterVozvrat = =============================");

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> CancelVozvrat(VozvratViewModel model)
        {
            try
            {
                var vv = new Vozvrat();

                int UserId = Convert.ToInt32(User.Identity.Name);
                var ustinfo = UserInfo.GetUserInfo(UserId);

                model.departmentbase = ustinfo.DepartmentBase;
                model.userid = UserId;

                //All logic realized
                await vv.CancelVozvrat(model);

                //History making
                var histModel = new PutHistViewModel
                {
                    event_tp = (int)HistEventType.UdalitPutIzVozvrat,
                    departmentbase = ustinfo.DepartmentBase,
                    id_sanat = model.id_sanat,
                    num_put = model.num_put,
                    remark = string.Format("Сумма {0} - Санаторий {1}", model.symmap, model.sanatoriyName),
                    userid = UserId
                };

                PutHistDataManager.Create(histModel);

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CurrentAkvVozv"></param>
        /// <param name="IzmDate"></param>
        /// <returns></returns>
        public async Task<JsonResult> IzmDataVozvrat(VozvratViewModel CurrentAkvVozv, DateTime IzmDate)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            CurrentAkvVozv.departmentbase = ustinfo.DepartmentBase;

            try
            {
                await VozvratDataManager.ModifyDate(CurrentAkvVozv, IzmDate);

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Zakritiya Kavartala

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ZakritiyaKvartalaPoPutyovkam()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrg"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetZakritiyaPutyovkas(long idOrg)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            try
            {
                var list = await Vipl_PutDataManager.GetZakritiya(idOrg, ustinfo.DepartmentBase);

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrg"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetZakritiyaPutyovkasRes(long idOrg)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            try
            {
                var list = await Vipl_PutDataManager.GetZakritiya(idOrg, ustinfo.DepartmentBase);

                var total = new TotalResults()
                {
                    PutyovkaCount = list.Count,
                    SummaNakladnix = list.Sum(z => z.symma)
                };

                return Json(total, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetEmptyForAddZakritiye()
        {
            var zakr = new ZakritiyeKvartalaViewModel();

            return Json(zakr, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> ZakritKvartal(ZakritiyeKvartalaViewModel model)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            try
            {
                //model
                var zakrit = new ZakritKvartal4Putyovka();

                await zakrit.Process(model);

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainMdl"></param>
        /// <param name="lrasxm"></param>
        /// <returns></returns>
        public async Task<JsonResult> ZakritKvartal2(ZakritiyeKvartalaViewModelTemp mainMdl
            , List<long> lrasxm)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            try
            {
                //model
                var model = new ZakritiyeKvartalaViewModel();

                model.year = mainMdl.year;
                model.id_kv = mainMdl.id_kv;
                model.IdOrg = mainMdl.IdOrg;

                foreach (var rasxodId in lrasxm)
                {
                    var rasxm = await Vipl_PutDataManager.Get2(rasxodId);

                    if (!rasxm.Any()) continue;

                    var rmodel = rasxm.First();

                    model.putyovkaList.Add(rmodel);
                }

                var zakrit = new ZakritKvartal4Putyovka();

                await zakrit.Process(model);

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Danniye o Vladelsev

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult DanniyeVladelsaxPutyovok()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginDt"></param>
        /// <param name="endDt"></param>
        /// <param name="IdOrg"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetDanniyVladelsaPutyovki(DateTime beginDt, DateTime endDt, long IdOrg)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = await Vipl_PutDataManager.GetDetail4DanniyeVlad(beginDt, endDt, IdOrg, ustinfo.DepartmentBase);

            return Json(list, JsonRequestBehavior.AllowGet);
        }  


        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginDt"></param>
        /// <param name="endDt"></param>
        /// <param name="IdOrg"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetDanniyVladelsaNakladnoy(DateTime beginDt, DateTime endDt, long IdOrg)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = await ViplMainDataManager.GetForEnterDanniyeVlad(beginDt, endDt, IdOrg, ustinfo.DepartmentBase);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginDt"></param>
        /// <param name="endDt"></param>
        /// <param name="IdOrg"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetDanniyTotal(DateTime beginDt, DateTime endDt, long IdOrg)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var total =await Vipl_PutDataManager.GetDetail4DanniyeVladTotal(beginDt, endDt, IdOrg, ustinfo.DepartmentBase);

            return Json(total, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetEmptyForm(long IdPut)
        {
            var checklist = FaceDataManager.Get(new FaceViewModel()
            {
                id_put = IdPut
            });

            if (!checklist.Any())
            {
                var model = new RaznoskaGeneralViewModel();

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var raznoska = new RaznoskaPutyovok();

                var modelr = raznoska.FillById(IdPut);

                return Json(modelr, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> SaveChanges(RaznoskaGeneralViewModel model)
        {
            if (model == null) return Json("fail", JsonRequestBehavior.AllowGet);

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            try
            {
                var raznoska = new RaznoskaPutyovok();

                await raznoska.SaveChanges(model);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdViplPut"></param>
        /// <param name="faceModel"></param>
        /// <param name="fmModel"></param>
        /// <returns></returns>
        public async Task<JsonResult> SaveChanges2(long IdViplPut , FaceViewModel faceModel, Face_MedViewModel fmModel)
        {
            if (faceModel == null) return Json("fail", JsonRequestBehavior.AllowGet);

            if (fmModel == null) return Json("fail", JsonRequestBehavior.AllowGet);

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var model = new RaznoskaGeneralViewModel();

            faceModel.userid = UserId;
            faceModel.departmentbase = ustinfo.DepartmentBase;
            fmModel.userid = UserId;
            fmModel.departmentbase  = ustinfo.DepartmentBase;

            model.viplPutId = IdViplPut;
            model.DepartmentBase = ustinfo.DepartmentBase;
            model.faceMain = faceModel;
            model.faceMedDtl = fmModel;

            try
            {
                var raznoska = new RaznoskaPutyovok();

                await raznoska.SaveChanges(model);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region SverkaDannixPoOtixayushim
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formCollection"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ReadFileSverki(FormCollection formCollection) 
        {
            log.Info("ReadFileSverki");
            try
            {
                if (Request != null)
                {
                    int year = 0;
                    int owner_tp = 0;
                    int sanat_id = 0;
                    string owner_code = "";

                    foreach (var key in formCollection.AllKeys)
                    {
                        if (key.Equals("SelectedYear"))
                        {
                            year = int.Parse(formCollection[key]);
                        }
                        if (key.Equals("SelectedSide"))
                        {
                            owner_tp = int.Parse(formCollection[key]);
                        }
                        if (key.Equals("SelectedSanatory"))
                        {
                            sanat_id = int.Parse(formCollection[key]);
                        }
                        if (key.Equals("SelectedPravsoyuz"))
                        {
                            owner_code = formCollection[key];
                        }
                    }

                    HttpPostedFileBase file = Request.Files["UploadedFile"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        var sverList = new List<SanatDataCompareViewModel>();
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                var sver = new SanatDataCompareViewModel();

                                sver.year = year;
                                sver.owner_tp = owner_tp;
                                sver.sanat_id = sanat_id;
                                sver.owner_code = owner_code;

                                try
                                {
                                    sver.num_put = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    sver.date_san = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    sver.price = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    sver.fio_otdix = workSheet.Cells[rowIterator, 4].Value.ToString();
                                    sver.adress = workSheet.Cells[rowIterator, 5].Value.ToString();
                                    sver.work_place = workSheet.Cells[rowIterator, 6].Value.ToString();
                                    sver.otrasl = workSheet.Cells[rowIterator, 7].Value.ToString();
                                }
                                catch(NullReferenceException ex)
                                {
                                    log.Info(ex.ToString());
                                    continue;
                                }

                                sverList.Add(sver);
                                log.Info("Count = " + sverList.ToList().Count);
                                await SanatDataCompareDataManager.SaveData(sver);
                            }
                        }
                    }
                }
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public JsonResult GetLoadedRows(int year)
        {
            var list = SanatDataCompareDataManager.Get(new SanatDataCompareViewModel() {
                year = year
            }).Take(100);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FileResult DownloadTemplate()
        {
            var ArxiveDirectory = Properties.Settings.Default.TemplateDir;

            var targetFile = Path.Combine(ArxiveDirectory, "shbln01.xlsx");

            if (!System.IO.File.Exists(targetFile)) return null;

            byte[] fileBytes = System.IO.File.ReadAllBytes(targetFile);
            string fileName = "template.xlsx";
            return File(fileBytes, "application/vnd.ms-excel", fileName);
        }


        public FileResult DownloadTemplateManual()
        {
            var ArxiveDirectory = Properties.Settings.Default.TemplateDir;

            var targetFile = Path.Combine(ArxiveDirectory, "Manual", "Manual.chm");

            if (!System.IO.File.Exists(targetFile)) return null;

            byte[] fileBytes = System.IO.File.ReadAllBytes(targetFile);
            string fileName = "Руководство.chm";
            return File(fileBytes, "pplication/vnd.ms-htmlhelp", fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public JsonResult GetDifferByYearFiles(int year, int IdSan, string Obl)
        {
            var sverObj = new SverManager();

            var list = sverObj.MatchingEngineFiles(year, IdSan, Obl);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public JsonResult GetDifferByYearExternal_Vvod(int year)
        {
            var sverObj = new SverManager();

            var list = sverObj.MatchingEngineExternal_Vvod(year);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public JsonResult GetDifferByYearMix(int year)
        {
            var sverObj = new SverManager();

            var list = sverObj.MatchingEngineMix(year);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region StartWork

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        public async Task<JsonResult> PutyovkaOstatok(FormCollection formCollection)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            log.Info("Save");
            try
            {
                if (Request != null)
                {
                    HttpPostedFileBase file = Request.Files["PutyovkaFile"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;
                            var listModel = new List<PutyovkiOstatokViewModel>();

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                try
                                {
                                    var putyovki = new PutyovkiOstatokViewModel();

                                    int SanatCode = 0;

                                    if (int.TryParse(workSheet.Cells[rowIterator, 1].Value.ToString(), out SanatCode))
                                    {
                                        putyovki.SanatoryCode = SanatCode;
                                    }
                                    else
                                    {
                                        putyovki.SanatoryCode = 0;
                                    }

                                    putyovki.NumPut         = workSheet.Cells[rowIterator, 2].Value.ToString();

                                    DateTime trandate;

                                    string DataSan = workSheet.Cells[rowIterator, 3].Value.ToString();

                                    if (DataSan.Length > 10)
                                    {
                                        DataSan = DataSan.Substring(0, 10);
                                    }

                                    if (DateTime.TryParseExact(DataSan, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out trandate))
                                    {
                                        putyovki.DataSan = trandate;
                                    }
                                    else
                                    {
                                        putyovki.DataSan = DateTime.Now;

                                    }

                                    decimal amount = 0;

                                    if (decimal.TryParse(workSheet.Cells[rowIterator, 4].Value.ToString(), out amount))
                                    {
                                        putyovki.Summa = amount;
                                    }
                                    else
                                    {
                                        putyovki.Summa = 0;

                                    }

                                    listModel.Add(putyovki);
                                }
                                catch (Exception ex)
                                {
                                    log.Info(ex);
                                }
                            }

                            PutyovkaOstatokDataManager.Enter(ustinfo.DepartmentBase, UserId, listModel, KasabaDB);

                            await KasabaDB.SaveChangesAsync();
                        }
                    }
                }
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
                return Json("error", JsonRequestBehavior.AllowGet);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        public async Task<JsonResult> PutyovkaOstatokKengash(FormCollection formCollection)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            log.Info("Save");
            try
            {
                if (Request != null)
                {
                    HttpPostedFileBase file = Request.Files["PutyovkaKengashFile"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;
                            var listModel = new List<PutyovkiOstatokViewModel>();

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                try
                                {
                                    var putyovki = new PutyovkiOstatokViewModel();

                                    int SanatCode = 0;

                                    if (int.TryParse(workSheet.Cells[rowIterator, 1].Value.ToString(), out SanatCode))
                                    {
                                        putyovki.SanatoryCode = SanatCode;
                                    }
                                    else
                                    {
                                        putyovki.SanatoryCode = 0;
                                    }

                                    putyovki.NumPut = workSheet.Cells[rowIterator, 2].Value.ToString();

                                    DateTime trandate;

                                    string DataSan = workSheet.Cells[rowIterator, 3].Value.ToString();

                                    if (DataSan.Length > 10)
                                    {
                                        DataSan = DataSan.Substring(0, 10);
                                    }

                                    if (DateTime.TryParseExact(DataSan, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out trandate))
                                    {
                                        putyovki.DataSan = trandate;
                                    }
                                    else
                                    {
                                        putyovki.DataSan = DateTime.Now;

                                    }

                                    decimal amount = 0;

                                    if (decimal.TryParse(workSheet.Cells[rowIterator, 4].Value.ToString(), out amount))
                                    {
                                        putyovki.Summa = amount;
                                    }
                                    else
                                    {
                                        putyovki.Summa = 0;

                                    }

                                    putyovki.Inn = workSheet.Cells[rowIterator, 5].Value.ToString();

                                    listModel.Add(putyovki);
                                }
                                catch (Exception ex)
                                {
                                    log.Info(ex);
                                }
                            }

                            PutyovkaOstatokDataManager.EnterKengash(ustinfo.DepartmentBase, UserId, listModel, KasabaDB);

                            await KasabaDB.SaveChangesAsync();
                        }
                    }
                }
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
                return Json("error", JsonRequestBehavior.AllowGet);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        public async Task<JsonResult> BalanceSaldo(FormCollection formCollection)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            log.Info("Save");
            try
            {
                if (Request != null)
                {
                    HttpPostedFileBase file = Request.Files["BalanceFile"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                try
                                {
                                    var balance = new BalanceSaldoViewModel();

                                    balance.department_base = ustinfo.DepartmentBase;
                                    balance.user_id = UserId;

                                    int cur_year = 0;

                                    if (int.TryParse(workSheet.Cells[rowIterator, 1].Value.ToString(), out cur_year))
                                    {
                                        balance.cur_year = cur_year;
                                    }
                                    else
                                    {
                                        balance.cur_year = 2017;
                                    }

                                    int cur_kv = 0;

                                    if (int.TryParse(workSheet.Cells[rowIterator, 2].Value.ToString(), out cur_kv))
                                    {
                                        balance.cur_kv = cur_kv;
                                    }
                                    else
                                    {
                                        balance.cur_kv = 4;
                                    }

                                    balance.account_code = workSheet.Cells[rowIterator, 3].Value.ToString();


                                    decimal debit = 0;

                                    if (decimal.TryParse(workSheet.Cells[rowIterator, 4].Value.ToString(), out debit))
                                    {
                                        balance.debit_bal = debit;
                                    }
                                    else
                                    {
                                        balance.debit_bal = 0;

                                    }


                                    decimal credit = 0;

                                    if (decimal.TryParse(workSheet.Cells[rowIterator, 5].Value.ToString(), out credit))
                                    {
                                        balance.credit_bal = credit;
                                    }
                                    else
                                    {
                                        balance.credit_bal = 0;

                                    }

                                    BalanceSaldoDataManager.Enter(balance, KasabaDB);
                                }
                                catch (Exception ex)
                                {
                                    log.Info(ex);
                                }
                            }

                            await KasabaDB.SaveChangesAsync();
                        }
                    }
                }
                
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
                return Json("error", JsonRequestBehavior.AllowGet);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        public async Task<JsonResult> OrganizationDebitCredit(FormCollection formCollection)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            log.Info("Save");
            try
            {
                if (Request != null)
                {
                    HttpPostedFileBase file = Request.Files["DebitCreditFile"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                try
                                {
                                    var org = new DebitorVaCreditorViewModel();

                                    org.departmentbase = ustinfo.DepartmentBase;
                                    org.inn = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    org.NameOrg = workSheet.Cells[rowIterator, 2].Value.ToString();

                                    decimal debit_bal = 0;

                                    if (decimal.TryParse(workSheet.Cells[rowIterator, 3].Value.ToString(), out debit_bal))
                                    {
                                        org.debit_bal = debit_bal;
                                    }
                                    else
                                    {
                                        org.debit_bal = 0;

                                    }

                                    decimal credit_bal = 0;

                                    if (decimal.TryParse(workSheet.Cells[rowIterator, 4].Value.ToString(), out credit_bal))
                                    {
                                        org.credit_bal = credit_bal;
                                    }
                                    else
                                    {
                                        org.credit_bal = 0;

                                    }

                                    DebitorVaCreditorDataManager.Enter(org, KasabaDB);


                                }
                                catch (Exception ex)
                                {
                                    log.Info(ex);
                                }
                            }

                            await KasabaDB.SaveChangesAsync();
                        }
                    }
                }
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
                return Json("error", JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult StartProcess()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FileResult DownloadPutyovkiTemplate()
        {
            var ArxiveDirectory = Properties.Settings.Default.TemplateDir;

            var targetFile = Path.Combine(ArxiveDirectory, "PutyovkiOstatok.xlsx");

            if (!System.IO.File.Exists(targetFile)) return null;

            byte[] fileBytes = System.IO.File.ReadAllBytes(targetFile);
            string fileName = "PutyovkiOstatok.xlsx";
            return File(fileBytes, "application/vnd.ms-excel", fileName);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FileResult DownloadPutyovkiKengashTemplate()
        {
            var ArxiveDirectory = Properties.Settings.Default.TemplateDir;

            var targetFile = Path.Combine(ArxiveDirectory, "PutyovkiOstatokKengash.xlsx");

            if (!System.IO.File.Exists(targetFile)) return null;

            byte[] fileBytes = System.IO.File.ReadAllBytes(targetFile);
            string fileName = "PutyovkiOstatokKengash.xlsx";
            return File(fileBytes, "application/vnd.ms-excel", fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FileResult DownloadBalanceTemplate()
        {
            var ArxiveDirectory = Properties.Settings.Default.TemplateDir;

            var targetFile = Path.Combine(ArxiveDirectory, "BalanceOstatok.xlsx");

            if (!System.IO.File.Exists(targetFile)) return null;

            byte[] fileBytes = System.IO.File.ReadAllBytes(targetFile);
            string fileName = "BalanceOstatok.xlsx";
            return File(fileBytes, "application/vnd.ms-excel", fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FileResult DownloadDebitorTemplate()
        {
            var ArxiveDirectory = Properties.Settings.Default.TemplateDir;

            var targetFile = Path.Combine(ArxiveDirectory, "DebitorkaOstatok.xlsx");

            if (!System.IO.File.Exists(targetFile)) return null;

            byte[] fileBytes = System.IO.File.ReadAllBytes(targetFile);
            string fileName = "DebitorkaOstatok.xlsx";
            return File(fileBytes, "application/vnd.ms-excel", fileName);
        }

        #endregion

        #region PutyovkaHistory

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdSanat"></param>
        /// <param name="Num_Put"></param>
        /// <returns></returns>
        public JsonResult GetPutyovkaHistory(int IdSanat, string Num_Put)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var list = PutHistDataManager.Get(new PutHistViewModel()
            {
                departmentbase = ustinfo.DepartmentBase,
                id_sanat = IdSanat,
                num_put = Num_Put
            });

            return Json(list.ToList().OrderByDescending(z=>z.timestamp_t).Take(30), JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}