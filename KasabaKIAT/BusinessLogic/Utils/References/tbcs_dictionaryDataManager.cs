using KasabaKIAT.Models;
using System.Collections.Generic;
using System.Linq;

namespace KasabaKIAT.BusinessLogic.Utils.References
{
    public class tbcs_dictionaryDataManager
    {
        public static void Create(ref tbcs_dictionaryViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbcs_dictionary os = new tbcs_dictionary();

                os.CODE               = model.code                 ;
                os.NAME_RU            = model.name_ru              ;
                os.NAME_UZ            = model.name_uz              ;
                os.NAME_LT            = model.name_lt              ;
                os.DICTIONARY_TYPE_ID = model.dictionary_type_id   ;
                os.STATUS             = model.status               ;
                os.USER_ID            = model.user_id              ;

                var osIns = db.tbcs_dictionary.Add(os);

                db.SaveChanges();

                model.rec_id = os.REC_ID;
            }
        }

        public static void Modify(tbcs_dictionaryViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var os = db.tbcs_dictionary.Where(z => z.REC_ID == model.rec_id);

                if (os.Any())
                {
                    var os1 = os.FirstOrDefault();

                    if (os1 != null)
                    {

                        os1.CODE = model.code;
                        os1.NAME_RU = model.name_ru;
                        os1.NAME_UZ = model.name_uz;
                        os1.NAME_LT = model.name_lt;
                        os1.DICTIONARY_TYPE_ID = model.dictionary_type_id;
                        os1.STATUS = model.status;
                        os1.USER_ID = model.user_id;
                    }
                }

                db.SaveChanges();
            }
        }
        public static void Remove(tbcs_dictionaryViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var os = db.tbcs_dictionary.Where(z => z.REC_ID == model.rec_id);

                if (os.Any())
                {
                    var os1 = os.FirstOrDefault();

                    if (os1 != null)
                    {
                        db.tbcs_dictionary.Remove(os1);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<tbcs_dictionaryViewModel> Get(string dictTypeName)
        {
            List<tbcs_dictionaryViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from os in db.tbcs_dictionary
                               join dictType in db.tbcs_dictionarytype on os.DICTIONARY_TYPE_ID equals dictType.REC_ID
                               where dictType.CODE == dictTypeName
                               select new tbcs_dictionaryViewModel
                               {
                                   //rec_id               = os.REC_ID,
                                   code                 = os.CODE,
                                   name_ru              = os.NAME_RU         
                                   //name_uz              = os.NAME_UZ  ,         
                                   //name_lt              = os.NAME_LT  ,         
                                   //dictionary_type_id   = os.DICTIONARY_TYPE_ID,
                                   //status               = os.STATUS            ,
                                   //user_id              = os.USER_ID           

                               };

                list = queryRes.ToList();

            }

            return list;
        }
    }
}