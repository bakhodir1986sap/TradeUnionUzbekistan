using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class PutyovkiContainer
    {
        public static List<PutyovkiModel> GetPutyovkas()
        {
            var list = new List<PutyovkiModel>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_stat_OstatokPutyovkas_V2();

                foreach (var obj in query)
                {
                    if (obj.YEAR_OP != 2018)
                    {
                        continue;
                    }

                    var opl = new PutyovkiModel();

                    opl.YEAR_OP = string.Format("{0} год", obj.YEAR_OP); 
                    opl.QUARTER_op = string.Format("{0} квартал", obj.QUARTER_op); 
                    opl.ProfsoyzNaimenovanie = obj.ProfsoyzNaimenovanie;
                    opl.OstatokAmount = obj.OstatokAmount;
                    opl.OstatokCount = obj.OstatokCount;
                    opl.RealizAmount = obj.RealizAmount;
                    opl.RealizCount = obj.RealizCount;
                    opl.Name_San = obj.Name_San;
                    opl.Num_Put = obj.Num_Put;
                    opl.ComplexStr = string.Format("№ {0} {1}", obj.Num_Put, obj.Name_San);


                    list.Add(opl);
                }
            }

            return list;
        }

        public static List<PutyovkiModel> GetPutyovkas(string departmentBase)
        {
            var list = new List<PutyovkiModel>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_stat_OstatokPutyovkasObl_V2(departmentBase);

                foreach (var obj in query)
                {
                    if (obj.YEAR_OP != 2018)
                    {
                        continue;
                    }
                    var opl = new PutyovkiModel();

                    opl.YEAR_OP = string.Format("{0} год", obj.YEAR_OP);
                    opl.QUARTER_op = string.Format("{0} квартал", obj.QUARTER_op);
                    opl.ProfsoyzNaimenovanie = obj.ProfsoyzNaimenovanie;
                    opl.OstatokAmount = obj.OstatokAmount;
                    opl.OstatokCount = obj.OstatokCount;
                    opl.RealizAmount = obj.RealizAmount;
                    opl.RealizCount = obj.RealizCount;
                    opl.Name_San = obj.Name_San;
                    opl.Num_Put = obj.Num_Put;
                    opl.ComplexStr = string.Format("№ {0} {1}", obj.Num_Put, obj.Name_San);


                    list.Add(opl);
                }
            }

            return list;
        }
    }
}