using KasabaKIAT.BusinessLogic._3_Etap.Statistika.Stat_ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._3_Etap.Statistika
{
    public static class StatDataContainer
    {
        public static Dictionary<string,IBaseStatViewModel> statDataPerUser { get; set; }

        public static IBaseStatViewModel GetStataData(string depCode, long role_id, int? cur_year, int? cur_kvartal)
        {
            if (statDataPerUser == null)
            {
                statDataPerUser = new Dictionary<string, IBaseStatViewModel>();
            }

            IBaseStatViewModel _statModel = null;
            if (statDataPerUser.TryGetValue(depCode, out _statModel))
            {
                if (_statModel.LastChangeDate.AddDays(1) <= DateTime.Now)
                {
                    var result = StatController.Process(depCode, role_id, cur_year, cur_kvartal);
                    result.LastChangeDate = DateTime.Now;
                    statDataPerUser.Add(depCode, result);

                    return result;
                }
                else
                {
                    return _statModel;
                }
            }
            else
            {
                var result = StatController.Process(depCode, role_id, cur_year, cur_kvartal);
                result.LastChangeDate = DateTime.Now;
                statDataPerUser.Add(depCode, result);

                return result;
            }
        }
    }
}