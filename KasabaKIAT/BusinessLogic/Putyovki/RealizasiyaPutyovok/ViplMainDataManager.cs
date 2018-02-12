using KasabaKIAT.Models;
using KasabaKiat.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Putyovki.RealizasiyaPutyovok
{
    public class ViplMainDataManager
    {
        public static void Create(ref RealizasiyaViplMainViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblVipl vipl = new tblVipl();

                //vipl.ID                 = model.id                  ;
                vipl.ID_Org             = model.id_org              ;
                vipl.Data_Post          = model.data_post           ;
                vipl.Num_Doc            = model.num_doc             ;
                vipl.ID_Pok             = model.id_pok              ;
                vipl.Num_Kol            = model.num_kol             ;
                vipl.Symma              = model.symma               ;
                vipl.Name_Dov           = model.name_dov            ;
                vipl.ID_Kv              = model.id_kv               ;
                vipl.Vid                = model.vid                 ;
                vipl.SmFil              = model.smfil               ;
                vipl.UserId             = model.userid              ;
                vipl.DepartmentBase     = model.departmentbase;

                var pst = db.tblVipl.Add(vipl);

                model.id = pst.ID;

                db.SaveChanges();
            }
        }

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

        public static List<RealizasiyaViplMainViewModel> Get(RealizasiyaViplMainViewModel model)
        {
            List<RealizasiyaViplMainViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from vipl in db.tblVipl
                            select new RealizasiyaViplMainViewModel
                            {
                                id_org          = vipl.ID_Org           ,
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
    }
}