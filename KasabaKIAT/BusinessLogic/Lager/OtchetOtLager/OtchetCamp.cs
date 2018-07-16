using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Lager.OtchetOtLager
{
    interface IOtchetCamp
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        void AddOtchet(OtchetOtLagerViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        void ModifyOtchet(OtchetOtLagerViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        void DeleteOtchet(OtchetOtLagerViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        void SendOtchet(OtchetOtLagerViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        void UtverditOtchet(OtchetOtLagerViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        void OtklonitOtchet(OtchetOtLagerViewModel model);
    }

    public enum CampOtchetStatusType
    {
        New,
        Send,
        Accepted,
        Rejected
    }

    public class OtchetCamp : IOtchetCamp
    {
        public void AddOtchet(OtchetOtLagerViewModel model)
        {
            OtchetOtLagerDataManager.Create(model);
        }

        public void DeleteOtchet(OtchetOtLagerViewModel model)
        {
            OtchetOtLagerDataManager.Remove(model);
        }

        public void ModifyOtchet(OtchetOtLagerViewModel model)
        {
            OtchetOtLagerDataManager.Update(model);
        }

        public void OtklonitOtchet(OtchetOtLagerViewModel model)
        {
            OtchetOtLagerDataManager.UpdateStatus(model, CampOtchetStatusType.Rejected);
        }

        public void SendOtchet(OtchetOtLagerViewModel model)
        {
            OtchetOtLagerDataManager.UpdateStatus(model, CampOtchetStatusType.Send);
        }

        public void UtverditOtchet(OtchetOtLagerViewModel model)
        {
            OtchetOtLagerDataManager.UpdateStatus(model, CampOtchetStatusType.Accepted);
        }
    }
}