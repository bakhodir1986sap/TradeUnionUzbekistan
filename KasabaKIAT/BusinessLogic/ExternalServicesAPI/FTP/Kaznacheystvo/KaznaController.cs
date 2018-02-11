using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace KasabaKiat.BusinessLogic.BankOplati.Kaznacheystvo
{
    public class KaznaController
    {
        private FileProcessor fp;

        public void Process()
        {
            fp = new FileProcessor();

            var targetDirectory = KasabaKIAT.Properties.Settings.Default.KaznaDirectory;

            if (Directory.Exists(targetDirectory))
            {
                string[] fileEntries = Directory.GetFiles(targetDirectory, "*.*"); //Filter files
                foreach (string fileName in fileEntries)
                {
                    string filePath = fileName;

                    fp.Process(filePath);

                    var fileData = fp.FileData;

                    if (fileData != null)
                    {
                        fileData.SaveData();
                    }

                    var arxivePath = ManageArxiveDirectory(filePath);

                    MoveToArxive(filePath, arxivePath);
                }
            }
        }

        private string ManageArxiveDirectory(string filePath)
        {
            var ArxiveDirectory = KasabaKIAT.Properties.Settings.Default.ArxivKaznaDirectory;

            var CurrentYear = DateTime.Now.Year;
            var CurrentMonth = DateTime.Now.Month;
            var CurrentDay = DateTime.Now.Day;

            var yearPath = Path.Combine(ArxiveDirectory, CurrentYear.ToString());

            if (!Directory.Exists(yearPath))
            {
                Directory.CreateDirectory(yearPath);
            }

            var MonthPath = Path.Combine(yearPath, CurrentMonth.ToString());

            if (!Directory.Exists(MonthPath))
            {
                Directory.CreateDirectory(MonthPath);
            }

            var DayPath = Path.Combine(MonthPath, CurrentDay.ToString());

            if (!Directory.Exists(DayPath))
            {
                Directory.CreateDirectory(DayPath);
            }

            var DayPathWithFileName = Path.Combine(DayPath, filePath);

            return DayPathWithFileName;

        }

        private void MoveToArxive(string filePath, string arxivePath)
        {
            File.Move(filePath, arxivePath);
        }
    }
}