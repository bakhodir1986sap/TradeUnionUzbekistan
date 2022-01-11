using Epigov.Log;
using KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki.AutomaticTransactions.KaznaTransactions;
using System;
using System.Threading;

namespace KaznaFilesImporter
{
    class Program
    {
        private static ILogService _logService;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            try
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
