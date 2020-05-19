using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseumSystemORM.ORM
{
    public class Vystava
    {
        public int vID { get; set; }
        public string Popis_vystavy { get; set; }
        public DateTime? Datum_zacatku_vystavy { get; set; }
        public DateTime? Datum_konce_vystavy { get; set; }
    }
}
