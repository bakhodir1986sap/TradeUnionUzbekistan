using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using KasabaKIAT.ViewModel.BankOplati;
using System.Text;
using KaznaFilesImporter.BankOplati;
using Epigov.Log;

namespace KasabaKIAT.BusinessLogic.BankOplati.Kaznacheystvo
{
    public class FileProcessor
    {
        private PaymentFileData paymentFile;
        private static ILogService _logService;

        public FileProcessor()
        {
            _logService = new FileLogService(typeof(FileProcessor));
            paymentFile = new PaymentFileData();
        }

        public void Process(string filePath, string arxivePath)
        {
            if (Directory.Exists(filePath))
            {
                DirectoryInfo d = new DirectoryInfo(filePath);//Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles("PAYINFPRF*"); //Getting Text files
                foreach (FileInfo file in Files)
                {
                    try
                    {
                        if (File.Exists(file.FullName))
                        {
                            _logService.Info(file.FullName);

                            var fileName = Path.GetFileName(file.FullName);

                            paymentFile.FileName = fileName;

                            Console.WriteLine(Path.GetFileName(fileName));

                            Encoding en = Encoding.GetEncoding(1251, new EncoderReplacementFallback(" "), new DecoderReplacementFallback(" "));

                            string[] readText = File.ReadAllLines(file.FullName, en);

                            Console.WriteLine("Rows count: " + readText.Count());
                            _logService.Info("Rows count: " + readText.Count());

                            paymentFile.ClearPayment();

                            foreach (string s in readText)
                            {
                                var kazRow = ParseLine(s);
                                paymentFile.AddPaymentRow(kazRow);
                            }

                            var fileData = FileData;

                            if (fileData != null)
                            {
                                fileData.SaveData();
                            }
                        }

                        var arxiveFileName = Path.Combine(arxivePath, Path.GetFileName(file.FullName));

                        if (!File.Exists(arxiveFileName))
                        {
                            _logService.Info("MoveTo");
                            file.MoveTo(arxiveFileName);
                        }
                        else
                        {
                            _logService.Info("Delete");
                            file.Delete();
                        }
                    }
                    catch(Exception ex)
                    {
                        _logService.Info(ex.ToString());
                        Console.WriteLine(ex.ToString());
                    }
                }

                KaznaFilesDataManager.EnterNotExistsOrganizations();
            }
        }

        private BaseKaznaFile ParseLine(string line)
        {
            BaseKaznaFile retLine = null;
            KaznaPaymentFile pLine = null;
            SverkaFile sLine = null;

            if (!string.IsNullOrEmpty(line))
            {
                Char delimiter = (char)29;

                String[] substrings = line.Split(delimiter);

                if (substrings[0].Equals("1"))
                {
                    if (substrings.Length >= 20)
                    {
                        pLine = new KaznaPaymentFile();
                        pLine.k_type = substrings[0];
                        pLine.k_operday = substrings[1];
                        pLine.k_id = substrings[2];
                        pLine.k_bankid = substrings[3];
                        pLine.k_klsdox = substrings[4];
                        pLine.k_docnumb = substrings[5];
                        pLine.k_docdate = substrings[6];
                        pLine.k_doctype = substrings[7];
                        pLine.k_sumpay = substrings[8];
                        pLine.k_clmfo = substrings[9];
                        pLine.k_clacc = substrings[10];
                        pLine.k_clinn = substrings[11];
                        pLine.k_clname = substrings[12];
                        pLine.k_comfo = substrings[13];
                        pLine.k_coacc = substrings[14];
                        pLine.k_coinn = substrings[15];
                        pLine.k_coname = substrings[16];
                        pLine.k_purpose = substrings[17];
                        pLine.k_chapter = substrings[18];
                        pLine.k_okonh = substrings[19];
                    }
                }
                else if (substrings[0].Equals("2"))
                {
                    if (substrings.Length >= 4)
                    {
                        sLine = new SverkaFile();
                        sLine.k_type = substrings[0];
                        sLine.k_operday = substrings[1];
                        sLine.file_name = substrings[2];
                        sLine.create_datetime = substrings[3];
                    }
                }
            }

            if (pLine != null)
            {
                retLine = pLine;
            }
            else if (sLine != null)
            {
                retLine = sLine;
            }
            
            return retLine;
        }

        public PaymentFileData FileData
        {
            get
            {
                if (paymentFile != null)
                {
                    return paymentFile;
                }

                return null;
            }
        }
    }
}