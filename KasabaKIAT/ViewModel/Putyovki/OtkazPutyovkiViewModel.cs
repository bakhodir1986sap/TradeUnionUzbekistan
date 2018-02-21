using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Putyovki
{
    public class OtkazPutyovkiViewModel
    {
        public long     rec_id           {get; set;}
        public long?    user_id          {get; set;}
        public string   department_code  {get; set;}
        public DateTime otkaz_date       {get; set;}
        public long     realization_id   {get; set;}
        public string   remark           {get; set;}
    }
}