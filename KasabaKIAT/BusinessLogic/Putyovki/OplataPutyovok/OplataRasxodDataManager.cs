using KasabaKIAT.Models;
using KasabaKiat.ViewModel.Putyovki;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using KasabaKIAT.BusinessLogic.Putyovki.PutyovkiHistory;

namespace KasabaKiat.BusinessLogic.Putyovki.OplataPutyovok
{
    public class OplataRasxodDataManager
    {
        public static void Create(ref OplataRasxodiViewModel model, KasabaKIATEntities db, tblOplata opMain)
        {
                tblOplata_Ras opl = new tblOplata_Ras();

                opl.tblOplata = opMain;
                opl.ID_Opl          = model.ID_Opl          ;
                opl.ID_Sanat        = model.ID_Sanat        ;
                opl.Num_Put         = model.Num_Put         ;
                opl.Symma           = model.Symma           ;
                opl.ID_Pr           = model.ID_Pr           ;
                opl.Priz            = model.Priz            ;
                opl.UserId          = model.UserId          ;
                opl.Departmentbase  = model.Departmentbase;

                var opp = db.tblOplata_Ras.Add(opl);

            //History making
            var histModel = new PutHistViewModel
            {
                event_tp = (int)HistEventType.Oplata,
                departmentbase = model.Departmentbase,
                id_sanat = model.ID_Sanat,
                num_put = model.Num_Put,
                remark = string.Format("Процент {0} - Сумма {1} - Организация {2}" +
                "Дата счет фактуры -{3} Номер счет фактуры -{4} Доверенность - {5}", 
                                        model.ID_Pr.ToString(), 
                                        model.Symma,
                                        "",
                                        opMain.Data_Post.ToString(),
                                        opMain.Num_Doc,
                                        opMain.Osnov),
                userid = model.UserId
            };

            PutHistDataManager.CreateL(histModel, db);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static async Task Modify(OplataRasxodiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var opl = db.tblOplata_Ras.Where(z => z.ID == model.ID);

                if (opl.Any())
                {
                    var opl1 = opl.FirstOrDefault();

                    if (opl1 != null)
                    {
                        opl1.ID_Opl = model.ID_Opl;
                        opl1.ID_Sanat = model.ID_Sanat;
                        opl1.Num_Put = model.Num_Put;
                        opl1.Symma = model.Symma;
                        opl1.ID_Pr = model.ID_Pr;
                        opl1.Priz = model.Priz;
                        opl1.UserId = model.UserId;
                        opl1.Departmentbase = model.Departmentbase;
                    }
                }

                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task Remove(OplataRasxodiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var opl = db.tblOplata_Ras.Where(z => z.ID == model.ID);

                if (opl.Any())
                {
                    var reg = opl.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblOplata_Ras.Remove(reg);
                    }
                }

                await db.SaveChangesAsync();
            }
        }


        public static void RemoveByMaster(OplataRasxodiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var opls = db.tblOplata_Ras.Where(z => z.ID_Opl == model.ID_Opl);

                if (opls.Any())
                {
                    foreach(var opl in opls)
                    {
                        if (opl != null)
                        {
                            db.tblOplata_Ras.Remove(opl);
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<OplataRasxodiViewModel> Get(OplataRasxodiViewModel model)
        {
            List<OplataRasxodiViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from oplata in db.tblOplata_Ras
                            where oplata.ID == model.ID
                            select new OplataRasxodiViewModel
                            {
                                ID              = oplata.ID      ,
                                ID_Opl          = oplata.ID_Opl  ,      
                                ID_Sanat        = oplata.ID_Sanat,      
                                Num_Put         = oplata.Num_Put ,      
                                Symma           = oplata.Symma   ,      
                                ID_Pr           = oplata.ID_Pr   ,      
                                Priz            = oplata.Priz    ,      
                                UserId          = oplata.UserId  ,      
                                Departmentbase  = oplata.Departmentbase
                            };

                list = query.ToList();
            }

            return list;
        }

        /// <summary>
        /// Heararical
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<List<OplataRasxodiViewModel>> GetDetail(OplataRasxodiViewModel model)
        {
            List<OplataRasxodiViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from oplata in db.tblOplata_Ras
                            where oplata.Departmentbase == model.Departmentbase
                            && oplata.ID_Opl == model.ID_Opl
                            orderby oplata.Num_Put
                            select new OplataRasxodiViewModel
                            {
                                ID = oplata.ID,
                                ID_Opl = oplata.ID_Opl,
                                ID_Sanat = oplata.ID_Sanat,
                                SanatName = oplata.tblSprav_San.Name_San,
                                Num_Put = oplata.Num_Put,
                                Symma = oplata.Symma,
                                ID_Pr = oplata.ID_Pr,
                                Name_Proc = oplata.tblSprav_Pr1.Name_Proc,
                               
                                Priz = oplata.Priz,
                                UserId = oplata.UserId,
                                Departmentbase = oplata.Departmentbase
                            }).ToListAsync();

                list = query;
            }

            return list;
        }
    }
}