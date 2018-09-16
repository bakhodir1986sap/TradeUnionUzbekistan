using KasabaKIAT.BusinessLogic._3_Etap.Statistika.Stat_ViewModels;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._3_Etap.Statistika
{
    public class Stat_Federasiya : BaseStatistika
    {
        public override IBaseStatViewModel GetData(string depCode, int? cur_year, int? cur_kv)
        {
            var model = new Stat_FederasiyaViewModel();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {

                model.AktivUsers = db.sp_stat_AktiveUsersList(depCode, cur_year, cur_kv).FirstOrDefault();

                model.EcpIncome = db.sp_stat_EcpIncomeFed(depCode, cur_year, cur_kv).FirstOrDefault();
                model.PartialIncome = db.sp_stat_PartialIncome(depCode, cur_year, cur_kv).FirstOrDefault();
                model.OtherIncome = db.sp_stat_OtherIncome(depCode, cur_year, cur_kv).FirstOrDefault();
                var IncomePerRegionList = db.sp_stat_IncomePerRegionList_Fed(depCode, cur_year, cur_kv);

                model.IncomePerRegionList = new List<IncomeRegion>();

                foreach (var obj in IncomePerRegionList)
                {
                    var modelx = new IncomeRegion
                    {
                        RegionName = obj.ProfsoyzNaimenovanie,
                        TotalIncome = obj.TOTAL_AMT,
                        EcpIncome = obj.OBLSOVPROF_AMT,
                        PartialIncome = obj.CHASTICHKA,
                        OtherIncome = obj.OTHERS
                    };

                    model.IncomePerRegionList.Add(modelx);
                }

                model.UrgentPutyovkas = db.sp_stat_UrgentPutyovkas(depCode, cur_year, cur_kv).FirstOrDefault();

                model.UrgentPutyovkasRegionList = new List<UrgentPutyovkaRegion>();

                var UrgentPutyovkasRegionList = db.sp_stat_UrgentPutyovkasRegionListFed(depCode, cur_year, cur_kv);

                foreach (var obj in UrgentPutyovkasRegionList)
                {
                    var model2x = new UrgentPutyovkaRegion
                    {
                        RegionName = obj.ProfsoyzNaimenovanie,
                        PutyovkasCount = obj.PutCount
                    };

                    model.UrgentPutyovkasRegionList.Add(model2x);
                }

                model.PlanPutyovkaCount         = db.sp_stat_PlanPutyovkaCount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.PlanPutyovkaAmount        = db.sp_stat_PlanPutyovkaAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.RealizPutyovkaCount       = db.sp_stat_RealizPutyovkaCount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.RealizPutyovkaAmount      = db.sp_stat_RealizPutyovkaAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.OstatokPutyovkaCount      = db.sp_stat_OstatokPutyovkaCount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.OstatokPutyovkaAmount     = db.sp_stat_OstatokPutyovkaAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.S45PutyovkaCount          = db.sp_stat_S45PutyovkaCount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.S45PutyovkaAmount         = db.sp_stat_S45PutyovkaAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.S100PutyovkaCount         = db.sp_stat_S100PutyovkaCount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.S100PutyovkaAmount        = db.sp_stat_S100PutyovkaAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.S0PutyovkaCount           = db.sp_stat_S0PutyovkaCount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.S0PutyovkaAmount          = db.sp_stat_S0PutyovkaAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                model.TotalZadoljennostAmount   = db.sp_stat_TotalZadoljennostAmount(depCode, cur_year, cur_kv).FirstOrDefault();

                var ZadoljennostLists = db.sp_stat_ZadoljennostListFed(depCode, cur_year, cur_kv);
                model.ZadoljennostList = new List<ZadoljennostRegion>();

                foreach (var obj in ZadoljennostLists)
                {
                    var model3x = new ZadoljennostRegion();

                    model3x.RegionName = obj.ProfsoyzNaimenovanie;
                    model3x.ZadoljennostAmount = obj.ZadolAmount;

                    model.ZadoljennostList.Add(model3x);
                }


                model.ProfComExists       =  db.sp_stat_ProfComExists(depCode, cur_year, cur_kv).FirstOrDefault();
                model.ProfComNotExists    =  db.sp_stat_ProfComNotExists(depCode, cur_year, cur_kv).FirstOrDefault();
                model.ApprovedZayavka     =  db.sp_stat_ApprovedZayavka(depCode, cur_year, cur_kv).FirstOrDefault();
                model.RejectedZayavka     =  db.sp_stat_RejectedZayavka(depCode, cur_year, cur_kv).FirstOrDefault();
                model.InProgressZayavka   =  db.sp_stat_InProgressZayavka(depCode, cur_year, cur_kv).FirstOrDefault();
                model.OstatokPutyovkas    =  db.sp_stat_OstatokPutyovkas(depCode, cur_year, cur_kv).FirstOrDefault();
                model.RealizedPutyovkas   =  db.sp_stat_RealizedPutyovkas(depCode, cur_year, cur_kv).FirstOrDefault();
                model.VozvratPutyovkas    =  db.sp_stat_VozvratPutyovkas(depCode, cur_year, cur_kv).FirstOrDefault();
                
            }

            return model;
        }


    }
}