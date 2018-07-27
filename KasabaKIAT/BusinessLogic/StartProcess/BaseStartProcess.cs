using KasabaKIAT.BusinessLogic.ClosePeriod.CloseKvartal;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.StartProcess
{
    public class BaseStartProcess
    {
        //v Sisteme 2017 - 4 kvartal vsem ustanovim
        //Danniye po nije ukazannim pozisiyam budut udaleni
        //za 2018 god
        //Pozisii po kotorim budut udaleni:
        //1) Provodki
        //2) Balance
        //3) Prixod
        //4) Rasxod
        //5) Oplati i Viplati
        //6) Vozvrat
        //7) Danniye o vladelsev putyovok
        //
        //Zagruzit: danniye
        //
        //1) Ostatki po putyovkam Nakladnaya po XXX organizasii inn: 999999999
        //Sanatory_Code, Nut_Put, Summa, DataZaezda
        //2) Ostatki po organizasiyam 
        // inn , nazvaniye_org, Debet_Ost, Credit_Ost
        //3) Balance ostatki po balansovim 
        // Account_Code, Debit_Bal, Credit_Bal
        //
        //Zakrit Kvartal 2017 - 4 kvartal
        //
        //Otkroitsya noviy kvartal 2018 - 1 kvartal
        //
        //1) Nachnetsya Prixod s 01.01.2018 po 31.03.2018
        //2) Rasxod
        //3) Oplata i Viplati
        //4) Provodki Avtomaticheskiye po ECP zapustyatsya
        //

        public void MakeStartBusinessKvartal(string DepCode, long UserId, KasabaKIATEntities db)
        {
            //Make 2017 4 kvartal
            var cls = new CloseKvartal();

            cls.CancelClose(DepCode, UserId, db);

        }

        public void ClearAllNotNeededOperations(string DepCode, long UserId, KasabaKIATEntities db)
        {

        }

        public void CloseBusinessKvartal(string DepCode, long UserId, KasabaKIATEntities db)
        {
            //Make 2018 4 kvartal
        }
    }
}