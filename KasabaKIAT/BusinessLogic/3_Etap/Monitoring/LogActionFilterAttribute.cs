using KasabaKiat.Helpers;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KasabaKIAT.BusinessLogic._3_Etap.Monitoring
{
        public class LogActionFilterAttribute : IActionFilter
        {
            private KasabaKIATEntities db;
            private int unsaved = 0;

            public void OnActionExecuted(ActionExecutedContext filterContext)
            {
                //Log("OnActionExecuted", filterContext.RouteData);
            }

            public void OnActionExecuting(ActionExecutingContext filterContext)
            {
                Log("OnActionExecuting", filterContext.RouteData);
            }

            private void Log(string methodName, RouteData routeData)
            {
                if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name)) return;

                int UserId = Convert.ToInt32(HttpContext.Current.User.Identity.Name);

                var controllerName = routeData.Values["controller"];
                var actionName = routeData.Values["action"];
                var message = String.Format("{0} controller:{1} action:{2}", methodName, controllerName, actionName);
                Debug.WriteLine(message, "Action Filter Log");

                if (db == null)
                {
                    db = new KasabaKIATEntities();
                }


                var model = new useractions
                {
                    version = 0,
                    accesstime = DateTime.Now,
                    action = actionName.ToString(),
                    classname = controllerName.ToString(),
                    object_id = 0,
                    user_id = UserId,
                    object_name = HttpContext.Current.Request.UserHostAddress
                };

                unsaved++; 
                db.useractions.Add(model);
                
                if (unsaved >= 1)
                {
                    db.SaveChanges();
                    unsaved = 0;
                }
            }
        }

}