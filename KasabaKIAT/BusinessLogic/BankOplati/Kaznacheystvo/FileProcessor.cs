using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using KasabaKIAT.ViewModel.BankOplati;

namespace KasabaKIAT.BusinessLogic.BankOplati.Kaznacheystvo
{
    public class FileProcessor
    {
        private PaymentFileData paymentFile;

        public FileProcessor()
        {
            paymentFile = new PaymentFileData();
        }

        public void Process(string filePath)
        {
            if (File.Exists(filePath))
            {
                var fileName = Path.GetFileName(filePath);

                paymentFile.FileName = fileName;

                string[] readText = File.ReadAllLines(filePath);
                foreach (string s in readText)
                {
                    var kazRow = ParseLine(s);
                    paymentFile.AddPaymentRow(kazRow);
                }


            }
        }

        private KaznaPaymentFile ParseLine(string line)
        {
            KaznaPaymentFile pLine = null;

            if (!string.IsNullOrEmpty(line))
            {
                Char delimiter = (char)29;

                String[] substrings = line.Split(delimiter);

                if (substrings.Length == 20)
                {
                    pLine = new KaznaPaymentFile();
                    pLine.k_type        = substrings[0];
                    pLine.k_operday     = substrings[1];
                    pLine.k_id          = substrings[2];
                    pLine.k_bankid      = substrings[3];
                    pLine.k_klsdox      = substrings[4];
                    pLine.k_docnumb     = substrings[5];
                    pLine.k_docdate     = substrings[6];
                    pLine.k_doctype     = substrings[7];
                    pLine.k_sumpay      = substrings[8];
                    pLine.k_clmfo       = substrings[9];
                    pLine.k_clacc       = substrings[10];
                    pLine.k_clinn       = substrings[11];
                    pLine.k_clname      = substrings[12];
                    pLine.k_comfo       = substrings[13];
                    pLine.k_coacc       = substrings[14];
                    pLine.k_coinn       = substrings[15];
                    pLine.k_coname      = substrings[16];
                    pLine.k_purpose     = substrings[17];
                    pLine.k_chapter     = substrings[18];
                    pLine.k_okonh = substrings[19];
                }
            }

            return pLine;
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