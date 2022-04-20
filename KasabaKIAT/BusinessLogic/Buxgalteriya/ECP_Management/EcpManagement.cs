using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.ECP_Management
{
    public class EcpManagement
    {
        public static void ModifyPaymentData(EcpViewModel model, KasabaKIATEntities db)
        {
            if ((model == null) || (db == null)) return;

            var org = db.tblSprav_Org.Where(z => z.INN == model.iznInn);

            string orgName = string.Empty;

            if (org.Any())
            {
                var oorg = org.First();

                oorg.TIMESTAMP_CH = DateTime.Now;

                orgName = oorg.Name_Org;
            }

            var ecps = db.tbbu_ecp_calc.Where(z => z.ID == model.id && z.OBL_DEPARTMENT_BASE == model.obl_department_base);

            if (ecps.Any())
            {
                var ecp = ecps.First();

                ecp.INN = model.iznInn;

                if (!string.IsNullOrEmpty(orgName))
                {
                    ecp.NAME_ORG = orgName;
                }
            }
        }

        public static List<EcpViewModel> GetEcpPaymentList(EcpViewModel model, KasabaKIATEntities db)
        {
            var filteredEcp = from ecp in db.tbbu_ecp_calc
                        where (ecp.INN == model.inn
                        && ecp.OBL_DEPARTMENT_BASE == model.obl_department_base)
                        select new EcpViewModel
                        {
                            id = ecp.ID,
                            payment_id = ecp.PAYMENT_ID,
                            profsouz_amt = ecp.PROFSOUZ_AMT,
                            federation_amt = ecp.FEDERATION_AMT,
                            oblsovprof_amt = ecp.OBLSOVPROF_AMT,
                            obl_department_base = ecp.OBL_DEPARTMENT_BASE,
                            provodka_id = ecp.PROVODKA_ID,
                            remark = String.Empty,
                            inn = ecp.INN,
                            soato = ecp.SOATO,
                            soato_name = ecp.SOATO_NAME,
                            operdate = ecp.OPERDATE,
                            okonx_id = ecp.OKONX_ID,
                            okonx_name = ecp.OKONX_NAME,
                            branch_code = ecp.BRANCH_CODE,
                            branch_name = ecp.BRANCH_NAME,
                            clname = String.Empty
                        };

            var query = from ecp in filteredEcp
                        join payment in db.KaznaPaymentFile
                        on ecp.payment_id equals (long)payment.ID
                        select new EcpViewModel
                        {
                            id = ecp.id,
                            payment_id = ecp.payment_id,
                            profsouz_amt = ecp.profsouz_amt,
                            federation_amt = ecp.federation_amt,
                            oblsovprof_amt = ecp.oblsovprof_amt,
                            obl_department_base = ecp.obl_department_base,
                            provodka_id = ecp.provodka_id,
                            remark = payment.PURPOSE,
                            inn = ecp.inn,
                            soato = ecp.soato,
                            soato_name = ecp.soato_name,
                            operdate = ecp.operdate,
                            okonx_id = ecp.okonx_id,
                            okonx_name = ecp.okonx_name,
                            branch_code = ecp.branch_code,
                            branch_name = ecp.branch_name,
                            clname = payment.CLNAME
                        };


            return query.Take(1000).ToList();
        }
    }
}