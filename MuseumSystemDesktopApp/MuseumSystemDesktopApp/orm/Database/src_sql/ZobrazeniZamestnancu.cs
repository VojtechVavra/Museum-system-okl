using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseumSystemORM.ORM.src_sql
{
    public static class ZobrazeniZamestnancu
    {
        public static string SQL_SELECT_RECEPCE = "SELECT rID, Jmeno, Prijmeni FROM recepce";
        public static string SQL_SELECT_ARCHEOLOG = "SELECT aID, Jmeno, Prijmeni FROM archeolog";


        public static Collection<object> Select(string pozice)
        {
            Database db = new Database();
            SqlCommand command;

            if (pozice.ToLower() == "recepce")
                command = db.CreateCommand(SQL_SELECT_RECEPCE);
            else if (pozice.ToLower() == "archeolog")
                command = db.CreateCommand(SQL_SELECT_ARCHEOLOG);
            else
                return null;

            db.Connect();

            SqlDataReader reader = db.Select(command);

            Collection<object> zamestnanci = Read(reader);

            reader.Close();
            db.Close();
            return zamestnanci;
        }


        private static Collection<object> Read(SqlDataReader reader)
        {
            Collection<object> zamestnanci = new Collection<object>();

            while (reader.Read())
            {
                // String[3] zamestnanec = {rID, Jmeno, Prijmeni}
                // pro recepcni i archeologa
                string[] zamestnanec = new string[3];
                int i = -1;

                zamestnanec[0] = reader.GetInt32(++i).ToString();
                zamestnanec[1] = reader.GetString(++i);
                zamestnanec[2] = reader.GetString(++i);

                zamestnanci.Add(zamestnanec);
            }
            return zamestnanci;
        }

    }
}
