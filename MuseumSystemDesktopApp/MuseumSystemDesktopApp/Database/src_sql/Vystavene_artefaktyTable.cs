using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseumSystemORM.ORM.src_sql
{
    public static class Vystavene_artefaktyTable
    {
        // funkce 4. b)
        public static string SQL_INSERT_VYSTAVIT_ARTEFAKT = "INSERT INTO Vystavene_artefakty (Artefakt_aID, Vystava_vID) VALUES(@Artefakt_aID, @Vystava_vID)";
        // funkce 4. c)
        public static string SQL_DELETE_STAHNOUT_ARTEFAKT = "DELETE FROM Vystavene_artefakty WHERE Artefakt_aID = @Artefakt_aID";
        // funkce 4. e)
        public static string SQL_SELECT_ID_VSECHNY_VYSTAVENYCH_ARTEFAKTU = "SELECT * FROM vystavene_artefakty WHERE artefakt_aid IN (SELECT aid from artefakt)";


        // funkce 4. b)
        public static bool VystavitArtefaktId(int id_artefakt, int id_vystava)
        {
            Database db = new Database();

            SqlCommand command = db.CreateCommand(SQL_INSERT_VYSTAVIT_ARTEFAKT);
            command.Parameters.AddWithValue("@Artefakt_aID", id_artefakt);
            command.Parameters.AddWithValue("@Vystava_vID", id_vystava);

            db.Connect();
            int ret = db.ExecuteNonQuery(command);
            db.Close();

            return ret > 0 ? true : false;
        }

        // funkce 4. c)
        public static bool StahnoutArtefaktId(int id_artefakt)
        {
            Database db = new Database();

            SqlCommand command = db.CreateCommand(SQL_DELETE_STAHNOUT_ARTEFAKT);
            command.Parameters.AddWithValue("@Artefakt_aID", id_artefakt);

            db.Connect();
            int ret = db.ExecuteNonQuery(command);
            db.Close();

            return ret > 0 ? true : false;
        }


        // funkce 4. e)
        public static Collection<Vystavene_artefakty> VypisIdVsechVystavenychArtefaktu()
        {
            Database db = new Database();
            SqlCommand command;

            command = db.CreateCommand(SQL_SELECT_ID_VSECHNY_VYSTAVENYCH_ARTEFAKTU);

            // vypsani parametru z sql dotazovaciho stringu
            /*foreach (SqlParameter p in command.Parameters)
            {
                Console.WriteLine($"{p.ParameterName} {p.Value} {p.DbType}");
            }*/

            db.Connect();

            SqlDataReader reader = db.Select(command);

            Collection<Vystavene_artefakty> vystavene_artefakty = ReadVystavenyArtefakt(reader);

            reader.Close();
            db.Close();
            return vystavene_artefakty;
        }

        // read function for func 4. e)
        private static Collection<Vystavene_artefakty> ReadVystavenyArtefakt(SqlDataReader reader)
        {
            Collection<Vystavene_artefakty> vystavene_artefakty = new Collection<Vystavene_artefakty>();

            while (reader.Read())
            {
                Vystavene_artefakty jeden_vystaveny_artefakt = new Vystavene_artefakty();
                int i = -1;

                jeden_vystaveny_artefakt.Artefakt_aID = reader.GetInt32(++i);

                jeden_vystaveny_artefakt.Vystava_vID = reader.GetInt32(++i);

                vystavene_artefakty.Add(jeden_vystaveny_artefakt);
            }
            return vystavene_artefakty;
        }

    }
}
