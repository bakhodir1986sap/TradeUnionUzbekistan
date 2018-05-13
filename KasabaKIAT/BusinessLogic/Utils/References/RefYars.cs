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
    }

    public class RefYars
    {
        public List<Years> GetYears()
        {
            var list = new List<Years>();

            list.Add(new Years() { Id = 2017, Year = 2017 });
            list.Add(new Years() { Id = 2018, Year = 2018 });
            list.Add(new Years() { Id = 2019, Year = 2019 });
            list.Add(new Years() { Id = 2020, Year = 2020 });
            list.Add(new Years() { Id = 2021, Year = 2021 });
            list.Add(new Years() { Id = 2022, Year = 2022 });

            return list;
        }


    }
}