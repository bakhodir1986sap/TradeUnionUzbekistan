using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class PererasxodPoPutevkamContainer
    {
        public static List<PererasxodPoPutevkamModel> GetPererasxodInfo()
        {
            var listModel = new List<PererasxodPoPutevkamModel>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_monit_PererasxodPoPutyovkamV2();

                foreach (var obj in query)
                {
                    if (obj.YEAR_OP < DateTime.Now.Year)
                    {
                        continue;
                    }

                    var opl = new PererasxodPoPutevkamModel();

                    opl.YEAR_OP = string.Format("{0} год", obj.YEAR_OP.Value);     
                    opl.QUATER_OP               = string.Format("{0} квартал", obj.QUATER_OP.Value); 
                    opl.ProfsoyzNaimenovanie    = obj.ProfsoyzNaimenovanie  ;
                    opl.OrganizationName        = obj.OrganizationName      ;
                    opl.PostupleniyaZaPeriod    = obj.PostupleniyaZaPeriod  ;
                    opl.KolVoMogutPoluchit      = obj.KolVoMogutPoluchit    ;
                    opl.KolVoFakticheskiPoluch = obj.KolVoFakticheskiPoluch;

                    listModel.Add(opl);
                }
            }

            return listModel;
        }
    }
}