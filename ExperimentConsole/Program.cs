using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using KasabaKiat.BusinessLogic.Utils;

namespace ExperimentConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var ttt = RuDateAndMoneyConverter.CurrencyToTxt(1200000, true);

            var ttt2 = RuDateAndMoneyConverter.DateToTextQuarter(DateTime.Now);

            Console.WriteLine(ttt);

            Console.WriteLine(ttt2);

            Console.ReadKey();
        }
    }
}
