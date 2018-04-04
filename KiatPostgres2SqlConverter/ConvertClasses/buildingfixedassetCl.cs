using log4net;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiatPostgres2SqlConverter
{
    public class buildingfixedassetCl
    {
        ILog log = log4net.LogManager.GetLogger(typeof(buildingfixedassetCl));

        public long CountBuilding()
        {
            long rowsCount = 0;

            NpgsqlConnection conn = new NpgsqlConnection(PostgresConnectionStrings.pgsqlConnectionString);

            try
            {
                conn.Open();

                // Define a query returning a single row result set
                NpgsqlCommand command = new NpgsqlCommand("select count(*) from buildingfixedasset", conn);

                // Execute the query and obtain the value of the first column of the first row
                rowsCount = (long)command.ExecuteScalar();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                conn.Close();
            }

            return rowsCount;
        }


        public long MaxBuilding()
        {
            long rowsCount = 0;

            var conn = new NpgsqlConnection(PostgresConnectionStrings.pgsqlConnectionString);

            try
            {
                conn.Open();

                // Define a query returning a single row result set
                NpgsqlCommand command = new NpgsqlCommand("select MAX(id) from buildingfixedasset", conn);

                // Execute the query and obtain the value of the first column of the first row
                rowsCount = (long)command.ExecuteScalar();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                conn.Close();
            }

            return rowsCount;
        }


        public long CountBuilding(string sqript)
        {
            long rowsCount = 0;

            var conn = new NpgsqlConnection(PostgresConnectionStrings.pgsqlConnectionString);
            conn.Open();

            // Define a query returning a single row result set
            NpgsqlCommand command = new NpgsqlCommand(sqript, conn);

            // Execute the query and obtain the value of the first column of the first row
            rowsCount = (long)command.ExecuteScalar();

            conn.Close();

            return rowsCount;
        }


        public void MigrateBuildingController()
        {
            var SqlCount = SqlServerBuildingCount();
            var SqlMaxId = SqlServerBuildingMaxId();
            var PgCount = CountBuilding();
            var PgMaxId = MaxBuilding();

            long retcount = 0;
            long beginId = SqlMaxId + 1;
            long endId = beginId + 1000;

            do
            {
                var sqript = string.Format("select * from buildingfixedasset where id >= {0} and id <= {1} order by id", beginId, endId);
                var sqriptCount = string.Format("select count(*) from buildingfixedasset where id >= {0} and id <= {1}", beginId, endId);

                retcount = CountBuilding(sqriptCount);

                MigrateBuilding(sqript);

                beginId = endId + 1;
                endId = beginId + 1000;

            }
            while (retcount > 0);
        }


        private void MigrateBuilding(string sqript)
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
                    buildingfixedasset model = new buildingfixedasset();

                    //model.id            = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.version       = dr[1] == DBNull.Value ? 0 : (int)dr[1];
                    model.accept_date   = dr[2] == DBNull.Value ? null : (DateTime?)dr[2];
                    model.address       = dr[3] == DBNull.Value ? null : (string)dr[3];
                    model.build_year    = dr[4] == DBNull.Value ? 0 : (int)dr[4];
                    model.floor_count   = dr[5] == DBNull.Value ? 0 : (int)dr[5];
                    model.square        = dr[6] == DBNull.Value ? 0 : (float)dr[6];
                    model.start_price   = dr[7] == DBNull.Value ? 0 : (float)dr[7];
                    model.start_year    = dr[8] == DBNull.Value ? 0 : (int)dr[8];
                    model.soato_code    = dr[9] == DBNull.Value ? null : (string)dr[9];
                    model.NATIVE_ID     = dr[0] == DBNull.Value ? 0 : (long)dr[0];

                    db.buildingfixedassets.Add(model);

                    db.SaveChanges();
                }

            }

            conn.Close();

        }


        private long SqlServerBuildingMaxId()
        {
            long maxId = 0;

            using (KasabaKIAT_N1Entities db = new KasabaKIAT_N1Entities())
            {
                try
                {
                    maxId = db.buildingfixedassets.Max(z => z.id);
                }
                catch (Exception)
                {
                    maxId = 0;
                }
            }

            return maxId;
        }


        private long SqlServerBuildingCount()
        {
            long maxId = 0;

            using (KasabaKIAT_N1Entities db = new KasabaKIAT_N1Entities())
            {
                maxId = db.buildingfixedassets.Count();
            }

            return maxId;
        }
    }
}
