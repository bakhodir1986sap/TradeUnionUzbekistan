using KasabaKIAT.Models;
using KasabaKiat.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.VozvratPutyovki
{
    public class VozvratDataManager
    {
        public static void Create(ref VozvratViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblVozVrat vozv = new tblVozVrat();

              //vozv.ID                     = model.id              ;
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

                var vozz = db.tblVozVrat.Add(vozv);

                db.SaveChanges();

                model.id = vozz.ID;
            }
        }

        public static void Modify(VozvratViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var vozv = db.tblVozVrat.Where(z => z.ID == model.id);

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

                db.SaveChanges();
            }
        }

        public static void Remove(VozvratViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var sanInfo = db.tblVozVrat.Where(z => z.ID == model.id);

                if (sanInfo.Any())
                {
                    var reg = sanInfo.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblVozVrat.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<VozvratViewModel> Get(VozvratViewModel model)
        {
            List<VozvratViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from vozv in db.tblVozVrat
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
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}