using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KasabaKIAT.BusinessLogic._3_Etap.Statistika.Stat_ViewModels;

namespace KasabaKIAT.BusinessLogic._3_Etap.Statistika
{
    public class Stat_OblSovProf : BaseStatistika
    {
        public override IBaseStatViewModel GetData()
        {
            return new Stat_OblSovProfViewModel();
        }
    }
}