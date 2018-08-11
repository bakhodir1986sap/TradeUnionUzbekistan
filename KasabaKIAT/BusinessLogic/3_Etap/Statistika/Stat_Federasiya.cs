using KasabaKIAT.BusinessLogic._3_Etap.Statistika.Stat_ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._3_Etap.Statistika
{
    public class Stat_Federasiya : BaseStatistika
    {
        public override IBaseStatViewModel GetData()
        {
            return new Stat_FederasiyaViewModel();
        }


    }
}