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
            btnLamnain.Visible = false;     // Dessa element är dolda vid start. 
            LabelEjInloggad.Visible = false;     
            btnSeResultat.Visible = false;
            btnSeResultatAnstallda.Visible = false;
            btnGorProv.Visible = false;
            btnStartaprov.Visible = false;
        }

        public int GetPersonId(string anvandare)    // Det namn man skriver i textrutan är parametern "anvandare". Metoden returnerar id-nummer för användaren.
        {

            string connectionString = "Server=webblabb.miun.se; Port=5432; Database=pgmvaru_g8; User Id=pgmvaru_g8; Password=rockring; SslMode=Require";
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            int person_id = 0;
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
            //här skall det hämtas frågor för kunskapstest, som skall innehålla (""15 frågor"")
            string anvandare = TextBoxanvandare.Text;
            int person_id = 1;
            person_id = GetPersonId(anvandare);         // Returnerar användarens id-nummer

            if (SenasteProv(person_id))       // Tar reda på om användaren har ett giltigt provresultat. Dvs. är licensierad. 
            {                                           // om så är fallet så visas följande element på skärmen
                btnGorProv.Visible = true;
                LabelEjInloggad.Visible = false;
                TextBoxanvandare.Visible = false;
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
                TextBoxanvandare.Visible = false;
                LabelKompetensportal.Visible = true;
                Labelfornam.Visible = false;
                btnLamnain.Visible = false;
                LabelInloggad.Visible = true;
                LabelInloggad.Text = "Inloggad som: " + anvandare;   // Skriver ut namnet på inloggad användare. Denna label används sedan i metoden HittaNamn()
                btnOk.Visible = false;
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
            DateTime senasteprov = new DateTime();
            string senasteprovstring;
            bool godkand;

            string sql = "SELECT datum from u4_prov WHERE person_id = " + id + " ORDER BY datum DESC LIMIT 1";      // Tar ut datum för användarens senaste prov
            string sql2 = "SELECT godkant from u4_prov WHERE person_id = " + id + " ORDER BY datum DESC LIMIT 1";      // Vet att detta är en sunkig lösning men min reader krånglade

            NpgsqlConnection con = new NpgsqlConnection("Server=webblabb.miun.se; Port=5432; Database=pgmvaru_g8; User Id=pgmvaru_g8; Password=rockring; SslMode=Require");

            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
            NpgsqlCommand cmd2 = new NpgsqlCommand(sql2, con);

            con.Open();
            senasteprovstring = Convert.ToString(cmd.ExecuteScalar());
            godkand = Convert.ToBoolean(cmd2.ExecuteScalar());
            con.Close();

            if (senasteprovstring != "")
            {
                senasteprov = Convert.ToDateTime(senasteprovstring);
                DateTime nastaprov = senasteprov.AddYears(1);   //  Nästa prov skall skrivas senaste ett år efter det första
                LabelKompetensportal.Text = "Ditt senaste prov gjordes " + senasteprov.Date + ". Du måste göra provet igen innan " + nastaprov.Date + ".";
                LabelKompetensportal.Visible = true;
                btnSeResultat.Visible = true;
            }

            else
            {
                LabelKompetensportal.Visible = false;
                btnSeResultat.Visible = false;
            }

            if (godkand)
            {
                return true;
            }

            else
            {
                return false;
            }

        }

        protected void btnGorProv_Click(object sender, EventArgs e)
        {
            btnGorProv.Visible = false;                 // Gömmer undan en massa saker ur formuläret
            btnSeResultat.Visible = false;
            btnSeResultatAnstallda.Visible = false;
            LabelEjInloggad.Visible = false;
            TextBoxanvandare.Visible = false;
            LabelKompetensportal.Visible = true;
            LabelKompetensportal.Text = "Tryck på knappen för att starta testet. Det finns ingen tidsgräns så ta de piano och gör dig noggrann!";
            Labelfornam.Visible = false;
            btnLamnain.Visible = false;
            LabelInloggad.Visible = true;
            btnStartaprov.Visible = true;
        }

        protected void btnStartaprov_Click(object sender, EventArgs e)     //  När man klickar på "Gör Provet". 
        {
            btnLamnain.Visible = true;

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
                HamtaFragor();    // Skriver ut frågelistan i Repeater1. Se repeatern i "default.aspx"
            }

                btnStartaprov.Visible = false;                 // Gömmer undan en massa saker ur formuläret      
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

        public void HamtaFragor()   
        {
            string xml = Server.MapPath("fragor.xml");  // Frågor finns i "frågor.xml

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
            
            int person_id = HamtaID2();   // Returnerar id-nummer på användaren som är inloggad


            prov provet = new prov();

            
            if (SenasteProv(person_id))     // Returnerar en boolean som berättar om man gjort provet tidigare
            {
                provet = HamtaFragorLicensierad();    //Skapar ett prov bestående av 15 frågeobjekt av utvald kategori
            }

            else
            {
                provet = HamtaFragor2();    //Skapar ett prov bestående av samtliga 25 frågor
            }

            List<fraga> gjortprov = HittaSvar(provet);        // Färgar valda svarsalternativ röda och returnerar en lista med valda svarsalternativ för varje fråga
            SerializaSvar(gjortprov);   // Serialiserar svarslistan
            
            var tuple = RattaProv(gjortprov);
            prov facit = tuple.Item1;
            int resultat = tuple.Item2;
            int produkterochhanteringavkundensaffärer = tuple.Item3;
            int ekonominationalekonomifinansiellekonomiochprivatekonomi = tuple.Item4;
            int etikochregelverk = tuple.Item5;

            bool godkand = VisaSvar(facit, resultat, produkterochhanteringavkundensaffärer, ekonominationalekonomifinansiellekonomiochprivatekonomi, etikochregelverk);

            SparaTest(resultat, produkterochhanteringavkundensaffärer, ekonominationalekonomifinansiellekonomiochprivatekonomi, etikochregelverk, godkand);



            btnGorProv.Visible = false;
            btnLamnain.Visible = false;
            btnSeResultat.Visible = false;
            btnSeResultatAnstallda.Visible = false;
            LabelKompetensportal.Visible = true;
        }

        public prov HamtaFragor2()  // Returnerar en lista med samtliga 25 frågor som en lista av frågeobjekt
        {
            string xml = Server.MapPath("fragor.xml");      // Se metod HamtaFragorLicensierad()

            XmlSerializer deserializer = new XmlSerializer(typeof(prov));
            TextReader reader = new StreamReader(xml);
            object obj = deserializer.Deserialize(reader);
            prov laddatprov = (prov)obj;
            reader.Close();

            return laddatprov;

        }

        public List<fraga> HittaSvar(prov provet)       // Färgar valda svarsalternativ röda och returnerar en lista med valda svarsalternativ för varje fråga
        {
            List <fraga> gjortprov = new List<fraga>();
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
                gjortprov.Add(fragaobj); // skapar en lista med valda svarsalternativ
            }

            return gjortprov;
        }

        public Tuple<prov, int, int, int, int> RattaProv(List<fraga> gjortprov)
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
                    if (gjortprov[l].nr == facit.fragelista[k].nr)
                    {
                        nyfacit.fragelista.Add(facit.fragelista[k]);
                        l++;

                        if (l == 15)
                        {
                            break;
                        }
                    }
                }
                facit.fragelista.Clear();
                facit = nyfacit;
            }



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
            return Tuple.Create(facit, resultat, produkterochhanteringavkundensaffärer, ekonominationalekonomifinansiellekonomiochprivatekonomi, etikochregelverk);
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

            // Ersätta med tabell kanske samt att man kanske ska hämta anta frågor ur varje kategori ur xml istället
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
                LabelKompetensportal.Text = "Grattis du har klarat kompetenstestet! Ditt resultat är " + resultat + " av " + totalt + ". " + produkterochhanteringavkundensaffärer + " av " + totaltkategori1 + " inom kategorin Produkter och hantering av kundens affärer. " + ekonominationalekonomifinansiellekonomiochprivatekonomi + " av " + totaltkategori2 + " inom Ekonomi - Nationalekonomi, finansiell enkonomi och privatekonomi. " + etikochregelverk + " av " + totaltkategori3 + " i kategorin Etik och regelverk";
                return true;
            }

            else
            {
                LabelKompetensportal.Text = "Du har tyvärr inte klarat kompetenstestet. Ditt resultat är " + resultat + " av " + totalt + ". " + produkterochhanteringavkundensaffärer + " av " + totaltkategori1 + " inom kategorin Produkter och hantering av kundens affärer. " + ekonominationalekonomifinansiellekonomiochprivatekonomi + " av " + totaltkategori2 + " inom Ekonomi - Nationalekonomi, finansiell enkonomi och privatekonomi. " + etikochregelverk + " av " + totaltkategori3 + " i kategorin Etik och regelverk";
                return false;
            }

        }

        public void SparaTest(int resultat, int produkterochhanteringavkundensaffärer, int ekonominationalekonomifinansiellekonomiochprivatekonomi, int etikochregelverk, bool godkand)
        {
            int person_id = HamtaID2();   // Returnerar id-nummer på användaren som är inloggad

            DateTime dagens = DateTime.Today;

            string facit = Server.MapPath("facit.xml");
            string facitxml = File.ReadAllText(facit);

            string xml = Server.MapPath("svar.xml");
            string svarxml = File.ReadAllText(xml);

            string connectionString = "Server=webblabb.miun.se; Port=5432; Database=pgmvaru_g8; User Id=pgmvaru_g8; Password=rockring; SslMode=Require";
            string sql = "INSERT INTO u4_prov (person_id, datum, provxml, facit, ressek1, ressek2, ressek3, godkant) VALUES (@person_id, @datum, @provxml, @facit, @ressek1, @ressek2, @ressek3, @godkant)";

            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);


            con.Open();
            cmd.Parameters.AddWithValue("person_id", person_id);
            cmd.Parameters.AddWithValue("datum", dagens);
            cmd.Parameters.AddWithValue("provxml", svarxml);
            cmd.Parameters.AddWithValue("facit", facitxml);
            cmd.Parameters.AddWithValue("ressek1", produkterochhanteringavkundensaffärer);
            cmd.Parameters.AddWithValue("ressek2", ekonominationalekonomifinansiellekonomiochprivatekonomi);
            cmd.Parameters.AddWithValue("ressek3", etikochregelverk);
            cmd.Parameters.AddWithValue("godkant", godkand);

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
                    for(int j = i + 2 ; j < inloggadtext.Length; j++)
                    {
                        anvandare = anvandare + inloggadtext[j];
                    }
                }
            }

            return anvandare;
        }
    
        protected void btnSeResultat_Click(object sender, EventArgs e)
        {

        }
    }
}

