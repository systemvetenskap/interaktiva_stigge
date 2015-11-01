using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using NpgsqlTypes;

namespace bankprov
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public static int GetPersonId(string anvandare)
        {
            //a connectionString = "Server=webblabb.miun.se;Port=5432;Database=pgmvaru_g1;User Id=pgmvaru_g1;Password=enhjuling;SSL=true";
            string connectionString = "Server=webblabb.miun.se; Port=5432; Database=pgmvaru_g8; User Id=pgmvaru_g8; Password=rockring; SslMode=Require; Trust Server Certificate=true";

            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            int person_id = 0;
            try
            {
                conn.Open();
                string sql = "SELECT fk_person_id FROM u4_konto WHERE anvandarnamn = :anvandare;";
                NpgsqlCommand command = new NpgsqlCommand(@sql, conn);
                command.Parameters.Add(new NpgsqlParameter("anvandare", NpgsqlDbType.Varchar));
                command.Parameters["anvandare"].Value = anvandare;
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    person_id = Convert.ToInt32(dr["fk_person_id"]);
                }
            }
            catch (NpgsqlException ex)
            {
            }
            finally
            {
                conn.Close();
            }
            return person_id;
        }
        public static bool arlinsensierad(int fk_person_id)
        {
            string connectionString = "Server=webblabb.miun.se; Port=5432; Database=pgmvaru_g8; User Id=pgmvaru_g8; Password=rockring; SslMode=Require; Trust Server Certificate=true";
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            bool linsensierad = false;
            try
            {
                conn.Open();
                string sql = "SELECT linsensierad FROM u4_person WHERE id = :fk_person_id;";
                NpgsqlCommand command = new NpgsqlCommand(@sql, conn);
                command.Parameters.Add(new NpgsqlParameter("fk_person_id", NpgsqlDbType.Varchar));
                command.Parameters["fk_person_id"].Value = fk_person_id;
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

        protected void ButtonPersonal_Click(object sender, EventArgs e)
        {
            string anvandare = TextBoxanvandare.Text;
            int person_id = 0;
            person_id = GetPersonId(anvandare);
            string provtip;
            if (arlinsensierad(person_id) == true)
            {
                //öppna sidan för kunskapsprov
                provtip = "kunskapsprov";
            }
            else
            {
                //öppna sidan för linsensiering
                provtip = "lisensiering";
            }


        }

        protected void ButtonProvledare_Click(object sender, EventArgs e)
        {
            string anvandare = TextBoxprovledare.Text;
            int person_id = 0;
            person_id = GetPersonId(anvandare);
            string provtip;
            if (arlinsensierad(person_id) == true)
            {
                //öppna sidan för kunskapsprov
                provtip = "kunskapsprov";
            }
            else
            {
                //öppna sidan för linsensiering
                provtip = "lisensiering";
            }
        }
    }
}
