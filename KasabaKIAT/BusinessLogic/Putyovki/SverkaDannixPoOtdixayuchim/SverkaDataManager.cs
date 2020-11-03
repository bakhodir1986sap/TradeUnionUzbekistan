using KasabaKIAT.Models;
using KasabaKiat.ViewModel.Putyovki;
using System.Collections.Generic;
using System.Linq;

namespace KasabaKiat.BusinessLogic.SverkaDannixPoOtdixayuchim
{
    public class SverkaDataManager
    {
        public static void Create(ref SverkaViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbpt_sanatory_info sanInfo = new tbpt_sanatory_info();

                sanInfo.USER_ID             = model.user_id             ;
                sanInfo.SERIAL_NUMBER       = model.serial_number       ;
                sanInfo.REST_ORDER_BEGIN_DT = model.rest_order_begin_dt ;
                sanInfo.REST_ORDER_END_DT   = model.rest_order_end_dt   ;
                sanInfo.FACT_REST_BEGIN_DT  = model.fact_rest_begin_dt  ;
                sanInfo.FACT_REST_END_DT    = model.fact_rest_end_dt    ;
                sanInfo.FULL_NAME           = model.full_name           ;
                sanInfo.PASSPORT_NUMBER     = model.passport_number     ;
                sanInfo.BIRTH_YEAR          = model.birth_year          ;
                sanInfo.RESIDENCE_PLACE     = model.residence_place     ;
                sanInfo.WORK_PALCE          = model.work_palce          ;
                sanInfo.YULLANMA_BERGAN     = model.yullanma_bergan     ;
                sanInfo.IJTIMOIY_TARKIB     = model.ijtimoiy_tarkib     ;
                sanInfo.DAYS_BY_PUTYOVKA    = model.days_by_putyovka    ;
                sanInfo.DAYS_LATE           = model.days_late           ;
                sanInfo.DAYS_EARLY          = model.days_early          ;
                sanInfo.DAYS_PROLONG        = model.days_prolong        ;
                sanInfo.DAYS_MOVED          = model.days_moved          ;
                sanInfo.DAYS_ALL            = model.days_all            ;
                sanInfo.PUTYOVKA_PRICE      = model.putyovka_price      ;
                sanInfo.ADDITIONAL_PAYMENTS = model.additional_payments ;
                sanInfo.BUILDING            = model.building            ;
                sanInfo.FLAT                = model.flat                ;
                sanInfo.TREATING_DOCTOR     = model.treating_doctor     ;
                sanInfo.TALON_BACK          = model.talon_back          ;
                sanInfo.DEPARTMENT_CODE     = model.department_code     ;

                var sanIi = db.tbpt_sanatory_info.Add(sanInfo);

                model.rec_id = sanIi.REC_ID;

                db.SaveChanges();
            }
        }

        public static void Modify(SverkaViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var sanInfo = db.tbpt_sanatory_info.Where(z => z.REC_ID == model.rec_id);

                if (sanInfo.Any())
                {
                    var sanInfo1 = sanInfo.FirstOrDefault();

                    if (sanInfo1 != null)
                    {
                        sanInfo1.USER_ID = model.user_id;
                        sanInfo1.SERIAL_NUMBER = model.serial_number;
                        sanInfo1.REST_ORDER_BEGIN_DT = model.rest_order_begin_dt;
                        sanInfo1.REST_ORDER_END_DT = model.rest_order_end_dt;
                        sanInfo1.FACT_REST_BEGIN_DT = model.fact_rest_begin_dt;
                        sanInfo1.FACT_REST_END_DT = model.fact_rest_end_dt;
                        sanInfo1.FULL_NAME = model.full_name;
                        sanInfo1.PASSPORT_NUMBER = model.passport_number;
                        sanInfo1.BIRTH_YEAR = model.birth_year;
                        sanInfo1.RESIDENCE_PLACE = model.residence_place;
                        sanInfo1.WORK_PALCE = model.work_palce;
                        sanInfo1.YULLANMA_BERGAN = model.yullanma_bergan;
                        sanInfo1.IJTIMOIY_TARKIB = model.ijtimoiy_tarkib;
                        sanInfo1.DAYS_BY_PUTYOVKA = model.days_by_putyovka;
                        sanInfo1.DAYS_LATE = model.days_late;
                        sanInfo1.DAYS_EARLY = model.days_early;
                        sanInfo1.DAYS_PROLONG = model.days_prolong;
                        sanInfo1.DAYS_MOVED = model.days_moved;
                        sanInfo1.DAYS_ALL = model.days_all;
                        sanInfo1.PUTYOVKA_PRICE = model.putyovka_price;
                        sanInfo1.ADDITIONAL_PAYMENTS = model.additional_payments;
                        sanInfo1.BUILDING = model.building;
                        sanInfo1.FLAT = model.flat;
                        sanInfo1.TREATING_DOCTOR = model.treating_doctor;
                        sanInfo1.TALON_BACK = model.talon_back;
                        sanInfo1.DEPARTMENT_CODE = model.department_code;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(SverkaViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var sanInfo = db.tbpt_sanatory_info.Where(z => z.REC_ID == model.rec_id);

                if (sanInfo.Any())
                {
                    var reg = sanInfo.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tbpt_sanatory_info.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<SverkaViewModel> Get(SverkaViewModel model)
        {
            List<SverkaViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from sanInfo in db.tbpt_sanatory_info
                            select new SverkaViewModel
                            {
                                rec_id              = sanInfo.REC_ID                ,
                                user_id             = sanInfo.USER_ID               ,
                                serial_number       = sanInfo.SERIAL_NUMBER         ,
                                rest_order_begin_dt = sanInfo.REST_ORDER_BEGIN_DT   ,
                                rest_order_end_dt   = sanInfo.REST_ORDER_END_DT     ,
                                fact_rest_begin_dt  = sanInfo.FACT_REST_BEGIN_DT    ,
                                fact_rest_end_dt    = sanInfo.FACT_REST_END_DT      ,
                                full_name           = sanInfo.FULL_NAME             ,
                                passport_number     = sanInfo.PASSPORT_NUMBER       ,
                                birth_year          = sanInfo.BIRTH_YEAR            ,
                                residence_place     = sanInfo.RESIDENCE_PLACE       ,
                                work_palce          = sanInfo.WORK_PALCE            ,
                                yullanma_bergan     = sanInfo.YULLANMA_BERGAN       ,
                                ijtimoiy_tarkib     = sanInfo.IJTIMOIY_TARKIB       ,
                                days_by_putyovka    = sanInfo.DAYS_BY_PUTYOVKA      ,
                                days_late           = sanInfo.DAYS_LATE             ,
                                days_early          = sanInfo.DAYS_EARLY            ,
                                days_prolong        = sanInfo.DAYS_PROLONG          ,
                                days_moved          = sanInfo.DAYS_MOVED            ,
                                days_all            = sanInfo.DAYS_ALL              ,
                                putyovka_price      = sanInfo.PUTYOVKA_PRICE        ,
                                additional_payments = sanInfo.ADDITIONAL_PAYMENTS   ,
                                building            = sanInfo.BUILDING              ,
                                flat                = sanInfo.FLAT                  ,
                                treating_doctor     = sanInfo.TREATING_DOCTOR       ,
                                talon_back          = sanInfo.TALON_BACK            ,
                                department_code     = sanInfo.DEPARTMENT_CODE     
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}