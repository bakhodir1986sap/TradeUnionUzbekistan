using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiatPostgres2SqlConverter.ConvertClasses
{
    class t_ku_reportCl
    {
        public long CountEgrpo()
        {
            long rowsCount = 0;

            NpgsqlConnection conn = new NpgsqlConnection(PostgresConnectionStrings.pgsqlConnectionString);
            conn.Open();

            // Define a query returning a single row result set
            NpgsqlCommand command = new NpgsqlCommand("select count(*) from t_ku_report", conn);

            // Execute the query and obtain the value of the first column of the first row
            rowsCount = (long)command.ExecuteScalar();

            conn.Close();

            return rowsCount;
        }


        public long MaxEgrpo()
        {
            long rowsCount = 0;

            NpgsqlConnection conn = new NpgsqlConnection(PostgresConnectionStrings.pgsqlConnectionString);
            conn.Open();

            // Define a query returning a single row result set
            NpgsqlCommand command = new NpgsqlCommand("select MAX(id) from t_ku_report", conn);

            // Execute the query and obtain the value of the first column of the first row
            rowsCount = (long)command.ExecuteScalar();

            conn.Close();

            return rowsCount;
        }


        public long CountEgrpo(string sqript)
        {
            long rowsCount = 0;

            NpgsqlConnection conn = new NpgsqlConnection(PostgresConnectionStrings.pgsqlConnectionString);
            conn.Open();

            // Define a query returning a single row result set
            NpgsqlCommand command = new NpgsqlCommand(sqript, conn);

            // Execute the query and obtain the value of the first column of the first row
            rowsCount = (long)command.ExecuteScalar();

            conn.Close();

            return rowsCount;
        }


        public void MigrateEgrpoController()
        {
            var SqlCount = SqlServerEgrpoCount();
            var SqlMaxId = SqlServerEgrpoMaxId();
            var PgCount = CountEgrpo();
            var PgMaxId = MaxEgrpo();

            long retcount = 0;
            long beginId = SqlMaxId + 1;
            long endId = beginId + 1000;

            do
            {
                var sqript = string.Format("select * from t_ku_report where id >= {0} and id <= {1} order by id", beginId, endId);
                var sqriptCount = string.Format("select count(*) from t_ku_report where id >= {0} and id <= {1}", beginId, endId);

                retcount = CountEgrpo(sqriptCount);

                MigrateEgrpo(sqript);

                beginId = endId + 1;
                endId = beginId + 1000;

            }
            while (retcount > 0);
        }


        private void MigrateEgrpo(string sqript)
        {
            NpgsqlConnection conn = new NpgsqlConnection(PostgresConnectionStrings.pgsqlConnectionString);
            conn.Open();

            // Define a query
            NpgsqlCommand cmd = new NpgsqlCommand(sqript, conn);

            // Execute a query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // Read all rows and output the first column in each row
            while (dr.Read())
            {

                using (KasabaKIAT_N1Entities db = new KasabaKIAT_N1Entities())
                {
                    t_ku_report model = new t_ku_report();

                    model.version                           = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.azolikdan_chiqarilgan_xodimlar    = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.daromad_jsh                       = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.date                              = dr[1] == DBNull.Value ? null : (DateTime?)dr[1];
                    model.s_comment                         = dr[1] == DBNull.Value ? null : (string)dr[1];
                    model.s_create                          = dr[1] == DBNull.Value ? null : (DateTime?)dr[1];
                    model.s_status                          = dr[1] == DBNull.Value ? null : (string)dr[1];
                    model.ku_emp                            = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.ku_masul                          = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.ku_shtat                          = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.ku_technik                        = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.le_emp_age30                      = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.le_emp_all                        = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.le_emp_female                     = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.mablag_mehnat_muhofazasi          = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.member_emp_age30                  = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.member_emp_all                    = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.member_emp_female                 = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.member_pensioner                  = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.member_student_age30              = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.member_student_all                = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.member_student_female             = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.student_age30                     = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.student_all                       = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.student_female                    = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.yangi_azolik_bileti               = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.yangi_xodimlar                    = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.year                              = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.s_user_id                         = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.ku_id                             = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.computer                          = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.internet                          = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.jamoa_shartnoma                   = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.kollegiyaga_azo                   = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.rais_ozod                         = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.xaq_tulanadigan_apparati          = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.xona                              = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.yuqori_jamoa_shartnoma            = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.member_kasanachilar               = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.member_nogironlar                 = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.NATIVE_ID                         = dr[1] == DBNull.Value ? 0 : (int)dr[1];

                    db.t_ku_report.Add(model);

                    db.SaveChanges();
                }

            }

            conn.Close();

        }


        private long SqlServerEgrpoMaxId()
        {
            long maxId = 0;

            using (KasabaKIAT_N1Entities db = new KasabaKIAT_N1Entities())
            {
                try
                {
                    maxId = db.t_ku_report.Max(z => z.id);
                }
                catch (Exception)
                {
                    maxId = 0;
                }
            }

            return maxId;
        }


        private long SqlServerEgrpoCount()
        {
            long maxId = 0;

            using (KasabaKIAT_N1Entities db = new KasabaKIAT_N1Entities())
            {
                maxId = db.t_ku_report.Count();
            }

            return maxId;
        }

    }
}
