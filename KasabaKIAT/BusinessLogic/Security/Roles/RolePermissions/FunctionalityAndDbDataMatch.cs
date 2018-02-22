using KasabaKiat.ViewModel;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Security.Roles.RolePermissions
{
    public class FunctionalityAndDbDataMatch
    {
        public FunctionalityList MatchProcess(UserViewModel user)
        {
            var rolFumc = new FunctionalityList();

            rolFumc.UserId = user.userid;

            if (!user.role_id.HasValue)
            {
                throw new ApplicationException("Invalid rol id");
            }

            rolFumc.RoleId = user.role_id.Value;
            rolFumc.UserName = user.username;


            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var rolPermitL = db.role_permission.Where(p => p.role_id == rolFumc.RoleId);

                if (rolPermitL.Any())
                {
                    foreach(var rolperm in rolPermitL)
                    {

                    }
                }
                else
                {
                    throw new ApplicationException("");
                }


            }

            return null;
        }
    }
}