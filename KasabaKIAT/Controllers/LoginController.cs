using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KasabaKIAT.Models;
using System.Web.Security;
using System.IO;
using log4net;
using KasabaKiat.Helpers;

namespace KasabaKIAT.Controllers
{
    public class LoginController : Controller
    {
        KasabaKIATEntities KasabaDB = new KasabaKIATEntities();
        ILog log = log4net.LogManager.GetLogger(typeof(LoginController));
        userlogin ul = new userlogin();
        UserInfo UserLog = new UserInfo();
        public static string message = "";
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(user u,string ReturnUrl = "")
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var retCode = ul.Ruxsatni_tekshir(u);

                    if (retCode > 0)
                    {
                        IEnumerable<users> r = from a in KasabaDB.users where a.user_name == u.login select a;
                        foreach (var b in r)
                        {
                            int UserID = Convert.ToInt32(b.id);
                            UserInfo.UserLoginAction(new KasabaKiat.ViewModel.UserViewModel { userid = UserID });
                            //~~~~~~~~~~~~~~~~~~~~~
                            KasabaKIATEntities db = new KasabaKIATEntities();
                            var list = from uu in db.users
                                       where uu.id == UserID
                                       join rr in db.roles on uu.role_id equals rr.id
                                       select new
                                       {
                                           R = rr.id
                                       };
                            //~~~~~~~~~~~~~~~~~~~~~
                            Session["name"] = b.email;
                            Session["role"] = list.FirstOrDefault().R;
                            FormsAuthentication.SetAuthCookie(b.id.ToString(), false);
                            message = list.FirstOrDefault().R.ToString();
                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                return Json(message, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else if (retCode == -999)
                    {
                        message = "-9";
                    }
                    else
                    {
                        message = "-1";
                    }
                }
                else
                {
                    message = "-2";
                }
                if (message != "-1" && message != "-2" && message != "-9")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }
    }
}