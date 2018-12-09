using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Utils.References
{
    public class Years
    {
        public int Id { get; set; }

        public int Year { get; set; }

        public string year_str { get; set; }

        public string year_id { get; set; }
    }

    public class RefYars
    {
        public List<Years> GetYears()
        {
            var list = new List<Years>();

            list.Add(new Years() { Id = 2017, Year = 2017, year_str ="2017",year_id ="2017"   });
            list.Add(new Years() { Id = 2018, Year = 2018, year_str ="2018",year_id ="2018"   });
            list.Add(new Years() { Id = 2019, Year = 2019, year_str ="2019",year_id ="2019"   });
            list.Add(new Years() { Id = 2020, Year = 2020, year_str ="2020",year_id ="2020"   });
            list.Add(new Years() { Id = 2021, Year = 2021, year_str ="2021",year_id ="2021"   });
            list.Add(new Years() { Id = 2022, Year = 2022, year_str ="2022",year_id = "2022" });

            return list;
        }


    }
}