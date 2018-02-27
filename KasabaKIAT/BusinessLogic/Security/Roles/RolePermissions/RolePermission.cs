using KasabaKIAT.BusinessLogic.Security.role_permission.RolePermissions;
using KasabaKIAT.ViewModel.Security.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Security.Roles.RolePermissions
{
    public class RolePermission
    {
        public void EnterRolePermit(string rolepermitName)
        {
            var role = new RolePermissionViewModel();

            if (string.IsNullOrEmpty(rolepermitName))
            {
                throw new ApplicationException("");
            }
            else
            {
                role.action = rolepermitName;
            }

            role.s_create = DateTime.Now;

            RolePermissionDataManager.Create(ref role);
        }

        public void UpdateRolePermit(long id, string rolepermitName)
        {
            if ((id <= 0) || (string.IsNullOrEmpty(rolepermitName)))
            {
                throw new ApplicationException("");
            }

            var role = new RolePermissionViewModel();

            role.id = id;
            role.action = rolepermitName;

            RolePermissionDataManager.Modify(role);
        }

        public void DeleteRolePermit(long id)
        {
            if (id <= 0) 
            {
                throw new ApplicationException("");
            }

            var role = new RolePermissionViewModel();

            role.id = id;

            RolePermissionDataManager.Remove(role);
        }

        public List<RolePermissionViewModel> GetRolePermitsAllList()
        {
            return RolePermissionDataManager.Get();
        }


        public List<RolePermissionViewModel> GetRolePermitsByRoleList(long role_id)
        {
            return RolePermissionDataManager.GetByRole(role_id);
        }
    }
}