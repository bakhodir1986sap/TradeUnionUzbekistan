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
using VipiskaParser.Controller;
using VipiskaParser.Model;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.Chastichka.Federasiya
{
    public class ChastFedController
    {
        public static List<BankpaymentsViewModel> ProcessV1(Stream InputStream)
        {
            var platList = new List<BankpaymentsViewModel>();

            using (var package = new ExcelPackage(InputStream))
            {
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                //var noOfCol = workSheet.Dimension.End.Column;
                //var noOfRow = workSheet.Dimension.End.Row;

                List<RowInput> tempData = new List<RowInput>();

                List<InputRow> resultList = new List<InputRow>();

                int startRow = 9;
                int totalRowsCounter = 0;

                while (true)
                {
                    totalRowsCounter++;
                    var tempRow = new RowInput();

                    if (workSheet.Cells[startRow, 1] != null && workSheet.Cells[startRow, 1].Value != null)
                    {
                        tempRow.Field01 = workSheet.Cells[startRow, 1].Value.ToString();
                    }
                    else
                    {
                        if (startRow == 9)
                        {
                            startRow++;
                            if (workSheet.Cells[startRow, 1] != null && workSheet.Cells[startRow, 1].Value != null)
                            {
                                tempRow.Field01 = workSheet.Cells[startRow, 1].Value.ToString();
                            }
                        }
                    }

                    if (!(totalRowsCounter % 3 == 0))
                    {
                        if (string.IsNullOrEmpty(tempRow.Field01))
                        {
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(tempRow.Field01))
                    {
                        if (tempRow.Field01.Equals("Сумма оборотов"))
                        {
                            break;
                        }
                    }

                    if (workSheet.Cells[startRow, 2] != null && workSheet.Cells[startRow, 2].Value != null)
                    {
                        tempRow.Field02 = workSheet.Cells[startRow, 2].Value.ToString();
                    }

                    if (workSheet.Cells[startRow, 3] != null && workSheet.Cells[startRow, 3].Value != null)
                    {
                        tempRow.Field03 = workSheet.Cells[startRow, 3].Value.ToString();
                    }

                    if (workSheet.Cells[startRow, 4] != null && workSheet.Cells[startRow, 4].Value != null)
                    {
                        tempRow.Field04 = workSheet.Cells[startRow, 4].Value.ToString();
                    }

                    if (workSheet.Cells[startRow, 4] != null && workSheet.Cells[startRow, 4].Value != null)
                    {
                        tempRow.Field04 = workSheet.Cells[startRow, 4].Value.ToString();
                    }

                    if (workSheet.Cells[startRow, 5] != null && workSheet.Cells[startRow, 5].Value != null)
                    {
                        tempRow.Field05 = workSheet.Cells[startRow, 5].Value.ToString();
                    }

                    if (workSheet.Cells[startRow, 6] != null && workSheet.Cells[startRow, 6].Value != null)
                    {
                        tempRow.Field06 = workSheet.Cells[startRow, 6].Value.ToString();
                    }

                    tempData.Add(tempRow);

                    if (totalRowsCounter % 3 == 0)
                    {
                        InputRow model = new InputRow();
                        model.DateStr = tempData[0].Field01;
                        model.TimeStr = tempData[1].Field01;
                        model.DocumentNumber = tempData[0].Field02;
                        model.Operation = tempData[0].Field03;

                        model.detailsRow1 = tempData[0].Field04;
                        model.detailsRow2 = tempData[1].Field04;
                        model.detailsRow3 = tempData[2].Field04;

                        model.Debit = tempData[0].Field05;
                        model.Credit = tempData[0].Field06;

                        Console.WriteLine(string.Format("DateStr = {0}", model.DateStr));
                        Console.WriteLine(string.Format("TimeStr = {0}", model.TimeStr));
                        Console.WriteLine(string.Format("DocumentNumber = {0}", model.DocumentNumber));
                        Console.WriteLine(string.Format("detailsRow1 = {0}", model.detailsRow1));
                        Console.WriteLine(string.Format("detailsRow2 = {0}", model.detailsRow2));
                        Console.WriteLine(string.Format("detailsRow3 = {0}", model.detailsRow3));
                        Console.WriteLine(string.Format("Debit = {0}", model.Debit));
                        Console.WriteLine(string.Format("Credit = {0}", model.Credit));
                        Console.WriteLine("========================================================== ");
                        Console.WriteLine(startRow);

                        resultList.Add(model);

                        tempData.Clear();
                    }

                    startRow++;
                }

                platList = ConvertRow2KaznaData.Process(resultList);

                #region CommentedCode
                //for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                //{
                //    try
                //    {
                //        var user = new BankpaymentsViewModel();

                //        user.num_doc              = workSheet.Cells[rowIterator, 1].Value.ToString();
                //        user.payer_name           = workSheet.Cells[rowIterator, 2].Value.ToString();
                //        user.payer_inn            = workSheet.Cells[rowIterator, 3].Value.ToString();
                //        user.payer_mfo            = workSheet.Cells[rowIterator, 4].Value.ToString();
                //        user.payer_account        = workSheet.Cells[rowIterator, 5].Value.ToString();
                //        DateTime trandate;
                //        string DateOfPayment = workSheet.Cells[rowIterator, 6].Value.ToString();

                //        if (DateOfPayment.Length > 10)
                //        {
                //            DateOfPayment = DateOfPayment.Substring(0, 10);
                //        }

                //        if (DateTime.TryParseExact(DateOfPayment, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out trandate))
                //        {
                //            user.tran_date = trandate;
                //        }
                //        else
                //        {
                //            user.tran_date = DateTime.Now;

                //        }
                //        user.payment_datail = workSheet.Cells[rowIterator, 7].Value.ToString();
                //        decimal amount = 0;

                //        if (decimal.TryParse(workSheet.Cells[rowIterator, 8].Value.ToString(), out amount))
                //        {
                //            user.amount = amount;
                //        }
                //        else
                //        {
                //            user.amount = 0;

                //        }
                //        user.benef_name       = workSheet.Cells[rowIterator, 9].Value.ToString();
                //        user.benef_inn        = workSheet.Cells[rowIterator, 10].Value.ToString();
                //        user.benef_mfo        = workSheet.Cells[rowIterator, 11].Value.ToString();
                //        user.benef_account    = workSheet.Cells[rowIterator, 12].Value.ToString();
                //        user.dbcrflg          = workSheet.Cells[rowIterator, 13].Value.ToString();

                //        platList.Add(user);
                //    }
                //    catch (Exception ex)
                //    {
                //        throw ex;
                //    }

                //}
                #endregion
            }

            return platList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<BankpaymentsViewModel> Process(string filePath)
        {
            var platList = new List<BankpaymentsViewModel>();

            var sheet = GetFileStream(filePath);

                List<RowInput> tempData = new List<RowInput>();

                List<InputRow> resultList = new List<InputRow>();

                int startRow = 9;
                int totalRowsCounter = 0;

                while (true)
                {
                    totalRowsCounter++;
                    var tempRow = new RowInput();

                    if (sheet.GetRow(startRow).GetCell(0).StringCellValue != null)
                    {
                        tempRow.Field01 = sheet.GetRow(startRow).GetCell(0).StringCellValue;
                    }
                    else
                    {
                        if (startRow == 9)
                        {
                            startRow++;
                            if (sheet.GetRow(startRow).GetCell(0).StringCellValue != null)
                            {
                                tempRow.Field01 = sheet.GetRow(startRow).GetCell(0).StringCellValue;
                            }
                        }
                    }

                    if (!(totalRowsCounter % 3 == 0))
                    {
                        if (string.IsNullOrEmpty(tempRow.Field01))
                        {
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(tempRow.Field01))
                    {
                        if (tempRow.Field01.Equals("Сумма оборотов"))
                        {
                            break;
                        }
                    }

                    if (sheet.GetRow(startRow).GetCell(1).StringCellValue != null)
                    {
                        tempRow.Field02 = sheet.GetRow(startRow).GetCell(1).StringCellValue;
                    }

                    if (sheet.GetRow(startRow).GetCell(2).StringCellValue != null)
                    {
                        tempRow.Field03 = sheet.GetRow(startRow).GetCell(2).StringCellValue;
                    }

                    if (sheet.GetRow(startRow).GetCell(3).StringCellValue != null)
                    {
                        tempRow.Field04 = sheet.GetRow(startRow).GetCell(3).StringCellValue;
                    }

                    try
                    {
                        if (sheet.GetRow(startRow).GetCell(4).StringCellValue != null)
                        {
                            tempRow.Field05 = sheet.GetRow(startRow).GetCell(4).StringCellValue.ToString();
                        }
                    }
                    catch
                    {
                        if (sheet.GetRow(startRow).GetCell(4).NumericCellValue.ToString() != null)
                        {
                            tempRow.Field05 = sheet.GetRow(startRow).GetCell(4).NumericCellValue.ToString();
                        }
                    }

                    try
                    {
                        if (sheet.GetRow(startRow).GetCell(5).StringCellValue != null)
                        {
                            tempRow.Field06 = sheet.GetRow(startRow).GetCell(5).StringCellValue.ToString();
                        }
                    }
                    catch
                    {
                        if (sheet.GetRow(startRow).GetCell(5).NumericCellValue.ToString() != null)
                        {
                            tempRow.Field06 = sheet.GetRow(startRow).GetCell(5).NumericCellValue.ToString();
                        }
                    }

                    tempData.Add(tempRow);

                    if (totalRowsCounter % 3 == 0)
                    {
                        InputRow model = new InputRow();
                        model.DateStr = tempData[0].Field01;
                        model.TimeStr = tempData[1].Field01;
                        model.DocumentNumber = tempData[0].Field02;
                        model.Operation = tempData[0].Field03;

                        model.detailsRow1 = tempData[0].Field04;
                        model.detailsRow2 = tempData[1].Field04;
                        model.detailsRow3 = tempData[2].Field04;

                        model.Debit = tempData[0].Field05;
                        model.Credit = tempData[0].Field06;

                        Console.WriteLine(string.Format("DateStr = {0}", model.DateStr));
                        Console.WriteLine(string.Format("TimeStr = {0}", model.TimeStr));
                        Console.WriteLine(string.Format("DocumentNumber = {0}", model.DocumentNumber));
                        Console.WriteLine(string.Format("detailsRow1 = {0}", model.detailsRow1));
                        Console.WriteLine(string.Format("detailsRow2 = {0}", model.detailsRow2));
                        Console.WriteLine(string.Format("detailsRow3 = {0}", model.detailsRow3));
                        Console.WriteLine(string.Format("Debit = {0}", model.Debit));
                        Console.WriteLine(string.Format("Credit = {0}", model.Credit));
                        Console.WriteLine("========================================================== ");
                        Console.WriteLine(startRow);

                        if (!model.detailsRow1.Contains("23402000300100001010"))
                        {
                            resultList.Add(model);
                        }

                        tempData.Clear();
                    }

                    startRow++;
                }

                platList = ConvertRow2KaznaData.Process(resultList);

            return platList;
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