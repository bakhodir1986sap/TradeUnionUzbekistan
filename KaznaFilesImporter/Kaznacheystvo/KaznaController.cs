using Epigov.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.BankOplati.Kaznacheystvo
{
    public class KaznaController
    {
        private static ILogService _logService;
        private FileProcessor fp;

        public static ILogService LogService { get => _logService; set => _logService = value; }

        public void Process()
        {
            LogService = new FileLogService(typeof(KaznaController));

            fp = new FileProcessor();

            //Get Incoming File
            string filePath = ConfigurationManager.AppSettings["RecieveFilePath"];
            string arxivePath = ConfigurationManager.AppSettings["ArxiveFilePath"];

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            if (!Directory.Exists(arxivePath))
            {
                Directory.CreateDirectory(arxivePath);
            }

            LogService.Info("File Path: " + filePath);
            LogService.Info("Arxive Directory: " + arxivePath);

            fp.Process(filePath, arxivePath);
        }
    }
}