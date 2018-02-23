using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiatPostgres2SqlConverter.ConvertClasses
{
    class t_camp_statisticsCl
    {
        public long CountEgrpo()
        {
            long rowsCount = 0;

            NpgsqlConnection conn = new NpgsqlConnection(PostgresConnectionStrings.pgsqlConnectionString);
            conn.Open();

            // Define a query returning a single row result set
            NpgsqlCommand command = new NpgsqlCommand("select count(*) from t_camp_statistics", conn);

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
            NpgsqlCommand command = new NpgsqlCommand("select MAX(id) from t_camp_statistics", conn);

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
                var sqript = string.Format("select * from t_camp_statistics where id >= {0} and id <= {1} order by id", beginId, endId);
                var sqriptCount = string.Format("select count(*) from t_camp_statistics where id >= {0} and id <= {1}", beginId, endId);

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
                    t_camp_statistics model = new t_camp_statistics();

                    model.version               = dr[1] == DBNull.Value ? 0 : (int)dr [1];
                    model.capital_reapir        = dr[2 ] == DBNull.Value ? null : (string)dr[2 ];
                    model.capital_reapir_amount = dr[3 ] == DBNull.Value ? null : (string)dr[3 ];
                    model.child_shift           = dr[4 ] == DBNull.Value ? 0 : (int)dr[4 ];
                    model.child_shift_1         = dr[5 ] == DBNull.Value ? 0 : (int)dr[5 ];
                    model.child_shift_2         = dr[6 ] == DBNull.Value ? 0 : (int)dr[6 ];
                    model.child_shift_3         = dr[7 ] == DBNull.Value ? 0 : (int)dr[7 ];
                    model.child_shift_4         = dr[8 ] == DBNull.Value ? 0 : (int)dr[8 ];
                    model.child_shift_5         = dr[9 ] == DBNull.Value ? 0 : (int)dr[9 ];
                    model.child_shift_all       = dr[10] == DBNull.Value ? 0 : (int)dr[10];
                    model.count_shift           = dr[11] == DBNull.Value ? 0 : (int)dr[11];
                    model.current_reapir        = dr[12] == DBNull.Value ? null : (string)dr[12];
                    model.current_reapir_amount = dr[13] == DBNull.Value ? null : (string)dr[13];
                    model.s_comment             = dr[14] == DBNull.Value ? null : (string)dr[14];
                    model.s_create              = dr[15] == DBNull.Value ? null : (DateTime?)dr[15];
                    model.s_status              = dr[16] == DBNull.Value ? null : (string)dr[16];
                    model.reconstruction        = dr[17] == DBNull.Value ? null : (string)dr[17];
                    model.reconstruction_amount = dr[18] == DBNull.Value ? null : (string)dr[18];
                    model.year                  = dr[19] == DBNull.Value ? null : (DateTime?)dr[19];
                    model.camp_id               = dr[20] == DBNull.Value ? 0 : (int)dr[20];
                    model.s_user_id             = dr[21] == DBNull.Value ? 0 : (int)dr[21];
                    model.NATIVE_ID             = dr[0] == DBNull.Value ? 0 : (int)dr[0];
                                                                                       
                    db.t_camp_statistics.Add(model);

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
                    maxId = db.t_camp_statistics.Max(z => z.id);
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
                maxId = db.t_camp_statistics.Count();
            }

            return maxId;
        }

    }
}
