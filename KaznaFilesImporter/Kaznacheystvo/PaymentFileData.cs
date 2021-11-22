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
        private readonly List<KaznaPaymentFile> paymentRows;
        private readonly List<SverkaFile> sverkaFile;
        private readonly List<BankPaymentFile> bankFile;

        public PaymentFileData()
        {
            file = new KaznaFiles();

            paymentRows = new List<KaznaPaymentFile>();
            sverkaFile = new List<SverkaFile>();
            bankFile = new List<BankPaymentFile>();
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
            if (row is KaznaPaymentFile payment)
            {
                paymentRows.Add(payment);
            }
            else if (row is SverkaFile sverka)
            {
                sverkaFile.Add(sverka);
            }
            else if (row is BankPaymentFile bank)
            {
                bankFile.Add(bank);
            }
            else
            {
                throw new NotImplementedException("3 uuuch!!!!");
            }
        }

        public void ClearPayment()
        {
            paymentRows.Clear();
            sverkaFile.Clear();
            bankFile.Clear();
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

            foreach (var row in bankFile)
            {
                try
                {
                    row.file_id = file.Id;
                    KaznaFilesDataManager.InsertKaznaBankRows(row);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

    }
}