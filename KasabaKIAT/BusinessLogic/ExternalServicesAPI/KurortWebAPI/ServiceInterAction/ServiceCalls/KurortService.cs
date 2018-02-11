using KasabaKIAT.KurortUrlService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KurortUrvServiceTest.ServiceInterAction.ServiceCalls
{
    /// <summary>
    /// Service Interaction
    /// </summary>
    public class KurortService
    {
        private SkuTradeUnionExchg service;
        public KurortService(string login = "", string password = "")
        {
            service = new SkuTradeUnionExchg();
            service.PreAuthenticate = true;

            if (string.IsNullOrEmpty(login))
            {
                login = "SovetFederatsii";
            }

            if (string.IsNullOrEmpty(password))
            {
                password = "20sKu18";
            }

            service.Credentials = new NetworkCredential(login, password);
        }

        public typeSpisokProfsoyzov GetSpiskaProfsoyzov()
        {
            if (service == null) return null;

            var spisokProf = service.ZaprosSpiskaProfsoyzov();

            return spisokProf;
        }

        public typeGrafikZaezdovSanatoriev GetZaezdovSanatoriev(string GodZaezdov, string SanatoryKod="")
        {
            if (service == null) return null;

            var zaezdiSan = service.ZaprosZaezdovSanatoriev(GodZaezdov, SanatoryKod);

            return zaezdiSan;
        }

        public string SetRaspredeleniyaZaezdov(string xmlstr)
        {
            if (service == null) return null;

            string retCode = string.Empty;

            retCode = service.ZagruzkaRaspredeleniyaZaezdov(xmlstr);

            return retCode;
        }

        public typeSpisokSchetFaktur GetSchetFactura(DateTime PeriodNachalo,
                                                    DateTime PeriodOkonchanie,
                                                    string ProfsoyzKod = "",
                                                    string SanatoryType= "")
        {
            if (service == null) return null;

            var schetfactura = service.ZaprosSpiskaSchetFaktur(PeriodNachalo, 
                                                                PeriodOkonchanie, 
                                                                ProfsoyzKod, 
                                                                SanatoryType);

            return schetfactura;
        }


        public typeZaezdyClientov GetZaezdovClientov(string SanatoryKod,
                                                    DateTime PeriodNachalo,
                                                    DateTime PeriodOkonchanie)
        {
            if (service == null) return null;

            var zaezdovclientov = service.ZaprosZaezdovClientov(SanatoryKod, PeriodNachalo, PeriodOkonchanie);

            return zaezdovclientov;
        }
    }
}
