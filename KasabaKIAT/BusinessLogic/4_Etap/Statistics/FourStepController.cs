using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class FourStepController
    {
        public static FourStapViewModel Get4StageInfo(string departmentBase, int? cur_year)
        {
            FourStapViewModel obj = null;

            if (departmentBase.Equals("17"))
            {
                obj = new FourStapViewModel
                {
                    kaznaInfo = KaznaInfoContainer.GetKaznaFederationInfo(),
                    urgentInfo = UrgentPutyovkasContainer.GetUrgentPutyovkas(),
                    putyovkaInfo = PutyovkiContainer.GetPutyovkas(),
                    realizInfo = RealizContainer.GetRealizData(),
                    zadoljInfo = ZadoljennostContainer.GetZadoljennost(),
                    bronInfo = BronContainer.GetBrons(),
                    activeUsersInfo = ActiveUserContainer.GetActiveUser(),
                    organizationPieInfo = PieChartDataContainer.GetOrganizationData(),
                    zayavkiPieInfo = PieChartDataContainer.GetZayavki(cur_year),
                    putyovkiPieInfo = PieChartDataContainer.GetPutyovki(cur_year),
                    chastichkaInfo = ChastichkaContainer.GetChastichka()
                };
            }
            else
            {
                obj = new FourStapViewModel
                {
                    kaznaInfo = KaznaInfoContainer.GetKaznaFederationInfo(departmentBase),
                    urgentInfo = UrgentPutyovkasContainer.GetUrgentPutyovkas(departmentBase),
                    putyovkaInfo = PutyovkiContainer.GetPutyovkas(departmentBase),
                    realizInfo = RealizContainer.GetRealizData(departmentBase),
                    zadoljInfo = ZadoljennostContainer.GetZadoljennost(departmentBase),
                    bronInfo = BronContainer.GetBrons(departmentBase),
                    activeUsersInfo = ActiveUserContainer.GetActiveUser(departmentBase),
                    organizationPieInfo = PieChartDataContainer.GetOrganizationData(departmentBase),
                    zayavkiPieInfo = PieChartDataContainer.GetZayavki(departmentBase, cur_year),
                    putyovkiPieInfo = PieChartDataContainer.GetPutyovki(departmentBase, cur_year),
                    chastichkaInfo = ChastichkaContainer.GetChastichka(departmentBase)
                };
            }

            return obj;
        }

        public static void GetActiveUsers(FourStapViewModel current , string departmentBase, int? cur_year)
        {
            current.activeUsersInfo = ActiveUserContainer.GetActiveUser(departmentBase);
        }
    }
}