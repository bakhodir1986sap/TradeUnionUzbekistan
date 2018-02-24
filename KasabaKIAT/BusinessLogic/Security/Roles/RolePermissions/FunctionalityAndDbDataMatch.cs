using KasabaKiat.Helpers;
using KasabaKiat.ViewModel;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Security.Roles.RolePermissions
{
    public class FunctionalityAndDbDataMatch
    {
        public static FunctionalityList MatchProcess(UserViewModel user)
        {
            var rolFumc = new FunctionalityList();

            rolFumc.UserId = user.userid;

            if (!user.role_id.HasValue)
            {
                throw new ApplicationException("Invalid rol id");
            }

            rolFumc.RoleId = user.role_id.Value;
            rolFumc.UserName = user.username;


            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var rolPermitL = db.role_permission.Where(p => p.role_id == rolFumc.RoleId && p.action.Contains("Asp.") );

                if (rolPermitL.Any())
                {
                    foreach(var rolperm in rolPermitL)
                    {
                        
                        if (rolperm.action.Contains("Asp.GrafikZaezdov")) { rolFumc.GrafikZaezdov = true; }
                        if (rolperm.action.Contains("Asp.GodovoyPlanPutyovok")) { rolFumc.GodovoyPlanPutyovok = true; }
                        if (rolperm.action.Contains("Asp.PrixodPutyovok")) { rolFumc.PrixodPutyovok = true; }
                        if (rolperm.action.Contains("Asp.ProsmotrOstatkaPutyovok")) { rolFumc.ProsmotrOstatkaPutyovok = true; }
                        if (rolperm.action.Contains("Asp.IzmeneniyaPutyovok")) { rolFumc.IzmeneniyaPutyovok = true; }
                        if (rolperm.action.Contains("Asp.RealizasiyaPutyovok")) { rolFumc.RealizasiyaPutyovok = true; }
                        if (rolperm.action.Contains("Asp.ProsmotrRealPutyovok")) { rolFumc.ProsmotrRealPutyovok = true; }
                        if (rolperm.action.Contains("Asp.IzmeneniyaDetaleyRealizasiiPutyovok")) { rolFumc.IzmeneniyaDetaleyRealizasiiPutyovok = true; }
                        if (rolperm.action.Contains("Asp.OformleniyaVozvrataPutyovok")) { rolFumc.OformleniyaVozvrataPutyovok = true; }
                        if (rolperm.action.Contains("Asp.SverkaDannixPoOtdixayushim")) { rolFumc.SverkaDannixPoOtdixayushim = true; }
                        if (rolperm.action.Contains("Asp.VvodZayavok")) { rolFumc.VvodZayavok = true; }
                        if (rolperm.action.Contains("Asp.PoiskIRedaktirovaniyaZayavok")) { rolFumc.PoiskIRedaktirovaniyaZayavok = true; }
                        if (rolperm.action.Contains("Asp.RassmotreniyaZayavok")) { rolFumc.RassmotreniyaZayavok = true; }
                        if (rolperm.action.Contains("Asp.BuxUchetVvodDannix")) { rolFumc.BuxUchetVvodDannix = true; }
                        if (rolperm.action.Contains("Asp.PrasmotrDannix")) { rolFumc.PrasmotrDannix = true; }
                        if (rolperm.action.Contains("Asp.RuchniyeProvodki")) { rolFumc.RuchniyeProvodki = true; }
                        if (rolperm.action.Contains("Asp.PlatejniyePorucheniya")) { rolFumc.PlatejniyePorucheniya = true; }
                        if (rolperm.action.Contains("Asp.PlanByudjeta")) { rolFumc.PlanByudjeta = true; }
                        if (rolperm.action.Contains("Asp.RaznoskaDannixPoOtraslyam")) { rolFumc.RaznoskaDannixPoOtraslyam = true; }
                        if (rolperm.action.Contains("Asp.SvodniyOtchetPoZayavkam")) { rolFumc.SvodniyOtchetPoZayavkam = true; }
                        if (rolperm.action.Contains("Asp.Talabnomalar")) { rolFumc.Talabnomalar = true; }
                        if (rolperm.action.Contains("Asp.FormaN13")) { rolFumc.FormaN13 = true; }
                        if (rolperm.action.Contains("Asp.FormaN14")) { rolFumc.FormaN14 = true; }
                        if (rolperm.action.Contains("Asp.TablisaPoUchetuOtdixayushix")) { rolFumc.TablisaPoUchetuOtdixayushix = true; }
                        if (rolperm.action.Contains("Asp.SvodniyOtchetPoNakladnim")) { rolFumc.SvodniyOtchetPoNakladnim = true; }
                        if (rolperm.action.Contains("Asp.GlavnayaKniga")) { rolFumc.GlavnayaKniga = true; }
                        if (rolperm.action.Contains("Asp.SvodNachisleniyPoOtraslyam")) { rolFumc.SvodNachisleniyPoOtraslyam = true; }
                        if (rolperm.action.Contains("Asp.JurnalniyOrder1")) { rolFumc.JurnalniyOrder1 = true; }
                        if (rolperm.action.Contains("Asp.JurnalniyOrder2")) { rolFumc.JurnalniyOrder2 = true; }
                        if (rolperm.action.Contains("Asp.JurnalniyOrder3")) { rolFumc.JurnalniyOrder3 = true; }
                        if (rolperm.action.Contains("Asp.JurnalniyOrder4")) { rolFumc.JurnalniyOrder4 = true; }
                        if (rolperm.action.Contains("Asp.JurnalniyOrder5")) { rolFumc.JurnalniyOrder5 = true; }
                        if (rolperm.action.Contains("Asp.JurnalniyOrder6")) { rolFumc.JurnalniyOrder6 = true; }
                        if (rolperm.action.Contains("Asp.JurnalniyOrder7")) { rolFumc.JurnalniyOrder7 = true; }
                        if (rolperm.action.Contains("Asp.JurnalniyOrder8")) { rolFumc.JurnalniyOrder8 = true; }
                        if (rolperm.action.Contains("Asp.FinansoviyKvartalniy")) { rolFumc.FinansoviyKvartalniy = true; }
                        if (rolperm.action.Contains("Asp.FinansoviyGodovoy")) { rolFumc.FinansoviyGodovoy = true; }
                        if (rolperm.action.Contains("Asp.FinansoviyPoLageryam")) { rolFumc.FinansoviyPoLageryam = true; }
                        if (rolperm.action.Contains("Asp.PoSanatoriyamProfilaktoriyam")) { rolFumc.PoSanatoriyamProfilaktoriyam = true; }
                        if (rolperm.action.Contains("Asp.DvijeniyaSredstvPoSchetam")) { rolFumc.DvijeniyaSredstvPoSchetam = true; }
                        if (rolperm.action.Contains("Asp.AktSverki")) { rolFumc.AktSverki = true; }
                        if (rolperm.action.Contains("Asp.ZadoljennostSFP")) { rolFumc.ZadoljennostSFP = true; }
                        if (rolperm.action.Contains("Asp.SozdaniyaUchetnixZapisey")) { rolFumc.SozdaniyaUchetnixZapisey = true; }
                        if (rolperm.action.Contains("Asp.SozdaniyaRoley")) { rolFumc.SozdaniyaRoley = true; }
                        if (rolperm.action.Contains("Asp.SozdaniyaProvodok")) { rolFumc.SozdaniyaProvodok = true; }
                        if (rolperm.action.Contains("Asp.PrivyazkaProvodokkJO")) { rolFumc.PrivyazkaProvodokkJO = true; }
                        if (rolperm.action.Contains("Asp.UpravleniyaSpravochnikami")) { rolFumc.UpravleniyaSpravochnikami = true; }

                    }
                }
                else
                {
                   //// throw new ApplicationException("");
                }

                
            }

            return rolFumc;
        }

        public static FunctionalityList MatchProcess(long userId)
        {
            var userinf = UserInfo.GetUserInfo(userId);

            return MatchProcess(userinf);
        }

        public static FunctionalityList MatchProcess(string userName)
        {
            var userinf = UserInfo.GetUserInfo(userName);

            return MatchProcess(userinf);
        }
    }
}