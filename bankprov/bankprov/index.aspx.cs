using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Xml;

namespace bankprov
{
    public partial class index : System.Web.UI.Page
    {
        public string anv = "jahy1400";
        public bool provledare = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            arlinsensierad();
        }

        public void arlinsensierad()
        {
           if (anv == "jahy1400" && provledare == true)
            {
                btnGorProv.Visible = true;
                btnSeResultat.Visible = true;
                btnSeResultatAnstallda.Visible = true;
                LabelEjInloggad.Visible = false;

            }

           else if (anv == "jahy1400" && provledare == false)
           {
               btnGorProv.Visible = true;
               btnSeResultat.Visible = true;
               btnSeResultatAnstallda.Visible = false;
               LabelEjInloggad.Visible = false;

           }

           else
           {
               btnGorProv.Visible = false;
               btnSeResultat.Visible = false;
               btnSeResultatAnstallda.Visible = false;
               LabelEjInloggad.Visible = true;
               LabelEjInloggad.Text = "Du måste vara inloggad för att använda kometensportalen";
           }

        }

        protected void btnGorProv_Click(object sender, EventArgs e)
        {
            btnGorProv.Visible = false;
            btnSeResultat.Visible = false;
            btnSeResultatAnstallda.Visible = false;
            LabelEjInloggad.Visible = false;
            LabelKompetensportal.Visible = false;

            HamtaFragor();
        }

        public void HamtaFragor()
        {

        }
    }


}


