using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.ECP_Management
{
    public class EcpManagement
    {
        public static void ModifyPaymentData(EcpViewModel model, KasabaKIATEntities db)
        {
            if ((model == null) || (db == null)) return;

            var ecps = db.tbbu_ecp_calc.Where(z => z.ID == model.id && z.OBL_DEPARTMENT_BASE == model.obl_department_base);

            if (ecps.Any())
            {
                var ecp = ecps.First();

                ecp.INN = model.iznInn;
            }
        }

        public static List<EcpViewModel> GetEcpPaymentList(EcpViewModel model, KasabaKIATEntities db)
        {
            var list = new List<EcpViewModel>();

            var query = from ecp in db.tbbu_ecp_calc
                        join payment in db.KaznaPaymentFile
                        on ecp.PAYMENT_ID equals (long)payment.ID
                        where (ecp.INN == model.inn 
                        && ecp.OBL_DEPARTMENT_BASE == model.obl_department_base)
                        select new EcpViewModel
                        {
                            id                  = ecp.ID                    ,
                            payment_id          = ecp.PAYMENT_ID            ,
                            profsouz_amt        = ecp.PROFSOUZ_AMT          ,
                            federation_amt      = ecp.FEDERATION_AMT        ,
                            oblsovprof_amt      = ecp.OBLSOVPROF_AMT        ,
                            obl_department_base = ecp.OBL_DEPARTMENT_BASE   ,
                            provodka_id         = ecp.PROVODKA_ID.Value     ,   
                            remark              = payment.PURPOSE           ,
                            inn                 = ecp.INN                   ,
                            soato               = ecp.SOATO                 ,
                            soato_name          = ecp.SOATO_NAME            ,
                            operdate            = ecp.OPERDATE              ,
                            okonx_id            = ecp.OKONX_ID              ,
                            okonx_name          = ecp.OKONX_NAME            ,
                            branch_code         = ecp.BRANCH_CODE           ,
                            branch_name         = ecp.BRANCH_NAME           ,
                            clname              = payment.CLNAME
                        };

            list = query.ToList();

            return list;
        }
    }
}