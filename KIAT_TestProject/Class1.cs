using KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok;
using KasabaKiat.ViewModel;
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


        }
    }
}
