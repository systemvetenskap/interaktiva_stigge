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
using System.Xml.Serialization;
using System.IO;

namespace bankprov
{
    public partial class index : System.Web.UI.Page
    {
        public string anv = "jahy1400";
        public bool provledare = true;


        protected void Page_Load(object sender, EventArgs e)
        {
            btnLamnain.Visible = false;
            arlinsensierad();
            //LiteralFraga.Visible = false;
            //LabelA.Visible = false;
            //LabelB.Visible = false;
            //LabelC.Visible = false;
            //LabelD.Visible = false;
            //RadioButtonA.Visible = false;
            //RadioButtonB.Visible = false;
            //RadioButtonC.Visible = false;
            //RadioButtonD.Visible = false;


        }

        public void arlinsensierad()
        {
            if (anv == "jahy1400" && provledare == true)
            {
                btnGorProv.Visible = true;
                btnSeResultat.Visible = true;
                btnSeResultatAnstallda.Visible = true;
                LabelEjInloggad.Visible = false;
                btnLamnain.Visible = false;


            }

            else if (anv == "jahy1400" && provledare == false)
            {
                btnGorProv.Visible = true;
                btnSeResultat.Visible = true;
                btnSeResultatAnstallda.Visible = false;
                LabelEjInloggad.Visible = false;
                btnLamnain.Visible = false;


            }

            else
            {
                btnGorProv.Visible = false;
                btnSeResultat.Visible = false;
                btnSeResultatAnstallda.Visible = false;
                LabelEjInloggad.Visible = true;
                LabelEjInloggad.Text = "Du måste vara inloggad för att använda kompetensportalen";
                btnLamnain.Visible = false;

            }

        }

        protected void btnGorProv_Click(object sender, EventArgs e)
        {
            btnGorProv.Visible = false;
            btnSeResultat.Visible = false;
            btnSeResultatAnstallda.Visible = false;
            LabelEjInloggad.Visible = false;
            LabelKompetensportal.Visible = false;
            btnLamnain.Visible = true;


            HamtaFragor();

            //LiteralFraga.Visible = true;
            //LabelA.Visible = true;
            //LabelB.Visible = true;
            //LabelC.Visible = true;
            //LabelD.Visible = true;
            //RadioButtonA.Visible = true;
            //RadioButtonB.Visible = true;
            //RadioButtonC.Visible = true;
            //RadioButtonD.Visible = true;


        }

        public void HamtaFragor()
        {
            string xml = Server.MapPath("test.xml");

            XmlSerializer deserializer = new XmlSerializer(typeof(prov));
            TextReader reader = new StreamReader(xml);
            object obj = deserializer.Deserialize(reader);
            prov XmlData = (prov)obj;
            reader.Close();



            Repeater1.DataSource = XmlData.fragelista;
            Repeater1.DataBind();

        }

        protected void btnLamnain_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in Repeater1.Items)
            {
                // Checking the item is a data item
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var rdbList = item.FindControl("RadioButtonList1") as RadioButtonList;
                    // Get the selected value
                    string selected = rdbList.SelectedValue;
                }
            }
        }




    }
}

