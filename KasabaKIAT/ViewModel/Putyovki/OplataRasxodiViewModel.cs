using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Putyovki
{
    public class OplataRasxodiViewModel
    {
      public long ID         { get; set;}
      public long ID_Opl     { get; set;}
      public int ID_Sanat   { get; set;}
      public string Num_Put    { get; set;}
      public decimal Symma      { get; set;}
      public long ID_Pr      { get; set;}
      public int Priz       { get; set;}
      public long UserId     { get; set;}
    }
}