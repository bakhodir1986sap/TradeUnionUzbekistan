using KasabaKIAT.Models;
using KurortUrvServiceTest.ServiceInterAction.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurortUrvServiceTest.ServiceInterAction.DataManagers
{
    public class ZaezdyClientovDataManager
    {
        public static void Add(ZaezdyClientovViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbku_ZaezdyClientov zcl = new tbku_ZaezdyClientov();

                zcl.SanatoryKod             = model.SanatoryKod         ;
                zcl.SanatoryNaimenovanie    = model.SanatoryNaimenovanie;
                zcl.NomerPutevki            = model.NomerPutevki        ;
                zcl.DataZaezdaPlan          = model.DataZaezdaPlan      ;
                zcl.DataZaezdaFakt          = model.DataZaezdaFakt      ;
                zcl.StoimostPutevki         = model.StoimostPutevki     ;
                zcl.OplachenaStoimost       = model.OplachenaStoimost   ;
                zcl.ClientFIO               = model.ClientFIO           ;
                zcl.ClientPasport           = model.ClientPasport;

                db.tbku_ZaezdyClientov.Add(zcl);

                db.SaveChanges();
            }
        }

        public static void Delete(ZaezdyClientovViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {

                var rList = db.tbku_ZaezdyClientov.Where(z => z.SanatoryKod == model.SanatoryKod &&
                                                            z.NomerPutevki == model.NomerPutevki);

                if (rList.Any())
                {
                    foreach(var tt in rList)
                    {
                        db.tbku_ZaezdyClientov.Remove(tt);
                    }
                }

                db.SaveChanges();
            }
        }
    }
}
