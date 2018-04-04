using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiatPostgres2SqlConverter.ConvertClasses
{
    class useractionsCl
    {
        public long CountEgrpo()
        {
            long rowsCount = 0;

            NpgsqlConnection conn = new NpgsqlConnection(PostgresConnectionStrings.pgsqlConnectionString);
            conn.Open();

            // Define a query returning a single row result set
            NpgsqlCommand command = new NpgsqlCommand("select count(*) from useractions", conn);

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
            NpgsqlCommand command = new NpgsqlCommand("select MAX(id) from useractions", conn);

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
                var sqript = string.Format("select * from useractions where id >= {0} and id <= {1} order by id", beginId, endId);
                var sqriptCount = string.Format("select count(*) from useractions where id >= {0} and id <= {1}", beginId, endId);

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
                    useraction model = new useraction();

                    model.version           = dr[1 ] == DBNull.Value ? 0 : (int)dr[1 ];
                    model.accesstime        = dr[2 ] == DBNull.Value ? null : (DateTime?)dr[2 ];
                    model.action            = dr[3 ] == DBNull.Value ? null : (string)dr[3 ];
                    model.classname         = dr[4 ] == DBNull.Value ? null : (string)dr[4 ];
                    model.object_id         = dr[5 ] == DBNull.Value ? 0 : (int)dr[5 ];
                    model.user_id           = dr[6 ] == DBNull.Value ? 0 : (int)dr[6 ];
                    model.object_name       = dr[7 ] == DBNull.Value ? null : (string)dr[7 ];
                    model.NATIVE_ID         = dr[0] == DBNull.Value ? 0 :  (int)dr[0];
                                                                                    
                    //model.version = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    //model.address = dr[2] == DBNull.Value ? "" : (string)dr[2];
                    //model.close_date = dr[3] == DBNull.Value ? null : (DateTime?)dr[3];
                    //model.close_number = dr[4] == DBNull.Value ? null : (string)dr[4];
                    //model.count_workers = dr[5] == DBNull.Value ? null : (int?)dr[5];
                    //model.s_comment = dr[6] == DBNull.Value ? null : (string)dr[6];
                    //model.s_create = dr[7] == DBNull.Value ? null : (DateTime?)dr[7];
                    //model.s_status = dr[8] == DBNull.Value ? null : (string)dr[8];
                    //model.email = dr[9] == DBNull.Value ? null : (string)dr[9];
                    //model.fin_xoz_work = dr[10] == DBNull.Value ? null : (bool?)dr[10];
                    //model.name_full = dr[11] == DBNull.Value ? null : (string)dr[11];
                    //model.inn = dr[12] == DBNull.Value ? null : (string)dr[12];
                    //model.liquidation = dr[13] == DBNull.Value ? null : (bool?)dr[13];
                    //model.osn_worktype = dr[14] == DBNull.Value ? null : (string)dr[14];
                    //model.phones = dr[15] == DBNull.Value ? null : (string)dr[15];
                    //model.recreate = dr[16] == DBNull.Value ? null : (bool?)dr[16];
                    //model.reg_date = dr[17] == DBNull.Value ? null : (DateTime?)dr[17];
                    //model.reg_number = dr[18] == DBNull.Value ? null : (string)dr[18];
                    //model.reg_org = dr[19] == DBNull.Value ? null : (string)dr[19];
                    //model.reorganization = dr[20] == DBNull.Value ? null : (bool?)dr[20];
                    //model.restored = dr[21] == DBNull.Value ? null : (bool?)dr[21];
                    //model.name_short = dr[22] == DBNull.Value ? null : (string)dr[22];
                    //model.street = dr[23] == DBNull.Value ? null : (string)dr[23];
                    //model.update_doc = dr[24] == DBNull.Value ? null : (bool?)dr[24];
                    //model.update_email = dr[25] == DBNull.Value ? null : (bool?)dr[25];
                    //model.mail_index = dr[26] == DBNull.Value ? null : (string)dr[26];
                    //model.s_user_id = dr[27] == DBNull.Value ? null : (long?)dr[27];
                    //model.okonx_id = dr[28] == DBNull.Value ? null : (string)dr[28];
                    //model.opf_id = dr[29] == DBNull.Value ? null : (string)dr[29];
                    //model.fs_id = dr[30] == DBNull.Value ? null : (string)dr[30];
                    //model.soato_id = dr[31] == DBNull.Value ? null : (string)dr[31];
                    //model.soogu_id = dr[32] == DBNull.Value ? null : (string)dr[32];
                    //model.okpo = dr[33] == DBNull.Value ? null : (string)dr[33];
                    //model.NATIVE_ID = dr[0] == DBNull.Value ? 0 : (long)dr[0];

                    db.useractions.Add(model);

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
                    maxId = db.useractions.Max(z => z.id);
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
                maxId = db.useractions.Count();
            }

            return maxId;
        }

    }
}
