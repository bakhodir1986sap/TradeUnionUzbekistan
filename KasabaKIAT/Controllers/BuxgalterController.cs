using KasabaKiat.BusinessLogic.Utils;
using KasabaKiat.Helpers;
using KasabaKIAT.BusinessLogic.Buxgalteriya.ECP_Management;
using KasabaKIAT.BusinessLogic.Buxgalteriya.FinansoviyOtchet;
using KasabaKIAT.BusinessLogic.Buxgalteriya.GlavnayaKniga;
using KasabaKIAT.BusinessLogic.Buxgalteriya.PlanirovaniyaBudjeta.Region;
using KasabaKIAT.BusinessLogic.Buxgalteriya.PlanProvodok;
using KasabaKIAT.BusinessLogic.Buxgalteriya.PrixodChastichka;
using KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki;
using KasabaKIAT.BusinessLogic.ClosePeriod.CloseDay;
using KasabaKIAT.BusinessLogic.ClosePeriod.CloseKvartal;
using KasabaKIAT.BusinessLogic.ClosePeriod.Holydays;
using KasabaKIAT.BusinessLogic.Security.Roles.RolePermissions;
using KasabaKIAT.BusinessLogic.Utils;
using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Buxgalteriya;
using log4net;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KasabaKIAT.BusinessLogic.Buxgalteriya;

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
        public async Task<string> EnterOsushestvleniyeProvodki(ProvodkiViewModel d)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var uu = UserInfo.GetUserInfo(UserId);
            Provodki pr = new Provodki();
            d.department_base = uu.DepartmentBase;
            d.user_id = UserId;
            d.trref = "RUK";
            d.trrefno = "0000000";
            tbbu_provodki provod = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                pr.Enter(d, db, out provod);


                await db.SaveChangesAsync();
            }

            return "yes";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public string GetSummaDescriptionFor(string amount)
        {
            double summa;

            if (double.TryParse(amount, NumberStyles.Number, CultureInfo.InvariantCulture, out summa))
            {
                return RuDateAndMoneyConverter.CurrencyToTxt((double)summa, true);
            }

            return "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSchetDebit()
        {
            var list = AccountTypeDataManager.GetDebitSchets();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSchetCredit()
        {
            var list = AccountTypeDataManager.GetCreditSchets();

            return Json(list, JsonRequestBehavior.AllowGet);
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
                                try
                                {
                                    var user = new BankpaymentsViewModel();

                                    user.num_doc = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    user.payer_name = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    user.payer_inn = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    user.payer_mfo = workSheet.Cells[rowIterator, 4].Value.ToString();
                                    user.payer_account = workSheet.Cells[rowIterator, 5].Value.ToString();
                                    DateTime trandate;
                                    string DateOfPayment = workSheet.Cells[rowIterator, 6].Value.ToString();

                                    if (DateOfPayment.Length > 10)
                                    {
                                        DateOfPayment = DateOfPayment.Substring(0, 10);
                                    }

                                    if (DateTime.TryParseExact(DateOfPayment, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out trandate))
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
                                }
                                catch (Exception ex)
                                {
                                    log.Info(ex);
                                }

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
        public async Task<ActionResult> Save(FormCollection formCollection)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

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
                                try
                                {
                                    var user = new BankpaymentsViewModel();

                                    user.num_doc = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    user.payer_name = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    user.payer_inn = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    user.payer_mfo = workSheet.Cells[rowIterator, 4].Value.ToString();
                                    user.payer_account = workSheet.Cells[rowIterator, 5].Value.ToString();
                                    DateTime trandate;

                                    string DateOfPayment = workSheet.Cells[rowIterator, 6].Value.ToString();

                                    if (DateOfPayment.Length > 10)
                                    {
                                        DateOfPayment = DateOfPayment.Substring(0, 10);
                                    }

                                    if (DateTime.TryParseExact(DateOfPayment, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out trandate))
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
                                    user.user_id = UserId;
                                    user.department_base = ustinfo.DepartmentBase;

                                    platList.Add(user);
                                    log.Info("Count = " + platList.ToList().Count);
                                    BankpaymentsDataManager.Create(ref user, KasabaDB);
                                }
                                catch (Exception ex)
                                {
                                    log.Info(ex);
                                }
                            }

                            await KasabaDB.SaveChangesAsync();

                            await MakeBankProvodka();
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
        public async Task<JsonResult> MakeBankProvodka()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            await BankpaymentsDataManager.MakeProvodka(ustinfo.DepartmentBase, UserId);

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<RedirectResult> CloseKvartal()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var kvartal = new CloseKvartal();

            kvartal.Close(ustinfo.DepartmentBase, UserId, KasabaDB);

            try
            {
                await KasabaDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            var users = UserPermissions.userData.FirstOrDefault(u => u.userid == UserId);
            UserPermissions.userData.Remove(users);
            var ustinfo2 = UserInfo.GetUserInfo(UserId);

            return Redirect("/Home/Putyovki");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public JsonResult GlavnayaKniga(int year, int kvartal)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var gvk = new GK_Structure(ustinfo.DepartmentBase, year, kvartal);
            const bool isSvodniyOtchet = false;
            var OutList = gvk.MakeGlavKniga(isSvodniyOtchet);

            return Json(OutList, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public JsonResult GlavnayaKnigaVRazreze(int year, int kvartal, string departmentBase)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var gvk = new GK_Structure(departmentBase, year, kvartal);
            const bool isSvodniyOtchet = false;
            var OutList = gvk.MakeGlavKniga(isSvodniyOtchet);

            return Json(OutList, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public JsonResult GlavnayaKnigaSvodniy(int year, int kvartal)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var gvk = new GK_Structure("17", year, kvartal);
            const bool isSvodniyOtchet = true;
            var OutList = gvk.MakeGlavKniga(isSvodniyOtchet);

            return Json(OutList, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public JsonResult FinansoviyOtchet(int year, int kvartal)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var finOtchet = new FinOtchetStrukture();
            finOtchet.MakeFinOtchet(ustinfo.DepartmentBase, year, kvartal, UserId);

            finOtchet._totaloutput.SelectedKvartalYear =  string.Format("{0} год -{1} кв", year, kvartal);
            finOtchet._totaloutput.OrgName = ustinfo.OrgName;

            var result = finOtchet._totaloutput;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveRasxod(SaveFinOthers2 postData)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            if (postData.rasxod_arr == null) return Json("error", JsonRequestBehavior.AllowGet);

            var finOtchet = new FinOtchetStrukture();
            finOtchet.SaveOtherInfo(ustinfo.DepartmentBase, postData.year, postData.kvartal, UserId, postData.rasxod_arr);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSanatoriyProfilaktoriy(int year, int kvartal)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var bux = new SanatoriyProfilaktoriy();

            var result = bux.GetData(ustinfo.DepartmentBase , year, kvartal);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveSanatoriyProfilaktoriy(SanatoriyProfilaktoriyModel postData)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var bux = new SanatoriyProfilaktoriy();

            bux.SaveData(ustinfo.DepartmentBase, postData.year, postData.kvartal, postData);

            return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetLagerFinOtchet(int year, int kvartal)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var bux = new LagerFinOtchet();

            var result = bux.GetData(ustinfo.DepartmentBase, year, kvartal);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveLagerFinOtchet(LagerFinOtchetModel postData)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var bux = new LagerFinOtchet();

            bux.SaveData(ustinfo.DepartmentBase, postData.year, postData.kvartal, postData);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public JsonResult FinansoviyOtchetVRazreze(int year, int kvartal , string departmentBase)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var finOtchet = new FinOtchetStrukture();
            finOtchet.MakeFinOtchet(departmentBase, year, kvartal, UserId);

            finOtchet._totaloutput.SelectedKvartalYear = string.Format("{0} год -{1} кв", year, kvartal);
            finOtchet._totaloutput.OrgName = KasabaDB.tbku_StrokaProfsoyza.Where(z=>z.MATCH_DEPARTMENTBASE == departmentBase).First().ProfsoyzNaimenovanie;

            var result = finOtchet._totaloutput;

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public JsonResult FinansoviyOtchetSvodniy(int year, int kvartal)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var finOtchet = new FinOtchetStrukture();
            finOtchet.MakeFinOtchet("17", year, kvartal, UserId, true);

            finOtchet._totaloutput.SelectedKvartalYear = string.Format("{0} год -{1} кв", year, kvartal);
            finOtchet._totaloutput.OrgName = ustinfo.OrgName;

            var result = finOtchet._totaloutput;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <returns></returns>
        public JsonResult GlavnayaKnigaTest(string depCode , int year, int kvartal)
        {
            var gvk = new GK_Structure(depCode, year, kvartal);

            var OutList = gvk.MakeGlavKniga();

            return Json(OutList, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Test purpose
        /// </summary>
        /// <returns></returns>
        public async Task CloseKvartalParams(string depCode, long UserId)
        {
            //int UserId = Convert.ToInt32(User.Identity.Name);
            //var ustinfo = UserInfo.GetUserInfo(UserId);

            var kvartal = new CloseKvartal();

            kvartal.Close(depCode, UserId, KasabaDB);

            await KasabaDB.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetVvodDannix(int year, int kvartal, string inn, string account)
        {
            var data = KasabaDB.tbgl_bankpayments;
            var list = from d in data
                       where d.PAYER_INN == (string.IsNullOrEmpty(inn) ? d.PAYER_INN : inn)
                       && d.TRAN_DATE.Year == year
                       && d.PAYER_ACCOUNT == (string.IsNullOrEmpty(account) ? d.PAYER_ACCOUNT : account)
                       && (((kvartal == 1) && (d.TRAN_DATE.Month >= 1 && d.TRAN_DATE.Month <= 3)) ||
                            ((kvartal == 2) && (d.TRAN_DATE.Month >= 4 && d.TRAN_DATE.Month <= 6)) ||
                            ((kvartal == 3) && (d.TRAN_DATE.Month >= 7 && d.TRAN_DATE.Month <= 9)) ||
                            ((kvartal == 4) && (d.TRAN_DATE.Month >= 10 && d.TRAN_DATE.Month <= 12)) 
                       )
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

            var OutList = list.ToList();

            return Json(OutList , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
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

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soato_code"></param>
        /// <param name="year"></param>
        /// <returns></returns>
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

            return Json(O.ToList(), JsonRequestBehavior.AllowGet);
        }


        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult SpisokPravsoyuzovOrg()
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

            return Json(O.ToList(), JsonRequestBehavior.AllowGet);
        }

        #region ECP_Transaction Managemant


        [OutputCache(Duration = 5, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public ActionResult EcpManagementV()
        {
            return View("EcpManagement");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        public JsonResult GetEcpTransactions(string inn)
        {
            log.Info("GetEcpTransactions => " + inn);
            var UserId = Convert.ToInt32(User.Identity.Name);
            var userInfo = UserInfo.GetUserInfo(UserId);

            var model = new EcpViewModel();
            model.obl_department_base = userInfo.DepartmentBase;
            model.inn = inn;

            var list = EcpManagement.GetEcpPaymentList(model, KasabaDB);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> ModifyEcpTransaction(EcpViewModel model)
        {
            var UserId = Convert.ToInt32(User.Identity.Name);
            var userInfo = UserInfo.GetUserInfo(UserId);

            try
            {
                EcpManagement.ModifyPaymentData(model, KasabaDB);

                await KasabaDB.SaveChangesAsync();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}