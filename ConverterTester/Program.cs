using KiatPostgres2SqlConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConverterTester
{
    class Program
    {
        static void Main(string[] args)
        {
            PostgresADO cl = new PostgresADO();

            //var count = cl.CountEgrpo();

            //Console.WriteLine(count);

            cl.MigrateEgrpoController();

            Console.ReadKey();
        }
    }
}
