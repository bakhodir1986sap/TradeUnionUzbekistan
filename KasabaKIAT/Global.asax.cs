using KasabaKIAT.BusinessLogic._3_Etap.Monitoring;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace KasabaKIAT
{
    public class MvcApplication : System.Web.HttpApplication
    {
        ILog log = log4net.LogManager.GetLogger(typeof(MvcApplication));

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
            if (cookie != null && cookie.Value != null)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cookie.Value);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cookie.Value);
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru");
            }
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            try
            {
                if (custom == "userId")
                {
                    return User.Identity.Name;
                }
                else
                {
                    return base.GetVaryByCustomString(context, custom);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return base.GetVaryByCustomString(context, custom);
        }

        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            //log the error!
            log.Error("Global log");
            log.Error(ex);
        }
    }
}
