using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Viloyats.TashVil.Chastichka
{
    public class Oplata
    {
        public void Process()
        {
            using (KasabaKIAT_N1Entities db = new KasabaKIAT_N1Entities())
            {
                var oplataMs = db.tbl_mig_oplata.Where(z => z.POLYA019 != null);

                if (oplataMs.Any())
                {
                    foreach(var oplata in oplataMs)
                    {
                        decimal Symma = decimal.Parse(oplata.POLYA017);

                        var oplatass = db.tblOplata_Ras.Where(s => s.Num_Put == oplata.POLYA012 && s.Symma == Symma);

                        if (oplatass.Any())
                        {
                            var Putovka = oplatass.First();

                            var OplMain = Putovka.tblOplata;

                            var opllMain = new tblOplata();

                            opllMain.Departmentbase = "17227";
                            opllMain.SmFil = 2018;
                            opllMain.ID_KV = 1;
                            opllMain.UserId = 1;
                            opllMain.ID_Otras = OplMain.ID_Otras;
                            opllMain.Num_Doc = OplMain.Num_Doc;
                            opllMain.Data_Post = OplMain.Data_Post;
                            opllMain.Tip_Org = OplMain.Tip_Org;
                            opllMain.PrizOpl = OplMain.PrizOpl;

                            decimal Symma19 = decimal.Parse(oplata.POLYA019);

                            opllMain.Symma_KT = Symma19;
                            opllMain.Symma_DT = 0;

                            var resOplMain = db.tblOplata.Add(opllMain);

                            var opllPut = new tblOplata_Ras();

                            opllPut.tblOplata = opllMain;
                            opllPut.Departmentbase = "17227";
                            opllPut.UserId = 1;
                            opllPut.ID_Sanat = Putovka.ID_Sanat;
                            opllPut.Num_Put = Putovka.Num_Put;
                            opllPut.Symma = Symma19;
                            opllPut.ID_Pr = 1;
                            opllPut.Priz = 1;

                            db.tblOplata_Ras.Add(opllPut);
                        }
                    }

                    db.SaveChanges();
                }
            }
        }
    }
}
