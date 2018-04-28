using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.ClosePeriod.CloseHistory
{
    public class CloseHistoryViewModel
    {
        public long rec_id               {get; set;}
        public string department_code      {get; set;}
        public DateTime? closed_date          {get; set;}
        public DateTime? timestamp            {get; set;}
        public long? user_id              {get; set;}
        public string remark               {get; set;}
    }
}