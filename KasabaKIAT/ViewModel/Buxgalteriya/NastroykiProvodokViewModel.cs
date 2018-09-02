using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Buxgalteriya
{
    public class NastroykiProvodokViewModel
    {
      public long       id                { get; set;}
      public long?       user_id           { get; set;}
      public string     department_base { get; set;}
      public string     description     { get; set;}
      public string     score_begin     { get; set;}
      public string     score_end       { get; set; }
    }


    public class ProvodkaSettingViewModel
    {
        public long   rec_id            { get; set;}
        public string department_base   { get; set;}
        public long?  user_id           { get; set;}
        public string code              { get; set;}
        public string debit_acc         { get; set;}
        public string creadit_acc       { get; set;}
    }
}