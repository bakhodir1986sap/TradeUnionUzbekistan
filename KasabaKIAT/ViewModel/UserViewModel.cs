using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel
{
    public class UserViewModel
    {
        public long userid { get; set; }
        public string username { get; set; }
        public string Password { get; set; }
        public string DepartmentBase { get; set; }
        public string usertype { get; set; }

        public long? role_id { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        public string username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

    }
}