using KasabaKiat.BusinessLogic;
using KasabaKiat.Helpers;
using KasabaKiat.ViewModel.Putyovki;
using KasabaKIAT.BusinessLogic.Putyovki.RealizasiyaPutyovok.RealizasitaPoTz;
using KasabaKIAT.BusinessLogic.Putyovki.VozvratPutyovki.VozvratPoTz;
using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Putyovki;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

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
        [OutputCache(Duration = 3000, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
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
        [OutputCache(Duration = 3000, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public ActionResult ProsmotrRealzPutevok()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 3000, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
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
        [OutputCache(Duration = 3000, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
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
        [OutputCache(Duration = 3000, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
        public ActionResult SverkaDannix()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        [OutputCache(Duration = 3000, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "inn", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
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
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
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
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
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
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "sc_id",VaryByCustom = "userId")]
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
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "sc_id", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 60, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "inn;name", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "inn;name", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 300, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "inn", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 3000, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
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
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
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
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "id", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByCustom = "userId")]
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
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "id", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "id", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "sc_id", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "sc_id", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "sc_id", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "sc_id", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "inn", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "inn", VaryByCustom = "userId")]
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
        [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "inn", VaryByCustom = "userId")]
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
    }
}