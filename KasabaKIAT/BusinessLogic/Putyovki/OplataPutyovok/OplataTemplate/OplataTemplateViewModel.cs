using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.OplataPutyovok.OplataTemplate
{
    public class OplataTemplateViewModel
    {
        //INN 
        //NAIMENOVANIE    
        //NUM_SCETFAK 
        //SANATORIY_CODE  
        //NOMER_PUTYOVKI 
        //SUMMA_PUYOVKI   
        //PROCENT 
        //OPLACHENO   
        //DATA_OPLATI

        public string inn { get; set; }
        public string naimenovanie { get; set; }
        public string num_scetfak { get; set; }
        public int sanatoriy_code { get; set; }
        public string nomer_putyovki { get; set; }
        public decimal summa_puyovki { get; set; }
        public int procent { get; set; }
        public decimal oplacheno { get; set; }
        public DateTime data_oplati { get; set; } //dd.MM.yyyy
    }
}