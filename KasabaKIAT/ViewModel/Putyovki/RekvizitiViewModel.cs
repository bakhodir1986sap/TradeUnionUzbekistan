using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Putyovki
{
    public class RekvizitiViewModel
    {
        public string ID_Prof   {get; set;}
        public string ID_Name   {get; set;}
        public string ID_Pred   {get; set;}
        public string ID_Pass   {get; set;}
        public string Adress    {get; set;}
        public string RS        {get; set;}
        public string Name_bank {get; set;}
        public string MFO       {get; set;}
        public string INN       {get; set;}
        public string OKONX     {get; set;}
        public string Name_Dir  {get; set;}
        public string PutDay    {get; set;}
        public bool VIO { get; set; }

        public string DepartmentBase { get; set; }
    }
}