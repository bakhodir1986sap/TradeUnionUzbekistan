using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel.Putyovki
{
    public class PostPutyovkaViewModel
    {
        public long             Id              { get; set; }

        public long?            IdPost          { get; set; }

        public int?            IdSanat         { get; set; }

        public string           SanatName       { get; set; }

        public string           Num_Put         { get; set; }

        public DateTime?        DataSan         { get; set; }

        public DateTime?        DataSanEnd      { get; set; }

        public decimal?         Symma_Put       { get; set; }

        public bool             ID_PolVoz       { get; set; }

        public DateTime?        Data_Post       { get; set; }

        public long?            UserId          { get; set; }

        public string           Departmentbase  { get; set; }

        public bool             Checked         { get; set; }
    }
}