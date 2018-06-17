using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Viloyats.Common.Mapping.Sanatory
{
    public class SanatoryMapper
    {
        //1	    Чимион              17  - 22
        //10	Умид Гульшани       29  - 11
        //12	Чартак              16  - 21
        //14	Ак-Таш              3   -  13
        //21	Чаткал              41  -  48
        //29	Зоамин              6   -  16
        //3	    Чинабад             18  - 23
        //34	Зангиата Зам Зам    56  - 45
        //37	Касансай            7   - 17
        //4	    Турон               14  - 20
        //44	Ховотог             15  - 28 
        //5	    Ботаника            4   - 14
        //6	    Бустон              5   - 15
        //8	    Хумсан              40  - 50 
        //9	    Ситора-Мохи-Хоса    13  - 19

        private Dictionary<string, int> Mapp;

        public SanatoryMapper()
        {
            Mapp = new Dictionary<string, int>();
            Map();
        }

        public void Map()
        {
            Mapp.Add("1"  , 17); //Чимион           
            Mapp.Add("10" , 29); //Умид Гульшани    
            Mapp.Add("12" , 16); //Чартак           
            Mapp.Add("14" , 3 ); //Ак-Таш           
            Mapp.Add("21" , 41); //Чаткал           
            Mapp.Add("29" , 6 ); //Зоамин           
            Mapp.Add("3"  , 18); //Чинабад          
            Mapp.Add("34" , 56); //Зангиата Зам Зам 
            Mapp.Add("37" , 7 ); //Касансай         
            Mapp.Add("4"  , 14); //Турон            
            Mapp.Add("44" , 15); //Ховотог          
            Mapp.Add("5"  , 4 ); //Ботаника         
            Mapp.Add("6"  , 5 ); //Бустон           
            Mapp.Add("8"  , 40); //Хумсан           
            Mapp.Add("9"  , 13); //Ситора-Мохи-Хоса 
        }

        public int GetSanatId(string migSanatId)
        {
            int value;

            if (Mapp.TryGetValue(migSanatId, out value))
            {
                return value;
            }

            return -1;
        }
    }
}
