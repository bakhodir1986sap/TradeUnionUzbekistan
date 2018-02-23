using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiatPostgres2SqlConverter.ConvertClasses
{
    class t_fixedassetsCl
    {
        public long CountEgrpo()
        {
            long rowsCount = 0;

            NpgsqlConnection conn = new NpgsqlConnection(PostgresConnectionStrings.pgsqlConnectionString);
            conn.Open();

            // Define a query returning a single row result set
            NpgsqlCommand command = new NpgsqlCommand("select count(*) from t_fixedassets", conn);

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
            NpgsqlCommand command = new NpgsqlCommand("select MAX(id) from t_fixedassets", conn);

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
                var sqript = string.Format("select * from t_fixedassets where id >= {0} and id <= {1} order by id", beginId, endId);
                var sqriptCount = string.Format("select count(*) from t_fixedassets where id >= {0} and id <= {1}", beginId, endId);

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
                    t_fixedassets model = new t_fixedassets();

                    model.group_code            = dr[0] == DBNull.Value ? null : (string)dr[0 ];
                    model.version               = dr[2 ] == DBNull.Value ? 0 : (int)dr[2 ];
                    model.code                  = dr[3 ] == DBNull.Value ? null : (string)dr[3 ];
                    model.s_comment             = dr[4 ] == DBNull.Value ? null : (string)dr[4 ];
                    model.s_create              = dr[5 ] == DBNull.Value ? null : (DateTime?)dr[5 ];
                    model.s_status              = dr[6 ] == DBNull.Value ? null : (string)dr[6 ];
                    model.name                  = dr[7 ] == DBNull.Value ? null : (string)dr[7 ];
                    model.accept_date           = dr[8 ] == DBNull.Value ? null : (DateTime?)dr[8 ];
                    model.address               = dr[9 ] == DBNull.Value ? null : (string)dr[9 ];
                    model.build_year            = dr[10] == DBNull.Value ? 0 : (int)dr[10];
                    model.floor_count           = dr[11] == DBNull.Value ? 0 : (int)dr[11];
                    model.square                = dr[12] == DBNull.Value ? 0 : (int)dr[12];
                    model.start_price           = dr[13] == DBNull.Value ? 0 : (int)dr[13];
                    model.start_year            = dr[14] == DBNull.Value ? 0 : (int)dr[14];
                    model.department_code       = dr[15] == DBNull.Value ? null : (string)dr[15];
                    model.s_user_id             = dr[16] == DBNull.Value ? 0 : (int)dr[16];
                    model.soato_code            = dr[17] == DBNull.Value ? null : (string)dr[17];
                    model.color                 = dr[18] == DBNull.Value ? null : (string)dr[18];
                    model.dvigatel              = dr[19] == DBNull.Value ? null : (string)dr[19];
                    model.kuzov                 = dr[20] == DBNull.Value ? null : (string)dr[20];
                    model.count                 = dr[21] == DBNull.Value ? 0 : (int)dr[21];
                    model.measure               = dr[22] == DBNull.Value ? null : (string)dr[22];
                    model.fagroup               = dr[23] == DBNull.Value ? null : (string)dr[23];
                    model.NATIVE_ID             = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                                                     
                    db.t_fixedassets.Add(model);

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
                    maxId = db.t_fixedassets.Max(z => z.id);
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
                maxId = db.t_fixedassets.Count();
            }

            return maxId;
        }

    }
}
