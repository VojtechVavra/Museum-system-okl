using System;
using System.Collections.ObjectModel;
using System.Globalization;
using MuseumSystemORM.ORM;
using MuseumSystemORM.ORM.src_sql;

namespace MuseumSystemORM
{
    class Program2Console
    {
        static void aa(string[] args)
        {
            /* 
             * Zakomentovane jsou ty funkce, ktere neco vytvari, nebo vkladaji do tabulky
            */

            // Tato funkce neni ve specifikaci
            // funkce na vypis poctu recepcnich
            //int count1 = RecepceTable.Select().Count;
            //Console.WriteLine("pocet recepcnich: " + count1);



            // funkce 1. a) 
            // zobrazeni zamestnancu dle pracovni pozice
            Console.WriteLine("\nfunkce 1. a)");
            Collection<object> zamestnanci = ZobrazeniZamestnancu.Select("recepce");
            foreach (var zam in zamestnanci)
            {
                string[] zamestnanec = (string[])zam;
                string id = zamestnanec[0];
                string jmeno = zamestnanec[1];
                string prijmeni = zamestnanec[2];

                //string id2 = (zam as string[])[0];
                //string id3 = ((string[])zam)[0];
                Console.WriteLine($"{id} {jmeno} {prijmeni}");
            }
            


            // funkce 2. a)
            // Výpis všech rezervací seřazených podle vstupniho parametru ("DESC" nebo "ASC", default "DESC")
            // DESC od nejnovějších podle zarezervovaného data, ASC naopak
            Collection<Rezervace> rezervace = RezervaceTable.SelectRezervace();
            Console.WriteLine("\nfunkce 2. a)");
            Console.WriteLine("{rID} {Jmeno} {Prijmeni} {Pocet_osob} {Pruvodce_pID} {Zarezervovane_datum} {Datum_vytvoreni}");
            foreach ( var rez in rezervace )
            {
                Console.WriteLine($"{rez.rID} \t {rez.Jmeno} \t {rez.Prijmeni} \t {rez.Pocet_osob} \t {rez.Pruvodce_pID} \t {rez.Zarezervovane_datum} \t {rez.Datum_vytvoreni}");
            }



            // funkce 2. b)
            // Výpis všech rezervací k určitému datu (od - do)
            DateTime od_data = new DateTime(2019, 6, 18, 10, 0, 0);
            // '2019-06-18 10:00:00.000' - sql format
            DateTime do_data = new DateTime(2019, 6, 19, 15, 0, 0);
            Collection<Rezervace> rezervace_od_do = RezervaceTable.SelectRezervaceOdDo(od_data, do_data);

            Console.WriteLine("\nfunkce 2. b)");
            Console.WriteLine("{rID} {Jmeno} {Prijmeni} {Pocet_osob} {Pruvodce_pID} {Zarezervovane_datum}");
            foreach (var rez in rezervace_od_do)
            {
                Console.WriteLine($"{rez.rID} \t {rez.Jmeno} \t {rez.Prijmeni} \t {rez.Pocet_osob} \t {rez.Pruvodce_pID} \t {rez.Zarezervovane_datum.ToString("yyyy MM dd HH:mm:ss") } ");
            }



            // funkce 2. c)
            // Vytvořit rezervaci ( transakce )
            /* 
            Console.WriteLine("\nfunkce 2. c)");
            Rezervace newRezervace = new Rezervace();
            newRezervace.Jmeno = "Pepek";
            newRezervace.Prijmeni = "Namornik";
            newRezervace.Pocet_osob = 64;
            DateTime datum_rezervace = new DateTime(2020, 6, 19, 10, 0, 0);
            newRezervace.Zarezervovane_datum = datum_rezervace;
            newRezervace.Pruvodce_pID = 1;

            bool rezervace_vytvorena = RezervaceTable.CreateReservation(newRezervace);
            Console.WriteLine(String.Format("{0}", rezervace_vytvorena ? "Rezervace probehla uspesne." : "Rezervaci se nepodarilo provest."));
            */



            // funkce 2. d)
            // Zrušení rezervace
            bool a = RezervaceTable.DeleteRezervaceById(1002);
            Console.WriteLine("\nfunkce 2. d)");
            Console.WriteLine(String.Format("Mazani probehlo {0}", a ? "uspesne." : "neuspechem."));



            // funkce 2. e)
            // Změna rezervace ( transakce )
            /*
            Console.WriteLine("\nfunkce 2. d)");
            bool bbl = RezervaceTable.ChangeReservation(1, new DateTime(2020, 6, 19, 10, 0, 0));
            Console.WriteLine(String.Format("Zmena rezervace byla {0}", bbl ? "uspesna." : "neuspesna."));
            */


            // funkce 2. f)
            // Vlozeni navstevy bez rezervace
            /*
            Console.WriteLine("\nfunkce 2. f)");
            int count2 = NavstevaTable.VlozitNavstevu(34, null, 1);
            if (count2 > 0)
                Console.WriteLine("Navsteva zaevidovana");

            // vlozeni navstevy s rezervaci
            int count2 = NavstevaTable.VlozitNavstevu(36, 1, 1);
            if (count2 > 0)
                Console.WriteLine("Navsteva zaevidovana");
            */



            // funkce 3. a)
            // Vytvoření výstavy
            /*
            Console.WriteLine("\nfunkce 3. a)");
            Vystava newVystava = new Vystava();
            DateTime zacatek_vystavy = new DateTime(2020, 1, 2, 7, 0, 0);

            newVystava.Popis_vystavy = "Tohle je prvni vystava v muzeu zamerena na Velkomoravske vykopavky";
            newVystava.Datum_zacatku_vystavy = zacatek_vystavy;
            newVystava.Datum_konce_vystavy = null;
            bool vlozeno = VystavaTable.InsertVystava(newVystava);
            Console.WriteLine(String.Format("Vystava vlozena {0}", vlozeno ? "uspesne." : "neuspesne."));
            */


            // funkce 3. b)
            // Zrušení výstavy
            /*
            Console.WriteLine("\nfunkce 3. b)");
            bool smazano = VystavaTable.DeleteVystavaById(3);
            Console.WriteLine(String.Format("{0}", smazano ? "Vystava uspesne smazana." : "Vystavu se nepodarilo smazat."));
            */


            // funkce 3. c)
            // Kontrola vytvořené výstavy
            /*
            Console.WriteLine("\nfunkce 3. c)");
            bool povedloSe = VystavaTable.KontrolaVytvoreneVystavy(4);
            Console.WriteLine(String.Format("{0}", povedloSe ? "Kontrola probehla uspesne." : "Kontrolu se nepodarilo provest."));
            */



            // funkce 4. a)
            // Přidání artefaktu
            /*
            Console.WriteLine("\nfunkce 4. a)");
            Artefakt newArtefakt = new Artefakt();
            newArtefakt.Nazev = "motyka";                       // povinny atribut
            DateTime datum_nalezeni = new DateTime(2015, 1, 2, 8, 0, 0);
            newArtefakt.Datum_nalezeni = datum_nalezeni;        // povinny atribut
            newArtefakt.Zeme_nalezu = "UK";                     // povinny atribut
            newArtefakt.Popis = "velmi stara motyka :]";
            newArtefakt.Je_prozkouman_a_zdokumentovan = true;   // povinny atribut
            newArtefakt.Archeolog_aID = 1;                      // povinny atribut

            bool bylPridan = ArtefaktTable.InsertArtefact(newArtefakt);
            Console.WriteLine(String.Format("{0}", bylPridan ? "Artefakt byl pridan." : "Artefakt se nepodarilo pridat."));
            */


            // funkce 4. b)
            // Vystavit artefakt do muzea
            /*
            Console.WriteLine("\nfunkce 4. b)");
            bool bylVystavene = Vystavene_artefaktyTable.VystavitArtefaktId(2, 1);
            Console.WriteLine(String.Format("{0}", bylVystavene ? "Artefakt byl vystaven." : "Artefakt se nepodarilo vystavit."));
            */


            // funkce 4. c)
            // Stáhnout artefakt z muzea do skladu
            /*
            Console.WriteLine("\nfunkce 4. c)");
            bool bylstazen = Vystavene_artefaktyTable.StahnoutArtefaktId(2);
            Console.WriteLine(String.Format("{0}", bylstazen ? "Artefakt byl uspesne stazen z vystavy." : "Artefakt se nepodarilo stahnout z vystavy."));
            */



            // funkce 4. d)
            // Výpis všech artefaktů
            Console.WriteLine("\nfunkce 4. d)");
            Collection<Artefakt> artefakt = ArtefaktTable.VypisVsechArtefaktu();
            Console.WriteLine("{aID} {Nazev} \t {Datum_nalezeni} \t {Odhadovane_stari_artefaktu} \t {Zeme_nalezu} {Zeme_puvodu_artefaktu} {GPS_souradnice_nalezu} {Popis} {Datum_vystaveni} \t {Je_prozkouman_a_zdokumentovan} {Datum_posledni_kontroly} {Vypujceno_od} {Propujceno_muzeu} {Archeolog_aID}");
            foreach (var art in artefakt)
            {
                string datum_vystaveni; 
                if(art.Datum_vystaveni != null)
                {
                    DateTime dt = (DateTime)art.Datum_vystaveni;
                    datum_vystaveni = dt.ToString("yyyy MM dd HH:mm:ss");
                }
                else
                {
                    datum_vystaveni = string.Empty;
                }
                string datum_posledni_kontroly;
                if (art.Datum_posledni_kontroly != null)
                {
                    DateTime dtpk = (DateTime)art.Datum_posledni_kontroly;
                    datum_posledni_kontroly = dtpk.ToString("yyyy MM dd HH:mm:ss");
                }
                else
                {
                    datum_posledni_kontroly = string.Empty;
                }

                Console.WriteLine($"{art.aID}    {art.Nazev} \t {art.Datum_nalezeni.ToString("yyyy MM dd HH:mm:ss")} \t {art.Odhadovane_stari_artefaktu} \t {art.Zeme_nalezu}    {art.Zeme_puvodu_artefaktu }    {art.GPS_souradnice_nalezu}     {art.Popis}    {datum_vystaveni} \t {art.Je_prozkouman_a_zdokumentovan}    {datum_posledni_kontroly}    {art.Vypujceno_od}    {art.Propujceno_muzeu}    {art.Archeolog_aID}");
            }




            // funkce 4. e)
            // Výpis všech vystavených artefaktů
            Console.WriteLine("\nfunkce 4. e)");
            Collection<Vystavene_artefakty> vystavene_artefakty = Vystavene_artefaktyTable.VypisIdVsechVystavenychArtefaktu();
            Console.WriteLine("{Artefakt_aID} {Vystava_vID}");
            foreach (var vart in vystavene_artefakty)
            {
                Console.WriteLine($"{vart.Artefakt_aID} \t\t {vart.Vystava_vID}");
            }




            // funkce 4. f)
            // Výpis všech artefaktů, které nejsou vystavené
            Console.WriteLine("\nfunkce 4. f)");
            Collection<Artefakt> nevystavene_artefakty = ArtefaktTable.VypisNevystavenychArtefaktu();
            Console.WriteLine("{aID} {Nazev} \t {Datum_nalezeni} \t {Odhadovane_stari_artefaktu} \t {Zeme_nalezu} {Zeme_puvodu_artefaktu} {GPS_souradnice_nalezu} {Popis} {Datum_vystaveni} \t {Je_prozkouman_a_zdokumentovan} {Datum_posledni_kontroly} {Vypujceno_od} {Propujceno_muzeu} {Archeolog_aID}");
            foreach (var art in nevystavene_artefakty)
            {
                string datum_vystaveni;
                if (art.Datum_vystaveni != null)
                {
                    DateTime dt = (DateTime)art.Datum_vystaveni;
                    datum_vystaveni = dt.ToString("yyyy MM dd HH:mm:ss");
                }
                else
                {
                    datum_vystaveni = string.Empty;
                }
                string datum_posledni_kontroly;
                if (art.Datum_posledni_kontroly != null)
                {
                    DateTime dtpk = (DateTime)art.Datum_posledni_kontroly;
                    datum_posledni_kontroly = dtpk.ToString("yyyy MM dd HH:mm:ss");
                }
                else
                {
                    datum_posledni_kontroly = string.Empty;
                }

                Console.WriteLine($"{art.aID}    {art.Nazev} \t {art.Datum_nalezeni.ToString("yyyy MM dd HH:mm:ss")} \t {art.Odhadovane_stari_artefaktu} \t {art.Zeme_nalezu}    {art.Zeme_puvodu_artefaktu }    {art.GPS_souradnice_nalezu}     {art.Popis}    {datum_vystaveni} \t {art.Je_prozkouman_a_zdokumentovan}    {datum_posledni_kontroly}    {art.Vypujceno_od}    {art.Propujceno_muzeu}    {art.Archeolog_aID}");
            }



            Console.ReadKey();
        }
    }
}
