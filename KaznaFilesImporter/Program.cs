using Epigov.Log;
using KasabaKIAT.BusinessLogic.BankOplati.Kaznacheystvo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaznaFilesImporter
{
    class Program
    {
        private static ILogService _logService;

        static void Main(string[] args)
        {
            _logService = new FileLogService(typeof(Program));

            _logService.Info("Begin Service======");

            while (true)
            {
                var kk = new KaznaController();

                kk.Process();
            }
        }
    }
}
