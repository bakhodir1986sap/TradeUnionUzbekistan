using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KasabaKIAT.ViewModel.BankOplati;
using KasabaKIAT.Models;
using KasabaKIAT.BusinessLogic.BankOplati;

namespace KasabaKIAT.BusinessLogic.BankOplati.Kaznacheystvo
{
    public class PaymentFileData
    {
        private KaznaFiles file;
        private List<ViewModel.BankOplati.KaznaPaymentFile> paymentRows;

        public PaymentFileData()
        {
            file = new KaznaFiles();

            paymentRows = new List<ViewModel.BankOplati.KaznaPaymentFile>();
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

        public void AddPaymentRow(ViewModel.BankOplati.KaznaPaymentFile row)
        {
            paymentRows.Add(row);
        }

        public void SaveData()
        {
            KaznaFilesDataManager.InsertKaznaFile(ref file);

            foreach(var row in paymentRows)
            {
                row.file_id = file.Id;
                KaznaFilesDataManager.InsertKaznaPaymentRows(row);
            }
        }
    }
}