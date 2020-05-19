using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseumSystemORM.ORM.src_sql
{
    public static class VystavaTable
    {
        // funkce 3. a)
        public static string SQL_INSERT_VYSTAVA = "INSERT INTO Vystava (Popis_vystavy, Datum_zacatku_vystavy, Datum_konce_vystavy) " +
            "VALUES(@popis_vystavy, @datum_zacatku_vystavy, @datum_konce_vystavy)";
        // funkce 3. b)
        public static string SQL_DELETE_ALL_ARTEFAKT_BY_ID_VYSTAVY = "DELETE FROM Vystavene_artefakty WHERE Vystava_vID = @vystava_vID";
        public static string SQL_DELETE_VYSTAVA = "DELETE FROM Vystava WHERE vID = @vID";


        // funkce 3. a)
        public static bool InsertVystava(Vystava vystava)
        {
            Database db = new Database();

            SqlCommand command = db.CreateCommand(SQL_INSERT_VYSTAVA);
            PrepareCommand(command, vystava);

            db.Connect();
            int ret = db.ExecuteNonQuery(command);

            db.Close();

            return ret > 0 ? true : false;
        }

        /// <summary>
        /// Prepare a command.
        /// </summary>
        private static void PrepareCommand(SqlCommand command, Vystava vystava)
        {
            string popis = vystava.Popis_vystavy.Substring(0, (vystava.Popis_vystavy.Length > 400 ? 400 : vystava.Popis_vystavy.Length));
            
            command.Parameters.AddWithValue("@popis_vystavy", String.IsNullOrEmpty(popis) ? DBNull.Value : (object)popis);
            command.Parameters.AddWithValue("@datum_zacatku_vystavy", vystava.Datum_zacatku_vystavy == null ? DBNull.Value : (object)vystava.Datum_zacatku_vystavy);
            command.Parameters.AddWithValue("@datum_konce_vystavy", vystava.Datum_konce_vystavy == null ? DBNull.Value : (object)vystava.Datum_konce_vystavy);
        }


        // funkce 3. b)
        public static bool DeleteVystavaById(int vystava_id)
        {
            Database db = new Database();

            // odebrani vsech vystavenych artefakty z dane vystavy
            SqlCommand command = db.CreateCommand(SQL_DELETE_ALL_ARTEFAKT_BY_ID_VYSTAVY);
            command.Parameters.AddWithValue("@vystava_vID", vystava_id);

            db.Connect();
            int ret = db.ExecuteNonQuery(command);
            db.Close();

            // smazani vystavy podle id
            SqlCommand command2 = db.CreateCommand(SQL_DELETE_VYSTAVA);
            command2.Parameters.AddWithValue("@vID", vystava_id);

            db.Connect();
            int ret2 = db.ExecuteNonQuery(command2);
            db.Close();

            return ret2 > 0 ? true : false;
        }


        // funkce 3. c)
        // Změna rezervace ( transakce )
        public static bool KontrolaVytvoreneVystavy(int id_vystavy)
        {
            bool povedloSe = false;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringMsSql"].ConnectionString))
            {
                conn.Open();

                // 1.  create a command object identifying the stored procedure
                SqlCommand cmd = new SqlCommand("kontrolaVytvoreneVystavy", conn);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new SqlParameter("@p_vID", id_vystavy));

                // execute the command
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // iterate through results, printing each to console
                    /* while (rdr.Read())
                     {
                         if(rdr != null)
                             Console.WriteLine("Email: {0}", rdr["Email"]);
                         //if(rdr["StazeneArtefakty"] != null)
                         //if (rdr != null)
                             //Console.WriteLine("Pocet stazenych artefaktu: {0}", rdr["StazeneArtefakty"]);
                     }

                     // this advances to the next resultset 
                     while (rdr.NextResult())
                     {
                         // iterate through results, printing each to console
                         while (rdr.Read())
                         {
                             //if(rdr["Email"] != null)
                             //Console.WriteLine("Email: {0}", rdr["Email"]);
                             //if(rdr["StazeneArtefakty"] != null)
                             if (rdr != null)
                                 Console.WriteLine("Pocet stazenych artefaktu: {0}", rdr["StazeneArtefakty"]);
                         }
                     }*/


                    var tables = new List<List<Dictionary<string, object>>>();
                    do
                    {
                        //var val = reader[0];
                        string s = reader.GetName(0);
                        //Console.WriteLine($"{reader.GetName(0)}: {reader[0] } {s}");

                        while (reader.Read())
                        {
                            if (s == "Email")
                                Console.WriteLine("Email: {0}", reader["Email"]);
                            else if(s == "StazeneArtefakty")
                                Console.WriteLine("Pocet stazenych artefaktu: {0}", reader["StazeneArtefakty"]);
                            //if(rdr["StazeneArtefakty"] != null)
                            //if (rdr != null)
                            //Console.WriteLine("Pocet stazenych artefaktu: {0}", rdr["StazeneArtefakty"]);
                        }


                        //var table = new List<Dictionary<string, object>>();
                        //while (reader.Read())
                        //    table.Add(ReadMe(reader));
                        //tables.Add(table);
                    } while (reader.NextResult());


                    povedloSe = true;
                }
            }
            return povedloSe;
        }

        private static Dictionary<string, object> ReadMe(IDataRecord reader)
        {
            var row = new Dictionary<string, object>();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                var val = reader[i];
                row[reader.GetName(i)] = val == DBNull.Value ? null : val;
            }
            return row;
        }

    }
}
