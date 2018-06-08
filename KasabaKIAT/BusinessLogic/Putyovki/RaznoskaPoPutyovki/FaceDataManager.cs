using KasabaKIAT.Models;
using KasabaKiat.ViewModel.Putyovki.RaznoskaPutyovok;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Putyovki.RaznoskaPoPutyovki
{
    /// <summary>
    /// 
    /// </summary>
    public class FaceDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Create(ref FaceViewModel model, KasabaKIATEntities db, out tblFase face)
        {
            tblFase fc = new tblFase();

            long IdValue = 0;

            if (db.tblFase.Count() != 0)
            {
                IdValue = db.tblFase.Max(z => z.ID);
            }

            fc.ID = IdValue + 1;
            fc.ID_Put = model.id_put;
            fc.ID_Sanat = model.id_sanat;
            fc.Num_Put = model.num_put;
            fc.LastName = model.lastname;
            fc.Name = model.name;
            fc.FirstName = model.firstname;
            fc.Adress = model.adress;
            fc.Rabota = model.rabota;
            fc.Dolj = model.dolj;
            fc.Stav_Vid = model.stav_vid;
            fc.Pas_ser = model.pas_ser;
            fc.Pas_Num = model.pas_num;
            fc.Pas_KD = model.pas_kd;
            fc.Pas_Data = model.pas_data;
            fc.Gost_Pr = model.gost_pr;
            fc.ID_Osnov = model.id_osnov;
            fc.Data_Post = DateTime.Now;
            fc.Pol = model.pol;
            fc.UserId = model.userid;
            fc.Departmentbase = model.departmentbase;
            fc.INN = model.inn;
            fc.PNFL = model.pnfl;

            face = db.tblFase.Add(fc);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Update(FaceViewModel model, KasabaKIATEntities db)
        {
            var fc = db.tblFase.Where(z => z.ID_Put == model.id_put);

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
                    //fc1.Data_Post = model.data_post;
                    fc1.Pol = model.pol;
                    fc1.UserId = model.userid;
                    fc1.Departmentbase = model.departmentbase;
                    fc1.INN = model.inn;
                    fc1.PNFL = model.pnfl;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<FaceViewModel> Get(FaceViewModel model)
        {
            List<FaceViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from face in db.tblFase
                            where face.ID_Put == model.id_put
                            select new FaceViewModel
                            {
                                id = face.ID,
                                id_put = face.ID_Put,
                                id_sanat = face.ID_Sanat,
                                num_put = face.Num_Put,
                                lastname = face.LastName,
                                name = face.Name,
                                firstname = face.FirstName,
                                adress = face.Adress,
                                rabota = face.Rabota,
                                dolj = face.Dolj,
                                stav_vid = face.Stav_Vid,
                                pas_ser = face.Pas_ser,
                                pas_num = face.Pas_Num,
                                pas_kd = face.Pas_KD,
                                pas_data = face.Pas_Data,
                                gost_pr = face.Gost_Pr,
                                id_osnov = face.ID_Osnov,
                                data_post = face.Data_Post,
                                pol = face.Pol,
                                userid = face.UserId,
                                departmentbase = face.Departmentbase,
                                inn = face.INN,
                                pnfl = face.PNFL
                            };

                list = query.ToList();
            }

            return list;
        }
    }

}