using KasabaKIAT.Models;
using KasabaKiat.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok;
using System.Threading.Tasks;
using System.Data.Entity;
using KasabaKIAT.BusinessLogic.Putyovki.PutyovkiHistory;

namespace KasabaKiat.BusinessLogic.VozvratPutyovki
{
    public class VozvratDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Create(ref VozvratViewModel model, KasabaKIATEntities db, out tblVozVrat avozv)
        {
                tblVozVrat vozv = new tblVozVrat();

                vozv.Data_Post              = model.data_post       ;
                vozv.Num_Doc                = model.num_doc         ;
                vozv.ID_Sanat               = model.id_sanat        ;
                vozv.Num_Put                = model.num_put         ;
                vozv.ID_Ot                  = model.id_ot           ;
                vozv.Dover                  = model.dover           ;
                vozv.ID_Pol                 = model.id_pol          ;
                vozv.SymmaP                 = model.symmap          ;
                vozv.ID_KV                  = model.id_kv           ;
                vozv.SmFil                  = model.smfil           ;
                vozv.Tip_Voz                = model.tip_voz         ;
                vozv.UserId                 = model.userid          ;
                vozv.DepartmentBase         = model.departmentbase;

                avozv = db.tblVozVrats.Add(vozv);

            //History making
            var histModel = new PutHistViewModel
            {
                event_tp = (int)HistEventType.Vozvrat,
                departmentbase = model.departmentbase,
                id_sanat = model.id_sanat,
                num_put = model.num_put,
                remark = string.Format("Дата {0} - Сумма {1}", model.data_post, model.symmap),
                userid = model.userid
            };

            PutHistDataManager.CreateL(histModel, db);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static async Task Modify(VozvratViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var vozv = db.tblVozVrats.Where(z => z.ID == model.id);

                if (vozv.Any())
                {
                    var vozv1 = vozv.FirstOrDefault();

                    if (vozv1 != null)
                    {
                        vozv1.Data_Post = model.data_post;
                        vozv1.Num_Doc = model.num_doc;
                        vozv1.ID_Sanat = model.id_sanat;
                        vozv1.Num_Put = model.num_put;
                        vozv1.ID_Ot = model.id_ot;
                        vozv1.Dover = model.dover;
                        vozv1.ID_Pol = model.id_pol;
                        vozv1.SymmaP = model.symmap;
                        vozv1.ID_KV = model.id_kv;
                        vozv1.SmFil = model.smfil;
                        vozv1.Tip_Voz = model.tip_voz;
                        vozv1.UserId = model.userid;
                        vozv1.DepartmentBase = model.departmentbase;
                    }
                }

                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static async Task Remove(VozvratViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var sanInfo = db.tblVozVrats.Where(z => z.ID == model.id);

                if (sanInfo.Any())
                {
                    var reg = sanInfo.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblVozVrats.Remove(reg);
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
        public static async Task<List<VozvratViewModel>> Get(VozvratViewModel model)
        {
            List<VozvratViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from vozv in db.tblVozVrats
                            select new VozvratViewModel
                            {
                                id              = vozv.ID           ,
                                data_post       = vozv.Data_Post    ,
                                num_doc         = vozv.Num_Doc      , 
                                id_sanat        = vozv.ID_Sanat     , 
                                num_put         = vozv.Num_Put      , 
                                id_ot           = vozv.ID_Ot        , 
                                dover           = vozv.Dover        , 
                                id_pol          = vozv.ID_Pol       , 
                                symmap          = vozv.SymmaP       , 
                                id_kv           = vozv.ID_KV        , 
                                smfil           = vozv.SmFil        , 
                                tip_voz         = vozv.Tip_Voz      , 
                                userid          = vozv.UserId       , 
                                departmentbase  = vozv.DepartmentBase
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
        public static async Task<List<VozvratViewModel>> GetFilter(VozvratViewModel model)
        {
            List<VozvratViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from vozv in db.tblVozVrats
                            join sanat in db.tblSprav_San on vozv.ID_Sanat equals sanat.ID
                            join org in db.tblSprav_Org on vozv.ID_Ot equals org.ID
                            where vozv.DepartmentBase == model.departmentbase
                            && vozv.SmFil == model.smfil 
                            && vozv.ID_KV == model.id_kv
                            orderby vozv.Data_Post, vozv.Num_Doc
                            select new VozvratViewModel
                            {
                                id                                          = vozv.ID,
                                data_post                                   = vozv.Data_Post,
                                num_doc                                     = vozv.Num_Doc,
                                id_sanat                                    = vozv.ID_Sanat,
                                sanatoriyName                               = sanat.Name_San,
                                num_put                                     = vozv.Num_Put,
                                id_ot                                       = vozv.ID_Ot,
                                orgName                                     = org.Name_Org,
                                dover                                       = vozv.Dover,
                                id_pol                                      = vozv.ID_Pol,
                                symmap                                      = vozv.SymmaP,
                                id_kv                                       = vozv.ID_KV,
                                smfil                                       = vozv.SmFil,
                                tip_voz                                     = vozv.Tip_Voz,
                                userid                                      = vozv.UserId,
                                departmentbase                              = vozv.DepartmentBase
                            }).ToListAsync();

                list = query;
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdVipl"></param>
        /// <returns></returns>
        public static async Task<List<RealizasiyaViplPutViewModel>> GetPutyovkas(long idOrg, string departmentBase)
        {
            List<RealizasiyaViplPutViewModel> Mainlist = new List<RealizasiyaViplPutViewModel>();
            List<RealizasiyaViplPutViewModel> list = null;
            List<RealizasiyaViplPutViewModel> prixodlist = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from viplPut in db.tblVipl_Put
                            where viplPut.tblVipl.ID_Org == idOrg &&
                            viplPut.DepartmentBase == departmentBase
                            select new RealizasiyaViplPutViewModel
                            {
                                id = viplPut.ID,
                                id_vipl = viplPut.ID_Vipl,
                                id_pok = viplPut.ID_Pok,
                                id_sanat = viplPut.ID_Sanat,
                                sanatName = viplPut.tblSprav_San.Name_San,
                                num_put = viplPut.Num_Put,
                                num_proc = viplPut.Num_Proc,
                                symma = viplPut.Symma,
                                id_polvoz = viplPut.ID_PolVoz,
                                data_post = viplPut.Data_Post,
                                userid = viplPut.UserId,
                                zayavkaid = viplPut.ZayavkaID,
                                departmentbase = viplPut.DepartmentBase
                            }).ToListAsync();

                list = query;

                Mainlist.AddRange(list);

                //var orgs = db.tblSprav_Org.Where(z => z.ID == idOrg && z.DepartmentBase == departmentBase);

                //if (orgs.Any())
                //{

                //    string owner_Code = orgs.First().OWNER_CODE;

                //    var query2 = await (from viplPut in db.tblPos_Put
                //                        join viplMain in db.tblPost on viplPut.ID_Post equals viplMain.ID
                //                        where viplMain.DepartmentBase == owner_Code
                //                        && viplPut.ID_PolVoz == false
                //                        select new RealizasiyaViplPutViewModel
                //                        {
                //                            id = viplPut.ID,
                //                            id_vipl = viplPut.ID_Post,
                //                            id_pok = viplPut.ID_Post,
                //                            id_sanat = viplPut.ID_Sanat,
                //                            sanatName = viplPut.tblSprav_San.Name_San,
                //                            num_put = viplPut.Num_Put,
                //                            num_proc = 1,
                //                            symma = viplPut.Symma_Put,
                //                            id_polvoz = viplPut.ID_PolVoz,
                //                            data_post = viplPut.Data_Post,
                //                            userid = viplPut.UserId,
                //                            zayavkaid = null,
                //                            departmentbase = viplPut.Departmentbase
                //                        }).ToListAsync();

                //    prixodlist = query2;

                //    Mainlist.AddRange(prixodlist);
                //}

            }

            return Mainlist;
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
                var query = await (from vozv in db.tblVozVrats
                            where vozv.ID_KV == kvartal && vozv.SmFil == year
                            && vozv.DepartmentBase == departmentBase
                            select vozv).ToListAsync();

                var list = query.ToList();

                if (list.Any())
                {
                    totals.PutyovkaCount = list.Count();
                    totals.SummaNakladnix = list.Sum(z => z.SymmaP);
                }
                else
                {
                    totals.PutyovkaCount = 0;
                    totals.SummaNakladnix = 0;
                }
            }

            return totals;
        }
    }
}