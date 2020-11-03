using KasabaKIAT.Models;
using System.Collections.Generic;
using System.Linq;

namespace KasabaKiat.BusinessLogic.Utils.References
{
    public class Sprav_Reg
    {
        public static void Create(ref Sprav_RegViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblSprav_Reg reg = new tblSprav_Reg();

                reg.NameReg = model.NameReg;
                reg.CodeReg = model.CodeReg;
                reg.UserId = model.UserId;

                var regIns = db.tblSprav_Reg.Add(reg);

                model.ID = regIns.ID;

                db.SaveChanges();
            }
        }

        public static void Modify(Sprav_RegViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var reg = db.tblSprav_Reg.Where(z => z.ID == model.ID);

                if (reg.Any())
                {
                    var reg1 = reg.FirstOrDefault();

                    if (reg1 != null)
                    {
                        reg1.NameReg = model.NameReg;
                        reg1.CodeReg = model.CodeReg;
                        reg1.UserId = model.UserId;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(Sprav_RegViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var reg = db.tblSprav_Reg.Where(z => z.ID == model.ID);

                if (reg.Any())
                {
                    var reg1 = reg.FirstOrDefault();

                    if (reg1 != null)
                    {
                        db.tblSprav_Reg.Remove(reg1);
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
        public static List<Sprav_RegViewModel> Get(Sprav_RegViewModel model)
        {
            List<Sprav_RegViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from procent in db.tblSprav_Reg
                               select new Sprav_RegViewModel
                               {
                                   ID       = procent.ID,
                                   NameReg  = procent.NameReg,
                                   CodeReg  = procent.CodeReg,
                                   UserId   = procent.UserId 
                               };

                list = queryRes.ToList();

            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentCode"></param>
        /// <returns></returns>
        public static List<Sprav_RegViewModel> GetByDepartmentCode(string departmentCode , bool IsPriv = false)
        {
            List<Sprav_RegViewModel> list = null;

            string soato = string.Empty;

            if (!string.IsNullOrEmpty(departmentCode))
            {
                if (departmentCode.Length >= 3)
                {
                    soato = departmentCode.Remove(2, 1);
                }
            }

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                if (!IsPriv)
                {
                    var queryRes = from procent in db.tblSprav_Reg
                                   where procent.DepartmentBase.Equals(departmentCode)
                                   select new Sprav_RegViewModel
                                   {
                                       ID = (long)procent.CodeReg,
                                       NameReg = procent.NameReg
                                   };

                    if (queryRes != null)
                    {
                        list = queryRes.ToList();
                    }
                    else
                    {
                        list = new List<Sprav_RegViewModel>();
                    }
                }
                else
                {
                    var queryRes = from procent in db.tblSprav_Reg
                                   where procent.SOATO_CODE.Contains(soato)
                                   select new Sprav_RegViewModel
                                   {
                                       ID = (long)procent.CodeReg,
                                       NameReg = procent.NameReg
                                   };

                    if (queryRes != null)
                    {
                        list = queryRes.ToList();
                    }
                    else
                    {
                        list = new List<Sprav_RegViewModel>();
                    }
                }

                list.Insert( 0,new Sprav_RegViewModel()
                {
                    ID = -1,
                    NameReg = "Пустой"
                });
            }

            return list;
        }
    }
}