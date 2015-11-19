using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;


namespace bankprov
{
    public class Anvandare
    {
        public int person_id;
        public string fnamn;
        public string enamn;

        public override string ToString()
        {
            return fnamn + " " + enamn;
        }
    }
}