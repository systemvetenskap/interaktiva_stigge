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
            prov laddatprov = (prov)obj;
            reader.Close();

            Repeater1.DataSource = laddatprov.fragelista;
            Repeater1.DataBind();
        }

        protected void btnLamnain_Click(object sender, EventArgs e)
        {
            prov provet = new prov();

            provet = HamtaFragor2();
            HittaSvar(provet);
        }

        public prov HamtaFragor2()
        {
            string xml = Server.MapPath("test.xml");

            XmlSerializer deserializer = new XmlSerializer(typeof(prov));
            TextReader reader = new StreamReader(xml);
            object obj = deserializer.Deserialize(reader);
            prov laddatprov = (prov)obj;
            reader.Close();

            return laddatprov;

        }

        public void HittaSvar(prov provet)
        {
            List <fraga> gjortprov = new List<fraga>();
            int i = -1;
            int j = 0;

            foreach (RepeaterItem item in Repeater1.Items) // loopar genom alla objekt i repeatern
            {
                i++;

                fraga fragaobj = new fraga();
                fragaobj.nr = i;
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem) 
                {
                    var checkBoxA = (CheckBox)item.FindControl("CheckBoxA"); 
                    if (checkBoxA.Checked == true)
                    {
                        fragaobj.svarsalternativa = provet.fragelista[i].svarsalternativa;
                    }

                    var checkBoxB = (CheckBox)item.FindControl("CheckBoxB");
                    if (checkBoxB.Checked == true)
                    {
                        fragaobj.svarsalternativb = provet.fragelista[i].svarsalternativb;
                    }

                    var checkBoxC = (CheckBox)item.FindControl("CheckBoxC");
                    if (checkBoxC.Checked == true)
                    {
                        fragaobj.svarsalternativc = provet.fragelista[i].svarsalternativc;
                    }

                    var checkBoxD = (CheckBox)item.FindControl("CheckBoxD");
                    if (checkBoxD.Checked == true)
                    {
                        fragaobj.svarsalternativd = provet.fragelista[i].svarsalternativd;
                    }
                }
                gjortprov.Add(fragaobj); // lägger till svaret i en lista
            }

            RattaProv(gjortprov);

            SerializaSvar(gjortprov); // ropar på serializern
        }

        public void RattaProv(List<fraga> gjortprov)
        {
            string xml = Server.MapPath("facittest.xml");

            XmlSerializer deserializer = new XmlSerializer(typeof(prov));
            TextReader reader = new StreamReader(xml);
            object obj = deserializer.Deserialize(reader);
            prov facit = (prov)obj;
            reader.Close();

            int i = -1;
            int resultat = 0;

            foreach (object objekt in gjortprov)
            {   
                i++;

                if (gjortprov[i].svarsalternativa == facit.fragelista[i].svarsalternativa && gjortprov[i].svarsalternativa != null)
                    {   
                    resultat++;
                    }

                if (gjortprov[i].svarsalternativb == facit.fragelista[i].svarsalternativb && gjortprov[i].svarsalternativb != null)
                    {   
                    resultat++;
                    }

                if (gjortprov[i].svarsalternativc == facit.fragelista[i].svarsalternativc && gjortprov[i].svarsalternativc != null)
                    {   
                    resultat++;
                    }

                if (gjortprov[i].svarsalternativd == facit.fragelista[i].svarsalternativd && gjortprov[i].svarsalternativd != null)
                    {   
                    resultat++;
                    }
            }

            LabelEjInloggad.Visible = true;
            LabelEjInloggad.Text = "Du fick " + resultat + "rätt";

        }

        public void SerializaSvar(List<fraga> svar)
        {
            string directory = Server.MapPath("svar.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(List<fraga>));
            using (TextWriter writer = new StreamWriter(directory))

            {
                serializer.Serialize(writer, svar);
            }
        }
        
    }
}

