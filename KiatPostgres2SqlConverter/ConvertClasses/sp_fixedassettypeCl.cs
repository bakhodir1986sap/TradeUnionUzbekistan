using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace KiatPostgres2SqlConverter.ConvertClasses
{
    public class sp_fixedassettypeCl
    {
        public long CountEgrpo()
        {
            long rowsCount = 0;

            NpgsqlConnection conn = new NpgsqlConnection(PostgresConnectionStrings.pgsqlConnectionString);
            conn.Open();

            // Define a query returning a single row result set
            NpgsqlCommand command = new NpgsqlCommand("select count(*) from sp_fixedassettype", conn);

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
            NpgsqlCommand command = new NpgsqlCommand("select MAX(id) from sp_fixedassettype", conn);

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
                var sqript = string.Format("select * from sp_fixedassettype where id >= {0} and id <= {1} order by id", beginId, endId);
                var sqriptCount = string.Format("select count(*) from sp_fixedassettype where id >= {0} and id <= {1}", beginId, endId);

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
                    sp_fixedassettype model = new sp_fixedassettype();

                    model.version =     dr[1 ] == DBNull.Value ? 0 : (int)dr[1 ];
                    model.keys =        dr[2 ] == DBNull.Value ? null : (string)dr[2 ];
                    model.code =        dr[3 ] == DBNull.Value ? null : (string)dr[3 ];
                    model.s_comment =   dr[4 ] == DBNull.Value ? null : (string)dr[4 ];
                    model.s_create =    dr[5 ] == DBNull.Value ? null : (DateTime?)dr[5 ];
                    model.s_status =    dr[6 ] == DBNull.Value ? null : (string)dr[6 ];
                    model.name_ru =     dr[7 ] == DBNull.Value ? null : (string)dr[7 ];
                    model.name_uz =     dr[8 ] == DBNull.Value ? null : (string)dr[8 ];
                    model.name_uzl =    dr[9 ] == DBNull.Value ? null : (string)dr[9 ];
                    model.parent_code = dr[10] == DBNull.Value ? null : (string)dr[10];
                    model.s_user_id =   dr[11] == DBNull.Value ? 0 : (long)dr[11];
                    model.NATIVE_ID =   dr[0] == DBNull.Value ? 0 : (long)dr[0];

                    db.sp_fixedassettype.Add(model);

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
                    maxId = db.sp_fixedassettype.Max(z => z.id);
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
                maxId = db.sp_fixedassettype.Count();
            }

            return maxId;
        }

    }
}
