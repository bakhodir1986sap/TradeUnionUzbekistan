using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel
{
    public class UserViewModel
    {
        public long userid              { get; set; }
        public string username          { get; set; }
        public string Password          { get; set; }
        public string DepartmentBase    { get; set; }
        public string usertype          { get; set; }

        public long? role_id            { get; set; }

        //Odildan meros
        public long  ID                 { get; set; }
        public long? Rl_id              { get; set; }
        public string Rl                { get; set; }
        public string L                 { get; set; }
        public string N                 { get; set; }
        public DateTime? D              { get; set; }
        public string P                 { get; set; }
        public long? O_id               { get; set; }
        public string O                 { get; set; }

        public long? bkut_id            { get; set; }
        public long? dep_id             { get; set; }
        public long? bkuq_id            { get; set; }

        public string CurKvartalYear    { get; set; }
        public bool   IsPrivOrg         { get; set; }

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