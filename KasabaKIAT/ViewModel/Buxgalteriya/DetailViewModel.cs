using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Buxgalteriya
{
    public class DetailViewModel
    {
       public DateTime? trdt              { get; set;}
       public string department_code   { get; set;}
       public int? dyseq             { get; set;}
       public int? dytrseq           { get; set;}
       public string account           { get; set;}
       public string drcr              { get; set;}
       public string ccy               { get; set;}
       public decimal? amount            { get; set;}
       public string remark            { get; set;}
       public long? crtuser_id        { get; set;}
       public long rec_id            { get; set;}
       public string inn               { get; set;}
    }
}