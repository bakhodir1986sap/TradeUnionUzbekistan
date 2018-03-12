using Epigov.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.BankOplati.Kaznacheystvo
{
    public class KaznaController
    {
        private static ILogService _logService;
        private FileProcessor fp;

        public void Process()
        {
            _logService = new FileLogService(typeof(KaznaController));

            fp = new FileProcessor();

            //Get Incoming File
            string filePath = ConfigurationManager.AppSettings["RecieveFilePath"];
            string arxivePath = ConfigurationManager.AppSettings["ArxiveFilePath"];


            _logService.Info("File Path: " + filePath);
            _logService.Info("Arxive Directory: " + arxivePath);

            fp.Process(filePath, arxivePath);
        }
    }
}