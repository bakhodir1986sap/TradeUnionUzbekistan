using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KasabaKIAT.Models
{
    public class user
    {
        [Required]
        public string login { get; set; }
        [Required]
        public string parol { get; set; }
    }
    public class userlogin
    {
        string constr = ConfigurationManager.ConnectionStrings["LoginEntities"].ConnectionString;
        public int Ruxsatni_tekshir(user user)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("uspLogin", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add(new SqlParameter("@login", user.login));
                com.Parameters.Add(new SqlParameter("@parol", user.parol));
                return Convert.ToInt32(com.ExecuteScalar());
            }
        }
    }
}