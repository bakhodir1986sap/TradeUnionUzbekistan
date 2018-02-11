using KasabaKIAT.Models;
using KurortUrvServiceTest.ServiceInterAction.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurortUrvServiceTest.ServiceInterAction.DataManagers
{
    public class SpisokRaspredeleniyaDataManager
    {
        public static void Add(SpisokRaspredeleniyaViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbku_SpisokRaspredeleniya sr = new tbku_SpisokRaspredeleniya();

                sr.GodZaezdov               = model.GodZaezdov              ;
                sr.SanatoryKod              = model.SanatoryKod             ;
                sr.ProfsoyzKod              = model.ProfsoyzKod             ;
                sr.SanatoryRazmeshenieKod   = model.SanatoryRazmeshenieKod  ;
                sr.Putevok1kv               = model.Putevok1kv              ;
                sr.Putevok2kv               = model.Putevok2kv              ;
                sr.Putevok3kv               = model.Putevok3kv              ;
                sr.Putevok4kv = model.Putevok4kv;

                db.tbku_SpisokRaspredeleniya.Add(sr);

                db.SaveChanges();
            }
        }
    }
}
