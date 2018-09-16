using KasabaKIAT.BusinessLogic._3_Etap.Statistika.Stat_ViewModels;
using KasabaKIAT.BusinessLogic.ClosePeriod.CloseKvartal;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._3_Etap.Statistika
{
    public class StatController
    {
         static ILog log = log4net.LogManager.GetLogger(typeof(StatController));

        public static IBaseStatViewModel  Process(string depCode, long role_id, int? cur_year, int? cur_kvartal)
        {
            try
            {
                log.Info("Statistika Federasiya");

                var stat = BaseStatistika.CreateStat(role_id);

                var result = stat.GetData(depCode, cur_year, cur_kvartal);

                return result;
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }

            return null;
        }
    }
}