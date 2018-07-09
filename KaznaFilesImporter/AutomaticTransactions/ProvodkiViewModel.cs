using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Buxgalteriya
{
    public class ProvodkiViewModel
    {
      public long       rec_id             { get; set;}
      public string     department_base    { get; set;}
      public long?      user_id            { get; set;}
      public bool?      inoutflg           { get; set;}
      public DateTime?  trdt               { get; set;}
      public string     number             { get; set;}
      public bool?      tipscheta          { get; set;}
      public bool?      tipoborota         { get; set;}
      public string     platelname         { get; set;}
      public string     platelinn          { get; set;}
      public string     platelmfo          { get; set;}
      public string     poluchname         { get; set;}
      public string     poluchinn          { get; set;}
      public string     poluchmfo          { get; set;}
      public string     platelrsch         { get; set;}
      public string     poluchrsch         { get; set;}
      public decimal?   summa              { get; set;}
      public string     comment            { get; set;}
      public string     trref { get; set; }
      public string     trrefno { get; set; }
    }
}