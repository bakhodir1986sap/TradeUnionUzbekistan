using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.RealizasiyaPutyovok.RealizasitaPoTz
{
    public class RealizasiyaDataManager
    {
        public static void Create(ref RealizasiyaByTzViewModel model)
        {
            var numGen = new ViplNumGen();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbpt_putyovkarealizasiya put = new tbpt_putyovkarealizasiya();

                //put.REC_ID                  = 
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
                put.ORG_INN = model.org_inn;
                put.NAKLADNOY_NOMER = numGen.GetNextNumDoc(model.to_department_code, DateTime.Now.Year).
                    ToString().PadLeft(6, '0');
                put.STATUS_CODE = true;

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

            //using (KasabaKIATEntities db = new KasabaKIATEntities())
            //{
            //    var query = from vipl in db.tblVipl
            //                select new RealizasiyaByTzViewModel
            //                {
            //                    id_org = vipl.ID_Org,
            //                    data_post = vipl.Data_Post,
            //                    num_doc = vipl.Num_Doc,
            //                    id_pok = vipl.ID_Pok,
            //                    num_kol = vipl.Num_Kol,
            //                    symma = vipl.Symma,
            //                    name_dov = vipl.Name_Dov,
            //                    id_kv = vipl.ID_Kv,
            //                    vid = vipl.Vid,
            //                    smfil = vipl.SmFil,
            //                    userid = vipl.UserId,
            //                    departmentbase = vipl.DepartmentBase
            //                };

            //    list = query.ToList();
            //}

            return list;
        }
    }
}