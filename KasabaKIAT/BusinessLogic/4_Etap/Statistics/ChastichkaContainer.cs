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
                    if (obj.YEAR_OP < 2018)
                    {
                        continue;
                    }

                    var opl = new ChastichkaModel();

                    opl.year = string.Format("{0} год", obj.YEAR_OP); 
                    opl.quarter = string.Format("{0} квартал", obj.QUARTER_op);
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
                    if (obj.YEAR_OP < 2018)
                    {
                        continue;
                    }

                    var opl = new ChastichkaModel();

                    opl.year = string.Format("{0} год", obj.YEAR_OP);
                    opl.quarter = string.Format("{0} квартал", obj.QUARTER_op);
                    opl.ProfsoyzNaimenovanie = obj.ProfsoyzNaimenovanie;
                    opl.amount = obj.CHASTICHKA;


                    list.Add(opl);
                }
            }

            return list;
        }
    }
}