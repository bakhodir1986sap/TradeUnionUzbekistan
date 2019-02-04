using KasabaKIAT.BusinessLogic.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Zayavki
{
    public enum ZayavkiSubjectOrder
    {
        None = -1,
        PPO = 0,
        TumanTarmoq = 1,
        Oblast = 2,
        Federasiya = 3
    }

    public enum ZayavkaStates
    {
        New,
        Sent,
        InProgress,
        Reject,
        Apply,
        Realized
    }

    public class ZayavkiModel2
    {
        public long id { get; set; }
        public string departmentBase { get; set; }
        public int status { get; set; }
    }

    public class ZayavkaStateChangeHist
    {
        public static void AddZayavkaHist(ZayavkiModel2 model, ZayavkaStates state, ZayavkiSubjectOrder order)
        {

        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class ZayavkiProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        private StructureOfKiat orderFindHelper { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ZayavkiProcessor()
        {
            orderFindHelper = new StructureOfKiat();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        private void AddZayavka(ZayavkiModel2 model)
        {
            var order = GetOrder(model.departmentBase);
            ZayavkaStateChangeHist.AddZayavkaHist(model, ZayavkaStates.New, order);
            SetTotalZayavkaState(model, ZayavkaStates.New);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentBase"></param>
        /// <returns></returns>
        private ZayavkiSubjectOrder GetOrder(string departmentBase)
        {
            var result = orderFindHelper.GetWhoIsThisCode(departmentBase);

            if (result == KIAT_STRUCT.UnKnown)
            {
                return ZayavkiSubjectOrder.None;
            }
            else if (result == KIAT_STRUCT.PPO)
            {
                return ZayavkiSubjectOrder.PPO;
            }
            else if (result == KIAT_STRUCT.TumanBranch)
            {
                return ZayavkiSubjectOrder.TumanTarmoq;
            }
            else if (result == KIAT_STRUCT.Oblsovprof)
            {
                return ZayavkiSubjectOrder.Oblast;
            }
            else if (result == KIAT_STRUCT.Federation)
            {
                return ZayavkiSubjectOrder.Federasiya;
            }

            return ZayavkiSubjectOrder.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        private void ModifyZayavka(ZayavkiModel2 model)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tostate"></param>
        private void SetTotalZayavkaState(ZayavkiModel2 model, ZayavkaStates tostate)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        private void DeleteZayavka(ZayavkiModel2 model)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        private void SendZayavka(ZayavkiModel2 model)
        {

            var order = GetOrder(model.departmentBase);
            ZayavkaStateChangeHist.AddZayavkaHist(model, ZayavkaStates.Sent, order);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        private void ApplyZayavka(ZayavkiModel2 model)
        {
            //check if this zayavka is already appied by this order
            //
            var order = GetOrder(model.departmentBase);
            ZayavkaStateChangeHist.AddZayavkaHist(model, ZayavkaStates.Apply, order);

            if (order == ZayavkiSubjectOrder.Federasiya)
            {
                SetTotalZayavkaState(model, ZayavkaStates.Apply);
            }
            else
            {
                SetTotalZayavkaState(model, ZayavkaStates.InProgress);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        private void RejectZayavka(ZayavkiModel2 model)
        {

            var order = GetOrder(model.departmentBase);
            ZayavkaStateChangeHist.AddZayavkaHist(model, ZayavkaStates.Reject, order);
            SetTotalZayavkaState(model, ZayavkaStates.Reject);
        }

        private List<ZayavkiModel2> GetZayavkaList(string departmentBase)
        {
            List<ZayavkiModel2> list = new List<ZayavkiModel2>();
            //Send and InProcess status
            var order = GetOrder(departmentBase);

            if (order == ZayavkiSubjectOrder.Federasiya)
            {
                //Give zayavkas which Applied in OblSovprof and 
                //Created by Federasiya itself
                //Sent by Oblsovprof

            }
            else if (order == ZayavkiSubjectOrder.Oblast)
            {
                //Give zayavkas which Applied by Tuman or PPO dependent who is his/her parent
                //and zayavkas which created by Oblsovprof itself and zayavkas which send 
                //to Oblsovprof.

            }
            else if (order == ZayavkiSubjectOrder.TumanTarmoq)
            {
                //Give zayavkas which Send by PPO. And zayavkas which created by Tuman tarmoq

            }
            else if (order == ZayavkiSubjectOrder.PPO)
            {
                //Give zayavkas which created by PPO only

            }
            else
            {
                throw new ApplicationException("invalid order type");
            }

            return list;
        }
    }
}