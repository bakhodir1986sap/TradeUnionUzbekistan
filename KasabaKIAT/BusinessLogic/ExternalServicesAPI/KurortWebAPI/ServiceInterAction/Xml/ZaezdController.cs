using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace KurortUrvServiceTest.ServiceInterAction.Xml
{
    public class ZaezdController
    {
        private bool _DEBUG = true;

        public string Process(TypeRaspredelenieZaezdov xmlCl)
        {
            if (_DEBUG) //Test
            {
                xmlCl = new TypeRaspredelenieZaezdov();

                xmlCl.StrokiRaspredelenyaZaezdov = new List<StrokiRaspredelenyaZaezdov>();

                StrokiRaspredelenyaZaezdov obj1 = new StrokiRaspredelenyaZaezdov();
                obj1.SanatoryKod = "12";
                obj1.ProfsoyzKod = "000000035";
                obj1.SanatoryRazmeshenieKod = "000000111";
                obj1.Putevok1kv = "20";
                obj1.Putevok2kv = "30";
                obj1.Putevok3kv = "50";
                obj1.Putevok4kv = "40";

                xmlCl.StrokiRaspredelenyaZaezdov.Add(obj1);

                StrokiRaspredelenyaZaezdov obj2 = new StrokiRaspredelenyaZaezdov();
                obj2.SanatoryKod = "12";
                obj2.ProfsoyzKod = "000000035";
                obj2.SanatoryRazmeshenieKod = "000000112";
                obj2.Putevok1kv = "20";
                obj2.Putevok2kv = "30";
                obj2.Putevok3kv = "50";
                obj2.Putevok4kv = "40";

                xmlCl.StrokiRaspredelenyaZaezdov.Add(obj2);

                xmlCl.GodZaezdov = "2018";
            }

            var res = SerializeToString(xmlCl);

            return res;
        }

        private string SerializeToString(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            using (StringWriterWithEncoding writer = new StringWriterWithEncoding(Encoding.UTF8))
            {
                serializer.Serialize(writer, obj);

                return writer.ToString();
            }
        }

        private sealed class StringWriterWithEncoding : StringWriter
        {
            public override Encoding Encoding { get; }

            public StringWriterWithEncoding(Encoding encoding)
            {
                Encoding = encoding;
            }
        }

        private sealed class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}
