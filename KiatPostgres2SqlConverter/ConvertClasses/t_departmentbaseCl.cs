using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiatPostgres2SqlConverter.ConvertClasses
{
    class t_departmentbaseCl
    {
        public long CountEgrpo()
        {
            long rowsCount = 0;

            NpgsqlConnection conn = new NpgsqlConnection(PostgresConnectionStrings.pgsqlConnectionString);
            conn.Open();

            // Define a query returning a single row result set
            NpgsqlCommand command = new NpgsqlCommand("select count(*) from t_departmentbase", conn);

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
            NpgsqlCommand command = new NpgsqlCommand("select MAX(id) from t_departmentbase", conn);

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
                var sqript = string.Format("select * from t_departmentbase where id >= {0} and id <= {1} order by id", beginId, endId);
                var sqriptCount = string.Format("select count(*) from t_departmentbase where id >= {0} and id <= {1}", beginId, endId);

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
                    t_departmentbase model = new t_departmentbase();

                    model.dtype                             = dr[0 ] == DBNull.Value ? null :  (string)dr[0];
                    model.version                           = dr[2 ] == DBNull.Value ? 0 : (int)dr[2 ];
                    model.keys                              = dr[3 ] == DBNull.Value ? null : (string)dr[3 ];
                    model.childseq                          = dr[4 ] == DBNull.Value ? 0 : (int)dr[4 ];
                    model.code                              = dr[5 ] == DBNull.Value ? null : (string)dr[5 ];
                    model.s_comment                         = dr[6 ] == DBNull.Value ? null : (string)dr[6 ];
                    model.s_create                          = dr[7 ] == DBNull.Value ? null : (DateTime?)dr[7 ];
                    model.s_status                          = dr[8 ] == DBNull.Value ? null : (string)dr[8 ];
                    model.isleaf                            = dr[9 ] == DBNull.Value ? false : (bool?)dr[9 ];
                    model.s_level                           = dr[10] == DBNull.Value ? 0 : (int)dr[10];
                    model.name_ru                           = dr[11] == DBNull.Value ? null : (string)dr[11];
                    model.name_uz                           = dr[12] == DBNull.Value ? null : (string)dr[12];
                    model.name_uzl                          = dr[13] == DBNull.Value ? null : (string)dr[13];
                    model.address                           = dr[14] == DBNull.Value ? null : (string)dr[14];
                    model.alohida_xona_bormi                = dr[15] == DBNull.Value ? 0 : (int)dr[15];
                    model.computer_bormi                    = dr[16] == DBNull.Value ? 0 : (int)dr[16];
                    model.count_of_members                  = dr[17] == DBNull.Value ? null : (string)dr[17];
                    model.davlat_ruyxatidan_otganmi         = dr[18] == DBNull.Value ? 0 : (int)dr[18];
                    model.email                             = dr[19] == DBNull.Value ? null : (string)dr[19];
                    model.inn                               = dr[20] == DBNull.Value ? null : (string)dr[20];
                    model.internet_bormi                    = dr[21] == DBNull.Value ? 0 : (int)dr[21];
                    model.jamoa_shartnoma_bormi             = dr[22] == DBNull.Value ? 0 : (int)dr[22];
                    model.kasaba_uyushmasi_xaq_tulanadi     = dr[23] == DBNull.Value ? 0 : (int)dr[23];
                    model.bkut_name                         = dr[24] == DBNull.Value ? null : (string)dr[24];
                    model.phone                             = dr[25] == DBNull.Value ? null : (string)dr[25];
                    model.rais_kollegia_azomi               = dr[26] == DBNull.Value ? 0 : (int)dr[26];
                    model.rais_ozodmi                       = dr[27] == DBNull.Value ? 0 : (int)dr[27];
                    model.spec_number                       = dr[28] == DBNull.Value ? null : (string)dr[28];
                    model.type                              = dr[29] == DBNull.Value ? 0 : (int)dr[29];
                    model.xisob_raqam                       = dr[30] == DBNull.Value ? null : (string)dr[30];
                    model.yuqori_shartnomadan_foydalanadimi = dr[31] == DBNull.Value ? 0 : (int)dr[31];
                    model.parent_code                       = dr[32] == DBNull.Value ? null : (string)dr[32];
                    model.branch_code                       = dr[33] == DBNull.Value ? null : (string)dr[33];
                    model.deptype_code                      = dr[34] == DBNull.Value ? null : (string)dr[34];
                    model.s_user_id                         = dr[35] == DBNull.Value ? 0 : (int)dr[35];
                    model.soato_code                        = dr[36] == DBNull.Value ? null : (string)dr[36];
                    model.employee_id                       = dr[37] == DBNull.Value ? 0 : (int)dr[37];
                    model.okonx                             = dr[38] == DBNull.Value ? null : (string)dr[38];
                    model.camps_address                     = dr[39] == DBNull.Value ? null : (string)dr[39];
                    model.camps_type1                       = dr[40] == DBNull.Value ? null : (string)dr[40];
                    model.camps_type2                       = dr[41] == DBNull.Value ? null : (string)dr[41];
                    model.camps_organization_address        = dr[42] == DBNull.Value ? null : (string)dr[42];
                    model.camps_organization_cheef          = dr[43] == DBNull.Value ? null : (string)dr[43];
                    model.camps_organization_cheef_tel      = dr[44] == DBNull.Value ? null : (string)dr[44];
                    model.camps_organization_name           = dr[45] == DBNull.Value ? null : (string)dr[45];
                    model.camps_union                       = dr[46] == DBNull.Value ? null : (string)dr[46];
                    model.camps_union_tel                   = dr[47] == DBNull.Value ? null : (string)dr[47];
                    model.camps_cheef                       = dr[48] == DBNull.Value ? null : (string)dr[48];
                    model.camps_cheef_tel                   = dr[49] == DBNull.Value ? null : (string)dr[49];
                    model.camps_email                       = dr[50] == DBNull.Value ? null : (string)dr[50];
                    model.camps_name                        = dr[51] == DBNull.Value ? null : (string)dr[51];
                    model.camps_site                        = dr[52] == DBNull.Value ? null : (string)dr[52];
                    model.camps_tel                         = dr[53] == DBNull.Value ? null : (string)dr[53];
                    model.soato_organization_id             = dr[54] == DBNull.Value ? 0 : (int)dr[54];
                    model.department_code                   = dr[55] == DBNull.Value ? 0 : (int)dr[55];
                    model.sanatorium_name                   = dr[56] == DBNull.Value ? null : (string)dr[56];
                    model.site                              = dr[57] == DBNull.Value ? null : (string)dr[57];
                    model.telephone                         = dr[58] == DBNull.Value ? null : (string)dr[58];
                    model.sanatorium_profil_id              = dr[59] == DBNull.Value ? 0 : (int)dr[59];
                    model.sanatorium_type_id                = dr[60] == DBNull.Value ? 0 : (int)dr[60];
                    model.employeefullname                  = dr[61] == DBNull.Value ? null : (string)dr[61];
                    model.dep_name                          = dr[62] == DBNull.Value ? null : (string)dr[62];
                    model.le_inn                            = dr[63] == DBNull.Value ? null : (string)dr[63];
                    model.group_2ti                         = dr[64] == DBNull.Value ? null : (short?)dr[64];
                    model.NATIVE_ID                         = dr[1] == DBNull.Value ? 0 : (int)dr[1];

                    db.t_departmentbase.Add(model);

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
                    maxId = db.t_departmentbase.Max(z => z.id);
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
                maxId = db.t_departmentbase.Count();
            }

            return maxId;
        }

    }
}
