using KiatPostgres2SqlConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KiatPostgres2SqlConverter.ConvertClasses;

namespace ConverterTester
{
    class Program
    {
        static void Main(string[] args)
        {
            t_sanatoriumCl cl = new t_sanatoriumCl();

            //var count = cl.CountEgrpo();

            //Console.WriteLine(count);

            cl.MigrateEgrpoController();

            Console.ReadKey();
        }
    }
}
