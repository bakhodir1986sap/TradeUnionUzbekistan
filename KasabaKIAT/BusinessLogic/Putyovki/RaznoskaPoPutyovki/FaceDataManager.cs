using KasabaKIAT.Models;
using KasabaKiat.ViewModel.Putyovki.RaznoskaPutyovok;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Putyovki.RaznoskaPoPutyovki
{
    public class FaceDataManager
    {
        public static void Create(ref FaceViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblFase fc = new tblFase();

                //fc.ID                 = model.id              ;
                fc.ID_Put               = model.id_put          ;
                fc.ID_Sanat             = model.id_sanat        ;
                fc.Num_Put              = model.num_put         ;
                fc.LastName             = model.lastname        ;
                fc.Name                 = model.name            ;
                fc.FirstName            = model.firstname       ;
                fc.Adress               = model.adress          ;
                fc.Rabota               = model.rabota          ;
                fc.Dolj                 = model.dolj            ;
                fc.Stav_Vid             = model.stav_vid        ;
                fc.Pas_ser              = model.pas_ser         ;
                fc.Pas_Num              = model.pas_num         ;
                fc.Pas_KD               = model.pas_kd          ;
                fc.Pas_Data             = model.pas_data        ;
                fc.Gost_Pr              = model.gost_pr         ;
                fc.ID_Osnov             = model.id_osnov        ;
                fc.Data_Post            = model.data_post       ;
                fc.Pol                  = model.pol             ;
                fc.UserId               = model.userid          ;
                fc.Departmentbase       = model.departmentbase;

                var fcc = db.tblFase.Add(fc);

                model.id = fcc.ID;

                db.SaveChanges();
            }
        }

        public static void Update(FaceViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var fc = db.tblFase.Where(z => z.ID == model.id);

                if (fc.Any())
                {
                    var fc1 = fc.FirstOrDefault();

                    if (fc1 != null)
                    {
                        //fc1.ID_Put = model.id_put;
                        fc1.ID_Sanat = model.id_sanat;
                        fc1.Num_Put = model.num_put;
                        fc1.LastName = model.lastname;
                        fc1.Name = model.name;
                        fc1.FirstName = model.firstname;
                        fc1.Adress = model.adress;
                        fc1.Rabota = model.rabota;
                        fc1.Dolj = model.dolj;
                        fc1.Stav_Vid = model.stav_vid;
                        fc1.Pas_ser = model.pas_ser;
                        fc1.Pas_Num = model.pas_num;
                        fc1.Pas_KD = model.pas_kd;
                        fc1.Pas_Data = model.pas_data;
                        fc1.Gost_Pr = model.gost_pr;
                        fc1.ID_Osnov = model.id_osnov;
                        fc1.Data_Post = model.data_post;
                        fc1.Pol = model.pol;
                        fc1.UserId = model.userid;
                        fc1.Departmentbase = model.departmentbase;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(FaceViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var post = db.tblFase.Where(z => z.ID == model.id);

                if (post.Any())
                {
                    var reg = post.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblFase.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<FaceViewModel> GetBron(FaceViewModel model)
        {
            List<FaceViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from face in db.tblFase
                            select new FaceViewModel
                            {
                                id              = face.ID           ,
                                id_put          = face.ID_Put       ,  
                                id_sanat        = face.ID_Sanat     ,  
                                num_put         = face.Num_Put      ,  
                                lastname        = face.LastName     ,  
                                name            = face.Name         ,  
                                firstname       = face.FirstName    ,  
                                adress          = face.Adress       ,  
                                rabota          = face.Rabota       ,  
                                dolj            = face.Dolj         ,  
                                stav_vid        = face.Stav_Vid     ,  
                                pas_ser         = face.Pas_ser      ,  
                                pas_num         = face.Pas_Num      ,  
                                pas_kd          = face.Pas_KD       ,  
                                pas_data        = face.Pas_Data     ,  
                                gost_pr         = face.Gost_Pr      ,  
                                id_osnov        = face.ID_Osnov     ,  
                                data_post       = face.Data_Post    ,  
                                pol             = face.Pol          ,  
                                userid          = face.UserId       ,  
                                departmentbase  = face.Departmentbase 
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}