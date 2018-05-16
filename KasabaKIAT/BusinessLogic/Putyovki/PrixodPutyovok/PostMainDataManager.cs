using KasabaKIAT.Models;
using KasabaKiat.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Validation;
using KasabaKiat.BusinessLogic.Putyovki.OplataPutyovok;

namespace KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok
{
    /// <summary>
    /// 
    /// </summary>
    public class TotalResults
    {
        /// <summary>
        /// 
        /// </summary>
        public int NakladnoyCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PutyovkaCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? SummaNakladnix { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TotalOplataResults
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal? SummaDebit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? SummaCredit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? Saldo { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TotalOplataDtlResults
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal? RaznostSummi { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? Summa { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PostMainDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
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

                try
                {
                    db.SaveChanges();

                    model.Id = post.ID;
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
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void CloseNakladnoy(PostupleniyaViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var post = db.tblPost.Where(z => z.ID == model.Id);

                if (post.Any())
                {
                    var post1 = post.FirstOrDefault();

                    var total = GetTotals(model.SmFil.Value, model.Id_Kv.Value, model.DepartmentBase);

                    if (post1 != null)
                    {
                        if (post1.Vid) return;

                        post1.Num_Kol = total.PutyovkaCount;
                        post1.Symma = total.SummaNakladnix;
                        post1.Vid = true;

                        db.SaveChanges();

                        var oplata = new Oplata();

                        oplata.ClosePrixodPutyovok(post1.ID);
                    }
                }

                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
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
                        PostPutDetailDataManager.RemoveAllByMain(model.Id);

                        db.tblPost.Remove(reg);
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
        public static List<PostupleniyaViewModel> Get(PostupleniyaViewModel model)
        {
            List<PostupleniyaViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from post in db.tblPost
                            join org in db.tblSprav_Org on post.ID_Pos equals org.ID
                            where post.SmFil == model.SmFil &&
                            post.ID_KV == model.Id_Kv &&
                            post.DepartmentBase == model.DepartmentBase
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
                               EndNumber     =  post.EndNumber         ,
                               PosName       = org.Name_Org
                            };

                list = query.ToList();
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="kvartal"></param>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public static TotalResults GetTotals(int year, int kvartal, string departmentBase)
        {
            var totals = new TotalResults();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from post in db.tblPost
                            join postDtl in db.tblPos_Put on post.ID equals postDtl.ID_Post
                            where post.ID_KV == kvartal && post.SmFil == year 
                            && post.DepartmentBase == departmentBase
                            select postDtl;

                var nakladCount = db.tblPost.Where(z => z.SmFil == year && z.ID_KV == kvartal
                && z.DepartmentBase == departmentBase).
                    Count();

                totals.NakladnoyCount = nakladCount;

                var list = query.ToList();

                if (list.Any())
                {
                    totals.PutyovkaCount = list.Count();
                    totals.SummaNakladnix = list.Sum(z=>z.Symma_Put);
                }
                else
                {
                    totals.PutyovkaCount = 0;
                    totals.SummaNakladnix = 0;
                }
            }

            return totals;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdPos"></param>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public static TotalResults GetDetailTotals(long IdPos, string departmentBase)
        {
            var totals = new TotalResults();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from post in db.tblPost
                            join postDtl in db.tblPos_Put on post.ID equals postDtl.ID_Post
                            where post.ID == IdPos
                            && post.DepartmentBase == departmentBase
                            select postDtl;

                var nakladCount = 1;

                totals.NakladnoyCount = nakladCount;

                var list = query.ToList();

                if (list.Any())
                {
                    totals.PutyovkaCount = list.Count();
                    totals.SummaNakladnix = list.Sum(z => z.Symma_Put);
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