using KasabaKIAT.Models;
using KasabaKIAT.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Data.Linq.SqlClient;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Zayavki
{
    public enum ZayavkaStates
    {
        New,
        Sent,
        InProgress,
        Reject,
        Apply,
        Realized
    }

    public class ZayavkiDataManager
    {
        public static void EnterZayavka(ref ZayavkiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbpt_zayavki zayavka = new tbpt_zayavki();

                zayavka.GROUP_ID = CalculateGroupId(model);
                zayavka.MEDICAL_ORDER = model.MedicalOrder;
                zayavka.FULL_NAME = model.FullName;
                zayavka.DEPARTMENT_CODE = model.departmentCode;
                zayavka.USER_ID = model.userid;
                zayavka.POSITION = model.Position;
                zayavka.QUANTITY = 1;
                zayavka.TIP_OTDIXAYUSHEGO = string.Empty;
                zayavka.SANATORY_ID = model.SanatoryId;
                zayavka.ZAEZD_BEGIN = model.ZaezdBegin;
                zayavka.ZAEZD_END = model.ZaezdEnd;
                zayavka.STATUS = "0";//New
                zayavka.CAMP_ID = model.CampId;
                zayavka.TIMESTAMP = DateTime.Now;
                zayavka.KUK_QARORI              = string.Empty;
                zayavka.MEDICAL_ORDER_EXISTS = model.MedicalOrderExists;
                zayavka.ORDER_QUANTITY = 0;

                var inserted = db.tbpt_zayavki.Add(zayavka);

                model.rec_id = inserted.REC_ID;

                db.SaveChanges();
            }
        }

        private static long CalculateGroupId(ZayavkiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var notsentZayavki = from zayavka in db.tbpt_zayavki
                                     where ((zayavka.DEPARTMENT_CODE == model.departmentCode)
                                     && (zayavka.STATUS == "0"))
                                     select zayavka;

                if (notsentZayavki.Any())
                {
                    var firstzayavka = notsentZayavki.First();

                    return firstzayavka.GROUP_ID.Value;
                }
                else
                {
                    var maxGroupId = db.tbpt_zayavki.Where(z => z.DEPARTMENT_CODE == model.departmentCode)
                        .Max(z => z.GROUP_ID);

                    if (maxGroupId.HasValue)
                    {
                        return maxGroupId.Value + 1;
                    }
                }
            }

            return 0;
        }

        public static void ModifyZayavka(ZayavkiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var zayavka = db.tbpt_zayavki.Where(z => (z.REC_ID == model.rec_id) && (z.STATUS == "0")
                                                         && (z.DEPARTMENT_CODE == model.departmentCode))
                    .FirstOrDefault();

                if (zayavka != null)
                {
                    //zayavka.GROUP_ID = CalculateGroupId(model);
                    zayavka.MEDICAL_ORDER = model.MedicalOrder;
                    zayavka.FULL_NAME = model.FullName;
                    zayavka.DEPARTMENT_CODE = model.departmentCode;
                    //zayavka.USER_ID = model.userid;
                    zayavka.POSITION = model.Position;
                    zayavka.QUANTITY = 1;
                    //zayavka.TIP_OTDIXAYUSHEGO = string.Empty;
                    zayavka.SANATORY_ID = model.SanatoryId;
                    zayavka.ZAEZD_BEGIN = model.ZaezdBegin;
                    zayavka.ZAEZD_END = model.ZaezdEnd;
                    //zayavka.STATUS = "0";//New
                    zayavka.CAMP_ID = model.CampId;
                    //zayavka.TIMESTAMP = DateTime.Now;
                    zayavka.KUK_QARORI = model.Kuk_qarori;
                    zayavka.MEDICAL_ORDER_EXISTS = model.MedicalOrderExists;
                    //zayavka.ORDER_QUANTITY = 0;

                    db.SaveChanges();
                }
            }
        }

        public static void DeleteZayavka(ZayavkiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var zayavka = db.tbpt_zayavki.Where(z => (z.REC_ID == model.rec_id) && (z.STATUS == "0")
                                                         && (z.DEPARTMENT_CODE == model.departmentCode))
                    .FirstOrDefault();

                if (zayavka != null)
                {
                    zayavka.STATUS = "-1";//New

                    db.SaveChanges();
                }
            }
        }

        public static void ModifyByStaffZayavka(ZayavkiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var zayavka = db.tbpt_zayavki.Where(z => (z.REC_ID == model.rec_id) && (z.STATUS != "0")
                                                         )
                    .FirstOrDefault();

                if (zayavka != null)
                {
                    //zayavka.GROUP_ID = CalculateGroupId(model);
                    zayavka.MEDICAL_ORDER = model.MedicalOrder;
                    zayavka.FULL_NAME = model.FullName;
                    zayavka.DEPARTMENT_CODE = model.departmentCode;
                    //zayavka.USER_ID = model.userid;
                    zayavka.POSITION = model.Position;
                    zayavka.QUANTITY = 1;
                    //zayavka.TIP_OTDIXAYUSHEGO = string.Empty;
                    zayavka.SANATORY_ID = model.SanatoryId;
                    zayavka.ZAEZD_BEGIN = model.ZaezdBegin;
                    zayavka.ZAEZD_END = model.ZaezdEnd;
                    //zayavka.STATUS = "0";//New
                    zayavka.CAMP_ID = model.CampId;
                    //zayavka.TIMESTAMP = DateTime.Now;
                    zayavka.KUK_QARORI = model.Kuk_qarori;
                    zayavka.MEDICAL_ORDER_EXISTS = model.MedicalOrderExists;
                    //zayavka.ORDER_QUANTITY = 0;

                    db.SaveChanges();
                }
            }
        }

        public static List<ZayavkiViewModel> GetZayevitelList(ZayavkiViewModel model)
        {
            List<ZayavkiViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var zayavki = from zayavk in db.tbpt_zayavki
                              join depart in db.t_departmentbase on zayavk.DEPARTMENT_CODE equals depart.code
                              where ((zayavk.USER_ID == model.userid) &&
                              (zayavk.DEPARTMENT_CODE == model.departmentCode)
                              && (zayavk.STATUS == "0"))
                              select new ZayavkiViewModel
                              {
                                  rec_id = zayavk.REC_ID,
                                  groupId = zayavk.GROUP_ID.Value,
                                  ZaezdBegin = zayavk.ZAEZD_BEGIN == null ? DateTime.Now: zayavk.ZAEZD_BEGIN.Value,
                                  ZaezdEnd = zayavk.ZAEZD_END == null ? DateTime.Now : zayavk.ZAEZD_END.Value,
                                  SanatoryId = zayavk.SANATORY_ID,
                                  SanatoryName = zayavk.t_sanatorium.sanatorium_name,
                                  CampId = zayavk.CAMP_ID,
                                  CampName = zayavk.t_camp.camps_name,
                                  departmentCode = zayavk.DEPARTMENT_CODE,
                                  OrdanizationName = depart.name_ru ?? depart.bkut_name,
                                  FullName = zayavk.FULL_NAME,
                                  Position = zayavk.POSITION,
                                  Quantity = zayavk.QUANTITY == null ? 0: zayavk.QUANTITY.Value,
                                  OrderQuantity = zayavk.ORDER_QUANTITY == null ? 0 : zayavk.ORDER_QUANTITY.Value,
                                  TimeStamp = zayavk.TIMESTAMP == null ? DateTime.Now : zayavk.TIMESTAMP.Value,
                                  MedicalOrderExists = zayavk.MEDICAL_ORDER_EXISTS == null ? false : zayavk.MEDICAL_ORDER_EXISTS.Value,
                                  MedicalOrder = zayavk.MEDICAL_ORDER,
                                  Kuk_qarori = zayavk.KUK_QARORI,
                                  userid = zayavk.USER_ID == null ? 0 : zayavk.USER_ID.Value,
                              };

                list = zayavki.ToList();

            }

            return list;
        }

        //Rassmotreniye Zayavok Query
        public static List<ZayavkiViewModel> GetZayavkiRassmotritelList(string rassmotDepartmentCode, ZayavkiViewModel model)
        {
            List<ZayavkiViewModel> list = null;

            ///////////////////////////////////////////////////
            //Filter making
            ///////////////////////////////////////////////////

            //Filter fields
            //1) input date TIMESTAMP+ 
            //2) ZaezdBegin date+
            //3) Sanatory
            //3.1) campId
            //4) OrdanizationName INN
            //5) FIO
            //6) Position

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var zayavki = from zayavk in db.tbpt_zayavki
                              join depart in db.t_departmentbase on zayavk.DEPARTMENT_CODE equals depart.code
                              where ((zayavk.STATUS != "0") && 
                              (zayavk.DEPARTMENT_CODE.Contains(rassmotDepartmentCode)) &&
                              (zayavk.TIMESTAMP == (model.TimeStamp.HasValue ? model.TimeStamp :zayavk.TIMESTAMP)) &&
                              (zayavk.ZAEZD_BEGIN == (model.ZaezdBegin.HasValue ? model.ZaezdBegin : zayavk.ZAEZD_BEGIN)) &&
                              (zayavk.SANATORY_ID == (model.SanatoryId.HasValue ? model.SanatoryId : zayavk.SANATORY_ID)) &&
                              (zayavk.CAMP_ID == (model.CampId.HasValue ? model.CampId : zayavk.CAMP_ID)) &&
                              (depart.inn == (!string.IsNullOrEmpty(model.Inn) ? model.Inn : depart.inn)) &&
                              (zayavk.FULL_NAME == (!string.IsNullOrEmpty(model.FullName) ? model.FullName : zayavk.FULL_NAME)) &&
                              (zayavk.POSITION == (!string.IsNullOrEmpty(model.Position) ? model.Position : zayavk.POSITION)) 
                              )
                              select new ZayavkiViewModel
                              {
                                  rec_id = zayavk.REC_ID,
                                  groupId = zayavk.GROUP_ID.Value,
                                  ZaezdBegin = zayavk.ZAEZD_BEGIN ,
                                  ZaezdEnd = zayavk.ZAEZD_END ,
                                  SanatoryId = zayavk.SANATORY_ID,
                                  SanatoryName = zayavk.t_sanatorium.sanatorium_name,
                                  CampId = zayavk.CAMP_ID,
                                  CampName = zayavk.t_camp.camps_name,
                                  departmentCode = zayavk.DEPARTMENT_CODE,
                                  OrdanizationName = depart.name_ru ?? depart.bkut_name,
                                  FullName = zayavk.FULL_NAME,
                                  Position = zayavk.POSITION,
                                  Quantity = zayavk.QUANTITY == null ? 0 : zayavk.QUANTITY.Value,
                                  OrderQuantity = zayavk.ORDER_QUANTITY == null ? 0 : zayavk.ORDER_QUANTITY.Value,
                                  TimeStamp = zayavk.TIMESTAMP,
                                  MedicalOrderExists = zayavk.MEDICAL_ORDER_EXISTS == null ? false : zayavk.MEDICAL_ORDER_EXISTS.Value,
                                  MedicalOrder = zayavk.MEDICAL_ORDER,
                                  Kuk_qarori = zayavk.KUK_QARORI,
                                  userid = zayavk.USER_ID == null ? 0 : zayavk.USER_ID.Value,
                              };

                list = zayavki.ToList();

            }

            return list;
        }

        //Rassmotreniye Zayavok Query Test
        public static void TestGetZayavkiRassmotritelList()
        {
            List<ZayavkiViewModel> gg = GetZayavkiRassmotritelList("173", new ZayavkiViewModel());
        }

        //////////////////////////////////////////////////////////////////////
        //
        // Rssmotreniye Zayavok
        //
        //////////////////////////////////////////////////////////////////////

        public static List<ZayavkiViewModel> GetZayavkiGrouped(string inn, int year)
        {
            List<ZayavkiViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                //Filtered by Inn from departmentBase AND Timestamp from zayavki, STATUS = "0"
                //Grouped by Sanatory, SUM(QUANTITY)

                //var zayavki = from zayavk in db.tbpt_zayavki
                //              join depart in db.t_departmentbase on zayavk.DEPARTMENT_CODE equals depart.code
                //              where ((zayavk.STATUS != "0") && (depart.inn == inn) && (zayavk.TIMESTAMP.Value.Year == year))
                //              group zayavk.SANATORY_ID  

            }

            return list;
        }



    }

    public class AvaragePriceManager
    {
        public static void EnterAvaragePrice(ref AvaragePriceViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbpt_avarageprice avar = new tbpt_avarageprice();

                avar.SET_DATE = model.SetDate;
                avar.AMOUNT = model.Amount;
                avar.USER_ID = model.UserId;

                var inserted = db.tbpt_avarageprice.Add(avar);

                db.SaveChanges();
            }
        }

        public static void ModifyAvaragePrice(AvaragePriceViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var avarage = db.tbpt_avarageprice.Where(a => a.REC_ID == model.RecId).FirstOrDefault();

                if (avarage != null)
                {
                    avarage.SET_DATE = model.SetDate;
                    avarage.AMOUNT = model.Amount;
                    avarage.USER_ID = model.UserId;

                    db.SaveChanges();
                }
            }
        }

        public static void DeleteAvaragePrice(AvaragePriceViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var avarage = db.tbpt_avarageprice.Where(a => a.REC_ID == model.RecId).FirstOrDefault();

                if (avarage != null)
                {
                    db.tbpt_avarageprice.Remove(avarage);

                    db.SaveChanges();
                }
            }
        }

        public static List<AvaragePriceViewModel> GetAvaragePriceList(AvaragePriceViewModel model)
        {
            List<AvaragePriceViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {

                var query = from avar in db.tbpt_avarageprice
                            where ((avar.SET_DATE == (model.SetDate.HasValue ? model.SetDate : avar.SET_DATE)) &&
                            (avar.AMOUNT == (model.Amount.HasValue ? model.Amount : avar.AMOUNT)))
                            select new AvaragePriceViewModel
                            {
                                RecId = avar.REC_ID,
                                SetDate = avar.SET_DATE,
                                Amount = avar.AMOUNT,
                                UserId = avar.USER_ID
                            };

                list = query.ToList();
            }

            return list;
        }

        public static AvaragePriceViewModel GetAvaragePriceForYear(string year)
        {
            AvaragePriceViewModel model = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {

                var query = from avar in db.tbpt_avarageprice
                            where (avar.SET_DATE.Value.Year.ToString() == year) 
                            select new AvaragePriceViewModel
                            {
                                RecId = avar.REC_ID,
                                SetDate = avar.SET_DATE,
                                Amount = avar.AMOUNT,
                                UserId = avar.USER_ID
                            };

                model = query.FirstOrDefault();
            }

            return model;
        }
    }


}