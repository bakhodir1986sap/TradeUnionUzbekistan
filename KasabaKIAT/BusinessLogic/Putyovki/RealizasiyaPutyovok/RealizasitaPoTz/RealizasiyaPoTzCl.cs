using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.RealizasiyaPutyovok.RealizasitaPoTz
{
    public class RealizasiyaPoTzCl
    {
        public List<RealizasiyaByTzViewModel> GetOformRealizList(List<SanAndPutViewModel> checkedList , string inn)
        {
            var list = new List<RealizasiyaByTzViewModel>();

            if (checkedList == null) return null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var orgDepBase = db.t_departmentbase.Where(d => d.le_inn == inn).FirstOrDefault();

                if (orgDepBase != null)
                {
                    foreach (var chk in checkedList)
                    {
                        var real = new RealizasiyaByTzViewModel();

                        real.sanatorium_id = chk.SanId;

                        var queryZayav = db.tbpt_zayavki.Where(z => z.DEPARTMENT_CODE == orgDepBase.code 
                        && z.SANATORY_ID == chk.SanId);

                        if (queryZayav.Any())
                        {

                        }
                    }
                   
                }
                else
                {
                    throw new ApplicationException("invalid departmentbase");
                }


            }


            return null;
        }
    }
}