using KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok;
using KasabaKiat.BusinessLogic.Utils.References;
using KasabaKiat.ViewModel;
using KasabaKiat.ViewModel.Putyovki;
using KasabaKIAT.Models;
using System;
using System.Linq;


namespace KasabaKIAT.BusinessLogic.Putyovki.PrixodPutyovok
{
    public class Oprixodovaniye
    {
        public void PrixodKurort(DateTime   periodNachalo, 
                                 DateTime   periodOkonchaniye, 
                                 string     profsoyuzKod,
                                 long       userId,
                                 string     departmentBase)
        {
            DateTime aperiodNachalo = periodNachalo;
            TimeSpan ts1 = new TimeSpan(00, 00, 01);
            aperiodNachalo = aperiodNachalo.Date + ts1;

            DateTime aperiodOkonchaniye = periodOkonchaniye;
            TimeSpan ts = new TimeSpan(23, 59, 59);
            aperiodOkonchaniye = aperiodOkonchaniye.Date + ts;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from docSchetfak in db.tbku_DocumentSchetFaktura
                            where docSchetfak.DocData >= aperiodNachalo && docSchetfak.DocData <= aperiodOkonchaniye
                            && docSchetfak.ProfsoyzKod == profsoyuzKod && docSchetfak.DocTypeRealVozv != "В" 
                            select docSchetfak;

                if (query.Any())
                {
                    foreach(var docSchetfak in query.ToList())
                    {

                        var doc_Date = docSchetfak.DocData.HasValue ? docSchetfak.DocData.Value.Date : 
                            docSchetfak.DocData;

                        var query1 = from tblpost in db.tblPost
                                     where tblpost.Num_Doc == docSchetfak.DocNomer
                                     && tblpost.Data_Post == doc_Date
                                     select tblpost;

                        //Proverka chto uje oprixodavanno
                        if (query1.Any())
                        {
                            continue;
                        }
                        else
                        {
                            //Main Insert
                            var prixMain = new PostupleniyaViewModel();

                            if (doc_Date.HasValue)
                            {
                                prixMain.Id_Kv = (doc_Date.Value.Month + 2) / 3;
                            }
                            else
                            {
                                prixMain.Id_Kv = 1; //By Default
                            }
                            
                            prixMain.Data_Post = doc_Date;
                            prixMain.Num_Doc = docSchetfak.DocNomer;
                            prixMain.ID_Pos = 2; //Po dannim Kurortnoe Upravleniye
                            prixMain.Num_Kol = docSchetfak.KolichestvoPutevok;
                            prixMain.Symma = docSchetfak.SummaDoc;

                            var totalStr = string.Format("{0} {1} {2} {3}",
                                docSchetfak.ProfsoyzDoverennost, docSchetfak.ProfsoyzDoverennostData,
                                docSchetfak.ProfsoyzDoverennostMOL, docSchetfak.ProfsoyzDogovor);

                            if (!string.IsNullOrEmpty(totalStr))
                            {
                                if (totalStr.Length > 50)
                                {
                                    prixMain.Name_Dov = totalStr.Substring(49);
                                }
                                else
                                {
                                    prixMain.Name_Dov = totalStr;
                                }
                            }
                            else
                            {
                                prixMain.Name_Dov = string.Empty;
                            }

                            prixMain.Vid = false;
                            prixMain.SmFil = doc_Date.Value.Year;
                            prixMain.UserId = userId;

                            var query11 = from prof in db.tbku_StrokaProfsoyza
                                          where prof.ProfsoyzKod == docSchetfak.ProfsoyzKod
                                          select prof;


                            if (query11.Any())
                            {
                                prixMain.DepartmentBase = query11.FirstOrDefault().MATCH_DEPARTMENTBASE;
                            }
                            else
                            {
                                prixMain.DepartmentBase = departmentBase;
                            }

                            tblPost pp = null;

                            PostMainDataManager.Create(ref prixMain, db, out pp);

                            //Details Insert

                            var query2 = from prixDetail in db.tbku_StrokaSchetFakturi
                                         where prixDetail.SchetFactura_ID == docSchetfak.ID
                                         select prixDetail;

                            if (query2.Any())
                            {
                                foreach(var prixDetail in query2)
                                {
                                    uint beginNumber = 0;
                                    uint endNumber = 0;

                                    if (uint.TryParse(prixDetail.Blank1 , out beginNumber) &&
                                        uint.TryParse(prixDetail.Blank2, out endNumber))
                                    {
                                        for (uint j = beginNumber; j <= endNumber; j++)
                                        {
                                            var postPut = new PostPutyovkaViewModel();

                                            postPut.IdPost = prixMain.Id;

                                            int sanatId = 0;

                                            if (int.TryParse(prixDetail.SanatoryKod, out sanatId))
                                            {
                                                var query21 = from sanat in db.tblSprav_San
                                                              where sanat.KurortSanID == sanatId
                                                              select sanat;

                                                if (query21.Any())
                                                {
                                                    postPut.IdSanat = query21.First().ID;
                                                }
                                                else
                                                {
                                                    var sabViewModel = new Sprav_SanViewModel();

                                                    sabViewModel.Name_San = prixDetail.SanatoryNaimenovanie;
                                                    sabViewModel.Num_Kod = prixDetail.SanatoryKod;
                                                    sabViewModel.KurortSanId = sanatId;

                                                    Sprav_San.Create(ref sabViewModel);

                                                    postPut.IdSanat = sabViewModel.ID;
                                                }
                                            }
                                            else
                                            {
                                                postPut.IdSanat = db.tblSprav_San.First().ID;
                                            }

                                            postPut.Num_Put = j.ToString().PadLeft(6, '0');

                                            if ((DateTime.MinValue >= prixDetail.DataZaezda) 
                                                || (DateTime.MaxValue <= prixDetail.DataZaezda))
                                            {
                                                postPut.DataSan = null;
                                            }
                                            else
                                            {
                                                postPut.DataSan = prixDetail.DataZaezda;
                                            }

                                            
                                            postPut.Symma_Put = prixDetail.Cena;
                                            postPut.ID_PolVoz = false;
                                            postPut.Data_Post = DateTime.Now.Date;
                                            postPut.UserId = userId;

                                            if (query11.Any())
                                            {
                                                postPut.Departmentbase = query11.FirstOrDefault().MATCH_DEPARTMENTBASE;
                                            }
                                            else
                                            {
                                                postPut.Departmentbase = departmentBase;
                                            }

                                            PostPutDetailDataManager.Create(ref postPut, db, pp);
                                        }
                                    }
                                }
                            }
                        }

                        db.SaveChanges();
                    }
                }
            }
        }
    }
}