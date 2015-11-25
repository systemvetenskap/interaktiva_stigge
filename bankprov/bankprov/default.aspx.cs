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
using System.Text;

namespace bankprov
{
    public partial class index : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            HideAll();
            btnOk.Visible = true;
            ListBoxanvandare.Visible = true;
            LabelKompetensportal.Visible = true;

            if (!IsPostBack)
            {
                ListBoxanvandare.Items.Clear();
                LaddaAnvandare();
            }


        }

        public void HideAll()   // Döljer alla element
        {
            btnLamnain.Visible = false;     // Dessa element är dolda vid start. 
            LabelEjInloggad.Visible = false;
            btnSeResultat.Visible = false;
            btnSeResultatAnstallda.Visible = false;
            btnStart.Visible = false;
            btnGorProv.Visible = false;
            btnStartaprov.Visible = false;
            GridView1.Visible = false;
            btnOk.Visible = false;
            ListBoxanvandare.Visible = false;
            Repeater1.Visible = false;
            LabelKompetensportal.Visible = false;

        }

        public void LaddaAnvandare()
        {
            string sql = "SELECT anvandarnamn FROM u4_konto ORDER by anvandarnamn ASC";

            NpgsqlConnection con = new NpgsqlConnection("Server=webblabb.miun.se; Port=5432; Database=pgmvaru_g8; User Id=pgmvaru_g8; Password=rockring; SslMode=Require");
            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);

            List<string> lista = new List<string>();

            con.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();
            con.Close();

            while (dr.Read())
            {
                string anvandarnamn = (string)dr["anvandarnamn"];
                lista.Add(anvandarnamn);
            }

            ListBoxanvandare.DataSource = lista;
            ListBoxanvandare.DataBind();
        }


        public int GetPersonId()    // Det namn man skriver i textrutan är parametern "anvandare". Metoden returnerar id-nummer för användaren.
        {

            string connectionString = "Server=webblabb.miun.se; Port=5432; Database=pgmvaru_g8; User Id=pgmvaru_g8; Password=rockring; SslMode=Require";
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            int person_id = 0;
            string anvandare = ListBoxanvandare.SelectedValue.ToString();


            try
            {
                conn.Open();
                string sql = "SELECT id FROM u4_konto WHERE anvandarnamn = @anvandare;";    //Tar ut id för den användare som skrivits in i textrutan. 
                NpgsqlCommand command = new NpgsqlCommand(sql, conn);
                command.Parameters.AddWithValue("anvandare", anvandare);

                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    person_id = Convert.ToInt32(dr["id"]);      // Id-nummret sparas i variabeln
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

            SenasteProv(person_id);                // Skriver ut när användaren senast skrev ett prov och när nästa prov måste skrivas. Returnerar en boolean som berättar om man gjort provet tidigare
            Chef(person_id);                    // Om användaren är chef så visas knappen för att se de anställdas resultat

            return person_id;                   //Returnerar id-nummer för användaren

        }

        public void btnOK_Click(object sender, EventArgs e)     // Kollar vilken behörighet angiven användare har samt öppnar upp startsidan
        {
            if (ListBoxanvandare.SelectedIndex != -1)
            {
                //här skall det hämtas frågor för kunskapstest, som skall innehålla (""15 frågor"")
                string anvandare = ListBoxanvandare.Text;
                int person_id = 1;
                person_id = GetPersonId();         // Returnerar användarens id-nummer samt visar "Se anställdas resultat" om personen är chef

                if (SenasteProv(person_id))       // Tar reda på om användaren har ett giltigt provresultat. Dvs. är licensierad. 
                {                                           // om så är fallet så visas följande element på skärmen
                    btnGorProv.Visible = true;
                    LabelEjInloggad.Visible = false;
                    ListBoxanvandare.Visible = false;
                    LabelKompetensportal.Visible = true;
                    Labelfornam.Visible = false;
                    btnLamnain.Visible = false;
                    LabelInloggad.Visible = true;
                    LabelInloggad.Text = "Inloggad som: " + anvandare;   // Skriver ut namnet på inloggad användare. Denna label används sedan i metoden HittaNamn()
                    btnOk.Visible = false;

                }
                else
                {
                    //öppna sidan för licensiering.    
                    //här skall man hämta frågor för licensiering
                    //öppna sidan för licensiering den skall inehålla (""""25 frågor"""")
                    btnGorProv.Visible = true;
                    LabelEjInloggad.Visible = false;
                    ListBoxanvandare.Visible = false;
                    LabelKompetensportal.Visible = true;
                    Labelfornam.Visible = false;
                    btnLamnain.Visible = false;
                    LabelInloggad.Visible = true;
                    LabelInloggad.Text = "Inloggad som: " + anvandare;   // Skriver ut namnet på inloggad användare. Denna label används sedan i metoden HittaNamn()
                    btnOk.Visible = false;
                }
            }

            else
            {
                Labelfornam.CssClass = "felsvar";
            }
        }

        public void Chef(int id)      // Om användaren är chef så visas knappen för att se de anställdas resultat
        {
            string sql = "SELECT chef FROM u4_konto WHERE chef = " + id;      //VI BORDE INTE HA CHEF SOM BOOLEAN UTAN SOM EN FRÄMMANDE NYCKEL TILL KONTO-ID

            NpgsqlConnection con = new NpgsqlConnection("Server=webblabb.miun.se; Port=5432; Database=pgmvaru_g8; User Id=pgmvaru_g8; Password=rockring; SslMode=Require");

            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);

            con.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();
            con.Close();


            if (dr.HasRows)
            {
                btnSeResultatAnstallda.Visible = true;
            }

            else
            {
                btnSeResultatAnstallda.Visible = false;
            }
        }

        public bool SenasteProv(int id)     // Skriver ut när användaren senast skrev ett prov och när nästa prov måste skrivas. Returnerar en boolean som berättar om man gjort provet tidigare
        {
            DataTable dt = new DataTable();
            DateTime senaste = new DateTime();

            string senasteprovstring;

            string sql = "SELECT datum from u4_prov WHERE person_id = " + id + " ORDER BY datum DESC LIMIT 1";      // Tar ut datum för användarens senaste prov
            string sql2 = "SELECT godkant from u4_prov WHERE person_id = " + id + "ORDER BY datum DESC";

            NpgsqlConnection con = new NpgsqlConnection("Server=webblabb.miun.se; Port=5432; Database=pgmvaru_g8; User Id=pgmvaru_g8; Password=rockring; SslMode=Require");

            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql2, con);

            con.Open();
            senasteprovstring = Convert.ToString(cmd.ExecuteScalar());
            da.Fill(dt);
            con.Close();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dt.Rows[i][0]) == true)
                {
                    if (i == 0)
                    {
                        senaste = Convert.ToDateTime(senasteprovstring);
                        string senasteprov = (senaste.ToString("dd/MM/yyyy"));
                        string nastaprov = (senaste.AddYears(1).ToString("dd/MM/yyyy"));
                        LabelKompetensportal.Text = "Ditt senaste prov gjordes " + senasteprov + ". Du måste göra provet igen innan " + nastaprov + ".";
                        LabelKompetensportal.Visible = true;
                        btnSeResultat.Visible = true;
                    }

                    else
                    {
                        LabelKompetensportal.Visible = true;
                        LabelKompetensportal.Text = "Ditt senaste prov var tyvärr inte godkänt och du måste göra ett nytt snarast!";
                        btnSeResultat.Visible = true;
                    }

                    return true;
                }
            }

            LabelKompetensportal.Visible = false;
            btnSeResultat.Visible = false;

            return false;

        }

        protected void btnGorProv_Click(object sender, EventArgs e)
        {
            HideAll();

            //btnGorProv.Visible = false;       
            //btnSeResultat.Visible = false;
            //btnSeResultatAnstallda.Visible = false;
            //LabelEjInloggad.Visible = false;
            //TextBoxanvandare.Visible = false;
            LabelKompetensportal.Visible = true;
            LabelKompetensportal.Text = "Tryck på knappen för att starta testet. Det finns ingen tidsgräns så ta de piano och gör dig noggrann!";
            //Labelfornam.Visible = false;
            //btnLamnain.Visible = false;
            LabelInloggad.Visible = true;
            btnStartaprov.Visible = true;

        }

        protected void btnStartaprov_Click(object sender, EventArgs e)     //  När man klickar på "Starta provet". 
        {



            string xmlpath = Server.MapPath("fragor.xml");

            string xml;
            using (StreamReader reader = new StreamReader(xmlpath))
            {
                xml = reader.ReadLine();
            }


            int person_id = HamtaID2();
            prov prov = new prov();

            if (SenasteProv(person_id))     // Returnerar en boolean som berättar om man gjort provet tidigare.  Om man gjort prov tidigare så är satsen true
            {
                prov = HamtaFragorLicensierad();
                Repeater1.DataSource = prov.fragelista;
                Repeater1.DataBind();
            }

            else                     // Om man inte gjort provet tidigare       TOLKAR JAG DETTA RÄTT???? SKALL DET INTE VARA OM MAN HAR ETT FÖR GAMMALT PROV ELLER EJ GJORT DET ALLS?
            {
                prov = HamtaFragor();    // Skriver ut frågelistan i Repeater1. Se repeatern i "default.aspx"
                Repeater1.DataSource = prov.fragelista;
                Repeater1.DataBind();
            }

            HideAll();
            btnLamnain.Visible = true;
            Repeater1.Visible = true;

            //    btnStartaprov.Visible = false;                 // Gömmer undan Starta Prov-knappen 
            //btnSeResultat.Visible = false;                      // Gömmer "Visa tidigare"-knappen
        }

        public prov HamtaFragorLicensierad()    //Returnerar en lista med 15 frågeobjekt av utvald kategori
        {
            string xml = Server.MapPath("fragor.xml");  // xml filen läses in till en textsträng

            XmlSerializer deserializer = new XmlSerializer(typeof(prov));
            TextReader reader = new StreamReader(xml);
            object obj = deserializer.Deserialize(reader);  // Xml-filen läses in och deserialiseras till en lista av fråge objekt i enlighet med klassen "fragor.cs"
            prov XmlData = (prov)obj;
            reader.Close();

            int i = 0;
            int antal = 0;
            prov listafragor = new prov();

            foreach (object objekt in XmlData.fragelista)       //Loopar igenom frågorna
            {
                if (XmlData.fragelista[i].kategori == "Produkter och hantering av kundens affärer")     // Väljer ut frågor av en viss kategori
                {
                    if (antal <= 4)
                    {
                        antal++;
                        listafragor.fragelista.Add(XmlData.fragelista[i]);      // De första 5 frågorna i provet skall tillhöra den valda kategorin
                    }
                }

                if (XmlData.fragelista[i].kategori == "Ekonomi – nationalekonomi, finansiell ekonomi och privatekonomi")
                {
                    if (antal >= 5 && antal <= 9)
                    {
                        antal++;
                        listafragor.fragelista.Add(XmlData.fragelista[i]);
                    }
                }

                if (XmlData.fragelista[i].kategori == "Etik och regelverk.")
                {
                    if (antal <= 14 && antal >= 10)
                    {
                        antal++;
                        listafragor.fragelista.Add(XmlData.fragelista[i]);
                    }
                }

                i++;
            }

            return listafragor;
        }

        protected void btnLamnain_Click(object sender, EventArgs e)
        {
            string xml = Server.MapPath("fragor.xml");
            int person_id = HamtaID2();   // Returnerar id-nummer på användaren som är inloggad
            bool visagammalt = false; //Hade gjort så att funktionen RattaProv slutar vid 15 frågor om man gjort prov tidigare. Problemet är att om man kör den via visa gamla prov så stannar den ju vid 15 fast det gamla provet kan ha 25 frågor. Alltså skickar jag bara in en variabel också och gör det enkelt för mig


            prov provet = new prov();


            if (SenasteProv(person_id))     // Returnerar en boolean som berättar om man gjort provet tidigare
            {
                provet = HamtaFragorLicensierad();    //Skapar ett prov bestående av 15 frågeobjekt av utvald kategori
            }

            else
            {
                provet = HamtaFragor();
            }

            prov gjortprov = HittaSvar(provet);
            SerializaSvar(gjortprov);

            var tuple = RattaProv(gjortprov, visagammalt);
            prov facit = tuple.Item1;
            int resultat = tuple.Item2;
            int produkterochhanteringavkundensaffärer = tuple.Item3;
            int ekonominationalekonomifinansiellekonomiochprivatekonomi = tuple.Item4;
            int etikochregelverk = tuple.Item5;

            bool godkand = VisaSvar(facit, resultat, produkterochhanteringavkundensaffärer, ekonominationalekonomifinansiellekonomiochprivatekonomi, etikochregelverk);

            // Ersätta med tabell kanske samt att man kanske ska hämta anta frågor ur varje kategori ur xml istället
            int totalt = 25;
            int totaltkategori1 = 8;
            int totaltkategori2 = 8;
            int totaltkategori3 = 9;

            if (SenasteProv(person_id))
            {
                totalt = 15;
                totaltkategori1 = 5;
                totaltkategori2 = 5;
                totaltkategori3 = 5;
            }

            if (godkand)
            {
                LabelKompetensportal.Text = "Grattis du har klarat kompetenstestet! Ditt resultat är " + resultat + " av " + totalt + ". " + produkterochhanteringavkundensaffärer + " av " + totaltkategori1 + " inom kategorin Produkter och hantering av kundens affärer. " + ekonominationalekonomifinansiellekonomiochprivatekonomi + " av " + totaltkategori2 + " inom Ekonomi - Nationalekonomi, finansiell enkonomi och privatekonomi. " + etikochregelverk + " av " + totaltkategori3 + " i kategorin Etik och regelverk";
            }

            else
            {
                LabelKompetensportal.Text = "Du har tyvärr inte klarat kompetenstestet. Ditt resultat är " + resultat + " av " + totalt + ". " + produkterochhanteringavkundensaffärer + " av " + totaltkategori1 + " inom kategorin Produkter och hantering av kundens affärer. " + ekonominationalekonomifinansiellekonomiochprivatekonomi + " av " + totaltkategori2 + " inom Ekonomi - Nationalekonomi, finansiell enkonomi och privatekonomi. " + etikochregelverk + " av " + totaltkategori3 + " i kategorin Etik och regelverk";
            }

            SparaTest(resultat, totalt, produkterochhanteringavkundensaffärer, ekonominationalekonomifinansiellekonomiochprivatekonomi, etikochregelverk, godkand);

            HideAll();

            //btnGorProv.Visible = false;
            //btnLamnain.Visible = false;
            //btnSeResultat.Visible = false;
            //btnSeResultatAnstallda.Visible = false;
            LabelKompetensportal.Visible = true;
            btnStart.Visible = true;
            Repeater1.Visible = true;
        }

        public prov HamtaFragor()
        {
            string xml = Server.MapPath("fragor.xml");  // Frågor finns i "frågor.xml

            XmlSerializer deserializer = new XmlSerializer(typeof(prov));
            TextReader reader = new StreamReader(xml);
            object obj = deserializer.Deserialize(reader);
            prov laddatprov = (prov)obj;
            reader.Close();

            return laddatprov;
        }

        public prov HittaSvar(prov provet)
        {
            prov gjortprov = new prov();
            int checkboxkontroll;

            int i = -1;

            foreach (RepeaterItem item in Repeater1.Items) // loopar genom alla objekt i repeatern
            {
                i++;
                checkboxkontroll = 0;

                fraga fragaobj = new fraga();
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var checkBoxA = (CheckBox)item.FindControl("CheckBoxA");
                    if (checkBoxA.Checked == true)      // Kollar om checkbox A är markerad för gällande fråga
                    {
                        fragaobj.svarsalternativa = provet.fragelista[i].svarsalternativa; // Skapar ett frågeobjekt med endast valda svarsalternativ
                        fragaobj.nr = provet.fragelista[i].nr;

                        checkboxkontroll++;

                        var LabelA = (Label)item.FindControl("LabelA");
                        LabelA.CssClass = "felsvar";        // Om svar A är valt så blir det röd-färgat
                        // Alla svar som man svarat blir röda, de korrekta ändras sedan till gröna i VisaSvar()
                    }

                    var checkBoxB = (CheckBox)item.FindControl("CheckBoxB");
                    if (checkBoxB.Checked == true)
                    {
                        fragaobj.svarsalternativb = provet.fragelista[i].svarsalternativb;
                        fragaobj.nr = provet.fragelista[i].nr;

                        checkboxkontroll++;

                        var LabelB = (Label)item.FindControl("LabelB");
                        LabelB.CssClass = "felsvar";

                    }

                    var checkBoxC = (CheckBox)item.FindControl("CheckBoxC");
                    if (checkBoxC.Checked == true)
                    {
                        fragaobj.svarsalternativc = provet.fragelista[i].svarsalternativc;
                        fragaobj.nr = provet.fragelista[i].nr;

                        checkboxkontroll++;

                        var LabelC = (Label)item.FindControl("LabelC");
                        LabelC.CssClass = "felsvar";

                    }

                    var checkBoxD = (CheckBox)item.FindControl("CheckBoxD");
                    if (checkBoxD.Checked == true)
                    {
                        fragaobj.svarsalternativd = provet.fragelista[i].svarsalternativd;
                        fragaobj.nr = provet.fragelista[i].nr;

                        checkboxkontroll++;

                        var LabelD = (Label)item.FindControl("LabelD");
                        LabelD.CssClass = "felsvar";
                    }

                    if (checkBoxA.Checked == false && checkBoxB.Checked == false && checkBoxC.Checked == false && checkBoxD.Checked == false)
                    {
                        fragaobj.nr = provet.fragelista[i].nr;
                    }
                }
                fragaobj.info = checkboxkontroll.ToString();
                gjortprov.fragelista.Add(fragaobj); // lägger till svaret i en lista
            }

            return gjortprov;
        }

        public Tuple<prov, int, int, int, int> RattaProv(prov gjortprov, bool visagammalt)
        {
            string xml = Server.MapPath("facit.xml");

            XmlSerializer deserializer = new XmlSerializer(typeof(prov));
            TextReader reader = new StreamReader(xml);
            object obj = deserializer.Deserialize(reader);
            prov facit = (prov)obj;
            reader.Close();

            int l = 0;
            int k = -1;
            int i = -1;
            int resultat = 0;
            int flersvarsfraga;
            int produkterochhanteringavkundensaffärer = 0;
            int ekonominationalekonomifinansiellekonomiochprivatekonomi = 0;
            int etikochregelverk = 0;

            int person_id = HamtaID2();

            if (SenasteProv(person_id))     //Returnerar en boolean som berättar om man gjort provet tidigare
            {
                prov nyfacit = new prov();

                foreach (object objektobjekt in facit.fragelista)
                {
                    k++;
                    if (gjortprov.fragelista[l].nr == facit.fragelista[k].nr)
                    {
                        nyfacit.fragelista.Add(facit.fragelista[k]);
                        l++;

                        if (l == 15 && visagammalt == false)
                        {
                            break;
                        }
                    }
                }
                facit.fragelista.Clear();
                facit = nyfacit;
            }



            foreach (object objekt in gjortprov.fragelista)
            {
                flersvarsfraga = 0;
                i++;

                if (gjortprov.fragelista[i].info != facit.fragelista[i].info)
                {
                    //För många eller för få alternativ kryssade. Skickar vidare till nästa fråga
                }

                else
                {
                    if (gjortprov.fragelista[i].svarsalternativa == facit.fragelista[i].svarsalternativa && gjortprov.fragelista[i].svarsalternativa != null)
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

                    if (gjortprov.fragelista[i].svarsalternativb == facit.fragelista[i].svarsalternativb && gjortprov.fragelista[i].svarsalternativb != null)
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

                    if (gjortprov.fragelista[i].svarsalternativc == facit.fragelista[i].svarsalternativc && gjortprov.fragelista[i].svarsalternativc != null)
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

                    if (gjortprov.fragelista[i].svarsalternativd == facit.fragelista[i].svarsalternativd && gjortprov.fragelista[i].svarsalternativd != null)
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
            return Tuple.Create(facit, resultat, produkterochhanteringavkundensaffärer, ekonominationalekonomifinansiellekonomiochprivatekonomi, etikochregelverk);
        }

        public void SerializaSvar(prov svar)
        {
            string directory = Server.MapPath("svar.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(prov));
            using (TextWriter writer = new StreamWriter(directory))
            {
                serializer.Serialize(writer, svar);
            }
        }

        public bool VisaSvar(prov facit, int resultat, int produkterochhanteringavkundensaffärer, int ekonominationalekonomifinansiellekonomiochprivatekonomi, int etikochregelverk)
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

            int totalt = 25;
            int totaltkategori1 = 8;
            int totaltkategori2 = 8;
            int totaltkategori3 = 9;

            int person_id = HamtaID2();
            if (SenasteProv(person_id))
            {
                totalt = 15;
                totaltkategori1 = 5;
                totaltkategori2 = 5;
                totaltkategori3 = 5;
            }

            if (resultat >= 0.7 * totalt && produkterochhanteringavkundensaffärer >= 0.6 * totaltkategori1 && ekonominationalekonomifinansiellekonomiochprivatekonomi >= 0.6 * totaltkategori2 && etikochregelverk >= 0.6 * totaltkategori3)
            {
                return true;
            }

            else
            {
                return false;
            }

        }

        public void SparaTest(int resultat, int totalt, int produkterochhanteringavkundensaffärer, int ekonominationalekonomifinansiellekonomiochprivatekonomi, int etikochregelverk, bool godkand)
        {
            int person_id = HamtaID2();   // Returnerar id-nummer på användaren som är inloggad

            DateTime dagens = DateTime.Today;

            string facit = Server.MapPath("facit.xml");
            string facitxml = File.ReadAllText(facit);

            string xml = Server.MapPath("svar.xml");
            string svarxml = File.ReadAllText(xml);

            string connectionString = "Server=webblabb.miun.se; Port=5432; Database=pgmvaru_g8; User Id=pgmvaru_g8; Password=rockring; SslMode=Require";
            string sql = "INSERT INTO u4_prov (person_id, datum, facit, ressek1, ressek2, ressek3, godkant, svarxml, antalfragor) VALUES (@person_id, @datum, @facit, @ressek1, @ressek2, @ressek3, @godkant, @svarxml, @antalfragor)";

            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);


            con.Open();
            cmd.Parameters.AddWithValue("person_id", person_id);
            cmd.Parameters.AddWithValue("datum", dagens);
            cmd.Parameters.AddWithValue("svarxml", svarxml);
            cmd.Parameters.AddWithValue("facit", facitxml);
            cmd.Parameters.AddWithValue("ressek1", produkterochhanteringavkundensaffärer);
            cmd.Parameters.AddWithValue("ressek2", ekonominationalekonomifinansiellekonomiochprivatekonomi);
            cmd.Parameters.AddWithValue("ressek3", etikochregelverk);
            cmd.Parameters.AddWithValue("godkant", godkand);
            cmd.Parameters.AddWithValue("antalfragor", totalt);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public int HamtaID2()   // Returnerar id-nummer på användaren som är inloggad
        {
            string anvandare = HittaNamn();     // Returnerar användarnamnet

            string connectionString = "Server=webblabb.miun.se; Port=5432; Database=pgmvaru_g8; User Id=pgmvaru_g8; Password=rockring; SslMode=Require";
            string sql = "SELECT id FROM u4_konto WHERE anvandarnamn = '" + anvandare + "'";        // Söker vilket Id som tillhör användarnamnet

            NpgsqlConnection con = new NpgsqlConnection(connectionString);

            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);


            con.Open();
            int person_id = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            return person_id;
        }

        public string HittaNamn() // Löjligt komplicerat för att hitta användarnamnet men det funkade inte med global variabel som användar id och jag orkade inte krångla
        {
            string inloggadtext = LabelInloggad.Text;       // Läser in namnet på den som är inloggad från en label.  Se metod btnOK_Click()
            string anvandare = "";

            for (int i = 0; i < inloggadtext.Length; i++)
            {
                if (inloggadtext[i] == ':')
                {
                    for (int j = i + 2; j < inloggadtext.Length; j++)
                    {
                        anvandare = anvandare + inloggadtext[j];
                    }
                }
            }

            return anvandare;
        }

        protected void btnSeResultat_Click(object sender, EventArgs e)
        {
            HamtaGjordaProv();
            HideAll();
            // btnGorProv.Visible = false;
            // btnSeResultat.Visible = false;
            // btnSeResultatAnstallda.Visible = false;
            GridView1.Visible = true;
            btnStart.Visible = true;
        }

        public List<gjordaprov> HamtaGjordaProv()
        {
            int person_id = HamtaID2();
            List<gjordaprov> lista = new List<gjordaprov>();
            int resultatdel1;
            int resultatdel2;
            int resultatdel3;
            bool godkand;

            string sql = "SELECT prov_id, datum, ressek1, ressek2, ressek3, godkant, antalfragor FROM u4_prov WHERE person_id= " + person_id;

            NpgsqlConnection con = new NpgsqlConnection("Server=webblabb.miun.se; Port=5432; Database=pgmvaru_g8; User Id=pgmvaru_g8; Password=rockring; SslMode=Require");
            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);

            con.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                gjordaprov gjortprov = new gjordaprov();
                gjortprov.id = Convert.ToInt32(dr["prov_id"]);
                DateTime datum = Convert.ToDateTime(dr["datum"]);
                string datumstring = (datum.ToString("dd/MM/yyyy"));
                gjortprov.datum = datumstring;
                resultatdel1 = Convert.ToInt32(dr["ressek1"]);
                resultatdel2 = Convert.ToInt32(dr["ressek2"]);
                resultatdel3 = Convert.ToInt32(dr["ressek3"]);
                gjortprov.poang = Convert.ToString(resultatdel1 + resultatdel2 + resultatdel3) + "/" + Convert.ToString(dr["antalfragor"]);
                godkand = Convert.ToBoolean(dr["godkant"]);


                if (godkand == true)
                {
                    gjortprov.resultat = "Godkänt";
                }

                if (godkand == false)
                {
                    gjortprov.resultat = "Icke Godkänt";
                }

                lista.Add(gjortprov);
            }

            con.Close();

            GridView1.DataSource = lista;
            GridView1.DataBind();
            //GridView1.Columns[1].Visible = false;
            //GridView1.Columns[2].Visible = false;

            foreach (GridViewRow row in GridView1.Rows)
            {
                LinkButton lb = (LinkButton)row.Cells[0].Controls[0];
                lb.Text = "Visa Provet";
            }

            return lista;

        }

        public void GridView1_SelectedIndexChanged(Object sender, EventArgs e)
        {
            bool visagammalt = true;
            var tuple = HamtaFragorDB();
            string svarxml = tuple.Item1;
            string facitxml = tuple.Item2;


            prov provet = new prov();

            if (RaknaSvar(facitxml) == 15)
            {
                provet = HamtaFragorLicensierad();
            }

            if (RaknaSvar(facitxml) == 25)
            {
                provet = HamtaFragor();
            }

            Repeater1.DataSource = provet.fragelista;
            Repeater1.DataBind();

            CheckaCheckboxar(svarxml);

            prov gjortprov = HittaSvar(provet);

            var tuple2 = RattaProv(gjortprov, visagammalt);
            prov facit = tuple2.Item1;
            int resultat = tuple2.Item2;
            int produkterochhanteringavkundensaffärer = tuple2.Item3;
            int ekonominationalekonomifinansiellekonomiochprivatekonomi = tuple2.Item4;
            int etikochregelverk = tuple2.Item5;

            VisaSvar(facit, resultat, produkterochhanteringavkundensaffärer, ekonominationalekonomifinansiellekonomiochprivatekonomi, etikochregelverk);

            HideAll();
            btnStart.Visible = true;
            Repeater1.Visible = true;
            //btnSeResultatAnstallda.Visible = false;
            //btnSeResultat.Visible = false;
        }

        public Tuple<string, string> HamtaFragorDB()
        {
            string svarxml;
            string facitxml;

            DataTable dt = new DataTable();

            int row = GridView1.SelectedIndex;
            int prov_id = Convert.ToInt32(GridView1.DataKeys[row].Value);



            string sql = "SELECT facit FROM u4_prov WHERE prov_id= " + prov_id;
            string sql2 = "SELECT svarxml FROM u4_prov WHERE prov_id = " + prov_id;

            NpgsqlConnection con = new NpgsqlConnection("Server=webblabb.miun.se; Port=5432; Database=pgmvaru_g8; User Id=pgmvaru_g8; Password=rockring; SslMode=Require");
            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
            NpgsqlCommand cmd2 = new NpgsqlCommand(sql2, con);

            con.Open();
            facitxml = Convert.ToString(cmd.ExecuteScalar());
            svarxml = Convert.ToString(cmd2.ExecuteScalar());
            con.Close();

            return Tuple.Create(svarxml, facitxml);
        }

        public int RaknaSvar(string xml)
        {
            var serializer = new XmlSerializer(typeof(prov));
            prov result;

            using (TextReader reader = new StringReader(xml))
            {
                result = (prov)serializer.Deserialize(reader);
            }

            int i = 0;

            foreach (object objekt in result.fragelista)
            {
                i++;
            }

            return i;
        }

        public void CheckaCheckboxar(string svarxml)
        {
            var serializer = new XmlSerializer(typeof(prov));
            prov svar;

            using (TextReader reader = new StringReader(svarxml))
            {
                svar = (prov)serializer.Deserialize(reader);
            }

            int i = 0;

            foreach (RepeaterItem item in Repeater1.Items) // loopar genom alla objekt i repeatern
            {

                fraga fragaobj = new fraga();
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var checkBoxA = (CheckBox)item.FindControl("CheckBoxA");
                    if (svar.fragelista[i].svarsalternativa != null)
                    {
                        checkBoxA.Checked = true;
                    }

                    var checkBoxB = (CheckBox)item.FindControl("CheckBoxB");
                    if (svar.fragelista[i].svarsalternativb != null)
                    {
                        checkBoxB.Checked = true;
                    }

                    var checkBoxC = (CheckBox)item.FindControl("CheckBoxC");
                    if (svar.fragelista[i].svarsalternativc != null)
                    {
                        checkBoxC.Checked = true;
                    }

                    var checkBoxD = (CheckBox)item.FindControl("CheckBoxD");
                    if (svar.fragelista[i].svarsalternativd != null)
                    {
                        checkBoxD.Checked = true;
                    }
                    i++;
                }
            }
        }

        protected void btnSeResultatAnstallda_Click(object sender, EventArgs e)
        {
            HamtaProvAnstallda();   //Returnerar listan av objekt med provresultat för den inloggades anställda och fyller på dessa i griden
            HideAll();
            //btnGorProv.Visible = false;
            //btnSeResultat.Visible = false;
            //btnSeResultatAnstallda.Visible = false;
            GridView1.Visible = true;       // Döljer allt utom griden
            btnStart.Visible = true;
            SnartProv();
        }

        protected void btnStart_Click(object sender, EventArgs e)
        {

            //här skall det hämtas frågor för kunskapstest, som skall innehålla (""15 frågor"")
            string anvandare = HittaNamn();
            int person_id = 1;
            HideAll();
            person_id = GetPersonId();         // Returnerar användarens id-nummer samt visar "Se anställdas resultat" om personen är chef



            if (SenasteProv(person_id))       // Tar reda på om användaren har ett giltigt provresultat. Dvs. är licensierad. 
            {                                           // om så är fallet så visas följande element på skärmen
                btnGorProv.Visible = true;
                //  LabelEjInloggad.Visible = false;
                // TextBoxanvandare.Visible = false;
                LabelKompetensportal.Visible = true;
                //    Labelfornam.Visible = false;
                //    btnLamnain.Visible = false;
                LabelInloggad.Visible = true;
                LabelInloggad.Text = "Inloggad som: " + anvandare;   // Skriver ut namnet på inloggad användare. Denna label används sedan i metoden HittaNamn()
                //      btnOk.Visible = false;

            }
            else
            {
                //öppna sidan för licensiering.    
                //här skall man hämta frågor för licensiering
                //öppna sidan för licensiering den skall inehålla (""""25 frågor"""")
                btnGorProv.Visible = true;
                //     LabelEjInloggad.Visible = false;
                //      TextBoxanvandare.Visible = false;
                LabelKompetensportal.Visible = true;
                //       Labelfornam.Visible = false;
                //      btnLamnain.Visible = false;
                LabelInloggad.Visible = true;
                LabelInloggad.Text = "Inloggad som: " + anvandare;   // Skriver ut namnet på inloggad användare. Denna label används sedan i metoden HittaNamn()
                //      btnOk.Visible = false;
            }
        }

        public List<gjordaprov> HamtaProvAnstallda()   //Returnerar listan av objekt med provresultat för den inloggades anställda och fyller på dessa i griden
        {
            int person_id = HamtaID2();         // Hämtar konto-id för den inloggade
            List<gjordaprov> lista = new List<gjordaprov>();
            int resultatdel1;
            int resultatdel2;
            int resultatdel3;
            bool godkand;

            string sql = "SELECT a.*, b.* FROM u4_konto a INNER JOIN u4_prov b ON a.id = b.person_id INNER JOIN (SELECT person_id, MAX(datum) maxdatum FROM u4_prov GROUP BY person_id) c ON b.person_id = c.person_id AND b.datum = c.maxdatum WHERE chef = 1";

            NpgsqlConnection con = new NpgsqlConnection("Server=webblabb.miun.se; Port=5432; Database=pgmvaru_g8; User Id=pgmvaru_g8; Password=rockring; SslMode=Require");
            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);    // Hämtar resultat för den línloggades anställdas prov

            con.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                gjordaprov gjortprov = new gjordaprov();        // Läser in resultatet till ett objekt av klassen "gjordaprov.cs"
                gjortprov.id = Convert.ToInt32(dr["prov_id"]);
                gjortprov.fornamn = Convert.ToString(dr["fnamn"]);
                gjortprov.efternamn = Convert.ToString(dr["enamn"]);
                DateTime datum = Convert.ToDateTime(dr["datum"]);
                string datumstring = (datum.ToString("dd/MM/yyyy"));
                gjortprov.datum = datumstring; resultatdel1 = Convert.ToInt32(dr["ressek1"]);
                resultatdel2 = Convert.ToInt32(dr["ressek2"]);
                resultatdel3 = Convert.ToInt32(dr["ressek3"]);
                gjortprov.poang = Convert.ToString(resultatdel1 + resultatdel2 + resultatdel3) + "/" + Convert.ToString(dr["antalfragor"]);
                godkand = Convert.ToBoolean(dr["godkant"]);

                if (godkand == true)    // Ändrar texten i Resultatkolumnen
                {
                    gjortprov.resultat = "Godkänt";
                }

                if (godkand == false)
                {
                    gjortprov.resultat = "Icke Godkänt";
                }

                lista.Add(gjortprov);   //Lägger till objektet i listan som sedan skall presenteras
            }

            con.Close();

            GridView1.Columns[1].Visible = true;
            GridView1.Columns[2].Visible = true;
            GridView1.DataSource = lista;   // Visar listan i griden
            GridView1.DataBind();

            foreach (GridViewRow row in GridView1.Rows)
            {
                LinkButton lb = (LinkButton)row.Cells[0].Controls[0];
                lb.Text = "Visa Provet";
            }

            return lista;   //Returnerar listan av objekt
        }

        public void SnartProv()
        {
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                DateTime senaste = DateTime.ParseExact(this.GridView1.Rows[i].Cells[3].Text, "dd/MM/yyyy", null);

                if (senaste.AddMonths(11) < DateTime.Today)
                {
                    GridView1.Rows[i].CssClass = "GridViewSnart";
                }

            }
        }

        protected void LinkButtonLoggaut_Click(object sender, EventArgs e)
        {
            HideAll();
            LabelInloggad.Text = "";
            btnOk.Visible = true;
            LabelKompetensportal.Visible = true;
            LabelKompetensportal.Text = "Välkommen till JE-Bankens kompetensportal. Här kan du enkelt svara på frågor om Volvobilar, skidåkning och Bamsetidningar och på ett snabbt och smidigt sätt erhålla ett bevis på din kompetens inom dessa områden.";
            ListBoxanvandare.Visible = true;
        }
    }
}

