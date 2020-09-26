using KasabaKIAT.BusinessLogic._3_Etap.Statistika.Stat_ViewModels;
using KasabaKIAT.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KasabaKIAT.BusinessLogic._3_Etap.Statistika
{
    public class Stat_Federasiya : BaseStatistika
    {
        public override IBaseStatViewModel GetData(string depCode, int? cur_year, int? cur_kv)
        {
                var model = new Stat_FederasiyaViewModel();

                List<Task> tasks = new List<Task>();

                var outer1 = Task.Factory.StartNew(() =>     // AktiveUsersList
                {
                    using (KasabaKIATEntities db1 = new KasabaKIATEntities())
                    {
                        return db1.sp_stat_AktiveUsersList(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer1.ContinueWith((t) => {
                    if (outer1.Status == TaskStatus.RanToCompletion)
                    {
                        model.AktivUsers = t.Result;
                    }
                    if (outer1.Status == TaskStatus.Faulted)
                    {
                        model.AktivUsers = 0;
                    }
                });

                tasks.Add(outer1);

                var outer2 = Task.Factory.StartNew(() =>     // EcpIncomeFed
                {
                    using (KasabaKIATEntities db2 = new KasabaKIATEntities())
                    {
                        return db2.sp_stat_EcpIncomeFed(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                    
                });

                outer2.ContinueWith((t) => {
                    if (outer2.Status == TaskStatus.RanToCompletion)
                    {
                        model.EcpIncome = t.Result;
                    }
                    if (outer2.Status == TaskStatus.Faulted)
                    {
                        model.EcpIncome = 0;
                    }
                });

                tasks.Add(outer2);

                var outer3 = Task.Factory.StartNew(() =>     // PartialIncome
                {
                    using (KasabaKIATEntities db3 = new KasabaKIATEntities())
                    {
                        return db3.sp_stat_PartialIncome(depCode, cur_year, cur_kv).FirstOrDefault();
                    }

                });

                outer3.ContinueWith((t) => {
                    if (outer3.Status == TaskStatus.RanToCompletion)
                    {
                        model.PartialIncome = t.Result;
                    }
                    if (outer3.Status == TaskStatus.Faulted)
                    {
                        model.PartialIncome = 0;
                    }
                });

                tasks.Add(outer3);

                var outer4 = Task.Factory.StartNew(() =>     // OtherIncome
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_OtherIncome(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer4.ContinueWith((t) => {
                    if (outer4.Status == TaskStatus.RanToCompletion)
                    {
                        model.OtherIncome = t.Result;
                    }
                    if (outer4.Status == TaskStatus.Faulted)
                    {
                        model.OtherIncome = 0;
                    }
                });

                tasks.Add(outer4);

                var outer5 = Task.Factory.StartNew(() =>     // IncomePerRegionList_Fed
                {
                    using (KasabaKIATEntities db5 = new KasabaKIATEntities())
                    {
                        return db5.sp_stat_IncomePerRegionList_Fed(depCode, cur_year, cur_kv).ToList();
                    }
                });

                outer5.ContinueWith((t) => {
                    if (outer5.Status == TaskStatus.RanToCompletion)
                    {
                        var IncomePerRegionList = t.Result;

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
                    }
                    if (outer5.Status == TaskStatus.Faulted)
                    {  
                        model.IncomePerRegionList = new List<IncomeRegion>();
                    }
                });

                tasks.Add(outer5);

                var outer6 = Task.Factory.StartNew(() =>     // UrgentPutyovkas
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_UrgentPutyovkas(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer6.ContinueWith((t) => {
                    if (outer6.Status == TaskStatus.RanToCompletion)
                    {
                        model.UrgentPutyovkas = t.Result;
                    }
                    if (outer6.Status == TaskStatus.Faulted)
                    {
                        model.UrgentPutyovkas = 0;
                    }
                });

                tasks.Add(outer6);

                var outer7 = Task.Factory.StartNew(() =>     // UrgentPutyovkasRegionListFed
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_UrgentPutyovkasRegionListFed(depCode, cur_year, cur_kv).ToList();
                    }
                });

                outer7.ContinueWith((t) => {
                    if (outer7.Status == TaskStatus.RanToCompletion)
                    {
                        var UrgentPutyovkasRegionList = t.Result;

                        model.UrgentPutyovkasRegionList = new List<UrgentPutyovkaRegion>();

                        foreach (var obj in UrgentPutyovkasRegionList)
                        {
                            var model2x = new UrgentPutyovkaRegion
                            {
                                RegionName = obj.ProfsoyzNaimenovanie,
                                PutyovkasCount = obj.PutCount
                            };

                            model.UrgentPutyovkasRegionList.Add(model2x);
                        }
                    }
                    if (outer7.Status == TaskStatus.Faulted)
                    {
                        model.UrgentPutyovkasRegionList = new List<UrgentPutyovkaRegion>();
                    }
                });

                tasks.Add(outer7);

                var outer8 = Task.Factory.StartNew(() =>     // PlanPutyovkaCount
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_PlanPutyovkaCount(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer8.ContinueWith((t) => {
                    if (outer8.Status == TaskStatus.RanToCompletion)
                    {
                        model.PlanPutyovkaCount = t.Result;
                    }
                    if (outer8.Status == TaskStatus.Faulted)
                    {
                        model.PlanPutyovkaCount = 0;
                    }
                });

                tasks.Add(outer8);

                var outer9 = Task.Factory.StartNew(() =>     // PlanPutyovkaAmount
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_PlanPutyovkaAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer9.ContinueWith((t) => {
                    if (outer9.Status == TaskStatus.RanToCompletion)
                    {
                        model.PlanPutyovkaAmount = t.Result;
                    }
                    if (outer9.Status == TaskStatus.Faulted)
                    {
                        model.PlanPutyovkaAmount = 0;
                    }
                });

                tasks.Add(outer9);

                var outer10 = Task.Factory.StartNew(() =>     // RealizPutyovkaCount
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_RealizPutyovkaCount(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer10.ContinueWith((t) => {
                    if (outer10.Status == TaskStatus.RanToCompletion)
                    {
                        model.RealizPutyovkaCount = t.Result;
                    }
                    if (outer10.Status == TaskStatus.Faulted)
                    {
                        model.RealizPutyovkaCount = 0;
                    }
                });

                tasks.Add(outer10);

                var outer11 = Task.Factory.StartNew(() =>     // RealizPutyovkaAmount
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_RealizPutyovkaAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer11.ContinueWith((t) => {
                    if (outer11.Status == TaskStatus.RanToCompletion)
                    {
                        model.RealizPutyovkaAmount = t.Result;
                    }
                    if (outer11.Status == TaskStatus.Faulted)
                    {
                        model.RealizPutyovkaAmount = 0;
                    }
                });

                tasks.Add(outer11);

                var outer12 = Task.Factory.StartNew(() =>     // OstatokPutyovkaCount
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_OstatokPutyovkaCount(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer12.ContinueWith((t) => {
                    if (outer12.Status == TaskStatus.RanToCompletion)
                    {
                        model.OstatokPutyovkaCount = t.Result;
                    }
                    if (outer12.Status == TaskStatus.Faulted)
                    {
                        model.OstatokPutyovkaCount = 0;
                    }
                });

                tasks.Add(outer12);

                var outer13 = Task.Factory.StartNew(() =>     // OstatokPutyovkaAmount
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_OstatokPutyovkaAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer13.ContinueWith((t) => {
                    if (outer13.Status == TaskStatus.RanToCompletion)
                    {
                        model.OstatokPutyovkaAmount = t.Result;
                    }
                    if (outer13.Status == TaskStatus.Faulted)
                    {
                        model.OstatokPutyovkaAmount = 0;
                    }
                });

                tasks.Add(outer13);

                var outer14 = Task.Factory.StartNew(() =>     // S45PutyovkaCount
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_S45PutyovkaCount(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer14.ContinueWith((t) => {
                    if (outer14.Status == TaskStatus.RanToCompletion)
                    {
                        model.S45PutyovkaCount = t.Result;
                    }
                    if (outer14.Status == TaskStatus.Faulted)
                    {
                        model.S45PutyovkaCount = 0;
                    }
                });

                tasks.Add(outer14);

                var outer15 = Task.Factory.StartNew(() =>     // S45PutyovkaAmount
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_S45PutyovkaAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer15.ContinueWith((t) => {
                    if (outer15.Status == TaskStatus.RanToCompletion)
                    {
                        model.S45PutyovkaAmount = t.Result;
                    }
                    if (outer15.Status == TaskStatus.Faulted)
                    {
                        model.S45PutyovkaAmount = 0;
                    }
                });

                tasks.Add(outer15);

                var outer16 = Task.Factory.StartNew(() =>     // S100PutyovkaCount
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_S100PutyovkaCount(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer16.ContinueWith((t) => {
                    if (outer16.Status == TaskStatus.RanToCompletion)
                    {
                        model.S100PutyovkaCount = t.Result;
                    }
                    if (outer16.Status == TaskStatus.Faulted)
                    {
                        model.S100PutyovkaCount = 0;
                    }
                });

                tasks.Add(outer16);

                var outer17 = Task.Factory.StartNew(() =>     // S100PutyovkaAmount
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_S100PutyovkaAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer17.ContinueWith((t) => {
                    if (outer17.Status == TaskStatus.RanToCompletion)
                    {
                        model.S100PutyovkaAmount = t.Result;
                    }
                    if (outer17.Status == TaskStatus.Faulted)
                    {
                        model.S100PutyovkaAmount = 0;
                    }
                });

                tasks.Add(outer17);

                var outer18 = Task.Factory.StartNew(() =>     // S0PutyovkaCount
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_S0PutyovkaCount(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer18.ContinueWith((t) => {
                    if (outer18.Status == TaskStatus.RanToCompletion)
                    {
                        model.S0PutyovkaCount = t.Result;
                    }
                    if (outer18.Status == TaskStatus.Faulted)
                    {
                        model.S0PutyovkaCount = 0;
                    }
                });

                tasks.Add(outer18);

                var outer19 = Task.Factory.StartNew(() =>     // S0PutyovkaAmount
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_S0PutyovkaAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer19.ContinueWith((t) => {
                    if (outer19.Status == TaskStatus.RanToCompletion)
                    {
                        model.S0PutyovkaAmount = t.Result;
                    }
                    if (outer19.Status == TaskStatus.Faulted)
                    {
                        model.S0PutyovkaAmount = 0;
                    }
                });

                tasks.Add(outer19);

                var outer20 = Task.Factory.StartNew(() =>     // TotalZadoljennostAmount
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_TotalZadoljennostAmount(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer20.ContinueWith((t) => {
                    if (outer20.Status == TaskStatus.RanToCompletion)
                    {
                        model.TotalZadoljennostAmount = t.Result;
                    }
                    if (outer20.Status == TaskStatus.Faulted)
                    {
                        model.TotalZadoljennostAmount = 0;
                    }
                });

                tasks.Add(outer20);

                var outer21 = Task.Factory.StartNew(() =>     // ZadoljennostListFed
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_ZadoljennostListFed(depCode, cur_year, cur_kv).ToList();
                    }
                });

                outer21.ContinueWith((t) => {
                    if (outer21.Status == TaskStatus.RanToCompletion)
                    {
                        var ZadoljennostLists = t.Result;
                        model.ZadoljennostList = new List<ZadoljennostRegion>();

                        foreach (var obj in ZadoljennostLists)
                        {
                            var model3x = new ZadoljennostRegion();

                            model3x.RegionName = obj.ProfsoyzNaimenovanie;
                            model3x.ZadoljennostAmount = obj.ZadolAmount;

                            model.ZadoljennostList.Add(model3x);
                        }
                    }
                    if (outer21.Status == TaskStatus.Faulted)
                    {
                        model.TotalZadoljennostAmount = 0;
                    }
                });

                tasks.Add(outer21);

                var outer22 = Task.Factory.StartNew(() =>     // ProfComExists
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_ProfComExists(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer22.ContinueWith((t) => {
                    if (outer22.Status == TaskStatus.RanToCompletion)
                    {
                        model.ProfComExists = t.Result;
                    }
                    if (outer22.Status == TaskStatus.Faulted)
                    {
                        model.ProfComExists = 0;
                    }
                });

                tasks.Add(outer22);

                var outer23 = Task.Factory.StartNew(() =>     // ProfComNotExists
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_ProfComNotExists(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer23.ContinueWith((t) => {
                    if (outer23.Status == TaskStatus.RanToCompletion)
                    {
                        model.ProfComNotExists = t.Result;
                    }
                    if (outer23.Status == TaskStatus.Faulted)
                    {
                        model.ProfComNotExists = 0;
                    }
                });

                tasks.Add(outer23);

                var outer24 = Task.Factory.StartNew(() =>     // ApprovedZayavka
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_ApprovedZayavka(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer24.ContinueWith((t) => {
                    if (outer24.Status == TaskStatus.RanToCompletion)
                    {
                        model.ApprovedZayavka = t.Result;
                    }
                    if (outer24.Status == TaskStatus.Faulted)
                    {
                        model.ApprovedZayavka = 0;
                    }
                });

                tasks.Add(outer24);

                var outer25 = Task.Factory.StartNew(() =>     // RejectedZayavka
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_RejectedZayavka(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer25.ContinueWith((t) => {
                    if (outer25.Status == TaskStatus.RanToCompletion)
                    {
                        model.RejectedZayavka = t.Result;
                    }
                    if (outer25.Status == TaskStatus.Faulted)
                    {
                        model.RejectedZayavka = 0;
                    }
                });

                tasks.Add(outer25);

                var outer26 = Task.Factory.StartNew(() =>     // InProgressZayavka
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_InProgressZayavka(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer26.ContinueWith((t) => {
                    if (outer26.Status == TaskStatus.RanToCompletion)
                    {
                        model.InProgressZayavka = t.Result;
                    }
                    if (outer26.Status == TaskStatus.Faulted)
                    {
                        model.InProgressZayavka = 0;
                    }
                });

                tasks.Add(outer26);

                var outer27 = Task.Factory.StartNew(() =>     // OstatokPutyovkas
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_OstatokPutyovkas(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer27.ContinueWith((t) => {
                    if (outer27.Status == TaskStatus.RanToCompletion)
                    {
                        model.OstatokPutyovkas = t.Result;
                    }
                    if (outer27.Status == TaskStatus.Faulted)
                    {
                        model.OstatokPutyovkas = 0;
                    }
                });

                tasks.Add(outer27);

                var outer28 = Task.Factory.StartNew(() =>     // RealizedPutyovkas
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_RealizedPutyovkas(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer28.ContinueWith((t) => {
                    if (outer28.Status == TaskStatus.RanToCompletion)
                    {
                        model.RealizedPutyovkas = t.Result;
                    }
                    if (outer28.Status == TaskStatus.Faulted)
                    {
                        model.RealizedPutyovkas = 0;
                    }
                });

                tasks.Add(outer28);

                var outer29 = Task.Factory.StartNew(() =>     // VozvratPutyovkas
                {
                    using (KasabaKIATEntities db4 = new KasabaKIATEntities())
                    {
                        return db4.sp_stat_VozvratPutyovkas(depCode, cur_year, cur_kv).FirstOrDefault();
                    }
                });

                outer29.ContinueWith((t) => {
                    if (outer28.Status == TaskStatus.RanToCompletion)
                    {
                        model.VozvratPutyovkas = t.Result;
                    }
                    if (outer28.Status == TaskStatus.Faulted)
                    {
                        model.VozvratPutyovkas = 0;
                    }
                });

            tasks.Add(outer29);

            Task.WaitAll(tasks.ToArray(), 60000);

            return model;
        }


    }
}