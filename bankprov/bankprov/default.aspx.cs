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
        //////public string anv = "jahy1400";
        //////public bool provledare = true;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public static int GetPersonId(string anvandare)
        {

            string connectionString = "Server=webblabb.miun.se; Port=5432; Database=pgmvaru_g8; User Id=pgmvaru_g8; Password=rockring; SslMode=Require";
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            int person_id = 0;
            try
            {
                conn.Open();
                string sql = "SELECT id FROM u4_konto WHERE anvandarnamn = @anvandare;";
                NpgsqlCommand command = new NpgsqlCommand(sql, conn);
                command.Parameters.AddWithValue("anvandare", anvandare);

                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    person_id = Convert.ToInt32(dr["id"]);
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
            return person_id;
        }

        public static bool ArLicensierad(int fk_person_id)
        {
            string connectionString = "Server=webblabb.miun.se; Port=5432; Database=pgmvaru_g8; User Id=pgmvaru_g8; Password=rockring; SslMode=Require;";
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            bool linsensierad = false;
            try
            {
                conn.Open();
                string sql = "SELECT linsensierad FROM u4_person WHERE id = @fk_person_id;";
                NpgsqlCommand command = new NpgsqlCommand(sql, conn);
                command.Parameters.AddWithValue("fk_person_id", fk_person_id);
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    linsensierad = (bool)(dr["linsensierad"]);
                }
            }
            catch (NpgsqlException ex)
            {
            }
            finally
            {
                conn.Close();
            }
            return linsensierad;
        }

        protected void btnGorProv_Click(object sender, EventArgs e)
        {
            string anvandare = TextBoxanvandare.Text;
            int person_id = 1;
            person_id = GetPersonId(anvandare);

            if (ArLicensierad(person_id) == true)
            {
                HamtaFragor();
            }
            else
            {
                //öppna sidan för linsensiering

            }

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
            int checkboxkontroll;

            int i = -1;
            int j = 0;

            foreach (RepeaterItem item in Repeater1.Items) // loopar genom alla objekt i repeatern
            {
                i++;
                checkboxkontroll = 0;

                fraga fragaobj = new fraga();
                fragaobj.nr = i;
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem) 
                {
                    var checkBoxA = (CheckBox)item.FindControl("CheckBoxA"); 
                    if (checkBoxA.Checked == true)
                    {
                        fragaobj.svarsalternativa = provet.fragelista[i].svarsalternativa;
                        checkboxkontroll++;
                    }

                    var checkBoxB = (CheckBox)item.FindControl("CheckBoxB");
                    if (checkBoxB.Checked == true)
                    {
                        fragaobj.svarsalternativb = provet.fragelista[i].svarsalternativb;
                        checkboxkontroll++;

                    }

                    var checkBoxC = (CheckBox)item.FindControl("CheckBoxC");
                    if (checkBoxC.Checked == true)
                    {
                        fragaobj.svarsalternativc = provet.fragelista[i].svarsalternativc;
                        checkboxkontroll++;

                    }

                    var checkBoxD = (CheckBox)item.FindControl("CheckBoxD");
                    if (checkBoxD.Checked == true)
                    {
                        fragaobj.svarsalternativd = provet.fragelista[i].svarsalternativd;
                        checkboxkontroll++;

                    }
                }
                fragaobj.info = checkboxkontroll.ToString(); 
                gjortprov.Add(fragaobj); // lägger till svaret i en lista
            }

            SerializaSvar(gjortprov); // ropar på serializern
            RattaProv(gjortprov);

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
            int flersvarsfraga = 0;

            foreach (object objekt in gjortprov)
            {   
                i++;

                if (gjortprov[i].info != facit.fragelista[i].info)
                {
                    //För många eller för få alternativ kryssade. Skickar vidare till nästa fråga
                }

                else
                {                
                    if (gjortprov[i].svarsalternativa == facit.fragelista[i].svarsalternativa && gjortprov[i].svarsalternativa != null)
                    {
                        if (Convert.ToInt32(facit.fragelista[i].info) == 1)
                        {
                            resultat++;
                        }

                        else if (Convert.ToInt32(facit.fragelista[i].info) > 1)
                        {
                            flersvarsfraga++;
                        }
                    }

                    if (gjortprov[i].svarsalternativb == facit.fragelista[i].svarsalternativb && gjortprov[i].svarsalternativb != null)
                    {
                        if (Convert.ToInt32(facit.fragelista[i].info) == 1)
                        {
                            resultat++;
                        }
    
                        else if (Convert.ToInt32(facit.fragelista[i].info) > 1)
                        {
                            flersvarsfraga++;
                        }

                        if (flersvarsfraga == Convert.ToInt32(facit.fragelista[i].info))
                        {
                            resultat++;
                        }
                    }

                    if (gjortprov[i].svarsalternativc == facit.fragelista[i].svarsalternativc && gjortprov[i].svarsalternativc != null)
                    {
                        if (Convert.ToInt32(facit.fragelista[i].info) == 1)
                        {
                            resultat++;
                        }

                        else if (Convert.ToInt32(facit.fragelista[i].info) > 1)
                        {
                            flersvarsfraga++;
                        }

                        if (flersvarsfraga == Convert.ToInt32(facit.fragelista[i].info))
                        {
                            resultat++;
                        }
                    }

                    if (gjortprov[i].svarsalternativd == facit.fragelista[i].svarsalternativd && gjortprov[i].svarsalternativd != null)
                    {
                        if (Convert.ToInt32(facit.fragelista[i].info) == 1)
                        {
                            resultat++;
                        }

                        else if (Convert.ToInt32(facit.fragelista[i].info) > 1)
                        {
                            flersvarsfraga++;
                        }

                        if (flersvarsfraga == Convert.ToInt32(facit.fragelista[i].info))
                        {
                            resultat++;
                        }
                    }
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

