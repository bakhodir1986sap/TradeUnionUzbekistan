using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Buxgalteriya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Buxgalteriya.Balance.Transactions
{
    public class JournalDetailDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Create(ref DetailViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                    tbgl_jnldd reg = new tbgl_jnldd();

                    reg.TRDT                = model.trdt            ;
                    reg.DEPARTMENT_CODE     = model.department_code ;
                    reg.DYSEQ               = model.dyseq           ;
                    reg.DYTRSEQ             = model.dytrseq         ;
                    reg.ACCOUNT             = model.account         ;
                    reg.DRCR                = model.drcr            ;
                    reg.CCY                 = model.ccy             ;
                    reg.AMOUNT              = model.amount          ;
                    reg.REMARK              = model.remark          ;
                    reg.CRTUSER_ID          = model.crtuser_id      ;
                    reg.REC_ID              = model.rec_id          ;
                    reg.INN                 = model.inn             ;

                    var planOtrasl = db.tbgl_jnldd.Add(reg);

                    db.SaveChanges();

                    model.rec_id = reg.REC_ID;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Modify(DetailViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbgl_jnldd.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.TRDT = model.trdt;
                        reg.DEPARTMENT_CODE = model.department_code;
                        reg.DYSEQ = model.dyseq;
                        reg.DYTRSEQ = model.dytrseq;
                        reg.ACCOUNT = model.account;
                        reg.DRCR = model.drcr;
                        reg.CCY = model.ccy;
                        reg.AMOUNT = model.amount;
                        reg.REMARK = model.remark;
                        reg.CRTUSER_ID = model.crtuser_id;
                        //reg.REC_ID = model.rec_id;
                        reg.INN = model.inn;
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Remove(DetailViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbgl_jnldd.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tbgl_jnldd.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Filter by department_base and trdt
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<DetailViewModel> Get(DetailViewModel model)
        {
            List<DetailViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tbgl_jnldd
                            where bank.DEPARTMENT_CODE == model.department_code
                            && bank.TRDT == model.trdt
                            select new DetailViewModel
                            {
                                trdt                = bank.TRDT             ,
                                department_code     = bank.DEPARTMENT_CODE  ,
                                dyseq               = bank.DYSEQ            ,
                                dytrseq             = bank.DYTRSEQ          ,
                                account             = bank.ACCOUNT          ,
                                drcr                = bank.DRCR             ,
                                ccy                 = bank.CCY              ,
                                amount              = bank.AMOUNT           ,
                                remark              = bank.REMARK           ,
                                crtuser_id          = bank.CRTUSER_ID       ,
                                rec_id              = bank.REC_ID           ,
                                inn                 = bank.INN            
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}