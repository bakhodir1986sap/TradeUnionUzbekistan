using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KasabaKIAT.Models;
using System.Web.Security;
using System.IO;

namespace KasabaKIAT.Controllers
{
    public class LoginController : Controller
    {
        KasabaKIATEntities KasabaDB = new KasabaKIATEntities();
        userlogin ul = new userlogin();
        public static string message = "";
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(user u,string ReturnUrl = "")
        {
            StreamWriter stw;
            try
            {
                if (ModelState.IsValid)
                {
                    if (ul.Ruxsatni_tekshir(u) > 0)
                    {
                        IEnumerable<users> r = from a in KasabaDB.users where a.user_name == u.login select a;
                        foreach (var b in r)
                        {
                            Session["name"] = b.email;
                            FormsAuthentication.SetAuthCookie(b.id.ToString(), false);
                            message = b.user_name;
                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                return RedirectToAction("index", "Home");
                            }
                        }
                    }
                    else
                    {
                        message = "1";
                    }
                }
                else
                {
                    message = "2";
                }
                if (message != "1" && message != "2")
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
                try
                {
                    stw = System.IO.File.AppendText("D:\\Log\\Test.txt");
                }
                catch
                {
                    Directory.CreateDirectory("D:\\Log");
                    stw = System.IO.File.AppendText("D:\\Log\\Test.txt");
                }
                stw.WriteLine("Login Error -> " + ex.ToString());
                stw.Close();
                return RedirectToAction("index", "Home");
            }

            //using (KasabaKIATEntities db = new KasabaKIATEntities())
            //{
            //    var dataItem = db.users.Where(x => x.user_name.Equals(u.user_name)).FirstOrDefault();
            //    if (dataItem != null)
            //    {
            //        /*
            //         && ReturnUrl.Length > 1 && ReturnUrl.StartsWith("/") && !ReturnUrl.StartsWith("//")
            //            && !ReturnUrl.StartsWith("/\\")
            //         */
            //        FormsAuthentication.SetAuthCookie(u.user_name, false);
            //        if (Url.IsLocalUrl(ReturnUrl))
            //        {
            //            return Redirect(ReturnUrl);
            //        }
            //        else
            //        {
            //            return RedirectToAction("index", "Home");
            //        }
            //    }
            //    else
            //    {
            //        ModelState.AddModelError("", "Invalid user name or password");
            //        return View();
            //    }
            //}
        }
        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }
    }
}