using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Security.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Security.Roles.Roles
{
    public class RoleDataManager
    {
        public static void Create(ref RoleViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                roles reg = new roles();

                reg.id          = model.id          ;
                reg.version     = 0     ;
                reg.name        = model.name        ;
                reg.s_comment   = model.s_comment   ;
                reg.s_create    = model.s_create    ;
                reg.s_status    = model.s_status    ;
                reg.s_user_id   = model.s_user_id   ;
                reg.NATIVE_ID = null;
                db.roles.Add(reg);
                db.SaveChanges();
            }
        }

        public static void Modify(RoleViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.roles.Where(z => z.id == model.id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.name = model.name;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(RoleViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.roles.Where(z => z.id == model.id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.roles.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<RoleViewModel> Get(RoleViewModel model)
        {
            List<RoleViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.roles
                            select new RoleViewModel
                            {
                                id = bank.id,
                                name = bank.name
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}