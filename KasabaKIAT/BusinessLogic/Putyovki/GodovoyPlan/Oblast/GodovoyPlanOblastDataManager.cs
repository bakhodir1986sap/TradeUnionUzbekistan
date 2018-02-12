using KasabaKIAT.Models;
using KasabaKiat.ViewModel.Putyovki.GodovoyPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.GodovoyPlan.Oblast
{
    public class GodovoyPlanOblastDataManager
    {
        public static void Create(ref GodovoyPlanOblastViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbpt_yearlyplanotrasl reg = new tbpt_yearlyplanotrasl();

                reg.YEAR = model.year;
                reg.KVARTAL = model.kvartal;
                reg.SANATORIUM_ID = model.sanatorium_id;
                reg.CAMP_ID = model.camp_id;
                reg.PRICE = model.price;
                reg.QUANTITY = model.quantity;
                reg.STATUS = "0"; //Just Save
                reg.USER_ID = model.user_id;
                reg.REGION_ID = model.region_id;
                reg.MODIFY_DATE = DateTime.Now;
                reg.DEPARTMANT_BASE = model.departmant_base;

                var planOtrasl = db.tbpt_yearlyplanotrasl.Add(reg);

                if (planOtrasl != null)
                {
                    model.rec_id = planOtrasl.REC_ID;
                }

                db.SaveChanges();
            }
        }

        public static void Modify(GodovoyPlanOblastViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbpt_yearlyplanotrasl.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.YEAR = model.year;
                        reg.KVARTAL = model.kvartal;
                        reg.SANATORIUM_ID = model.sanatorium_id;
                        reg.CAMP_ID = model.camp_id;
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

        public static void Remove(GodovoyPlanOblastViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbpt_yearlyplanotrasl.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tbpt_yearlyplanotrasl.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<GodovoyPlanOblastViewModel> Get(GodovoyPlanOblastViewModel model)
        {
            List<GodovoyPlanOblastViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from godPlan in db.tbpt_yearlyplanotrasl
                            join region in db.sp_soato on godPlan.REGION_ID equals region.code
                            join branch in db.sp_branch on godPlan.OTRASL_ID equals branch.id
                            where godPlan.YEAR == model.year &&
                            godPlan.KVARTAL == model.kvartal &&
                            (godPlan.SANATORIUM_ID == (model.sanatorium_id != null ? model.sanatorium_id : godPlan.SANATORIUM_ID)) &&
                            (godPlan.REGION_ID == (string.IsNullOrEmpty(model.region_id) ? godPlan.REGION_ID : model.region_id)) &&
                            (godPlan.OTRASL_ID == (model.otrasl_id != null ? model.otrasl_id : godPlan.OTRASL_ID))
                            select new GodovoyPlanOblastViewModel
                            {
                                rec_id = godPlan.REC_ID,
                                year = godPlan.YEAR,
                                kvartal = godPlan.KVARTAL,
                                sanatorium_id = godPlan.SANATORIUM_ID,
                                sanatorium_name = godPlan.t_sanatorium.sanatorium_name,
                                price = godPlan.PRICE,
                                quantity = godPlan.QUANTITY,
                                region_id = godPlan.REGION_ID,
                                region_name = region.name_uz,
                                otrasl_id = godPlan.OTRASL_ID,
                                otrasl_name = branch.name_ru
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}