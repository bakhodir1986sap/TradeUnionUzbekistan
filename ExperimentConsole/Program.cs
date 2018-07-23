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
            string[] numbers = Regex.Split(@"08102~2001108600184013212110093~100010860184017097400221003~202001213~СамДЧТИ~(Инновация) 2018 йил апрел ойи учун иш хакидан  25% ажратма кучирилмокда; Ст.  42.99.990", @"\D+");

            //other case try to get INN 
            var Inn = numbers.FirstOrDefault(x => x.Length == 9);

            if ((Inn != null) && (Inn.StartsWith("2") || (Inn.StartsWith("3"))))
            {
                //model.remark += string.Format("INN:{0};", Inn);
                //innFromDtl = Inn;
            }
            else
            {
                //model.remark += string.Format("INN:{0};", payment.CLINN);
                //innFromDtl = payment.CLINN;
            }

            Console.ReadKey();
        }
    }
}
