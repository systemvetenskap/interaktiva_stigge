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
        protected void Page_Load(object sender, EventArgs e)
        {
            btnLamnain.Visible = false;
            LabelEjInloggad.Visible = false;      
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

        protected void btnGorProv_Click(object sender, EventArgs e)// inlågning 
        {
            string anvandare = TextBoxanvandare.Text;
            int person_id = 1;
            person_id = GetPersonId(anvandare);

            if (ArLicensierad(person_id) == true)
            {
                HamtaFragor();
                btnGorProv.Visible = false;
                btnSeResultat.Visible = false;
                btnSeResultatAnstallda.Visible = false;
                LabelEjInloggad.Visible = false;
                TextBoxanvandare.Visible = false;
                LabelKompetensportal.Visible = false;
                Labelfornam.Visible = false;
                btnLamnain.Visible = true;
            }
            else
            {
                //öppna sidan för linsensiering

            }

        }



    public void HamtaFragor()
        {
            string xml = Server.MapPath("fragor.xml");

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
            DoljKontroller();
        }

        public prov HamtaFragor2()
        {
            string xml = Server.MapPath("fragor.xml");

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

                        var LabelA = (Label)item.FindControl("LabelA"); // Alla svar som man svarat blir röda, de korrekta ändras sedan till gröna i VisaSvar()
                        LabelA.CssClass = "felsvar";
                    }

                    var checkBoxB = (CheckBox)item.FindControl("CheckBoxB");
                    if (checkBoxB.Checked == true)
                    {
                        fragaobj.svarsalternativb = provet.fragelista[i].svarsalternativb;
                        checkboxkontroll++;

                        var LabelB = (Label)item.FindControl("LabelB");
                        LabelB.CssClass = "felsvar";

                    }

                    var checkBoxC = (CheckBox)item.FindControl("CheckBoxC");
                    if (checkBoxC.Checked == true)
                    {
                        fragaobj.svarsalternativc = provet.fragelista[i].svarsalternativc;
                        checkboxkontroll++;

                        var LabelC = (Label)item.FindControl("LabelC");
                        LabelC.CssClass = "felsvar";

                    }

                    var checkBoxD = (CheckBox)item.FindControl("CheckBoxD");
                    if (checkBoxD.Checked == true)
                    {
                        fragaobj.svarsalternativd = provet.fragelista[i].svarsalternativd;
                        checkboxkontroll++;

                        var LabelD = (Label)item.FindControl("LabelD");
                        LabelD.CssClass = "felsvar";

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
            string xml = Server.MapPath("facit.xml");

            XmlSerializer deserializer = new XmlSerializer(typeof(prov));
            TextReader reader = new StreamReader(xml);
            object obj = deserializer.Deserialize(reader);
            prov facit = (prov)obj;
            reader.Close();

            int i = -1;
            int resultat = 0;
            int flersvarsfraga;
            int produkterochhanteringavkundensaffärer = 0;
            int ekonominationalekonomifinansiellekonomiochprivatekonomi = 0;
            int etikochregelverk = 0;

            foreach (object objekt in gjortprov)
            {   
                flersvarsfraga = 0;
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
                            if (facit.fragelista[i].kategori.ToString() == "Produkter och hantering av kundens affärer")
                            {                                
                            resultat++;
                                produkterochhanteringavkundensaffärer++;
                        }

                            if (facit.fragelista[i].kategori.ToString() == "Ekonomi – nationalekonomi, finansiell ekonomi och privatekonomi")
                            {
                                resultat++;
                                ekonominationalekonomifinansiellekonomiochprivatekonomi++;
                            }

                            if (facit.fragelista[i].kategori.ToString() == "Etik och regelverk")
                            {
                                resultat++;
                                etikochregelverk++;
                            }
                        }

                        else if (Convert.ToInt32(facit.fragelista[i].info) > 1)
                        {
                            if (facit.fragelista[i].kategori.ToString() == "Produkter och hantering av kundens affärer")
                            {
                                resultat++;
                                produkterochhanteringavkundensaffärer++;
                            }

                            if (facit.fragelista[i].kategori.ToString() == "Ekonomi – nationalekonomi, finansiell ekonomi och privatekonomi")
                            {
                                resultat++;
                                ekonominationalekonomifinansiellekonomiochprivatekonomi++;
                            }

                            if (facit.fragelista[i].kategori.ToString() == "Etik och regelverk")
                            {
                                resultat++;
                                etikochregelverk++;
                            }
                        }
                    }

                    if (gjortprov[i].svarsalternativb == facit.fragelista[i].svarsalternativb && gjortprov[i].svarsalternativb != null)
                    {
                        if (Convert.ToInt32(facit.fragelista[i].info) == 1)
                        {
                            if (facit.fragelista[i].kategori.ToString() == "Produkter och hantering av kundens affärer")
                            {
                                resultat++;
                                produkterochhanteringavkundensaffärer++;
                            }

                            if (facit.fragelista[i].kategori.ToString() == "Ekonomi – nationalekonomi, finansiell ekonomi och privatekonomi")
                            {
                                resultat++;
                                ekonominationalekonomifinansiellekonomiochprivatekonomi++;
                            }

                            if (facit.fragelista[i].kategori.ToString() == "Etik och regelverk")
                            {
                            resultat++;
                                etikochregelverk++;
                            }
                        }
    
                        else if (Convert.ToInt32(facit.fragelista[i].info) > 1)
                        {
                            flersvarsfraga++;
                        }

                        if (flersvarsfraga == Convert.ToInt32(facit.fragelista[i].info))
                        {
                            if (facit.fragelista[i].kategori.ToString() == "Produkter och hantering av kundens affärer")
                            {
                                resultat++;
                                produkterochhanteringavkundensaffärer++;
                            }

                            if (facit.fragelista[i].kategori.ToString() == "Ekonomi – nationalekonomi, finansiell ekonomi och privatekonomi")
                            {
                            resultat++;
                                ekonominationalekonomifinansiellekonomiochprivatekonomi++;
                            }

                            if (facit.fragelista[i].kategori.ToString() == "Etik och regelverk")
                            {
                                resultat++;
                                etikochregelverk++;
                            }
                        }
                    }

                    if (gjortprov[i].svarsalternativc == facit.fragelista[i].svarsalternativc && gjortprov[i].svarsalternativc != null)
                    {
                        if (Convert.ToInt32(facit.fragelista[i].info) == 1)
                        {
                            if (facit.fragelista[i].kategori.ToString() == "Produkter och hantering av kundens affärer")
                            {
                                resultat++;
                                produkterochhanteringavkundensaffärer++;
                            }

                            if (facit.fragelista[i].kategori.ToString() == "Ekonomi – nationalekonomi, finansiell ekonomi och privatekonomi")
                            {
                            resultat++;
                                ekonominationalekonomifinansiellekonomiochprivatekonomi++;
                            }

                            if (facit.fragelista[i].kategori.ToString() == "Etik och regelverk")
                            {
                                resultat++;
                                etikochregelverk++;
                            }
                        }

                        else if (Convert.ToInt32(facit.fragelista[i].info) > 1)
                        {
                            flersvarsfraga++;
                        }

                        if (flersvarsfraga == Convert.ToInt32(facit.fragelista[i].info))
                        {
                            if (facit.fragelista[i].kategori.ToString() == "Produkter och hantering av kundens affärer")
                            {
                                resultat++;
                                produkterochhanteringavkundensaffärer++;
                            }

                            if (facit.fragelista[i].kategori.ToString() == "Ekonomi – nationalekonomi, finansiell ekonomi och privatekonomi")
                            {
                            resultat++;
                                ekonominationalekonomifinansiellekonomiochprivatekonomi++;
                            }

                            if (facit.fragelista[i].kategori.ToString() == "Etik och regelverk")
                            {
                                resultat++;
                                etikochregelverk++;
                            }
                        }
                    }

                    if (gjortprov[i].svarsalternativd == facit.fragelista[i].svarsalternativd && gjortprov[i].svarsalternativd != null)
                    {
                        if (Convert.ToInt32(facit.fragelista[i].info) == 1)
                        {
                            if (facit.fragelista[i].kategori.ToString() == "Produkter och hantering av kundens affärer")
                            {
                                resultat++;
                                produkterochhanteringavkundensaffärer++;
                            }

                            if (facit.fragelista[i].kategori.ToString() == "Ekonomi – nationalekonomi, finansiell ekonomi och privatekonomi")
                            {
                            resultat++;
                                ekonominationalekonomifinansiellekonomiochprivatekonomi++;
                            }

                            if (facit.fragelista[i].kategori.ToString() == "Etik och regelverk")
                            {
                                resultat++;
                                etikochregelverk++;
                            }
                        }

                        else if (Convert.ToInt32(facit.fragelista[i].info) > 1)
                        {
                            flersvarsfraga++;
                        }

                        if (flersvarsfraga == Convert.ToInt32(facit.fragelista[i].info))
                        {
                            if (facit.fragelista[i].kategori.ToString() == "Produkter och hantering av kundens affärer")
                            {
                                resultat++;
                                produkterochhanteringavkundensaffärer++;
                            }

                            if (facit.fragelista[i].kategori.ToString() == "Ekonomi – nationalekonomi, finansiell ekonomi och privatekonomi")
                            {
                            resultat++;
                                ekonominationalekonomifinansiellekonomiochprivatekonomi++;
                            }

                            if (facit.fragelista[i].kategori.ToString() == "Etik och regelverk")
                            {
                                resultat++;
                                etikochregelverk++;
                        }
                    }
                }
            }
        }
            VisaSvar(facit, resultat, produkterochhanteringavkundensaffärer, ekonominationalekonomifinansiellekonomiochprivatekonomi, etikochregelverk);
        }

        public void SerializaSvar(List<fraga> svar)
        {
            string directory = Server.MapPath("svar.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(List<fraga>));
            using (TextWriter writer = new StreamWriter(directory))

            {
                serializer.Serialize(writer, svar);
            }

            SparaTest();
        }
        
        public void DoljKontroller()
        {
            btnGorProv.Visible = false;
            btnLamnain.Visible = false;
            btnSeResultat.Visible = false;
            btnSeResultatAnstallda.Visible = false;
            LabelKompetensportal.Visible = true;
        }

        public void VisaSvar(prov facit, int resultat, int produkterochhanteringavkundensaffärer, int ekonominationalekonomifinansiellekonomiochprivatekonomi, int etikochregelverk)
        {
            int i = -1;

            foreach (RepeaterItem item in Repeater1.Items) // loopar genom alla objekt i repeatern
            {
                i++;
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem) 
                {
                    if (facit.fragelista[i].svarsalternativa != null)
                    {
                        var LabelA = (Label)item.FindControl("LabelA");
                        LabelA.CssClass = "korrektsvar";
                    }
                    
                    if (facit.fragelista[i].svarsalternativb != null)
                    {
                        var LabelB = (Label)item.FindControl("LabelB");
                        LabelB.CssClass = "korrektsvar";
                    }

                    if (facit.fragelista[i].svarsalternativc != null)
                    {
                        var LabelC = (Label)item.FindControl("LabelC");
                        LabelC.CssClass = "korrektsvar";
                    }

                    if (facit.fragelista[i].svarsalternativd != null)
                    {
                        var LabelD = (Label)item.FindControl("LabelD");
                        LabelD.CssClass = "korrektsvar";
                    }
                }

            }

            // funkar ej men det löser sig under helgen
            if (resultat >= 0.7 * 25 && produkterochhanteringavkundensaffärer >= 0.6 * 8 && ekonominationalekonomifinansiellekonomiochprivatekonomi >= 0.6 * 8 && etikochregelverk >= 0.6 * 9)
            {
                LabelKompetensportal.Text = "Grattis du har klarat kompetenstestet! Ditt resultat är " + resultat + " av 25. " + produkterochhanteringavkundensaffärer + "av 8 inom kategorin Produkter och hantering av kundens affärer. " + ekonominationalekonomifinansiellekonomiochprivatekonomi + " av 8 inom Ekonomi - Nationalekonomi, finansiell enkonomi och privatekonomi. " + etikochregelverk + " av 9 i kategorin Etik och regelverk";
            }

            else
            {
                LabelKompetensportal.Text = "Du har tyvärr inte klarat kompetenstestet. Ditt resultat är " + resultat + " av 25. " + produkterochhanteringavkundensaffärer + "av 8 inom kategorin Produkter och hantering av kundens affärer. " + ekonominationalekonomifinansiellekonomiochprivatekonomi + " av 8 inom Ekonomi - Nationalekonomi, finansiell enkonomi och privatekonomi. " + etikochregelverk + " av 9 i kategorin Etik och regelverk";
            }
        }

        public void SparaTest()
        {
            DateTime dagens = DateTime.Today;

            string facit = Server.MapPath("facit.xml");
            string facitxml = File.ReadAllText(facit);

            string xml = Server.MapPath("svar.xml");
            string svarxml = File.ReadAllText(xml);

            string connectionString = "Server=webblabb.miun.se; Port=5432; Database=pgmvaru_g8; User Id=pgmvaru_g8; Password=rockring; SslMode=Require";
            string sql = "INSERT INTO u4_prov (person_id, datum, provxml, facit) VALUES (@person_id, @datum, @provxml, @facit)";

            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);


            con.Open();
            cmd.Parameters.AddWithValue("person_id", 1); // Fixa så íd på inloggad skickas in här
            cmd.Parameters.AddWithValue("datum", dagens);
            cmd.Parameters.AddWithValue("provxml", svarxml);
            cmd.Parameters.AddWithValue("facit", facitxml);


            cmd.ExecuteNonQuery();
            con.Close();


        }
    }
}

