using Epigov.Log;
using KasabaKIAT.BusinessLogic.BankOplati.Kaznacheystvo;
using KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki.AutomaticTransactions.KaznaTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
                Console.WriteLine(DateTime.Now.ToString() + " " + "...");

                var obj = new EcpCalculation();

                obj.CalcProcessN();

                Thread.Sleep(100000);
            }
        }
    }
}
