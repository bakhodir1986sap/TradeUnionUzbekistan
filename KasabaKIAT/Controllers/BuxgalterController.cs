using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using LinqToExcel;

namespace KasabaKIAT.Controllers
{
    //[Authorize(Roles = "ADMIN")]
    [Authorize]
    public class BuxgalterController : Controller
    {
        // GET: Buxgalter
        KasabaKIATEntities KasabaDB = new KasabaKIATEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadFile(tbgl_bankpayments users, HttpPostedFileBase FileUpload)
        {
            List<string> data = new List<string>();
            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {


                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/Doc/");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    var connectionString = "";
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];

                    string sheetName = "Sheet1";

                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var artistAlbums = from a in excelFile.Worksheet<tbgl_bankpayments>(sheetName) select a;

                    foreach (var a in artistAlbums)
                    {
                        try
                        {
                            if (a.AMOUNT != 0 && a.BENEF_ACCOUNT != "" && a.BENEF_INN!= "")
                            {
                                tbgl_bankpayments TU = new tbgl_bankpayments();
                                TU.NUM_DOC = a.NUM_DOC;
                                TU.PAYER_NAME = a.PAYER_NAME;
                                TU.PAYER_INN = a.PAYER_INN;
                                KasabaDB.tbgl_bankpayments.Add(TU);

                                KasabaDB.SaveChanges();



                            }
                            else
                            {
                                data.Add("<ul>");
                                if (a.Name == "" || a.Name == null) data.Add("<li> name is required</li>");
                                if (a.Address == "" || a.Address == null) data.Add("<li> Address is required</li>");
                                if (a.ContactNo == "" || a.ContactNo == null) data.Add("<li>ContactNo is required</li>");

                                data.Add("</ul>");
                                data.ToArray();
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                        }

                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {

                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {

                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                }

                            }
                        }
                    }
                    //deleting excel file from folder  
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //alert message for invalid file format  
                    data.Add("<ul>");
                    data.Add("<li>Only Excel file format is allowed</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.Add("<ul>");
                if (FileUpload == null) data.Add("<li>Please choose Excel file</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            //if (excelfile==null||excelfile.ContentLength == 0)
            //{
            //    ViewBag.error = "Please select a excel file<br>";
            //    return View("VvodDannix");
            //}
            //else
            //{
            //    if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
            //    {
            //        return View("Success");
            //    }
            //    else
            //    {
            //        ViewBag.error = "File type is incorrect<br>";
            //        return View("VvodDannix");
            //    }
            //}

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
    }
}