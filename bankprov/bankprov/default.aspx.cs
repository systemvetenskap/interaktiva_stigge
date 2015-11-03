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
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("Kategori");
        //    dt.Columns.Add("Frågestallning");
        //    dt.Columns.Add("Svarsalternativ a");
        //    dt.Columns.Add("Svarsalternativ b");
        //    dt.Columns.Add("Svarsalternativ c");
        //    dt.Columns.Add("Svarsalternativ d");
        //    dt.Columns.Add("Info d");

        //    dt.Rows.Add();

        //    string xml = Server.MapPath("test.xml");

        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(xml);

        //    XmlNodeList aktiekurser = doc.SelectNodes("prov/fraga");

        //    foreach (XmlNode nod in aktiekurser)
        //    {
        //        dt.Rows.Add(nod["kategori"].InnerText, nod["fragestallning"].InnerText, nod["svarsalternativa"].InnerText, nod["svarsalternativb"].InnerText, nod["svarsalternativc"].InnerText, nod["svarsalternativd"].InnerText, nod["info"].InnerText);
                
        //    }
        //    LiteralKategori.Text = dt.Rows[1][0].ToString();
        //    LiteralFraga.Text = dt.Rows[1][1].ToString();
        //    LabelA.Text = dt.Rows[1][2].ToString();
        //    LabelB.Text = dt.Rows[1][3].ToString();
        //    LabelC.Text = dt.Rows[1][4].ToString();
        //    LabelD.Text = dt.Rows[1][5].ToString();



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

        }



    }
}

