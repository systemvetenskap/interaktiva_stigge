using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace bankprov
{
    public class prov

        {
            [XmlElement("fraga")]
            public List<fraga> fragelista = new List<fraga>();      // Skapar en lista av frågeobjekt
        }

    public class fraga      //Frågeobjekt
    {
        public int nr { get; set; }
        public string kategori { get; set; }
        public string fragestallning { get; set; }
        public string svarsalternativa { get; set; }
        public string svarsalternativb { get; set; }
        public string svarsalternativc { get; set; }
        public string svarsalternativd { get; set; }
        public string info { get; set; }

    }
}