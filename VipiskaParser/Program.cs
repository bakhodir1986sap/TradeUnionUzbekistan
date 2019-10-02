using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VipiskaParser.Controller;

namespace VipiskaParser
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadFileData.ReadAllVipiskaFiles();

            Console.ReadKey();
        }
    }
}
