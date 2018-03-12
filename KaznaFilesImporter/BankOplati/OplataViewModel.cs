using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel.BankOplati
{
    public class OplataViewModel
    {
      public string inn        {get; set;}
      public string namefir    {get; set;}
      public DateTime datapos    {get; set;}
      public decimal debet      {get; set;}
      public decimal symotches  {get; set;}
      public string info       {get; set;}
      public string numdoc     {get; set;}
      public int period     {get; set;}
      public int region     {get; set;}
      public int otrasel    {get; set;}
      public long id         {get; set;}
      public long userid     {get; set;}
    }
}