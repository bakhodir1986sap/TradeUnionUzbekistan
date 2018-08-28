using KasabaKIAT.BusinessLogic._3_Etap.Statistika.Stat_ViewModels;
using KasabaKIAT.BusinessLogic.ClosePeriod.CloseKvartal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._3_Etap.Statistika
{
    public class StatController
    {
        public static IBaseStatViewModel  Process(string depCode, long role_id, int? cur_year, int? cur_kvartal)
        {
            var stat = BaseStatistika.CreateStat(role_id);

            var result =  stat.GetData(depCode, cur_year, cur_kvartal);

            return result;
        }
    }
}