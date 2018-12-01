using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class ZadoljennostContainer
    {
        public static List<ZadoljennostModel> GetZadoljennost()
        {
            var list = new List<ZadoljennostModel>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_stat_ZadoljennostList_V2();

                foreach (var obj in query)
                {
                    var opl = new ZadoljennostModel();

                    opl.ProfsoyzNaimenovanie = obj.ProfsoyzNaimenovanie;
                    opl.Name_Org = obj.Name_Org;
                    opl.ZadolAmount = obj.ZadolAmount;
                    opl.Year = 2018;

                    list.Add(opl);
                }
            }

            return list;
        }

        public static List<ZadoljennostModel> GetZadoljennost(string departmentBase)
        {
            var list = new List<ZadoljennostModel>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_stat_ZadoljennostListObl_V2(departmentBase);

                foreach (var obj in query)
                {
                    var opl = new ZadoljennostModel();

                    opl.ProfsoyzNaimenovanie = obj.ProfsoyzNaimenovanie;
                    opl.Name_Org = obj.Name_Org;
                    opl.ZadolAmount = obj.ZadolAmount;
                    opl.Year = 2018;

                    list.Add(opl);
                }
            }

            return list;
        }
    }
}