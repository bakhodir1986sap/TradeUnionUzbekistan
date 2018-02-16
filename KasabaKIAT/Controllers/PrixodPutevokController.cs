using KasabaKiat.Helpers;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KasabaKIAT.Controllers
{
    [Authorize]
    public class PrixodPutevokController : Controller
    {
        KasabaKIATEntities KasabaDB = new KasabaKIATEntities();
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
                           d8=d.URGENT
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
                           d6 = d.URGENT_FLG
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPrixodPutevok_S(int sc_id)
        {
            var list = (from a in KasabaDB.tbpt_putyovki
                        where a.STATUS == "1" && a.SANATORIUM_ID==sc_id
                        //join u in KasabaDB.users on a.USER_ID equals u.id
                        join sc in KasabaDB.t_sanatorium on a.SANATORIUM_ID equals sc.id
                        //join d in KasabaDB.t_departmentbase on a.DEPARTMENT_CODE equals d.code
                        select new
                        {
                            ID = a.REC_ID,
                            SB = a.SERIAL_BEGIN,
                            SE = a.SERIAL_END,
                            SC = sc.sanatorium_name,
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
                        join sc in KasabaDB.t_sanatorium on a.SANATORIUM_ID equals sc.id
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
                        join sc in KasabaDB.t_sanatorium on a.SANATORIUM_ID equals sc.id
                        join d in KasabaDB.t_departmentbase on a.DEPARTMENT_CODE equals d.code
                        where d.inn==inn
                        select new
                        {
                            ID = a.REC_ID,
                            SB = a.SERIAL_BEGIN,
                            SE = a.SERIAL_END,
                            SC = sc.sanatorium_name,
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