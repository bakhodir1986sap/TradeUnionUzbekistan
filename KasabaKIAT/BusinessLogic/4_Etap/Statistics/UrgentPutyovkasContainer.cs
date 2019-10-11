using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class UrgentPutyovkasContainer
    {
        public static List<UrgentPutyovkas> GetUrgentPutyovkas()
        {
            var list = new List<UrgentPutyovkas>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_stat_UrgentPutyovkasRegionListFed_V2();

                foreach (var obj in query)
                {
                    if (obj.YEAR_OP < DateTime.Now.Year)
                    {
                        continue;
                    }

                    var opl = new UrgentPutyovkas();

                    opl.Year_Op = string.Format("{0} год", obj.YEAR_OP);
                    opl.Quarter = string.Format("{0} квартал", obj.QUARTER_op);
                    opl.ProfsoyzNaimenovanie = obj.ProfsoyzNaimenovanie;
                    opl.Num_Put = obj.Num_Put;
                    opl.Name_San = obj.Name_San;
                    opl.DataSan = obj.DataSan;
                    opl.CounterU = obj.CounterU;
                    opl.ComplexString = string.Format("№ {0}  {1} {2}", opl.Num_Put, opl.Name_San, opl.DataSan.HasValue ? opl.DataSan.Value.ToString("dd.MM.yyyy"): DateTime.MinValue.ToString("dd.MM.yyyy"));

                    list.Add(opl);
                }
            }

            return list;
        }

        public static List<UrgentPutyovkas> GetUrgentPutyovkas(string departmentBase)
        {
            var list = new List<UrgentPutyovkas>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_stat_UrgentPutyovkasRegionListFedObl_V2(departmentBase);

                foreach (var obj in query)
                {
                    if (obj.YEAR_OP < DateTime.Now.Year)
                    {
                        continue;
                    }

                    var opl = new UrgentPutyovkas();


                    opl.Year_Op = string.Format("{0} год", obj.YEAR_OP);
                    opl.Quarter = string.Format("{0} квартал", obj.QUARTER_op);
                    opl.ProfsoyzNaimenovanie = obj.ProfsoyzNaimenovanie;
                    opl.Num_Put = obj.Num_Put;
                    opl.Name_San = obj.Name_San;
                    opl.DataSan = obj.DataSan;
                    opl.CounterU = obj.CounterU;
                    opl.ComplexString = string.Format("№ {0}  {1} {2}", opl.Num_Put, opl.Name_San, opl.DataSan.HasValue ? opl.DataSan.Value.ToString("dd.MM.yyyy") : DateTime.MinValue.ToString("dd.MM.yyyy"));

                    list.Add(opl);
                }
            }

            return list;
        }
    }
}