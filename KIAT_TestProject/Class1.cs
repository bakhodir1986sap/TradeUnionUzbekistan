using KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok;
using KasabaKiat.ViewModel;
using KasabaKIAT.BusinessLogic.Buxgalteriya.FinansoviyOtchet;
using KasabaKIAT.BusinessLogic.Buxgalteriya.PrixodChastichka;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIAT_TestProject
{
    public class Class1
    {
        public class PostMainDataManagerTests
        {
            [Test]
            public void GetTest()
            {
                //Assign
                int kvartal = 1;
                int year = 2018;
                string DepartmentBase = "17227";

                //Act
                var list = PostMainDataManager.Get(new PostupleniyaViewModel()
                {

                    Id_Kv = kvartal,
                    SmFil = year,
                    DepartmentBase = DepartmentBase

                }).Result;

                //Assert
                if(list.Any())
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }


            }


            [Test]
            public void MakeChatichkaTest()
            {
                BankpaymentsDataManager.MakeProvodka("17226", 94).Wait();
            }


            [Test]
            public void CloseKvartal()
            {
                var lll = new KasabaKIAT.Controllers.BuxgalterController();

                lll.CloseKvartal("17227", 1).Wait();
            }


            [Test]
            public void CloseGlavKniga()
            {
                var lll = new KasabaKIAT.Controllers.BuxgalterController();

                lll.GlavnayaKnigaTest("17227", 2018, 1);
            }


            [Test]
            public void PivodTest()
            {
                var lll = new KasabaKIAT.Controllers.OtchotController();

                lll.PivodTest();
            }

            [Test]
            public void FinOtchetTest()
            {
                var finOtchet = new FinOtchetStrukture();
                finOtchet.year = 2018;
                finOtchet.kvartal = 1;
                finOtchet.departmentCode = "17";

                finOtchet.FillFactData(2018, 1);

                var res_out = finOtchet.out_FinOtchet;

                Assert.IsNotNull(res_out);
            }
        }
    }
}
