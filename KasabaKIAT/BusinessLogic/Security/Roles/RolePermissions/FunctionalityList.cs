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

        public bool TotalPutyovki { get; private set; }

        private bool _grafikZaezdov                         { get; set; }
        private bool _godovoyPlanPutyovok                   { get; set; }
        private bool _prixodPutyovok                        { get; set; }
        private bool _prosmotrOstatkaPutyovok               { get; set; }
        private bool _izmeneniyaPutyovok                    { get; set; }
        private bool _realizasiyaPutyovok                   { get; set; }
        private bool _prosmotrRealPutyovok                  { get; set; }
        private bool _izmeneniyaDetaleyRealizasiiPutyovok   { get; set; }
        private bool _oformleniyaVozvrataPutyovok           { get; set; }
        private bool _sverkaDannixPoOtdixayushim            { get; set; }

        private bool _rassmotreniyaZayavok                  { get; set; }

        public bool GrafikZaezdov
        {
            get
            {
                return _grafikZaezdov;
            }

            set
            {
                _grafikZaezdov = value;

                if (_grafikZaezdov)
                {
                    TotalPutyovki = true;
                }
            }
        }

        public bool GodovoyPlanPutyovok
        {
            get
            {
                return _godovoyPlanPutyovok;
            }
            set
            {
                _godovoyPlanPutyovok = value;

                if (_godovoyPlanPutyovok)
                { TotalPutyovki = true; }
            }

        }

        public bool PrixodPutyovok
        {
            get
            {
                return _prixodPutyovok;
            } set{_prixodPutyovok                     = value;  if (_prixodPutyovok                     )  {TotalPutyovki = true;} }}
        public bool ProsmotrOstatkaPutyovok             { get{ return _prosmotrOstatkaPutyovok            ;}set{_prosmotrOstatkaPutyovok            = value;  if (_prosmotrOstatkaPutyovok            )  {TotalPutyovki = true;}}}
        public bool IzmeneniyaPutyovok                  { get{ return _izmeneniyaPutyovok                 ;}set{_izmeneniyaPutyovok                 = value;  if (_izmeneniyaPutyovok                 )  {TotalPutyovki = true;}}}
        public bool RealizasiyaPutyovok                 { get{ return _realizasiyaPutyovok                ;}set{_realizasiyaPutyovok                = value;  if (_realizasiyaPutyovok                )  {TotalPutyovki = true;}}}
        public bool ProsmotrRealPutyovok                { get{ return _prosmotrRealPutyovok               ;}set{_prosmotrRealPutyovok               = value;  if (_prosmotrRealPutyovok               )  {TotalPutyovki = true;}}}
        public bool IzmeneniyaDetaleyRealizasiiPutyovok { get{ return _izmeneniyaDetaleyRealizasiiPutyovok;}set{_izmeneniyaDetaleyRealizasiiPutyovok= value;  if (_izmeneniyaDetaleyRealizasiiPutyovok)  {TotalPutyovki = true;}}}
        public bool OformleniyaVozvrataPutyovok         { get{ return _oformleniyaVozvrataPutyovok        ;}set{_oformleniyaVozvrataPutyovok        = value;  if (_oformleniyaVozvrataPutyovok        )  {TotalPutyovki = true;}}}
        public bool SverkaDannixPoOtdixayushim          { get{ return _sverkaDannixPoOtdixayushim         ;}set{_sverkaDannixPoOtdixayushim         = value;  if (_sverkaDannixPoOtdixayushim         )  {TotalPutyovki = true;}}}  

        public bool RassmotreniyaZayavok { get { return               _rassmotreniyaZayavok               ;}set{ _rassmotreniyaZayavok = value;               if (_rassmotreniyaZayavok               )  { TotalPutyovki = true; } } }



        #endregion

        public bool TotalZayavki { get; private set; }

        private bool _vvodZayavok { get; set; }
        private bool _poiskIRedaktirovaniyaZayavok { get; set; }

        public bool VvodZayavok { get { return _vvodZayavok; } set { _vvodZayavok = value; if (_vvodZayavok) { TotalZayavki = true; } } }
        public bool PoiskIRedaktirovaniyaZayavok { get { return _poiskIRedaktirovaniyaZayavok; } set { _poiskIRedaktirovaniyaZayavok = value; if (_poiskIRedaktirovaniyaZayavok) { TotalZayavki = true; } } }

        #region BuxUchet
        public bool TotalBuxUchet { get; private set; }


        private bool _buxUchetVvodDannix        ;
        private bool _prasmotrDannix            ;
        private bool _ruchniyeProvodki          ;
        private bool _platejniyePorucheniya     ;
        private bool _planByudjeta              ;
        private bool _raznoskaDannixPoOtraslyam ;

        public bool BuxUchetVvodDannix { get { return _buxUchetVvodDannix; } set { _buxUchetVvodDannix = value; if (_buxUchetVvodDannix) { TotalBuxUchet = true; } } }
        public bool PrasmotrDannix { get { return _prasmotrDannix; } set { _prasmotrDannix = value; if (_prasmotrDannix) { TotalBuxUchet = true; } } }
        public bool RuchniyeProvodki { get { return _ruchniyeProvodki; } set { _ruchniyeProvodki = value; if (_ruchniyeProvodki) { TotalBuxUchet = true; } } }
        public bool PlatejniyePorucheniya { get { return _platejniyePorucheniya; } set { _platejniyePorucheniya = value; if (_platejniyePorucheniya) { TotalBuxUchet = true; } } }
        public bool PlanByudjeta { get { return _planByudjeta; } set { _planByudjeta = value; if (_planByudjeta) { TotalBuxUchet = true; } } }
        public bool RaznoskaDannixPoOtraslyam { get { return _raznoskaDannixPoOtraslyam; } set { _raznoskaDannixPoOtraslyam = value; if (_raznoskaDannixPoOtraslyam) { TotalBuxUchet = true; } } }

        #endregion

        #region Reports

        public bool TotalReports { get; private set; }

        private bool _svodniyOtchetPoZayavkam;
        private bool _talabnomalar;
        private bool _formaN13;
        private bool _formaN14;
        private bool _tablisaPoUchetuOtdixayushix;
        private bool _svodniyOtchetPoNakladnim;
        private bool _glavnayaKniga;
        private bool _svodNachisleniyPoOtraslyam;
        private bool _jurnalniyOrder1;
        private bool _jurnalniyOrder2;
        private bool _jurnalniyOrder3;
        private bool _jurnalniyOrder4;
        private bool _jurnalniyOrder5;
        private bool _jurnalniyOrder6;
        private bool _jurnalniyOrder7;
        private bool _jurnalniyOrder8;
        private bool _finansoviyKvartalniy;
        private bool _finansoviyGodovoy;
        private bool _finansoviyPoLageryam;
        private bool _poSanatoriyamProfilaktoriyam;
        private bool _dvijeniyaSredstvPoSchetam;
        private bool _aktSverki;
        private bool _zadoljennostSFP;

        public bool SvodniyOtchetPoZayavkam { get { return _svodniyOtchetPoZayavkam; } set { _svodniyOtchetPoZayavkam = value; if (_svodniyOtchetPoZayavkam) { TotalReports = true; } } }
        public bool Talabnomalar { get { return _talabnomalar; } set { _talabnomalar = value; if (_talabnomalar) { TotalReports = true; } } }
        public bool FormaN13 { get { return _formaN13; } set { _formaN13 = value; if (_formaN13) { TotalReports = true; } } }
        public bool FormaN14 { get { return _formaN14; } set { _formaN14 = value; if (_formaN14) { TotalReports = true; } } }
        public bool TablisaPoUchetuOtdixayushix { get { return _tablisaPoUchetuOtdixayushix; } set { _tablisaPoUchetuOtdixayushix = value; if (_tablisaPoUchetuOtdixayushix) { TotalReports = true; } } }
        public bool SvodniyOtchetPoNakladnim { get { return _svodniyOtchetPoNakladnim; } set { _svodniyOtchetPoNakladnim = value; if (_svodniyOtchetPoNakladnim) { TotalReports = true; } } }
        public bool GlavnayaKniga { get { return _glavnayaKniga; } set { _glavnayaKniga = value; if (_glavnayaKniga) { TotalReports = true; } } }//Главная книга
        public bool SvodNachisleniyPoOtraslyam { get { return _svodNachisleniyPoOtraslyam; } set { _svodNachisleniyPoOtraslyam = value; if (_svodNachisleniyPoOtraslyam) { TotalReports = true; } } }//Свод начислений по отраслям
        public bool JurnalniyOrder1 { get { return _jurnalniyOrder1; } set { _jurnalniyOrder1 = value; if (_jurnalniyOrder1) { TotalReports = true; } } }
        public bool JurnalniyOrder2 { get { return _jurnalniyOrder2; } set { _jurnalniyOrder2 = value; if (_jurnalniyOrder2) { TotalReports = true; } } }
        public bool JurnalniyOrder3 { get { return _jurnalniyOrder3; } set { _jurnalniyOrder3 = value; if (_jurnalniyOrder3) { TotalReports = true; } } }
        public bool JurnalniyOrder4 { get { return _jurnalniyOrder4; } set { _jurnalniyOrder4 = value; if (_jurnalniyOrder4) { TotalReports = true; } } }
        public bool JurnalniyOrder5 { get { return _jurnalniyOrder5; } set { _jurnalniyOrder5 = value; if (_jurnalniyOrder5) { TotalReports = true; } } }
        public bool JurnalniyOrder6 { get { return _jurnalniyOrder6; } set { _jurnalniyOrder6 = value; if (_jurnalniyOrder6) { TotalReports = true; } } }
        public bool JurnalniyOrder7 { get { return _jurnalniyOrder7; } set { _jurnalniyOrder7 = value; if (_jurnalniyOrder7) { TotalReports = true; } } }
        public bool JurnalniyOrder8 { get { return _jurnalniyOrder8; } set { _jurnalniyOrder8 = value; if (_jurnalniyOrder8) { TotalReports = true; } } }
        public bool FinansoviyKvartalniy { get { return _finansoviyKvartalniy; } set { _finansoviyKvartalniy = value; if (_finansoviyKvartalniy) { TotalReports = true; } } }//Финансовый квартальный
        public bool FinansoviyGodovoy { get { return _finansoviyGodovoy; } set { _finansoviyGodovoy = value; if (_finansoviyGodovoy) { TotalReports = true; } } }//Финансовый годовой
        public bool FinansoviyPoLageryam { get { return _finansoviyPoLageryam; } set { _finansoviyPoLageryam = value; if (_finansoviyPoLageryam) { TotalReports = true; } } } //Финансовый по лагерям
        public bool PoSanatoriyamProfilaktoriyam { get { return _poSanatoriyamProfilaktoriyam; } set { _poSanatoriyamProfilaktoriyam = value; if (_poSanatoriyamProfilaktoriyam) { TotalReports = true; } } }//По санаториям-профилакториям
        public bool DvijeniyaSredstvPoSchetam { get { return _dvijeniyaSredstvPoSchetam; } set { _dvijeniyaSredstvPoSchetam = value; if (_dvijeniyaSredstvPoSchetam) { TotalReports = true; } } }//Движение средств по счетам
        public bool AktSverki { get { return _aktSverki; } set { _aktSverki = value; if (_aktSverki) { TotalReports = true; } } } //Акт сверки
        public bool ZadoljennostSFP { get { return _zadoljennostSFP; } set { _zadoljennostSFP = value; if (_zadoljennostSFP) { TotalReports = true; } } }//Задолженность СФП


        #endregion

        #region Administrirovaniya

        public bool TotalAdmin { get; private set; }

        private bool _sozdaniyaUchetnixZapisey;
        private bool _sozdaniyaRoley;
        private bool _sozdaniyaProvodok;
        private bool _privyazkaProvodokkJO;
        private bool _upravleniyaSpravochnikami;

        public bool SozdaniyaUchetnixZapisey { get { return _sozdaniyaUchetnixZapisey; } set { _sozdaniyaUchetnixZapisey = value; if (_sozdaniyaUchetnixZapisey) { TotalAdmin = true; } } }//Создание учетных записей
        public bool SozdaniyaRoley { get { return _sozdaniyaRoley; } set { _sozdaniyaRoley = value; if (_sozdaniyaRoley) { TotalAdmin = true; } } }//Создание ролей
        public bool SozdaniyaProvodok { get { return _sozdaniyaProvodok; } set { _sozdaniyaProvodok = value; if (_sozdaniyaProvodok) { TotalAdmin = true; } } }//Создание проводок
        public bool PrivyazkaProvodokkJO { get { return _privyazkaProvodokkJO; } set { _privyazkaProvodokkJO = value; if (_privyazkaProvodokkJO) { TotalAdmin = true; } } }//Привязка проводок к ЖО
        public bool UpravleniyaSpravochnikami { get { return _upravleniyaSpravochnikami; } set { _upravleniyaSpravochnikami = value; if (_upravleniyaSpravochnikami) { TotalAdmin = true; } } }//Управление справочниками

        private DateTime _lastUpdateTime;

        public DateTime LastUpdateTime
        {
            get { return _lastUpdateTime; }

            set { _lastUpdateTime = value; }
        }

        #endregion
    }
}