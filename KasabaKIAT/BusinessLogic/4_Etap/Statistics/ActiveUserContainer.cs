using KasabaKIAT.Models;
using System;
using System.Collections.Generic;


namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class ActiveUserContainer
    {
        public static List<ActiveUsersModel> GetActiveUser()
        {
            var list = new List<ActiveUsersModel>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_stat_AktiveUsersListFed_V2();

                foreach (var obj in query)
                {
                    var opl = new ActiveUsersModel();

                    opl.ProfsoyzNaimenovanie = obj.ProfsoyzNaimenovanie;
                    opl.UserName = obj.user_name;
                    opl.UserCount = obj.CounterU;
                    opl.Year = string.Format("{0} год", DateTime.Now.Year);

                    list.Add(opl);
                }
            }

            return list;
        }

        public static List<ActiveUsersModel> GetActiveUser(string departmentBase)
        {
            var list = new List<ActiveUsersModel>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = db.sp_stat_AktiveUsersListObl_V2(departmentBase);

                foreach (var obj in query)
                {
                    var opl = new ActiveUsersModel();

                    opl.ProfsoyzNaimenovanie = obj.ProfsoyzNaimenovanie;
                    opl.UserName = obj.user_name;
                    opl.UserCount = obj.CounterU;
                    opl.Year = string.Format("{0} год", DateTime.Now.Year);

                    list.Add(opl);
                }
            }

            return list;
        }
    }
}