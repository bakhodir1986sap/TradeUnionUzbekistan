using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ExperimentConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = @"00599~1000228602142123600101093~Перебросить по предприятию ИНН: 203589461-МАДАД СКАТ МЧЖ согласно заключения ГНИ № 1000733766 от 19.01.2018";

            Regex regex = new Regex(text);
            Match match = regex.Match(@"[0-9]{9}");
            string[] numbers = Regex.Split(text, @"\D+");

            var Inn = numbers.FirstOrDefault(x => x.Length == 9);

            if (Inn != null)
            {

            }

            if (match.Success)
            {
                Console.WriteLine("Found Match for {0}", match.Value);
                Console.WriteLine("ID was {0}", match.Groups[1].Value);
            }

            Console.ReadKey();
        }
    }
}
