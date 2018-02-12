using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Buxgalteriya.Balance
{
    public class Provodka
    {
        public void MakeProvodka(ProvodkaParams param)
        {
            //Change in Journal Master
            JournalMasterChange(param);

            //Change in Journal Detail
            JournalDetailsChange(param);

            //Change In Balance
            BalanceChange(param);
        }

        private void JournalMasterChange(ProvodkaParams param)
        {

        }

        private void JournalDetailsChange(ProvodkaParams param)
        {

        }

        private void BalanceChange(ProvodkaParams param)
        {

        }
    }
}