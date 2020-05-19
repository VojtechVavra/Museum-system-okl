using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseumSystemORM.ORM.src_sql
{
    public static class SafeData
    {
        public static string GetString(SqlDataReader reader, int collIndex)
        {
            if (!reader.IsDBNull(collIndex))
                return reader.GetString(collIndex);
            return string.Empty;
        }

        public static int? GetInt(SqlDataReader reader, int collIndex)
        {
            if (!reader.IsDBNull(collIndex))
                return reader.GetInt32(collIndex);
            return null;
        }
    }
}
