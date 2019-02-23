using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class RealizContainer
    {
        public static List<RealizModel> GetRealizData()
        {
            var list = new List<RealizModel>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_stat_Realiz_V2();

                foreach (var obj in query)
                {
                    if (obj.YEAR_OP < 2018)
                    {
                        continue;
                    }

                    var opl = new RealizModel();

                    opl.YEAR_OP = string.Format("{0} год", obj.YEAR_OP);
                    opl.QUARTER_op = string.Format("{0} квартал", obj.QUARTER_op); 
                    opl.ProfsoyzNaimenovanie = obj.ProfsoyzNaimenovanie;
                    opl.Name_San = obj.Name_San;
                    opl.Num_Put = obj.Num_Put;
                    opl.s45Amount = obj.s45Amount;
                    opl.s45Count = obj.s45Count;
                    opl.s100Amount = obj.s100Amount;
                    opl.s100Count = obj.s100Count;
                    opl.s0Amount = obj.s0Amount;
                    opl.s0Count = obj.s0Count;

                    list.Add(opl);
                }
            }

            return list;
        }

        public static List<RealizModel> GetRealizData(string departmentBase)
        {
            var list = new List<RealizModel>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_stat_RealizObl_V2(departmentBase);

                foreach (var obj in query)
                {
                    if (obj.YEAR_OP < 2018)
                    {
                        continue;
                    }

                    var opl = new RealizModel();

                    opl.YEAR_OP = string.Format("{0} год", obj.YEAR_OP);
                    opl.QUARTER_op = string.Format("{0} квартал", obj.QUARTER_op);
                    opl.ProfsoyzNaimenovanie = obj.ProfsoyzNaimenovanie;
                    opl.Name_San = obj.Name_San;
                    opl.Num_Put = obj.Num_Put;
                    opl.s45Amount = obj.s45Amount;
                    opl.s45Count = obj.s45Count;
                    opl.s100Amount = obj.s100Amount;
                    opl.s100Count = obj.s100Count;
                    opl.s0Amount = obj.s0Amount;
                    opl.s0Count = obj.s0Count;

                    list.Add(opl);
                }
            }

            return list;
        }
    }
}