using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Buxgalteriya
{
    public class PlatejkiViewModel
    {
      public long rec_id             { get; set;}
      public string department_code    { get; set;}
      public long? userid             { get; set;}
      public DateTime? plat_date          { get; set;}
      public string plat_num           { get; set;}
      public string platelname         { get; set;}
      public string platelinn          { get; set;}
      public string platelraschet      { get; set;}
      public string platelbankname     { get; set;}
      public string platelbankmfo      { get; set;}
      public string poluchname         { get; set;}
      public string poluchinn          { get; set;}
      public string poluchraschet      { get; set;}
      public string poluchbankname     { get; set;}
      public string poluchbankmfo      { get; set;}
      public decimal? summa              { get; set;}
      public string comment            { get; set;}
      public string rukovoditel        { get; set;}
      public string glavbux            { get; set;}
    }
}