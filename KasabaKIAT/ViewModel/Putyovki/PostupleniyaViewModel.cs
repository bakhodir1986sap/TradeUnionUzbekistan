using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel
{
    public class PostupleniyaViewModel
    {

        public long         Id              { get; set; }

        public int?         Id_Kv           { get; set; }

        public string       Kvartal         { get; set; }

        public DateTime?    Data_Post       { get; set; }

        public string       Num_Doc         { get; set; }

        public long?        ID_Pos          { get; set; }

        public string       PosName         { get; set; }

        public int?         Num_Kol         { get; set; }

        public decimal?     Symma           { get; set; }

        public string       Name_Dov        { get; set; }

        public bool         Vid             { get; set; }

        public int?         SmFil           { get; set; } 

        public long?        UserId          { get; set; }

        public string       DepartmentBase  { get; set; }

        public int?         BeginNumber     { get; set; }

        public int?         EndNumber       { get; set; }

    }
}