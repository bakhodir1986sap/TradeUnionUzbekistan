using KasabaKIAT.Models;
using KasabaKiat.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.PlanGrafikZaezdov
{
    public class GrafikZaezdovDataManager
    {
        public static void Create(PlanGrafikZaezdovViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbpt_zaezdi zaez = new tbpt_zaezdi();

                zaez.ZAEZD_BEGIN = model.zaezd_begin;
                zaez.ZAEZD_END = model.zaezd_end;
                zaez.PLACE_TYPE_ID = model.place_type_id;
                zaez.PRICE = model.price;
                zaez.QUANTITY = model.quantity;
                zaez.USER_ID = model.user_id;
                //zaez.DEPARTMENT_BASE = model.Department_base;
                zaez.SANATORY_ID = model.sanatory_id;
                zaez.CAMP_ID = model.camp_id;
                //zaez.STATUS = model.status;

                db.tbpt_zaezdi.Add(zaez);

                db.SaveChanges();

            }
        }

        public static void Update(PlanGrafikZaezdovViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var zaezdi = db.tbpt_zaezdi.Where(z => z.REC_ID == model.rec_id);

                if (zaezdi.Any())
                {
                    var zaez = zaezdi.FirstOrDefault();

                    if (zaez != null)
                    {
                        zaez.ZAEZD_BEGIN = model.zaezd_begin;
                        zaez.ZAEZD_END = model.zaezd_end;
                        zaez.PLACE_TYPE_ID = model.place_type_id;
                        zaez.PRICE = model.price;
                        zaez.QUANTITY = model.quantity;
                        zaez.USER_ID = model.user_id;
                        //zaez.DEPARTMENT_BASE = model.Department_base;
                        zaez.SANATORY_ID = model.sanatory_id;
                        zaez.CAMP_ID = model.camp_id;
                        //zaez.STATUS = model.status;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(PlanGrafikZaezdovViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var zaezdi = db.tbpt_zaezdi.Where(z => z.REC_ID == model.rec_id);

                if (zaezdi.Any())
                {
                    var reg = zaezdi.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tbpt_zaezdi.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<PlanGrafikZaezdovViewModel> Get(PlanGrafikZaezdovViewModel model)
        {
            List<PlanGrafikZaezdovViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from zaezdi in db.tbpt_zaezdi
                            where zaezdi.SANATORY_ID == model.sanatory_id
                            select new PlanGrafikZaezdovViewModel
                            {
                                zaezd_begin         = zaezdi.ZAEZD_BEGIN,
                                zaezd_end           = zaezdi.ZAEZD_END    ,   
                                place_type_id       = zaezdi.PLACE_TYPE_ID  , 
                                place_type_name     = zaezdi.tbcs_dictionary.NAME_UZ,
                                price               = zaezdi.PRICE          , 
                                quantity            = zaezdi.QUANTITY      ,  
                                user_id             = zaezdi.USER_ID         ,
                                //Department_base     = zaezdi.DEPARTMENT_BASE ,
                                sanatory_id         = zaezdi.SANATORY_ID     ,
                                camp_id             = zaezdi.CAMP_ID         ,
                                //status              = zaezdi.STATUS          ,
                                sanatory_name       = zaezdi.t_sanatorium.sanatorium_name
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}