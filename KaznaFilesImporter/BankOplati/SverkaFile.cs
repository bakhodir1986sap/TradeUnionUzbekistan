using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaznaFilesImporter.BankOplati
{
    public class SverkaFile: BaseKaznaFile
    {
        public string k_type { get; set; }//Always 2
        public string k_operday { get; set; } 
        public string file_name { get; set; } 
        public string create_datetime { get; set; } 

        public long file_id { get; set; }
    }
}
