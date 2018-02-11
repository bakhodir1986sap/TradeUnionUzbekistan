using KasabaKIAT.Models;
using KurortUrvServiceTest.ServiceInterAction.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurortUrvServiceTest.ServiceInterAction.DataManagers
{
    public class DocumentSchetFakturaDataManager
    {
        public static void Add(ref DocumentSchetFakturaViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbku_DocumentSchetFaktura docFac = new tbku_DocumentSchetFaktura();

                docFac.DocNomer                 = model.DocNomer                ;
                docFac.DocData                  = model.DocData                 ;
                docFac.DocTypeRealVozv          = model.DocTypeRealVozv         ;
                docFac.ProfsoyzKod              = model.ProfsoyzKod             ;
                docFac.ProfsoyzDogovor          = model.ProfsoyzDogovor         ;
                docFac.DataKvartalaPutevok      = model.DataKvartalaPutevok     ;
                docFac.ProfsoyzDoverennost      = model.ProfsoyzDoverennost     ;
                docFac.ProfsoyzDoverennostData  = model.ProfsoyzDoverennostData ;
                docFac.ProfsoyzDoverennostMOL   = model.ProfsoyzDoverennostMOL  ;
                docFac.SummaDoc                 = model.SummaDoc                ;
                docFac.ProcentNacenki           = model.ProcentNacenki          ;
                docFac.SummaNacenki             = model.SummaNacenki            ;
                docFac.KolichestvoPutevok       = model.KolichestvoPutevok      ;
                docFac.TovarOtpustil            = model.TovarOtpustil            ;

                var resFac = db.tbku_DocumentSchetFaktura.Add(docFac);

                db.SaveChanges();

                model.ID = docFac.ID;
            }

        }

        public static void DeleteAlreadyLoaded(DocumentSchetFakturaViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var list = db.tbku_DocumentSchetFaktura.Where(z => z.DocNomer == model.DocNomer &&
                                                                   z.DocData == model.DocData &&
                                                                   z.DocTypeRealVozv == model.DocTypeRealVozv);

                if (list.Any())
                {
                    foreach(var schf in list)
                    {
                        var detailList = db.tbku_StrokaSchetFakturi.Where(ll => ll.SchetFactura_ID == schf.ID);

                        foreach(var dtl in detailList)
                        {
                            db.tbku_StrokaSchetFakturi.Remove(dtl);
                        }

                        db.tbku_DocumentSchetFaktura.Remove(schf);
                    }
                }

                db.SaveChanges();
            }
        }

    }
}
