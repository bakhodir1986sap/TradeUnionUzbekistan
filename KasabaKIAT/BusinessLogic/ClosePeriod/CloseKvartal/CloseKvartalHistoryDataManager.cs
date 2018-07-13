using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.ClosePeriod.CloseKvartal
{
    public class CloseKvartalHistoryDataManager
    {
        public static void Create(CloseKvartalHistoryViewModel model, KasabaKIATEntities db)
        {
            tblCloseKvartalHistory close = new tblCloseKvartalHistory
            {
                DEPARTMENT_CODE        = model.department_code,
                CLOSED_KVYEAR          = model.closed_kvyear  ,
                CLOSED_KV              = model.closed_kv      ,
                TIMESTAMP              = DateTime.Now      ,
                USER_ID                = model.user_id        ,
                REMARK                 = model.remark
            };

            var pst = db.tblCloseKvartalHistories.Add(close);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<CloseKvartalHistoryViewModel> Get(CloseKvartalHistoryViewModel model)
        {
            List<CloseKvartalHistoryViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from vipl in db.tblCloseKvartalHistories
                            where vipl.DEPARTMENT_CODE == model.department_code
                            select new CloseKvartalHistoryViewModel
                            {
                                rec_id          = vipl.REC_ID           ,     
                                department_code = vipl.DEPARTMENT_CODE  ,
                                closed_kvyear   = vipl.CLOSED_KVYEAR    ,
                                closed_kv       = vipl.CLOSED_KV        ,
                                timestamp       = vipl.TIMESTAMP        ,
                                user_id         = vipl.USER_ID          ,
                                remark          = vipl.REMARK
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}