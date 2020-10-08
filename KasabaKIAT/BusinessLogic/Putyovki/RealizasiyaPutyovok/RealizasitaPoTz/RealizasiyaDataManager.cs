using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Putyovki;
using System.Collections.Generic;
using System.Linq;

namespace KasabaKIAT.BusinessLogic.Putyovki.RealizasiyaPutyovok.RealizasitaPoTz
{
    public class RealizasiyaDataManager
    {
        public static void Create(ref RealizasiyaByTzViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbpt_putyovkarealizasiya put = new tbpt_putyovkarealizasiya();

                put.DEPARTMENT_CODE         = model.department_code         ;
                put.TO_DEPARTMENT_CODE      = model.to_department_code      ;
                put.USER_ID                 = model.user_id                 ;
                put.SANATORIUM_ID           = model.sanatorium_id           ;
                put.CAMP_ID                 = model.camp_id                 ;
                put.SERAIL_NUMBER           = model.serail_number           ;
                put.REST_BEGIN_DATE         = model.rest_begin_date         ;
                put.REST_END_DATE           = model.rest_end_date           ;
                put.FULL_NAME               = model.full_name               ;
                put.PRICE                   = model.price                   ;
                put.PAYMENT_PERSENT         = model.payment_persent         ;
                put.RECIEVER                = model.reciever                ;
                put.DOVERENNOST_NUMBER      = model.doverennost_number      ;
                put.DOVER_ISSUE_DATE        = model.dover_issue_date        ;
                put.DOVER_EXPIRE_DATE       = model.dover_expire_date       ;
                put.QUANTITY                = model.quantity                ;
                put.ZAYAVKA_ID              = model.zayavka_id              ;
                put.PLATEJ_DATE             = model.platej_date             ;
                put.PLATEJ_AMOUNT           = model.platej_amount           ;
                put.PLATEJ_NUMBER           = model.platej_number           ;
                put.TALON_BACK_DATE         = model.talon_back_date         ;
                put.REMARK                  = model.remark                  ;
                put.ORG_INN                 = model.org_inn;
                put.NAKLADNOY_NOMER         = model.nakladnoy_nomer;
                put.STATUS_CODE             = true;

                var pst = db.tbpt_putyovkarealizasiya.Add(put);
                
                db.SaveChanges();

                model.rec_id = put.REC_ID;
            }
        }

        public static void Modify(RealizasiyaByTzViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var put = db.tbpt_putyovkarealizasiya.Where(z => z.REC_ID == model.rec_id);

                if (put.Any())
                {
                    var put1 = put.FirstOrDefault();

                    if (put1 != null)
                    {
                        put1.PAYMENT_PERSENT = model.payment_persent;
                    }
                }

                db.SaveChanges();
            }
        }


        public static void ModifyDetails(RealizasiyaByTzViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var put = db.tbpt_putyovkarealizasiya.Where(z => z.REC_ID == model.rec_id);

                if (put.Any())
                {
                    var put1 = put.FirstOrDefault();

                    if (put1 != null)
                    {
                        put1.PAYMENT_PERSENT = model.payment_persent;
                    }
                }

                db.SaveChanges();
            }
        }


        public static void VozvratReal(RealizasiyaByTzViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var put = db.tbpt_putyovkarealizasiya.Where(z => z.REC_ID == model.rec_id);

                if (put.Any())
                {
                    var put1 = put.FirstOrDefault();

                    if (put1 != null)
                    {
                        put1.STATUS_CODE = false;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(RealizasiyaByTzViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var vipl = db.tbpt_putyovkarealizasiya.Where(z => z.REC_ID == model.rec_id);

                if (vipl.Any())
                {
                    var reg = vipl.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tbpt_putyovkarealizasiya.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<RealizasiyaByTzViewModel> Get(RealizasiyaByTzViewModel model)
        {
            List<RealizasiyaByTzViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from vipl in db.tbpt_putyovkarealizasiya
                            where vipl.REC_ID == model.rec_id
                            select new RealizasiyaByTzViewModel
                            {
                                rec_id                     = vipl.REC_ID                               ,
                                department_code            = vipl.DEPARTMENT_CODE             ,
                                to_department_code         = vipl.TO_DEPARTMENT_CODE       ,
                                user_id                    = vipl.USER_ID                             ,
                                sanatorium_id              = vipl.SANATORIUM_ID                 ,
                                camp_id                    = vipl.CAMP_ID                             ,
                                serail_number              = vipl.SERAIL_NUMBER                 ,
                                rest_begin_date            = vipl.REST_BEGIN_DATE             ,
                                rest_end_date              = vipl.REST_END_DATE                 ,
                                full_name                  = vipl.FULL_NAME                         ,
                                price                      = vipl.PRICE                                 ,
                                payment_persent            = vipl.PAYMENT_PERSENT             ,
                                reciever                   = vipl.RECIEVER                           ,
                                doverennost_number         = vipl.DOVERENNOST_NUMBER       ,
                                dover_issue_date           = vipl.DOVER_ISSUE_DATE           ,
                                dover_expire_date          = vipl.DOVER_EXPIRE_DATE         ,
                                quantity                   = vipl.QUANTITY                           ,
                                zayavka_id                 = vipl.ZAYAVKA_ID                       ,
                                platej_date                = vipl.PLATEJ_DATE                     ,
                                platej_amount              = vipl.PLATEJ_AMOUNT                 ,
                                platej_number              = vipl.PLATEJ_NUMBER                 ,
                                talon_back_date            = vipl.TALON_BACK_DATE             ,
                                remark                     = vipl.REMARK                               ,
                                org_inn                    = vipl.ORG_INN                             ,
                                nakladnoy_nomer            = vipl.NAKLADNOY_NOMER             ,
                                status_code           = vipl.STATUS_CODE
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}