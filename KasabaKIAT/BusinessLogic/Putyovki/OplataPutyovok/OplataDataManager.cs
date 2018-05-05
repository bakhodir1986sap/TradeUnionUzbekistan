using KasabaKIAT.Models;
using KasabaKiat.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Putyovki.OplataPutyovok
{
    public class OplataDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Create(ref OplataViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblOplata opl = new tblOplata();

                //opl.ID                = model.ID              ;
                opl.Data_Post           = model.Data_Post       ;
                opl.Num_Doc             = model.Num_Doc         ;
                opl.ID_Otras            = model.ID_Otras        ;
                opl.Symma_DT            = model.Symma_DT        ;
                opl.Symma_KT            = model.Symma_KT        ;
                opl.Osnov               = model.Osnov           ;
                opl.ID_KV               = model.ID_KV           ;
                opl.SmFil               = model.SmFil           ;
                opl.Name_org            = model.Name_org        ;
                opl.Tip_Org             = model.Tip_Org         ;
                opl.PrizOpl             = model.PrizOpl         ;
                opl.UserId              = model.UserId          ;
                opl.Departmentbase      = model.Departmentbase  ;

                var opp = db.tblOplatas.Add(opl);

                db.SaveChanges();

                model.ID = opl.ID;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Modify(OplataViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var opl = db.tblOplatas.Where(z => z.ID == model.ID);

                if (opl.Any())
                {
                    var opl1 = opl.FirstOrDefault();

                    if (opl1 != null)
                    {
                        //opl1.ID = model.ID;
                        opl1.Data_Post = model.Data_Post;
                        opl1.Num_Doc = model.Num_Doc;
                        opl1.ID_Otras = model.ID_Otras;
                        opl1.Symma_DT = model.Symma_DT;
                        opl1.Symma_KT = model.Symma_KT;
                        opl1.Osnov = model.Osnov;
                        opl1.ID_KV = model.ID_KV;
                        opl1.SmFil = model.SmFil;
                        opl1.Name_org = model.Name_org;
                        opl1.Tip_Org = model.Tip_Org;
                        opl1.PrizOpl = model.PrizOpl;
                        opl1.UserId = model.UserId;
                        opl1.Departmentbase = model.Departmentbase;
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Remove(OplataViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var opl = db.tblOplatas.Where(z => z.ID == model.ID);

                if (opl.Any())
                {
                    var reg = opl.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblOplatas.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<OplataViewModel> Get(OplataViewModel model)
        {
            List<OplataViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from oplata in db.tblOplatas
                            select new OplataViewModel
                            {
                                ID                  = oplata.ID         ,                 
                                Data_Post           = oplata.Data_Post  ,   
                                Num_Doc             = oplata.Num_Doc    ,   
                                ID_Otras            = oplata.ID_Otras   ,   
                                Symma_DT            = oplata.Symma_DT   ,   
                                Symma_KT            = oplata.Symma_KT   ,   
                                Osnov               = oplata.Osnov      ,   
                                ID_KV               = oplata.ID_KV      ,   
                                SmFil               = oplata.SmFil      ,   
                                Name_org            = oplata.Name_org   ,   
                                Tip_Org             = oplata.Tip_Org    ,   
                                PrizOpl             = oplata.PrizOpl    ,   
                                UserId              = oplata.UserId     ,   
                                Departmentbase      = oplata.Departmentbase
                            };

                list = query.ToList();
            }

            return list;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<OplataViewModel> GetFiltered(OplataViewModel model)
        {
            List<OplataViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from oplata in db.tblOplatas
                            where oplata.Departmentbase == model.Departmentbase
                            && oplata.SmFil == model.SmFil 
                            && oplata.ID_KV == model.ID_KV
                            select new OplataViewModel
                            {
                                ID = oplata.ID,
                                Data_Post = oplata.Data_Post,
                                Num_Doc = oplata.Num_Doc,
                                ID_Otras = oplata.ID_Otras,
                                Symma_DT = oplata.Symma_DT,
                                Symma_KT = oplata.Symma_KT,
                                Osnov = oplata.Osnov,
                                ID_KV = oplata.ID_KV,
                                SmFil = oplata.SmFil,
                                Name_org = oplata.Name_org,
                                Tip_Org = oplata.Tip_Org,
                                PrizOpl = oplata.PrizOpl,
                                UserId = oplata.UserId,
                                Departmentbase = oplata.Departmentbase
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}