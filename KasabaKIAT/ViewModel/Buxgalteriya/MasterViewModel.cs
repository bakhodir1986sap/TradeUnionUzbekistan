using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Buxgalteriya
{
    public class MasterViewModel
    {
      public DateTime? trdt               {get; set;}
      public string department_base    {get; set;}
      public int? dyseq              {get; set;}
      public int? oper_id            {get; set;}
      public string trtp               {get; set;}
      public string orgtrdt            {get; set;}
      public long? orgorg_id          {get; set;}
      public long? orgcrt_usr_id      {get; set;}
      public int? orgdyseq           {get; set;}
      public string remark             {get; set;}
      public long? crtuser_id         {get; set;}
      public long rec_id             {get; set;}
    }
}