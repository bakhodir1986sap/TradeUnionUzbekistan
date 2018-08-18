using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.OplataPutyovok.Template
{
    public class OplataViplataTemplateViewModel
    {
        //
            public string       inn              { get; set;} 
            public string       naimenovanie     { get; set;}
            public string       num_scetfak      { get; set;}
            public int?         sanatoriy_code   { get; set;}
            public string       nomer_putyovki   { get; set;}
            public decimal?     summa_puyovki    { get; set;}
            public int?         procent          { get; set;}
            public decimal?     oplacheno        { get; set;}
            public DateTime?    data_oplati         { get; set; }

    }
}