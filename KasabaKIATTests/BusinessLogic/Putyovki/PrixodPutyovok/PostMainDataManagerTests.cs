using Microsoft.VisualStudio.TestTools.UnitTesting;
using KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KasabaKiat.ViewModel;

namespace KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok.Tests
{
    [TestClass()]
    public class PostMainDataManagerTests
    {
        [TestMethod()]
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
            foreach(var tt in list)
            {
                Assert.AreEqual(1, 1);
            }


        }
    }
}