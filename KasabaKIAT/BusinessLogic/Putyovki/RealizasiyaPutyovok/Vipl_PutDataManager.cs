using KasabaKIAT.Models;
using KasabaKiat.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KasabaKiat.BusinessLogic.Putyovki.OplataPutyovok;
using KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok;
using System.Data.Entity;
using System.Threading.Tasks;
using KasabaKIAT.BusinessLogic.Putyovki.PutyovkiHistory;

namespace KasabaKiat.BusinessLogic.Putyovki.RealizasiyaPutyovok
{
    /// <summary>
    /// 
    /// </summary>
    public class Vipl_PutDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Create(RealizasiyaViplPutViewModel model, KasabaKIATEntities db)
        {
                tblVipl_Put vipl = new tblVipl_Put();

              //vipl.ID               = model.id              ;
                vipl.ID_Vipl            = model.id_vipl         ;
                vipl.ID_Pok             = model.id_pok          ;
                vipl.ID_Sanat           = model.id_sanat        ;
                vipl.Num_Put            = model.num_put         ;
                vipl.Num_Proc           = model.num_proc        ;
                vipl.Symma              = model.symma           ;
                vipl.ID_PolVoz          = model.id_polvoz       ;
                vipl.Data_Post          = model.data_post       ;
                vipl.UserId             = model.userid          ;
                vipl.ZayavkaID          = model.zayavkaid       ;
                vipl.DepartmentBase     = model.departmentbase;

                var pst = db.tblVipl_Put.Add(vipl);

                model.id = pst.ID;

            //History making
            var histModel = new PutHistViewModel
            {
                event_tp = (int)HistEventType.Rasxod,
                departmentbase = model.departmentbase,
                id_sanat = model.id_sanat,
                num_put = model.num_put,
                remark = string.Format("Дата расход {0} - Сумма {1}", model.data_post.ToString(), model.symma),
                userid = model.userid
            };

            PutHistDataManager.CreateL(histModel, db);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Modify(RealizasiyaViplPutViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var vipl = db.tblVipl_Put.Where(z => z.ID == model.id);

                if (vipl.Any())
                {
                    var vipl1 = vipl.FirstOrDefault();

                    if (vipl1 != null)
                    {
                        vipl1.ID_Vipl = model.id_vipl;
                        vipl1.ID_Pok = model.id_pok;
                        vipl1.ID_Sanat = model.id_sanat;
                        vipl1.Num_Put = model.num_put;
                        vipl1.Num_Proc = model.num_proc;
                        vipl1.Symma = model.symma;
                        vipl1.ID_PolVoz = model.id_polvoz;
                        vipl1.Data_Post = model.data_post;
                        vipl1.UserId = model.userid;
                        vipl1.ZayavkaID = model.zayavkaid;
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
        public static async Task ZakritPutyovki(RealizasiyaViplPutViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var vipl = db.tblVipl_Put.Where(z => z.ID == model.id);

                if (vipl.Any())
                {
                    var vipl1 = vipl.FirstOrDefault();

                    if (vipl1 != null)
                    {
                        vipl1.ID_PolVoz = true;
                    }
                }

                await db.SaveChangesAsync();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static async Task ZakritPutyovki2(RealizasiyaViplPutViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var vipl = db.tblVipl_Put.Where(z => z.ID_Sanat == model.id_sanat && z.Num_Put == model.num_put && z.DepartmentBase == model.departmentbase);

                if (vipl.Any())
                {
                    var vipl1 = vipl.FirstOrDefault();

                    if (vipl1 != null)
                    {
                        vipl1.ID_PolVoz = true;
                    }
                }

                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Remove(RealizasiyaViplPutViewModel model, KasabaKIATEntities db)
        {
                var vipl = db.tblVipl_Put.Where(z => z.ID == model.id);

                if (vipl.Any())
                {
                    var reg = vipl.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblVipl_Put.Remove(reg);
                    }
                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void RemoveAllByMain(RealizasiyaViplPutViewModel model, KasabaKIATEntities db)
        {
                var vipl = db.tblVipl_Put.Where(z => z.ID_Vipl == model.id_vipl);

                if (vipl.Any())
                {
                    foreach(var vv in vipl)
                    {
                        PostPutDetailDataManager.OtmenPolzov(vv.ID_Sanat.Value, vv.Num_Put, db);

                        db.tblVipl_Put.Remove(vv);
                    }
                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void RemoveByPutyovka(int IdSanat, string Num_Put, string departmentCode, KasabaKIATEntities db, tblVozVrat vozv)
        {
                var vipl = db.tblVipl_Put.Where(z => z.ID_Sanat == IdSanat && z.Num_Put == Num_Put && z.DepartmentBase == departmentCode);

                if (vipl.Any())
                {
                    if (vipl.Count() > 1)
                    {
                        throw new ApplicationException("More than one row found");
                    }

                    var reg = vipl.FirstOrDefault();
                    
                    vozv.tblVipl = reg.tblVipl;
                    
                    if (reg != null)
                    {
                        db.tblVipl_Put.Remove(reg);
                    }
                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<RealizasiyaViplPutViewModel> Get(RealizasiyaViplPutViewModel model)
        {
            List<RealizasiyaViplPutViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from viplPut in db.tblVipl_Put
                            orderby viplPut.Num_Put
                            select new RealizasiyaViplPutViewModel
                            {
                                id              = viplPut.ID                ,
                                id_vipl         = viplPut.ID_Vipl           ,
                                id_pok          = viplPut.ID_Pok            ,
                                id_sanat        = viplPut.ID_Sanat          ,
                                num_put         = viplPut.Num_Put           ,
                                num_proc        = viplPut.Num_Proc          ,
                                symma           = viplPut.Symma             ,
                                id_polvoz       = viplPut.ID_PolVoz         ,
                                data_post       = viplPut.Data_Post         ,
                                userid          = viplPut.UserId            ,
                                zayavkaid       = viplPut.ZayavkaID         ,
                                departmentbase  = viplPut.DepartmentBase
                            };

                list = query.ToList();
            }

            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdVipl"></param>
        /// <returns></returns>
        public static async Task<List<RealizasiyaViplPutViewModel>> Get(long IdVipl)
        {
            List<RealizasiyaViplPutViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from viplPut in db.tblVipl_Put
                            where viplPut.ID_Vipl == IdVipl && viplPut.ID_PolVoz == false
                            orderby viplPut.Num_Put
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

                foreach(var put in list)
                {
                    var postPut = db.tblPos_Put.Where(p => p.Num_Put == put.num_put && p.ID_Sanat == put.id_sanat);

                    if (postPut.Any())
                    {
                        put.data_post = postPut.First().DataSan;
                    }
                }
            }

            return list;
        }



        public static async Task<List<RealizasiyaViplPutViewModel>> Get2(long IdViplPut)
        {
            List<RealizasiyaViplPutViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from viplPut in db.tblVipl_Put
                            where viplPut.ID == IdViplPut && viplPut.ID_PolVoz == false
                            orderby viplPut.Num_Put
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

                list = query.ToList();
            }

            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdVipl"></param>
        /// <returns></returns>
        public static async Task<List<RealizasiyaViplPutViewModel>> GetDetail4DanniyeVlad(
            DateTime beginDt, 
            DateTime endDt, 
            long IdOrg, 
            string departmentCode)
        {
            List<RealizasiyaViplPutViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from viplPut in db.tblVipl_Put
                            where viplPut.tblVipl.Data_Post >= beginDt
                            && viplPut.tblVipl.Data_Post <= endDt
                            && viplPut.tblVipl.DepartmentBase == departmentCode 
                            && viplPut.ID_PolVoz == false
                            && viplPut.tblVipl.ID_Org == IdOrg
                            orderby viplPut.Num_Put
                            select new RealizasiyaViplPutViewModel
                            {
                                id = viplPut.ID,
                                id_vipl = viplPut.ID_Vipl,
                                id_pok = viplPut.ID_Pok,
                                id_sanat = viplPut.ID_Sanat,
                                num_put = viplPut.Num_Put,
                                num_proc = viplPut.Num_Proc,
                                symma = viplPut.Symma,
                                id_polvoz = viplPut.ID_PolVoz,
                                data_post = viplPut.Data_Post,
                                userid = viplPut.UserId,
                                zayavkaid = viplPut.ZayavkaID,
                                departmentbase = viplPut.DepartmentBase
                            }).ToListAsync();

                list = query.ToList();

                foreach(var mmodel in list)
                {
                    mmodel.primechaniye = mmodel.id_polvoz ? "Закрыта" : "Открыта";

                    var putyovkaOplata = db.tblOplata_Ras.Where(z => z.ID_Sanat == mmodel.id_sanat
                    && z.Num_Put == mmodel.num_put && z.Departmentbase == departmentCode
                    && z.Priz == (int)OplataRasxPriz.OrganizasiyaOplata);

                    if (putyovkaOplata.Any())
                    {
                        mmodel.oplata = "Да";
                    }
                    else
                    {
                        mmodel.oplata = "Нет";
                    }

                    var faceCount = db.tblFase.Where(z => z.ID_Put == mmodel.id).Count();

                    if (faceCount > 0)
                    {
                        mmodel.isFaceExists = true;
                    }
                    else
                    {
                        mmodel.isFaceExists = false;
                    }


                    var sanats = db.tblSprav_San.Where(z => z.ID == mmodel.id_sanat);
                    mmodel.sanatName = sanats.First().Name_San;
                }

            }

            return list;
        }


        public static async Task<TotalResults> GetDetail4DanniyeVladTotal(DateTime beginDt,
            DateTime endDt,
            long IdOrg,
            string departmentCode)
        {
            var list = await GetDetail4DanniyeVlad(beginDt, endDt, IdOrg, departmentCode);

            var total = new TotalResults();

            if (list.Any())
            {
                total.NakladnoyCount = 0;
                total.PutyovkaCount = list.Count();
                total.SummaNakladnix = list.Sum(p=>p.symma);
            }
            else
            {
                total.NakladnoyCount = 0;
                total.PutyovkaCount = 0;
                total.SummaNakladnix = 0;
            }

            return total;
        }


        /// <summary>
        /// For zakritiya kvartala
        /// </summary>
        /// <param name="IdVipl"></param>
        /// <returns></returns>
        public static async Task<List<RealizasiyaViplPutViewModel>> GetZakritiya(long IdOrg, string departmentCode)
        {
            List<RealizasiyaViplPutViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from viplPut in db.tblVipl_Put
                            where viplPut.tblVipl.ID_Org == IdOrg
                            && viplPut.ID_PolVoz == false
                            && viplPut.DepartmentBase == departmentCode
                            orderby viplPut.Num_Put
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
                                departmentbase = viplPut.DepartmentBase,
                                putyovkaChecked = false
                            }).ToListAsync();

                list = query.ToList();
            }

            return list;
        }
    }
}