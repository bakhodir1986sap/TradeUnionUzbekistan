using Migration.Viloyats.TashVil.Chastichka;
using Migration.Viloyats.TashVil.DanniyeVladelsev;
using Migration.Viloyats.TashVil.Rasxodi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration
{
    class Program
    {
        static void Main(string[] args)
        {
            var migRasxod = new Oplata();

            migRasxod.Process();
        }
    }
}
