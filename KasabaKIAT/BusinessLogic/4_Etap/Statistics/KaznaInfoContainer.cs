using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class KaznaInfoContainer
    {
        public static List<KaznaInfoModel> GetKaznaFederationInfo()
        {
            var listModel = new List<KaznaInfoModel>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_stat_KaznaDataFed_V2();

                foreach (var obj in query)
                {
                    if (obj.YEAR_OP != 2018)
                    {
                        continue;
                    }

                    var opl = new KaznaInfoModel();

                    opl.FederationAmount = obj.FEDERATION_AMT;
                    opl.OblSovProf = obj.ProfsoyzNaimenovanie;
                    opl.OblSovProvAmount = obj.OBLSOVPROF_AMT;
                    opl.TotalByRow = opl.FederationAmount + opl.OblSovProvAmount;
                    opl.Quarter = string.Format("{0} квартал" ,obj.QUARTER_op.Value);
                    opl.Tarmoq = obj.BRANCH_NAME;
                    opl.Tuman = obj.SOATO_NAME;
                    opl.Year = string.Format("{0} год", obj.YEAR_OP.Value); 

                    listModel.Add(opl);
                }
            }

            return listModel;
        }

        public static List<KaznaInfoModel> GetKaznaFederationInfo(string departmentbase)
        {
            var listModel = new List<KaznaInfoModel>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_stat_KaznaDataObl_V2(departmentbase);

                foreach (var obj in query)
                {
                    if (obj.YEAR_OP != 2018)
                    {
                        continue;
                    }

                    var opl = new KaznaInfoModel();

                    opl.FederationAmount = obj.FEDERATION_AMT;
                    opl.OblSovProf = obj.ProfsoyzNaimenovanie;
                    opl.OblSovProvAmount = obj.OBLSOVPROF_AMT;
                    opl.TotalByRow = opl.FederationAmount + opl.OblSovProvAmount;
                    opl.Quarter = string.Format("{0} квартал", obj.QUARTER_op.Value);
                    opl.Tarmoq = obj.BRANCH_NAME;
                    opl.Tuman = obj.SOATO_NAME;
                    opl.Year = string.Format("{0} год", obj.YEAR_OP.Value);

                    listModel.Add(opl);
                }
            }

            return listModel;
        }




        public static List<KaznaInfoModel> GetKaznaFederationLastDayInfo()
        {
            var listModel = new List<KaznaInfoModel>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_stat_KaznaDataFedLastDay_V2();

                foreach (var obj in query)
                {
                    var opl = new KaznaInfoModel();

                    opl.FederationAmount = obj.FEDERATION_AMT;
                    opl.OblSovProf = obj.ProfsoyzNaimenovanie;
                    opl.OblSovProvAmount = obj.OBLSOVPROF_AMT;
                    opl.TotalByRow = opl.FederationAmount + opl.OblSovProvAmount;
                    opl.Quarter = string.Format("{0} квартал", obj.QUARTER_op.Value);
                    opl.Tarmoq = obj.BRANCH_NAME;
                    opl.Tuman = obj.SOATO_NAME;
                    opl.Year = string.Format("{0} год", obj.YEAR_OP.Value);

                    listModel.Add(opl);
                }
            }

            return listModel;
        }


    }
}