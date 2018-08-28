using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using KasabaKiat.BusinessLogic.Utils;
using IronOcr;

namespace ExperimentConsole
{
    class Program
    {
        static void Main(string[] args)
        {


            //..
            //var Ocr = new AutoOcr();
            //var Result = Ocr.ReadPdf(@"D:\sssss.pdf");
            //Console.WriteLine(Result.Text);

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


            string s = @"08101~2001108606034013212110093~ 2018Й МАЙ ОЙИ УЧУН 25% ЯИТ ИНН-2 0 38 26 69 6: КМТУАНДИЖ МИНТАКАСИ";

            //"08101~2001108606034013212110093~ 2018Й ИЮН ОЙИ УЧУН 25% ЯИТ ИНН 2 03826696 КМТУ АТ";

            //"08101~2001108606034013212110093~ 2018Й МАЙ ОЙИ У-Н 25 % ЯИТ ИНН 2 03753646 АНДИЖОН ТЕМИРЙУЛ МАСОФАСИ ПЧ АНДИЖОН";

            //s = Regex.Replace(s, @"\s+", "");


            string[] numbers = Regex.Matches(s, @"([-:,.\s*/A-Za-zА-Яа-я](\d\s?){8}\d[-:,.\s)*~/A-Za-zА-Яа-я]|\s(\d\s?){8}\d$)")
                                        .Cast<Match>()
                                        .Select(m => m.Value)
                                        .ToArray();

            List<string> newArray = new List<string>();
            foreach (var str in numbers)
            {
                var str1 = Regex.Matches(str, @"\d+").Cast<Match>()
                                        .Select(m => m.Value)
                                        .ToArray();

                var sssdd = string.Join("", str1.Select(x => x.ToString()));

                newArray.Add(sssdd);

                Console.WriteLine("sssdd:{0} ", sssdd);
            }

            //
            //First 9 digit number As INN
            //var Inns = numbers.Where(x => x.Length == 9);

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
