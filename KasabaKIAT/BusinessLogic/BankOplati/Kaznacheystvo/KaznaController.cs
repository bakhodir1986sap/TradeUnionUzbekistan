using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.BankOplati.Kaznacheystvo
{
    public class KaznaController
    {
        private FileProcessor fp;

        public void Process()
        {
            fp = new FileProcessor();

            //Get Incoming File
            string filePath = string.Empty;

            fp.Process(filePath);

            var fileData = fp.FileData;

            if (fileData != null)
            {
                fileData.SaveData();
            }
        }
    }
}