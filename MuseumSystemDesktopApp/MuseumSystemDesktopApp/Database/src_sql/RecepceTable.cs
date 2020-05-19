using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;


namespace MuseumSystemORM.ORM.src_sql
{
    public static class RecepceTable
    {
        public static string SQL_SELECT = "SELECT * FROM \"Recepce\"";

        
        public static Collection<Recepce> Select()
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT);
            SqlDataReader reader = db.Select(command);

            Collection<Recepce> recepcni = Read(reader);

            db.Close();
            return recepcni;
        }

        private static Collection<Recepce> Read(SqlDataReader reader)
        {
            Collection<Recepce> recepcni = new Collection<Recepce>();

            while (reader.Read())
            {
                int i = -1;
                Recepce user = new Recepce();
                user.rID = reader.GetInt32(++i);
                user.Jmeno = reader.GetString(++i);
                user.Prijmeni = reader.GetString(++i);
                user.Email = SafeData.GetString(reader, ++i);
                user.Mobilni_cislo = reader.GetInt32(++i);

                recepcni.Add(user);
            }
            return recepcni;
        }


    }
}
