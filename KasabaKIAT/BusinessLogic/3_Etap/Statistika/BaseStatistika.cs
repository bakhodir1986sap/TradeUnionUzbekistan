using KasabaKIAT.BusinessLogic._3_Etap.Statistika.Stat_ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._3_Etap.Statistika
{
    public abstract class BaseStatistika
    {
        public BaseStatistika CreateStat(long roleId)
        {
            BaseStatistika obj = null;

            switch (roleId)
            {
                case 6:
                    obj = new Stat_RayonTuman();
                    break;
                case 8:
                    obj = new Stat_PPO();
                    break;
                case 13:
                    obj = new Stat_OblSovProf();
                    break;
                case 14:
                    obj = new Stat_Federasiya();
                    break;
                default:
                    break;
            }

            return obj;
        }

        public abstract IBaseStatViewModel GetData(); 
    }
}