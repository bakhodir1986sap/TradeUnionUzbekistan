using KasabaKIAT.Models;
using KasabaKiat.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Putyovki.OplataPutyovok
{
    public class OplataRasxodDataManager
    {
        public static void Create(ref OplataRasxodiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblOplata_Ras opl = new tblOplata_Ras();

                //opl.ID            = model.ID              ;
                opl.ID_Opl          = model.ID_Opl          ;
                opl.ID_Sanat        = model.ID_Sanat        ;
                opl.Num_Put         = model.Num_Put         ;
                opl.Symma           = model.Symma           ;
                opl.ID_Pr           = model.ID_Pr           ;
                opl.Priz            = model.Priz            ;
                opl.UserId          = model.UserId          ;
                opl.Departmentbase  = model.Departmentbase;

                var opp = db.tblOplata_Ras.Add(opl);

                db.SaveChanges();

                model.ID = opp.ID;
            }
        }

        public static void Modify(OplataRasxodiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var opl = db.tblOplata_Ras.Where(z => z.ID == model.ID);

                if (opl.Any())
                {
                    var opl1 = opl.FirstOrDefault();

                    if (opl1 != null)
                    {
                        //opl1.ID = model.ID;
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

                db.SaveChanges();
            }
        }

        public static void Remove(OplataRasxodiViewModel model)
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

                db.SaveChanges();
            }
        }

        public static List<OplataRasxodiViewModel> Get(OplataRasxodiViewModel model)
        {
            List<OplataRasxodiViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from oplata in db.tblOplata_Ras
                            select new OplataRasxodiViewModel
                            {
                                ID              = model.ID      ,
                                ID_Opl          = model.ID_Opl  ,      
                                ID_Sanat        = model.ID_Sanat,      
                                Num_Put         = model.Num_Put ,      
                                Symma           = model.Symma   ,      
                                ID_Pr           = model.ID_Pr   ,      
                                Priz            = model.Priz    ,      
                                UserId          = model.UserId  ,      
                                Departmentbase  = model.Departmentbase
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
        public static List<OplataRasxodiViewModel> GetDetail(OplataRasxodiViewModel model)
        {
            List<OplataRasxodiViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from oplata in db.tblOplata_Ras
                            where oplata.Departmentbase == model.Departmentbase
                            && oplata.ID_Opl == model.ID_Opl
                            select new OplataRasxodiViewModel
                            {
                                ID = model.ID,
                                ID_Opl = model.ID_Opl,
                                ID_Sanat = model.ID_Sanat,
                                Num_Put = model.Num_Put,
                                Symma = model.Symma,
                                ID_Pr = model.ID_Pr,
                                Priz = model.Priz,
                                UserId = model.UserId,
                                Departmentbase = model.Departmentbase
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}