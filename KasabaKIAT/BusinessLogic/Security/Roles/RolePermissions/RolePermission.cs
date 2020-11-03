using KasabaKIAT.BusinessLogic.Security.RolePermissions;
using KasabaKIAT.ViewModel.Security.Role;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KasabaKIAT.BusinessLogic.Security.Roles.RolePermissions
{
    public class RolePermission
    {
        public void EnterRolePermit(string rolepermitName, long role_id)
        {
            var role = new RolePermissionViewModel();

            if (string.IsNullOrEmpty(rolepermitName))
            {
                throw new ApplicationException("");
            }
            else
            {
                role.action = rolepermitName;
                role.role_id = role_id;
            }

            role.s_create = DateTime.Now;

            RolePermissionDataManager.Create(ref role);
        }


        public void EnterRolePermit(long rolep_id)
        {

            var rolep = RolePermissionDataManager.Get(rolep_id);

            if (!rolep.Any())
            {
                return;
            }

            var rolepp = rolep.First();

            var role = new RolePermissionViewModel();

            if (string.IsNullOrEmpty(rolepp.action))
            {
                throw new ApplicationException("");
            }
            else
            {
                role.action = rolepp.action;
                role.role_id = rolepp.role_id;
            }

            role.s_create = DateTime.Now;

            RolePermissionDataManager.Create(ref role);
        }


        public void EnterRolePermitN(List<long> ids ,long rolep_id)
        {
            if ((ids == null) || (rolep_id <= 0))
            {
                throw new ApplicationException("");
            }

            foreach(var id in ids)
            {
                var rolep = RolePermissionDataManager.Get(id);

                if (!rolep.Any())
                {
                    continue;
                }

                var rolepp = rolep.First();

                var ownRolePermit = RolePermissionDataManager.GetForCheck(rolep_id, rolepp.action);

                if (ownRolePermit.Any())
                {
                    continue;
                }

                var role = new RolePermissionViewModel();

                if (string.IsNullOrEmpty(rolepp.action))
                {
                    throw new ApplicationException("");
                }
                else
                {
                    role.action = rolepp.action;
                    role.role_id = rolep_id;
                }

                role.s_create = DateTime.Now;

                RolePermissionDataManager.Create(ref role);
            }
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