using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Security.Roles.RolePermissions
{
    public class FunctionalityList
    {
        public long UserId { get; set; }

        public long RoleId { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }

        #region Putyovki

        public bool GrafikZaezdov                       { get; set; }

        public bool GodovoyPlanPutyovok                 { get; set; }

        public bool PrixodPutyovok                      { get; set; }

        public bool ProsmotrOstatkaPutyovok             { get; set; }

        public bool IzmeneniyaPutyovok                  { get; set; }

        public bool RealizasiyaPutyovok                 { get; set; }

        public bool ProsmotrRealPutyovok                { get; set; }

        public bool IzmeneniyaDetaleyRealizasiiPutyovok { get; set; }

        public bool OformleniyaVozvrataPutyovok         { get; set; }

        public bool SverkaDannixPoOtdixayushim          { get; set; }

        public bool VvodZayavok                         { get; set; }

        public bool PoiskIRedaktirovaniyaZayavok        { get; set; }

        public bool RassmotreniyaZayavok                { get; set; }

        #endregion

        #region BuxUchet

        public bool BuxUchetVvodDannix                  { get; set; }

        public bool PrasmotrDannix              { get; set; }

        public bool RuchniyeProvodki                    { get; set; }

        public bool PlatejniyePorucheniya               { get; set; }

        public bool PlanByudjeta                        { get; set; }

        public bool RaznoskaDannixPoOtraslyam           { get; set; }

        #endregion

        #region Reports

        public bool SvodniyOtchetPoZayavkam             { get; set; }

        public bool Talabnomalar                        { get; set; }

        public bool FormaN13                            { get; set; }

        public bool FormaN14                            { get; set; }

        public bool TablisaPoUchetuOtdixayushix         { get; set; }

        public bool SvodniyOtchetPoNakladnim            { get; set; }

        public bool GlavnayaKniga                       { get; set; }//Главная книга

        public bool SvodNachisleniyPoOtraslyam          { get; set; }//Свод начислений по отраслям

        public bool JurnalniyOrder1                     { get; set; }

        public bool JurnalniyOrder2                     { get; set; }

        public bool JurnalniyOrder3                     { get; set; }

        public bool JurnalniyOrder4                     { get; set; }

        public bool JurnalniyOrder5                     { get; set; }

        public bool JurnalniyOrder6                     { get; set; }

        public bool JurnalniyOrder7                     { get; set; }

        public bool JurnalniyOrder8                     { get; set; }

        public bool FinansoviyKvartalniy                { get; set; }//Финансовый квартальный

        public bool FinansoviyGodovoy                   { get; set; }//Финансовый годовой

        public bool FinansoviyPoLageryam                { get; set; } //Финансовый по лагерям

        public bool PoSanatoriyamProfilaktoriyam        { get; set; }//По санаториям-профилакториям

        public bool DvijeniyaSredstvPoSchetam           { get; set; }//Движение средств по счетам

        public bool AktSverki                           { get; set; } //Акт сверки

        public bool ZadoljennostSFP                     { get; set; }//Задолженность СФП

        #endregion

        #region Administrirovaniya

        public bool SozdaniyaUchetnixZapisey            { get; set; }//Создание учетных записей

        public bool SozdaniyaRoley                      { get; set; }//Создание ролей

        public bool SozdaniyaProvodok                   { get; set; }//Создание проводок

        public bool PrivyazkaProvodokkJO                { get; set; }//Привязка проводок к ЖО

        public bool UpravleniyaSpravochnikami           { get; set; }//Управление справочниками
        #endregion
    }
}