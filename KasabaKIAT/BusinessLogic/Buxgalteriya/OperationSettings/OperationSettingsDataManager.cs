using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Buxgalteriya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.OperationSettings
{
    public class OperationSettingsDataManager
    {
        public static void Create(ref OperationSettingsViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbgl_opersettings reg = new tbgl_opersettings();

                reg.OPER_NAME       = model.oper_name           ;
                reg.OPER_TYPE       = model.oper_type           ;
                reg.DEBIT_ACCOUNT   = model.debit_account       ;
                reg.CREDIT_ACCOUNT  = model.credit_account      ;
                reg.STATUS          = model.status              ;
                reg.DEPARTMENT_CODE = model.department_code     ;
                reg.USER_ID         = model.user_id                 ;


                var planOtrasl = db.tbgl_opersettings.Add(reg);

                db.SaveChanges();

                model.rec_id = reg.REC_ID;
            }
        }

        public static void Modify(OperationSettingsViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbgl_opersettings.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.OPER_NAME = model.oper_name;
                        reg.OPER_TYPE = model.oper_type;
                        reg.DEBIT_ACCOUNT = model.debit_account;
                        reg.CREDIT_ACCOUNT = model.credit_account;
                        reg.STATUS = model.status;
                        reg.DEPARTMENT_CODE = model.department_code;
                        reg.USER_ID = model.user_id;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(OperationSettingsViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbgl_opersettings.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tbgl_opersettings.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<OperationSettingsViewModel> Get(OperationSettingsViewModel model)
        {
            List<OperationSettingsViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tbgl_opersettings
                            where bank.DEPARTMENT_CODE == model.department_code
                            select new OperationSettingsViewModel
                            {
                                rec_id = bank.REC_ID,
                                oper_name       = bank.OPER_NAME        ,
                                oper_type       = bank.OPER_TYPE        ,
                                debit_account   = bank.DEBIT_ACCOUNT    ,
                                credit_account  = bank.CREDIT_ACCOUNT   ,
                                status          = bank.STATUS           ,
                                department_code = bank.DEPARTMENT_CODE  ,
                                user_id         = bank.USER_ID        
                            };

                list = query.ToList();
            }

            return list;
        }


    }
}