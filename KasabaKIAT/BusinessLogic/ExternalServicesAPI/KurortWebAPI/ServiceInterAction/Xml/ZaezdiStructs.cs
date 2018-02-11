using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace KurortUrvServiceTest.ServiceInterAction.Xml
{
    [XmlRoot(ElementName = "StrokiRaspredelenyaZaezdov", Namespace = "http://www.sku-tradeunion.uz")]
    public class StrokiRaspredelenyaZaezdov
    {
        [XmlElement(ElementName = "SanatoryKod", Namespace = "http://www.sku-tradeunion.uz")]
        public string SanatoryKod { get; set; }
        [XmlElement(ElementName = "ProfsoyzKod", Namespace = "http://www.sku-tradeunion.uz")]
        public string ProfsoyzKod { get; set; }
        [XmlElement(ElementName = "SanatoryRazmeshenieKod", Namespace = "http://www.sku-tradeunion.uz")]
        public string SanatoryRazmeshenieKod { get; set; }
        [XmlElement(ElementName = "Putevok1kv", Namespace = "http://www.sku-tradeunion.uz")]
        public string Putevok1kv { get; set; }
        [XmlElement(ElementName = "Putevok2kv", Namespace = "http://www.sku-tradeunion.uz")]
        public string Putevok2kv { get; set; }
        [XmlElement(ElementName = "Putevok3kv", Namespace = "http://www.sku-tradeunion.uz")]
        public string Putevok3kv { get; set; }
        [XmlElement(ElementName = "Putevok4kv", Namespace = "http://www.sku-tradeunion.uz")]
        public string Putevok4kv { get; set; }
    }

    [XmlRoot(ElementName = "typeRaspredelenieZaezdov", Namespace = "http://www.sku-tradeunion.uz")]
    public class TypeRaspredelenieZaezdov
    {
        [XmlElement(ElementName = "StrokiRaspredelenyaZaezdov", Namespace = "http://www.sku-tradeunion.uz")]
        public List<StrokiRaspredelenyaZaezdov> StrokiRaspredelenyaZaezdov { get; set; }
        [XmlElement(ElementName = "GodZaezdov", Namespace = "http://www.sku-tradeunion.uz")]
        public string GodZaezdov { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "xs", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xs { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
    }

}
