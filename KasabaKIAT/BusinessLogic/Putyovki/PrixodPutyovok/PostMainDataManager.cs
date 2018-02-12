using KasabaKIAT.Models;
using KasabaKiat.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok
{
    public class PostMainDataManager
    {
        public static void Create(ref PostupleniyaViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblPost post = new tblPost();

                post.ID_KV = model.Id_Kv;
                post.Data_Post = model.Data_Post;
                post.Num_Doc = model.Num_Doc;
                post.ID_Pos = model.ID_Pos;
                post.Num_Kol = model.Num_Kol;
                post.Symma = model.Symma;
                post.Name_Dov = model.Name_Dov;
                post.Vid = model.Vid;
                post.SmFil = model.SmFil;
                post.UserId = model.UserId;
                post.DepartmentBase = model.DepartmentBase;
                post.BeginNumber = model.BeginNumber;
                post.EndNumber = model.EndNumber;

                var pst = db.tblPost.Add(post);

                model.Id = pst.ID;

                db.SaveChanges();
            }
        }

        public static void Modify(PostupleniyaViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var post = db.tblPost.Where(z => z.ID == model.Id);

                if (post.Any())
                {
                    var post1 = post.FirstOrDefault();

                    if (post1 != null)
                    {
                        post1.ID_KV = model.Id_Kv;
                        post1.Data_Post = model.Data_Post;
                        post1.Num_Doc = model.Num_Doc;
                        post1.ID_Pos = model.ID_Pos;
                        post1.Num_Kol = model.Num_Kol;
                        post1.Symma = model.Symma;
                        post1.Name_Dov = model.Name_Dov;
                        post1.Vid = model.Vid;
                        post1.SmFil = model.SmFil;
                        post1.UserId = model.UserId;
                        post1.DepartmentBase = model.DepartmentBase;
                        post1.BeginNumber = model.BeginNumber;
                        post1.EndNumber = model.EndNumber;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(PostupleniyaViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var post = db.tblPost.Where(z => z.ID == model.Id);

                if (post.Any())
                {
                    var reg = post.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblPost.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<PostupleniyaViewModel> Get(PostupleniyaViewModel model)
        {
            List<PostupleniyaViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from post in db.tblPost
                            where post.SmFil == model.SmFil &&
                            post.ID_KV == model.Id_Kv &&
                            post.Vid == false
                            select new PostupleniyaViewModel
                            {
                               Id = post.ID,
                               Id_Kv         =  post.ID_KV             ,
                               Data_Post     =  post.Data_Post         ,
                               Num_Doc       =  post.Num_Doc           ,
                               ID_Pos        =  post.ID_Pos            ,
                               Num_Kol       =  post.Num_Kol           ,
                               Symma         =  post.Symma             ,
                               Name_Dov      =  post.Name_Dov          ,
                               Vid           =  post.Vid               ,
                               SmFil         =  post.SmFil             ,
                               UserId        =  post.UserId            ,
                               DepartmentBase=  post.DepartmentBase    ,
                               BeginNumber   =  post.BeginNumber       ,
                               EndNumber     =  post.EndNumber         
                          };

                list = query.ToList();
            }

            return list;
        }
    }
}