using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.ExternalServicesAPI.FTP.Kaznacheystvo
{
    public class OtchislenCalc
    {

        private string _account1;

        private string _account2;

        private string _account3;

        private decimal procent1;

        private decimal procent23;

        public OtchislenCalc()
        {
            _account1 = "3212110";
            _account2 = "3212120";
            _account3 = "3212130";

            procent1 = (4 / 10);
            procent23 = (6 / 9);

        }

        public decimal GetOtchislen(string account, decimal symmavTiyinax)
        {
            var summavSumax = symmavTiyinax / 100;
            decimal symmaOtchislen = 0;

            if (account.Contains(_account1))
            {
                symmaOtchislen = Math.Round((summavSumax * procent1) / 100, 2);
            }
            else if ((account.Contains(_account2)) || (account.Contains(_account3)))
            {
                symmaOtchislen = Math.Round((summavSumax * procent23) / 100,2);
            }

            return symmaOtchislen;
        }
    }
}