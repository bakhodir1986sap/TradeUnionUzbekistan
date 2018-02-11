using KasabaKIAT.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace KasabaKIAT.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ILog log = log4net.LogManager.GetLogger(typeof(HomeController));

        KasabaKIATEntities KasabaDB = new KasabaKIATEntities();
        public ActionResult Index()
        {
            int UserId = 0;

            try
            {
                UserId = Convert.ToInt32(User.Identity.Name);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            if (UserId == 0)
            {
                return RedirectToAction("SignOut", "Login");
            }
            else
            {
                return View();
            }
        }

        public JsonResult Getdictionary()
        {
            var camp = from dict in KasabaDB.tbcs_dictionary
                       join dictType in KasabaDB.tbcs_dictionarytype on
                       dict.DICTIONARY_TYPE_ID equals dictType.REC_ID
                       where dictType.CODE == "placetype"
                       select new
                       {
                           REC_ID = dict.REC_ID,
                           NAME_UZ = dict.NAME_UZ
                       };

            return Json(camp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Getdictionary_RZ()
        {
            var camp = from dict in KasabaDB.tbcs_dictionary
                       join dictType in KasabaDB.tbcs_dictionarytype on
                       dict.DICTIONARY_TYPE_ID equals dictType.REC_ID
                       where dictType.CODE == "rassmot_status"
                       select new
                       {
                           ID = dict.CODE,
                           NAME_UZ = dict.NAME_UZ
                       };

            return Json(camp, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RassmotreniyeZayavok()
        {
            return View();
        }
        public ActionResult PodachaZayavok()
        {
           
            return View();
        }
        public JsonResult GetSanatory()
        {
            var S = (from s in KasabaDB.t_sanatorium
                     select new
                     {
                         id = s.id,
                         value = s.sanatorium_name
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
        public JsonResult GetPosition()
        {
            var P = (from s in KasabaDB.tbpt_zayavki
                     select new
                     {
                         id = s.REC_ID,
                         value = s.POSITION
                     });
            return Json(P.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllPRZ_S()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var list = (from e in KasabaDB.tbpt_zayavki
                        where e.USER_ID == UserId
                        where e.STATUS == "1"
                        join d in KasabaDB.t_sanatorium on e.SANATORY_ID equals d.id
                        //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = d.sanatorium_name,//a.camps_name,
                            SC_id = d.id,
                            P = e.POSITION,
                            FN = e.FULL_NAME,
                            S = e.STATUS,
                            INN = c.inn,
                            O = c.name_uz,
                            Q = e.QUANTITY,
                            TS = e.TIMESTAMP,
                            MOE = e.MEDICAL_ORDER_EXISTS,
                            MO = e.MEDICAL_ORDER
                        }).ToList();
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllPRZ_C()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var list = (from e in KasabaDB.tbpt_zayavki
                        where e.USER_ID == UserId
                        where e.STATUS == "1"
                        //join d in KasabaDB.t_sanatorium on e.SANATORY_ID equals d.id
                        join a in KasabaDB.t_camp on e.CAMP_ID equals a.id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = a.camps_name,
                            SC_id = a.id,
                            P = e.POSITION,
                            FN = e.FULL_NAME,
                            S = e.STATUS,
                            INN = c.inn,
                            O = c.name_uz,
                            Q = e.QUANTITY,
                            TS = e.TIMESTAMP,
                            MOE = e.MEDICAL_ORDER_EXISTS,
                            MO = e.MEDICAL_ORDER
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Load_PRZ_S(int id)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var list = (from e in KasabaDB.tbpt_zayavki
                        where e.USER_ID == UserId
                        where e.STATUS == "1"
                        join d in KasabaDB.t_sanatorium on e.SANATORY_ID equals d.id
                        where e.SANATORY_ID == id
                        //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = d.sanatorium_name,//a.camps_name,
                            SC_id = d.id,
                            P = e.POSITION,
                            FN = e.FULL_NAME,
                            S = e.STATUS,
                            INN = c.inn,
                            O = c.name_uz,
                            Q = e.QUANTITY,
                            TS = e.TIMESTAMP,
                            MOE = e.MEDICAL_ORDER_EXISTS,
                            MO = e.MEDICAL_ORDER
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Load_PRZ_C(int id)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var list = (from e in KasabaDB.tbpt_zayavki
                        where e.USER_ID == UserId
                        where e.STATUS == "1"
                        //join d in KasabaDB.t_sanatorium on e.SANATORY_ID equals d.id
                        join a in KasabaDB.t_camp on e.CAMP_ID equals a.id
                        where e.CAMP_ID == id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = a.camps_name,
                            SC_id = a.id,
                            P = e.POSITION,
                            FN = e.FULL_NAME,
                            S = e.STATUS,
                            INN = c.inn,
                            O = c.name_uz,
                            Q = e.QUANTITY,
                            TS = e.TIMESTAMP,
                            MOE = e.MEDICAL_ORDER_EXISTS,
                            MO = e.MEDICAL_ORDER
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Load_PRZ_D0_S(DateTime date)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var list = (from e in KasabaDB.tbpt_zayavki
                        where e.USER_ID == UserId
                        where e.TIMESTAMP == date
                        where e.STATUS == "1"
                        join d in KasabaDB.t_sanatorium on e.SANATORY_ID equals d.id
                        //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = d.sanatorium_name,//a.camps_name,
                            SC_id = d.id,
                            P = e.POSITION,
                            FN = e.FULL_NAME,
                            S = e.STATUS,
                            INN = c.inn,
                            O = c.name_uz,
                            Q = e.QUANTITY,
                            TS = e.TIMESTAMP,
                            MOE = e.MEDICAL_ORDER_EXISTS,
                            MO = e.MEDICAL_ORDER
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Load_PRZ_D0_C(DateTime date)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var list = (from e in KasabaDB.tbpt_zayavki
                        where e.USER_ID == UserId
                        where e.TIMESTAMP == date
                        where e.STATUS == "1"
                        join d in KasabaDB.t_camp on e.CAMP_ID equals d.id
                        //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = d.camps_name,//a.camps_name,
                            SC_id = d.id,
                            P = e.POSITION,
                            FN = e.FULL_NAME,
                            S = e.STATUS,
                            INN = c.inn,
                            O = c.name_uz,
                            Q = e.QUANTITY,
                            TS = e.TIMESTAMP,
                            MOE = e.MEDICAL_ORDER_EXISTS,
                            MO = e.MEDICAL_ORDER
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Load_PRZ_D1_S(DateTime date)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var list = (from e in KasabaDB.tbpt_zayavki
                        where e.USER_ID == UserId
                        where e.ZAEZD_BEGIN == date
                        where e.STATUS == "1"
                        join d in KasabaDB.t_sanatorium on e.SANATORY_ID equals d.id
                        //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = d.sanatorium_name,//a.camps_name,
                            SC_id = d.id,
                            P = e.POSITION,
                            FN = e.FULL_NAME,
                            S = e.STATUS,
                            INN = c.inn,
                            O = c.name_uz,
                            Q = e.QUANTITY,
                            TS = e.TIMESTAMP,
                            MOE = e.MEDICAL_ORDER_EXISTS,
                            MO = e.MEDICAL_ORDER
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Load_PRZ_D1_C(DateTime date)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var list = (from e in KasabaDB.tbpt_zayavki
                        where e.USER_ID == UserId
                        where e.ZAEZD_BEGIN == date
                        where e.STATUS == "1"
                        join d in KasabaDB.t_camp on e.CAMP_ID equals d.id
                        //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = d.camps_name,//a.camps_name,
                            SC_id = d.id,
                            P = e.POSITION,
                            FN = e.FULL_NAME,
                            S = e.STATUS,
                            INN = c.inn,
                            O = c.name_uz,
                            Q = e.QUANTITY,
                            TS = e.TIMESTAMP,
                            MOE = e.MEDICAL_ORDER_EXISTS,
                            MO = e.MEDICAL_ORDER
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Load_PRZ_P_S(int id)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var list = (from e in KasabaDB.tbpt_zayavki
                        where e.USER_ID == UserId
                        where e.REC_ID == id
                        where e.STATUS == "1"
                        join d in KasabaDB.t_sanatorium on e.SANATORY_ID equals d.id
                        //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = d.sanatorium_name,//a.camps_name,
                            SC_id = d.id,
                            P = e.POSITION,
                            FN = e.FULL_NAME,
                            S = e.STATUS,
                            INN = c.inn,
                            O = c.name_uz,
                            Q = e.QUANTITY,
                            TS = e.TIMESTAMP,
                            MOE = e.MEDICAL_ORDER_EXISTS,
                            MO = e.MEDICAL_ORDER
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Load_PRZ_P_C(int id)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var list = (from e in KasabaDB.tbpt_zayavki
                        where e.USER_ID == UserId
                        where e.REC_ID == id
                        where e.STATUS == "1"
                        join d in KasabaDB.t_camp on e.CAMP_ID equals d.id
                        //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = d.camps_name,//a.camps_name,
                            SC_id = d.id,
                            P = e.POSITION,
                            FN = e.FULL_NAME,
                            S = e.STATUS,
                            INN = c.inn,
                            O = c.name_uz,
                            Q = e.QUANTITY,
                            TS = e.TIMESTAMP,
                            MOE = e.MEDICAL_ORDER_EXISTS,
                            MO = e.MEDICAL_ORDER
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllPodachaZayavok()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var list = (from e in KasabaDB.tbpt_zayavki
                        where e.USER_ID == UserId
                        where e.STATUS == "0"
                        join d in KasabaDB.t_sanatorium on e.SANATORY_ID equals d.id
                        //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        where e.SANATORY_ID != null || e.CAMP_ID != null
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = (e.CAMP_ID == null) ? d.sanatorium_name : "",//a.camps_name,
                            SC_id = d.id,
                            P = e.POSITION,
                            FN = e.FULL_NAME,
                            S = e.STATUS,
                            INN = c.inn,
                            O = c.name_uz,
                            Q = e.QUANTITY,
                            TS = e.TIMESTAMP,
                            MOE = e.MEDICAL_ORDER_EXISTS,
                            MO = e.MEDICAL_ORDER
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllPZ_S(int sc_id)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var list = (from e in KasabaDB.tbpt_zayavki
                        where e.USER_ID == UserId&& e.STATUS == "0"&&e.SANATORY_ID==sc_id
                        join d in KasabaDB.t_sanatorium on e.SANATORY_ID equals d.id
                        //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        where e.SANATORY_ID != null || e.CAMP_ID != null
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = (e.CAMP_ID == null) ? d.sanatorium_name : "",//a.camps_name,
                            SC_id = d.id,
                            P = e.POSITION,
                            FN = e.FULL_NAME,
                            S = e.STATUS,
                            INN = c.inn,
                            O = c.name_uz,
                            Q = e.QUANTITY,
                            TS = e.TIMESTAMP,
                            MOE = e.MEDICAL_ORDER_EXISTS,
                            MO = e.MEDICAL_ORDER
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllPZ_C(int sc_id)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var list = (from e in KasabaDB.tbpt_zayavki
                        where e.USER_ID == UserId&& e.STATUS == "0"&&e.CAMP_ID==sc_id
                        //join d in KasabaDB.t_sanatorium on e.SANATORY_ID equals d.id
                        join a in KasabaDB.t_camp on e.CAMP_ID equals a.id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        where e.SANATORY_ID != null || e.CAMP_ID != null
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = (e.SANATORY_ID == null) ? a.camps_name : "",//d.sanatorium_name ,
                            SC_id = a.id,
                            P = e.POSITION,
                            FN = e.FULL_NAME,
                            S = e.STATUS,
                            INN = c.inn,
                            O = c.name_uz,
                            Q = e.QUANTITY,
                            TS = e.TIMESTAMP,
                            MOE = e.MEDICAL_ORDER_EXISTS,
                            MO = e.MEDICAL_ORDER
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllPodachaZayavok_S_LoadINN(string inn, int zb)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            //DateTime b = new DateTime(, 'dd.MM.yyyy');
            var list = (from e in KasabaDB.tbpt_zayavki
                        //where e.USER_ID == UserId
                        where e.STATUS == "1"
                        join d in KasabaDB.t_sanatorium on e.SANATORY_ID equals d.id
                        //join  a in KasabaDB.t_camp on  e.CAMP_ID equals a.id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        where c.inn == inn && e.ZAEZD_BEGIN.Value.Year == zb && e.ZAEZD_BEGIN.Value.Day > 0 && e.ZAEZD_BEGIN.Value.Month > 0
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = (e.CAMP_ID == null) ? d.sanatorium_name : "",//a.camps_name,
                            SC_id = d.id,
                            P = e.POSITION,
                            FN = e.FULL_NAME,
                            S = e.STATUS,
                            INN = c.inn,
                            O = c.name_uz,
                            Q = e.QUANTITY,
                            TS = e.TIMESTAMP,
                            MOE = e.MEDICAL_ORDER_EXISTS,
                            MO = e.MEDICAL_ORDER
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllPodachaZayavok_C_LoadINN(string inn, int zb)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var list = (from e in KasabaDB.tbpt_zayavki
                            //where e.USER_ID == UserId
                        where e.STATUS == "1"
                        //join d in KasabaDB.t_sanatorium on e.SANATORY_ID equals d.id
                        join a in KasabaDB.t_camp on e.CAMP_ID equals a.id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        where c.inn == inn && e.ZAEZD_BEGIN.Value.Year == zb && e.ZAEZD_BEGIN.Value.Day > 0 && e.ZAEZD_BEGIN.Value.Month > 0
                        select new
                        {
                            ID = e.REC_ID,
                            ZB = e.ZAEZD_BEGIN,
                            ZE = e.ZAEZD_END,
                            SC = (e.SANATORY_ID == null) ? a.camps_name : "",//d.sanatorium_name,
                            SC_id = a.id,
                            P = e.POSITION,
                            FN = e.FULL_NAME,
                            S = e.STATUS,
                            INN = c.inn,
                            O = c.name_uz,
                            Q = e.QUANTITY,
                            TS = e.TIMESTAMP,
                            MOE = e.MEDICAL_ORDER_EXISTS,
                            MO = e.MEDICAL_ORDER
                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GroupRZ_S(string inn, int zb)
        {
            var list = (from e in KasabaDB.tbpt_zayavki
                        where e.STATUS == "1"
                        join d in KasabaDB.t_sanatorium on e.SANATORY_ID equals d.id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        where c.inn == inn && e.ZAEZD_BEGIN.Value.Year == zb
                        select new
                        {
                            ZB = e.ZAEZD_BEGIN,
                            SC = d.sanatorium_name,
                            SC_id = d.id,
                            Q = e.QUANTITY,
                            OQ = e.ORDER_QUANTITY
                        });
            var Groups = from p in list
                         group p by new { p.ZB.Value.Month, p.SC, p.SC_id } into g
                         select new { ZB_g = g.Key.Month, SC_g = g.Key.SC, SC_g_id = g.Key.SC_id, OQ = g.Sum(i => i.OQ), Count = g.Count(), Q_Sum = g.Sum(i => i.Q) };
            return Json(Groups.ToList(), JsonRequestBehavior.AllowGet);
        }
        public string Add_RZ_S_OQ(string inn, int zb, int zb_g, int SC_id, int oq)
        {
            try
            {
                int i = 0;
                var list = from a in KasabaDB.tbpt_zayavki
                           orderby a.ZAEZD_BEGIN
                           //join d in KasabaDB.t_sanatorium on a.SANATORY_ID equals d.id
                           join c in KasabaDB.t_departmentbase on a.DEPARTMENT_CODE equals c.code
                           where a.STATUS == "1" && c.inn == inn && a.ZAEZD_BEGIN.Value.Year == zb && a.ZAEZD_BEGIN.Value.Month == zb_g && a.SANATORY_ID == SC_id
                           select a.REC_ID;
                foreach (var l in list)
                {
                    tbpt_zayavki tz = KasabaDB.tbpt_zayavki.FirstOrDefault(o => o.REC_ID == l);
                    i++;
                    if (i == oq)
                    {
                        tz.ORDER_QUANTITY = 1;
                        TryUpdateModel(tz);
                        break;
                    }
                    else
                    {
                        tz.ORDER_QUANTITY = 1;
                        TryUpdateModel(tz);
                    }
                }
                KasabaDB.SaveChanges();
                return "ok";
            }
            catch
            {
                return "error";
            }
        }
        public string Add_RZ_C_OQ(string inn, int zb, int zb_g, int SC_id, int oq)
        {
            try
            {
                int i = 0;
                var list = from a in KasabaDB.tbpt_zayavki
                           orderby a.ZAEZD_BEGIN
                           //join d in KasabaDB.t_camp on a.CAMP_ID equals d.id
                           join c in KasabaDB.t_departmentbase on a.DEPARTMENT_CODE equals c.code
                           where a.STATUS == "1" && c.inn == inn && a.ZAEZD_BEGIN.Value.Year == zb && a.ZAEZD_BEGIN.Value.Month == zb_g && a.CAMP_ID == SC_id
                           select a.REC_ID;
                foreach (var l in list)
                {
                    tbpt_zayavki tz = KasabaDB.tbpt_zayavki.FirstOrDefault(o => o.REC_ID == l);
                    i++;
                    if (i == oq)
                    {
                        tz.ORDER_QUANTITY = 1;
                        TryUpdateModel(tz);
                        break;
                    }
                    else
                    {
                        tz.ORDER_QUANTITY = 1;
                        TryUpdateModel(tz);
                    }
                }
                KasabaDB.SaveChanges();
                return "ok";
            }
            catch
            {
                return "error";
            }
        }
        public JsonResult GroupRZ_C(string inn, int zb)
        {
            var list = (from e in KasabaDB.tbpt_zayavki
                        where e.STATUS == "1"
                        join d in KasabaDB.t_camp on e.CAMP_ID equals d.id
                        join c in KasabaDB.t_departmentbase on e.DEPARTMENT_CODE equals c.code
                        where c.inn == inn && e.ZAEZD_BEGIN.Value.Year == zb
                        select new
                        {
                            ZB = e.ZAEZD_BEGIN,
                            SC = d.camps_name,
                            SC_id = d.id,
                            Q = e.QUANTITY,
                            OQ = e.ORDER_QUANTITY
                        });
            var Groups = from p in list
                         group p by new { p.ZB.Value.Month, p.SC, p.SC_id } into g
                         select new { ZB_g = g.Key.Month, SC_g = g.Key.SC, SC_g_id = g.Key.SC_id, OQ = g.Sum(i => i.OQ), Count = g.Count(), Q_Sum = g.Sum(i => i.Q) };
            return Json(Groups.ToList(), JsonRequestBehavior.AllowGet);
        }
        public string UpdateStatusRZ(int id, string s)
        {
            try
            {
                tbpt_zayavki tz = KasabaDB.tbpt_zayavki.FirstOrDefault(o => o.REC_ID == id);
                tz.STATUS = s;
                TryUpdateModel(tz);
                KasabaDB.SaveChanges();
                return "ok";
            }
            catch
            {
                return "error";
            }
        }
        public JsonResult Getbankpayments(string inn, int zb)
        {
            var list = from a in KasabaDB.tbgl_bankpayments
                       where a.TRAN_DATE.Value.Year == zb && a.BENEF_INN == inn
                       select new
                       {
                           TD = a.TRAN_DATE,
                           A = a.AMOUNT
                       };
            var Groups = from p in list
                         group p by p.TD.Value.Month into g
                         select new { ZB_g = g.Key, Count = g.Count(), A_Sum = g.Sum(i => i.A) };
            return Json(Groups.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAvarageprice(int yil)
        {
            var list = from a in KasabaDB.tbpt_avarageprice
                       where a.SET_DATE.Value.Year == yil
                       select a;
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbankpaymentsYear(string inn, int zb)
        {
            var list = from a in KasabaDB.tbgl_bankpayments
                       where a.TRAN_DATE.Value.Year == zb && a.BENEF_INN == inn
                       select new
                       {
                           TD = a.TRAN_DATE,
                           A = a.AMOUNT
                       };
            var Groups = from p in list
                         group p by p.TD.Value.Year into g
                         select new { ZB_g = g.Key, Count = g.Count(), A_Sum = g.Sum(i => i.A) };
            return Json(Groups.ToList(), JsonRequestBehavior.AllowGet);
        }
        public string SplitValue1(string value)
        {
            return value.Split(new Char[] { '&' })[0];
        }
        public string SplitValue2(string value)
        {
            return value.Split(new Char[] { '&' })[1];
        }
        public string AddPodachaZayavok(tbpt_zayavki pz)
        {
            if (pz != null)
            {
                using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                {
                    var x = MAX_GROUP_ID();
                    tbpt_zayavki z = new tbpt_zayavki();
                    z.USER_ID = Convert.ToInt32(User.Identity.Name);
                    z.ZAEZD_BEGIN = pz.ZAEZD_BEGIN;
                    z.ZAEZD_END = pz.ZAEZD_END;
                    z.GROUP_ID = x;
                    z.MEDICAL_ORDER = pz.MEDICAL_ORDER;
                    z.FULL_NAME = pz.FULL_NAME;
                    z.DEPARTMENT_CODE = LOAD_DEPARTMENT_CODE(pz.DEPARTMENT_CODE);
                    z.POSITION = pz.POSITION;
                    z.QUANTITY = pz.QUANTITY;
                    z.SANATORY_ID = pz.SANATORY_ID;
                    z.CAMP_ID = pz.CAMP_ID;
                    z.STATUS = "0";
                    z.ORDER_QUANTITY = 0;
                    z.TIMESTAMP = DateTime.Now;
                    z.MEDICAL_ORDER_EXISTS = pz.MEDICAL_ORDER_EXISTS;
                    contextObj.tbpt_zayavki.Add(z);
                    contextObj.SaveChanges();
                    return "tbpt_zayavki record added successfully";
                }
            }
            else
            {
                return "Invalid tbpt_zayavki record";
            }
        }
        public string LOAD_DEPARTMENT_CODE(string inn)
        {
            string name = "";
            var a = from d in KasabaDB.t_departmentbase orderby d.id select d;
            foreach (var v in a)
            {
                if (v.inn == inn)
                {
                    name = v.code;
                    break;
                }
            }
            return name;
        }
        public string LoadINN(int inn)
        {
            //string name = "";
            //var a = from d in KasabaDB.t_departmentbase orderby d.id select d;
            //foreach(var v in a)
            //{
            //    if (v.inn == inn.ToString())
            //    {
            //        name = v.name_uz;
            //        break;
            //    }
            //}
            //return name;

            string strInn = inn.ToString();

            var query = from department in KasabaDB.t_departmentbase
                        where department.inn == strInn
                        select department.name_uz;

            if (query.Any())
            {
                return query.First();
            }


            return "No Data Found";

        }
        public int MAX_GROUP_ID()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            int x = 0;
            var g = from a in KasabaDB.tbpt_zayavki where (a.STATUS == "0" && a.USER_ID == UserId) orderby a.REC_ID select a;
            foreach (var v in g)
            {
                x = Convert.ToInt32(v.GROUP_ID);
            }
            if (x == 0)
            {
                var a = from b in KasabaDB.tbpt_zayavki where (b.STATUS == "1" && b.USER_ID == UserId) orderby b.REC_ID select b;
                foreach (var v in a)
                {
                    x = Convert.ToInt32(v.GROUP_ID);
                }
                x++;
                return x;
            }
            return x;
        }
        public string STATUS(int group_id)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            string d = "";
            var a = from b in KasabaDB.tbpt_zayavki where b.GROUP_ID == group_id where b.USER_ID == UserId orderby b.REC_ID select b;
            foreach (var s in a)
            {
                d = s.STATUS;
            }
            return d;
        }
        public string DeletePodachaZayavok(string id)
        {

            if (!String.IsNullOrEmpty(id))
            {
                try
                {
                    int _bookId = Int32.Parse(id);
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        var _book = contextObj.tbpt_zayavki.Find(_bookId);
                        contextObj.tbpt_zayavki.Remove(_book);
                        contextObj.SaveChanges();
                        return "Selected book record deleted sucessfully";
                    }
                }
                catch (Exception)
                {
                    return "Book details not found";
                }
            }
            else
            {
                return "Invalid operation";
            }
        }
        //Update PodachaZayavok
        public string UpdatePodachaZayavok(tbpt_zayavki pz)
        {
            if (pz != null)
            {
                using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                {
                    int bookId = Convert.ToInt32(pz.REC_ID);
                    tbpt_zayavki _gz = contextObj.tbpt_zayavki.Where(b => b.REC_ID == bookId).FirstOrDefault();
                    _gz.ZAEZD_BEGIN = pz.ZAEZD_BEGIN;
                    _gz.ZAEZD_END = pz.ZAEZD_END;
                    _gz.USER_ID = Convert.ToInt32(User.Identity.Name);
                    _gz.POSITION = pz.POSITION;
                    _gz.QUANTITY = pz.QUANTITY;
                    _gz.CAMP_ID = pz.CAMP_ID;
                    _gz.SANATORY_ID = pz.SANATORY_ID;
                    _gz.FULL_NAME = pz.FULL_NAME;
                    _gz.DEPARTMENT_CODE = LOAD_DEPARTMENT_CODE(pz.DEPARTMENT_CODE);
                    _gz.MEDICAL_ORDER_EXISTS = pz.MEDICAL_ORDER_EXISTS;
                    _gz.MEDICAL_ORDER = pz.MEDICAL_ORDER;
                    _gz.TIMESTAMP = DateTime.Now;
                    contextObj.SaveChanges();
                    return "GZ record updated successfully";
                }
            }
            else
            {
                return "Invalid gz record";
            }
        }
        public string OTPPodachaZayavok()
        {
            using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                //int bookId = Convert.ToInt32(pz.REC_ID);
                var bb = from a in KasabaDB.tbpt_zayavki where a.USER_ID == UserId select a;
                foreach (var v in bb)
                {
                    tbpt_zayavki _gz = contextObj.tbpt_zayavki.Where(b => b.REC_ID == v.REC_ID).FirstOrDefault();
                    _gz.STATUS = "1";
                }
                contextObj.SaveChanges();
                return "OTP succes!!!";
            }
        }
        public ActionResult buxgalteriya()
        {
            return View();
        }
        public ActionResult PoiskRedaktZayavok()
        {
            return View();
        }
        public ActionResult ProsmotrRealzPutevok()
        {
            return View();
        }
        public ActionResult BU_RuchnayaPravodka()
        {
            return View();
        }
        public ActionResult BU_PrasmotrDannix()
        {
            return View();
        }
        public ActionResult VvodDannix()
        {
            return View();
        }
        public ActionResult SverkaDannix()
        {
            return View();
        }
        public ActionResult Putyovki()
        {
            int aaa = Convert.ToInt32(User.Identity.Name);
            if (Session["name"] == null)
            {
                IEnumerable<users> r = from a in KasabaDB.users where a.id ==aaa select a;
                foreach (var b in r)
                {
                    Session["name"] = b.email;
                    //FormsAuthentication.SetAuthCookie(b.id.ToString(), false);
                  
                }
            }

            return View();
        }
        public ActionResult Zayavok()
        {
            return View();
        }
        public ActionResult Buxgalter()
        {
            return View();
        }
        public ActionResult Otchot()
        {
            return View();
        }
        public ActionResult Admin()
        {
            return View();
        }
    }
}