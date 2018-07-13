using KasabaKIAT.BusinessLogic.Utils;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KasabaKIAT.BusinessLogic.Buxgalteriya.GlavnayaKniga;

namespace KasabaKIAT.BusinessLogic.ClosePeriod.CloseKvartal
{
    public delegate void QuoterEvent(CloseKvartalViewModel model);

    public class CloseKvartal
    {

        public event QuoterEvent OnQuoterClosing;
        public event QuoterEvent OnQuoterClosed;
        public event QuoterEvent OnCancelQuoterClosing;
        public event QuoterEvent OnCancelQuoterClosed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DepCode"></param>
        /// <returns></returns>
        public CloseKvartalViewModel GetCloseKvartalRow(string DepCode)
        {
            var businessDates = CloseKvartalDataManager.Get(new CloseKvartalViewModel() { department_code = DepCode });

            if (businessDates != null)
            {
                var businessDate = businessDates.FirstOrDefault();

                if (businessDate != null)
                {
                    return businessDate;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DepCode"></param>
        /// <param name="UserId"></param>
        public void Close(string DepCode, long UserId, KasabaKIATEntities db)
        {
            var clsRow = GetCloseKvartalRow(DepCode);

            if (OnQuoterClosing != null)
                OnQuoterClosing(clsRow);

            if (clsRow != null)
            {
                var clsChange = new CloseKvartalViewModel();
                clsChange.department_code = DepCode;
                clsChange.prevkv_year = clsRow.curkv_year;
                clsChange.prevkv = clsRow.curkv;
                clsChange.curkv_year = clsRow.nextkv_year;
                clsChange.curkv = clsRow.nextkv;

                int o_year, o_kv;

                DateOperations.GetNextQuarter(clsRow.nextkv_year.Value, clsRow.nextkv.Value, out o_year, out o_kv);

                clsChange.nextkv_year = o_year;
                clsChange.nextkv = o_kv;

                CloseKvartalDataManager.Modify(clsChange, db);

                var history = new CloseKvartalHistoryViewModel();

                history.department_code = DepCode;
                history.closed_kvyear = clsChange.prevkv_year;
                history.closed_kv = clsChange.prevkv;
                history.remark = "Close Kvartal";

                CloseKvartalHistoryDataManager.Create(history, db);

                Balance.CloseBalance(DepCode, clsChange.prevkv_year.Value, clsChange.prevkv.Value, db);

                if (OnQuoterClosed != null)
                    OnQuoterClosed(clsChange);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DepCode"></param>
        /// <param name="UserId"></param>
        public void CancelClose(string DepCode, long UserId, KasabaKIATEntities db)
        {
            var clsRow = GetCloseKvartalRow(DepCode);

            if (OnCancelQuoterClosing != null)
                OnCancelQuoterClosing(clsRow);

            if (clsRow != null)
            {
                var clsChange = new CloseKvartalViewModel();
                clsChange.department_code = DepCode;

                clsChange.nextkv_year = clsChange.curkv_year;
                clsChange.nextkv = clsChange.curkv;
                clsChange.curkv_year = clsChange.prevkv_year;
                clsChange.curkv = clsChange.prevkv;


                int o_year, o_kv;

                DateOperations.GetPrevQuarter(clsRow.prevkv_year.Value, clsRow.prevkv.Value, out o_year, out o_kv);

                clsChange.prevkv_year = o_year;
                clsChange.prevkv = o_kv;

                CloseKvartalDataManager.Modify(clsChange, db);

                var history = new CloseKvartalHistoryViewModel();

                history.department_code = DepCode;
                history.closed_kvyear = clsChange.nextkv_year;
                history.closed_kv = clsChange.nextkv;
                history.remark = "Cancel Close Kvartal";

                CloseKvartalHistoryDataManager.Create(history, db);

                if (OnCancelQuoterClosed != null)
                    OnCancelQuoterClosed(clsChange);

            }
        }


    }
}