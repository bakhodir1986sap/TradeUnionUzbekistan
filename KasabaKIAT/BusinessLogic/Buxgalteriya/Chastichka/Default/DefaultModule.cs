using KasabaKIAT.ViewModel.Buxgalteriya;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.Chastichka.Default
{
    public class DefaultModule
    {
        public static List<BankpaymentsViewModel> ProcessV1(Stream InputStream)
        {
            var platList = new List<BankpaymentsViewModel>();

            using (var package = new ExcelPackage(InputStream))
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
                        throw ex;
                    }

                }
            }

            return platList;
        }


        public static List<BankpaymentsViewModel> ProcessV2(string filePath)
        {
            var platList = new List<BankpaymentsViewModel>();

            var sheet = GetFileStream(filePath);

                for (int rowIterator = 2; rowIterator <= sheet.LastRowNum; rowIterator++)
                {
                    try
                    {
                        var user = new BankpaymentsViewModel();

                        user.num_doc = sheet.GetRow(rowIterator).GetCell(1).StringCellValue;//workSheet.Cells[rowIterator, 1].Value.ToString();
                        user.payer_name     = sheet.GetRow(rowIterator).GetCell(2).StringCellValue;//workSheet.Cells[rowIterator, 2].Value.ToString();
                        user.payer_inn      = sheet.GetRow(rowIterator).GetCell(3).StringCellValue;//workSheet.Cells[rowIterator, 3].Value.ToString();
                        user.payer_mfo      = sheet.GetRow(rowIterator).GetCell(4).StringCellValue;//workSheet.Cells[rowIterator, 4].Value.ToString();
                        user.payer_account  = sheet.GetRow(rowIterator).GetCell(5).StringCellValue;//workSheet.Cells[rowIterator, 5].Value.ToString();
                        DateTime trandate;

                        string DateOfPayment = sheet.GetRow(rowIterator).GetCell(6).StringCellValue;//workSheet.Cells[rowIterator, 6].Value.ToString();

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
                        user.payment_datail = sheet.GetRow(rowIterator).GetCell(7).StringCellValue;//workSheet.Cells[rowIterator, 7].Value.ToString();
                        decimal amount = 0;

                        if (decimal.TryParse(sheet.GetRow(rowIterator).GetCell(8).StringCellValue, out amount))
                        {
                            user.amount = amount;
                        }
                        else
                        {
                            user.amount = 0;

                        }
                        user.benef_name     = sheet.GetRow(rowIterator).GetCell(9 ).StringCellValue;//workSheet.Cells[rowIterator, 9 ].Value.ToString();
                        user.benef_inn      = sheet.GetRow(rowIterator).GetCell(10).StringCellValue;//workSheet.Cells[rowIterator, 10].Value.ToString();
                        user.benef_mfo      = sheet.GetRow(rowIterator).GetCell(11).StringCellValue;//workSheet.Cells[rowIterator, 11].Value.ToString();
                        user.benef_account  = sheet.GetRow(rowIterator).GetCell(12).StringCellValue;//workSheet.Cells[rowIterator, 12].Value.ToString();
                        user.dbcrflg        = sheet.GetRow(rowIterator).GetCell(13).StringCellValue;//workSheet.Cells[rowIterator, 13].Value.ToString();

                        platList.Add(user);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }

            return platList;
        }


        public static List<BankpaymentsViewModel> Process(string filePath)
        {
            var platList = new List<BankpaymentsViewModel>();

            var sheet = GetFileStream(filePath);

            for (int rowIterator = 1; rowIterator <= sheet.LastRowNum; rowIterator++)
            {
                try
                {
                    var user = new BankpaymentsViewModel();

                    user.num_doc = sheet.GetRow(rowIterator).GetCell(1).StringCellValue;//workSheet.Cells[rowIterator, 1].Value.ToString();

                    user.payer_name = sheet.GetRow(rowIterator).GetCell(3).StringCellValue;//workSheet.Cells[rowIterator, 2].Value.ToString();
                    user.payer_inn = sheet.GetRow(rowIterator).GetCell(4).StringCellValue.Substring(0, 9);//workSheet.Cells[rowIterator, 3].Value.ToString();
                    user.payer_mfo = sheet.GetRow(rowIterator).GetCell(5).StringCellValue.Substring(0, 5);//workSheet.Cells[rowIterator, 4].Value.ToString();
                    user.payer_account = sheet.GetRow(rowIterator).GetCell(6).StringCellValue.Substring(0, 20);//workSheet.Cells[rowIterator, 5].Value.ToString();
                    DateTime trandate;

                    string DateOfPayment = string.Empty;

                    try
                    {
                        DateOfPayment = sheet.GetRow(rowIterator).GetCell(7).NumericCellValue.ToString();//workSheet.Cells[rowIterator, 6].Value.ToString();
                    }
                    catch
                    {
                        DateOfPayment = sheet.GetRow(rowIterator).GetCell(7).StringCellValue;
                    }
                        
                        

                    if (DateOfPayment.Length > 10)
                    {
                        DateOfPayment = DateOfPayment.Substring(0, 10);
                    }

                    if (DateTime.TryParseExact(DateOfPayment, "dd.MM.yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out trandate))
                    {
                        user.tran_date = trandate;
                    }
                    else
                    {
                        user.tran_date = DateTime.Now;

                    }
                    user.payment_datail = sheet.GetRow(rowIterator).GetCell(8).StringCellValue;//workSheet.Cells[rowIterator, 7].Value.ToString();

                    decimal amount = 0;

                    try
                    {
                        if (sheet.GetRow(rowIterator).GetCell(9).StringCellValue != null)
                        {
                            var res = CorrectAmount(sheet.GetRow(rowIterator).GetCell(9).StringCellValue.ToString());

                            if (decimal.TryParse(res, out amount))
                            {
                                user.amount = amount;
                            }
                            else
                            {
                                user.amount = 0;

                            }
                        }
                    }
                    catch
                    {
                        if (sheet.GetRow(rowIterator).GetCell(9).NumericCellValue.ToString() != null)
                        {
                            var res = CorrectAmount(sheet.GetRow(rowIterator).GetCell(9).NumericCellValue.ToString());

                            if (decimal.TryParse(res, out amount))
                            {
                                user.amount = amount;
                            }
                            else
                            {
                                user.amount = 0;

                            }
                        }
                    }

                    user.benef_name = sheet.GetRow(rowIterator).GetCell(10).StringCellValue;//workSheet.Cells[rowIterator, 9 ].Value.ToString();
                    user.benef_inn = sheet.GetRow(rowIterator).GetCell(11).StringCellValue;//workSheet.Cells[rowIterator, 10].Value.ToString();
                    user.benef_mfo = sheet.GetRow(rowIterator).GetCell(12).StringCellValue.Substring(0, 5);//workSheet.Cells[rowIterator, 11].Value.ToString();
                    user.benef_account = sheet.GetRow(rowIterator).GetCell(13).StringCellValue.Substring(0, 20);//workSheet.Cells[rowIterator, 12].Value.ToString();
                    user.dbcrflg = "C";//workSheet.Cells[rowIterator, 13].Value.ToString();

                    platList.Add(user);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return platList;
        }

        public static string CorrectAmount(string inputAmount)
        {
            if (!inputAmount.Any(z => z == '.'))
            {
                if (inputAmount.Any(z => z == ','))
                {
                    return inputAmount;
                }
            }

            return inputAmount.Replace(",", "").Replace(".", ",");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullFilePath"></param>
        /// <returns></returns>
        private static ISheet GetFileStream(string fullFilePath)
        {
            var fileExtension = Path.GetExtension(fullFilePath);
            string sheetName;
            ISheet sheet = null;
            switch (fileExtension)
            {
                case ".xlsx":
                    using (var fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var wb = new XSSFWorkbook(fs);
                        sheetName = wb.GetSheetAt(0).SheetName;
                        sheet = (XSSFSheet)wb.GetSheet(sheetName);
                    }
                    break;
                case ".xls":
                    using (var fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var wb = new HSSFWorkbook(fs);
                        sheetName = wb.GetSheetAt(0).SheetName;
                        sheet = (HSSFSheet)wb.GetSheet(sheetName);
                    }
                    break;
            }
            return sheet;
        }
    }
}