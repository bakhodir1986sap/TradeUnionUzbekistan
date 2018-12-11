using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class ChastichkaContainer
    {
        public static List<ChastichkaModel> GetChastichka()
        {
            var list = new List<ChastichkaModel>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_stat_Chastichka_V2();

                foreach (var obj in query)
                {
                    var opl = new ChastichkaModel();

                    opl.year = obj.YEAR_OP;
                    opl.quarter = obj.QUARTER_op;
                    opl.ProfsoyzNaimenovanie = obj.ProfsoyzNaimenovanie;
                    opl.amount = obj.CHASTICHKA;

                    list.Add(opl);
                }
            }

            return list;
        }

        public static List<ChastichkaModel> GetChastichka(string departmentBase)
        {
            var list = new List<ChastichkaModel>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_stat_ChastichkaObl_V2(departmentBase);

                foreach (var obj in query)
                {
                    var opl = new ChastichkaModel();

                    opl.year = obj.YEAR_OP;
                    opl.quarter = obj.QUARTER_op;
                    opl.ProfsoyzNaimenovanie = obj.ProfsoyzNaimenovanie;
                    opl.amount = obj.CHASTICHKA;


                    list.Add(opl);
                }
            }

            return list;
        }
    }
}