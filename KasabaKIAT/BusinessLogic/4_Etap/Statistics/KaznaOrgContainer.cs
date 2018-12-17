using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class KaznaOrgContainer
    {
        public static List<KaznaOrgModel> GetKaznaFederationInfo()
        {
            var listModel = new List<KaznaOrgModel>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_stat_KaznaDataFedOrg_V2();

                foreach (var obj in query)
                {
                    var opl = new KaznaOrgModel();

                    opl.FEDERATION_AMT = obj.FEDERATION_AMT;
                    opl.ProfsoyzNaimenovanie = obj.ProfsoyzNaimenovanie;
                    opl.OBLSOVPROF_AMT = obj.OBLSOVPROF_AMT;
                    opl.BRANCH_NAME = obj.BRANCH_NAME;
                    opl.SOATO_NAME = obj.SOATO_NAME;
                    opl.NAME_ORG = obj.NAME_ORG;

                    listModel.Add(opl);
                }
            }

            return listModel;
        }
    }
}