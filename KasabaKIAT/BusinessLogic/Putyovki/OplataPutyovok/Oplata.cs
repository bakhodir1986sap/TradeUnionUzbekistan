using KasabaKiat.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Putyovki.OplataPutyovok
{
    public class Oplata
    {
        /*
         * 1) Viplata qilib nakladnoy berilgandan keyin 
         * Oplataga o'tilsa u yerda Viplata Summasi debet tomonda ko'rinadi
         * 
         * 2) Oplata modulida Raznoska qilinadi Qaysi tashkilot necha pul o'tkazganligi
         * ko'rsatiladi va qaysi putyovkalar uchun to'lamganligi ko'rsatiladi
         * 
         * 3) Xar keyingi raznoskada qoldiq keyingi safarga summaga qo'shiladi
         * 
         * 4) Oplata kreditga o'tiradi
         * 
         * 5) Tanlangan putyovkalar Oplata_Rasxodiga o'tiradi
         */

         /// <summary>
         /// 
         /// </summary>
        public Oplata()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="main1"></param>
        /// <param name="putList"></param>
        public void ViplataProizoshla(RealizasiyaViplMainViewModel main1, 
            List<RealizasiyaViplPutViewModel> putList)
        {
            //Soditsya po debetu
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void AddOplataCredit(OplataViewModel model)
        {

        }


        public void AddOplataRasxod(OplataRasxodiViewModel putModel)
        {

        }

        public List<RealizasiyaViplPutViewModel> GetViplataPutList()
        {
            return new List<RealizasiyaViplPutViewModel>();
        }

    }
}