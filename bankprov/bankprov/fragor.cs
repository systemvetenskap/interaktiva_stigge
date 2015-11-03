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
            public List<fraga> fragelista = new List<fraga>();
        }

    public class fraga
    {
        public int nr { get; set; }
        public string kategori { get; set; }
        public string fragestallning { get; set; }
        public string svarsalternativa { get; set; }
        public string svarsalternativb { get; set; }
        public string svarsalternativc { get; set; }
        public string svarsalternativd { get; set; }
        public bool facitA { get; set; }
        public bool facitB { get; set; }
        public bool facitC { get; set; }
        public bool facitD { get; set; }
        public string info { get; set; }

    }
}