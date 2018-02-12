using KasabaKIAT.Models;
using KasabaKiat.ViewModel.Putyovki.GodovoyPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.GodovoyPlan.Federasiya
{
    public class GdovoyPlanFedDataManager
    {
        public static void Create(ref GdovoyPlanFedViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbpt_yearlyplanregion reg = new tbpt_yearlyplanregion();

                reg.YEAR = model.year;
                reg.KVARTAL = model.kvartal;
                reg.SANATORIUM_ID = model.sanatorium_id;
                reg.CAMP_ID = model.camp_id;
                reg.TIP_MEST_ID = model.tip_mest_id;
                reg.PRICE = model.price;
                reg.QUANTITY = model.quantity;
                reg.STATUS = "0"; //Just Save
                reg.USER_ID = model.user_id;
                reg.REGION_ID = model.region_id;
                reg.MODIFY_DATE = DateTime.Now;
                reg.DEPARTMENT_BASE = model.department_base;

                var planOtrasl = db.tbpt_yearlyplanregion.Add(reg);

                if (planOtrasl != null)
                {
                    model.rec_id = planOtrasl.REC_ID;
                }

                db.SaveChanges();
            }
        }

        public static void Modify(GdovoyPlanFedViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbpt_yearlyplanregion.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.YEAR = model.year;
                        reg.KVARTAL = model.kvartal;
                        reg.SANATORIUM_ID = model.sanatorium_id;
                        reg.CAMP_ID = model.camp_id;
                        reg.TIP_MEST_ID = model.tip_mest_id;
                        reg.PRICE = model.price;
                        reg.QUANTITY = model.quantity;
                        //reg.STATUS = "0"; //Just Save
                        //reg.USER_ID = model.user_id;
                        reg.REGION_ID = model.region_id;
                        reg.MODIFY_DATE = DateTime.Now;
                        //reg.DEPARTMENT_BASE = model.department_base;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(GdovoyPlanFedViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbpt_yearlyplanregion.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tbpt_yearlyplanregion.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<GdovoyPlanFedViewModel> Get(GdovoyPlanFedViewModel model)
        {
            List<GdovoyPlanFedViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from godPlan in db.tbpt_yearlyplanregion
                            join region in db.sp_soato on godPlan.REGION_ID equals region.code
                            where godPlan.YEAR == model.year && 
                            godPlan.KVARTAL == model.kvartal &&
                            (godPlan.SANATORIUM_ID == (model.sanatorium_id != null ? model.sanatorium_id : godPlan.SANATORIUM_ID)) &&
                            (godPlan.REGION_ID == (string.IsNullOrEmpty(model.region_id) ? godPlan.REGION_ID : model.region_id))
                            select new GdovoyPlanFedViewModel
                            {
                                rec_id = godPlan.REC_ID,
                                year = godPlan.YEAR,
                                kvartal = godPlan.KVARTAL,
                                sanatorium_id = godPlan.SANATORIUM_ID,
                                sanatorium_name = godPlan.t_sanatorium.sanatorium_name,
                                tip_mest_id = godPlan.TIP_MEST_ID,
                                tip_mest_name = godPlan.tbcs_dictionary.NAME_UZ,
                                price = godPlan.PRICE,
                                quantity = godPlan.QUANTITY,
                                region_id = godPlan.REGION_ID,
                                region_name = region.name_uz
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}