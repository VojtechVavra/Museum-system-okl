using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseumSystemORM.ORM
{
    public class Artefakt
    {
        public int aID { get; set; }
        public string Nazev { get; set; }                   // VARCHAR(30)
        public DateTime Datum_nalezeni { get; set; }
        public int? Odhadovane_stari_artefaktu { get; set; }
        public string Zeme_nalezu { get; set; }             // VARCHAR(40)
        public string Zeme_puvodu_artefaktu { get; set; }   // VARCHAR(50)
        public double? GPS_souradnice_nalezu { get; set; }
        public string Popis { get; set; }                   // VARCHAR(MAX)
        public DateTime? Datum_vystaveni { get; set; }
        public bool Je_prozkouman_a_zdokumentovan { get; set; }
        public DateTime? Datum_posledni_kontroly { get; set; }
        public string Vypujceno_od { get; set; }            // VARCHAR(100)
        public string Propujceno_muzeu { get; set; }        // VARCHAR(100)
        public int Archeolog_aID { get; set; }
    }
}
