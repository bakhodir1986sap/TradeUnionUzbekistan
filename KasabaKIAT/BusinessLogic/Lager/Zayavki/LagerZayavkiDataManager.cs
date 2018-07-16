using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Lager.Zayavki
{
    public class LagerZayavkiDataManager
    {
        public static void Create(LagerZayavkiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblZayavkiLager zaez = new tblZayavkiLager();
     
                zaez.ID_Otras       = model.id_otras        ;
                zaez.ID_Camp        = model.id_camp         ;
                zaez.Quantuty       = model.quantuty        ;
                zaez.TotalAmount    = model.totalamount     ;
                zaez.TIMESTAMP      = model.timestamp       ;
                zaez.STATUS         = (int)CampZayavkaStatusType.New;
                zaez.DEPARTMENT_CODE= model.department_code ;
                zaez.UserId = model.userid;

                db.tblZayavkiLagers.Add(zaez);

                db.SaveChanges();

            }
        }

        public static void Update(LagerZayavkiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var zaezdi = db.tblZayavkiLagers.Where(z => z.REC_ID == model.rec_id);

                if (zaezdi.Any())
                {
                    var zaez = zaezdi.FirstOrDefault();

                    if (zaez != null)
                    {
                        zaez.ID_Otras = model.id_otras;
                        zaez.ID_Camp = model.id_camp;
                        zaez.Quantuty = model.quantuty;
                        zaez.TotalAmount = model.totalamount;
                        zaez.TIMESTAMP = model.timestamp;
                        zaez.STATUS = model.status;
                        zaez.DEPARTMENT_CODE = model.department_code;
                        zaez.UserId = model.userid;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void UpdateStatus(LagerZayavkiViewModel model, CampZayavkaStatusType status)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var zaezdi = db.tblZayavkiLagers.Where(z => z.REC_ID == model.rec_id);

                if (zaezdi.Any())
                {
                    var zaez = zaezdi.FirstOrDefault();

                    if (zaez != null)
                    {
                        zaez.TIMESTAMP = DateTime.Now;
                        zaez.STATUS = (int)status;

                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(LagerZayavkiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var zaezdi = db.tblZayavkiLagers.Where(z => z.REC_ID == model.rec_id);

                if (zaezdi.Any())
                {
                    var reg = zaezdi.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblZayavkiLagers.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<LagerZayavkiViewModel> Get(LagerZayavkiViewModel model)
        {
            List<LagerZayavkiViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from zaezdi in db.tblZayavkiLagers
                            where zaezdi.DEPARTMENT_CODE == model.department_code
                            select new LagerZayavkiViewModel
                            {
                                rec_id          = zaezdi.REC_ID             ,
                                id_otras        = zaezdi.ID_Otras           ,
                                id_camp         = zaezdi.ID_Camp            ,
                                quantuty        = zaezdi.Quantuty           ,
                                totalamount     = zaezdi.TotalAmount        ,
                                timestamp       = zaezdi.TIMESTAMP          ,
                                status          = zaezdi.STATUS             ,
                                department_code = zaezdi.DEPARTMENT_CODE    ,
                                userid          = zaezdi.UserId         
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}