using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Buxgalteriya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Buxgalteriya.Balance.Transactions
{
    public class JournalMasterDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Create(ref MasterViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbgl_jnlmst reg = new tbgl_jnlmst();

                reg.TRDT = model.trdt;
                reg.DEPARTMENT_BASE = model.department_base;
                reg.DYSEQ = model.dyseq;
                reg.OPER_ID = model.oper_id;
                reg.TRTP = model.trtp;
                reg.ORGTRDT = model.orgtrdt;
                reg.ORGORG_ID = model.orgorg_id;
                reg.ORGCRT_USR_ID = model.orgcrt_usr_id;
                reg.ORGDYSEQ = model.orgdyseq;
                reg.REMARK = model.remark;
                reg.CRTUSER_ID = model.crtuser_id;
                reg.REC_ID = model.rec_id;

                var planOtrasl = db.tbgl_jnlmst.Add(reg);

                db.SaveChanges();

                model.rec_id = reg.REC_ID;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Modify(MasterViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbgl_jnlmst.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.TRDT = model.trdt;
                        reg.DEPARTMENT_BASE = model.department_base;
                        reg.DYSEQ = model.dyseq;
                        reg.OPER_ID = model.oper_id;
                        reg.TRTP = model.trtp;
                        reg.ORGTRDT = model.orgtrdt;
                        reg.ORGORG_ID = model.orgorg_id;
                        reg.ORGCRT_USR_ID = model.orgcrt_usr_id;
                        reg.ORGDYSEQ = model.orgdyseq;
                        reg.REMARK = model.remark;
                        reg.CRTUSER_ID = model.crtuser_id;
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Remove(MasterViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbgl_jnlmst.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tbgl_jnlmst.Remove(reg);
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
        public static List<MasterViewModel> Get(MasterViewModel model)
        {
            List<MasterViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tbgl_jnlmst
                            where bank.DEPARTMENT_BASE == model.department_base
                            && bank.TRDT == model.trdt
                            select new MasterViewModel
                            {
                                trdt = bank.TRDT,
                                department_base = bank.DEPARTMENT_BASE,
                                dyseq = bank.DYSEQ,
                                oper_id = bank.OPER_ID,
                                trtp = bank.TRTP,
                                orgtrdt = bank.ORGTRDT,
                                orgorg_id = bank.ORGORG_ID,
                                orgcrt_usr_id = bank.ORGCRT_USR_ID,
                                orgdyseq = bank.ORGDYSEQ,
                                remark = bank.REMARK,
                                crtuser_id = bank.CRTUSER_ID,
                                rec_id = bank.REC_ID
                            };

                list = query.ToList();
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="department_code"></param>
        /// <param name="beginDt"></param>
        /// <param name="endDt"></param>
        /// <returns></returns>
        public static List<MasterViewModel> Get(string department_code, DateTime beginDt, DateTime endDt)
        {
            List<MasterViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tbgl_jnlmst
                            where bank.DEPARTMENT_BASE == department_code
                            && bank.TRDT >= beginDt && bank.TRDT <= endDt
                            select new MasterViewModel
                            {
                                trdt = bank.TRDT,
                                department_base = bank.DEPARTMENT_BASE,
                                dyseq = bank.DYSEQ,
                                oper_id = bank.OPER_ID,
                                trtp = bank.TRTP,
                                orgtrdt = bank.ORGTRDT,
                                orgorg_id = bank.ORGORG_ID,
                                orgcrt_usr_id = bank.ORGCRT_USR_ID,
                                orgdyseq = bank.ORGDYSEQ,
                                remark = bank.REMARK,
                                crtuser_id = bank.CRTUSER_ID,
                                rec_id = bank.REC_ID
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}