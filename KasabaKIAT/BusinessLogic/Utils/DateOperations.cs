using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Utils
{
    public class DateOperations
    {
        public static int GetQuarter(DateTime date)
        {
            if (date.Month >= 4 && date.Month <= 6)
                return 2;
            else if (date.Month >= 7 && date.Month <= 9)
                return 3;
            else if (date.Month >= 10 && date.Month <= 12)
                return 4;
            else
                return 1;
        }

        public static int GetYear(DateTime date)
        {
                return date.Year;
        }

        public static void GetNextQuarter(int in_year, int in_kv, out int o_year, out int o_kv)
        {
            if (in_kv < 4)
            {
                o_year = in_year;
                o_kv= in_kv + 1;
            }
            else
            {
                o_year = in_year + 1;
                o_kv = 1;
            }
        }


        public static void GetPrevQuarter(int in_year, int in_kv, out int o_year, out int o_kv)
        {
            if (in_kv > 1)
            {
                o_year = in_year;
                o_kv = in_kv - 1;
            }
            else
            {
                o_year = in_year - 1;
                o_kv = 4;
            }
        }


        public static DateTime GetCurKvartalDate(int curYear, int curKvartal)
        {
            int month = 0;
            if (curKvartal == 1)
            {
                month = 1;
            }
            else if (curKvartal == 2)
            {
                month = 4;
            }
            else if (curKvartal == 3)
            {
                month = 7;
            }
            else if (curKvartal == 4)
            {
                month = 10;
            }
            var dd = new DateTime(curYear, month, 1);

            return dd;
        }
    }
}