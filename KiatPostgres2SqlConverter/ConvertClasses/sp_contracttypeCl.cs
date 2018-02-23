using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace KiatPostgres2SqlConverter.ConvertClasses
{
    class sp_contracttypeCl
    {
        public long CountEgrpo()
        {
            long rowsCount = 0;

            NpgsqlConnection conn = new NpgsqlConnection(PostgresConnectionStrings.pgsqlConnectionString);
            conn.Open();

            // Define a query returning a single row result set
            NpgsqlCommand command = new NpgsqlCommand("select count(*) from sp_contracttype", conn);

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
            NpgsqlCommand command = new NpgsqlCommand("select MAX(id) from sp_contracttype", conn);

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
                var sqript = string.Format("select * from sp_contracttype where id >= {0} and id <= {1} order by id", beginId, endId);
                var sqriptCount = string.Format("select count(*) from sp_contracttype where id >= {0} and id <= {1}", beginId, endId);

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
                    sp_contracttype model = new sp_contracttype();

                    //model.id            = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.version       = dr[1] == DBNull.Value ? 0 : (int)         dr[1];
                    model.code          = dr[2] == DBNull.Value ? null : (string)   dr[2];
                    model.s_comment     = dr[3] == DBNull.Value ? null : (string)   dr[3];
                    model.s_create      = dr[4] == DBNull.Value ? null : (DateTime?)dr[4];
                    model.s_status      = dr[5] == DBNull.Value ? null : (string)   dr[5];
                    model.name_ru       = dr[6] == DBNull.Value ? null : (string)   dr[6];
                    model.name_uz       = dr[7] == DBNull.Value ? null : (string)   dr[7];
                    model.name_uzl      = dr[8] == DBNull.Value ? null : (string)   dr[8];
                    model.s_user_id     = dr[9] == DBNull.Value ? 0 : (int)         dr[9];
                    model.NATIVE_ID     = dr[0] == DBNull.Value ? 0 : (int)         dr[0];

                    db.sp_contracttype.Add(model);

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
                    maxId = db.sp_contracttype.Max(z => z.id);
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
                maxId = db.sp_contracttype.Count();
            }

            return maxId;
        }

    }
}
