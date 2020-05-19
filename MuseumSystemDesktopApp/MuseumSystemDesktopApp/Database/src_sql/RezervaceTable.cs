using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseumSystemORM.ORM.src_sql
{
    public static class RezervaceTable
    {
        // funkce 2. a)
        private static string SQL_SELECT_VSECH_REZERVACI_OD_NEJNOVEJSICH = "SELECT * FROM rezervace ORDER BY Zarezervovane_datum DESC";
        private static string SQL_SELECT_VSECH_REZERVACI_OD_NEJSTARSICH = "SELECT * FROM rezervace ORDER BY Zarezervovane_datum ASC";
        // funkce 2. b)
        private static string SQL_SELECT_OD_DO = "SELECT * FROM Rezervace WHERE Zarezervovane_datum >= @od AND Zarezervovane_datum <= @do";
        // funkce 2. d)
        private static string SQL_SELECT_COUNT_REZERVACI = "SELECT COUNT(*) AS rezervace FROM Rezervace WHERE rID = @p_rID";
        private static string SQL_DELETE_REZERVACE_ID = "DELETE FROM Rezervace WHERE rID = @p_rID";
        private static string SQL_DELETE_ZMENA_REZERVACE_ID = "DELETE FROM Zmena_rezervace WHERE Rezervace_rID = @p_rID";

        private static bool exceeded = false;

        // funkce 2. a)
        public static Collection<Rezervace> SelectRezervace(string poradi = "DESC")
        {
            Database db = new Database();
            SqlCommand command;

            if (poradi.ToUpper() == "ASC")
                command = db.CreateCommand(SQL_SELECT_VSECH_REZERVACI_OD_NEJSTARSICH);
            else
                command = db.CreateCommand(SQL_SELECT_VSECH_REZERVACI_OD_NEJNOVEJSICH);

            // vzpsani parametru z sql dotazovaciho stringu
            /*foreach (SqlParameter p in command.Parameters)
            {
                Console.WriteLine($"{p.ParameterName} {p.Value} {p.DbType}");
            }*/

            db.Connect();

            SqlDataReader reader = db.Select(command);

            Collection<Rezervace> rezervace = Read(reader);
            
            reader.Close();
            db.Close();
            return rezervace;
        }

        // funkce 2. b)
        public static Collection<Rezervace> SelectRezervaceOdDo(DateTime od_data, DateTime do_data)
        {
            Database db = new Database();
            SqlCommand command = db.CreateCommand(SQL_SELECT_OD_DO);

            // 20100301 - format yyyyMMdd
            string od_data_string_format = od_data.ToString("yyyy-MM-dd HH:mm:ss");
            string do_data_string_format = do_data.ToString("yyyy-MM-dd HH:mm:ss");
            command.Parameters.AddWithValue("@od", od_data_string_format);
            command.Parameters.AddWithValue("@do", do_data_string_format);

            // vzpsani parametru z sql dotazovaciho stringu
            /*foreach (SqlParameter p in command.Parameters)
            {
                Console.WriteLine($"{p.ParameterName} {p.Value} {p.DbType}");
            }*/

            db.Connect();

            SqlDataReader reader = db.Select(command);

            Collection<Rezervace> rezervace = Read(reader);

            reader.Close();
            db.Close();
            return rezervace;
        }

        private static Collection<Rezervace> Read(SqlDataReader reader)
        {
            Collection<Rezervace> rezervace = new Collection<Rezervace>();

            while (reader.Read())
            {
                Rezervace jedna_rezervace = new Rezervace();
                int i = -1;
                jedna_rezervace.rID = reader.GetInt32(++i);
                jedna_rezervace.Jmeno = reader.GetString(++i);
                jedna_rezervace.Prijmeni = reader.GetString(++i);
                jedna_rezervace.Pocet_osob = reader.GetInt32(++i);
                jedna_rezervace.Zarezervovane_datum = reader.GetDateTime(++i);
                jedna_rezervace.Datum_vytvoreni = reader.GetDateTime(++i);
                if (!reader.IsDBNull(++i))
                {
                    jedna_rezervace.Pruvodce_pID = reader.GetInt32(i);
                }

                rezervace.Add(jedna_rezervace);
            }
            return rezervace;
        }

        // funkce 2. d)
        public static bool DeleteRezervaceById(int p_rID)
        {
            Database db = new Database();
            SqlCommand command = db.CreateCommand(SQL_SELECT_COUNT_REZERVACI);
            
            command.Parameters.AddWithValue("@p_rID", p_rID);

            db.Connect();
            SqlDataReader reader = db.Select(command);
            int pocet_rezervaci = GetNumberOfIdReservations(reader);
            reader.Close();

            if (pocet_rezervaci >= 1)
            {
                bool deletedZmeny = DeleteZmenyRezervace(p_rID);

                if(deletedZmeny || !deletedZmeny)
                {
                    SqlCommand command2 = db.CreateCommand(SQL_DELETE_REZERVACE_ID);

                    command2.Parameters.AddWithValue("@p_rID", p_rID);
                    int ret = db.ExecuteNonQuery(command2);

                    db.Close();

                    return ret > 0 ? true : false;
                }
                else
                {
                    db.Close();
                    return false;
                }
            }
            else
            {
                db.Close();
                return false;
            }

        }

        private static bool DeleteZmenyRezervace(int id)
        {
            Database db = new Database();
            SqlCommand command = db.CreateCommand(SQL_DELETE_ZMENA_REZERVACE_ID);

            command.Parameters.AddWithValue("@p_rID", id);

            db.Connect();
            int ret = db.ExecuteNonQuery(command);

            db.Close();

            return ret > 0 ? true : false;
        }

        private static int GetNumberOfIdReservations(SqlDataReader reader)
        {
            int pocet_recepcnich = 0;
            while(reader.Read())
            {
                pocet_recepcnich = reader.GetInt32(0);
            }
            
            return pocet_recepcnich;
        }

        // funkce 2. c)
        // Vytvořit rezervaci ( transakce )
        public static bool CreateReservation(Rezervace rezervace)
        {
            bool povedloSe = false;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringMsSql"].ConnectionString))
            {
                conn.Open();

                // 1.  create a command object identifying the stored procedure
                SqlCommand cmd = new SqlCommand("vytvoreniRezervace", conn);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new SqlParameter("@p_jmeno", rezervace.Jmeno));
                cmd.Parameters.Add(new SqlParameter("@p_prijmeni", rezervace.Prijmeni));
                cmd.Parameters.Add(new SqlParameter("@p_pocet_osob", rezervace.Pocet_osob));
                cmd.Parameters.Add(new SqlParameter("@p_rezervovane_datum", rezervace.Zarezervovane_datum));
                cmd.Parameters.Add(new SqlParameter("@p_pruvodce", rezervace.Pruvodce_pID));

                // execute the command
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    // iterate through results, printing each to console
                    while (rdr.Read())
                    {
                        //Console.WriteLine("Product: {0,-35} Total: {1,2}", rdr["ProductName"], rdr["Total"]);
                    }
                    povedloSe = true;
                }
            }
            return povedloSe;
        }

        // funkce 2. e)
        // Změna rezervace ( transakce )
        public static bool ChangeReservation(int id_rezervace, DateTime noveDatumRezervace)
        {
            bool povedloSe = false;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringMsSql"].ConnectionString))
            {
                conn.Open();

                // 1.  create a command object identifying the stored procedure
                SqlCommand cmd = new SqlCommand("zmenaRezervace", conn);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new SqlParameter("@p_rID", id_rezervace));
                cmd.Parameters.Add(new SqlParameter("@p_noveDatum", noveDatumRezervace));

                // added
                conn.InfoMessage += connection_InfoMessage;

                //var result = conn.ExecuteNonQuery();
                // execute the command
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    // iterate through results, printing each to console
                    while (rdr.Read())
                    {
                        //Console.WriteLine("Product: {0,-35} Total: {1,2}", rdr["ProductName"], rdr["Total"]);
                        Console.WriteLine(rdr);
                    }
                    povedloSe = true;
                }
            }

            if (povedloSe && exceeded)
                return false;
            else if (povedloSe && !exceeded)
                return true;
            else
                return false;
        }

        static void connection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            // this gets the print statements (maybe the error statements?)
            string outputFromStoredProcedure = e.Message;
            string maximumExceeded = "Rezervaci jiz vicekrat nelze zmenit (max 3 zmeny)!";
            if (outputFromStoredProcedure == maximumExceeded)
            {
                exceeded = true;
            }
            else
            {
                exceeded = false;
            }
        }

    }
}
