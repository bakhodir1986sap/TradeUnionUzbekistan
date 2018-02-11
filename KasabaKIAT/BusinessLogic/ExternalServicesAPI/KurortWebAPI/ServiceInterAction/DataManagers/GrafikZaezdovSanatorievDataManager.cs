using KasabaKIAT.Models;
using KurortUrvServiceTest.ServiceInterAction.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurortUrvServiceTest.ServiceInterAction.DataManagers
{
    public class GrafikZaezdovSanatorievDataManager
    {
        public static void Add(GrafikZaezdovSanatorievViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbku_GrafikZaezdovSanatoriev gz = new tbku_GrafikZaezdovSanatoriev();

                gz.SanatoryKod                      = model.SanatoryKod                     ;
                gz.SanatoryNaimenovanie             = model.SanatoryNaimenovanie            ;

                if (model.SanatoryProfil.Length > 50)
                {
                    gz.SanatoryProfil = model.SanatoryProfil.Substring(0, 50);
                }
                else
                {
                    gz.SanatoryProfil = model.SanatoryProfil;
                }
                
                gz.SanatoryKodOblasti               = model.SanatoryKodOblasti              ;
                gz.SanatoryOblast                   = model.SanatoryOblast                  ;
                gz.SanatoryKoekVsego                = model.SanatoryKoekVsego               ;
                gz.SanatoryKoekLetom                = model.SanatoryKoekLetom               ;
                gz.SanatoryKoekZimoy                = model.SanatoryKoekZimoy               ;
                gz.SanatoryRazmeshenieKod           = model.SanatoryRazmeshenieKod          ;
                gz.SanatoryRazmeshenieNaimenovanie  = model.SanatoryRazmeshenieNaimenovanie ;
                gz.Putevok1kv                       = model.Putevok1kv                      ;
                gz.Putevok2kv                       = model.Putevok2kv                      ;
                gz.Putevok3kv                       = model.Putevok3kv                      ;
                gz.Putevok4kv                       = model.Putevok4kv                      ;
                gz.GodZaezdov                       = model.GodZaezdov;

                db.tbku_GrafikZaezdovSanatoriev.Add(gz);
                try {
                    db.SaveChanges();
                }
                catch(DbEntityValidationException e)
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

        public static void DeleteGod(int godZaezda)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var zaezdList = db.tbku_GrafikZaezdovSanatoriev.Where(z => z.GodZaezdov == godZaezda);

                if (!zaezdList.Any())
                {
                    return;
                }

                foreach(var zaez in zaezdList)
                {
                    db.tbku_GrafikZaezdovSanatoriev.Remove(zaez);
                }

                db.SaveChanges();
            }
        }

    }
}
