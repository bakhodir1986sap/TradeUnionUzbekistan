using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiatPostgres2SqlConverter.ConvertClasses
{
    class usersCl
    {
        public long CountEgrpo()
        {
            long rowsCount = 0;

            NpgsqlConnection conn = new NpgsqlConnection(PostgresConnectionStrings.pgsqlConnectionString);
            conn.Open();

            // Define a query returning a single row result set
            NpgsqlCommand command = new NpgsqlCommand("select count(*) from users", conn);

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
            NpgsqlCommand command = new NpgsqlCommand("select MAX(id) from users", conn);

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
                var sqript = string.Format("select * from users where id >= {0} and id <= {1} order by id", beginId, endId);
                var sqriptCount = string.Format("select count(*) from users where id >= {0} and id <= {1}", beginId, endId);

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

                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    user model = new user();

                    model.version           = dr[1 ] == DBNull.Value ? 0 : (int)dr[1 ];
                    model.account_expired   = dr[2 ] == DBNull.Value ? false : (bool)dr[2 ];
                    model.account_locked    = dr[3 ] == DBNull.Value ? false : (bool)dr[3 ];
                    model.changed           = dr[4 ] == DBNull.Value ? null : (DateTime?)dr[4 ];
                    model.created           = dr[5 ] == DBNull.Value ? null : (DateTime?)dr[5 ];
                    model.email             = dr[6 ] == DBNull.Value ? null : (string)dr[6 ];
                    model.last_logged       = dr[7 ] == DBNull.Value ? null : (DateTime?)dr[7 ];
                    model.last_logged_id    = dr[8 ] == DBNull.Value ? null : (string)dr[8 ];
                    model.state             = dr[9 ] == DBNull.Value ? null : (string)dr[9 ];
                    model.password          = dr[10] == DBNull.Value ? null : (string)dr[10];
                    model.salt              = dr[11] == DBNull.Value ? null : (string)dr[11];
                    model.language          = dr[12] == DBNull.Value ? null : (string)dr[12];
                    model.is_super_admin    = dr[13] == DBNull.Value ? null : (bool?)dr[13];
                    model.user_name         = dr[14] == DBNull.Value ? null : (string)dr[14];
                    model.s_comment         = dr[15] == DBNull.Value ? null : (string)dr[15];
                    model.s_create          = dr[16] == DBNull.Value ? null : (DateTime?)dr[16];
                    model.s_status          = dr[17] == DBNull.Value ? null : (string)dr[17];
                    model.s_user_id         = dr[18] == DBNull.Value ? 0 : (int)dr[18];
                    model.usertype          = dr[19] == DBNull.Value ? null : (string)dr[19];
                    model.bkut_id           = dr[20] == DBNull.Value ? 0 : (int)dr[20];
                    model.dep_id            = dr[21] == DBNull.Value ? 0 : (int)dr[21];
                    model.role_id           = dr[22] == DBNull.Value ? 0 : (int)dr[22];
                    model.bkuq_id           = dr[23] == DBNull.Value ? 0 : (int)dr[23];
                    model.REGION_ID         = dr[24] == DBNull.Value ? null : (string)dr[24];
                    model.BRANCH_ID         = dr[25] == DBNull.Value ? 0 : (int)dr[25];
                    model.NATIVE_ID         = dr[26] == DBNull.Value ? 0 : (int)dr[26];
                                                                                   
                    db.users.Add(model);

                    db.SaveChanges();
                }

            }

            conn.Close();

        }


        private long SqlServerEgrpoMaxId()
        {
            long maxId = 0;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                try
                {
                    maxId = db.users.Max(z => z.id);
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

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                maxId = db.users.Count();
            }

            return maxId;
        }

    }
}
