using KasabaKIAT.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace KasabaKIAT.Controllers
{
    [Authorize]
    public class GodovoyPlanController : Controller
    {
        ILog log = log4net.LogManager.GetLogger(typeof(AdministirirovaniyeController));
        KasabaKIATEntities KasabaDB = new KasabaKIATEntities();
        public ActionResult Index()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var user = from u in KasabaDB.users
                       where u.id == UserId
                       join dp in KasabaDB.t_departmentbase on u.dep_id equals dp.id
                       where dp.code.Contains("172") && dp.code.ToString().Length == 5
                       select dp;
            if (user.Any())
            {
                ViewBag.TR = "visibility:hidden;position:absolute";
                ViewBag.R = "Отрасль";
                ViewBag.px200 = "180px";
            }
            else
            {
                ViewBag.TR = "";
                ViewBag.R = "Регион";
                ViewBag.px200 = "";
            }
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
        public JsonResult GetRegion()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var user = from u in KasabaDB.users
                       where u.id == UserId
                       join dp in KasabaDB.t_departmentbase on u.dep_id equals dp.id
                       where dp.code.Contains("172") && dp.code.ToString().Length == 5
                       select dp;
            if (user.Any())
            {
                var R = from r in KasabaDB.sp_branch
                        select new
                        {
                            id = r.id,
                            value = r.name_uz
                        };
                return Json(R, JsonRequestBehavior.AllowGet);
            }
            else
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
        public JsonResult GetGodovoyPlan_S(int R,int SC)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var user = from u in KasabaDB.users where u.id== UserId
                       join dp in KasabaDB.t_departmentbase on u.dep_id equals dp.id
                       where dp.code.Contains("172") && dp.code.ToString().Length == 5
                       select dp;
            if (user.Any())
            {
                var list = (from a in KasabaDB.tbpt_yearlyplanotrasl
                            where a.STATUS == "1"&&a.USER_ID == UserId && a.OTRASL_ID == R&&a.SANATORIUM_ID==SC
                            //join u in KasabaDB.users on a.USER_ID equals u.id
                            join sc in KasabaDB.t_sanatorium on a.SANATORIUM_ID equals sc.id
                            join d in KasabaDB.t_departmentbase on a.DEPARTMANT_BASE equals d.code
                            //join dc in KasabaDB.tbcs_dictionary on a.TIP_MEST_ID equals dc.REC_ID
                            join r in KasabaDB.sp_branch on a.OTRASL_ID equals r.id
                            select new
                            {
                                ID = a.REC_ID,
                                Y = a.YEAR,
                                K = a.KVARTAL,
                                SC = sc.sanatorium_name,
                                SC_id = sc.id,
                                TM = "",
                                TM_id = 0,
                                P = a.PRICE,
                                Q = a.QUANTITY,
                                R = r.name_uz,
                                R_id = r.id,
                                MD = a.MODIFY_DATE,
                                O = d.name_uz,
                                O_id = d.code
                            });
                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string re = R.ToString();
                var list = (from a in KasabaDB.tbpt_yearlyplanregion
                            where a.STATUS == "1"&&a.USER_ID == UserId && a.REGION_ID==re&&a.SANATORIUM_ID==SC
                            //join u in KasabaDB.users on a.USER_ID equals u.id
                            join sc in KasabaDB.t_sanatorium on a.SANATORIUM_ID equals sc.id
                            join d in KasabaDB.t_departmentbase on a.DEPARTMENT_BASE equals d.code
                            join dc in KasabaDB.tbcs_dictionary on a.TIP_MEST_ID equals dc.REC_ID
                            join r in KasabaDB.sp_soato on a.REGION_ID equals r.code
                            select new
                            {
                                ID = a.REC_ID,
                                Y = a.YEAR,
                                K = a.KVARTAL,
                                SC = sc.sanatorium_name,
                                SC_id = sc.id,
                                TM = dc.NAME_UZ,
                                TM_id = dc.REC_ID,
                                P = a.PRICE,
                                Q = a.QUANTITY,
                                R = r.name_uz,
                                R_id = r.code,
                                MD = a.MODIFY_DATE,
                                O = d.name_uz,
                                O_id = d.code
                            });
                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
        }
        //[HttpPost]
        public JsonResult GetGodovoyPlan_C(int R, int SC)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var user = from u in KasabaDB.users
                       where u.id == UserId
                       join dp in KasabaDB.t_departmentbase on u.dep_id equals dp.id
                       where dp.code.Contains("172") && dp.code.ToString().Length == 5
                       select dp;
            if (user.Any())
            {
                var list = (from a in KasabaDB.tbpt_yearlyplanotrasl
                            where a.STATUS == "1" && a.USER_ID == UserId && a.OTRASL_ID == R && a.CAMP_ID == SC
                            //join u in KasabaDB.users on a.USER_ID equals u.id
                            join sc in KasabaDB.t_camp on a.CAMP_ID equals sc.id
                            join d in KasabaDB.t_departmentbase on a.DEPARTMANT_BASE equals d.code
                            //join dc in KasabaDB.tbcs_dictionary on a.TIP_MEST_ID equals dc.REC_ID
                            join r in KasabaDB.sp_branch on a.OTRASL_ID equals r.id
                            select new
                            {
                                ID = a.REC_ID,
                                Y = a.YEAR,
                                K = a.KVARTAL,
                                SC = sc.camps_name,
                                SC_id = sc.id,
                                TM = "",
                                TM_id = 0,
                                P = a.PRICE,
                                Q = a.QUANTITY,
                                R = r.name_uz,
                                R_id = r.id,
                                MD = a.MODIFY_DATE,
                                O = d.name_uz,
                                O_id = d.code
                            });
                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string re = R.ToString();
                var list = (from a in KasabaDB.tbpt_yearlyplanregion
                            where a.STATUS == "1" && a.USER_ID == UserId && a.REGION_ID == re && a.SANATORIUM_ID == SC
                            //join u in KasabaDB.users on a.USER_ID equals u.id
                            join sc in KasabaDB.t_camp on a.CAMP_ID.Value equals sc.id
                            join d in KasabaDB.t_departmentbase on a.DEPARTMENT_BASE equals d.code
                            join dc in KasabaDB.tbcs_dictionary on a.TIP_MEST_ID equals dc.REC_ID
                            join r in KasabaDB.sp_soato on a.REGION_ID equals r.code
                            select new
                            {
                                ID = a.REC_ID,
                                Y = a.YEAR,
                                K = a.KVARTAL,
                                SC = sc.camps_name,
                                SC_id = sc.id,
                                TM = dc.NAME_UZ,
                                TM_id = dc.REC_ID,
                                P = a.PRICE,
                                Q = a.QUANTITY,
                                R = r.name_uz,
                                R_id = r.code,
                                MD = a.MODIFY_DATE,
                                O = d.name_uz,
                                O_id = d.code
                            });
                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
           
        }
        [HttpPost]
        public string AddGodovoyPlan(tbpt_yearlyplanregion GP)
        {
            int UserId= Convert.ToInt32(User.Identity.Name);
            if (GP != null)
            {
                using (KasabaKIATEntities k = new KasabaKIATEntities())
                {
                    var user = from u in KasabaDB.users
                               where u.id == UserId
                               join dp in KasabaDB.t_departmentbase on u.dep_id equals dp.id
                               where dp.code.Contains("172") && dp.code.ToString().Length == 5
                               select dp.code;
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
                    if (user.Any())
                    {
                        tbpt_yearlyplanotrasl _GP = new tbpt_yearlyplanotrasl();
                        _GP.YEAR = DateTime.Now.Year.ToString();
                        _GP.KVARTAL = GP.KVARTAL;
                        _GP.SANATORIUM_ID = GP.SANATORIUM_ID;
                        _GP.CAMP_ID = GP.CAMP_ID;
                        //_GP.TIP_MEST_ID = GP.TIP_MEST_ID;
                        _GP.PRICE = GP.PRICE;
                        _GP.QUANTITY = GP.QUANTITY;
                        _GP.STATUS = "1";
                        _GP.USER_ID = UserId;
                        _GP.OTRASL_ID = Convert.ToInt32(GP.REGION_ID);
                        _GP.MODIFY_DATE = DateTime.Now;
                        _GP.DEPARTMANT_BASE = DEPARTMENT;
                        k.tbpt_yearlyplanotrasl.Add(_GP);
                    }
                    else
                    {
                        tbpt_yearlyplanregion _GP = new tbpt_yearlyplanregion();
                        _GP.YEAR = DateTime.Now.Year.ToString();
                        _GP.KVARTAL = GP.KVARTAL;
                        _GP.SANATORIUM_ID = GP.SANATORIUM_ID;
                        _GP.CAMP_ID = GP.CAMP_ID;
                        _GP.TIP_MEST_ID = GP.TIP_MEST_ID;
                        _GP.PRICE = GP.PRICE;
                        _GP.QUANTITY = GP.QUANTITY;
                        _GP.STATUS = "1";
                        _GP.USER_ID = UserId;
                        _GP.REGION_ID = GP.REGION_ID;
                        _GP.MODIFY_DATE = DateTime.Now;
                        _GP.DEPARTMENT_BASE = DEPARTMENT;
                        k.tbpt_yearlyplanregion.Add(_GP);
                    }
                    k.SaveChanges();
                    return "ok";
                }
            }
            else
            {
                return "error";
            }
        }
        [HttpPost]
        public string UpdateGodovoyPlan(tbpt_yearlyplanregion GP)
        {
            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                if (GP != null)
                {
                    using (KasabaKIATEntities k = new KasabaKIATEntities())
                    {
                        int id = Convert.ToInt32(GP.REC_ID);
                        var user = from u in KasabaDB.users
                                   where u.id == UserId
                                   join dp in KasabaDB.t_departmentbase on u.dep_id equals dp.id
                                   where dp.code.Contains("172") && dp.code.ToString().Length == 5
                                   select dp;
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
                        if (user.Any())
                        {
                            tbpt_yearlyplanotrasl _GP = k.tbpt_yearlyplanotrasl.Where(i => i.REC_ID == id).FirstOrDefault();
                            _GP.YEAR = DateTime.Now.Year.ToString();
                            _GP.KVARTAL = GP.KVARTAL;
                            _GP.SANATORIUM_ID = GP.SANATORIUM_ID;
                            _GP.CAMP_ID = GP.CAMP_ID;
                            //_GP.TIP_MEST_ID = GP.TIP_MEST_ID;
                            _GP.PRICE = GP.PRICE;
                            _GP.QUANTITY = GP.QUANTITY;
                            _GP.STATUS = "1";
                            _GP.USER_ID = UserId;
                            _GP.OTRASL_ID = Convert.ToInt32(GP.REGION_ID);
                            _GP.MODIFY_DATE = DateTime.Now;
                            _GP.DEPARTMANT_BASE = DEPARTMENT;
                            k.SaveChanges();
                            return "ok";
                        }
                        else
                        {
                            tbpt_yearlyplanregion _GP = k.tbpt_yearlyplanregion.Where(i => i.REC_ID == id).FirstOrDefault();
                            _GP.YEAR = DateTime.Now.Year.ToString();
                            _GP.KVARTAL = GP.KVARTAL;
                            _GP.SANATORIUM_ID = GP.SANATORIUM_ID;
                            _GP.CAMP_ID = GP.CAMP_ID;
                            _GP.TIP_MEST_ID = GP.TIP_MEST_ID;
                            _GP.PRICE = GP.PRICE;
                            _GP.QUANTITY = GP.QUANTITY;
                            _GP.STATUS = "1";
                            _GP.USER_ID = UserId;
                            _GP.REGION_ID = GP.REGION_ID;
                            _GP.MODIFY_DATE = DateTime.Now;
                            _GP.DEPARTMENT_BASE = DEPARTMENT;
                            k.SaveChanges();
                            return "ok";
                        }
                    }
                }
                else
                {
                    return "error";
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
                return "no";
            }
        }
        [HttpPost]
        public string DeleteGodovoyPlan(string id)
        {
            try
            {
                using (KasabaKIATEntities k = new KasabaKIATEntities())
                {
                    int _id = Convert.ToInt32(id);
                    int UserId = Convert.ToInt32(User.Identity.Name);
                    var user = from u in KasabaDB.users
                               where u.id == UserId
                               join dp in KasabaDB.t_departmentbase on u.dep_id equals dp.id
                               where dp.code.Contains("172") && dp.code.ToString().Length == 5
                               select dp;
                    if (user.Any())
                    {

                        tbpt_yearlyplanotrasl p1 = k.tbpt_yearlyplanotrasl.Where(i => i.REC_ID == _id).FirstOrDefault();
                        p1.STATUS = "0";
                        k.SaveChanges();
                        return "ok";
                    }
                    else
                    {
                        tbpt_yearlyplanregion p1 = k.tbpt_yearlyplanregion.Where(i => i.REC_ID == _id).FirstOrDefault();
                        p1.STATUS = "0";
                        k.SaveChanges();
                        return "ok";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return "error";
            }
        }
    }
}