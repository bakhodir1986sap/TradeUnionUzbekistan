using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.ClosePeriod
{
    public class CloseCommon
    {
        #region System Reqs
        public static DateTime GetCurrentBusinessDay(string departmentBase)
        {
            return DateTime.Now;
        }

        public static DateTime GetPrevBusinessDay(string departmentBase)
        {
            return DateTime.Now;
        }

        public static DateTime GetNextBusinessDay(string departmentBase)
        {
            return DateTime.Now;
        }

        public static int GetCurrentMonth(string departmentBase)
        {
            return 1;
        }

        public static int GetPrevMonth(string departmentBase)
        {
            return 12;              
        }

        public static int GetNextMonth(string departmentBase)
        {
            return 2;
        }

        public static int GetCurrentKvartal(string departmentBase)
        {
            return 1;
        }

        public static int GetPrevKvartal(string departmentBase)
        {
            return 4;
        }

        public static int GetNextKvartal(string departmentBase)
        {
            return 2;
        }


        public static int GetCurrentYear(string departmentBase)
        {
            return 1;
        }

        public static int GetPrevYear(string departmentBase)
        {
            return 4;
        }

        public static int GetNextYear(string departmentBase)
        {
            return 2;
        }
        #endregion

        public void ChangeBusinessDay()
        {

        }
    }
}
