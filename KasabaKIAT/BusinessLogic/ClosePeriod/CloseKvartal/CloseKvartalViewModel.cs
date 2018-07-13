using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.ClosePeriod.CloseKvartal
{
    public class CloseKvartalViewModel
    {
      public long   rec_id          {get; set;}
      public string department_code {get; set;}
      public int?    prevkv_year     {get; set;}
      public int?    prevkv          {get; set;}
      public int?    curkv_year      {get; set;}
      public int?    curkv           {get; set;}
      public int?    nextkv_year     {get; set;}
      public int?    nextkv          { get; set; }
    }
}