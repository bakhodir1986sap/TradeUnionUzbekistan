using KasabaKIAT.ViewModel.Security.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Security.Roles.Roles
{
    public class Roles
    {
        public void EnterRole(string roleName)
        {
            var role = new RoleViewModel();

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ApplicationException("");
            }
            else
            {
                role.name = roleName;
            }

            role.s_create = DateTime.Now;

            RoleDataManager.Create(ref role);
        }

        public void UpdateRole(long id, string roleName)
        {
            if ((id <= 0) || (string.IsNullOrEmpty(roleName)))
            {
                throw new ApplicationException("");
            }

            var role = new RoleViewModel();

            role.id = id;
            role.name = roleName;

            RoleDataManager.Modify(role);
        }

        public void DeleteRole(long id)
        {
            throw new NotImplementedException();
        }

        public List<RoleViewModel> GetRolesList()
        {
            return RoleDataManager.Get(new RoleViewModel());
        }
    }
}