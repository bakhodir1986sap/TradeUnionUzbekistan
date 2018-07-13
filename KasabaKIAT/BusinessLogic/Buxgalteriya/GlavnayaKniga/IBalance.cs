using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.GlavnayaKniga
{
    interface IBalance
    {
        /// <summary>
        /// For the begining of Project initializes balance rows
        /// 
        /// </summary>
        /// <param name="model"></param>
        void InitBalance(BalanceViewModel model);

        /// <summary>
        /// Initialized balance changed by this method
        /// </summary>
        /// <param name="model"></param>
        void ModifyBalance(BalanceViewModel model);

        /// <summary>
        /// Create new balance for not closed kvartal
        /// </summary>
        /// <param name="model"></param>
        void CloseBranch(BalanceViewModel model);

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BalanceViewModel GetCurBalance(BalanceViewModel model);


    }
}
