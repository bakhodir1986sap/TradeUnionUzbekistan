using KasabaKIAT.BusinessLogic._3_Etap.Statistika.Stat_ViewModels;

namespace KasabaKIAT.BusinessLogic._3_Etap.Statistika
{
    public abstract class BaseStatistika
    {
        public static BaseStatistika CreateStat(long roleId)
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
                    obj = new Stat_Federasiya();
                    break;
            }

            return obj;
        }

        public abstract IBaseStatViewModel GetData(string depCode, int? cur_year, int? cur_kv); 
    }
}