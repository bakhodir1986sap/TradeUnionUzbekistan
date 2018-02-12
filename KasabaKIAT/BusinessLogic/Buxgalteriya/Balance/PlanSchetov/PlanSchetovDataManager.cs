using KasabaKIAT.Models;
using KasabaKiat.ViewModel.Buxgalteriya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Buxgalteriya.Balance.PlanSchetov
{
    public class PlanSchetovDataManager
    {
        public static void Create(ref PlanSchetovViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbgl_accounttype acttp = new tbgl_accounttype();

                acttp.ACCOUNT_NAME      = model.account_name    ;
                acttp.ACCOUNT_CODE      = model.account_code    ;
                acttp.ACCOUNT_TYPE      = model.account_type    ;
                acttp.STATUS            = model.status          ;
                acttp.UserUd            = model.userud          ;
                acttp.DepartmentBase    = model.departmentbase  ;

                var osIns = db.tbgl_accounttype.Add(acttp);

                db.SaveChanges();

                model.rec_id = acttp.REC_ID;
            }
        }

        public static void Modify(PlanSchetovViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var acttp = db.tbgl_accounttype.Where(z => z.REC_ID == model.rec_id);

                if (acttp.Any())
                {
                    var acttp1 = acttp.FirstOrDefault();

                    if (acttp1 != null)
                    {
                        acttp1.ACCOUNT_NAME = model.account_name;
                        acttp1.ACCOUNT_CODE = model.account_code;
                        acttp1.ACCOUNT_TYPE = model.account_type;
                        acttp1.STATUS = model.status;
                        acttp1.UserUd = model.userud;
                        acttp1.DepartmentBase = model.departmentbase;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(PlanSchetovViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var os = db.tbgl_accounttype.Where(z => z.REC_ID == model.rec_id);

                if (os.Any())
                {
                    var os1 = os.FirstOrDefault();

                    if (os1 != null)
                    {
                        db.tbgl_accounttype.Remove(os1);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<PlanSchetovViewModel> Get(PlanSchetovViewModel model)
        {
            List<PlanSchetovViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from os in db.tbgl_accounttype
                               select new PlanSchetovViewModel
                               {
                                    rec_id          = os.REC_ID,
                                    account_name    = os.ACCOUNT_NAME   ,
                                    account_code    = os.ACCOUNT_CODE   ,
                                    account_type    = os.ACCOUNT_TYPE   ,
                                    status          = os.STATUS         ,
                                    userud          = os.UserUd         ,
                                    departmentbase  = os.DepartmentBase
                               };

                list = queryRes.ToList();

            }

            return list;
        }
    }
}