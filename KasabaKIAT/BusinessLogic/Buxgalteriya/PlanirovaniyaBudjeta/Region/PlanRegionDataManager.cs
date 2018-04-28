using KasabaKIAT.BusinessLogic.Buxgalteriya.PlanirovaniyaBudjeta.Region;
using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Buxgalteriya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Buxgalteriya.PlanirovaniyaBudjeta.Region
{
    public class PlanRegionDataManager
    {
        public static void Create(ref PlanRegionViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbbu_planbyudjetregion reg = new tbbu_planbyudjetregion();

               reg.DEPARTMENT_BASE  = model.department_base ;
               reg.USERID           = model.userid          ;
               reg.YEAR             = model.year            ;
               reg.SOATO_CODE       = model.soato_code      ;
               reg.ITEM_CODE        = model.item_code       ;
               reg.ITEM_NAME        = model.item_name       ;
               reg.GODOVOY          = model.godovoy         ;
               reg.QUARTER1         = model.quarter1        ;
               reg.HALFYEAR         = model.halfyear        ;
               reg.MONTH9           = model.month9          ;
        
                var planOtrasl = db.tbbu_planbyudjetregion.Add(reg);

                db.SaveChanges();

                model.rec_id = reg.REC_ID;
            }
        }


        public static bool FillNotExisting(string soato_code, int year, string department_code, long UserId)
        {
            bool isFilled = false;

            var ii = new ItemController();
            ii.InitializeItems();

            foreach (var model in ii.Items)
            {
                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    var planOblast = db.tbbu_planbyudjetregion.Where(z => z.SOATO_CODE == soato_code && z.YEAR == year && z.ITEM_CODE == model.ItemCode);

                    if (planOblast.Any()) continue;

                    tbbu_planbyudjetregion reg = new tbbu_planbyudjetregion();

                    reg.DEPARTMENT_BASE = department_code;
                    reg.USERID = UserId;
                    reg.YEAR = year;
                    reg.SOATO_CODE = soato_code;
                    reg.ITEM_CODE = model.ItemCode;
                    reg.ITEM_NAME = model.ItemDescription;
                    reg.GODOVOY = model.ValueYearly;
                    reg.QUARTER1 = model.Value1Quater;
                    reg.HALFYEAR = model.Value1Half;
                    reg.MONTH9 = model.Value9Month;

                    var planOtrasl = db.tbbu_planbyudjetregion.Add(reg);

                    db.SaveChanges();

                    isFilled = true;
                }
            }

            return isFilled;
        }

        public static bool Modify(PlanRegionViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbbu_planbyudjetregion.Where(z => z.SOATO_CODE == model.soato_code && z.YEAR == model.year && z.ITEM_CODE == model.item_code);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        //reg.DEPARTMENT_BASE = model.department_base;
                        //reg.USERID = model.userid;
                        //reg.YEAR = model.year;
                        //reg.SOATO_CODE = model.soato_code;
                        //reg.ITEM_CODE = model.item_code;
                        //reg.ITEM_NAME = model.item_name;
                        reg.GODOVOY = model.godovoy.HasValue ? model.godovoy: reg.GODOVOY;
                        reg.QUARTER1 = model.quarter1.HasValue ? model.quarter1 : reg.QUARTER1;
                        reg.HALFYEAR = model.halfyear.HasValue ? model.halfyear : reg.HALFYEAR;
                        reg.MONTH9 = model.month9.HasValue ? model.month9: reg.MONTH9;
                    }
                }
                else
                {
                    return false;
                }

                db.SaveChanges();

                
            }

            return true;
        }

        public static void Remove(string soato_code, int year)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbbu_planbyudjetregion.Where(z => z.SOATO_CODE == soato_code && z.YEAR == year);

                if (planOblast.Any())
                {
                    foreach(var mm in planOblast)
                    {
                        db.tbbu_planbyudjetregion.Remove(mm);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<PlanRegionViewModel> Get(PlanRegionViewModel model)
        {
            List<PlanRegionViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tbbu_planbyudjetregion
                            where bank.DEPARTMENT_BASE == model.department_base
                            select new PlanRegionViewModel
                            {
                                rec_id           = bank.REC_ID          ,
                                department_base  = bank.DEPARTMENT_BASE ,
                                userid           = bank.USERID          ,
                                year             = bank.YEAR            ,
                                soato_code       = bank.SOATO_CODE      ,
                                item_code        = bank.ITEM_CODE       ,
                                item_name        = bank.ITEM_NAME       ,
                                godovoy          = bank.GODOVOY         ,
                                quarter1         = bank.QUARTER1        ,
                                halfyear         = bank.HALFYEAR        ,
                                month9           = bank.MONTH9         
                            };

                list = query.ToList();
            }

            return list;
        }


        public static List<PlanRegionViewModel> Get(string soato_code, int year)
        {
            List<PlanRegionViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tbbu_planbyudjetregion
                            where ((bank.YEAR == year) && (bank.SOATO_CODE == soato_code)) 
                            select new PlanRegionViewModel
                            {
                                rec_id = bank.REC_ID,
                                department_base = bank.DEPARTMENT_BASE,
                                userid = bank.USERID,
                                year = bank.YEAR,
                                soato_code = bank.SOATO_CODE,
                                item_code = bank.ITEM_CODE,
                                item_name = bank.ITEM_NAME,
                                godovoy = bank.GODOVOY,
                                quarter1 = bank.QUARTER1,
                                halfyear = bank.HALFYEAR,
                                month9 = bank.MONTH9
                            };

                list = query.ToList();
            }

            return list;
        }

        public static List<int> GetYears()
        {
            List<int> yearsList = new List<int>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var list = db.tbbu_planbyudjetregion.GroupBy(p => p.YEAR).Select(g=>new { Year = g.Key });

                foreach(var year in list)
                {
                    yearsList.Add(year.Year.Value);
                }
            }

            return yearsList;
        }

        public static List<YearAndRegion> GetRegionByYears(int year)
        {
            List<YearAndRegion> regionsList = new List<YearAndRegion>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var list = db.tbbu_planbyudjetregion.Where(p=>p.YEAR == year).
                    GroupBy(p => p.SOATO_CODE).
                    Select(g => new { SoatoCode = g.Key });

                foreach(var region in list)
                {
                    var obj = new YearAndRegion();

                    var RegionName = db.tbku_StrokaProfsoyza.Where(s => s.MATCH_DEPARTMENTBASE == region.SoatoCode);
                    
                    if (RegionName.Any())
                    {
                        obj.Year = year;
                        obj.SoatoCode = region.SoatoCode;
                        obj.RegionName = RegionName.First().ProfsoyzNaimenovanie;

                        regionsList.Add(obj);
                    } 
                }
            }

            return regionsList;
        }
    }
}