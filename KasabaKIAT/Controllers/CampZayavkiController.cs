using KasabaKiat.Helpers;
using KasabaKIAT.BusinessLogic.Lager.OtchetOtLager;
using KasabaKIAT.BusinessLogic.Lager.Zayavki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KasabaKIAT.Controllers
{
    public class CampZayavkiController : Controller
    {
        // GET: CampZayavki
        public ActionResult CampZayavki()
        {
            return View();
        }

        // GET: CampZayavki
        public ActionResult CampOtchet()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult AddZayavku(LagerZayavkiViewModel model)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            if (model == null) return Json("fail", JsonRequestBehavior.AllowGet);

            model.department_code = ustinfo.DepartmentBase;
            model.userid = UserId;

            try
            {
                var camp = new CampZayavki();
                camp.AddZayavku(model);
            }
            catch(Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult DeleteZayavku(LagerZayavkiViewModel model)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            if (model == null) return Json("fail", JsonRequestBehavior.AllowGet);

            model.department_code = ustinfo.DepartmentBase;
            model.userid = UserId;

            try
            {
                var camp = new CampZayavki();
                camp.DeleteZayavku(model);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult ModifyZayavku(LagerZayavkiViewModel model)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            if (model == null) return Json("fail", JsonRequestBehavior.AllowGet);

            model.department_code = ustinfo.DepartmentBase;
            model.userid = UserId;

            try
            {
                var camp = new CampZayavki();
                camp.ModifyZayavku(model);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
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

            if (model == null) return Json("fail", JsonRequestBehavior.AllowGet);

            model.department_code = ustinfo.DepartmentBase;
            model.userid = UserId;

            try
            {
                var camp = new CampZayavki();
                camp.OtklonitZayavku(model);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
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

            if (model == null) return Json("fail", JsonRequestBehavior.AllowGet);

            model.department_code = ustinfo.DepartmentBase;
            model.userid = UserId;

            try
            {
                var camp = new CampZayavki();
                camp.SendZayavKu(model);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);

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

            if (model == null) return Json("fail", JsonRequestBehavior.AllowGet);

            model.department_code = ustinfo.DepartmentBase;
            model.userid = UserId;

            try
            {
                var camp = new CampZayavki();
                camp.UtverditZayavku(model);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult AddOtchet(OtchetOtLagerViewModel model)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            if (model == null) return Json("fail", JsonRequestBehavior.AllowGet);

            model.department_code = ustinfo.DepartmentBase;
            model.userid = UserId;

            try
            {
                var otchet = new OtchetCamp();
                otchet.AddOtchet(model);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult DeleteOtchet(OtchetOtLagerViewModel model)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            if (model == null) return Json("fail", JsonRequestBehavior.AllowGet);

            model.department_code = ustinfo.DepartmentBase;
            model.userid = UserId;

            try
            {
                var otchet = new OtchetCamp();
                otchet.DeleteOtchet(model);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult ModifyOtchet(OtchetOtLagerViewModel model)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            if (model == null) return Json("fail", JsonRequestBehavior.AllowGet);

            model.department_code = ustinfo.DepartmentBase;
            model.userid = UserId;

            try
            {
                var otchet = new OtchetCamp();
                otchet.DeleteOtchet(model);

                OtchetOtLagerDataManager.Update(model);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult OtklonitOtchet(OtchetOtLagerViewModel model)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            if (model == null) return Json("fail", JsonRequestBehavior.AllowGet);

            model.department_code = ustinfo.DepartmentBase;
            model.userid = UserId;

            try
            {
                var otchet = new OtchetCamp();
                otchet.OtklonitOtchet(model);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult SendOtchet(OtchetOtLagerViewModel model)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            if (model == null) return Json("fail", JsonRequestBehavior.AllowGet);

            model.department_code = ustinfo.DepartmentBase;
            model.userid = UserId;

            try
            {
                var otchet = new OtchetCamp();
                otchet.SendOtchet(model);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult UtverditOtchet(OtchetOtLagerViewModel model)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            if (model == null) return Json("fail", JsonRequestBehavior.AllowGet);

            model.department_code = ustinfo.DepartmentBase;
            model.userid = UserId;

            try
            {
                var otchet = new OtchetCamp();
                otchet.UtverditOtchet(model);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }
    }
}