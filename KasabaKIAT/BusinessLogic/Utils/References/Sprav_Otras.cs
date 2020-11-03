using KasabaKIAT.Models;
using System.Collections.Generic;
using System.Linq;

namespace KasabaKiat.BusinessLogic.Utils.References
{
    /// <summary>
    /// 
    /// </summary>
    public class Sprav_Otras
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Create(ref Sprav_OtrasViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblSprav_Otras otras = new tblSprav_Otras();

                otras.ID_Ot     = model.ID_Ot   ;
                otras.Name_Ot   = model.Name_Ot ;
                otras.Priz      = model.Priz    ;
                otras.UserId = model.UserId;

                var otrasIns = db.tblSprav_Otras.Add(otras);

                model.ID = otrasIns.ID;

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Modify(Sprav_OtrasViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var otras = db.tblSprav_Otras.Where(z => z.ID == model.ID);

                if (otras.Any())
                {
                    var otras1 = otras.FirstOrDefault();

                    if (otras1 != null)
                    {
                        otras1.ID_Ot = model.ID_Ot;
                        otras1.Name_Ot = model.Name_Ot;
                        otras1.Priz = model.Priz;
                        otras1.UserId = model.UserId;
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Remove(Sprav_OtrasViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var otras = db.tblSprav_Otras.Where(z => z.ID == model.ID);

                if (otras.Any())
                {
                    var otras1 = otras.FirstOrDefault();

                    if (otras1 != null)
                    {
                        db.tblSprav_Otras.Remove(otras1);
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
        public static List<Sprav_OtrasViewModel> Get(Sprav_OtrasViewModel model)
        {
            List<Sprav_OtrasViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from otras in db.tblSprav_Otras
                               select new Sprav_OtrasViewModel
                               {
                                   ID = otras.ID,
                                   ID_Ot = otras.ID_Ot,
                                   Name_Ot = otras.Name_Ot,
                                   Priz = otras.Priz,
                                   UserId = otras.UserId,
                                   DepartmentBase = otras.DepartmentBase
                               };

                list = queryRes.ToList();

            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        public static List<Sprav_OtrasViewModel> GetByDepartmentBase(string departmentBase)
        {
            List<Sprav_OtrasViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                if (departmentBase.Equals("17"))
                {
                    var queryRes = from otras in db.tblSprav_Otras
                                   group otras by new { otras.ID_Ot , otras.Name_Ot } into g
                                   //where otras.DepartmentBase == departmentBase
                                   orderby g.Key.Name_Ot
                                   select new Sprav_OtrasViewModel
                                   {
                                       ID_Ot = g.Key.ID_Ot,
                                       Name_Ot = g.Key.Name_Ot,
                                   };

                    list = queryRes.ToList();
                }
                else
                {
                    var queryRes = from otras in db.tblSprav_Otras
                                   where otras.DepartmentBase == departmentBase
                                   orderby otras.Name_Ot
                                   select new Sprav_OtrasViewModel
                                   {
                                       ID_Ot = otras.ID_Ot,
                                       Name_Ot = otras.Name_Ot,
                                   };

                    list = queryRes.ToList();
                }
            }

            return list;
        }
    }
}