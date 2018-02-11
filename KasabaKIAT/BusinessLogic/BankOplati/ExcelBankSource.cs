using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KasabaKIAT.ViewModel;
using KasabaKIAT.BusinessLogic.BankOplati.OraslAndRegionDefine;

namespace KasabaKIAT.BusinessLogic.BankOplati
{
    public class ExcelBankSource : BankDataSource
    {
        private IPersonalizeFirm personalization;

        private List<BankOplatViewModel> list { get; set; }

        public ExcelBankSource()
        {
            personalization = new KiatPersonalization();

            list = new List<BankOplatViewModel>();
        }

        public string FileName { get; set; }

        private void ReadFromExcelFile()
        {
            //Read file and 

            //Fill List<BankOplatViewModel> list 
        }

        public override List<BankOplatViewModel> Process()
        {
            ReadFromExcelFile();

            foreach(var oplata in list)
            {
                oplata.Otrasl = personalization.FindOtraslByAccount(oplata.RaschetniySchet);
                oplata.Okonx = personalization.FindOtraslByInn(oplata.Inn);

                oplata.Region = personalization.FindRegionByAccount(oplata.RaschetniySchet);
                oplata.Soato = personalization.FindRegionByInn(oplata.Inn);
            }

            return list;
        }
    }
}