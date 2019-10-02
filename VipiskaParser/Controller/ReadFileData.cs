using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VipiskaParser.Model;
using Excel = Microsoft.Office.Interop.Excel;

namespace VipiskaParser.Controller
{
    public class ReadFileData
    {
        public static void ReadAllVipiskaFiles()
        {
            //Files
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Files");

            var files = Directory.GetFiles(path, "выписка*.xls*");

            foreach(var spath in files)
            {
                Process(spath);

                Console.WriteLine("Продолжить прочитывать следующий файл? Y/N");

                string res = Console.ReadLine();

                if (!res.Equals("Y"))
                {
                    break;
                }
            }
        }

        public static void Process(string path)
        {
            try
            {
                List<RowInput> tempData = new List<RowInput>();

                List<InputRow> resultList = new List<InputRow>();

                Excel.Application xlApp = new Excel.Application();

                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);
                // @"D:\RAD SOFT\Projects\PravSouz\FederasiyaExcelParser\IN\test.xls"

                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];

                Excel.Range xlRange = xlWorksheet.UsedRange;

                int startRow = 9;
                int totalRowsCounter = 0;

                while (true)
                {
                    totalRowsCounter++;
                    var tempRow = new RowInput();

                    if (xlRange.Cells[startRow, 1] != null && xlRange.Cells[startRow, 1].Value2 != null)
                    {
                        tempRow.Field01 = xlRange.Cells[startRow, 1].Value2.ToString();
                    }
                    else
                    {
                        if (startRow == 9)
                        {
                            startRow++;
                            if (xlRange.Cells[startRow, 1] != null && xlRange.Cells[startRow, 1].Value2 != null)
                            {
                                tempRow.Field01 = xlRange.Cells[startRow, 1].Value2.ToString();
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

                    if (xlRange.Cells[startRow, 2] != null && xlRange.Cells[startRow, 2].Value2 != null)
                    {
                        tempRow.Field02 = xlRange.Cells[startRow, 2].Value2.ToString();
                    }

                    if (xlRange.Cells[startRow, 3] != null && xlRange.Cells[startRow, 3].Value2 != null)
                    {
                        tempRow.Field03 = xlRange.Cells[startRow, 3].Value2.ToString();
                    }

                    if (xlRange.Cells[startRow, 4] != null && xlRange.Cells[startRow, 4].Value2 != null)
                    {
                        tempRow.Field04 = xlRange.Cells[startRow, 4].Value2.ToString();
                    }

                    if (xlRange.Cells[startRow, 4] != null && xlRange.Cells[startRow, 4].Value2 != null)
                    {
                        tempRow.Field04 = xlRange.Cells[startRow, 4].Value2.ToString();
                    }

                    if (xlRange.Cells[startRow, 5] != null && xlRange.Cells[startRow, 5].Value2 != null)
                    {
                        tempRow.Field05 = xlRange.Cells[startRow, 5].Value2.ToString();
                    }

                    if (xlRange.Cells[startRow, 6] != null && xlRange.Cells[startRow, 6].Value2 != null)
                    {
                        tempRow.Field06 = xlRange.Cells[startRow, 6].Value2.ToString();
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

                var KaznaList = ConvertRow2KaznaData.Process(resultList);
                var chastichkaList = ConvertRow2KaznaData.chastichka;
                var otherList = ConvertRow2KaznaData.other;
                ConvertRow2KaznaData.GroupKaznaAndChastichka();
                var KaznaListGr = ConvertRow2KaznaData.resListGrouped;
                var ChastGr = ConvertRow2KaznaData.chastichkaGrouped;

                Export2Excel();

                GC.Collect();
                GC.WaitForPendingFinalizers();

                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);

                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkbook);
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error!!!");
                Console.WriteLine(ex.Message);
            }

        }



        public static void Export2Excel()
        {

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;

            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);

            try
            {
                xlWorkSheet = xlWorkBook.Sheets[1];
                xlWorkSheet.Name = "Vipiska_Analiz";

                ///
                xlWorkSheet.Columns[1].ColumnWidth = 30;
                xlWorkSheet.Cells[1, 1] = "Количество оборотов ";
                xlWorkSheet.Cells[1, 2] = ConvertRow2KaznaData.totalAborot;
                xlWorkSheet.Cells[2, 1] = "Общая сумма по Дебиту   ";
                xlWorkSheet.Cells[2, 2] = ConvertRow2KaznaData.totalAmountDebit;
                xlWorkSheet.Cells[3, 1] = "Общая сумма по Кредиту   ";
                xlWorkSheet.Cells[3, 2] = ConvertRow2KaznaData.totalAmount;
                xlWorkSheet.Cells[4, 1] = "Сумма от Казначейства   ";
                xlWorkSheet.Cells[4, 2] = ConvertRow2KaznaData.KaznaAmount;
                xlWorkSheet.Cells[5, 1] = "Сумма по Частичным оплатам   ";
                xlWorkSheet.Cells[5, 2] = ConvertRow2KaznaData.chatichka;
                xlWorkSheet.Cells[6, 1] = "Другие суммы   ";
                xlWorkSheet.Cells[6, 2] = ConvertRow2KaznaData.OtherAmount;

                //Поступление по Областьям от Казначейства
                int startRow = 10;
                xlWorkSheet.Cells[startRow, 1] = "Поступление по Областьям от Казначейства";

                if (ConvertRow2KaznaData.KaznaAmount > 0)
                {
                    int counter1 = 0;
                    foreach (var obj1 in ConvertRow2KaznaData.resListGrouped)
                    {
                        xlWorkSheet.Cells[startRow + 2 + counter1, 1] = obj1.Oblast;
                        xlWorkSheet.Cells[startRow + 2 + counter1, 2] = obj1.Amount;
                        counter1++;
                    }

                    startRow += 20;
                }

                xlWorkSheet.Cells[startRow, 1] = "Поступление по Областьям по Частичным оплатам";

                if (ConvertRow2KaznaData.chatichka > 0)
                {
                    int counter2 = 0;
                    foreach (var obj in ConvertRow2KaznaData.chastichkaGrouped)
                    {
                        xlWorkSheet.Cells[startRow + 2 + counter2, 1] = obj.Oblast;
                        xlWorkSheet.Cells[startRow + 2 + counter2, 2] = obj.Amount;
                        counter2++;
                    }

                    startRow += 20;
                }


                xlWorkSheet.Cells[startRow, 1] = "Поступление по Прочим оплатам";
                if (ConvertRow2KaznaData.OtherAmount > 0)
                {
                    int counter3 = 0;
                    foreach (var obj in ConvertRow2KaznaData.other)
                    {
                        xlWorkSheet.Cells[startRow + 2 + counter3, 1] = obj.Oblast;
                        xlWorkSheet.Cells[startRow + 2 + counter3, 2] = obj.Amount;
                        xlWorkSheet.Cells[startRow + 2 + counter3, 3] = obj.Rayon;
                        counter3++;
                    }

                    startRow += 20;
                }

                xlWorkSheet.Cells[startRow, 1] = "Дебитовой оборот";
                if (ConvertRow2KaznaData.totalAmountDebit > 0)
                {
                    int counter4 = 0;
                    foreach (var obj in ConvertRow2KaznaData.debitOborot)
                    {
                        xlWorkSheet.Cells[startRow + 2 + counter4, 1] = obj.Oblast;
                        xlWorkSheet.Cells[startRow + 2 + counter4, 2] = obj.Amount;
                        xlWorkSheet.Cells[startRow + 2 + counter4, 3] = obj.Rayon;
                        counter4++;
                    }
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), "Files" , string.Format("{0}-{1}","Analiz_Vipiska", DateTime.Now.ToString("yyyyMMddhhmmss")));
                xlWorkBook.SaveAs(path);
                xlApp.Quit();

                try
                {
                    System.Diagnostics.Process.Start(string.Format("{0}.xls", path));
                }
                catch
                {
                }

                try
                {
                    System.Diagnostics.Process.Start(string.Format("{0}.xlsx", path));
                }
                catch
                {
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //xlApp.Quit();
                xlWorkBook = null;
                xlApp = null;
            }

        }
    }
}
