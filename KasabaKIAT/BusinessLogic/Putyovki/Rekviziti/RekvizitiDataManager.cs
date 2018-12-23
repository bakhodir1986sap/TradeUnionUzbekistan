using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.Rekviziti
{
    public class RekvizitiDataManager
    {
        public static void Create(RekvizitiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblPred vipl = new tblPred();

                vipl.ID_Prof = 100;
                vipl.ID_Name            = model.ID_Name         ;
                vipl.ID_Pred            = model.ID_Pred         ;
                vipl.ID_Pass            = model.ID_Pass         ;
                vipl.Adress             = model.Adress          ;
                vipl.RS                 = model.RS              ;
                vipl.Name_bank          = model.Name_bank       ;
                vipl.MFO                = model.MFO             ;
                vipl.INN                = model.INN             ;
                vipl.OKONX              = model.OKONX           ;
                vipl.Name_Dir           = model.Name_Dir;
                vipl.Name_Zaveduyushiy  = model.Name_Zaveduyushiy;
                vipl.Name_VedSpes       = model.Name_VedSpes;
                vipl.VIO                = model.VIO             ;
                vipl.Departmentbase     = model.DepartmentBase  ;

                var pst = db.tblPreds.Add(vipl);

                db.SaveChanges();
            }
        }

        public static void Modify(RekvizitiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var vipl = db.tblPreds.Where(z => z.Departmentbase == model.DepartmentBase);

                if (vipl.Any())
                {
                    var vipl1 = vipl.FirstOrDefault();

                    if (vipl1 != null)
                    {
                        vipl1.ID_Name = model.ID_Name;
                        vipl1.ID_Pred = model.ID_Pred;
                        vipl1.ID_Pass = model.ID_Pass;
                        vipl1.Adress = model.Adress;
                        vipl1.RS = model.RS;
                        vipl1.Name_bank = model.Name_bank;
                        vipl1.MFO = model.MFO;
                        vipl1.INN = model.INN;
                        vipl1.OKONX = model.OKONX;
                        vipl1.Name_Dir = model.Name_Dir;
                        vipl1.Name_Zaveduyushiy = model.Name_Zaveduyushiy;
                        vipl1.Name_VedSpes = model.Name_VedSpes;
                        vipl1.VIO = model.VIO;
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
        //public static void Remove(RekvizitiViewModel model)
        //{
        //    using (KasabaKIATEntities db = new KasabaKIATEntities())
        //    {
        //        var vipl = db.tblPred.Where(z => z.ID == model.id);

        //        if (vipl.Any())
        //        {
        //            var reg = vipl.FirstOrDefault();

        //            if (reg != null)
        //            {
        //                db.tblPred.Remove(reg);
        //            }
        //        }

        //        db.SaveChanges();
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<RekvizitiViewModel> Get(RekvizitiViewModel model)
        {
            List<RekvizitiViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from vipl in db.tblPreds
                            where vipl.Departmentbase == model.DepartmentBase
                            select new RekvizitiViewModel
                            {
                                ID_Name         = vipl.ID_Name          ,
                                ID_Pred         = vipl.ID_Pred          ,
                                ID_Pass         = vipl.ID_Pass          ,
                                Adress          = vipl.Adress           ,
                                RS              = vipl.RS               ,
                                Name_bank       = vipl.Name_bank        ,
                                MFO             = vipl.MFO              ,
                                INN             = vipl.INN              ,
                                OKONX           = vipl.OKONX            ,
                                Name_Dir        = vipl.Name_Dir         ,
                                VIO             = vipl.VIO              ,
                                DepartmentBase  = vipl.Departmentbase
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}