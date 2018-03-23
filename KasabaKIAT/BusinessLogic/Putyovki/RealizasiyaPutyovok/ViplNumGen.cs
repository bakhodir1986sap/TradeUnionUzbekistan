using KasabaKIAT.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.RealizasiyaPutyovok
{
    public class ViplNumGen
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public int GetNextNumDoc(string departmentBase, int? year)
        {
            if (!year.HasValue)
            {
                year = DateTime.Now.Year;
            }

            int nextNum = 0;

            var amodel = new ViplNumGenViewModel();
            amodel.department_base = departmentBase;
            amodel.year = year.Value;

            var resmodel = ViplNumGenDataManager.Get(amodel);

            if (resmodel != null)
            {
                if (resmodel.Any())
                {
                    nextNum = resmodel.First().numdoc_gen.Value + 1;

                    amodel.numdoc_gen = nextNum;

                    ViplNumGenDataManager.Increase(amodel);
                }
            }

            return nextNum;
        }


    }
}