using KasabaKiat.Helpers;
using KasabaKIAT.BusinessLogic.Buxgalteriya.PlanirovaniyaBudjeta.Region;
using KasabaKIAT.BusinessLogic.Buxgalteriya.PrixodChastichka;
using KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki;
using KasabaKIAT.BusinessLogic.ClosePeriod.CloseDay;
using KasabaKIAT.BusinessLogic.ClosePeriod.Holydays;
using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Buxgalteriya;
using log4net;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace KasabaKIAT.Controllers
{

    [Authorize]
    public class BuxgalterController : Controller
    {
        ILog log = log4net.LogManager.GetLogger(typeof(BuxgalterController));
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult BU_RuchnayaPravodka()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult BU_PrasmotrDannix()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult VvodDannix()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult OsushestvleniyeProvodki()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string EnterOsushestvleniyeProvodki(ProvodkiViewModel d)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var uu = UserInfo.GetUserInfo(UserId);
            Provodki pr = new Provodki();
            d.department_base = uu.DepartmentBase;
            d.user_id = UserId;
            d.trdt = DateTime.Now;
            pr.Enter(d);
            return "yes";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult Nakladniye()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult RaspredleniyeByudjeta()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult PlanByudjetaOblastey()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult PlanByudjetaOblasteyNoviy()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult PlanByudjetaOblasteyIzmenit()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        public ActionResult Read(FormCollection formCollection)
        {
            log.Info("Read");
            try
            {
                if (Request != null)
                {
                    HttpPostedFileBase file = Request.Files["UploadedFile"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        var platList = new List<BankpaymentsViewModel>();
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                var user = new BankpaymentsViewModel();

                                user.num_doc = workSheet.Cells[rowIterator, 1].Value.ToString();
                                user.payer_name = workSheet.Cells[rowIterator, 2].Value.ToString();
                                user.payer_inn = workSheet.Cells[rowIterator, 3].Value.ToString();
                                user.payer_mfo = workSheet.Cells[rowIterator, 4].Value.ToString();
                                user.payer_account = workSheet.Cells[rowIterator, 5].Value.ToString();
                                DateTime trandate;

                                if (DateTime.TryParseExact(workSheet.Cells[rowIterator, 6].Value.ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out trandate))
                                {
                                    user.tran_date = trandate;
                                }
                                else
                                {
                                    user.tran_date = DateTime.Now;

                                }
                                user.payment_datail = workSheet.Cells[rowIterator, 7].Value.ToString();
                                decimal amount = 0;

                                if (decimal.TryParse(workSheet.Cells[rowIterator, 8].Value.ToString(), out amount))
                                {
                                    user.amount = amount;
                                }
                                else
                                {
                                    user.amount = 0;

                                }
                                user.benef_name = workSheet.Cells[rowIterator, 9].Value.ToString();
                                user.benef_inn = workSheet.Cells[rowIterator, 10].Value.ToString();
                                user.benef_mfo = workSheet.Cells[rowIterator, 11].Value.ToString();
                                user.benef_account = workSheet.Cells[rowIterator, 12].Value.ToString();
                                user.dbcrflg = workSheet.Cells[rowIterator, 13].Value.ToString();

                                platList.Add(user);
                                //BankpaymentsDataManager.Create(ref user);
                            }
                        }
                        var data1 = from d in platList
                                    select new
                                    {
                                        d1 = d.num_doc,
                                        d2 = d.payer_name,
                                        d3 = d.payer_inn,
                                        d4 = d.payer_mfo,
                                        d5 = d.payer_account,
                                        d6 = d.tran_date,
                                        d7 = d.payment_datail,
                                        d8 = d.amount,
                                        d9 = d.benef_name,
                                        d10 = d.benef_inn,
                                        d11 = d.benef_mfo,
                                        d12 = d.benef_account,
                                        d13 = d.dbcrflg,
                                    };
                        log.Info("Count = " + platList.ToList().Count);
                        return Json(data1, JsonRequestBehavior.AllowGet);
                    }
                }
                
                return View("VvodDannix");
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
        public ActionResult Save(FormCollection formCollection)
        {
            log.Info("Save");
            try
            {
                if (Request != null)
                {
                    HttpPostedFileBase file = Request.Files["UploadedFile"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        var platList = new List<BankpaymentsViewModel>();
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                var user = new BankpaymentsViewModel();

                                user.num_doc = workSheet.Cells[rowIterator, 1].Value.ToString();
                                user.payer_name = workSheet.Cells[rowIterator, 2].Value.ToString();
                                user.payer_inn = workSheet.Cells[rowIterator, 3].Value.ToString();
                                user.payer_mfo = workSheet.Cells[rowIterator, 4].Value.ToString();
                                user.payer_account = workSheet.Cells[rowIterator, 5].Value.ToString();
                                DateTime trandate;

                                if (DateTime.TryParseExact(workSheet.Cells[rowIterator, 6].Value.ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out trandate))
                                {
                                    user.tran_date = trandate;
                                }
                                else
                                {
                                    user.tran_date = DateTime.Now;

                                }
                                user.payment_datail = workSheet.Cells[rowIterator, 7].Value.ToString();
                                decimal amount = 0;

                                if (decimal.TryParse(workSheet.Cells[rowIterator, 8].Value.ToString(), out amount))
                                {
                                    user.amount = amount;
                                }
                                else
                                {
                                    user.amount = 0;

                                }
                                user.benef_name = workSheet.Cells[rowIterator, 9].Value.ToString();
                                user.benef_inn = workSheet.Cells[rowIterator, 10].Value.ToString();
                                user.benef_mfo = workSheet.Cells[rowIterator, 11].Value.ToString();
                                user.benef_account = workSheet.Cells[rowIterator, 12].Value.ToString();
                                user.dbcrflg = workSheet.Cells[rowIterator, 13].Value.ToString();

                                platList.Add(user);
                                log.Info("Count = " + platList.ToList().Count);
                                BankpaymentsDataManager.Create(ref user);
                            }
                        }
                    }
                }
                return View("VvodDannix");
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
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetVvodDannix()
        {
            var data = KasabaDB.tbgl_bankpayments;
            var list = from d in data
                       select new
                       {
                           d0 = d.REC_ID,
                           d1 = d.NUM_DOC,
                           d2 = d.PAYER_NAME,
                           d3 = d.PAYER_INN,
                           d4 = d.PAYER_MFO,
                           d5 = d.PAYER_ACCOUNT,
                           d6 = d.TRAN_DATE,
                           d7 = d.PAYMENT_DATAIL,
                           d8 = d.AMOUNT,
                           d9 = d.BENEF_NAME,
                           d10 = d.BENEF_INN,
                           d11 = d.BENEF_MFO,
                           d12 = d.BENEF_ACCOUNT,
                           d13 = d.DBCRFLG,
                           d14 = d.PARENT_ID ,
                           d15 = d.DEPARTMENT_BASE,
                           d16 = d.USER_ID,
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        private PlanRegion planByudjet = new PlanRegion();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetPBRYears()
        {
            var list = planByudjet.YearsList();

            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FileResult DownloadTemplate()
        {
            var ArxiveDirectory = Properties.Settings.Default.TemplateDir;

            var targetFile = Path.Combine(ArxiveDirectory, "ZagruzkaVvodDannix.xlsx");

            if (!System.IO.File.Exists(targetFile)) return null;

            byte[] fileBytes = System.IO.File.ReadAllBytes(targetFile);
            string fileName = "ZagruzkaVvodDannix.xlsx";
            return File(fileBytes, "application/vnd.ms-excel", fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetRegionListByYear(int year)
        {
            var list = planByudjet.GetRegionListByYear(year);

            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="soato_code"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetRegionPlan(string soato_code, int year)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var uu = UserInfo.GetUserInfo(UserId);
            var department_code = uu.DepartmentBase;

            var list = planByudjet.GetRegionPlan(soato_code, year, department_code, UserId);

            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveItem"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveRegionPlan(PlanRegionViewModel saveItem)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var uu = UserInfo.GetUserInfo(UserId);
            var department_code = uu.DepartmentBase;

            if (!planByudjet.SaveChanges(saveItem)) return Json("error", JsonRequestBehavior.AllowGet);

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult DeleteRegionPlan(string soato_code, int year)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var uu = UserInfo.GetUserInfo(UserId);
            var department_code = uu.DepartmentBase;

            if (!planByudjet.DeletePlan(soato_code, year, department_code, UserId)) return Json("error", JsonRequestBehavior.AllowGet);

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult SpisokPravsoyuzov()
        {
            log.Info("SpisokPravsoyuzov");
            IList<Models.ViewModel> O = null;

            var UserId = Convert.ToInt32(User.Identity.Name);

            var userInfo = UserInfo.GetUserInfo(UserId);

            try
            {
                O = (from o in KasabaDB.tbku_StrokaProfsoyza
                     where o.MATCH_DEPARTMENTBASE != null 
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

            return Json(O.ToList(), JsonRequestBehavior.AllowGet); ;
        }

    }
}