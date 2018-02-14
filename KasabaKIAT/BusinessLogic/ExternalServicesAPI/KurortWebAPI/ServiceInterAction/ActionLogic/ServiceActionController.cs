using KurortUrvServiceTest.ServiceInterAction.DataManagers;
using KurortUrvServiceTest.ServiceInterAction.ServiceCalls;
using KurortUrvServiceTest.ServiceInterAction.ViewModel;
using KurortUrvServiceTest.ServiceInterAction.Xml;
using System;
using System.Collections.Generic;

namespace KurortUrvServiceTest.ServiceInterAction.ActionLogic
{
    public class ServiceActionController
    {
        private KurortService _service;

        /// <summary>
        /// Construktor
        /// </summary>
        public ServiceActionController()
        {
            var login = KasabaKIAT.Properties.Settings.Default.Login;
            var password = KasabaKIAT.Properties.Settings.Default.Password;

            _service = new KurortService();
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadSpiskaProfsoyzov()
        {
            //Journal Enter
            var journal =  EnterJournal(1);

            //Call Service

            var spisokProf = _service.GetSpiskaProfsoyzov();

            if (!string.IsNullOrEmpty(spisokProf.KodOshibki))
            {
                journal.ErrorCode = int.Parse(spisokProf.KodOshibki);
            }

            journal.ErrorDesc = spisokProf.OpisanieOshibki;

            if (journal.ErrorCode != 0)
            {
                journal.State = 2; //Error
            }
            else
            {
                //Delete All Old Data
                StrokaProfsoyzaDataManager.DeleteAllPravsoyuzData();

                journal.State = 1; //Success
            }

            //Update Error Codes
            JournalDataManager.Modify(journal);

            //If OK insert data to DB
            if (spisokProf.StrokiProfsoyzov != null)
            {
                foreach (var prof in spisokProf.StrokiProfsoyzov)
                {
                    var model = new StrokaProfsoyzaViewModel();

                    model.ProfsoyzKod = prof.ProfsoyzKod;
                    model.ProfsoyzNaimenovanie = prof.ProfsoyzNaimenovanie;

                    StrokaProfsoyzaDataManager.AddPravsoyuzData(model);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GodZaezdov"></param>
        /// <param name="SanatoryKod"></param>
        public void LoadZaezdovSanatoriev(string GodZaezdov, string SanatoryKod = "")
        {
            int _godzaezdov = 0;

            if (string.IsNullOrEmpty(GodZaezdov))
            {
                throw new ApplicationException("GodZaezdov is empty");
            }
            else
            {

                if (!int.TryParse(GodZaezdov, out _godzaezdov))
                {
                    throw new ApplicationException("GodZaezdov is invalid");
                }
            }

            var journal = EnterJournal(2);

            var resList = _service.GetZaezdovSanatoriev(GodZaezdov , SanatoryKod);

            if (!string.IsNullOrEmpty(resList.KodOshibki))
            {
                journal.ErrorCode = int.Parse(resList.KodOshibki);
            }

            journal.ErrorDesc = resList.OpisanieOshibki;

            if (journal.ErrorCode != 0)
            {
                journal.State = 2; //Error
            }
            else
            {
                //Delete All Old Data
                GrafikZaezdovSanatorievDataManager.DeleteGod(_godzaezdov);

                journal.State = 1; //Success
            }

            //Update Error Codes
            JournalDataManager.Modify(journal);

            //If OK insert data to DB
            if (resList.StrokiGrafikaZaezdovSanatoriev != null)
            {
                foreach (var res in resList.StrokiGrafikaZaezdovSanatoriev)
                {
                    var gzmodel = new GrafikZaezdovSanatorievViewModel();

                    gzmodel.SanatoryKod = res.SanatoryKod;
                    gzmodel.SanatoryNaimenovanie = res.SanatoryNaimenovanie;
                    gzmodel.SanatoryProfil = res.SanatoryProfil;
                    gzmodel.SanatoryKodOblasti = res.SanatoryKodOblasti;
                    gzmodel.SanatoryOblast = res.SanatoryOblast;
                    gzmodel.SanatoryKoekVsego = int.Parse(res.SanatoryKoekVsego);
                    gzmodel.SanatoryKoekLetom = int.Parse(res.SanatoryKoekLetom);
                    gzmodel.SanatoryKoekZimoy = int.Parse(res.SanatoryKoekZimoy);
                    gzmodel.SanatoryRazmeshenieKod = res.SanatoryRazmeshenieKod;
                    gzmodel.SanatoryRazmeshenieNaimenovanie = res.SanatoryRazmeshenieNaimenovanie;
                    gzmodel.Putevok1kv = int.Parse(res.Putevok1kv);
                    gzmodel.Putevok2kv = int.Parse(res.Putevok2kv);
                    gzmodel.Putevok3kv = int.Parse(res.Putevok3kv);
                    gzmodel.Putevok4kv = int.Parse(res.Putevok4kv);
                    gzmodel.GodZaezdov = _godzaezdov;

                    GrafikZaezdovSanatorievDataManager.Add(gzmodel);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="raspredeleniye"></param>
        public void SetZagruzkaRaspredeleniyaZaezdov(TypeRaspredelenieZaezdov raspredeleniye)
        {
            if (raspredeleniye == null)
            {
                throw new ApplicationException("null object reference 1");
            }

            if (raspredeleniye.StrokiRaspredelenyaZaezdov == null)
            {
                throw new ApplicationException("null object reference 2");
            }

            if (raspredeleniye.StrokiRaspredelenyaZaezdov.Count == 0)
            {
                throw new ApplicationException("StrokiRaspredelenyaZaezdov objects list is 0");
            }

            //Prepare Data for transfer
            var zController = new ZaezdController();

            var xmlRes = zController.Process(raspredeleniye);

            //Journal Enter
            var journal = EnterJournal(3);


            var setRes = _service.SetRaspredeleniyaZaezdov(xmlRes);

            if (!string.IsNullOrEmpty(setRes))
            {
                journal.ErrorCode = -1;
            }

            journal.ErrorDesc = setRes;

            if (journal.ErrorCode != 0)
            {
                journal.State = 2; //Error
            }
            else
            {
                journal.State = 1; //Success
            }

            //Update Error Codes
            JournalDataManager.Modify(journal);

            //Save sant data
            foreach(var rasp in raspredeleniye.StrokiRaspredelenyaZaezdov)
            {
                var model = new SpisokRaspredeleniyaViewModel();

                model.GodZaezdov = int.Parse(raspredeleniye.GodZaezdov);
                model.SanatoryKod = rasp.SanatoryKod;
                model.ProfsoyzKod = rasp.ProfsoyzKod;
                model.SanatoryRazmeshenieKod = rasp.SanatoryRazmeshenieKod;
                model.Putevok1kv = int.Parse(rasp.Putevok1kv);
                model.Putevok2kv = int.Parse(rasp.Putevok2kv);
                model.Putevok3kv = int.Parse(rasp.Putevok3kv);
                model.Putevok4kv = int.Parse(rasp.Putevok4kv);

                SpisokRaspredeleniyaDataManager.Add(model);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PeriodNachalo"></param>
        /// <param name="PeriodOkonchanie"></param>
        /// <param name="ProfsoyzKod"></param>
        /// <param name="SanatoryType"></param>
        public void LoadSchetFactura(DateTime   PeriodNachalo,
                                     DateTime   PeriodOkonchanie,
                                     string     ProfsoyzKod = "",
                                     string     SanatoryType = "")
        {
            if (PeriodNachalo > PeriodOkonchanie)
            {
                throw new ApplicationException("PeriodNachalo is bigger than  PeriodOkonchanie");
            }

            //Journal Enter
            var journal = EnterJournal(4);

            var resList = _service.GetSchetFactura(PeriodNachalo, PeriodOkonchanie, ProfsoyzKod);

            if (!string.IsNullOrEmpty(resList.KodOshibki))
            {
                journal.ErrorCode = int.Parse(resList.KodOshibki);
            }

            journal.ErrorDesc = resList.OpisanieOshibki;

            if (journal.ErrorCode != 0)
            {
                journal.State = 2; //Error
            }
            else
            {
                //Delete All Old Data
                //GrafikZaezdovSanatorievDataManager.DeleteGod(_godzaezdov);

                journal.State = 1; //Success
            }

            //Update Error Codes
            JournalDataManager.Modify(journal);
            if (resList.StrokiDocumentov == null)
                return;
            foreach (var resTop in resList.StrokiDocumentov)
            {
                DocumentSchetFakturaViewModel model = new DocumentSchetFakturaViewModel();

                model.DocNomer = resTop.DocNomer;
                model.DocData = resTop.DocData;
                model.DocTypeRealVozv = resTop.DocTypeRealVozv;
                model.ProfsoyzKod = resTop.ProfsoyzKod;
                model.ProfsoyzDogovor = resTop.ProfsoyzDogovor;
                model.DataKvartalaPutevok = resTop.DataKvartalaPutevok;
                model.ProfsoyzDoverennost = resTop.ProfsoyzDoverennost;
                model.ProfsoyzDoverennostData = resTop.ProfsoyzDoverennostData;
                model.ProfsoyzDoverennostMOL = resTop.ProfsoyzDoverennostMOL;
                model.SummaDoc = resTop.SummaDoc;
                model.ProcentNacenki = resTop.ProcentNacenki;
                model.SummaNacenki = resTop.SummaNacenki;
                model.KolichestvoPutevok = (int)resTop.KolichestvoPutevok;
                model.TovarOtpustil = resTop.TovarOtpustil;

                DocumentSchetFakturaDataManager.DeleteAlreadyLoaded(model);

                DocumentSchetFakturaDataManager.Add(ref model);

                foreach (var doc in resTop.StrokiSchetaFakturi)
                {
                    StrokaSchetFakturiViewModel dtlModel = new StrokaSchetFakturiViewModel();

                    dtlModel.SchetFactura_ID = model.ID;
                    dtlModel.SanatoryKod = doc.SanatoryKod;
                    dtlModel.SanatoryNaimenovanie = doc.SanatoryNaimenovanie;
                    dtlModel.SanatoryRazmeshenieKod = doc.SanatoryRazmeshenieKod;
                    dtlModel.SanatoryRazmeshenieNaimenovanie = doc.SanatoryRazmeshenieNaimenovanie;
                    dtlModel.SanatoryType = doc.SanatoryType;
                    dtlModel.DataZaezda = doc.DataZaezda;
                    dtlModel.Blank1 = doc.Blank1;
                    dtlModel.Blank2 = doc.Blank2;
                    dtlModel.Kolichestvo = (int)doc.Kolichestvo;
                    dtlModel.Sebestoimost = doc.Sebestoimost;
                    dtlModel.SummaSebestoimosti = doc.SummaSebestoimosti;
                    dtlModel.ProcentNacenki = doc.ProcentNacenki;
                    dtlModel.Cena = doc.Cena;

                    StrokaSchetFakturiDataManager.Add(dtlModel);
                }

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sanatoryCode"></param>
        /// <param name="PeriodNachalo"></param>
        /// <param name="PeriodOkonchanie"></param>
        public void LoadZaezdyClientov(string sanatoryCode,
                                      DateTime PeriodNachalo,
                                      DateTime PeriodOkonchanie)
        {
            if (PeriodNachalo > PeriodOkonchanie)
            {
                throw new ApplicationException("PeriodNachalo is bigger than  PeriodOkonchanie");
            }

            if (string.IsNullOrEmpty(sanatoryCode))
            {
                throw new ApplicationException("sanatoryCode is invalid");
            }

            //Journal Enter
            var journal = EnterJournal(5);

            var resList = _service.GetZaezdovClientov(sanatoryCode ,PeriodNachalo, PeriodOkonchanie);

            if (!string.IsNullOrEmpty(resList.KodOshibki))
            {
                journal.ErrorCode = int.Parse(resList.KodOshibki);
            }

            journal.ErrorDesc = resList.OpisanieOshibki;

            if (journal.ErrorCode != 0)
            {
                journal.State = 2; //Error
            }
            else
            {
                //Delete All Old Data
                //GrafikZaezdovSanatorievDataManager.DeleteGod(_godzaezdov);

                journal.State = 1; //Success
            }

            //Update Error Codes
            JournalDataManager.Modify(journal);

            if (resList.StrokiZaezdovClientov == null) return;

            foreach (var client in resList.StrokiZaezdovClientov)
            {
                ZaezdyClientovViewModel model = new ZaezdyClientovViewModel();

                model.SanatoryKod = client.SanatoryKod;
                model.SanatoryNaimenovanie = client.SanatoryNaimenovanie;
                model.NomerPutevki = client.NomerPutevki;
                model.DataZaezdaPlan = client.DataZaezdaPlan;
                model.DataZaezdaFakt = client.DataZaezdaFakt;
                model.StoimostPutevki = client.StoimostPutevki;
                model.OplachenaStoimost = client.OplachenaStoimost;
                model.ClientFIO = client.ClientFIO;
                model.ClientPasport = client.ClientPasport;

                ZaezdyClientovDataManager.Delete(model);

                ZaezdyClientovDataManager.Add(model);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        private JournalViewModel EnterJournal(int serviceId)
        {
            //Journal Enter
            JournalViewModel journal = new JournalViewModel();

            journal.ServiceID = serviceId;
            journal.CreateDT = DateTime.Now;
            journal.UserID = 1; //CurrentUser
            journal.DepartmentBase = "173";
            journal.State = 0; //New

            JournalDataManager.Add(ref journal);

            return journal;
        }


    }
}
