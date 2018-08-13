using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Buxgalteriya
{
    public class EcpCalcViewModel
    {
        public long id                    { get; set;}
        public long? payment_id            { get; set;}
        public decimal? profsouz_amt          { get; set;}
        public decimal? federation_amt        { get; set;}
        public decimal? oblsovprof_amt        { get; set;}
        public string obl_department_base   { get; set;}
        public long? provodka_id           { get; set; }
        public string remark { get; set; }
        public string inn { get; set; }
        public string soato { get; set; }
        public string soato_name { get; set; }

        public DateTime? operdate     { get; set; }
        public string    okonx_id     { get; set; }
        public string    okonx_name   { get; set; }
        public string    branch_code  { get; set; }
        public string    branch_name  { get; set; }

        public string adep_code { get; set; }
    }
}