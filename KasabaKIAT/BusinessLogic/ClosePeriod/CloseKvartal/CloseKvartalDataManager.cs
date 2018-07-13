using KasabaKIAT.Models;
using System.Collections.Generic;
using System.Linq;

namespace KasabaKIAT.BusinessLogic.ClosePeriod.CloseKvartal
{
    public class CloseKvartalDataManager
    {
        public static void Create(CloseKvartalViewModel model, KasabaKIATEntities db)
        {
            tblCloseKvartal close = new tblCloseKvartal
            {
                DEPARTMENT_CODE = model.department_code,
                PREVKV_YEAR = model.prevkv_year,
                PREVKV = model.prevkv,
                CURKV_YEAR = model.curkv_year,
                CURKV = model.curkv,
                NEXTKV_YEAR = model.nextkv_year,
                NEXTKV = model.nextkv
            };

            var pst = db.tblCloseKvartals.Add(close);
        }

        public static void Modify(CloseKvartalViewModel model, KasabaKIATEntities db)
        {
                var vipl = db.tblCloseKvartals.Where(z => z.DEPARTMENT_CODE == model.department_code);

                if (vipl.Any())
                {
                    var vipl1 = vipl.FirstOrDefault();

                    if (vipl1 != null)
                    {
                        vipl1.PREVKV_YEAR = model.prevkv_year;
                        vipl1.PREVKV = model.prevkv;
                        vipl1.CURKV_YEAR = model.curkv_year;
                        vipl1.CURKV = model.curkv;
                        vipl1.NEXTKV_YEAR = model.nextkv_year;
                        vipl1.NEXTKV = model.nextkv;
                    }
                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<CloseKvartalViewModel> Get(CloseKvartalViewModel model)
        {
            List<CloseKvartalViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from vipl in db.tblCloseKvartals
                            where vipl.DEPARTMENT_CODE == model.department_code
                            select new CloseKvartalViewModel
                            {
                                rec_id            = vipl.REC_ID,
                                department_code   = vipl.DEPARTMENT_CODE,
                                prevkv_year       = vipl.PREVKV_YEAR,
                                prevkv            = vipl.PREVKV,
                                curkv_year        = vipl.CURKV_YEAR,
                                curkv             = vipl.CURKV,
                                nextkv_year       = vipl.NEXTKV_YEAR,
                                nextkv            = vipl.NEXTKV
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}