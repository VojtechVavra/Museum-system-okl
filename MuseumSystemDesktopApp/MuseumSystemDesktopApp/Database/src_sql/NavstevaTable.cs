using System;
using System.Data.SqlClient;


namespace MuseumSystemORM.ORM.src_sql
{
    public static class NavstevaTable
    {
        public static string SQL_ZAEVIDOVAT_NAVSTEVU = "INSERT INTO Navsteva (Pocet_osob, Datum_cas_navstevy, Rezervace_rID, Recepce_rID, Vystava_vID) " +
            "VALUES(@pocet_osob, @Datum_cas_navstevy, @p_rID, @id_recepcni, 1)";


        /// <summary>
        /// Insert the record.
        /// </summary>
        public static int VlozitNavstevu(int pocet_osob, int? rezervace_id, int recepcni = 1)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_ZAEVIDOVAT_NAVSTEVU);
            command.Parameters.AddWithValue("@pocet_osob", pocet_osob);
            command.Parameters.AddWithValue("@Datum_cas_navstevy", DateTime.Now);
            // Both operands need to be object. Use explicit cast: (object)rezervace_id
            command.Parameters.AddWithValue("@p_rID", (object)rezervace_id ?? DBNull.Value);
            command.Parameters.AddWithValue("@id_recepcni", recepcni );

            int ret = db.ExecuteNonQuery(command);

            db.Close();
            return ret;
        }

    }
}
