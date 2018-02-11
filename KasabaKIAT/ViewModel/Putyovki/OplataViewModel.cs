using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Putyovki
{
    public class OplataViewModel
    {
      public long? ID         {get; set;}
      public DateTime Data_Post  {get; set;}
      public string Num_Doc    {get; set;}
      public long ID_Otras   {get; set;}
      public decimal Symma_DT   {get; set;}
      public decimal Symma_KT   {get; set;}
      public string Osnov      {get; set;}
      public int ID_KV      {get; set;}
      public int SmFil      {get; set;}
      public string Name_org   {get; set;}
      public bool Tip_Org    {get; set;}
      public int PrizOpl    {get; set;}
      public long UserId     {get; set;}
    }
}