using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Buxgalteriya;
using System.Collections.Generic;
using System.Linq;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.Platejki
{
    public class PlatejkiDataManager
    {
        public static void Create(ref PlatejkiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbbu_platejka reg = new tbbu_platejka();

                reg.DEPARTMENT_CODE     = model.department_code ;  
                reg.USERID              = model.userid          ;  
                reg.PLAT_DATE           = model.plat_date       ;  
                reg.PLAT_NUM            = model.plat_num        ;  
                reg.PLATELNAME          = model.platelname      ;  
                reg.PLATELINN           = model.platelinn       ;  
                reg.PLATELRASCHET       = model.platelraschet   ;  
                reg.PLATELBANKNAME      = model.platelbankname  ;  
                reg.PLATELBANKMFO       = model.platelbankmfo   ;  
                reg.POLUCHNAME          = model.poluchname      ;  
                reg.POLUCHINN           = model.poluchinn       ;  
                reg.POLUCHRASCHET       = model.poluchraschet   ;  
                reg.POLUCHBANKNAME      = model.poluchbankname  ;  
                reg.POLUCHBANKMFO       = model.poluchbankmfo   ;  
                reg.SUMMA               = model.summa           ;  
                reg.COMMENT             = model.comment         ;  
                reg.RUKOVODITEL         = model.rukovoditel     ;
                reg.GLAVBUX             = model.glavbux;  

                var planOtrasl = db.tbbu_platejka.Add(reg);

                if (planOtrasl != null)
                {
                    model.rec_id = planOtrasl.REC_ID;
                }

                db.SaveChanges();
            }
        }

        public static void Modify(PlatejkiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbbu_platejka.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.DEPARTMENT_CODE = model.department_code;
                        reg.USERID = model.userid;
                        reg.PLAT_DATE = model.plat_date;
                        reg.PLAT_NUM = model.plat_num;
                        reg.PLATELNAME = model.platelname;
                        reg.PLATELINN = model.platelinn;
                        reg.PLATELRASCHET = model.platelraschet;
                        reg.PLATELBANKNAME = model.platelbankname;
                        reg.PLATELBANKMFO = model.platelbankmfo;
                        reg.POLUCHNAME = model.poluchname;
                        reg.POLUCHINN = model.poluchinn;
                        reg.POLUCHRASCHET = model.poluchraschet;
                        reg.POLUCHBANKNAME = model.poluchbankname;
                        reg.POLUCHBANKMFO = model.poluchbankmfo;
                        reg.SUMMA = model.summa;
                        reg.COMMENT = model.comment;
                        reg.RUKOVODITEL = model.rukovoditel;
                        reg.GLAVBUX = model.glavbux;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(PlatejkiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbbu_platejka.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tbbu_platejka.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<PlatejkiViewModel> Get(PlatejkiViewModel model)
        {
            List<PlatejkiViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tbbu_platejka
                            where bank.DEPARTMENT_CODE == model.department_code
                            select new PlatejkiViewModel
                            {
                                department_code     = bank.DEPARTMENT_CODE,
                                userid              = bank.USERID         , 
                                plat_date           = bank.PLAT_DATE      , 
                                plat_num            = bank.PLAT_NUM       , 
                                platelname          = bank.PLATELNAME     , 
                                platelinn           = bank.PLATELINN      , 
                                platelraschet       = bank.PLATELRASCHET  , 
                                platelbankname      = bank.PLATELBANKNAME , 
                                platelbankmfo       = bank.PLATELBANKMFO  , 
                                poluchname          = bank.POLUCHNAME     , 
                                poluchinn           = bank.POLUCHINN      , 
                                poluchraschet       = bank.POLUCHRASCHET  , 
                                poluchbankname      = bank.POLUCHBANKNAME , 
                                poluchbankmfo       = bank.POLUCHBANKMFO  , 
                                summa               = bank.SUMMA          , 
                                comment             = bank.COMMENT        , 
                                rukovoditel         = bank.RUKOVODITEL    ,
                                glavbux             = bank.GLAVBUX
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}