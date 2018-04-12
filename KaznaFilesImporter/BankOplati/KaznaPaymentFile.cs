using KaznaFilesImporter.BankOplati;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.BankOplati
{
    public class KaznaPaymentFile : BaseKaznaFile
    {
        public string k_type           {get; set;}//Always 1
        public string k_operday        {get; set;} //ddmmyyyy
        public string k_id             {get; set;}
        public string k_bankid         {get; set;}
        public string k_grpid { get; set; } //New Field
        public string k_klsdox         {get; set;}
        public string k_docnumb        {get; set;}
        public string k_docdate        {get; set; } //ddmmyyyy
        public string k_doctype        {get; set;}
        public string k_sumpay         {get; set;} //Amount * 100 = tiyins
        public string k_clmfo          {get; set;}
        public string k_clacc          {get; set;}
        public string k_clinn          {get; set;}
        public string k_clname         {get; set;}
        public string k_comfo          {get; set;}
        public string k_coacc          {get; set;}
        public string k_coinn          {get; set;}
        public string k_coname         {get; set;}
        public string k_purpose        {get; set;}
        public string k_chapter        {get; set;}
        public string k_okonh          {get; set;}

        public long file_id { get; set; }

    }
}