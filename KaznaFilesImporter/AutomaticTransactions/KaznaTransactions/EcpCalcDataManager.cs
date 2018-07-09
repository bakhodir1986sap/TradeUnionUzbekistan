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
                tbbu_ecp_calc reg = new tbbu_ecp_calc();

                //reg.ID                    = model.id                      ;
                reg.PAYMENT_ID              = model.payment_id              ;
                reg.PROFSOUZ_AMT            = model.profsouz_amt            ;
                reg.FEDERATION_AMT          = model.federation_amt          ;
                reg.OBLSOVPROF_AMT          = model.oblsovprof_amt          ;
                reg.OBL_DEPARTMENT_BASE     = model.obl_department_base     ;
                reg.PROVODKA_ID             = model.provodka_id             ;
                reg.REMARK                  = model.remark                  ;
                reg.INN                     = model.inn                     ;
                reg.SOATO                   = model.soato                   ;
                reg.SOATO_NAME              = model.soato_name              ;
                reg.OPERDATE = model.operdate;
                reg.OKONX_ID = model.okonx_id;
                reg.OKONX_NAME = model.okonx_name;
                reg.BRANCH_CODE = model.branch_code;
                reg.BRANCH_NAME = model.branch_name;

                var planOtrasl = db.tbbu_ecp_calc.Add(reg);

                try
                {
                    //Console.WriteLine("SaveChanges");
                    //db.SaveChanges();

                    //model.id = reg.ID;
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
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