using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.SverkaDannix.Ruchnoy
{
    public class SanatDataCompareViewModel
    {
      public long   id          {get; set;}
      public int?   year        {get; set;}
      public int?   owner_tp    {get; set;}
      public int?   sanat_id    {get; set;}
      public string num_put     {get; set;}
      public string date_san    {get; set;}
      public string price       {get; set;}
      public string fio_otdix   {get; set;}
      public string adress      {get; set;}
      public string work_place  {get; set;}
      public string otrasl      {get; set;}
      public string owner_code { get; set; }
    }
}