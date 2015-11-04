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
        public int antalrätt { get; set; }
        public char facit1 { get; set; }
        public char facit2 { get; set; }
        public char facit3 { get; set; }
        public char facit4 { get; set; }
        public string info { get; set; }

    }
}