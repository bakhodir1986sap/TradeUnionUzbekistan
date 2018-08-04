using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.ECP_Management
{
    public class EcpManagement
    {
        public void ModifyPaymentData(EcpViewModel model, KasabaKIATEntities db)
        {
            if ((model == null) || (db == null)) return;

            var ecps = db.tbbu_ecp_calc.Where(z => z.ID == model.id && z.OBL_DEPARTMENT_BASE == model.obl_department_base);

            if (ecps.Any())
            {
                var ecp = ecps.First();

                ecp.INN = model.inn;
            }
        }
    }
}