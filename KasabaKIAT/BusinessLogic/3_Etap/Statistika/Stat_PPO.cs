using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KasabaKIAT.BusinessLogic._3_Etap.Statistika.Stat_ViewModels;
using KasabaKIAT.Models;

namespace KasabaKIAT.BusinessLogic._3_Etap.Statistika
{
    public class Stat_PPO : BaseStatistika
    {
        public override IBaseStatViewModel GetData(string depCode, int? cur_year, int? cur_kv)
        {
            var model = new Stat_PPOViewModel();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                model.ApprovedZayavka = db.sp_stat_ApprovedZayavka(depCode, cur_year, cur_kv).FirstOrDefault();
                model.RejectedZayavka = db.sp_stat_RejectedZayavka(depCode, cur_year, cur_kv).FirstOrDefault();
                model.InProgressZayavka = db.sp_stat_InProgressZayavka(depCode, cur_year, cur_kv).FirstOrDefault();
            }

            return model;
        }
    }
}