using KaznaFilesImporter;
using KasabaKIAT.ViewModel.Buxgalteriya;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki
{
    public class ProvodkaSettingViewModel
    {
        public long rec_id { get; set; }
        public string department_base { get; set; }
        public long? user_id { get; set; }
        public string code { get; set; }
        public string debit_acc { get; set; }
        public string creadit_acc { get; set; }
    }

    public class ProvodkaSettingsDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Create(ref ProvodkaSettingViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbbu_nastroykiProvodok reg = new tbbu_nastroykiProvodok();

                reg.DEPARTMENT_BASE = model.department_base;
                reg.USER_ID = model.user_id;
                reg.CODE = model.code;
                reg.DEBIT_ACC = model.debit_acc;
                reg.CREADIT_ACC = model.creadit_acc;

                var planOtrasl = db.tbbu_nastroykiProvodok.Add(reg);

                try
                {
                    db.SaveChanges();

                    model.rec_id = reg.REC_ID;
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
        public static void Modify(ProvodkaSettingViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbbu_nastroykiProvodok.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.DEPARTMENT_BASE = model.department_base;
                        reg.USER_ID = model.user_id;
                        reg.CODE = model.code;
                        reg.DEBIT_ACC = model.debit_acc;
                        reg.CREADIT_ACC = model.creadit_acc;
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Remove(ProvodkaSettingViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbbu_nastroykiProvodok.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tbbu_nastroykiProvodok.Remove(reg);
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
        public static List<ProvodkaSettingViewModel> Get(ProvodkaSettingViewModel model)
        {
            List<ProvodkaSettingViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tbbu_nastroykiProvodok
                            where bank.DEPARTMENT_BASE == model.department_base
                            && bank.CODE == model.code
                            select new ProvodkaSettingViewModel
                            {
                                department_base = bank.DEPARTMENT_BASE,
                                user_id = bank.USER_ID,
                                code = bank.CODE,
                                debit_acc = bank.DEBIT_ACC,
                                creadit_acc = bank.CREADIT_ACC,
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}