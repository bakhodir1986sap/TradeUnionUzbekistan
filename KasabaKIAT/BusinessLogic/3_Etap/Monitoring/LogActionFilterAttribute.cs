using KasabaKIAT.Models;
using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KasabaKIAT.BusinessLogic._3_Etap.Monitoring
{
        public class LogActionFilterAttribute : IActionFilter
        {
            private KasabaKIATEntities _db;
            private int _unsaved = 0;

            public void OnActionExecuted(ActionExecutedContext filterContext)
            {
            }

            public void OnActionExecuting(ActionExecutingContext filterContext)
            {
                Log("OnActionExecuting", filterContext.RouteData);
            }

            private void Log(string methodName, RouteData routeData)
            {
                if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name)) return;

                int userId = Convert.ToInt32(HttpContext.Current.User.Identity.Name);

                var controllerName = routeData.Values["controller"];
                var actionName = routeData.Values["action"];
                var message = String.Format("{0} controller:{1} action:{2}", methodName, controllerName, actionName);
                Debug.WriteLine(message, "Action Filter Log");

                if (_db == null)
                {
                    _db = new KasabaKIATEntities();
                }


                var model = new useractions
                {
                    version = 0,
                    accesstime = DateTime.Now,
                    action = actionName.ToString(),
                    classname = controllerName.ToString(),
                    object_id = 0,
                    user_id = userId,
                    object_name = HttpContext.Current.Request.UserHostAddress
                };

                _unsaved++; 
                _db.useractions.Add(model);
                
                if (_unsaved >= 1)
                {
                    _db.SaveChanges();
                    _unsaved = 0;
                }
            }
        }

}