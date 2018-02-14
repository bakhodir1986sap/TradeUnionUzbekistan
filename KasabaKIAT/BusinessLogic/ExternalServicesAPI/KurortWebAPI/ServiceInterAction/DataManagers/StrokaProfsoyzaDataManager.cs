using KasabaKIAT.Models;
using KurortUrvServiceTest.ServiceInterAction.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KurortUrvServiceTest.ServiceInterAction.DataManagers
{
    public class StrokaProfsoyzaDataManager
    {
        public static void AddPravsoyuzData(StrokaProfsoyzaViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbku_StrokaProfsoyza pr = new tbku_StrokaProfsoyza();

                pr.ProfsoyzKod = model.ProfsoyzKod;
                pr.ProfsoyzNaimenovanie = model.ProfsoyzNaimenovanie;
                pr.CreateDT = DateTime.Now;

                db.tbku_StrokaProfsoyza.Add(pr);

                db.SaveChanges();
            }
        }

        public static void DeleteAllPravsoyuzData()
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                foreach(var prof in db.tbku_StrokaProfsoyza)
                {
                    db.tbku_StrokaProfsoyza.Remove(prof);
                }

                db.SaveChanges();
            }
        }


        public static void AddPravsoyuzDataN(StrokaProfsoyzaViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var pf = db.tbku_StrokaProfsoyza.FirstOrDefault(z => z.ProfsoyzKod == model.ProfsoyzKod);

                if (pf != null) return;


                tbku_StrokaProfsoyza pr = new tbku_StrokaProfsoyza();

                pr.ProfsoyzKod = model.ProfsoyzKod;
                pr.ProfsoyzNaimenovanie = model.ProfsoyzNaimenovanie;
                pr.CreateDT = DateTime.Now;

                db.tbku_StrokaProfsoyza.Add(pr);

                db.SaveChanges();
            }
        }
    }
}
