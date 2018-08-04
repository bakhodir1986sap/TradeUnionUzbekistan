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



            //string[] numbers = Regex.Split(@"08101~2001108606034013212110093~ 2018Й МАЙ ОЙИ У-Н 25 % ЯИТ ИНН 2 03753646 АНДИЖОН ТЕМИРЙУЛ МАСОФАСИ ПЧ АНДИЖОН", @"([^\d]|^)\d{1,2},\d{3}([^\d]|$)");

            ////other case try to get INN 
            //var Inn = numbers.FirstOrDefault(x => x.Length == 9);

            //if ((Inn != null) && (Inn.StartsWith("2") || (Inn.StartsWith("3"))))
            //{
            //    //model.remark += string.Format("INN:{0};", Inn);
            //    //innFromDtl = Inn;
            //}
            //else
            //{
            //    //model.remark += string.Format("INN:{0};", payment.CLINN);
            //    //innFromDtl = payment.CLINN;
            //}


            string s = @"08101~2001108606034013212110093~ 2018Й МАЙ ОЙИ УЧУН 25% ЯИТ ИНН 2 03826696 КМТУАНДИЖ МИНТАКАСИ";

                //"08101~2001108606034013212110093~ 2018Й ИЮН ОЙИ УЧУН 25% ЯИТ ИНН 2 03826696 КМТУ АТ";

            //"08101~2001108606034013212110093~ 2018Й МАЙ ОЙИ У-Н 25 % ЯИТ ИНН 2 03753646 АНДИЖОН ТЕМИРЙУЛ МАСОФАСИ ПЧ АНДИЖОН";

            s = Regex.Replace(s, @"\s+", "");

            string[] numbers = Regex.Split(s, @"\D+");

            //First 9 digit number As INN
            var Inns = numbers.Where(x => x.Length == 9);

            //Regex regex = new Regex(@"\d+", RegexOptions.IgnoreCase|RegexOptions.IgnorePatternWhitespace);
            //MatchCollection matches = regex.Matches(s);
            //if (matches.Count > 0)
            //{
            //    foreach (Match match in matches)
            //        Console.WriteLine(match.Value);
            //}
            //else
            //{
            //    Console.WriteLine("Совпадений не найдено");
            //}

            Console.ReadKey();
        }
    }
}
