using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Security.Role
{
    public class RolePermissionViewModel
    {
        public long id           {get; set;}
        public int version      {get; set;}
        public string action       {get; set;}
        public long? role_id      {get; set;}
        public bool access       {get; set;}
        public string s_comment    {get; set;}
        public DateTime s_create     {get; set;}
        public string s_status     {get; set;}
        public long?  s_user_id    {get; set;}
        public long?  native_id    {get; set;}
    }
}