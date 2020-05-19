using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;


namespace MuseumSystemORM.ORM.src_sql
{
    public static class ArtefaktTable
    {
        // funkce 4. a)
        private static string SQL_INSERT_ARTEFAKT = "INSERT INTO artefakt (nazev, datum_nalezeni, odhadovane_stari_artefaktu, zeme_nalezu, zeme_puvodu_artefaktu, gps_souradnice_nalezu, " +
            "popis, datum_vystaveni, je_prozkouman_a_zdokumentovan, datum_posledni_kontroly, vypujceno_od, propujceno_muzeu, archeolog_aid) " +
            "VALUES(@nazev, @datum_nalezeni, @odhadovane_stari_artefaktu, @zeme_nalezu, @zeme_puvodu_artefaktu, @gps_souradnice_nalezu, " +
            "@popis, @datum_vystaveni, @je_prozkouman_a_zdokumentovan, @datum_posledni_kontroly, @vypujceno_od, @propujceno_muzeu, @archeolog_aid)";

        // funkce 4. d)
        private static string SQL_SELECT_VSECHNY_ARTEFAKTY = "SELECT * FROM artefakt";
        // funkce 4. f)
        private static string SQL_SELECT_NEVYSTAVENE_ARTEFAKTY = "SELECT * FROM artefakt WHERE aid NOT IN (SELECT artefakt_aid FROM vystavene_artefakty)";


        // funkce 4. a)
        public static bool InsertArtefact(Artefakt artefakt)
        {
            Database db = new Database();

            SqlCommand command = db.CreateCommand(SQL_INSERT_ARTEFAKT);
            PrepareCommand(command, artefakt);

            db.Connect();
            int ret = db.ExecuteNonQuery(command);
            db.Close();

            return ret > 0 ? true : false;
        }


        // Prepare a command for function 4. a).
        private static void PrepareCommand(SqlCommand command, Artefakt artefakt)
        {
            string nazev = artefakt.Nazev.Substring(0, (artefakt.Nazev.Length > 30 ? 30 : artefakt.Nazev.Length));

            string zeme_nalezu = artefakt.Zeme_nalezu.Substring(0, (artefakt.Zeme_nalezu.Length > 40 ? 40 : artefakt.Zeme_nalezu.Length));

            string zeme_puvodu_artefaktu = artefakt.Zeme_puvodu_artefaktu;
            if(zeme_puvodu_artefaktu != null)
            {
                zeme_puvodu_artefaktu = artefakt.Zeme_puvodu_artefaktu.Substring(0, (zeme_puvodu_artefaktu.Length > 50 ? 50 : zeme_puvodu_artefaktu.Length));
            }

            string vypujceno_od = artefakt.Vypujceno_od;
            if (vypujceno_od != null)
            {
                vypujceno_od = vypujceno_od.Substring(0, (vypujceno_od.Length > 100 ? 100 : vypujceno_od.Length));
            }

            string propujceno_muzeu = artefakt.Propujceno_muzeu;
            if(propujceno_muzeu != null)
            {
                propujceno_muzeu = propujceno_muzeu.Substring(0, (propujceno_muzeu.Length > 100 ? 100 : propujceno_muzeu.Length));
            }


            command.Parameters.AddWithValue("@nazev", nazev ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@datum_nalezeni", artefakt.Datum_nalezeni);
            command.Parameters.AddWithValue("@odhadovane_stari_artefaktu", artefakt.Odhadovane_stari_artefaktu ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@zeme_nalezu", zeme_nalezu ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@zeme_puvodu_artefaktu", zeme_puvodu_artefaktu ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@gps_souradnice_nalezu", artefakt.GPS_souradnice_nalezu ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@popis", string.IsNullOrEmpty(artefakt.Popis) ? DBNull.Value : (object)artefakt.Popis);
            command.Parameters.AddWithValue("@datum_vystaveni", artefakt.Datum_vystaveni ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@je_prozkouman_a_zdokumentovan", artefakt.Je_prozkouman_a_zdokumentovan ? 1 : 0);
            command.Parameters.AddWithValue("@datum_posledni_kontroly", artefakt.Datum_posledni_kontroly ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@vypujceno_od", string.IsNullOrEmpty(vypujceno_od) ? DBNull.Value : (object)vypujceno_od);
            command.Parameters.AddWithValue("@propujceno_muzeu", string.IsNullOrEmpty(propujceno_muzeu) ? DBNull.Value : (object)propujceno_muzeu);
            command.Parameters.AddWithValue("@archeolog_aid", artefakt.Archeolog_aID);

            // print parametru a jejich nahrad
            foreach (SqlParameter p in command.Parameters)
            {
                Console.WriteLine($"{p.ParameterName} {p.Value} {p.DbType}");
            }
        }


        // funkce 4. d)
        public static Collection<Artefakt> VypisVsechArtefaktu()
        {
            Database db = new Database();
            SqlCommand command;

            command = db.CreateCommand(SQL_SELECT_VSECHNY_ARTEFAKTY);

            // vypsani parametru z sql dotazovaciho stringu
            /*foreach (SqlParameter p in command.Parameters)
            {
                Console.WriteLine($"{p.ParameterName} {p.Value} {p.DbType}");
            }*/

            db.Connect();

            SqlDataReader reader = db.Select(command);

            Collection<Artefakt> artefakt = ReadArtefakt(reader);

            reader.Close();
            db.Close();
            return artefakt;
        }


        // funkce 4. d)
        public static Collection<Artefakt> VypisNevystavenychArtefaktu()
        {
            Database db = new Database();
            SqlCommand command;

            command = db.CreateCommand(SQL_SELECT_NEVYSTAVENE_ARTEFAKTY);

            // vypsani parametru z sql dotazovaciho stringu
            /*foreach (SqlParameter p in command.Parameters)
            {
                Console.WriteLine($"{p.ParameterName} {p.Value} {p.DbType}");
            }*/

            db.Connect();

            SqlDataReader reader = db.Select(command);

            Collection<Artefakt> artefakt = ReadArtefakt(reader);

            reader.Close();
            db.Close();
            return artefakt;
        }


        // read function for func 4. d) and 4. f)
        private static Collection<Artefakt> ReadArtefakt(SqlDataReader reader)
        {
            Collection<Artefakt> artefakt = new Collection<Artefakt>();

            while (reader.Read())
            {
                Artefakt jeden_artefakt = new Artefakt();
                int i = -1;

                jeden_artefakt.aID = reader.GetInt32(++i);

                jeden_artefakt.Nazev = reader.GetString(++i);

                jeden_artefakt.Datum_nalezeni = reader.GetDateTime(++i);

                if (!reader.IsDBNull(++i))
                {
                    jeden_artefakt.Odhadovane_stari_artefaktu = reader.GetInt32(i);
                }

                jeden_artefakt.Zeme_nalezu = reader.GetString(++i);

                if (!reader.IsDBNull(++i))
                {
                    jeden_artefakt.Zeme_puvodu_artefaktu = reader.GetString(i);
                }
                if (!reader.IsDBNull(++i))
                {
                    jeden_artefakt.GPS_souradnice_nalezu = reader.GetDouble(i);
                }
                if (!reader.IsDBNull(++i))
                {
                    jeden_artefakt.Popis = reader.GetString(i);
                }
                if (!reader.IsDBNull(++i))
                {
                    jeden_artefakt.Datum_vystaveni = reader.GetDateTime(i);
                }

                jeden_artefakt.Je_prozkouman_a_zdokumentovan = reader.GetBoolean(++i);

                if (!reader.IsDBNull(++i))
                {
                    jeden_artefakt.Datum_posledni_kontroly = reader.GetDateTime(i);
                }
                if (!reader.IsDBNull(++i))
                {
                    jeden_artefakt.Vypujceno_od = reader.GetString(i);
                }
                if (!reader.IsDBNull(++i))
                {
                    jeden_artefakt.Propujceno_muzeu = reader.GetString(i);
                }
                if (!reader.IsDBNull(++i))
                {
                    jeden_artefakt.Archeolog_aID = reader.GetInt32(i);
                }


                artefakt.Add(jeden_artefakt);
            }
            return artefakt;
        }
    }
}
