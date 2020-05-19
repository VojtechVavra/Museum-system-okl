using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseumSystemORM.ORM
{
    public class Zmena_rezervace
    {
        public int zrID { get; set; }
        public DateTime Datum_puvodni_rezervace { get; set; }
        public DateTime Datum_nove_rezervace { get; set; }
        public DateTime Datum_zmeny { get; set; }
        public int Rezervace_rID { get; set; }
    }
}
