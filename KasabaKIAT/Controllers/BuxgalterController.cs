using KasabaKIAT.BusinessLogic.Buxgalteriya.PrixodChastichka;
using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Buxgalteriya;
using log4net;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KasabaKIAT.Controllers
{
    //[Authorize(Roles = "ADMIN")]
    [Authorize]
    public class BuxgalterController : Controller
    {
        ILog log = log4net.LogManager.GetLogger(typeof(BuxgalterController));
        KasabaKIATEntities KasabaDB = new KasabaKIATEntities();
        // GET: Buxgalter
        public ActionResult Index()
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
        public ActionResult OsushestvleniyeProvodki()
        {
            return View();
        }
        public ActionResult Nakladniye()
        {
            return View();
        }
        public ActionResult RaspredleniyeByudjeta()
        {
            return View();
        }
        public ActionResult PlanByudjetaOblastey()
        {
            return View();
        }
        public ActionResult PlanByudjetaOblasteyNoviy()
        {
            return View();
        }
        public ActionResult PlanByudjetaOblasteyIzmenit()
        {
            return View();
        }

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
    }
}