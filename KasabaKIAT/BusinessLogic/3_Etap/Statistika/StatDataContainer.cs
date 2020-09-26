using KasabaKIAT.BusinessLogic._3_Etap.Statistika.Stat_ViewModels;
using KasabaKIAT.BusinessLogic._4_Etap.Statistics;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KasabaKIAT.BusinessLogic._3_Etap.Statistika
{
    /// <summary>
    /// 
    /// </summary>
    public static class StatDataContainer
    {
        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<string, IBaseStatViewModel> statDataPerUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="depCode"></param>
        /// <param name="role_id"></param>
        /// <param name="cur_year"></param>
        /// <param name="cur_kvartal"></param>
        /// <returns></returns>
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
                    try
                    {
                        var result = StatController.Process(depCode, role_id, cur_year, cur_kvartal);
                        result.LastChangeDate = DateTime.Now;
                        statDataPerUser[depCode] = result;

                        return result;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    _statModel.AktivUsers = AktiveUsersCount();
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

        /// <summary>
        /// 
        /// </summary>
        public static string _modelChangedTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="depCode"></param>
        /// <param name="cur_year"></param>
        /// <returns></returns>
        public static IBaseStatViewModel GetStataData(string depCode, int? cur_year)
        {
            IBaseStatViewModel _statModel = null;

            try
            {
                if (statDataPerUser == null)
                {
                    statDataPerUser = new Dictionary<string, IBaseStatViewModel>();
                }
                                
                if (statDataPerUser.TryGetValue(depCode, out _statModel))
                {
                    if (_statModel.LastChangeDate.AddMinutes(240) <= DateTime.Now)
                    {
                        try
                        {
                            FourStapViewModel obj = null;
                            var result = FourStepController.Get4StageInfo(depCode, cur_year, obj);
                            result.LastChangeDate = DateTime.Now;
                            _modelChangedTime = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss");
                            statDataPerUser[depCode] = result;

                            return result;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        FourStepController.GetActiveUsers((FourStapViewModel)_statModel, depCode, cur_year);

                        return _statModel;
                    }
                }
                else
                {
                    FourStapViewModel obj = null;
                    var result = FourStepController.Get4StageInfo(depCode, cur_year, obj);
                    result.LastChangeDate = DateTime.Now;
                    _modelChangedTime = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss");
                    statDataPerUser.Add(depCode, result);

                    return result;
                }
            }
            catch(Exception ex)
            {

            }

            return _statModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int? AktiveUsersCount()
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {

                return db.sp_stat_AktiveUsersList("", 0, 0).FirstOrDefault();
            }
        }
    }
}