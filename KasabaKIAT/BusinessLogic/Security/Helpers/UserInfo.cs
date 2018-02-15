using KasabaKIAT.Models;
using KasabaKiat.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.Helpers
{
    public class UserInfo
    {
        public static UserViewModel GetUserInfo(string userName)
        {
            UserViewModel model = null;

            try
            {
                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    var users = from usermodel in db.users
                                where usermodel.user_name == userName
                                select usermodel;

                    if (users.Any())
                    {
                        var currentUser = users.FirstOrDefault();

                        if (currentUser != null)
                        {
                            model = new UserViewModel();

                            model.userid = currentUser.id;
                            model.username = currentUser.user_name;
                            model.usertype = currentUser.usertype;
                            model.role_id = currentUser.role_id;

                            long? departmentcode = 0;

                            if (model.usertype.Equals("BKUQ"))
                            {
                                departmentcode = currentUser.bkuq_id;
                            }
                            else if (model.usertype.Equals("BKUT"))
                            {
                                departmentcode = currentUser.bkut_id;
                            }
                            else if (model.usertype.Equals("DEPARTMENT"))
                            {
                                departmentcode = currentUser.dep_id;
                            }
                            else if (model.usertype.Equals("SIMPLE"))
                            {
                                departmentcode = null;
                            }
                            else
                            {
                                departmentcode = null;
                            }

                            var departments = from dep in db.t_departmentbase
                                              where dep.id == departmentcode
                                              select dep;

                            if (departments.Any())
                            {
                                var depbase = departments.FirstOrDefault();

                                if (depbase != null)
                                {
                                    model.DepartmentBase = depbase.code;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            return model;
        }

        public static UserViewModel GetUserInfo(long userId)
        {
            UserViewModel model = null;

            try
            {
                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    var users = from usermodel in db.users
                                where usermodel.id == userId
                                select usermodel;

                    if (users.Any())
                    {
                        var currentUser = users.FirstOrDefault();

                        if (currentUser != null)
                        {
                            model = new UserViewModel();

                            model.userid = currentUser.id;
                            model.username = currentUser.user_name;
                            model.usertype = currentUser.usertype;
                            model.role_id = currentUser.role_id;

                            long? departmentcode = 0;

                            if (model.usertype.Equals("BKUQ"))
                            {
                                departmentcode = currentUser.bkuq_id;
                            }
                            else if (model.usertype.Equals("BKUT"))
                            {
                                departmentcode = currentUser.bkut_id;
                            }
                            else if (model.usertype.Equals("DEPARTMENT"))
                            {
                                departmentcode = currentUser.dep_id;
                            }
                            else if (model.usertype.Equals("SIMPLE"))
                            {
                                departmentcode = null;
                            }
                            else
                            {
                                departmentcode = null;
                            }

                            var departments = from dep in db.t_departmentbase
                                              where dep.id == departmentcode
                                              select dep;

                            if (departments.Any())
                            {
                                var depbase = departments.FirstOrDefault();

                                if (depbase != null)
                                {
                                    model.DepartmentBase = depbase.code;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            return model;
        }

        public static void UserLoginAction(UserViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                useractivity ua = new useractivity();

                ua.version = 0;
                ua.last_logged = DateTime.Now;
                ua.last_logged_id = HttpContext.Current.Request.UserHostAddress;
                ua.user_id = model.userid;

                db.useractivity.Add(ua);

                db.SaveChanges();
            }
        }

        public static void UserActions(string userName, 
            string controllerName, 
            string actionName, 
            string methodName)
        {
            var userinfo = GetUserInfo(userName);

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                useractions ua = new useractions();

                ua.version = 0;
                ua.accesstime = DateTime.Now;
                ua.action = actionName;
                ua.classname = controllerName;
                ua.object_name = methodName;
                ua.user_id = userinfo.userid;

                db.useractions.Add(ua);

                db.SaveChanges();
            }
        }


        public static List<string> GetAllowedScreenList(string username)
        {
            var userinfo = GetUserInfo(username);

            List<string> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var screens = from permission in db.role_permission
                              where permission.role_id == userinfo.role_id
                              select permission.action;

                if (screens.Any())
                {
                    list = new List<string>();

                    foreach (var sc in screens)
                    {
                        list.Add(sc);
                    }
                }
            }

            return list;
        }

    }
}