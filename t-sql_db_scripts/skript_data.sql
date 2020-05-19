-- skript_data.sql

-- Pruvodce
INSERT INTO pruvodce (jmeno, prijmeni, mobilni_cislo)
VALUES ('Jan', 'Sochor', 608482568);

INSERT INTO pruvodce (jmeno, prijmeni, mobilni_cislo)
VALUES ('Pavel', 'Hvozd', 608543102);


-- Rezervace - YYYYMMDD
INSERT INTO rezervace (jmeno, prijmeni, pocet_osob, zarezervovane_datum, datum_vytvoreni, pruvodce_pid)
VALUES ('Andre', 'Wasinsky', 23, '20190618 10:00:00', '20190610 15:36:48', 1);

INSERT INTO rezervace (jmeno, prijmeni, pocet_osob, zarezervovane_datum, datum_vytvoreni, pruvodce_pid)
VALUES ('Marie', 'Dolova', 15, '20190619 15:00:00', '20190601 19:48:02', 1);

INSERT INTO rezervace (jmeno, prijmeni, pocet_osob, zarezervovane_datum, datum_vytvoreni, pruvodce_pid)
VALUES ('Pudger', 'Hnus', 18, '20190620 09:00:00', '20190609 23:59:59', 2);


-- Recepce
INSERT INTO recepce(jmeno, prijmeni, mobilni_cislo)
VALUES ('Anna', 'Norova', 589482126);

INSERT INTO recepce(jmeno, prijmeni, email, mobilni_cislo)
VALUES ('Diana', 'Nova', 'diana.nova@seznam.cz', 589482126);


-- Vystava
INSERT INTO vystava(popis_vystavy, datum_zacatku_vystavy)
VALUES ('Vykop�vky Velk� Moravy', '20190101 08:00:00');


-- Navsteva
-- navstevy bez rezervace
INSERT INTO navsteva(pocet_osob, datum_cas_navstevy, recepce_rid, vystava_vid)
VALUES (1, '20190101 08:00:00', 1, 1);
INSERT INTO navsteva(pocet_osob, datum_cas_navstevy, recepce_rid, vystava_vid)
VALUES (1, '20190101 08:05:49', 1, 1);
INSERT INTO navsteva(pocet_osob, datum_cas_navstevy, recepce_rid, vystava_vid)
VALUES (4, '20190102 17:01:23', 2, 1);
INSERT INTO navsteva(pocet_osob, datum_cas_navstevy, recepce_rid, vystava_vid)
VALUES (2, '20190103 12:00:50', 2, 1);
INSERT INTO navsteva(pocet_osob, datum_cas_navstevy, recepce_rid, vystava_vid)
VALUES (1, '20190103 13:12:11', 2, 1);

-- navstevy s rezervaci
INSERT INTO navsteva(pocet_osob, datum_cas_navstevy, rezervace_rid, recepce_rid, vystava_vid)
VALUES (23, '20190101 08:00:20', 1, 1, 1);
INSERT INTO navsteva(pocet_osob, datum_cas_navstevy, rezervace_rid, recepce_rid, vystava_vid)
VALUES (10, '20190619 14:54:03', 2, 1, 1);
INSERT INTO navsteva(pocet_osob, datum_cas_navstevy, rezervace_rid, recepce_rid, vystava_vid)
VALUES (18, '20190620 08:58:00', 3, 2, 1);


-- Archeolog
INSERT INTO archeolog(jmeno, prijmeni, mobilni_cislo, email)
VALUES ('Mat�j', 'Vozek', 685043127, 'matej.vozek@gmail.com');
INSERT INTO archeolog(jmeno, prijmeni, mobilni_cislo, email)
VALUES ('Jan', 'M�lek', 686033212, 'jan.malek@gmail.com');


-- Artefakt
INSERT INTO artefakt(nazev, datum_nalezeni, zeme_nalezu, je_prozkouman_a_zdokumentovan, datum_posledni_kontroly, archeolog_aid)
VALUES ('Okovan� v�dro', '20111105 16:00:00', '�r', 1, '2020-04-01 10:00:00.000', 1);
INSERT INTO artefakt(nazev, datum_nalezeni, zeme_nalezu, je_prozkouman_a_zdokumentovan, datum_posledni_kontroly, archeolog_aid, popis)
VALUES ('Parohov� podlo�ka', '20101105 13:00:00', '�r', 1, '2020-01-01 15:00:00.000', 1, 'Parohov� podlo�ka k vytep�v�n� zlat�ch a st��brn�ch gomb�k�');
INSERT INTO artefakt(nazev, datum_nalezeni, zeme_nalezu, je_prozkouman_a_zdokumentovan, archeolog_aid)
VALUES ('��st ostruhy', '20101105 13:00:00', '�r', 1, 1);
INSERT INTO artefakt(nazev, datum_nalezeni, zeme_nalezu, je_prozkouman_a_zdokumentovan, archeolog_aid)
VALUES ('Doklady kov��stv�', '20080601 10:20:00', '�r', 1, 1);
INSERT INTO artefakt(nazev, datum_nalezeni, zeme_nalezu, je_prozkouman_a_zdokumentovan, archeolog_aid)
VALUES ('Doklady kovotepectv�', '20080601 10:10:00', '�r', 1, 1);
INSERT INTO artefakt(nazev, datum_nalezeni, zeme_nalezu, je_prozkouman_a_zdokumentovan, archeolog_aid)
VALUES ('Doklady kovolitectv�', '20080601 10:00:00', '�r', 1, 1);
INSERT INTO artefakt(nazev, datum_nalezeni, zeme_nalezu, je_prozkouman_a_zdokumentovan, archeolog_aid)
VALUES ('Kadluby - lic� formy', '20080601 15:00:00', '�r', 1, 1);
INSERT INTO artefakt(nazev, datum_nalezeni, zeme_nalezu, je_prozkouman_a_zdokumentovan, archeolog_aid)
VALUES ('Tyglik a �lomky n�dob', '20080602 09:10:00', '�r', 1, 1);
INSERT INTO artefakt(nazev, datum_nalezeni, zeme_nalezu, je_prozkouman_a_zdokumentovan, archeolog_aid)
VALUES ('��st rota�n�ho brusu', '20080603 14:30:00', '�r', 1, 1);
INSERT INTO artefakt(nazev, datum_nalezeni, zeme_nalezu, je_prozkouman_a_zdokumentovan, archeolog_aid)
VALUES ('Dyznov� cihla kovoliteck� pece', '20080613 11:21:38', '�r', 1, 1);
INSERT INTO artefakt(nazev, datum_nalezeni, zeme_nalezu, je_prozkouman_a_zdokumentovan, archeolog_aid)
VALUES ('�elez��sk� strusky a ruda', '20021202 15:46:00', '�r', 1, 1);


-- Vystavene artefakty
INSERT INTO vystavene_artefakty(artefakt_aid, vystava_vid)
VALUES (1, 1);
INSERT INTO vystavene_artefakty(artefakt_aid, vystava_vid)
VALUES (2, 1);
INSERT INTO vystavene_artefakty(artefakt_aid, vystava_vid)
VALUES (3, 1);
INSERT INTO vystavene_artefakty(artefakt_aid, vystava_vid)
VALUES (4, 1);
INSERT INTO vystavene_artefakty(artefakt_aid, vystava_vid)
VALUES (5, 1);
INSERT INTO vystavene_artefakty(artefakt_aid, vystava_vid)
VALUES (6, 1);
INSERT INTO vystavene_artefakty(artefakt_aid, vystava_vid)
VALUES (8, 1);
INSERT INTO vystavene_artefakty(artefakt_aid, vystava_vid)
VALUES (9, 1);
INSERT INTO vystavene_artefakty(artefakt_aid, vystava_vid)
VALUES (10, 1);

