using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KasabaKIAT.BusinessLogic._3_Etap.Statistika.Stat_ViewModels;
using KasabaKIAT.BusinessLogic.ClosePeriod.CloseKvartal;
using KasabaKIAT.Models;

namespace KasabaKIAT.BusinessLogic._3_Etap.Statistika
{
    public class Stat_RayonTuman : BaseStatistika
    {
        public override IBaseStatViewModel GetData(string depCode, int? cur_year, int? cur_kv)
        {
            var model = new Stat_RayonTumanViewModel();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                if ((!cur_year.HasValue) || (!cur_kv.HasValue))
                {
                    var parentCode = "172" + depCode.Substring(5, 2);

                    var cc = new CloseKvartal();
                    var ccrow = cc.GetCloseKvartalRow(parentCode);

                    cur_year = ccrow.curkv_year;
                    cur_kv = ccrow.curkv;
                }

                model.EcpIncome = db.sp_stat_EcpIncome(depCode, cur_year, cur_kv).FirstOrDefault();
                model.PartialIncome = db.sp_stat_PartialIncome(depCode, cur_year, cur_kv).FirstOrDefault();
                model.OtherIncome = db.sp_stat_OtherIncome(depCode, cur_year, cur_kv).FirstOrDefault();
                model.UrgentPutyovkas = db.sp_stat_UrgentPutyovkas(depCode, cur_year, cur_kv).FirstOrDefault();

                model.PlanPutyovkaCount = db.sp_stat_PlanPutyovkaCount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.PlanPutyovkaAmount = db.sp_stat_PlanPutyovkaAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.RealizPutyovkaCount = db.sp_stat_RealizPutyovkaCount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.RealizPutyovkaAmount = db.sp_stat_RealizPutyovkaAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.OstatokPutyovkaCount = db.sp_stat_OstatokPutyovkaCount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.OstatokPutyovkaAmount = db.sp_stat_OstatokPutyovkaAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.S45PutyovkaCount = db.sp_stat_S45PutyovkaCount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.S45PutyovkaAmount = db.sp_stat_S45PutyovkaAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.S100PutyovkaCount = db.sp_stat_S100PutyovkaCount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.S100PutyovkaAmount = db.sp_stat_S100PutyovkaAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.S0PutyovkaCount = db.sp_stat_S0PutyovkaCount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.S0PutyovkaAmount = db.sp_stat_S0PutyovkaAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.TotalZadoljennostAmount = db.sp_stat_TotalZadoljennostAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.ApprovedZayavka = db.sp_stat_ApprovedZayavka(depCode, cur_year, cur_kv).FirstOrDefault();
                model.RejectedZayavka = db.sp_stat_RejectedZayavka(depCode, cur_year, cur_kv).FirstOrDefault();
                model.InProgressZayavka = db.sp_stat_InProgressZayavka(depCode, cur_year, cur_kv).FirstOrDefault();
                model.OstatokPutyovkas = db.sp_stat_OstatokPutyovkas(depCode, cur_year, cur_kv).FirstOrDefault();
                model.RealizedPutyovkas = db.sp_stat_RealizedPutyovkas(depCode, cur_year, cur_kv).FirstOrDefault();
                model.VozvratPutyovkas = db.sp_stat_VozvratPutyovkas(depCode, cur_year, cur_kv).FirstOrDefault();
            }

            return model;
        }
    }
}