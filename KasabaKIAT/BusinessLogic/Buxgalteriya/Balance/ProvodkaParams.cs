using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Buxgalteriya.Balance
{
    public class ProvodkaParams
    {
        public ProvodkaParams()
        {
            details = new List<ProvodkaDetails>();
            master = new ProvodkaMaster();
        }

        public List<ProvodkaDetails> details { get; set; }

        public ProvodkaMaster master { get; set; }
    }

    public class ProvodkaDetails
    {
        public string trdt                 {get; set;}
        public string department_code      {get; set;}
        public int dyseq                {get; set;}
        public int dytrseq              {get; set;}
        public string account              {get; set;}
        public string drcr                 {get; set;}
        public string ccy                  {get; set;}
        public decimal amount               {get; set;}
        public string remark               {get; set;}
        public long crtuser_id           {get; set;}
        public long rec_id               {get; set;} //AutoGenerate
        public string inn { get; set; } //AutoGenerate

    }

    public class ProvodkaMaster
    {
      public string trdt               {get; set;}
      public string department_base    {get; set;}
      public int    dyseq              {get; set;}
      public int    oper_id            {get; set;}
      public string trtp               {get; set;}
      public string orgtrdt            {get; set;}
      public int orgorg_id          {get; set;}
      public long orgcrt_usr_id      {get; set;}
      public int orgdyseq           {get; set;}
      public string remark             {get; set;}
      public long crtuser_id         {get; set;}
      public long rec_id             {get; set;} //Autogeneration
    }
}