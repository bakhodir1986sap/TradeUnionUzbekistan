using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class BronContainer
    {
        public static List<BronModel> GetBrons()
        {
            var list = new List<BronModel>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_stat_BronFed_V2();

                foreach (var obj in query)
                {
                    if (obj.YEAR_OP < 2018)
                    {
                        continue;
                    }

                    var opl = new BronModel();

                    opl.YEAR_OP = string.Format("{0} год", obj.YEAR_OP); 
                    opl.QUARTER_op = string.Format("{0} квартал", obj.QUARTER_op); 
                    opl.ProfsoyzNaimenovanie = obj.ProfsoyzNaimenovanie;
                    opl.BronCount = obj.CounterU;
                    opl.Name_San = obj.Name_San;
                    opl.Num_Put = obj.Num_Put;

                    list.Add(opl);
                }
            }

            return list;
        }

        public static List<BronModel> GetBrons(string departmentBase)
        {
            var list = new List<BronModel>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_stat_BronObl_V2(departmentBase);

                foreach (var obj in query)
                {
                    if (obj.YEAR_OP < 2018)
                    {
                        continue;
                    }

                    var opl = new BronModel();

                    opl.YEAR_OP = string.Format("{0} год", obj.YEAR_OP);
                    opl.QUARTER_op = string.Format("{0} квартал", obj.QUARTER_op);
                    opl.ProfsoyzNaimenovanie = obj.ProfsoyzNaimenovanie;
                    opl.BronCount = obj.CounterU;
                    opl.Name_San = obj.Name_San;
                    opl.Num_Put = obj.Num_Put;


                    list.Add(opl);
                }
            }

            return list;
        }
    }
}