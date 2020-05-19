using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseumSystemORM.ORM
{
    public class Rezervace
    {
        public int rID { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public int Pocet_osob { get; set; }
        public DateTime Zarezervovane_datum { get; set; }
        public DateTime Datum_vytvoreni { get; set; }
        public int? Pruvodce_pID { get; set; }
    }
}
