using KasabaKIAT.Models;
using KasabaKiat.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using KasabaKIAT.BusinessLogic.Putyovki.PrixodPutyovok;
using System.Data.Entity;
using System.Threading.Tasks;
using KasabaKIAT.BusinessLogic.Putyovki.PutyovkiHistory;

namespace KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok
{
    public class PostPutDetailDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="db"></param>
        /// <param name="postMain"></param>
        public static void Create(ref PostPutyovkaViewModel model, KasabaKIATEntities db , tblPost postMain)
        {
                var masterId = model.IdPost;

                var vids = db.tblPost.Where(z => z.ID == masterId);

                tblPos_Put post = new tblPos_Put();

                post.tblPost = postMain;
                post.ID_Post     = model.IdPost            ;
                post.ID_Sanat     = model.IdSanat           ;
                post.Num_Put     = model.Num_Put           ;
                post.DataSan     = model.DataSan           ;
                post.DataSanEnd     = model.DataSanEnd        ;
                post.Symma_Put     = model.Symma_Put         ;
                post.ID_PolVoz     = model.ID_PolVoz         ;
                post.Data_Post     = model.Data_Post         ;
                post.UserId     = model.UserId            ;
                post.Departmentbase     = model.Departmentbase;
                post.TIMESTAMP_T = DateTime.Now;

                var pst = db.tblPos_Put.Add(post);

                model.Id = pst.ID;

            //History making
            var histModel = new PutHistViewModel
            {
                event_tp = (int)HistEventType.Prixod,
                departmentbase = model.Departmentbase,
                id_sanat = model.IdSanat,
                num_put = model.Num_Put,
                remark = string.Format("Дата Заезда {0} - Дата прихода {1} - Сумма {2}", model.DataSanEnd.ToString(), model.Data_Post.ToString(), model.Symma_Put),
                userid = model.UserId
            };

            PutHistDataManager.CreateL(histModel, db);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task Modify(PostPutyovkaViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var post = db.tblPos_Put.Where(z => z.ID == model.Id);

                if (post.Any())
                {
                    var post1 = post.FirstOrDefault();

                    if (post1 != null)
                    {
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

                await db.SaveChangesAsync();
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task ModifyIzmData(PostPutyovkaViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var post = db.tblPos_Put.Where(z => z.ID == model.Id);

                if (post.Any())
                {
                    var post1 = post.FirstOrDefault();

                    if (post1 != null)
                    {
                        post1.DataSan = model.DataSan;
                    }
                }

                await db.SaveChangesAsync();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SanId"></param>
        /// <param name="NumPut"></param>
        /// <param name="db"></param>
        public static void Polzov(int SanId, string NumPut, KasabaKIATEntities db)
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

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SanId"></param>
        /// <param name="NumPut"></param>
        /// <param name="db"></param>
        public static void Polzov(long putId, KasabaKIATEntities db)
        {
            var post = db.tblPos_Put.Where(z => z.ID == putId);

            if (post.Any())
            {
                var post1 = post.FirstOrDefault();

                if (post1 != null)
                {
                    post1.ID_PolVoz = true;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SanId"></param>
        /// <param name="NumPut"></param>
        /// <param name="db"></param>
        public static void OtmenPolzov(int SanId, string NumPut, KasabaKIATEntities db)
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

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static async Task Remove(PostPutyovkaViewModel model)
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

                await db.SaveChangesAsync();
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

        public static async Task<List<PostPutyovkaViewModel>> Get(PostPutyovkaViewModel model, long? IdVipl)
        {
            List<PostPutyovkaViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var ViplPosTime = db.tblVipl.Where(z => z.ID == IdVipl).First().Data_Post;


                var query = await (from postPut in db.tblPos_Put
                            join postMain in db.tblPost 
                            on postPut.ID_Post equals postMain.ID
                            where postMain.DepartmentBase == model.Departmentbase &&
                            postPut.ID_PolVoz == false &&
                            postPut.Num_Put == (string.IsNullOrEmpty(model.Num_Put) ? postPut.Num_Put : model.Num_Put) &&
                            postMain.Data_Post <= ViplPosTime
                            orderby postPut.Num_Put
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
                            }).ToListAsync();

                foreach(var putyovka in query)
                {
                    if (list == null)
                    {
                        list = new List<PostPutyovkaViewModel>();
                    }

                    var IsBronExists = db.tblBron.Where(z => z.Num_Put == putyovka.Num_Put
                        && z.ID_San == putyovka.IdSanat
                        && z.Uchet == true);

                    if (!IsBronExists.Any())
                    {
                        list.Add(putyovka);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<List<PostPutyovkaViewModel>> GetFiltered(PostPutyovkaViewModel model)
        {
            List<PostPutyovkaViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from postPut in db.tblPos_Put
                            where postPut.ID_Post == model.IdPost 
                            && postPut.tblPost.DepartmentBase == model.Departmentbase
                            orderby postPut.Num_Put
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
                            }).ToListAsync();

                list = query.ToList();

                foreach(var putyovka in list)
                {
                    var IsBronExists = db.tblBron.Where(z => z.Num_Put == putyovka.Num_Put 
                    && z.ID_San == putyovka.IdSanat
                    && z.Uchet == true);

                    if (IsBronExists.Any())
                    {
                        if (putyovka.ID_PolVoz)
                        {
                            putyovka.StatusPutyovki = "Реализованная путевка";
                        }
                        else
                        {
                            putyovka.StatusPutyovki = "Забронированная путевка";
                        }
                    }
                    else
                    {
                        if (putyovka.ID_PolVoz)
                        {
                            putyovka.StatusPutyovki = "Реализованная путевка";
                        }
                        else
                        {
                            putyovka.StatusPutyovki = "Свободная путевка";
                        }
                    }
                }
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


        public static async Task<List<PostPutyovkaViewModel>> Get2(PostPutyovkaViewModel model)
        {
            List<PostPutyovkaViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from postPut in db.tblPos_Put
                            join postMain in db.tblPost
                            on postPut.ID_Post equals postMain.ID
                            where postMain.DepartmentBase == model.Departmentbase &&
                            postPut.ID == model.Id
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
                                Departmentbase = postPut.Departmentbase,
                                Checked = false,
                                Id_Kv = postMain.ID_KV,
                                Data_Post_Main = postMain.Data_Post,
                                Num_Doc = postMain.Num_Doc,
                                PosName = postMain.tblSprav_Org.Name_Org,
                                SmFil = postMain.SmFil,
                                DepartmentBase = postMain.DepartmentBase
                            }).ToListAsync();

                list = query.ToList();
            }

            return list;
        }
    }
}