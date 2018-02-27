using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Security.Role
{
    public class RoleViewModel
    {
        public long id           {get; set;}
        public int? version      {get; set;}
        public string name         {get; set;}
        public string s_comment    {get; set;}
        public DateTime? s_create     {get; set;}
        public string s_status     {get; set;}
        public long? s_user_id    {get; set;}
        public long? native_id    {get; set;}
    }
}