using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Security.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Security.RolePermissions
{
    public class RolePermissionDataManager
    {
        public static void Create(ref RolePermissionViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                role_permission reg = new role_permission();

                //.id = model.id;
                reg.version = 0;
                reg.action = model.action;
                reg.s_comment = model.s_comment;
                reg.s_create = model.s_create;
                reg.s_status = model.s_status;
                reg.s_user_id = model.s_user_id;
                reg.NATIVE_ID = null;

                db.role_permission.Add(reg);

                var res = db.SaveChanges();
            }
        }

        public static void Modify(RolePermissionViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.role_permission.Where(z => z.id == model.id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.action = model.action;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(RolePermissionViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.role_permission.Where(z => z.id == model.id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.role_permission.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<RolePermissionViewModel> Get()
        {
            List<RolePermissionViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.role_permission
                            where bank.role_id == 1 && bank.action.Contains("Asp.")
                            select new RolePermissionViewModel
                            {
                                id = bank.id,
                                role_id = bank.role_id,
                                action = bank.action
                            };

                list = query.ToList();
            }

            return list;
        }


        public static List<RolePermissionViewModel> Get(long id)
        {
            List<RolePermissionViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.role_permission
                            where bank.id == id && bank.action.Contains("Asp.")
                            select new RolePermissionViewModel
                            {
                                id = bank.id,
                                role_id = bank.role_id,
                                action = bank.action
                            };

                list = query.ToList();
            }

            return list;
        }


        public static List<RolePermissionViewModel> GetForCheck(long role_id, string action)
        {
            List<RolePermissionViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.role_permission
                            where bank.role_id == role_id && bank.action.Contains("Asp.") && bank.action.Contains(action)
                            select new RolePermissionViewModel
                            {
                                id = bank.id,
                                role_id = bank.role_id,
                                action = bank.action
                            };

                list = query.ToList();
            }

            return list;
        }

        //public static List<RolePermissionViewModel> Get(string action,int rol_id)
        //{
        //    List<RolePermissionViewModel> list = null;

        //    using (KasabaKIATEntities db = new KasabaKIATEntities())
        //    {
        //        var query = from bank in db.role_permission
        //                    where bank.id == id && bank.action.Contains("Asp.")
        //                    select new RolePermissionViewModel
        //                    {
        //                        id = bank.id,
        //                        role_id = bank.role_id,
        //                        action = bank.action
        //                    };

        //        list = query.ToList();
        //    }

        //    return list;
        //}
        public static List<RolePermissionViewModel> GetByRole(long role_id)
        {
            List<RolePermissionViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.role_permission
                            where bank.role_id == role_id && bank.action.Contains("Asp.")
                            select new RolePermissionViewModel
                            {
                                id = bank.id,
                                role_id = bank.role_id,
                                action = bank.action
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}