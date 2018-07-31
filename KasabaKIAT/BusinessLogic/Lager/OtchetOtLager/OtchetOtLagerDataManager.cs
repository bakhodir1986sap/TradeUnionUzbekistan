using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Lager.OtchetOtLager
{
    public class OtchetOtLagerDataManager
    {
        public static void Create(OtchetOtLagerViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblOtchetiLager zaez = new tblOtchetiLager();

                zaez.ID_ORG             = model.id_org.Value;
                zaez.Quantuty           = model.quantuty;
                zaez.TotalAmount        = model.totalamount;
                zaez.TIMESTAMP          = DateTime.Now;
                zaez.STATUS             = (int)CampOtchetStatusType.New;
                zaez.DEPARTMENT_CODE    = model.department_code ;
                zaez.UserId             = model.userid;

                db.tblOtchetiLagers.Add(zaez);

                db.SaveChanges();

            }
        }

        public static void Update(OtchetOtLagerViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var zaezdi = db.tblOtchetiLagers.Where(z => z.REC_ID == model.rec_id);

                if (zaezdi.Any())
                {
                    var zaez = zaezdi.FirstOrDefault();

                    if (zaez != null)
                    {
                        zaez.ID_ORG = model.id_org.Value;
                        zaez.Quantuty = model.quantuty;
                        zaez.TotalAmount = model.totalamount;
                        zaez.TIMESTAMP = DateTime.Now;
                        zaez.STATUS = model.status;
                        zaez.DEPARTMENT_CODE = model.department_code;
                        zaez.UserId = model.userid;
                    }
                }

                db.SaveChanges();
            }
        }


        public static void UpdateStatus(OtchetOtLagerViewModel model, CampOtchetStatusType status)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var zaezdi = db.tblOtchetiLagers.Where(z => z.REC_ID == model.rec_id);

                if (zaezdi.Any())
                {
                    var zaez = zaezdi.FirstOrDefault();

                    if (zaez != null)
                    {
                        zaez.TIMESTAMP =DateTime.Now;
                        zaez.STATUS = (int)status;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(OtchetOtLagerViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var zaezdi = db.tblOtchetiLagers.Where(z => z.REC_ID == model.rec_id);

                if (zaezdi.Any())
                {
                    var reg = zaezdi.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblOtchetiLagers.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<OtchetOtLagerViewModel> Get(OtchetOtLagerViewModel model)
        {
            List<OtchetOtLagerViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from zaezdi in db.tblOtchetiLagers
                            join organiz in db.tblSprav_Org on zaezdi.ID_ORG equals organiz.ID
                            where zaezdi.DEPARTMENT_CODE == model.department_code
                            && zaezdi.ID_ORG == (model.id_org.HasValue ? model.id_org : zaezdi.ID_ORG)
                            select new OtchetOtLagerViewModel
                            {
                               rec_id           = zaezdi.REC_ID         ,
                               id_org           = zaezdi.ID_ORG         ,
                               quantuty         = zaezdi.Quantuty       ,
                               totalamount      = zaezdi.TotalAmount    ,
                               timestamp        = zaezdi.TIMESTAMP      ,
                               status           = zaezdi.STATUS         ,
                               department_code  = zaezdi.DEPARTMENT_CODE,
                               userid = zaezdi.UserId
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}