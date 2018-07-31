using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Lager.Zayavki
{
    interface ICampZayavki
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        void  AddZayavku(LagerZayavkiViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        void ModifyZayavku(LagerZayavkiViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        void DeleteZayavku(LagerZayavkiViewModel model);


        void SendZayavKu(LagerZayavkiViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        void UtverditZayavku(LagerZayavkiViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        void OtklonitZayavku(LagerZayavkiViewModel model);
    }

    public enum CampZayavkaStatusType
    {
        New,
        Send,
        Accepted,
        Rejected
    }

    public class CampZayavki : ICampZayavki
    {
        public void AddZayavku(LagerZayavkiViewModel model)
        {
            LagerZayavkiDataManager.Create(model);
        }

        public void DeleteZayavku(LagerZayavkiViewModel model)
        {
            LagerZayavkiDataManager.Remove(model);
        }

        public void ModifyZayavku(LagerZayavkiViewModel model)
        {
            LagerZayavkiDataManager.Update(model);
        }

        public void OtklonitZayavku(LagerZayavkiViewModel model)
        {
            LagerZayavkiDataManager.UpdateStatus(model, CampZayavkaStatusType.Rejected);
        }

        public void SendZayavKu(LagerZayavkiViewModel model)
        {
            LagerZayavkiDataManager.UpdateStatus(model, CampZayavkaStatusType.Send);
        }

        public void UtverditZayavku(LagerZayavkiViewModel model)
        {
            LagerZayavkiDataManager.UpdateStatus(model, CampZayavkaStatusType.Accepted);
        }

        public List<LagerZayavkiViewModel> GetCampZayavkas(string departmentBase, CampZayavkaStatusType status)
        {
            return LagerZayavkiDataManager.Get(new LagerZayavkiViewModel() { department_code = departmentBase, status = (int)status });
        }
    }
}