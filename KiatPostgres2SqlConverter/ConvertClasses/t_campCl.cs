using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiatPostgres2SqlConverter.ConvertClasses
{
    public class t_campCl
    {
        public long CountEgrpo()
        {
            long rowsCount = 0;

            NpgsqlConnection conn = new NpgsqlConnection(PostgresConnectionStrings.pgsqlConnectionString);
            conn.Open();

            // Define a query returning a single row result set
            NpgsqlCommand command = new NpgsqlCommand("select count(*) from t_camp", conn);

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
            NpgsqlCommand command = new NpgsqlCommand("select MAX(id) from t_camp", conn);

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
                var sqript = string.Format("select * from t_camp where id >= {0} and id <= {1} order by id", beginId, endId);
                var sqriptCount = string.Format("select count(*) from t_camp where id >= {0} and id <= {1}", beginId, endId);

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
                    t_camp model = new t_camp();

                    model.version                       = dr[1 ] == DBNull.Value ? 0 : (int)dr[1 ];
                    model.camps_address                 = dr[2 ] == DBNull.Value ? null : (string)dr[2 ];
                    model.camps_type1                   = dr[3 ] == DBNull.Value ? 0 : (int)dr[3 ];
                    model.camps_type2                   = dr[4 ] == DBNull.Value ? 0 : (int)dr[4 ];
                    model.camps_organization_address    = dr[5 ] == DBNull.Value ? null : (string)dr[5 ];
                    model.camps_organization_cheef      = dr[6 ] == DBNull.Value ? null : (string)dr[6 ];
                    model.camps_organization_cheef_tel  = dr[7 ] == DBNull.Value ? null : (string)dr[7 ];
                    model.camps_organization_name       = dr[8 ] == DBNull.Value ? null : (string)dr[8 ];
                    model.camps_union                   = dr[9 ] == DBNull.Value ? null : (string)dr[9 ];
                    model.camps_union_tel               = dr[10] == DBNull.Value ? null : (string)dr[10];
                    model.camps_cheef                   = dr[11] == DBNull.Value ? null : (string)dr[11];
                    model.camps_cheef_tel               = dr[12] == DBNull.Value ? null : (string)dr[12];
                    model.code                          = dr[13] == DBNull.Value ? null : (string)dr[13];
                    model.s_comment                     = dr[14] == DBNull.Value ? null : (string)dr[14];
                    model.s_create                      = dr[15] == DBNull.Value ? null : (DateTime?)dr[15];
                    model.s_status                      = dr[16] == DBNull.Value ? null : (string)dr[16];
                    model.camps_email                   = dr[17] == DBNull.Value ? null : (string)dr[17];
                    model.camps_name                    = dr[18] == DBNull.Value ? null : (string)dr[18];
                    model.camps_site                    = dr[19] == DBNull.Value ? null : (string)dr[19];
                    model.camps_tel                     = dr[20] == DBNull.Value ? null : (string)dr[20];
                    model.soato_organization_id         = dr[21] == DBNull.Value ? null : (long?)dr[21];
                    model.s_user_id                     = dr[22] == DBNull.Value ? null : (long?)dr[22];
                    model.soato_id                      = dr[23] == DBNull.Value ? null : (long?)dr[23];
                    model.NATIVE_ID                     = dr[0] == DBNull.Value ? 0 : (long)dr[0];
                                                                                               
                    db.t_camp.Add(model);

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
                    maxId = db.t_camp.Max(z => z.id);
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
                maxId = db.t_camp.Count();
            }

            return maxId;
        }

    }
}
