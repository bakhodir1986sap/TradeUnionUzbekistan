using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.FinansoviyOtchet
{
    public class FinOtchetSettingsDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="db"></param>
        public static void Create(FinOtchetSettingsViewModel model, KasabaKIATEntities db)
        {
            
            tbbu_finOtchetSetting finSetting = new tbbu_finOtchetSetting
            {
                OTCHET_PART     = model.otchet_part ,
                CODE            = model.code        ,
                DEBIT_SIDE      = model.debit_side  ,
                CREDIT_SIDE     = model.credit_side ,
                ACCOUNT_CODE    = model.account_code,
                DATA_TYPE       = model.data_type   ,
                DATA_DESC       = model.data_desc   
            };

            var pst = db.tbbu_finOtchetSetting.Add(finSetting);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="db"></param>
        public static void Modify(FinOtchetSettingsViewModel model, KasabaKIATEntities db)
        {
            var finSetting = db.tbbu_finOtchetSetting.Where(z => z.REC_ID == model.rec_id);

            if (finSetting.Any())
            {
                var finSetting1 = finSetting.FirstOrDefault();

                if (finSetting1 != null)
                {
                    finSetting1.OTCHET_PART = model.otchet_part;
                    finSetting1.CODE = model.code;
                    finSetting1.DEBIT_SIDE = model.debit_side;
                    finSetting1.CREDIT_SIDE = model.credit_side;
                    finSetting1.ACCOUNT_CODE = model.account_code;
                    finSetting1.DATA_TYPE = model.data_type;
                    finSetting1.DATA_DESC = model.data_desc;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<FinOtchetSettingsViewModel> Get(FinOtchetSettingsViewModel model)
        {
            List<FinOtchetSettingsViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from balance in db.tbbu_finOtchetSetting
                            where balance.CODE == model.code
                            select new FinOtchetSettingsViewModel
                            {
                                rec_id          = balance.REC_ID,
                                otchet_part     = balance.OTCHET_PART,
                                code            = balance.CODE,
                                debit_side      = balance.DEBIT_SIDE,
                                credit_side     = balance.CREDIT_SIDE,
                                account_code    = balance.ACCOUNT_CODE,
                                data_type       = balance.DATA_TYPE,
                                data_desc       = balance.DATA_DESC
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}