using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KasabaKIAT.Models;
using KurortUrvServiceTest.ServiceInterAction.ActionLogic;
using System.IO;
using log4net;
using System.Web.UI.WebControls;
using System.Web.UI;
using ClosedXML;
using ClosedXML.Excel;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace KasabaKIAT.Controllers
{
    [Authorize]
    public class AdministirirovaniyeController : Controller
    {
        ServiceActionController service = new ServiceActionController();
        ILog log = log4net.LogManager.GetLogger(typeof(AdministirirovaniyeController));
        KasabaKIATEntities KasabaDB = new KasabaKIATEntities();


        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UchetniyZapis()
        {
            return View();
        }
        public ActionResult SozdaniyeRoley()
        {
            return View();
        }
        public ActionResult SozdaniyeProvodok()
        {
            return View();
        }
        public ActionResult PrivyazkaProvodok()
        {
            return View();
        }
        public ActionResult Spravichniki()
        {
            return View();
        }
        public ActionResult SpravichnikiKurortnovoUpravleniya()
        {
            return View();
        }
        public ActionResult SpisokPravsoyuzov()
        {
            IList<Models.ViewModel> O = (from o in KasabaDB.tbku_StrokaProfsoyza
                                         select new Models.ViewModel
                                         {
                                             Kod = o.ProfsoyzKod,
                                             Name = o.ProfsoyzNaimenovanie
                                         }).ToList();
            return View(O);
        }
        public ActionResult GrafikSanatoryZaezdov()
        {
            return View();
        }
        public ActionResult SchetFaktura()
        {
            return View();
        }
        public void ServiceSpisokPravsoyuzov()
        {
            try
            {
                service.LoadSpiskaProfsoyzov();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        public JsonResult GetSpisokPravsoyuzov()
        {
            var O = from o in KasabaDB.tbku_StrokaProfsoyza
                    select new
                    {
                        d1 = o.ProfsoyzKod,
                        d2 = o.ProfsoyzNaimenovanie
                    };
            return Json(O.ToList(), JsonRequestBehavior.AllowGet);

        }
        public void ServiceGrafikSanatoryZaezdov(string yil)
        {
            try
            {
                service.LoadZaezdovSanatoriev(yil);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        public JsonResult GetGrafikSanatoryZaezdov(int yil)
        {
            var O = from o in KasabaDB.tbku_GrafikZaezdovSanatoriev
                    where o.GodZaezdov==yil
                    select new
                    {
                        d1 = o.SanatoryNaimenovanie,
                        d2 = o.SanatoryProfil,
                        d3=o.SanatoryOblast,
                        d4=o.SanatoryKoekVsego,
                        d5=o.SanatoryKoekLetom,
                        d6=o.SanatoryKoekZimoy,
                        d7=o.SanatoryRazmeshenieNaimenovanie,
                        d8=o.Putevok1kv,
                        d9=o.Putevok2kv,
                        d10=o.Putevok3kv,
                        d11=o.Putevok4kv
                    };
            return Json(O.ToList(), JsonRequestBehavior.AllowGet);
        }
        public void ServiceSchetFaktura(DateTime B,DateTime E, string p)
        {
            
            try
            {
                service.LoadSchetFactura(B, E, p);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        public JsonResult GetSchetFaktura(DateTime B,DateTime E,string p)
        {
            var O = from o in KasabaDB.tbku_DocumentSchetFaktura
                    where o.DocData >= B&&o.DocData <= E&&o.ProfsoyzKod==p
                    select new
                    {
                        d0=o.ID,
                        d1 = o.DocNomer,
                        d2 = o.DocData,
                        d3 = o.DocTypeRealVozv,
                        d4 = o.ProfsoyzDogovor,
                        d5 = o.DataKvartalaPutevok,
                        d6 = o.ProfsoyzDoverennost,
                        d7 = o.ProfsoyzDoverennostData,
                        d8 = o.ProfsoyzDoverennostMOL,
                        d9 = o.ProfsoyzDoverennostMOL,
                        d10 = o.ProcentNacenki,
                        d11 = o.SummaNacenki,
                        d12 = o.KolichestvoPutevok,
                        d13 = o.TovarOtpustil,
                    };
            return Json(O.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStrokaSchetFakturi(int id)
        {
            var O = from o in KasabaDB.tbku_StrokaSchetFakturi
                    where o.SchetFactura_ID==id
                    select new
                    {
                        d0 = o.ID,
                        d1 = o.SchetFactura_ID,
                        d2 = o.SanatoryNaimenovanie,
                        d3 = o.SanatoryRazmeshenieNaimenovanie,
                        d4 = o.SanatoryType,
                        d5 = o.DataZaezda,
                        d6 = o.Blank1,
                        d7 = o.Blank2,
                        d8 = o.Kolichestvo,
                        d9 = o.Sebestoimost,
                        d10 = o.SummaSebestoimosti,
                        d11 = o.ProcentNacenki,
                        d12 = o.Cena,
                        d13 = o.Summa,
                    };
            return Json(O.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSNP()
        {
            var camp = from dict in KasabaDB.tbcs_dictionary
                       join dictType in KasabaDB.tbcs_dictionarytype on
                       dict.DICTIONARY_TYPE_ID equals dictType.REC_ID
                       where dictType.CODE == "NaznacheniyaPlateja"
                       select new
                       {
                           REC_ID = dict.REC_ID,
                           NAME_UZ = dict.NAME_UZ
                       };

            return Json(camp, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSPP()
        {
            var camp = from dict in KasabaDB.tbcs_dictionary
                       join dictType in KasabaDB.tbcs_dictionarytype on
                       dict.DICTIONARY_TYPE_ID equals dictType.REC_ID
                       where dictType.CODE == "order"
                       select new
                       {
                           REC_ID = dict.REC_ID,
                           NAME_UZ = dict.NAME_UZ
                       };

            return Json(camp, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSSPO()
        {
            var camp = from dict in KasabaDB.tbcs_dictionary
                       join dictType in KasabaDB.tbcs_dictionarytype on
                       dict.DICTIONARY_TYPE_ID equals dictType.REC_ID
                       where dictType.CODE == "Otdixayushix"
                       select new
                       {
                           REC_ID = dict.REC_ID,
                           NAME_UZ = dict.NAME_UZ
                       };
            return Json(camp, JsonRequestBehavior.AllowGet);
        }
        public string AddSTRS(tbcs_dictionary pz)
        {
            if (pz != null)
            {
                using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                {
                    tbcs_dictionary z = new tbcs_dictionary();
                    tbcs_dictionarytype pti = contextObj.tbcs_dictionarytype.Where(b => b.CODE == "placetype").FirstOrDefault();
                    z.USER_ID = Convert.ToInt32(User.Identity.Name);
                    z.NAME_RU = pz.NAME_RU;
                    z.NAME_UZ = pz.NAME_UZ;
                    z.NAME_LT = pz.NAME_LT;
                    z.STATUS = true;
                    z.DICTIONARY_TYPE_ID = pti.REC_ID;
                    z.CODE = z.REC_ID.ToString();
                    contextObj.tbcs_dictionary.Add(z);
                    contextObj.SaveChanges();
                    return "tbcs_dictionary record added successfully";
                }
            }
            else
            {
                return "Invalid tbcs_dictionary record";
            }
        }
        public string AddSNP(tbcs_dictionary pz)
        {
            if (pz != null)
            {
                using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                {
                    tbcs_dictionary z = new tbcs_dictionary();
                    tbcs_dictionarytype pti = contextObj.tbcs_dictionarytype.Where(b => b.CODE == "NaznacheniyaPlateja").FirstOrDefault();
                    z.USER_ID = Convert.ToInt32(User.Identity.Name);
                    z.NAME_RU = pz.NAME_RU;
                    z.NAME_UZ = pz.NAME_UZ;
                    z.NAME_LT = pz.NAME_LT;
                    z.STATUS = true;
                    z.DICTIONARY_TYPE_ID = pti.REC_ID;
                    z.CODE = z.REC_ID.ToString();
                    contextObj.tbcs_dictionary.Add(z);
                    contextObj.SaveChanges();
                    return "tbcs_dictionary record added successfully";
                }
            }
            else
            {
                return "Invalid tbcs_dictionary record";
            }
        }
        public string AddSSPO(tbcs_dictionary pz)
        {
            if (pz != null)
            {
                using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                {
                    tbcs_dictionary z = new tbcs_dictionary();
                    tbcs_dictionarytype pti = contextObj.tbcs_dictionarytype.Where(b => b.CODE == "Otdixayushix").FirstOrDefault();
                    z.USER_ID = Convert.ToInt32(User.Identity.Name);
                    z.NAME_RU = pz.NAME_RU;
                    z.NAME_UZ = pz.NAME_UZ;
                    z.NAME_LT = pz.NAME_LT;
                    z.STATUS = true;
                    z.DICTIONARY_TYPE_ID = pti.REC_ID;
                    z.CODE = z.REC_ID.ToString();
                    contextObj.tbcs_dictionary.Add(z);
                    contextObj.SaveChanges();
                    return "tbcs_dictionary record added successfully";
                }
            }
            else
            {
                return "Invalid tbcs_dictionary record";
            }
        }
        public string UpdateSTRS(tbcs_dictionary pz)
        {
            if (pz != null)
            {
                using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                {
                    int bookId = Convert.ToInt32(pz.REC_ID);
                    tbcs_dictionary z = contextObj.tbcs_dictionary.Where(b => b.REC_ID == bookId).FirstOrDefault();
                    z.USER_ID = Convert.ToInt32(User.Identity.Name);
                    z.NAME_RU = pz.NAME_RU;
                    z.NAME_UZ = pz.NAME_UZ;
                    z.NAME_LT = pz.NAME_LT;
                    z.CODE = z.REC_ID.ToString();
                    z.STATUS = true;
                    contextObj.SaveChanges();
                    return "tbcs_dictionary record updated successfully";
                }
            }
            else
            {
                return "Invalid tbcs_dictionary record";
            }
        }
        public string DeleteSTRS(string id)
        {

            if (!String.IsNullOrEmpty(id))
            {
                try
                {
                    int _bookId = Int32.Parse(id);
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        var _book = contextObj.tbcs_dictionary.Find(_bookId);
                        contextObj.tbcs_dictionary.Remove(_book);
                        contextObj.SaveChanges();
                        return "sucessfully";
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return "not found";
                }
            }
            else
            {
                return "Invalid operation";
            }
        }
        public JsonResult GetSSSP()
        {
            var camp = from dict in KasabaDB.tbpt_avarageprice
                       select new
                       {
                           ID = dict.REC_ID,
                           ST = dict.SET_DATE,
                           A=dict.AMOUNT
                       };

            return Json(camp, JsonRequestBehavior.AllowGet);
        }
        public string AddSSSP(tbpt_avarageprice pz)
        {
            if (pz != null)
            {
                using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                {
                    tbpt_avarageprice z = new tbpt_avarageprice();
                    z.SET_DATE = pz.SET_DATE;
                    z.AMOUNT = pz.AMOUNT;
                    z.USER_ID = Convert.ToInt32(User.Identity.Name);
                    contextObj.tbpt_avarageprice.Add(z);
                    contextObj.SaveChanges();
                    return "tbcs_dictionary record added successfully";
                }
            }
            else
            {
                return "Invalid tbcs_dictionary record";
            }
        }
        public string UpdateSSSP(tbpt_avarageprice pz)
        {
            if (pz != null)
            {
                using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                {
                    int bookId = Convert.ToInt32(pz.REC_ID);
                    tbpt_avarageprice z = contextObj.tbpt_avarageprice.Where(b => b.REC_ID == bookId).FirstOrDefault();
                    z.SET_DATE = pz.SET_DATE;
                    z.AMOUNT = pz.AMOUNT;
                    z.USER_ID = Convert.ToInt32(User.Identity.Name);
                    contextObj.SaveChanges();
                    return "tbcs_dictionary record updated successfully";
                }
            }
            else
            {
                return "Invalid tbcs_dictionary record";
            }
        }
        public string DeleteSSSP(string id)
        {

            if (!String.IsNullOrEmpty(id))
            {
                try
                {
                    int _bookId = Int32.Parse(id);
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        var _book = contextObj.tbpt_avarageprice.Find(_bookId);
                        contextObj.tbpt_avarageprice.Remove(_book);
                        contextObj.SaveChanges();
                        return "sucessfully";
                    }
                }
                catch (Exception)
                {
                    return "not found";
                }
            }
            else
            {
                return "Invalid operation";
            }
        }
        public JsonResult GetDB()
        {
            var data = from dp in KasabaDB.t_departmentbase
                       select new
                       {
                           id = dp.code,
                           value = dp.name_uz
                       };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSP()
        {
            var camp = from dict in KasabaDB.provodok
                       join dp in KasabaDB.t_departmentbase on dict.DEPARTMENT_BASE equals dp.code
                       select new
                       {
                           ID = dict.id,
                           INN = dp.inn,
                           O = dp.name_uz,
                           D = dict.description,
                           SB=dict.score_begin,
                           SE=dict.score_end
                       };

            return Json(camp, JsonRequestBehavior.AllowGet);
        }
        public string AddSP(provodok pz)
        {
            if (pz != null)
            {
                using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                {
                    provodok z = new provodok();
                    z.DEPARTMENT_BASE = LOAD_DEPARTMENT_CODE(pz.DEPARTMENT_BASE);
                    z.description = pz.description;
                    z.score_begin = pz.score_begin;
                    z.score_end = pz.score_end;
                    z.USER_ID = Convert.ToInt32(User.Identity.Name);
                    contextObj.provodok.Add(z);
                    contextObj.SaveChanges();
                    return "tbcs_dictionary record added successfully";
                }
            }
            else
            {
                return "Invalid tbcs_dictionary record";
            }
        }
        public string UpdateSP(provodok pz)
        {
            if (pz != null)
            {
                using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                {
                    int bookId = Convert.ToInt32(pz.id);
                    provodok z = contextObj.provodok.Where(b => b.id == bookId).FirstOrDefault();
                    z.DEPARTMENT_BASE = LOAD_DEPARTMENT_CODE(pz.DEPARTMENT_BASE);
                    z.description = pz.description;
                    z.score_begin = pz.score_begin;
                    z.score_end = pz.score_end;
                    z.USER_ID = Convert.ToInt32(User.Identity.Name);
                    contextObj.SaveChanges();
                    return "tbcs_dictionary record updated successfully";
                }
            }
            else
            {
                return "Invalid tbcs_dictionary record";
            }
        }
        public string DeleteSP(string id)
        {

            if (!String.IsNullOrEmpty(id))
            {
                try
                {
                    int _bookId = Int32.Parse(id);
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        var _book = contextObj.provodok.Find(_bookId);
                        contextObj.provodok.Remove(_book);
                        contextObj.SaveChanges();
                        return "sucessfully";
                    }
                }
                catch (Exception)
                {
                    return "not found";
                }
            }
            else
            {
                return "Invalid operation";
            }
        }
        public JsonResult GetPP(int o_id,string r_id)
        {
            var camp = from dict in KasabaDB.p_provodok
                       where dict.order==o_id&&dict.REGION_ID==r_id
                       join dp in KasabaDB.t_departmentbase on dict.DEPARTMENT_BASE equals dp.code
                       join a in KasabaDB.tbcs_dictionary on dict.order equals a.REC_ID
                       join r in KasabaDB.sp_soato on dict.REGION_ID equals r.code
                       select new
                       {
                           ID = dict.id,
                           INN = dp.inn,
                           O = dp.name_uz,
                           JO_id=a.REC_ID,
                           JO=a.NAME_UZ,
                           R_id=r.code,
                           R=r.name_uz,
                           D = dict.description,
                           SB = dict.score_begin,
                           SE = dict.score_end
                       };

            return Json(camp, JsonRequestBehavior.AllowGet);
        }
        public string AddPP(p_provodok pz)
        {
            if (pz != null)
            {
                using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                {
                    p_provodok z = new p_provodok();
                    z.DEPARTMENT_BASE = LOAD_DEPARTMENT_CODE(pz.DEPARTMENT_BASE);
                    z.description = pz.description;
                    z.score_begin = pz.score_begin;
                    z.score_end = pz.score_end;
                    z.order = pz.order;
                    z.REGION_ID = pz.REGION_ID;
                    z.USER_ID = Convert.ToInt32(User.Identity.Name);
                    contextObj.p_provodok.Add(z);
                    contextObj.SaveChanges();
                    return "tbcs_dictionary record added successfully";
                }
            }
            else
            {
                return "Invalid tbcs_dictionary record";
            }
        }
        public string UpdatePP(p_provodok pz)
        {
            if (pz != null)
            {
                using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                {
                    int bookId = Convert.ToInt32(pz.id);
                    p_provodok z = contextObj.p_provodok.Where(b => b.id == bookId).FirstOrDefault();
                    z.DEPARTMENT_BASE = LOAD_DEPARTMENT_CODE(pz.DEPARTMENT_BASE);
                    z.description = pz.description;
                    z.score_begin = pz.score_begin;
                    z.score_end = pz.score_end;
                    z.order = pz.order;
                    z.REGION_ID = pz.REGION_ID;
                    z.USER_ID = Convert.ToInt32(User.Identity.Name);
                    contextObj.SaveChanges();
                    return "tbcs_dictionary record updated successfully";
                }
            }
            else
            {
                return "Invalid tbcs_dictionary record";
            }
        }
        public string DeletePP(string id)
        {

            if (!String.IsNullOrEmpty(id))
            {
                try
                {
                    int _bookId = Int32.Parse(id);
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        var _book = contextObj.p_provodok.Find(_bookId);
                        contextObj.p_provodok.Remove(_book);
                        contextObj.SaveChanges();
                        return "sucessfully";
                    }
                }
                catch (Exception)
                {
                    return "not found";
                }
            }
            else
            {
                return "Invalid operation";
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
        public ActionResult SredneyStoimostiPutevok()
        {
            return View();
        }
        public ActionResult RazmesheniyaSanatorii()
        {
            return View();
        }
        public ActionResult Otdixayushix()
        {
            return View();
        }
        public ActionResult NaznacheniyaPlateja()
        {
            return View();
        }
        public JsonResult GetUZ()
        {
            var U = from u in KasabaDB.users
                    join d in KasabaDB.t_departmentbase on u.bkut_id equals d.id
                    join r in KasabaDB.roles on u.role_id equals r.id
                    join rg in KasabaDB.sp_soato on u.REGION_ID equals rg.code
                    join ot in KasabaDB.sp_branch on u.BRANCH_ID equals ot.id
                    select new
                    {
                        ID = u.id,
                        O_id=d.id,
                        O=d.name_uz,
                        Rl_id=r.id,
                        Rl=r.name,
                        L = u.email,
                        Rg_id= rg.code,
                        Rg= rg.name_uz,
                        Ot_id=ot.id,
                        Ot=ot.name_uz,
                        N=u.user_name,
                        D=u.created,
                        P=u.password
                    };
            return Json(U.ToList(), JsonRequestBehavior.AllowGet);
            //string DEPARTMENT = "";
            //long? DEP = null;
            //var Usr = from u in KasabaDB.users select u;
            //if (Usr.Any())
            //{
            //    var d = Usr.FirstOrDefault();
            //    if (d.bkut_id != null)
            //    {
            //        DEP = Convert.ToInt32(d.bkut_id);
            //    }
            //    else
            //    {
            //        if (d.dep_id != null)
            //        {
            //            DEP = Convert.ToInt32(d.dep_id);
            //        }
            //        else
            //        {
            //            if (d.bkuq_id != null)
            //            {
            //                DEP = Convert.ToInt32(d.bkuq_id);
            //            }
            //        }
            //    }
            //}
            //var Dpr = KasabaDB.t_departmentbase.Where(i => i.id == DEP).FirstOrDefault();
            //DEPARTMENT = Dpr.code;
        }
        public string AddUZ(users pz)
        {
            if (pz != null)
            {
                using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                {
                    users z = new users();
                    z.version = 0;
                    z.account_expired = false;
                    z.account_locked = false;
                    z.changed = DateTime.Now;
                    z.created = DateTime.Now;
                    z.email = pz.email;
                    z.last_logged = DateTime.Now;
                    z.last_logged_id = HttpContext.Server.ToString();
                    z.state = "ACTIVE";
                    z.password = pz.password;
                    z.salt = "";
                    z.language = "UZ";
                    z.is_super_admin = false;
                    z.user_name = pz.user_name;
                    z.s_comment = "";
                    z.s_create = DateTime.Now;
                    z.s_status = "ACTIVE";
                    z.s_user_id = 12;
                    z.usertype = "BKUT";
                    z.bkut_id = pz.bkut_id;
                    z.role_id = pz.role_id;
                    z.REGION_ID = pz.REGION_ID;
                    z.BRANCH_ID = pz.BRANCH_ID;
                    contextObj.users.Add(z);
                    contextObj.SaveChanges();
                    return "tbcs_dictionary record added successfully";
                }
            }
            else
            {
                return "Invalid tbcs_dictionary record";
            }
        }
        public string UpdateUZ(users pz)
        {
            if (pz != null)
            {
                using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                {
                    int bookId = Convert.ToInt32(pz.id);
                    users z = contextObj.users.Where(b => b.id == bookId).FirstOrDefault();
                    z.version = 0;
                    z.account_expired = false;
                    z.account_locked = false;
                    z.changed = DateTime.Now;
                    z.created = DateTime.Now;
                    z.email = pz.email;
                    z.last_logged = DateTime.Now;
                    z.last_logged_id = HttpContext.Server.ToString();
                    z.state = "ACTIVE";
                    z.password = pz.password;
                    z.salt = "";
                    z.language = "UZ";
                    z.is_super_admin = false;
                    z.user_name = pz.user_name;
                    z.s_comment = "";
                    z.s_create = DateTime.Now;
                    z.s_status = "ACTIVE";
                    z.s_user_id = 12;
                    z.usertype = "BKUT";
                    z.bkut_id = pz.bkut_id;
                    z.role_id = pz.role_id;
                    z.REGION_ID = pz.REGION_ID;
                    z.BRANCH_ID = pz.BRANCH_ID;
                    contextObj.SaveChanges();
                    return "tbcs_dictionary record updated successfully";
                }
            }
            else
            {
                return "Invalid tbcs_dictionary record";
            }
        }
        public string DeleteUZ(string id)
        {

            if (!String.IsNullOrEmpty(id))
            {
                try
                {
                    int _bookId = Int32.Parse(id);
                    using (KasabaKIATEntities contextObj = new KasabaKIATEntities())
                    {
                        var _book = contextObj.users.Find(_bookId);
                        contextObj.users.Remove(_book);
                        contextObj.SaveChanges();
                        return "sucessfully";
                    }
                }
                catch (Exception)
                {
                    return "not found";
                }
            }
            else
            {
                return "Invalid operation";
            }
        }
        public JsonResult GelOrganizatsiya()
        {
            var O = from o in KasabaDB.t_departmentbase
                    select new
                    {
                        id = o.id,
                        value = o.name_uz
                    };
            return Json(O.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRoles()
        {
            var R = from r in KasabaDB.roles
                    select new
                    {
                        id = r.id,
                        value = r.name
                    };
            return Json(R.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOtrasl()
        {
            var O = from o in KasabaDB.sp_branch
                    select new
                    {
                        id = o.id,
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