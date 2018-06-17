using KasabaKiat.BusinessLogic.Putyovki.RaznoskaPoPutyovki;
using KasabaKiat.ViewModel.Putyovki.RaznoskaPutyovok;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Viloyats.TashVil.DanniyeVladelsev
{
    public class DanniyaVlad
    {
        public void Process()
        {
            using (KasabaKIAT_N1Entities db = new KasabaKIAT_N1Entities())
            {
                var DanVlads = db.tbl_mig_danniyeVlad.ToList();

                if (DanVlads.Any())
                {
                    tblFase fface = null;

                    foreach(var vlad in DanVlads)
                    {
                        decimal Symma = decimal.Parse(vlad.POLYA014);

                        Console.WriteLine(vlad.POLYA016);

                        var rasxPut = db.tblVipl_Put.Where(z => z.Num_Put == vlad.POLYA013 && z.Symma == Symma);

                        if (rasxPut.Any())
                        {
                            var putyovka = rasxPut.First();

                            var face = new FaceViewModel();

                            face.id_put = putyovka.ID;
                            face.id_sanat = putyovka.ID_Sanat;
                            face.num_put = putyovka.Num_Put;

                            string[] names = vlad.POLYA016.Split(' ');

                            if (names.Length > 0)
                            {
                                face.lastname = names[0];

                                if (names.Length > 1)
                                {
                                    face.name = names[1];

                                    if (names.Length > 2)
                                    {
                                        face.firstname = names[2];
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(vlad.POLYA017))
                            {
                                if (vlad.POLYA017.Length > 50)
                                {
                                    face.dolj = vlad.POLYA017.Substring(0, 50);
                                }
                                else
                                {
                                    face.dolj = vlad.POLYA017;
                                }
                            }

                            if (!string.IsNullOrEmpty(vlad.POLYA018))
                            {
                                if (vlad.POLYA018.Length > 300)
                                {
                                    face.adress = vlad.POLYA018.Substring(0, 300);
                                }
                                else
                                {
                                    face.adress = vlad.POLYA018;
                                }
                            }

                            FaceDataManager.Create(ref face, db, out fface);
                        }
                    }

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
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
        }
    }
}
