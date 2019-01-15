using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VipiskaParser.Model
{
    public class DebitPredefinedModel
    {
        public string Name { get; set; }

        public string Account { get; set; }


    }

    public class DebitPredefined
    {
        public List<DebitPredefinedModel> LDebitData { get; set; }

        public DebitPredefined()
        {
            LDebitData = new List<DebitPredefinedModel>();

            var model1 = new DebitPredefinedModel();
            model1.Name = "Ўзбекистон КУФК Қуёшли БСО";
            model1.Account = "20212000400272967001";
            LDebitData.Add(model1);

            var model2 = new DebitPredefinedModel();
            model2.Name = "Спорт мактаб";
            model2.Account = "20212000400887968003";
            LDebitData.Add(model2);

            var model3 = new DebitPredefinedModel();
            model3.Name = "ПрофБюджет ";
            model3.Account = "20212000200431324001";
            LDebitData.Add(model3);

            var model4 = new DebitPredefinedModel();
            model4.Name = "УКС ";
            model4.Account = "20210000300103379004";
            LDebitData.Add(model4);
        }
    }
}
