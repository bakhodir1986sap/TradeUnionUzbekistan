using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.SverkaDannix.Ruchnoy
{
    public class SanatDataCompareDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Create(ref SanatDataCompareViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var sanInfo = new tblSanatDataCompare
                {
                    YEAR = model.year,
                    OWNER_TP = model.owner_tp,
                    SANAT_ID = model.sanat_id,
                    NUM_PUT = model.num_put,
                    DATE_SAN = model.date_san,
                    PRICE = model.price,
                    FIO_OTDIX = model.fio_otdix,
                    ADRESS = model.adress,
                    WORK_PLACE = model.work_place,
                    OTRASL = model.otrasl,
                    OWNER_CODE = model.owner_code
                };

                var sanIi = db.tblSanatDataCompares.Add(sanInfo);

                db.SaveChanges();

                model.id = sanIi.ID;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Modify(SanatDataCompareViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var sanInfo = db.tblSanatDataCompares.Where(z => z.ID == model.id);

                if (sanInfo.Any())
                {
                    var sanInfo1 = sanInfo.FirstOrDefault();

                    if (sanInfo1 != null)
                    {
                        sanInfo1.YEAR = model.year;
                        sanInfo1.OWNER_TP = model.owner_tp;
                        sanInfo1.SANAT_ID = model.sanat_id;
                        sanInfo1.NUM_PUT = model.num_put;
                        sanInfo1.DATE_SAN = model.date_san;
                        sanInfo1.PRICE = model.price;
                        sanInfo1.FIO_OTDIX = model.fio_otdix;
                        sanInfo1.ADRESS = model.adress;
                        sanInfo1.WORK_PLACE = model.work_place;
                        sanInfo1.OTRASL = model.otrasl;
                        sanInfo1.OWNER_CODE = model.owner_code;
                    }
                }

                db.SaveChanges();
            }
        }

        public static async Task SaveData(SanatDataCompareViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var sanInfo = db.tblSanatDataCompares.Where(z => z.SANAT_ID == model.sanat_id
                && z.NUM_PUT == model.num_put && z.OWNER_TP == model.owner_tp);

                if (sanInfo.Any())
                {
                    var sanInfo1 = sanInfo.FirstOrDefault();

                    if (sanInfo1 != null)
                    {
                        sanInfo1.YEAR = model.year;
                        sanInfo1.OWNER_TP = model.owner_tp;
                        sanInfo1.SANAT_ID = model.sanat_id;
                        sanInfo1.NUM_PUT = model.num_put;
                        sanInfo1.DATE_SAN = model.date_san;
                        sanInfo1.PRICE = model.price;
                        sanInfo1.FIO_OTDIX = model.fio_otdix;
                        sanInfo1.ADRESS = model.adress;
                        sanInfo1.WORK_PLACE = model.work_place;
                        sanInfo1.OTRASL = model.otrasl;
                        sanInfo1.OWNER_CODE = model.owner_code;
                    }
                }else
                {
                    var sanInfo2 = new tblSanatDataCompare
                    {
                        YEAR = model.year,
                        OWNER_TP = model.owner_tp,
                        SANAT_ID = model.sanat_id,
                        NUM_PUT = model.num_put,
                        DATE_SAN = model.date_san,
                        PRICE = model.price,
                        FIO_OTDIX = model.fio_otdix,
                        ADRESS = model.adress,
                        WORK_PLACE = model.work_place,
                        OTRASL = model.otrasl,
                        OWNER_CODE = model.owner_code
                    };

                    var sanIi = db.tblSanatDataCompares.Add(sanInfo2);
                }

                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Remove(SanatDataCompareViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var sanInfo = db.tblSanatDataCompares.Where(z => z.ID == model.id);

                if (sanInfo.Any())
                {
                    var reg = sanInfo.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblSanatDataCompares.Remove(reg);
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
        public static List<SanatDataCompareViewModel> Get(SanatDataCompareViewModel model)
        {
            List<SanatDataCompareViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from sanInfo in db.tblSanatDataCompares
                            join sanat in db.tblSprav_San on sanInfo.SANAT_ID equals sanat.ID
                            where sanInfo.YEAR == model.year
                            select new SanatDataCompareViewModel
                            {
                                id          = sanInfo.ID        ,
                                year        = sanInfo.YEAR      ,
                                owner_tp    = sanInfo.OWNER_TP  ,
                                sanat_id    = sanInfo.SANAT_ID  ,
                                sanatName   = sanat.Name_San,
                                num_put     = sanInfo.NUM_PUT   ,
                                date_san    = sanInfo.DATE_SAN  ,
                                price       = sanInfo.PRICE     ,
                                fio_otdix   = sanInfo.FIO_OTDIX ,
                                adress      = sanInfo.ADRESS    ,
                                work_place  = sanInfo.WORK_PLACE,
                                otrasl      = sanInfo.OTRASL    ,
                                owner_code  = sanInfo.OWNER_CODE
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}