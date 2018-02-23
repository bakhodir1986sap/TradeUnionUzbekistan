using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiatPostgres2SqlConverter.ConvertClasses
{
    class eco_platejCl
    {
        public long CountEgrpo()
        {
            long rowsCount = 0;

            NpgsqlConnection conn = new NpgsqlConnection(PostgresConnectionStrings.pgsqlConnectionString);
            conn.Open();

            // Define a query returning a single row result set
            NpgsqlCommand command = new NpgsqlCommand("select count(*) from eco_platej", conn);

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
            NpgsqlCommand command = new NpgsqlCommand("select MAX(id) from eco_platej", conn);

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
                var sqript = string.Format("select * from eco_platej where id >= {0} and id <= {1} order by id", beginId, endId);
                var sqriptCount = string.Format("select count(*) from eco_platej where id >= {0} and id <= {1}", beginId, endId);

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
                    eco_platej model = new eco_platej();

                    model.version           = dr[1 ] == DBNull.Value ? 0 : (int)        dr[1 ];
                    model.accountnumber     = dr[2 ] == DBNull.Value ? null : (string)  dr[2 ];
                    model.clientnumber      = dr[3 ] == DBNull.Value ? null : (string)  dr[3 ];
                    model.cnt               = dr[4 ] == DBNull.Value ? 0 : (int)        dr[4 ];
                    model.codepayment       = dr[5 ] == DBNull.Value ? null : (string)  dr[5 ];
                    model.companyname       = dr[6 ] == DBNull.Value ? null : (string)  dr[6 ];
                    model.credit            = dr[7 ] == DBNull.Value ? 0 : (int)        dr[7 ];
                    model.datetime          = dr[8 ] == DBNull.Value ? null : (string)  dr[8 ];
                    model.debit             = dr[9 ] == DBNull.Value ? 0 : (int)        dr[9 ];
                    model.filename          = dr[10] == DBNull.Value ? null : (string)  dr[10];
                    model.inn               = dr[11] == DBNull.Value ? null : (string)  dr[11];
                    model.innnumber         = dr[12] == DBNull.Value ? null : (string)  dr[12];
                    model.mfo               = dr[13] == DBNull.Value ? null : (string)  dr[13];
                    model.numdoc            = dr[14] == DBNull.Value ? null : (string)  dr[14];
                    model.op                = dr[15] == DBNull.Value ? null : (string)  dr[15];
                    model.schet             = dr[16] == DBNull.Value ? null : (string)  dr[16];
                    model.textpayment       = dr[17] == DBNull.Value ? null : (string)  dr[17];
                    model.yr                = dr[18] == DBNull.Value ? null : (string)  dr[18];
                    model.NATIVE_ID         = dr[0] == DBNull.Value ? 0 : (long)         dr[0];

                    db.eco_platej.Add(model);

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
                    maxId = db.eco_platej.Max(z => z.id);
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
                maxId = db.eco_platej.Count();
            }

            return maxId;
        }
    }
}
