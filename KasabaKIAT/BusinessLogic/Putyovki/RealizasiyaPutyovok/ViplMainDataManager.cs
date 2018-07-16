using KasabaKIAT.Models;
using KasabaKiat.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KasabaKIAT.BusinessLogic.Putyovki.RealizasiyaPutyovok;
using KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok;
using KasabaKIAT.BusinessLogic.Putyovki.Rekviziti;
using KasabaKiat.BusinessLogic.Utils.References;
using KasabaKiat.BusinessLogic.Putyovki.OplataPutyovok;
using System.Threading.Tasks;
using System.Data.Entity;

namespace KasabaKiat.BusinessLogic.Putyovki.RealizasiyaPutyovok
{
    public class ViplMainDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Create(ref RealizasiyaViplMainViewModel model, KasabaKIATEntities db, out tblVipl viplMain)
        {
            var numGen = new ViplNumGen();

                tblVipl vipl = new tblVipl();

                vipl.ID_Org             = model.id_org              ;
                vipl.Data_Post          = model.data_post           ;
                vipl.Num_Doc = numGen.GetNextNumDoc(model.departmentbase, model.smfil).ToString().PadLeft(6, '0');
                vipl.ID_Pok             = model.id_pok              ;
                vipl.Num_Kol            = model.num_kol             ;
                vipl.Symma              = model.symma               ;
                vipl.Name_Dov           = model.concat_dov;
                vipl.ID_Kv              = model.id_kv               ;
                vipl.Vid                = false           ;
                vipl.SmFil              = model.smfil               ;

                vipl.UserId             = model.userid              ;
                vipl.DepartmentBase     = model.departmentbase;

                viplMain = db.tblVipl.Add(vipl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static async Task Modify(RealizasiyaViplMainViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var vipl = db.tblVipl.Where(z => z.ID == model.id);

                if (vipl.Any())
                {
                    var vipl1 = vipl.FirstOrDefault();

                    if (vipl1 != null)
                    {
                        vipl1.ID_Org = model.id_org;
                        vipl1.Data_Post = model.data_post;
                        vipl1.Num_Doc = model.num_doc;
                        vipl1.ID_Pok = model.id_pok;
                        vipl1.Num_Kol = model.num_kol;
                        vipl1.Symma = model.symma;
                        vipl1.Name_Dov = model.concat_dov;
                        vipl1.ID_Kv = model.id_kv;
                        vipl1.Vid = model.vid;
                        vipl1.SmFil = model.smfil;
                        vipl1.UserId = model.userid;
                        vipl1.DepartmentBase = model.departmentbase;
                    }
                }

                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static async Task Remove(RealizasiyaViplMainViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var vipl = db.tblVipl.Where(z => z.ID == model.id);

                if (vipl.Any())
                {
                    var reg = vipl.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblVipl.Remove(reg);
                    }
                }

                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static async Task RemoveWithChild(RealizasiyaViplMainViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var vipl = db.tblVipl.Where(z => z.ID == model.id);

                if (vipl.Any())
                {
                    Vipl_PutDataManager.RemoveAllByMain(new RealizasiyaViplPutViewModel()
                    {
                        id_vipl = model.id
                    } , db);

                    var reg = vipl.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblVipl.Remove(reg);
                    }
                }

                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<List<RealizasiyaViplMainViewModel>> Get(RealizasiyaViplMainViewModel model)
        {
            List<RealizasiyaViplMainViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from vipl in db.tblVipl
                            //join org in db.tblSprav_Org on vipl.ID_Org equals org.ID
                            where vipl.SmFil == model.smfil 
                            && vipl.ID_Kv == model.id_kv
                            && vipl.DepartmentBase == model.departmentbase
                            orderby vipl.ID
                            select new RealizasiyaViplMainViewModel
                            {
                                id              = vipl.ID,
                                id_org          = vipl.ID_Org           ,
                                organizationName = "",//org.Name_Org,
                                data_post       = vipl.Data_Post        ,
                                num_doc         = vipl.Num_Doc          ,
                                id_pok          = vipl.ID_Pok           ,
                                num_kol         = vipl.Num_Kol          ,
                                symma           = vipl.Symma            ,
                                concat_dov        = vipl.Name_Dov         ,
                                id_kv           = vipl.ID_Kv            ,
                                vid             = vipl.Vid              ,
                                smfil           = vipl.SmFil            ,
                                userid          = vipl.UserId           ,
                                departmentbase  = vipl.DepartmentBase
                            }).ToListAsync();

                list = query;

                foreach(var q in list)
                {
                    var orgs = db.tblSprav_Org.Where(z => z.ID == q.id_org);

                    if (orgs.Any())
                    {
                        q.organizationName = orgs.First().Name_Org;
                    }
                }
            }

            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<List<RealizasiyaViplMainViewModel>> GetForEnterDanniyeVlad(DateTime beginDt, DateTime endDt, long IdOrg, string departmentCode)
        {
            List<RealizasiyaViplMainViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from vipl in db.tblVipl
                            where vipl.Data_Post >= beginDt &&
                            vipl.Data_Post <= endDt
                            && vipl.DepartmentBase == departmentCode
                            && vipl.ID_Org == IdOrg
                            orderby vipl.Num_Doc
                            select new RealizasiyaViplMainViewModel
                            {
                                id_org = vipl.ID_Org,
                                data_post = vipl.Data_Post,
                                num_doc = vipl.Num_Doc,
                                id_pok = vipl.ID_Pok,
                                num_kol = vipl.Num_Kol,
                                symma = vipl.Symma,
                                concat_dov = vipl.Name_Dov,
                                id_kv = vipl.ID_Kv,
                                vid = vipl.Vid,
                                smfil = vipl.SmFil,
                                userid = vipl.UserId,
                                departmentbase = vipl.DepartmentBase
                            }).ToListAsync();

                list = query;
            }

            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<List<RealizasiyaViplMainViewModel>> GetUnique(long Id)
        {
            List<RealizasiyaViplMainViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from vipl in db.tblVipl
                            where vipl.ID == Id
                            select new RealizasiyaViplMainViewModel
                            {
                                id_org = vipl.ID_Org,
                                data_post = vipl.Data_Post,
                                num_doc = vipl.Num_Doc,
                                id_pok = vipl.ID_Pok,
                                num_kol = vipl.Num_Kol,
                                symma = vipl.Symma,
                                concat_dov = vipl.Name_Dov,
                                id_kv = vipl.ID_Kv,
                                vid = vipl.Vid,
                                smfil = vipl.SmFil,
                                userid = vipl.UserId,
                                departmentbase = vipl.DepartmentBase
                            }).ToListAsync();

                list = query;
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public static async Task<TotalResults> GetTotals(int year, int kvartal, string departmentBase)
        {
            var totals = new TotalResults();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from vipl in db.tblVipl
                            join viplDtl in db.tblVipl_Put on vipl.ID equals viplDtl.ID_Vipl
                            where vipl.ID_Kv == kvartal && vipl.SmFil == year
                            && vipl.DepartmentBase == departmentBase
                            select viplDtl).ToListAsync();

                var nakladCount = db.tblVipl.Where(z => z.SmFil == year && z.ID_Kv == kvartal
                && z.DepartmentBase == departmentBase).
                    Count();

                totals.NakladnoyCount = nakladCount;

                var list = query;

                if (list.Any())
                {
                    totals.PutyovkaCount = list.Count();
                    totals.SummaNakladnix = list.Sum(z => z.Symma);
                }
                else
                {
                    totals.PutyovkaCount = 0;
                    totals.SummaNakladnix = 0;
                }
            }

            return totals;
        }


        public static async Task<TotalResults> GetTotalsZakritiye(int year, int kvartal, string departmentBase, long IdVipl)
        {
            var totals = new TotalResults();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from vipl in db.tblVipl
                            join viplDtl in db.tblVipl_Put on vipl.ID equals viplDtl.ID_Vipl
                            where vipl.ID_Kv == kvartal && vipl.SmFil == year
                            && vipl.DepartmentBase == departmentBase
                            && vipl.ID == IdVipl
                            select viplDtl).ToListAsync();

                var nakladCount = db.tblVipl.Where(z => z.SmFil == year && z.ID_Kv == kvartal
                && z.DepartmentBase == departmentBase).
                    Count();

                totals.NakladnoyCount = nakladCount;

                var list = query.ToList();

                if (list.Any())
                {
                    totals.PutyovkaCount = list.Count();
                    totals.SummaNakladnix = list.Sum(z => z.Symma);
                }
                else
                {
                    totals.PutyovkaCount = 0;
                    totals.SummaNakladnix = 0;
                }
            }

            return totals;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdPos"></param>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public static async Task<TotalResults> GetDetailTotals(long IdPos, string departmentBase)
        {
            var totals = new TotalResults();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from vipl in db.tblVipl
                            join viplDtl in db.tblVipl_Put on vipl.ID equals viplDtl.ID_Vipl
                            where vipl.ID == IdPos
                            && vipl.DepartmentBase == departmentBase
                            select viplDtl).ToListAsync();

                totals.NakladnoyCount = 1;

                var list = query.ToList();

                if (list.Any())
                {
                    totals.PutyovkaCount = list.Count();
                    totals.SummaNakladnix = list.Sum(z => z.Symma);
                }
                else
                {
                    totals.PutyovkaCount = 0;
                    totals.SummaNakladnix = 0;
                }
            }

            return totals;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdVipl"></param>
        /// <returns></returns>
        public static async Task<SchetFakturaViewModel> GetSchetFakturaData(long IdVipl)
        {
            var schetFaktura = new SchetFakturaViewModel();

            var naklad1 = await GetUnique(IdVipl);

            var naklad = naklad1.FirstOrDefault();

            if (naklad == null) return null;

            schetFaktura.nakladnaya = naklad;

            schetFaktura.nakladDtl = await Vipl_PutDataManager.Get(IdVipl);

            schetFaktura.rekvizit = RekvizitiDataManager.Get(new KasabaKIAT.ViewModel.Putyovki.RekvizitiViewModel()
            {
                DepartmentBase = naklad.departmentbase
            }).First();

            if (naklad.id_org.HasValue)
                schetFaktura.clientData = Sprav_Org.GetByDeraprtmentBase(naklad.departmentbase, naklad.id_org.Value).FirstOrDefault();

            return schetFaktura;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static async Task CloseNakladnoy(RealizasiyaViplMainViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var post = db.tblVipl.Where(z => z.ID == model.id);

                if (post.Any())
                {
                    var post1 = post.FirstOrDefault();

                    var total = await GetTotalsZakritiye(model.smfil.Value, model.id_kv.Value, model.departmentbase, model.id);

                    if (post1 != null)
                    {
                        if (post1.Vid) return;

                        post1.Num_Kol = total.PutyovkaCount;
                        post1.Symma = total.SummaNakladnix;
                        post1.Vid = true;

                        await db.SaveChangesAsync();
                    }

                    var oplata = new Oplata();

                    await oplata.CloseRasxodPutyovok(post1.ID);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdVipl"></param>
        /// <returns></returns>
        public static async Task<decimal> GetOrgPaymentsLastYear(long IdVipl)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                decimal totalPayment = 0;

                try
                {
                    var vipl = db.tblVipl.Where(z => z.ID == IdVipl);

                    if (vipl.Any())
                    {
                        var viplMain = vipl.First();

                        var orgId = viplMain.ID_Org;

                        var orgs = db.tblSprav_Org.Where(z => z.ID == orgId);

                        if (orgs.Any())
                        {
                            var organization = orgs.First();

                            var orig_inn = organization.ORIG_INN;

                            if (!string.IsNullOrEmpty(orig_inn))
                            {
                                var today = DateTime.Now.Date;

                                var yearAgo = today.AddDays(-365);

                                var payments = await (from ecp in db.tbbu_ecp_calc
                                                      where ecp.INN == orig_inn
                                                      && ecp.OPERDATE >= yearAgo
                                                      && ecp.OPERDATE <= today
                                                      select ecp.OBLSOVPROF_AMT).ToListAsync();

                                if (payments.Any())
                                {
                                    totalPayment = payments.Sum(z => z.Value);
                                }
                                else
                                {
                                    totalPayment = 0;
                                }

                                //Chastichka postupleniye
                                var chastichkas = db.tbgl_bankpayments.Where(z => z.DEPARTMENT_BASE == viplMain.DepartmentBase &&
                                z.PAYER_INN == orig_inn && z.TRAN_DATE >= yearAgo && z.TRAN_DATE <= today);


                                if (chastichkas.Any())
                                {
                                    totalPayment += Decimal.Multiply(chastichkas.Sum(z => z.AMOUNT), 0.7M);
                                }
                                else
                                {
                                    totalPayment += 0;
                                }
                            }
                            else
                            {
                                throw new ApplicationException("Original Inn is not valid");
                            }
                        }
                        else
                        {
                            throw new ApplicationException("Organization Id is not valid");
                        }
                    }
                }
                catch
                {
                    totalPayment = 0;
                }

                return totalPayment;
            }
        }
    }
}