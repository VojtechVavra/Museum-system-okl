using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseumSystemORM.ORM
{
    public class Navsteva
    {
        public int nID { get; set; }
        public int Pocet_osob { get; set; }
        public DateTime Datum_cas_navstevy { get; set; }
        public int? Rezervace_rID { get; set; }
        public int Recepce_rID { get; set; }
        public int Vystava_vID { get; set; }
    }
}
