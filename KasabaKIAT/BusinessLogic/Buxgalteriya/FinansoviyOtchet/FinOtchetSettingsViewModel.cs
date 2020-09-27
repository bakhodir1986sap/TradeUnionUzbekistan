namespace KasabaKIAT.BusinessLogic.Buxgalteriya.FinansoviyOtchet
{
    public class FinOtchetSettingsViewModel
    {
      public long   rec_id          {get; set;}
      public int    otchet_part     {get; set;}
      public string code            {get; set;}
      public string debit_side      {get; set;}
      public string credit_side     {get; set;}
      public string account_code    {get; set;}
      public int?    data_type       {get; set;}
      public string data_desc       {get; set;}
    }
}