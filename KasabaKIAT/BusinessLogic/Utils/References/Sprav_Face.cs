using KasabaKIAT.Models;
using System.Collections.Generic;
using System.Linq;

namespace KasabaKiat.BusinessLogic.Utils.References
{
    public class Sprav_Face
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Create(ref Sprav_FaceViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblSprav_Face face = new tblSprav_Face();

                face.Name_Os = model.Name_Os;
                face.UserId = model.UserId;

                var faceIns = db.tblSprav_Face.Add(face);

                model.ID = faceIns.ID;

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Modify(Sprav_FaceViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var face = db.tblSprav_Face.Where(z => z.ID == model.ID);

                if (face.Any())
                {
                    var face1 = face.FirstOrDefault();

                    if (face1 != null)
                    {
                        face1.Name_Os = model.Name_Os;
                        face1.UserId = model.UserId;
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Remove(Sprav_FaceViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var face = db.tblSprav_Face.Where(z => z.ID == model.ID);

                if (face.Any())
                {
                    var face1 = face.FirstOrDefault();

                    if (face1 != null)
                    {
                        db.tblSprav_Face.Remove(face1);
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
        public static List<Sprav_FaceViewModel> Get(Sprav_FaceViewModel model)
        {
            List<Sprav_FaceViewModel> list = null; 

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from face in db.tblSprav_Face
                               select new Sprav_FaceViewModel
                               {
                                   Name_Os = face.Name_Os,
                                   UserId = face.UserId
                               };

                list = queryRes.ToList();

            }

            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<Sprav_FaceViewModel> Get()
        {
            List<Sprav_FaceViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from face in db.tblSprav_Face
                               select new Sprav_FaceViewModel
                               {
                                   ID       = face.ID,
                                   Name_Os  = face.Name_Os
                               };

                list = queryRes.ToList();

            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static string GetFaceById(long Id)
        {
            string res = string.Empty;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var face = db.tblSprav_Face.Where(z => z.ID == Id);

                if (face.Any())
                {
                    res = face.FirstOrDefault().Name_Os;
                }
            }

            return res;
        }

    }
}