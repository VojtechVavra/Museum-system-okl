using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MuseumSystemORM.ORM;
using MuseumSystemORM.ORM.src_sql;

namespace MuseumSystemDesktopApp
{
    //public class Functionality
    public partial class MainWindow : Window
    {
        private int? id_logged_receptione;

        public bool LoginRandomRecepce()
        {
            Collection<object> recepcni = ZobrazeniZamestnancu.Select("recepce");
            if(recepcni.Count == 0)
            {
                jmeno_prihl_zamestnance.Text = "xxx_neprihlasen";
                prijmeni_prihl_zamestnance.Text = "xxx_neprihlasen";
                return false;
            }

            Random rnd = new Random();
            int rand_recepcni = rnd.Next(0, recepcni.Count);    // creates a number between 0 and (recepcni.Count - 1)

            string[] rec = (string[])recepcni[rand_recepcni];
            jmeno_prihl_zamestnance.Text = rec[1];
            prijmeni_prihl_zamestnance.Text = rec[2];
            id_logged_receptione = Convert.ToInt32(rec[0]);

            return true;
        }

        public void PrepareDate()
        {
            dataGrid_rezervace.ItemsSource = null;

            int from_year = Convert.ToInt32(textBox_od_rok.Text);

            int from_month;
            if (textBox_od_mesic.Text != "")
                from_month = Convert.ToInt32(textBox_od_mesic.Text);
            else
                from_month = 1;

            int from_day;
            if (textBox_od_den.Text != "")
                from_day = Convert.ToInt32(textBox_od_den.Text);
            else
                from_day = 1;

            int from_hour;
            if (textBox_od_hodina.Text != "")
                from_hour = Convert.ToInt32(textBox_od_hodina.Text);
            else
                from_hour = 0;

            DateTime from_date = new DateTime(from_year, from_month, from_day, from_hour, 0, 0);

            int to_year;
            if (textBox_do_rok.Text != "")
                to_year = Convert.ToInt32(textBox_do_rok.Text);
            else
                to_year = DateTime.Now.Year;

            int to_month;
            if (textBox_do_mesic.Text != "")
                to_month = Convert.ToInt32(textBox_do_mesic.Text);
            else
                to_month = 1;

            int to_day;
            if (textBox_do_den.Text != "")
                to_day = Convert.ToInt32(textBox_do_den.Text);
            else
                to_day = 1;

            int to_hour;
            if (textBox_do_hodina.Text != "")
                to_hour = Convert.ToInt32(textBox_do_hodina.Text);
            else
                to_hour = 0;

            DateTime to_date = new DateTime(to_year, to_month, to_day, to_hour, 0, 0);

            Collection<Rezervace> reservations = GetReservations(from_date, to_date);

            dataGrid_rezervace.ItemsSource = reservations;
        }

        public Collection<Rezervace> GetReservations(DateTime from_date, DateTime to_date)
        {
            Collection<Rezervace> reservations = RezervaceTable.SelectRezervaceOdDo(from_date, to_date);

            return reservations;
        }

        private bool checkYear(string year)
        {
            int rok;
            bool res = int.TryParse(year, out rok);

            return (res && rok >= 2000 && rok <= 2100);
        }

        private bool checkMonth(string month)
        {
            int mesic;
            bool res = int.TryParse(month, out mesic);

            return (res && mesic > 0 && mesic < 13);
        }

        private bool checkDay(string day)
        {
            int den;
            bool res = int.TryParse(day, out den);

            return (res && den > 0 && den < 32);
        }

        private bool checkHour(string hour)
        {
            int hodina;
            bool res = int.TryParse(hour, out hodina);

            return (res && hodina >= 0 && hodina < 24);
        }

        public void RegisterVisitors()
        {
            int id_reception = id_logged_receptione ?? 1;
            Rezervace reservation = (Rezervace)dataGrid_rezervace.SelectedItem;
            int? id_reservation = null;

            if(reservation != null)
            {
                id_reservation = reservation.rID;
            }

            int num_of_visitors;
            bool res = int.TryParse(textBox_pocet.Text, out num_of_visitors);
            
            if(res && num_of_visitors > 0)
            {
                // without reservation
                if (!(bool)checkBox_ma_rezervaci.IsChecked)
                {
                    int count = NavstevaTable.VlozitNavstevu(num_of_visitors, null, id_reception);
                    if (count > 0)
                        textBlock_info_o_zaevidovani.Text = "Navsteva zaevidovana";
                    else
                        textBlock_info_o_zaevidovani.Text = "Navsteva nebyla zaevidovana";
                }
                // with reservation
                else if ((bool)checkBox_ma_rezervaci.IsChecked && id_reservation != null)
                {
                    int count = NavstevaTable.VlozitNavstevu(num_of_visitors, id_reservation, id_reception);
                    if (count > 0)
                        textBlock_info_o_zaevidovani.Text = "Navsteva zaevidovana";
                    else
                        textBlock_info_o_zaevidovani.Text = "Navsteva nebyla zaevidovana";
                }
                else
                {
                    textBlock_info_o_zaevidovani.Text = "Navsteva nebyla zaevidovana";
                }
            }
            else
            {
                textBlock_info_o_zaevidovani.Text = "Navsteva nebyla zaevidovana";
            }

            textBox_pocet.Text = "";
            checkBox_ma_rezervaci.IsChecked = false;
            vypis_rezervaci_grid.Visibility = Visibility.Hidden;
        }

        private void GetReservations(string order = "DESC")
        {
            Collection<Rezervace> rezervace = RezervaceTable.SelectRezervace(order);
            dataGrid_vypis_rezervaci2.ItemsSource = null;
            dataGrid_vypis_rezervaci2.ItemsSource = rezervace;
            textBlock_pocet_rezervaci.Text = rezervace.Count().ToString();
        }

        private bool DeleteSelectedReservation()
        {
            Rezervace reservation = (Rezervace)dataGrid_vypis_rezervaci2.SelectedItem;
            int id_reservation;
            bool success = false;
            if (reservation != null)
            {
                id_reservation = reservation.rID;
                try
                {
                    success = RezervaceTable.DeleteRezervaceById(id_reservation);
                    if (success)
                    {
                        textBlock_info_o_zruseni_rezervace.Text = "Rezervace zrušena";
                        GetReservations();
                        return true;
                    }
                }
                catch (Exception)
                {
                    // exception - nelze zrusit rezervaci, protoze rezervace uz byla vyuzita tzn. ze navstevnik uz navstivil muzeum
                    textBlock_info_o_zruseni_rezervace.Text = "Rezervace nelze zrušit";
                    return false;
                    //throw;
                }
            }
            else
            {
                textBlock_info_o_zruseni_rezervace.Text = "Nebyla vybrána rezervace";
                return false;
            }

            return false;
        }

        private void EditReservation()
        {
            Rezervace reservation = (Rezervace)dataGrid_vypis_rezervaci2.SelectedItem;
            int id_reservation;

            if (reservation == null)
            {
                upravit_rezervaci_grid.Visibility = Visibility.Hidden;
                btn_vypis_vsech_rezervaci.Visibility = Visibility.Visible;

                textBlock_info_o_editaci_rezervace.Text = "Rezervace nebyla vybrána";
                //Console.WriteLine("reservation is NULL!");
                return;
            }

            id_reservation = reservation.rID;
            textBlock_id_rezervace_cislo.Text = id_reservation.ToString();

            FillData(reservation);
        }

        private void FillData(Rezervace reservation)
        {
            textBlock_puvodni_datum_rezervace.Text = reservation.Zarezervovane_datum.ToString();

            for (int i = 2017; i < 2022; i++)
            {
                comboBox_year.Items.Add(i.ToString());
            }
            comboBox_year.SelectedIndex = 3;

            for (int i = 1; i < 13; i++)
            {
                comboBox_month.Items.Add(i.ToString());
            }

            int reservation_month = reservation.Zarezervovane_datum.Month;
            comboBox_month.SelectedIndex = reservation_month - 1;

            for (int i = 1; i < 32; i++)
            {
                comboBox_day.Items.Add(i.ToString());
            }

            int reservation_day = reservation.Zarezervovane_datum.Day;
            comboBox_day.SelectedIndex = reservation_day - 1;

            slider_hour.Value = reservation.Zarezervovane_datum.Hour;
            textBlock_set_hour.Text = slider_hour.Value.ToString();
            slider_minute.Value = reservation.Zarezervovane_datum.Minute;
            textBlock_set_minute.Text = slider_minute.Value.ToString();
        }


        private void ChangeReservation()
        {
            int id_reservation = Convert.ToInt32(textBlock_id_rezervace_cislo.Text);
            int year = Convert.ToInt32(comboBox_year.SelectedItem);
            int month = Convert.ToInt32(comboBox_month.SelectedItem);
            int day = Convert.ToInt32(comboBox_day.SelectedItem);
            int hour = Convert.ToInt32(slider_hour.Value);
            int minute = Convert.ToInt32(slider_minute.Value);

            bool success = RezervaceTable.ChangeReservation(id_reservation, new DateTime(year, month, day, hour, minute, 0));

            if(success)
            {
                textBlock_info_o_editaci_rezervace.Text = "Změna byla provedena";
            }
            else
            {
                textBlock_info_o_editaci_rezervace.Text = "Změnu se nepodařilo provést. (max 3 zmeny)!";
            }
        }

        private void CreateReservation()
        {
            Rezervace newRezervace = new Rezervace();
            newRezervace.Jmeno = textBox_jmeno.Text;
            newRezervace.Prijmeni = textBox_prijmeni.Text;

            if (string.IsNullOrEmpty(textBox_jmeno.Text) || string.IsNullOrEmpty(textBox_prijmeni.Text))
            {
                textBlock_info_create.Text = "Není zadané jméno nebo příjmení!";
                return;
            }

            int n;
            bool isNumeric = int.TryParse(textBox_pocet_osob.Text, out n);

            if(!isNumeric)
            {
                textBlock_info_create.Text = "Neni zadáno platné číslo počtu lidí!";
                return;
            }
            newRezervace.Pocet_osob = n;

            int? year = Convert.ToInt32(comboBox_year_new_reservation.SelectedItem);
            int? month = Convert.ToInt32(comboBox_month_new_reservation.SelectedItem);
            int? day = Convert.ToInt32(comboBox_day_new_reservation.SelectedItem);
            
            if(year == null || month == null || day == null)
            {
                textBlock_info_create.Text = "Neni vybrán rok, nebo měsíc, nebo den!";
                return;
            }
            int year2 = (int)year;
            int month2 = (int)month;
            int day2 = (int)day;

            int hour = (int)slider_hour_new_reservation.Value;
            int minute = (int)slider_minute_new_reservation.Value;

            DateTime datum_rezervace = new DateTime(year2, month2, day2, hour, minute, 0);
            newRezervace.Zarezervovane_datum = datum_rezervace;

            int pruvodce = 0;
            if((bool)checkBox_pruvodce.IsChecked)
            {
                pruvodce = 1;
            }
            newRezervace.Pruvodce_pID = pruvodce;

            bool rezervace_vytvorena = RezervaceTable.CreateReservation(newRezervace);

            if(rezervace_vytvorena)
            {
                textBlock_info_create.Text = "Rezervace probehla uspesne.";
                UnselectData();
            }
            else
            {
                textBlock_info_create.Text = "Rezervaci se nepodarilo provest.";
            }
        }

        private void FillData2()
        {
            textBlock_info_create.Text = "";

            comboBox_year_new_reservation.Items.Clear();
            for (int i = 2017; i < 2022; i++)
            {
                comboBox_year_new_reservation.Items.Add(i.ToString());
            }

            comboBox_year_new_reservation.SelectedIndex = 3;

            comboBox_month_new_reservation.Items.Clear();
            for (int i = 1; i < 13; i++)
            {
                comboBox_month_new_reservation.Items.Add(i.ToString());
            }
            comboBox_month_new_reservation.SelectedIndex = 0;

            comboBox_day_new_reservation.Items.Clear();
            for (int i = 1; i < 32; i++)
            {
                comboBox_day_new_reservation.Items.Add(i.ToString());
            }
            comboBox_day_new_reservation.SelectedIndex = 0;


            slider_hour_new_reservation.Value = 8;
            textBlock_set_hour_2.Text = "8";
            slider_minute_new_reservation.Value = 0;
            textBlock_set_minute_2.Text = "0";
        }

        private void UnselectData()
        {
            textBox_jmeno.Text = "";
            textBox_prijmeni.Text = "";

            comboBox_year_new_reservation.SelectedIndex = 3;
            comboBox_month_new_reservation.SelectedIndex = 0;
            comboBox_day_new_reservation.SelectedIndex = 0;

            slider_hour_new_reservation.Value = 8;
            textBlock_set_hour_2.Text = "8";
            slider_minute_new_reservation.Value = 0;
            textBlock_set_minute_2.Text = "0";

            checkBox_pruvodce.IsChecked = false;
        }
    }
}
