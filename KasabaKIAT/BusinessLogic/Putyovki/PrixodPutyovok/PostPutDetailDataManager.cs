using KasabaKIAT.Models;
using KasabaKiat.ViewModel;
using KasabaKiat.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Validation;
using KasabaKIAT.BusinessLogic.Putyovki.PrixodPutyovok;

namespace KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok
{
    public class PostPutDetailDataManager
    {
        public static void Create(ref PostPutyovkaViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var masterId = model.IdPost;

                var vids = db.tblPost.Where(z => z.ID == masterId);

                if (vids.Any())
                {
                    var vid = vids.First();

                    //Inpossible change details already closed nakladnoy
                    if (vid.Vid)
                    {
                        throw new ApplicationException("Inpossible change details already closed nakladnoy");
                    }
                }

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

        public static void Polzov(int SanId, string NumPut)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var post = db.tblPos_Put.Where(z => z.ID_Sanat == SanId && z.Num_Put == NumPut);

                if (post.Any())
                {
                    var post1 = post.FirstOrDefault();

                    if (post1 != null)
                    {
                        post1.ID_PolVoz = true;
                    }
                }

                db.SaveChanges();
            }

        }

        public static void OtmenPolzov(int SanId, string NumPut)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var post = db.tblPos_Put.Where(z => z.ID_Sanat == SanId && z.Num_Put == NumPut);

                if (post.Any())
                {
                    var post1 = post.FirstOrDefault();

                    if (post1 != null)
                    {
                        post1.ID_PolVoz = false;
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



        public static void RemoveAllByMain(long IdPost)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var post = db.tblPos_Put.Where(z => z.ID_Post == IdPost);

                if (post.Any())
                {
                    foreach(var detail in post)
                    {
                        db.tblPos_Put.Remove(detail);
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
                            join postMain in db.tblPost 
                            on postPut.ID_Post equals postMain.ID
                            where postMain.DepartmentBase == model.Departmentbase &&
                            postPut.ID_PolVoz == false
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
                               Departmentbase = postPut.Departmentbase         ,
                               Checked        = false
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
        public static List<PostPutyovkaViewModel> GetFiltered(PostPutyovkaViewModel model)
        {
            List<PostPutyovkaViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from postPut in db.tblPos_Put
                            where postPut.ID_Post == model.IdPost 
                            && postPut.tblPost.DepartmentBase == model.Departmentbase
                            && postPut.ID_PolVoz == false
                            select new PostPutyovkaViewModel
                            {
                                Id = postPut.ID,
                                IdPost = postPut.ID_Post,
                                IdSanat = postPut.ID_Sanat,
                                SanatName = postPut.tblSprav_San.Name_San,
                                Num_Put = postPut.Num_Put,
                                DataSan = postPut.DataSan,
                                DataSanEnd = postPut.DataSanEnd,
                                Symma_Put = postPut.Symma_Put,
                                ID_PolVoz = postPut.ID_PolVoz,
                                Data_Post = postPut.Data_Post,
                                UserId = postPut.UserId,
                                Departmentbase = postPut.Departmentbase
                            };

                list = query.ToList();
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entModel"></param>
        /// <returns></returns>
        public static List<PostPutyovkaViewModel> ParsePrixod(PrixodDetailViewModel entModel)
        {
            var list = new List<PostPutyovkaViewModel>();

            int count = 0;

            for (int i = entModel.beginNumber; i <= entModel.endNumber; i++)
            {
                count++;
                var mm = new PostPutyovkaViewModel
                {
                    Id = count,
                    DataSan = entModel.SanatDate,
                    Data_Post = DateTime.Now,
                    Departmentbase = entModel.DepartmentBase,
                    UserId = entModel.UserId,
                    IdSanat = entModel.SanatId,
                    SanatName = entModel.SanatName,
                    ID_PolVoz = false,
                    Num_Put = i.ToString(),
                    Symma_Put = entModel.Price,
                    IdPost = entModel.IdPost
                };

                list.Add(mm);
            }

            return list;
        }
    }
}