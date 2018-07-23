using KasabaKIAT.ViewModel.Buxgalteriya;
using KaznaFilesImporter;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki.AutomaticTransactions.KaznaTransactions
{
    public class EcpCalcDataManager
    {
        public static void Create(ref EcpCalcViewModel model, KasabaKIATEntities db)
        {
            long? payment_id = model.payment_id;

                var ecpCalc = db.tbbu_ecp_calc.Where(z => z.PAYMENT_ID == payment_id);

                if (ecpCalc.Any())
                {
                    var ecp_calc = ecpCalc.FirstOrDefault();

                    ecp_calc.INN = model.inn;

                    ecp_calc.INN = model.inn;
                    ecp_calc.SOATO = model.soato;
                    ecp_calc.SOATO_NAME = model.soato_name;
                    ecp_calc.OPERDATE = model.operdate;
                    ecp_calc.OKONX_ID = model.okonx_id;
                    ecp_calc.OKONX_NAME = model.okonx_name;
                    ecp_calc.BRANCH_CODE = model.branch_code;
                    ecp_calc.BRANCH_NAME = model.branch_name;
                }
                else
                {
                    tbbu_ecp_calc reg = new tbbu_ecp_calc();

                    //reg.ID                    = model.id                      ;
                    reg.PAYMENT_ID = model.payment_id;
                    reg.PROFSOUZ_AMT = model.profsouz_amt;
                    reg.FEDERATION_AMT = model.federation_amt;
                    reg.OBLSOVPROF_AMT = model.oblsovprof_amt;
                    reg.OBL_DEPARTMENT_BASE = model.obl_department_base;
                    reg.PROVODKA_ID = model.provodka_id;
                    reg.REMARK = model.remark;
                    reg.INN = model.inn;
                    reg.SOATO = model.soato;
                    reg.SOATO_NAME = model.soato_name;
                    reg.OPERDATE = model.operdate;
                    reg.OKONX_ID = model.okonx_id;
                    reg.OKONX_NAME = model.okonx_name;
                    reg.BRANCH_CODE = model.branch_code;
                    reg.BRANCH_NAME = model.branch_name;

                    var planOtrasl = db.tbbu_ecp_calc.Add(reg);
                }
        }

        public static void Modify(EcpCalcViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbbu_ecp_calc.Where(z => z.ID == model.id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        //reg.PAYMENT_ID = model.payment_id;
                        //reg.PROFSOUZ_AMT = model.profsouz_amt;
                        //reg.FEDERATION_AMT = model.federation_amt;
                        //reg.OBLSOVPROF_AMT = model.oblsovprof_amt;
                        //reg.OBL_DEPARTMENT_BASE = model.obl_department_base;
                        reg.PROVODKA_ID = model.provodka_id;
                    }
                }

                db.SaveChanges();
            }
        }


        public static void ModifyBank(decimal ID, long ProvodkaId)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.KaznaBankFiles.Where(z => z.ID == ID);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.PROVODKA_ID = ProvodkaId;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(EcpCalcViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbbu_ecp_calc.Where(z => z.ID == model.id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tbbu_ecp_calc.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<EcpCalcViewModel> Get(EcpCalcViewModel model)
        {
            List<EcpCalcViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tbbu_ecp_calc
                            select new EcpCalcViewModel
                            {
                                    id                      = bank.ID                   ,
                                    payment_id              = bank.PAYMENT_ID           ,
                                    profsouz_amt            = bank.PROFSOUZ_AMT         ,
                                    federation_amt          = bank.FEDERATION_AMT       ,
                                    oblsovprof_amt          = bank.OBLSOVPROF_AMT       ,
                                    obl_department_base     = bank.OBL_DEPARTMENT_BASE  ,
                                    provodka_id             = bank.PROVODKA_ID        
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}