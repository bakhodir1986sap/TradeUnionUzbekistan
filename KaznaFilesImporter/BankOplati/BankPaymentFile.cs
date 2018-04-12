using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaznaFilesImporter.BankOplati
{
    public class BankPaymentFile : BaseKaznaFile
    {
        public string   type              {get; set;}
        public string   operday   {get; set; } //ддммуууу
        public string   id        {get; set;}
        public string   grpid     {get; set;}
        public string   klsdox    {get; set;}
        public string   docnumb   {get; set;}
        public string   docdate   {get; set; } //ддммуууу
        public string   doctype   {get; set;}
        public string   sumpay    {get; set;}
        public string   clmfo     {get; set;}
        public string   clacc     {get; set;}
        public string   clinn     {get; set;}
        public string   clname    {get; set;}
        public string   comfo     {get; set;}
        public string   coacc     {get; set;}
        public string   coinn     {get; set;}
        public string   coname    {get; set;}
        public string   purpose { get; set; }
        public long file_id { get; set; }

    }
}
