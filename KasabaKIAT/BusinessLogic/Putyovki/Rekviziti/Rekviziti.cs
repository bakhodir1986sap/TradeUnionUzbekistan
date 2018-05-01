using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.Rekviziti
{
    public class Rekviziti
    {
        public bool SaveRekvizit(RekvizitiViewModel model)
        {
            try
            {
                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    var vipl = db.tblPreds.Where(z => z.Departmentbase == model.DepartmentBase);

                    if (!vipl.Any())
                    {
                        RekvizitiDataManager.Create(model);
                    }
                    else
                    {
                        RekvizitiDataManager.Modify(model);
                    }
                }
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}