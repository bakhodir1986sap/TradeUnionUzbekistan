using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Security;

namespace KasabaKIAT.MyRoleProvider
{
    public class SiteRole : RoleProvider
    {
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string userID)
        {
            //int UserID = Convert.ToInt32(userID);
            //if (!HttpContext.Current.User.Identity.IsAuthenticated)
            //{
            //    return null;
            //}
            //var cacheKey = string.Format("{0_role}", userID);
            //if (HttpRuntime.Cache[cacheKey] != null)
            //{
            //    return (string[])HttpRuntime.Cache[cacheKey];
            //}
            //string[] roles = new string[] {};
            //using(KasabaKIATEntities DB=new KasabaKIATEntities())
            //{
            //    roles = (from u in DB.users
            //             join r in DB.roles on u.role_id equals r.id
            //              //where u.id == UserID
            //             select r.name).ToArray<string>();
            //    if (roles.Count() > 0)
            //    {
            //        HttpRuntime.Cache.Insert(cacheKey, roles, null, DateTime.Now.AddMilliseconds(_cacheTimeoutInMinute), Cache.NoSlidingExpiration);

            //    }
            //}
            //return roles;
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
            int UserID = Convert.ToInt32(userID);
            KasabaKIATEntities db = new KasabaKIATEntities();
            var list = from u in db.users
                       where u.id == UserID
                       join r in db.roles on u.role_id equals r.id
                       select new
                       {
                           R = r.name
                       };
            string data = list.FirstOrDefault().R;
            string[] result = { data };
            return result;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var userRoles = GetRolesForUser(username);
            return userRoles.Contains(roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}