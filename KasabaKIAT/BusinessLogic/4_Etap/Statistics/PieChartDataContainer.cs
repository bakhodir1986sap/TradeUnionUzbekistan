using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class PieChartDataContainer
    {
        public static List<PieChartData> GetOrganizationData()
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var firstValue = db.sp_stat_ProfComExists("17", 0, 0).First().Value;
                var secondValue = db.sp_stat_ProfComNotExists("17", 0, 0).First().Value;

                List<PieChartData> chartData = new List<PieChartData>
                {
                    new PieChartData { xValue = "Есть профком", yValue = firstValue, text = string.Format("Есть профком {0} шт" , firstValue)},
                    new PieChartData { xValue = "Нет профкома", yValue = secondValue, text = string.Format("Нет профкома {0} шт" , secondValue)},
                };

                return chartData;
            }
        }


        public static List<PieChartData> GetOrganizationData(string departmentBase)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var firstValue = db.sp_stat_ProfComExists(departmentBase, 0, 0).First().Value;
                var secondValue = db.sp_stat_ProfComNotExists(departmentBase, 0, 0).First().Value;

                List<PieChartData> chartData = new List<PieChartData>
                {
                    new PieChartData { xValue = "Есть профком", yValue = firstValue, text = string.Format("Есть профком {0} шт" , firstValue)},
                    new PieChartData { xValue = "Нет профкома", yValue = secondValue, text = string.Format("Нет профкома {0} шт" , secondValue)},
                };

                return chartData;
            }
        }


        public static List<PieChartData> GetZayavki(int? cur_year)
        {
            //"Утвержденные", "Откланенные", "На рассмотрении"
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var firstValue = db.sp_stat_ApprovedZayavka_V2("17", cur_year).First().Value;
                var secondValue = db.sp_stat_RejectedZayavka_V2("17",  cur_year).First().Value;
                var therdValue = db.sp_stat_InProgressZayavka_V2("17",  cur_year).First().Value;

                List<PieChartData> chartData = new List<PieChartData>
                {
                    new PieChartData { xValue = "Утвержденные", yValue = firstValue, text = string.Format("Утвержденные {0} шт" , firstValue)},
                    new PieChartData { xValue = "Откланенные", yValue = secondValue, text = string.Format("Откланенные {0} шт" , secondValue)},
                    new PieChartData { xValue = "На рассмотрении", yValue = therdValue, text = string.Format("На рассмотрении {0} шт" , therdValue)},
                };

                return chartData;
            }
        }


        public static List<PieChartData> GetZayavki(string departmentBase, int? cur_year)
        {
            //"Утвержденные", "Откланенные", "На рассмотрении"
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var firstValue = db.sp_stat_ApprovedZayavka_V2(departmentBase, cur_year).First().Value;
                var secondValue = db.sp_stat_RejectedZayavka_V2(departmentBase, cur_year).First().Value;
                var therdValue = db.sp_stat_InProgressZayavka_V2(departmentBase, cur_year).First().Value;

                List<PieChartData> chartData = new List<PieChartData>
                {
                    new PieChartData { xValue = "Утвержденные", yValue = firstValue, text = string.Format("Утвержденные {0} шт" , firstValue)},
                    new PieChartData { xValue = "Откланенные", yValue = secondValue, text = string.Format("Откланенные {0} шт" , secondValue)},
                    new PieChartData { xValue = "На рассмотрении", yValue = therdValue, text = string.Format("На рассмотрении {0} шт" , therdValue)},
                };

                return chartData;
            }
        }

        public static List<PieChartData> GetPutyovki(int? cur_year)
        {
            //"Остаток", "Реализованные", "Возврат"
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var firstValue = db.sp_stat_OstatokPutyovka_V2("17", cur_year).First().Value;
                var secondValue = db.sp_stat_RealizedPutyovkas_V2("17", cur_year).First().Value;
                var therdValue = db.sp_stat_VozvratPutyovkas_V2("17", cur_year).First().Value;

                List<PieChartData> chartData = new List<PieChartData>
                {
                    new PieChartData { xValue = "Остатки", yValue = firstValue, text = string.Format("Остатки {0} шт" , firstValue)},
                    new PieChartData { xValue = "Реализованные", yValue = secondValue, text = string.Format("Реализованные {0} шт" , secondValue)},
                    new PieChartData { xValue = "Возврат", yValue = therdValue, text = string.Format("Возврат {0} шт" , therdValue)},
                };

                return chartData;
            }
        }


        public static List<PieChartData> GetPutyovki(string departmentBase, int? cur_year)
        {
            //"Остаток", "Реализованные", "Возврат"
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var firstValue = db.sp_stat_OstatokPutyovka_V2(departmentBase, cur_year).First().Value;
                var secondValue = db.sp_stat_RealizedPutyovkas_V2(departmentBase, cur_year).First().Value;
                var therdValue = db.sp_stat_VozvratPutyovkas_V2(departmentBase, cur_year).First().Value;

                List<PieChartData> chartData = new List<PieChartData>
                {
                    new PieChartData { xValue = "Остатки", yValue = firstValue, text = string.Format("Остатки {0} шт" , firstValue)},
                    new PieChartData { xValue = "Реализованные", yValue = secondValue, text = string.Format("Реализованные {0} шт" , secondValue)},
                    new PieChartData { xValue = "Возврат", yValue = therdValue, text = string.Format("Возврат {0} шт" , therdValue)},
                };

                return chartData;
            }
        }
    }
}