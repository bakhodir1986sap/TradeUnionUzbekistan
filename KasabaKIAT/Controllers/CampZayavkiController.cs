using KasabaKiat.Helpers;
using KasabaKIAT.BusinessLogic.Lager.OtchetOtLager;
using KasabaKIAT.BusinessLogic.Lager.Zayavki;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace KasabaKIAT.Controllers
{
    public class CampZayavkiController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        ILog log = log4net.LogManager.GetLogger(typeof(CampZayavkiController));

        // GET: CampZayavki
        public ActionResult Index()
        {
            return View();
        }

        // GET: CampZayavki
        public ActionResult CampOtchet()
        {
            return View();
        }

        #region ZayavkiNewLager

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 2, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetLagerNewZayavkaList()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var model = new CampZayavki();

            var listModel = model.GetCampZayavkas(ustinfo.DepartmentBase, CampZayavkaStatusType.New);

            return Json(listModel.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 2, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetLagerSendZayavkaList()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var model = new CampZayavki();

            var listModel = model.GetCampZayavkas(ustinfo.DepartmentBase, CampZayavkaStatusType.Send);

            return Json(listModel.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 2, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetLagerAcceptZayavkaList()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var model = new CampZayavki();

            var listModel = model.GetCampZayavkas(ustinfo.DepartmentBase, CampZayavkaStatusType.Accepted);

            return Json(listModel.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 2, Location = OutputCacheLocation.Client, VaryByCustom = "userId")]
        public JsonResult GetLagerRejectZayavkaList()
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var model = new CampZayavki();

            var listModel = model.GetCampZayavkas(ustinfo.DepartmentBase, CampZayavkaStatusType.Rejected);

            return Json(listModel.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amodel"></param>
        /// <returns></returns>
        public JsonResult AddZayavku(LagerZayavkiViewModel amodel)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var model = new CampZayavki();

            try
            {
                amodel.department_code = ustinfo.DepartmentBase;
                amodel.userid = UserId;
                model.AddZayavku(amodel);

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amodel"></param>
        /// <returns></returns>
        public JsonResult DeleteZayavku(LagerZayavkiViewModel amodel)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var model = new CampZayavki();

            try
            {
                amodel.department_code = ustinfo.DepartmentBase;
                amodel.userid = UserId;
                model.DeleteZayavku(amodel);

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amodel"></param>
        /// <returns></returns>
        public JsonResult ModifyZayavku(LagerZayavkiViewModel amodel)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var model = new CampZayavki();

            try
            {
                amodel.department_code = ustinfo.DepartmentBase;
                amodel.userid = UserId;
                model.ModifyZayavku(amodel);

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult OtklonitZayavku(LagerZayavkiViewModel model)
        {

            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            model.department_code = ustinfo.DepartmentBase;
            model.userid = UserId;

            try
            {
                LagerZayavkiDataManager.UpdateStatus(model, CampZayavkaStatusType.Rejected);

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult SendZayavKu(LagerZayavkiViewModel model)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            model.department_code = ustinfo.DepartmentBase;
            model.userid = UserId;

            try
            {
                LagerZayavkiDataManager.UpdateStatus(model, CampZayavkaStatusType.Send);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult UtverditZayavku(LagerZayavkiViewModel model)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            model.department_code = ustinfo.DepartmentBase;
            model.userid = UserId;

            try
            {
                LagerZayavkiDataManager.UpdateStatus(model, CampZayavkaStatusType.Accepted);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
            return Json("error", JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region OtchetLager

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amodel"></param>
        /// <returns></returns>
        public JsonResult AddCampOtchet(OtchetOtLagerViewModel amodel)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            try
            {
                amodel.department_code = ustinfo.DepartmentBase;
                amodel.userid = UserId;

                var otchet = new OtchetCamp();

                otchet.AddOtchet(amodel);

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amodel"></param>
        /// <returns></returns>
        public JsonResult DeleteOtchet(OtchetOtLagerViewModel amodel)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            try
            {
                amodel.department_code = ustinfo.DepartmentBase;
                amodel.userid = UserId;

                var otchet = new OtchetCamp();

                otchet.DeleteOtchet(amodel);

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amodel"></param>
        /// <returns></returns>
        public JsonResult ModifyOtchet(OtchetOtLagerViewModel amodel)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            try
            {
                amodel.department_code = ustinfo.DepartmentBase;
                amodel.userid = UserId;

                var otchet = new OtchetCamp();

                otchet.ModifyOtchet(amodel);

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amodel"></param>
        /// <returns></returns>
        public JsonResult GetOtchet(OtchetOtLagerViewModel amodel)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            amodel.department_code = ustinfo.DepartmentBase;
            amodel.userid = UserId;

            try
            {
                var otchet = new OtchetCamp();

                var list = otchet.Get(amodel);

                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}