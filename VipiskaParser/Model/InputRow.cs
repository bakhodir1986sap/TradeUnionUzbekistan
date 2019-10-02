using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VipiskaParser.Model
{
    public class InputRow
    {
        public string DateStr { get; set; }

        public string TimeStr { get; set; }

        public string DocumentNumber { get; set; }

        public string Operation { get; set; }

        public string detailsRow1 { set; get; }

        public string detailsRow2 { get; set; }

        public string detailsRow3 { get; set; }

        public string Debit { get; set; }

        public string Credit { get; set; }

    }
}
