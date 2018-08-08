using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.PutyovkiHistory
{
    public class PutHistDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Create(PutHistViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblPutHist hist = new tblPutHist
                {
                    EVENT_TP = model.event_tp,
                    Num_Put = model.num_put,
                    ID_Sanat = model.id_sanat,
                    REMARK = model.remark,
                    UserId = model.userid,
                    DepartmentBase = model.departmentbase,
                    TIMESTAMP_T = DateTime.Now
                };

                var pst = db.tblPutHists.Add(hist);

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="db"></param>
        public static void CreateL(PutHistViewModel model, KasabaKIATEntities db)
        {
            tblPutHist hist = new tblPutHist
            {
                EVENT_TP = model.event_tp,
                Num_Put = model.num_put,
                ID_Sanat = model.id_sanat,
                REMARK = model.remark,
                UserId = model.userid,
                DepartmentBase = model.departmentbase,
                TIMESTAMP_T = DateTime.Now
            };

            var pst = db.tblPutHists.Add(hist);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Modify(PutHistViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var vipl = db.tblPutHists.Where(z => z.DepartmentBase == model.departmentbase);

                if (vipl.Any())
                {
                    var hist1 = vipl.FirstOrDefault();

                    if (hist1 != null)
                    {
                        hist1.EVENT_TP = model.event_tp;
                        hist1.Num_Put = model.num_put;
                        hist1.ID_Sanat = model.id_sanat;
                        hist1.REMARK = model.remark;
                    }
                }

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="db"></param>
        public static void ModifyL(PutHistViewModel model, KasabaKIATEntities db)
        {
                var vipl = db.tblPutHists.Where(z => z.DepartmentBase == model.departmentbase);

                if (vipl.Any())
                {
                    var hist1 = vipl.FirstOrDefault();

                    if (hist1 != null)
                    {
                        hist1.EVENT_TP = model.event_tp;
                        hist1.Num_Put = model.num_put;
                        hist1.ID_Sanat = model.id_sanat;
                        hist1.REMARK = model.remark;
                    }
                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<PutHistViewModel> Get(PutHistViewModel model)
        {
            List<PutHistViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from hist in db.tblPutHists
                            where hist.DepartmentBase == model.departmentbase
                            && hist.ID_Sanat == model.id_sanat &&
                            hist.Num_Put == model.num_put
                            select new PutHistViewModel
                            {
                                id                   = hist.ID             ,
                                event_tp             = hist.EVENT_TP       ,
                                num_put              = hist.Num_Put        ,
                                id_sanat             = hist.ID_Sanat       ,
                                sanatName            = hist.tblSprav_San.Name_San,
                                remark               = hist.REMARK         ,
                                userid               = hist.UserId         ,
                                departmentbase       = hist.DepartmentBase ,
                                timestamp_t          = hist.TIMESTAMP_T ?? DateTime.Now
                            };

                list = query.ToList();

                foreach(var hist in list)
                {
                    hist.event_name = GetEventName(hist.event_tp);
                    hist.timeString = hist.timestamp_t.Value.ToShortDateString() + " " + hist.timestamp_t.Value.ToLongTimeString();
                }
            }

            return list;
        }

        public static string GetEventName(int eventTp)
        {
            //Prixod,
            //Rasxod,
            //Oplata,
            //Vozvrat,
            //UstanovVlad,
            //IzmPutDate,
            //BronPutyovka,
            //SnyatBronPutyovka,
            //UdalitPutIzPrixod,
            //UdalitPutIzRasxod,
            //UdalitPutIzVozvrat


            string eventName = string.Empty;
            switch(eventTp)
            {
                case 0:
                    eventName = "Приход";
                    break;
                case 1:
                    eventName = "Расход";
                    break;
                case 2:
                    eventName = "Оплата";
                    break;
                case 3:
                    eventName = "Возврат";
                    break;
                case 4:
                    eventName = "Установка владельца";
                    break;
                case 5:
                    eventName = "Изминение Даты";
                    break;
                case 6:
                    eventName = "Бронирование";
                    break;
                case 7:
                    eventName = "Снятие брони";
                    break;
                case 8:
                    eventName = "Удаление из прихода";
                    break;
                case 9:
                    eventName = "Удаление из расхода";
                    break;
                case 10:
                    eventName = "Удаление из возврата";
                    break;
            }

            return eventName;
        }
    }
}