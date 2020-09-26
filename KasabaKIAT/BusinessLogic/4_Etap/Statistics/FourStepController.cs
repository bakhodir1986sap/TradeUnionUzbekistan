using System.Collections.Generic;
using System.Threading.Tasks;

namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class FourStepController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <param name="cur_year"></param>
        /// <returns></returns>
        public static FourStapViewModel Get4StageInfo(string departmentBase, int? cur_year, FourStapViewModel obj)
        {
            List<Task> tasks = new List<Task>();
            obj = new FourStapViewModel();

            if (departmentBase.Equals("17"))
            {
                #region Federation_Statistika
                var outer1 = Task.Factory.StartNew(() =>     // внешняя задача
                {
                    return KaznaInfoContainer.GetKaznaFederationInfo();
                });

                outer1.ContinueWith((t) =>
                {
                    if (outer1.Status == TaskStatus.RanToCompletion)
                    {
                        obj.kaznaInfo = t.Result;
                    }
                    if (outer1.Status == TaskStatus.Faulted)
                    {
                        obj.kaznaInfo = null;
                    }
                });

                tasks.Add(outer1);

                var outer2 = Task.Factory.StartNew(() =>     // внешняя задача
                {
                    return UrgentPutyovkasContainer.GetUrgentPutyovkas();
                });

                outer2.ContinueWith((t) =>
                {
                    if (outer2.Status == TaskStatus.RanToCompletion)
                    {
                        obj.urgentInfo = t.Result;
                    }
                    if (outer2.Status == TaskStatus.Faulted)
                    {
                        obj.urgentInfo = null;
                    }
                });

                tasks.Add(outer2);


                var outer3 = Task.Factory.StartNew(() =>     // внешняя задача
                {
                    return PutyovkiContainer.GetPutyovkas();
                });

                outer3.ContinueWith((t) =>
                {
                    if (outer3.Status == TaskStatus.RanToCompletion)
                    {
                        obj.putyovkaInfo = t.Result;
                    }
                    if (outer3.Status == TaskStatus.Faulted)
                    {
                        obj.putyovkaInfo = null;
                    }
                });

                tasks.Add(outer3);


                var outer4 = Task.Factory.StartNew(() =>     // внешняя задача
                {
                    return RealizContainer.GetRealizData();
                });

                outer4.ContinueWith((t) =>
                {
                    if (outer4.Status == TaskStatus.RanToCompletion)
                    {
                        obj.realizInfo = t.Result;
                    }
                    if (outer4.Status == TaskStatus.Faulted)
                    {
                        obj.realizInfo = null;
                    }
                });

                tasks.Add(outer4);


                var outer5 = Task.Factory.StartNew(() =>     // внешняя задача
                {
                    return ZadoljennostContainer.GetZadoljennost();
                });

                outer5.ContinueWith((t) =>
                {
                    if (outer5.Status == TaskStatus.RanToCompletion)
                    {
                        obj.zadoljInfo = t.Result;
                    }
                    if (outer5.Status == TaskStatus.Faulted)
                    {
                        obj.zadoljInfo = null;
                    }
                });

                tasks.Add(outer5);


                var outer6 = Task.Factory.StartNew(() =>     // внешняя задача
                {
                    return BronContainer.GetBrons();
                });

                outer6.ContinueWith((t) =>
                {
                    if (outer6.Status == TaskStatus.RanToCompletion)
                    {
                        obj.bronInfo = t.Result;
                    }
                    if (outer6.Status == TaskStatus.Faulted)
                    {
                        obj.bronInfo = null;
                    }
                });

                tasks.Add(outer6);


                var outer7 = Task.Factory.StartNew(() =>     // внешняя задача
                {
                    return ActiveUserContainer.GetActiveUser();
                });

                outer7.ContinueWith((t) =>
                {
                    if (outer7.Status == TaskStatus.RanToCompletion)
                    {
                        obj.activeUsersInfo = t.Result;
                    }
                    if (outer7.Status == TaskStatus.Faulted)
                    {
                        obj.activeUsersInfo = null;
                    }
                });

                tasks.Add(outer7);

                var outer8 = Task.Factory.StartNew(() =>     // внешняя задача
                {
                    return PieChartDataContainer.GetOrganizationData();
                });

                outer8.ContinueWith((t) =>
                {
                    if (outer8.Status == TaskStatus.RanToCompletion)
                    {
                        obj.organizationPieInfo = t.Result;
                    }
                    if (outer8.Status == TaskStatus.Faulted)
                    {
                        obj.organizationPieInfo = null;
                    }
                });

                tasks.Add(outer8);

                var outer9 = Task.Factory.StartNew(() =>     // внешняя задача
                {
                    return PieChartDataContainer.GetZayavki(cur_year);
                });

                outer9.ContinueWith((t) =>
                {
                    if (outer9.Status == TaskStatus.RanToCompletion)
                    {
                        obj.zayavkiPieInfo = t.Result;
                    }
                    if (outer9.Status == TaskStatus.Faulted)
                    {
                        obj.zayavkiPieInfo = null;
                    }
                });

                tasks.Add(outer9);

                var outer10 = Task.Factory.StartNew(() =>     // внешняя задача
                {
                    return PieChartDataContainer.GetPutyovki(cur_year);
                });

                outer10.ContinueWith((t) =>
                {
                    if (outer10.Status == TaskStatus.RanToCompletion)
                    {
                        obj.putyovkiPieInfo = t.Result;
                    }
                    if (outer10.Status == TaskStatus.Faulted)
                    {
                        obj.putyovkiPieInfo = null;
                    }
                });

                tasks.Add(outer10);


                var outer11 = Task.Factory.StartNew(() =>     // внешняя задача
                {
                    return ChastichkaContainer.GetChastichka();
                });

                outer11.ContinueWith((t) =>
                {
                    if (outer11.Status == TaskStatus.RanToCompletion)
                    {
                        obj.chastichkaInfo = t.Result;
                    }
                    if (outer11.Status == TaskStatus.Faulted)
                    {
                        obj.chastichkaInfo = null;
                    }
                });

                tasks.Add(outer11);

                var outer12 = Task.Factory.StartNew(() =>     // внешняя задача
                {
                    return KaznaInfoContainer.GetKaznaFederationLastDayInfo();
                });

                outer12.ContinueWith((t) =>
                {
                    if (outer12.Status == TaskStatus.RanToCompletion)
                    {
                        obj.kaznaLastDayInfo = t.Result;
                    }
                    if (outer12.Status == TaskStatus.Faulted)
                    {
                        obj.kaznaLastDayInfo = null;
                    }
                });

                tasks.Add(outer12);


                var outer13 = Task.Factory.StartNew(() =>     // внешняя задача
                {
                    return PererasxodPoPutevkamContainer.GetPererasxodInfo();
                });

                outer13.ContinueWith((t) =>
                {
                    if (outer13.Status == TaskStatus.RanToCompletion)
                    {
                        obj.pererasxodInfo = t.Result;
                    }
                    if (outer13.Status == TaskStatus.Faulted)
                    {
                        obj.pererasxodInfo = null;
                    }
                });

                tasks.Add(outer13);


                var outer14 = Task.Factory.StartNew(() =>     // внешняя задача
                {
                    return KaznaOrgContainer.GetKaznaFederationInfo();
                });

                outer14.ContinueWith((t) =>
                {
                    if (outer14.Status == TaskStatus.RanToCompletion)
                    {
                        obj.kaznaOrgList = t.Result;
                    }
                    if (outer14.Status == TaskStatus.Faulted)
                    {
                        obj.kaznaOrgList = null;
                    }
                });

                tasks.Add(outer14);

                #endregion

            }
            else
            {

                var outer1 = Task.Factory.StartNew(() =>     // GetKaznaFederationInfo
                {
                    return KaznaInfoContainer.GetKaznaFederationInfo(departmentBase);
                });

                outer1.ContinueWith((t) =>
                {
                    if (outer1.Status == TaskStatus.RanToCompletion)
                    {
                        obj.kaznaInfo = t.Result;
                    }
                    if (outer1.Status == TaskStatus.Faulted)
                    {
                        obj.kaznaInfo = null;
                    }
                });

                tasks.Add(outer1);

                var outer2 = Task.Factory.StartNew(() =>     // GetUrgentPutyovkas
                {
                    return UrgentPutyovkasContainer.GetUrgentPutyovkas(departmentBase);
                });

                outer2.ContinueWith((t) =>
                {
                    if (outer2.Status == TaskStatus.RanToCompletion)
                    {
                        obj.urgentInfo = t.Result;
                    }
                    if (outer2.Status == TaskStatus.Faulted)
                    {
                        obj.urgentInfo = null;
                    }
                });

                tasks.Add(outer2);


                var outer3 = Task.Factory.StartNew(() =>     // GetPutyovkas
                {
                    return PutyovkiContainer.GetPutyovkas(departmentBase);
                });

                outer3.ContinueWith((t) =>
                {
                    if (outer3.Status == TaskStatus.RanToCompletion)
                    {
                        obj.putyovkaInfo = t.Result;
                    }
                    if (outer3.Status == TaskStatus.Faulted)
                    {
                        obj.putyovkaInfo = null;
                    }
                });

                tasks.Add(outer3);

                var outer4 = Task.Factory.StartNew(() =>     // GetRealizData
                {
                    return RealizContainer.GetRealizData(departmentBase);
                });

                outer4.ContinueWith((t) =>
                {
                    if (outer4.Status == TaskStatus.RanToCompletion)
                    {
                        obj.realizInfo = t.Result;
                    }
                    if (outer4.Status == TaskStatus.Faulted)
                    {
                        obj.realizInfo = null;
                    }
                });

                tasks.Add(outer4);


                var outer5 = Task.Factory.StartNew(() =>     // GetZadoljennost
                {
                    return ZadoljennostContainer.GetZadoljennost(departmentBase);
                });

                outer5.ContinueWith((t) =>
                {
                    if (outer5.Status == TaskStatus.RanToCompletion)
                    {
                        obj.zadoljInfo = t.Result;
                    }
                    if (outer5.Status == TaskStatus.Faulted)
                    {
                        obj.zadoljInfo = null;
                    }
                });

                tasks.Add(outer5);


                var outer6 = Task.Factory.StartNew(() =>     // GetBrons
                {
                    return BronContainer.GetBrons(departmentBase);
                });

                outer6.ContinueWith((t) =>
                {
                    if (outer6.Status == TaskStatus.RanToCompletion)
                    {
                        obj.bronInfo = t.Result;
                    }
                    if (outer6.Status == TaskStatus.Faulted)
                    {
                        obj.bronInfo = null;
                    }
                });

                tasks.Add(outer6);


                var outer7 = Task.Factory.StartNew(() =>     // GetActiveUser
                {
                    return ActiveUserContainer.GetActiveUser(departmentBase);
                });

                outer7.ContinueWith((t) =>
                {
                    if (outer7.Status == TaskStatus.RanToCompletion)
                    {
                        obj.activeUsersInfo = t.Result;
                    }
                    if (outer7.Status == TaskStatus.Faulted)
                    {
                        obj.activeUsersInfo = null;
                    }
                });

                tasks.Add(outer7);


                var outer8 = Task.Factory.StartNew(() =>     // GetOrganizationData
                {
                    return PieChartDataContainer.GetOrganizationData(departmentBase);
                });

                outer8.ContinueWith((t) =>
                {
                    if (outer8.Status == TaskStatus.RanToCompletion)
                    {
                        obj.organizationPieInfo = t.Result;
                    }
                    if (outer8.Status == TaskStatus.Faulted)
                    {
                        obj.organizationPieInfo = null;
                    }
                });

                tasks.Add(outer8);

                var outer9 = Task.Factory.StartNew(() =>     // GetZayavki
                {
                    return PieChartDataContainer.GetZayavki(departmentBase, cur_year);
                });

                outer9.ContinueWith((t) =>
                {
                    if (outer9.Status == TaskStatus.RanToCompletion)
                    {
                        obj.zayavkiPieInfo = t.Result;
                    }
                    if (outer9.Status == TaskStatus.Faulted)
                    {
                        obj.zayavkiPieInfo = null;
                    }
                });

                tasks.Add(outer9);


                var outer10 = Task.Factory.StartNew(() =>     // GetPutyovki
                {
                    return PieChartDataContainer.GetPutyovki(departmentBase, cur_year);
                });

                outer10.ContinueWith((t) =>
                {
                    if (outer10.Status == TaskStatus.RanToCompletion)
                    {
                        obj.putyovkiPieInfo = t.Result;
                    }
                    if (outer10.Status == TaskStatus.Faulted)
                    {
                        obj.putyovkiPieInfo = null;
                    }
                });

                tasks.Add(outer10);


                var outer11 = Task.Factory.StartNew(() =>     // GetChastichka
                {
                    return ChastichkaContainer.GetChastichka(departmentBase);
                });

                outer11.ContinueWith((t) =>
                {
                    if (outer11.Status == TaskStatus.RanToCompletion)
                    {
                        obj.chastichkaInfo = t.Result;
                    }
                    if (outer11.Status == TaskStatus.Faulted)
                    {
                        obj.chastichkaInfo = null;
                    }
                });

                tasks.Add(outer11);

                //obj = new FourStapViewModel
                //{
                //    //kaznaInfo = KaznaInfoContainer.GetKaznaFederationInfo(departmentBase),
                //    //urgentInfo = UrgentPutyovkasContainer.GetUrgentPutyovkas(departmentBase),
                //    //putyovkaInfo = PutyovkiContainer.GetPutyovkas(departmentBase),
                //    //realizInfo = RealizContainer.GetRealizData(departmentBase),
                //    //zadoljInfo = ZadoljennostContainer.GetZadoljennost(departmentBase),
                //    //bronInfo = BronContainer.GetBrons(departmentBase),
                //    //activeUsersInfo = ActiveUserContainer.GetActiveUser(departmentBase),
                //    //organizationPieInfo = PieChartDataContainer.GetOrganizationData(departmentBase),
                //    //zayavkiPieInfo = PieChartDataContainer.GetZayavki(departmentBase, cur_year),
                //    //putyovkiPieInfo = PieChartDataContainer.GetPutyovki(departmentBase, cur_year),
                //    //chastichkaInfo = ChastichkaContainer.GetChastichka(departmentBase)
                //};
            }

            Task.WaitAll(tasks.ToArray(), 30000);

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="departmentBase"></param>
        /// <param name="cur_year"></param>
        public static void GetActiveUsers(FourStapViewModel current, string departmentBase, int? cur_year)
        {
            current.activeUsersInfo = ActiveUserContainer.GetActiveUser(departmentBase);
            current.kaznaLastDayInfo = KaznaInfoContainer.GetKaznaFederationLastDayInfo();
        }
    }
}