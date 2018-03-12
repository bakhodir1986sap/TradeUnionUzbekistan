using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KasabaKIAT.ViewModel.BankOplati;
using KasabaKIAT.BusinessLogic.BankOplati;
using KaznaFilesImporter.BankOplati;

namespace KasabaKIAT.BusinessLogic.BankOplati.Kaznacheystvo
{
    public class PaymentFileData
    {
        private KaznaFiles file;
        private List<KaznaPaymentFile> paymentRows;
        private List<SverkaFile> sverkaFile;

        public PaymentFileData()
        {
            file = new KaznaFiles();

            paymentRows = new List<KaznaPaymentFile>();
            sverkaFile = new List<SverkaFile>();
        }

        public string FileName
        {
            get
            {
                return file.FileName;
            }
            set
            {
                file.FileName = value;
                file.CreateDateTime = DateTime.Now;
            }
        }

        public void AddPaymentRow(BaseKaznaFile row)
        {
            if (row is KaznaPaymentFile)
            {
                paymentRows.Add((KaznaPaymentFile)row);
            }
            else if (row is SverkaFile)
            {
                sverkaFile.Add((SverkaFile)row);
            }

        }

        public void ClearPayment()
        {
            paymentRows.Clear();
            sverkaFile.Clear();
        }

        public void SaveData()
        {
            KaznaFilesDataManager.InsertKaznaFile(ref file);

            foreach(var row in paymentRows)
            {
                try
                {
                    row.file_id = file.Id;
                    KaznaFilesDataManager.InsertKaznaPaymentRows(row);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            foreach (var row in sverkaFile)
            {
                try
                {
                    row.file_id = file.Id;
                    KaznaFilesDataManager.InsertSverkaFile(row);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public bool IsAlreadySettled()
        {


            return false;
        }
    }
}