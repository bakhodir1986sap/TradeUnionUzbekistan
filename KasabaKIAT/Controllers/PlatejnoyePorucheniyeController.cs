using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Buxgalteriya;
using KasabaKiat.Helpers;
using KasabaKIAT.BusinessLogic.Buxgalteriya.Platejki;
using log4net;
using System.Web.UI;
using KasabaKiat.BusinessLogic.Utils;

namespace KasabaKIAT.Controllers
{
    
    [Authorize]
    public class PlatejnoyePorucheniyeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        KasabaKIATEntities KasabaDB = new KasabaKIATEntities();

        /// <summary>
        /// 
        /// </summary>
        ILog log = log4net.LogManager.GetLogger(typeof(AdministirirovaniyeController));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult Index()
        {
            return View();
        }

        class platejkaOut
        {
            public long? d0        {get; set;}
            public DateTime? d1        {get; set;}
            public string d2        {get; set;}
            public string d3        {get; set;}
            public string d4        {get; set;}
            public string d5        {get; set;}
            public string d6        {get; set;}
            public string d7        {get; set;}
            public string d8        {get; set;}
            public string d9        {get; set;}
            public string d10       {get; set;}
            public string d11       {get; set;}
            public string d12       {get; set;}
            public decimal? d13       {get; set;}
            public string d14       {get; set;}
            public string d15       {get; set;}
            public string d16       {get; set;}
            public string d17 { get; set; }
        }

        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetPlatejnoyePorucheniye()
        {
            var list = from pp in KasabaDB.tbbu_platejka
                       select new platejkaOut
                       {
                           d0   =pp.REC_ID,
                           d1   =pp.PLAT_DATE,
                           d2   =pp.PLAT_NUM,
                           d3   =pp.PLATELNAME,
                           d4   =pp.PLATELINN,
                           d5   =pp.PLATELRASCHET,
                           d6   =pp.PLATELBANKNAME,
                           d7   =pp.PLATELBANKMFO,
                           d8   =pp.POLUCHNAME,
                           d9   =pp.POLUCHINN,
                           d10  =pp.POLUCHRASCHET,
                           d11  =pp.POLUCHBANKNAME,
                           d12  =pp.POLUCHBANKMFO,
                           d13  =pp.SUMMA,
                           d14  =pp.COMMENT,
                           d15  =pp.RUKOVODITEL,
                           d16  =pp.GLAVBUX,
                           d17  = ""
                       };


            List<platejkaOut> llist = list.ToList();

            foreach (var ll in llist)
            {
                ll.d17 = RuDateAndMoneyConverter.CurrencyToTxt((double)ll.d13, true);
            }

            return Json(llist, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string EnterPlatejnoyePorucheniye(PlatejkiViewModel d)
        {
            log.Info("EnterPlatejnoyePorucheniye");
            log.Info("PlatejkiViewModel = " + d);
            try
            {
                int UserId = Convert.ToInt32(User.Identity.Name);
                var uu = UserInfo.GetUserInfo(UserId);
                Platejki pl = new Platejki();
                d.department_code = uu.DepartmentBase;
                d.userid = UserId;

                pl.Enter(d);
                log.Info("Count = "+d.ToString().Count());
                return "yes";

                
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }

            return null;
           
        }
    }
}