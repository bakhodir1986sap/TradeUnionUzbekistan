using Microsoft.VisualStudio.TestTools.UnitTesting;
using KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KasabaKiat.ViewModel;
using KasabaKIAT.BusinessLogic.Buxgalteriya.FinansoviyOtchet;

namespace KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok.Tests
{
    [TestClass()]
    public class PostMainDataManagerTests
    {
        [TestMethod()]
        public void GetTest()
        {
            var finOtchet = new FinOtchetStrukture();
            finOtchet.year = 2018;
            finOtchet.kvartal = 1;
            finOtchet.departmentCode = "17";

            finOtchet.GetDataFromPlan();
            finOtchet.FillFactData(2018, 1);
            finOtchet.FillFactDataBalance(2018, 1);


            var res_out = finOtchet.out_FinOtchet;
            var res_bal_out = finOtchet.out_Balance;

        }
    }
}