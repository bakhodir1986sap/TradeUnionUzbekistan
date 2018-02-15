using KasabaKIAT.Models;
using KasabaKiat.ViewModel;
using KasabaKiat.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Validation;

namespace KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok
{
    public class PostPutDetailDataManager
    {
        public static void Create(ref PostPutyovkaViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblPos_Put post = new tblPos_Put();

                //post.ID     = model.Id                ;
                post.ID_Post     = model.IdPost            ;
                post.ID_Sanat     = model.IdSanat           ;
                //post.tblSprav_San.Name_San     = model.SanatName         ;
                post.Num_Put     = model.Num_Put           ;
                post.DataSan     = model.DataSan           ;
                post.DataSanEnd     = model.DataSanEnd        ;
                post.Symma_Put     = model.Symma_Put         ;
                post.ID_PolVoz     = model.ID_PolVoz         ;
                post.Data_Post     = model.Data_Post         ;
                post.UserId     = model.UserId            ;
                post.Departmentbase     = model.Departmentbase;

                var pst = db.tblPos_Put.Add(post);

                try
                {
                    db.SaveChanges();
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

                model.Id = post.ID;
            }
        }

        public static void Modify(PostPutyovkaViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var post = db.tblPos_Put.Where(z => z.ID == model.Id);

                if (post.Any())
                {
                    var post1 = post.FirstOrDefault();

                    if (post1 != null)
                    {
                        //post1.ID = model.Id;
                        post1.ID_Post = model.IdPost;
                        post1.ID_Sanat = model.IdSanat;
                        post1.tblSprav_San.Name_San = model.SanatName;
                        post1.Num_Put = model.Num_Put;
                        post1.DataSan = model.DataSan;
                        post1.DataSanEnd = model.DataSanEnd;
                        post1.Symma_Put = model.Symma_Put;
                        post1.ID_PolVoz = model.ID_PolVoz;
                        post1.Data_Post = model.Data_Post;
                        post1.UserId = model.UserId;
                        post1.Departmentbase = model.Departmentbase;
                    }
                }

                db.SaveChanges();
            }

        }

        public static void Remove(PostPutyovkaViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var post = db.tblPos_Put.Where(z => z.ID == model.Id);

                if (post.Any())
                {
                    var reg = post.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblPos_Put.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
            //Search by ID_Post find all rows and delete each
        }

        public static List<PostPutyovkaViewModel> Get(PostPutyovkaViewModel model)
        {
            List<PostPutyovkaViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from postPut in db.tblPos_Put
                            select new PostPutyovkaViewModel
                            {
                                Id= postPut.ID,
                               IdPost         = postPut.ID_Post                ,
                               IdSanat        = postPut.ID_Sanat               ,
                               SanatName      = postPut.tblSprav_San.Name_San  ,
                               Num_Put        = postPut.Num_Put                ,
                               DataSan        = postPut.DataSan                ,
                               DataSanEnd     = postPut.DataSanEnd             ,
                               Symma_Put      = postPut.Symma_Put              ,
                               ID_PolVoz      = postPut.ID_PolVoz              ,
                               Data_Post      = postPut.Data_Post              ,
                               UserId         = postPut.UserId                 ,
                               Departmentbase = postPut.Departmentbase         
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}