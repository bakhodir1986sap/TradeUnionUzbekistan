using KasabaKIAT.Models;
using KasabaKiat.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Putyovki.RealizasiyaPutyovok
{
    public class Vipl_PutDataManager
    {
        public static void Create(RealizasiyaViplPutViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
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

                db.SaveChanges();

                model.id = pst.ID;
            }
        }

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

        public static void Remove(RealizasiyaViplPutViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
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

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void RemoveAllByMain(RealizasiyaViplPutViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var vipl = db.tblVipl_Put.Where(z => z.ID_Vipl == model.id_vipl);

                if (vipl.Any())
                {
                    foreach(var vv in vipl)
                    {
                        db.tblVipl_Put.Remove(vv);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<RealizasiyaViplPutViewModel> Get(RealizasiyaViplPutViewModel model)
        {
            List<RealizasiyaViplPutViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from viplPut in db.tblVipl_Put
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



        public static List<RealizasiyaViplPutViewModel> Get(long IdVipl)
        {
            List<RealizasiyaViplPutViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from viplPut in db.tblVipl_Put
                            where viplPut.ID_Vipl == IdVipl
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
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}