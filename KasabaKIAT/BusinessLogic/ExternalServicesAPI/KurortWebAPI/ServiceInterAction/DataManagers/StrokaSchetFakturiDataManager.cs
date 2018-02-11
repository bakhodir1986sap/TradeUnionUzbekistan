using KasabaKIAT.Models;
using KurortUrvServiceTest.ServiceInterAction.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurortUrvServiceTest.ServiceInterAction.DataManagers
{
    public class StrokaSchetFakturiDataManager
    {
        public static void Add(StrokaSchetFakturiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbku_StrokaSchetFakturi sfak = new tbku_StrokaSchetFakturi();

                sfak.SchetFactura_ID                    = model.SchetFactura_ID                 ;
                sfak.SanatoryKod                        = model.SanatoryKod                     ;
                sfak.SanatoryNaimenovanie               = model.SanatoryNaimenovanie            ;
                sfak.SanatoryRazmeshenieKod             = model.SanatoryRazmeshenieKod          ;
                sfak.SanatoryRazmeshenieNaimenovanie    = model.SanatoryRazmeshenieNaimenovanie ;
                sfak.SanatoryType                       = model.SanatoryType                    ;
                sfak.DataZaezda                         = model.DataZaezda                      ;
                sfak.Blank1                             = model.Blank1                          ;
                sfak.Blank2                             = model.Blank2                          ;
                sfak.Kolichestvo                        = model.Kolichestvo                     ;
                sfak.Sebestoimost                       = model.Sebestoimost                    ;
                sfak.SummaSebestoimosti                 = model.SummaSebestoimosti              ;
                sfak.ProcentNacenki                     = model.ProcentNacenki                  ;
                sfak.Cena                               = model.Cena                            ;
                sfak.Summa = model.Summa;

                db.tbku_StrokaSchetFakturi.Add(sfak);

                db.SaveChanges();
            }
        }
    }
}
