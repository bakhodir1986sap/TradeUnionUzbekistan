using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Putyovki
{
    public class ViplNumGenViewModel
    {
        public long id                {get; set;}
        public string department_base   {get; set;}
        public int? numdoc_gen        {get; set;}
        public long? user_id { get; set; }
        public int? year { get; set; }
    }
}