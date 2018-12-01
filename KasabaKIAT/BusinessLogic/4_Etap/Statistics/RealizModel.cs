using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic._4_Etap.Statistics
{
    public class RealizModel
    {
            public int? YEAR_OP               {get; set;}
            public int? QUARTER_op            {get; set;}
            public string ProfsoyzNaimenovanie  {get; set;}
            public string Num_Put               {get; set;}
            public string Name_San              {get; set;}
            public decimal? s45Amount             {get; set;}
            public int? s45Count              {get; set;}
            public decimal? s100Amount            {get; set;}
            public int? s100Count             {get; set;}
            public decimal? s0Amount              {get; set;}
            public int? s0Count               {get; set;}
            
    }
}