using KasabaKiat.BusinessLogic;
using KasabaKiat.Helpers;
using KasabaKiat.ViewModel.Putyovki;
using KasabaKIAT.BusinessLogic.Putyovki.RealizasiyaPutyovok.RealizasitaPoTz;
using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KasabaKIAT.Controllers
{
    //[Authorize(Roles = "ADMIN")]
    [Authorize]
    public class PrixodPutevokController : Controller
    {
        KasabaKIATEntities KasabaDB = new KasabaKIATEntities();
        BronirovaniyaPutyovki BP = new BronirovaniyaPutyovki();
        RealizasiyaPoTzCl OR = new RealizasiyaPoTzCl();
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
        [Authorize(Roles = "COUNCIL_REGIONAL_REGION,ADMIN")]
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
        [Authorize(Roles = "COUNCIL_REGIONAL_REGION,ADMIN")]
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
        public JsonResult GetZList(int inn)
        {
            var data = KasabaDB.sp_rt_ZayavkiList(inn);
            var list = from d in data
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
                           d6=d.payment_persent
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
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
            //List<SanAndPutViewModel> br = new List<SanAndPutViewModel>();
            //foreach (var bb in b)
            //{
            //    var mm = new SanAndPutViewModel();

            //    mm.SanId = bb.SanId;
            //    mm.NumPut = bb.NumPut;
            //    br.Add(mm);
            //}
            int UserId = Convert.ToInt32(User.Identity.Name);
            var uu = UserInfo.GetUserInfo(UserId);

            return View(OR.GetOformRealizList(b, inn, uu.DepartmentBase, uu.userid));
        }
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
        public JsonResult Getegrpo(string inn,string name)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);
            string str = ustinfo.DepartmentBase.Substring(0, 2) + ustinfo.DepartmentBase.Substring(3, 2);
            var list = (from a in KasabaDB.t_egrpo
                        where a.soato_id.Contains(str)&&(a.inn.Contains(inn)||a.name_full.Trim().Contains(name.Trim())||a.name_short.Trim().Contains(name.Trim()))
                        select new
                        {
                            d0 = a.id,
                            d1 = a.address,
                            d2 = a.name_full,
                            d3 = a.name_short,
                            d4 = a.inn,
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
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
        public JsonResult RP2(int id)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);
            var data = KasabaDB.sp_get_RealizasiyaPutyovokDetail(ustinfo.DepartmentBase,id);
            var list = from d in data
                       select new
                       {
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
        public String AddPrixodPutevok(tbpt_putyovki PP)
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