using KasabaKiat.BusinessLogic.Putyovki.OplataPutyovok;
using KasabaKiat.ViewModel.Putyovki;
using KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki;
using KasabaKIAT.BusinessLogic.Utils;
using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Buxgalteriya;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.PrixodChastichka
{
    public class BankpaymentsDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="db"></param>
        public static void Create(ref BankpaymentsViewModel model, KasabaKIATEntities db)
        {
                tbgl_bankpayments reg = new tbgl_bankpayments();

                reg.NUM_DOC =           model.num_doc               ;
                reg.PAYER_NAME =        model.payer_name            ;
                reg.PAYER_INN =         model.payer_inn             ;
                reg.PAYER_MFO =         model.payer_mfo             ;
                reg.PAYER_ACCOUNT =     model.payer_account         ;
                reg.TRAN_DATE =         model.tran_date             ;
                reg.PAYMENT_DATAIL =    model.payment_datail        ;
                reg.AMOUNT =            model.amount                ;
                reg.BENEF_NAME =        model.benef_name            ;
                reg.BENEF_INN =         model.benef_inn             ;
                reg.BENEF_MFO =         model.benef_mfo             ;
                reg.BENEF_ACCOUNT =     model.benef_account         ;
                reg.DBCRFLG =           model.dbcrflg               ;
                reg.PARENT_ID =         model.parent_id             ;
                reg.DEPARTMENT_BASE =   model.department_base       ;
                reg.USER_ID = model.user_id;

                var planOtrasl = db.tbgl_bankpayments.Add(reg);

                if (planOtrasl != null)
                {
                    model.rec_id = planOtrasl.REC_ID;
                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Modify(BankpaymentsViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbgl_bankpayments.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.NUM_DOC = model.num_doc;
                        reg.PAYER_NAME = model.payer_name;
                        reg.PAYER_INN = model.payer_inn;
                        reg.PAYER_MFO = model.payer_mfo;
                        reg.PAYER_ACCOUNT = model.payer_account;
                        reg.TRAN_DATE = model.tran_date;
                        reg.PAYMENT_DATAIL = model.payment_datail;
                        reg.AMOUNT = model.amount;
                        reg.BENEF_NAME = model.benef_name;
                        reg.BENEF_INN = model.benef_inn;
                        reg.BENEF_MFO = model.benef_mfo;
                        reg.BENEF_ACCOUNT = model.benef_account;
                        reg.DBCRFLG = model.dbcrflg;
                        reg.PARENT_ID = model.parent_id;
                        reg.DEPARTMENT_BASE = model.department_base;
                        reg.USER_ID = model.user_id;
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Remove(BankpaymentsViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbgl_bankpayments.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tbgl_bankpayments.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<BankpaymentsViewModel> Get(BankpaymentsViewModel model)
        {
            List<BankpaymentsViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tbgl_bankpayments
                            where bank.DEPARTMENT_BASE == model.department_base
                            select new BankpaymentsViewModel
                            {
                                 rec_id         =                bank.REC_ID            ,  
                                 num_doc        =                bank.NUM_DOC           ,  
                                 payer_name     =                bank.PAYER_NAME        ,  
                                 payer_inn      =                bank.PAYER_INN         ,  
                                 payer_mfo      =                bank.PAYER_MFO         ,  
                                 payer_account  =                bank.PAYER_ACCOUNT     ,  
                                 tran_date      =                bank.TRAN_DATE         ,  
                                 payment_datail =                bank.PAYMENT_DATAIL    ,  
                                 amount         =                bank.AMOUNT            ,  
                                 benef_name     =                bank.BENEF_NAME        ,  
                                 benef_inn      =                bank.BENEF_INN         ,  
                                 benef_mfo      =                bank.BENEF_MFO         ,  
                                 benef_account  =                bank.BENEF_ACCOUNT     ,  
                                 dbcrflg        =                bank.DBCRFLG           ,  
                                 parent_id      =                bank.PARENT_ID         ,  
                                 department_base=                bank.DEPARTMENT_BASE   ,  
                                 user_id        =                bank.USER_ID               
                            };

                list = query.ToList();
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static List<BankpaymentsViewModel> Get(string departmentBase, DateTime beginDate, DateTime endDate)
        {
            List<BankpaymentsViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tbgl_bankpayments
                            where bank.DEPARTMENT_BASE == departmentBase && bank.TRAN_DATE >= beginDate 
                            && bank.TRAN_DATE <= endDate
                            select new BankpaymentsViewModel
                            {
                                rec_id = bank.REC_ID,
                                num_doc = bank.NUM_DOC,
                                payer_name = bank.PAYER_NAME,
                                payer_inn = bank.PAYER_INN,
                                payer_mfo = bank.PAYER_MFO,
                                payer_account = bank.PAYER_ACCOUNT,
                                tran_date = bank.TRAN_DATE,
                                payment_datail = bank.PAYMENT_DATAIL,
                                amount = bank.AMOUNT,
                                benef_name = bank.BENEF_NAME,
                                benef_inn = bank.BENEF_INN,
                                benef_mfo = bank.BENEF_MFO,
                                benef_account = bank.BENEF_ACCOUNT,
                                dbcrflg = bank.DBCRFLG,
                                parent_id = bank.PARENT_ID,
                                department_base = bank.DEPARTMENT_BASE,
                                user_id = bank.USER_ID
                            };

                list = query.ToList();
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public static async Task MakeProvodka(string departmentBase, long UserId)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var provodkaList = db.tbgl_bankpayments.Where(z => z.PROVODKA_ID == null && z.DEPARTMENT_BASE == departmentBase);

                if (provodkaList.Any())
                {
                    foreach(var pp in provodkaList)
                    {
                        var trans = new ProvodkiViewModel
                        {
                            comment = "Частичная оплата",
                            department_base = pp.DEPARTMENT_BASE,
                            inoutflg = false,
                            platelrsch = "005",
                            poluchrsch = "18",
                            summa = pp.AMOUNT,
                            user_id = pp.USER_ID,
                            trdt = pp.TRAN_DATE,
                            trref = "CHT",
                            trrefno = string.Format("{0}|{1}", pp.NUM_DOC, pp.PAYER_INN)
                        };

                        tbbu_provodki provodka = null;
                        ProvodkiDataManager.Create(ref trans, db, out provodka);

                        pp.tbbu_provodki = provodka;

                        long? OrgId = null;
                        var orgs = db.tblSprav_Org.Where(z => z.INN == pp.PAYER_INN);

                        if (orgs.Any())
                        {
                            OrgId = orgs.First().ID;
                        }


                        var oplata = new OplataViewModel
                        {
                            Data_Post = pp.TRAN_DATE,
                            Departmentbase = pp.DEPARTMENT_BASE,
                            ID_KV = DateOperations.GetQuarter(pp.TRAN_DATE),
                            ID_Otras = OrgId,
                            Num_Doc = pp.NUM_DOC,
                            PrizOpl = (int)OplataRasxPriz.OrganizasiyaOplata,
                            SmFil = pp.TRAN_DATE.Year,
                            Symma_DT = 0,
                            Symma_KT = pp.AMOUNT,
                            Tip_Org = true,
                            UserId = UserId
                        };

                        tblOplata opp = null;

                        OplataDataManager.Create(ref oplata, db, out opp);
                    }

                    try
                    {
                        await db.SaveChangesAsync();
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
                }
            }
        }
    }
}