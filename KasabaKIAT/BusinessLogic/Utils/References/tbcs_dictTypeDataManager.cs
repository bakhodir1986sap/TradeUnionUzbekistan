using KasabaKIAT.Models;
using System.Collections.Generic;
using System.Linq;

namespace KasabaKIAT.BusinessLogic.Utils.References
{
    public class tbcs_dictTypeDataManager
    {
        public static void Create(ref tbcs_dictTypeViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbcs_dictionarytype os = new tbcs_dictionarytype();

                os.CODE = model.code;
                os.NAME_RU = model.name_ru;
                os.NAME_UZ = model.name_uz;
                os.NAME_LT = model.name_lt;
                os.USER_ID = model.user_id;

                var osIns = db.tbcs_dictionarytype.Add(os);

                db.SaveChanges();

                model.rec_id = os.REC_ID;
            }
        }

        public static void Modify(tbcs_dictTypeViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var os = db.tbcs_dictionarytype.Where(z => z.REC_ID == model.rec_id);

                if (os.Any())
                {
                    var os1 = os.FirstOrDefault();

                    if (os1 != null)
                    {
                        os1.CODE = model.code;
                        os1.NAME_RU = model.name_ru;
                        os1.NAME_UZ = model.name_uz;
                        os1.NAME_LT = model.name_lt;
                        os1.USER_ID = model.user_id;
                    }
                }

                db.SaveChanges();
            }
        }
        public static void Remove(tbcs_dictTypeViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var os = db.tbcs_dictionarytype.Where(z => z.REC_ID == model.rec_id);

                if (os.Any())
                {
                    var os1 = os.FirstOrDefault();

                    if (os1 != null)
                    {
                        db.tbcs_dictionarytype.Remove(os1);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<tbcs_dictTypeViewModel> Get(tbcs_dictTypeViewModel model)
        {
            List<tbcs_dictTypeViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from os in db.tbcs_dictionarytype
                               select new tbcs_dictTypeViewModel
                               {
                                   code = os.CODE,
                                   name_ru = os.NAME_RU,
                                   name_uz = os.NAME_UZ,
                                   name_lt = os.NAME_LT,
                                   user_id = os.USER_ID

                               };

                list = queryRes.ToList();

            }

            return list;
        }
    }
}