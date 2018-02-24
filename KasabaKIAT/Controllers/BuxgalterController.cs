using KasabaKIAT.ViewModel.Buxgalteriya;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KasabaKIAT.Controllers
{
    //[Authorize(Roles = "ADMIN")]
    [Authorize]
    public class BuxgalterController : Controller
    {
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

        public ActionResult Upload(FormCollection formCollection)
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

                            user.num_doc        = workSheet.Cells[rowIterator, 1].Value.ToString();
                            user.payer_name     = workSheet.Cells[rowIterator, 1].Value.ToString();
                            user.payer_inn      = workSheet.Cells[rowIterator, 1].Value.ToString();
                            user.payer_mfo      = workSheet.Cells[rowIterator, 1].Value.ToString();
                            user.payer_account  = workSheet.Cells[rowIterator, 1].Value.ToString();
                            //user.tran_date      = workSheet.Cells[rowIterator, 1].Value.ToString();
                            user.payment_datail = workSheet.Cells[rowIterator, 1].Value.ToString();
                            //user.amount         = workSheet.Cells[rowIterator, 1].Value.ToString();
                            user.benef_name     = workSheet.Cells[rowIterator, 1].Value.ToString();
                            user.benef_inn      = workSheet.Cells[rowIterator, 1].Value.ToString();
                            user.benef_mfo      = workSheet.Cells[rowIterator, 1].Value.ToString();
                            user.benef_account  = workSheet.Cells[rowIterator, 1].Value.ToString();
                            user.dbcrflg        = workSheet.Cells[rowIterator, 1].Value.ToString();

                            platList.Add(user);
                        }
                    }
                }
            }
            return View("Index");
        }
    }
}