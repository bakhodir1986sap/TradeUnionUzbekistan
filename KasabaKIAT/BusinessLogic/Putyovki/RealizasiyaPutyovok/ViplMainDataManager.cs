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

namespace KasabaKiat.BusinessLogic.Putyovki.RealizasiyaPutyovok
{
    public class ViplMainDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Create(ref RealizasiyaViplMainViewModel model)
        {
            var numGen = new ViplNumGen();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblVipl vipl = new tblVipl();

                //vipl.ID                 = model.id                  ;
                vipl.ID_Org             = model.id_org              ;
                vipl.Data_Post          = model.data_post           ;
                vipl.Num_Doc = numGen.GetNextNumDoc(model.departmentbase, model.smfil).ToString().PadLeft(6, '0');
                vipl.ID_Pok             = model.id_pok              ;
                vipl.Num_Kol            = model.num_kol             ;
                vipl.Symma              = model.symma               ;
                vipl.Name_Dov           = model.name_dov            ;
                vipl.ID_Kv              = model.id_kv               ;
                vipl.Vid                = false           ;
                vipl.SmFil              = model.smfil               ;

                vipl.UserId             = model.userid              ;
                vipl.DepartmentBase     = model.departmentbase;

                var pst = db.tblVipl.Add(vipl);

                db.SaveChanges();

                model.id = vipl.ID;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Modify(RealizasiyaViplMainViewModel model)
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
                        vipl1.Name_Dov = model.name_dov;
                        vipl1.ID_Kv = model.id_kv;
                        vipl1.Vid = model.vid;
                        vipl1.SmFil = model.smfil;
                        vipl1.UserId = model.userid;
                        vipl1.DepartmentBase = model.departmentbase;
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Remove(RealizasiyaViplMainViewModel model)
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

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void RemoveWithChild(RealizasiyaViplMainViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var vipl = db.tblVipl.Where(z => z.ID == model.id);

                if (vipl.Any())
                {
                    Vipl_PutDataManager.RemoveAllByMain(new RealizasiyaViplPutViewModel()
                    {
                        id_vipl = model.id
                    });

                    var reg = vipl.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblVipl.Remove(reg);
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
        public static List<RealizasiyaViplMainViewModel> Get(RealizasiyaViplMainViewModel model)
        {
            List<RealizasiyaViplMainViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from vipl in db.tblVipl
                            join org in db.tblSprav_Org on vipl.ID_Org equals org.ID
                            where vipl.SmFil == model.smfil 
                            && vipl.ID_Kv == model.id_kv
                            && vipl.DepartmentBase == model.departmentbase
                            select new RealizasiyaViplMainViewModel
                            {
                                id              = vipl.ID,
                                id_org          = vipl.ID_Org           ,
                                organizationName = org.Name_Org,
                                data_post       = vipl.Data_Post        ,
                                num_doc         = vipl.Num_Doc          ,
                                id_pok          = vipl.ID_Pok           ,
                                num_kol         = vipl.Num_Kol          ,
                                symma           = vipl.Symma            ,
                                name_dov        = vipl.Name_Dov         ,
                                id_kv           = vipl.ID_Kv            ,
                                vid             = vipl.Vid              ,
                                smfil           = vipl.SmFil            ,
                                userid          = vipl.UserId           ,
                                departmentbase  = vipl.DepartmentBase
                            };

                list = query.ToList();
            }

            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<RealizasiyaViplMainViewModel> GetForEnterDanniyeVlad(DateTime beginDt, DateTime endDt, long IdOrg, string departmentCode)
        {
            List<RealizasiyaViplMainViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from vipl in db.tblVipl
                            where vipl.Data_Post >= beginDt &&
                            vipl.Data_Post <= endDt
                            && vipl.DepartmentBase == departmentCode
                            && vipl.ID_Org == IdOrg
                            select new RealizasiyaViplMainViewModel
                            {
                                id_org = vipl.ID_Org,
                                data_post = vipl.Data_Post,
                                num_doc = vipl.Num_Doc,
                                id_pok = vipl.ID_Pok,
                                num_kol = vipl.Num_Kol,
                                symma = vipl.Symma,
                                name_dov = vipl.Name_Dov,
                                id_kv = vipl.ID_Kv,
                                vid = vipl.Vid,
                                smfil = vipl.SmFil,
                                userid = vipl.UserId,
                                departmentbase = vipl.DepartmentBase
                            };

                list = query.ToList();
            }

            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<RealizasiyaViplMainViewModel> GetUnique(long Id)
        {
            List<RealizasiyaViplMainViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from vipl in db.tblVipl
                            where vipl.ID == Id
                            select new RealizasiyaViplMainViewModel
                            {
                                id_org = vipl.ID_Org,
                                data_post = vipl.Data_Post,
                                num_doc = vipl.Num_Doc,
                                id_pok = vipl.ID_Pok,
                                num_kol = vipl.Num_Kol,
                                symma = vipl.Symma,
                                name_dov = vipl.Name_Dov,
                                id_kv = vipl.ID_Kv,
                                vid = vipl.Vid,
                                smfil = vipl.SmFil,
                                userid = vipl.UserId,
                                departmentbase = vipl.DepartmentBase
                            };

                list = query.ToList();
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
        public static TotalResults GetTotals(int year, int kvartal, string departmentBase)
        {
            var totals = new TotalResults();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from vipl in db.tblVipl
                            join viplDtl in db.tblVipl_Put on vipl.ID equals viplDtl.ID_Vipl
                            where vipl.ID_Kv == kvartal && vipl.SmFil == year
                            && vipl.DepartmentBase == departmentBase
                            select viplDtl;

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
        public static TotalResults GetDetailTotals(long IdPos, string departmentBase)
        {
            var totals = new TotalResults();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from vipl in db.tblVipl
                            join viplDtl in db.tblVipl_Put on vipl.ID equals viplDtl.ID_Vipl
                            where vipl.ID == IdPos
                            && vipl.DepartmentBase == departmentBase
                            select viplDtl;

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
        public static SchetFakturaViewModel GetSchetFakturaData(long IdVipl)
        {
            var schetFaktura = new SchetFakturaViewModel();

            var naklad = GetUnique(IdVipl).FirstOrDefault();

            if (naklad == null) return null;

            schetFaktura.nakladnaya = naklad;

            schetFaktura.nakladDtl = Vipl_PutDataManager.Get(IdVipl);

            schetFaktura.rekvizit = RekvizitiDataManager.Get(new KasabaKIAT.ViewModel.Putyovki.RekvizitiViewModel()
            {
                DepartmentBase = naklad.departmentbase
            }).First();

            schetFaktura.clientData = Sprav_Org.GetByDeraprtmentBase(naklad.departmentbase, naklad.id_org.Value).First();

            return schetFaktura;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void CloseNakladnoy(RealizasiyaViplMainViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var post = db.tblVipl.Where(z => z.ID == model.id);

                if (post.Any())
                {
                    var post1 = post.FirstOrDefault();

                    var total = GetTotals(model.smfil.Value, model.id_kv.Value, model.departmentbase);

                    if (post1 != null)
                    {
                        if (post1.Vid) return;

                        post1.Num_Kol = total.PutyovkaCount;
                        post1.Symma = total.SummaNakladnix;
                        post1.Vid = true;

                        db.SaveChanges();
                    }

                    var oplata = new Oplata();

                    oplata.CloseRasxodPutyovok(post1.ID);
                }
            }
        }
    }
}