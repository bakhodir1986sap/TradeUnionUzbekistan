using KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok;
using KasabaKiat.ViewModel;
using KasabaKiat.ViewModel.Putyovki;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                            && docSchetfak.ProfsoyzKod == profsoyuzKod
                            select docSchetfak;

                if (query.Any())
                {
                    foreach(var docSchetfak in query)
                    {

                        var doc_Date = docSchetfak.DocData.HasValue ? docSchetfak.DocData.Value.Date : 
                            docSchetfak.DocData;

                        var query1 = from tblpost in db.tblPost
                                     where tblpost.Num_Doc == docSchetfak.DocNomer
                                     && tblpost.Data_Post == doc_Date
                                     select tblpost;

                        if (query1.Any())
                        {
                            continue;
                        }
                        else
                        {
                            //Main Insert
                            var prixMain = new PostupleniyaViewModel();

                            prixMain.Id_Kv = null;
                            prixMain.Data_Post = doc_Date;
                            prixMain.Num_Doc = docSchetfak.DocNomer;
                            prixMain.ID_Pos = 2; //Po dannim Kurortnoe Upravleniye
                            prixMain.Num_Kol = docSchetfak.KolichestvoPutevok;
                            prixMain.Symma = docSchetfak.SummaDoc;

                            var totalRes = string.Format("{0} {1} {2} {3}",
                                docSchetfak.ProfsoyzDoverennost, docSchetfak.ProfsoyzDoverennostData,
                                docSchetfak.ProfsoyzDoverennostMOL, docSchetfak.ProfsoyzDogovor);

                            if (totalRes.Length > 50)
                            {
                                prixMain.Name_Dov = totalRes.Substring(49);
                            }
                            else
                            {
                                prixMain.Name_Dov = totalRes;
                            }

                            prixMain.Vid = false;
                            prixMain.SmFil = null;
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

                            PostMainDataManager.Create(ref prixMain);

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
                                            }
                                            else
                                            {
                                                postPut.IdSanat = db.tblSprav_San.First().ID;
                                            }

                                            postPut.Num_Put = j.ToString();
                                            postPut.DataSan = prixDetail.DataZaezda;
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

                                            PostPutDetailDataManager.Create(ref postPut);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}