using KasabaKIAT.Models;
using KasabaKiat.ViewModel.Putyovki.RaznoskaPutyovok;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Putyovki.RaznoskaPoPutyovki
{
    public class Face_MedDataManager
    {
        public static void Create(ref Face_MedViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblFace_Med fm = new tblFace_Med();

                fm.ID                 = db.tblFace_Med.Max(z => z.ID) + 1; ;
                fm.ID_Face            = model.id_face       ;  
                fm.Med_Doc            = model.med_doc       ;  
                fm.Med_Data           = model.med_data      ;  
                fm.Med_San            = model.med_san       ;  
                fm.Opl_Doc            = model.opl_doc       ;  
                fm.Opl_Data           = model.opl_data      ;  
                fm.Opl_Symma          = model.opl_symma     ;  
                fm.Tal_Doc            = model.tal_doc       ;  
                fm.Tal_Data           = model.tal_data      ;  
                fm.UserId             = model.userid        ;
                fm.Departmentbase     = model.departmentbase;

                var fmm = db.tblFace_Med.Add(fm);

                model.id = fmm.ID;

                db.SaveChanges();
            }
        }

        public static void Update(Face_MedViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var fm = db.tblFace_Med.Where(z => z.ID == model.id);

                if (fm.Any())
                {
                    var fm1 = fm.FirstOrDefault();

                    if (fm1 != null)
                    {
                        //fm1.ID_Face = model.id_face;
                        fm1.Med_Doc = model.med_doc;
                        fm1.Med_Data = model.med_data;
                        fm1.Med_San = model.med_san;
                        fm1.Opl_Doc = model.opl_doc;
                        fm1.Opl_Data = model.opl_data;
                        fm1.Opl_Symma = model.opl_symma;
                        fm1.Tal_Doc = model.tal_doc;
                        fm1.Tal_Data = model.tal_data;
                        fm1.UserId = model.userid;
                        fm1.Departmentbase = model.departmentbase;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(Face_MedViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var post = db.tblFace_Med.Where(z => z.ID == model.id);

                if (post.Any())
                {
                    var reg = post.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblFace_Med.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<Face_MedViewModel> Get(Face_MedViewModel model)
        {
            List<Face_MedViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from faceMed in db.tblFace_Med
                            where faceMed.ID_Face == model.id_face
                            select new Face_MedViewModel
                            {
                                id              = faceMed.ID             ,
                                id_face         = faceMed.ID_Face        ,
                                med_doc         = faceMed.Med_Doc        ,
                                med_data        = faceMed.Med_Data       ,
                                med_san         = faceMed.Med_San        ,
                                opl_doc         = faceMed.Opl_Doc        ,
                                opl_data        = faceMed.Opl_Data       ,
                                opl_symma       = faceMed.Opl_Symma      ,
                                tal_doc         = faceMed.Tal_Doc        ,
                                tal_data        = faceMed.Tal_Data       ,
                                userid          = faceMed.UserId         ,
                                departmentbase  = faceMed.Departmentbase
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}