using log4net;
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
    public class userx
    {
        [Required]
        public string login { get; set; }
        [Required]
        public string parol { get; set; }
    }
    public class userlogin
    {
        ILog log = log4net.LogManager.GetLogger(typeof(userlogin));

        string constr = ConfigurationManager.ConnectionStrings["LoginEntities"].ConnectionString;

        public int Ruxsatni_tekshir(userx user)
        {
            try
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
            catch(SqlException ex)
            {
                log.Error(ex);
                return -999;
                
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return 0;
        }
    }
}