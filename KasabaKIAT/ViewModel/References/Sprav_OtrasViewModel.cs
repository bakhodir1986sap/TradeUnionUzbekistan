using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Utils.References
{
    public class Sprav_OtrasViewModel
    {
        public long ID { get; set; }
        public int?    ID_Ot    {get; set;}

        public string Name_Ot  {get; set;}

        public int?    Priz     {get; set;}
        public long?   UserId   {get; set;}

        public string DepartmentBase { get; set; }
    }
}