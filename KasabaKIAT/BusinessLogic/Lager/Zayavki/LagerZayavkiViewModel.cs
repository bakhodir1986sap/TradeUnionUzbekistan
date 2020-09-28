using System;

namespace KasabaKIAT.BusinessLogic.Lager.Zayavki
{
    public class LagerZayavkiViewModel
    {
      public long       rec_id              {get; set;}
      public int        id_otras            {get; set;}
      public string     otralsName          { get; set; }
      public long       id_camp             {get; set;}
      public string     campName            { get; set; }
      public int        quantuty            {get; set;}
      public decimal    totalamount         {get; set;}
      public DateTime   timestamp           {get; set;}
      public int        status              {get; set;}
      public string     department_code     {get; set;}
      public long       userid              { get; set; }
    }
}