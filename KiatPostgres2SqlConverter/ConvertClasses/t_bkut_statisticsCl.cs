using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiatPostgres2SqlConverter.ConvertClasses
{
    public class t_bkut_statisticsCl
    {
        public long CountEgrpo()
        {
            long rowsCount = 0;

            NpgsqlConnection conn = new NpgsqlConnection(PostgresConnectionStrings.pgsqlConnectionString);
            conn.Open();

            // Define a query returning a single row result set
            NpgsqlCommand command = new NpgsqlCommand("select count(*) from t_bkut_statistics", conn);

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
            NpgsqlCommand command = new NpgsqlCommand("select MAX(id) from t_bkut_statistics", conn);

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
                var sqript = string.Format("select * from t_bkut_statistics where id >= {0} and id <= {1} order by id", beginId, endId);
                var sqriptCount = string.Format("select count(*) from t_bkut_statistics where id >= {0} and id <= {1}", beginId, endId);

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
                    t_bkut_statistics model = new t_bkut_statistics();

                    model.version                           = dr[1 ] == DBNull.Value ? 0 : (int)dr[1 ];
                    model.C_16_y_nogiron_far_ayollar        = dr[2 ] == DBNull.Value ? 0 : (int)dr[2 ];
                    model.C_12_y_farzandli_ayollar          = dr[3 ] == DBNull.Value ? 0 : (int)dr[3 ];
                    model.C_30yoshgacha                     = dr[4 ] == DBNull.Value ? 0 : (int)dr[4 ];
                    model.C_3yoshgacha_farzandli_ayollar    = dr[5 ] == DBNull.Value ? 0 : (int)dr[5 ];
                    model.C_4yoki_kop_farzandli_ayollar     = dr[6 ] == DBNull.Value ? 0 : (int)dr[6 ];
                    model.all_emp                           = dr[7 ] == DBNull.Value ? 0 : (int)dr[7 ];
                    model.all_student                       = dr[8 ] == DBNull.Value ? 0 : (int)dr[8 ];
                    model.ayollar                           = dr[9 ] == DBNull.Value ? 0 : (int)dr[9 ];
                    model.azolikdan_chiqarilgan             = dr[10] == DBNull.Value ? 0 : (int)dr[10];
                    model.daromad                           = dr[11] == DBNull.Value ? 0 : (int)dr[11];
                    model.s_comment                         = dr[12] == DBNull.Value ? null : (string)dr[12];
                    model.s_create                          = dr[13] == DBNull.Value ? null : (DateTime?)dr[13];
                    model.s_status                          = dr[14] == DBNull.Value ? null : (string)dr[14];
                    model.hizmatchilar                      = dr[15] == DBNull.Value ? 0 : (int)dr[15];
                    model.ishchilar                         = dr[16] == DBNull.Value ? 0 : (int)dr[16];
                    model.ishlayotgan_nogironlar            = dr[17] == DBNull.Value ? 0 : (int)dr[17];
                    model.ishlayotgan_pensionerlar          = dr[18] == DBNull.Value ? 0 : (int)dr[18];
                    model.kam_taminlangan_oilalar           = dr[19] == DBNull.Value ? 0 : (int)dr[19];
                    model.muhofazasi_daromadi               = dr[20] == DBNull.Value ? 0 : (int)dr[20];
                    model.oliy_malumotli                    = dr[21] == DBNull.Value ? 0 : (int)dr[21];
                    model.raislar                           = dr[22] == DBNull.Value ? 0 : (int)dr[22];
                    model.summa                             = dr[23] == DBNull.Value ? 0 : (int)dr[23];
                    model.yangi_nusxa_azolar                = dr[24] == DBNull.Value ? 0 : (int)dr[24];
                    model.yangi_xodimlar                    = dr[25] == DBNull.Value ? 0 : (int)dr[25];
                    model.year                              = dr[26] == DBNull.Value ? null : (DateTime?)dr[26];
                    model.yolgiz_ayollar                    = dr[27] == DBNull.Value ? 0 : (int)dr[27];
                    model.bkut_id                           = dr[28] == DBNull.Value ? null : (long?)dr[28];
                    model.s_user_id                         = dr[29] == DBNull.Value ? null : (long?)dr[29];
                    model.NATIVE_ID                         = dr[0] == DBNull.Value ? 0 : (long)dr[0];
                                                                                                   
                    db.t_bkut_statistics.Add(model);

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
                    maxId = db.t_bkut_statistics.Max(z => z.id);
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
                maxId = db.t_bkut_statistics.Count();
            }

            return maxId;
        }

    }
}
