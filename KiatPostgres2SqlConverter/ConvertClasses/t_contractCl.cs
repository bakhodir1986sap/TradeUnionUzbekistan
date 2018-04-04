using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiatPostgres2SqlConverter.ConvertClasses
{
    public class t_contractCl
    {
        public long CountEgrpo()
        {
            long rowsCount = 0;

            NpgsqlConnection conn = new NpgsqlConnection(PostgresConnectionStrings.pgsqlConnectionString);
            conn.Open();

            // Define a query returning a single row result set
            NpgsqlCommand command = new NpgsqlCommand("select count(*) from t_contract", conn);

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
            NpgsqlCommand command = new NpgsqlCommand("select MAX(id) from t_contract", conn);

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
                var sqript = string.Format("select * from t_contract where id >= {0} and id <= {1} order by id", beginId, endId);
                var sqriptCount = string.Format("select count(*) from t_contract where id >= {0} and id <= {1}", beginId, endId);

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
                    t_contract model = new t_contract();

                    model.contracttype  = dr[0] == DBNull.Value ? null : (string)   dr [0];
                    model.version       = dr[2 ] == DBNull.Value ? 0 : (int)        dr[2 ];

                    try
                    {
                        model.accept_date  = dr[3] == DBNull.Value ? null : (DateTime?)dr[3];
                    }
                    catch
                    {
                        model.accept_date = null;
                    }

                    
                    model.code          = dr[4 ] == DBNull.Value ? null: (string)   dr[4 ];

                    try
                    {
                        model.contract_date = dr[5] == DBNull.Value ? null : (DateTime?)dr[5];
                    }
                    catch
                    {
                        model.contract_date = null;
                    }

                    model.contract_no   = dr[6 ] == DBNull.Value ? null: (string)   dr[6 ];
                    model.s_comment     = dr[7 ] == DBNull.Value ? null: (string)   dr[7 ];

                    try
                    {
                        model.s_create = dr[8] == DBNull.Value ? null : (DateTime?)dr[8];
                    }
                    catch
                    {
                        model.s_create = null;
                    }

                    model.s_status      = dr[9 ] == DBNull.Value ? null: (string)   dr[9 ];
                    model.kasaba_signer = dr[10] == DBNull.Value ? null: (string)   dr[10];
                    model.org_signer    = dr[11] == DBNull.Value ? null: (string)   dr[11];
                    model.protokol_no   = dr[12] == DBNull.Value ? null: (string)   dr[12];
                    model.bkut          = dr[13] == DBNull.Value ? null : (long?)   dr[13];
                    model.s_user_id     = dr[14] == DBNull.Value ? null : (long?)   dr[14];
                    model.ctype         = dr[15] == DBNull.Value ? null : (string)  dr[15];
                    model.NATIVE_ID     = dr[1]  == DBNull.Value ? 0 : (long)       dr[1];

                    db.t_contract.Add(model);

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
                    maxId = db.t_contract.Max(z => z.id);
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
                maxId = db.t_contract.Count();
            }

            return maxId;
        }

    }
}
