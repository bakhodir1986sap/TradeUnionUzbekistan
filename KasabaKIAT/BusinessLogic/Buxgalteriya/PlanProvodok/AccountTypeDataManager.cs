using KasabaKiat.ViewModel.Buxgalteriya;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MoreLinq;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.PlanProvodok
{
    public class AccountTypeDataManager
    {
        public static void Create(ref PlanSchetovViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbgl_accounttype reg = new tbgl_accounttype();

                reg.ACCOUNT_NAME        = model.account_name    ;
                reg.ACCOUNT_CODE        = model.account_code    ;
                reg.ACCOUNT_TYPE        = model.account_type    ;
                reg.STATUS              = model.status          ;
                reg.UserUd              = model.userud          ;
                reg.DepartmentBase      = model.departmentbase  ;

                var planOtrasl = db.tbgl_accounttype.Add(reg);

                db.SaveChanges();

                model.rec_id = reg.REC_ID;
            }
        }

        public static void Modify(PlanSchetovViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbgl_accounttype.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.ACCOUNT_NAME = model.account_name;
                        reg.ACCOUNT_CODE = model.account_code;
                        reg.ACCOUNT_TYPE = model.account_type;
                        reg.STATUS = model.status;
                        reg.UserUd = model.userud;
                        reg.DepartmentBase = model.departmentbase;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(PlanSchetovViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbgl_accounttype.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tbgl_accounttype.Remove(reg);
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
                var query = from bank in db.tbgl_accounttype
                            where bank.DepartmentBase == model.departmentbase
                            select new PlanSchetovViewModel
                            {
                                rec_id = bank.REC_ID,
                                account_name  = bank.ACCOUNT_NAME,
                                account_code  = bank.ACCOUNT_CODE,  
                                account_type  = bank.ACCOUNT_TYPE,  
                                status        = bank.STATUS      ,  
                                userud        = bank.UserUd      ,  
                                departmentbase= bank.DepartmentBase
                            };

                list = query.ToList();
            }

            return list;
        }


        public static List<PlanSchetovViewModel> Get(string accountCode)
        {
            List<PlanSchetovViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tbgl_accounttype
                            where bank.ACCOUNT_CODE == accountCode
                            select new PlanSchetovViewModel
                            {
                                rec_id = bank.REC_ID,
                                account_name = bank.ACCOUNT_NAME,
                                account_code = bank.ACCOUNT_CODE,
                                account_type = bank.ACCOUNT_TYPE,
                                status = bank.STATUS,
                                userud = bank.UserUd,
                                departmentbase = bank.DepartmentBase
                            };

                list = query.ToList();
            }

            return list;
        }


        public static List<string> GetDebitSchets()
        {
            List<string> list = null;
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var provList = db.tbbu_balance.DistinctBy(z => z.ACCOUNT_CODE.Trim()).Select(z => z.ACCOUNT_CODE);

                list = provList.ToList();
            }

            return list;
        }

        public static List<string> GetCreditSchets()
        {
            List<string> list = null;
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var provList = db.tbbu_balance.DistinctBy(z => z.ACCOUNT_CODE.Trim()).Select(z=>z.ACCOUNT_CODE);


                list = provList.ToList();
            }

            return list;
        }


    }
}