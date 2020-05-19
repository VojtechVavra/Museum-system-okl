using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MuseumSystemDesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // prihlaseni nahodne recepcni do aplikace
            LoginRandomRecepce();
        }



        private void Btn_evidence_navstev_Click(object sender, RoutedEventArgs e)
        {
            rezervace_grid.Visibility = Visibility.Hidden;
            textBox_pocet.Text = "";
            checkBox_ma_rezervaci.IsChecked = false;
            vypis_rezervaci_grid.Visibility = Visibility.Hidden;
            textBlock_info_o_zaevidovani.Text = "";
            evidence_navstev_grid.Visibility = evidence_navstev_grid.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }
        private void Btn_rezervace_Click(object sender, RoutedEventArgs e)
        {
            evidence_navstev_grid.Visibility = Visibility.Hidden;
            rezervace_grid.Visibility = rezervace_grid.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        private void Btn_odhlasit_se_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        
        private void CheckBox_ma_rezervaci_Click(object sender, RoutedEventArgs e)
        {
            if(checkBox_ma_rezervaci != null && (bool)checkBox_ma_rezervaci.IsChecked)
            {
                vypis_rezervaci_grid.Visibility = Visibility.Visible;
            }
            else if (checkBox_ma_rezervaci != null)
            {
                vypis_rezervaci_grid.Visibility = Visibility.Hidden;
            }
        }

        private void Btn_vyhledat_rezervaci_Click(object sender, RoutedEventArgs e)
        {
            PrepareDate();
        }


        // od data
        private void TextBox_od_rok_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (checkYear(textBox_od_rok.Text) && ( textBox_do_rok.Text == "" || checkYear(textBox_do_rok.Text)))
            {
                textBox_od_mesic.IsEnabled = true;
                btn_vyhledat_rezervaci.IsEnabled = true;
            }
            else
            {
                textBox_od_mesic.IsEnabled = false;
                textBox_od_den.IsEnabled = false;
                textBox_od_hodina.IsEnabled = false;
                btn_vyhledat_rezervaci.IsEnabled = false;
            }
        }

        private void TextBox_od_mesic_TextChanged(object sender, TextChangedEventArgs e)
        {
            int mesic;
            string od_mesice = textBox_od_mesic.Text;
            bool res = int.TryParse(od_mesice, out mesic);

            if (checkMonth(textBox_od_mesic.Text) && (textBox_do_mesic.Text == "" || checkMonth(textBox_do_mesic.Text)))
            {
                textBox_od_den.IsEnabled = true;
                btn_vyhledat_rezervaci.IsEnabled = true;
            }
            else
            {
                textBox_od_den.IsEnabled = false;
                textBox_od_hodina.IsEnabled = false;
                btn_vyhledat_rezervaci.IsEnabled = false;
            }
        }

        private void TextBox_od_den_TextChanged(object sender, TextChangedEventArgs e)
        {
            int den;
            string od_dne = textBox_od_den.Text;
            bool res = int.TryParse(od_dne, out den);

            if (checkDay(textBox_od_den.Text) && (textBox_do_den.Text == "" || checkDay(textBox_do_den.Text)))
            {
                textBox_od_hodina.IsEnabled = true;
                btn_vyhledat_rezervaci.IsEnabled = true;
            }
            else
            {
                textBox_od_hodina.IsEnabled = false;
                btn_vyhledat_rezervaci.IsEnabled = false;
            }
        }

        private void TextBox_od_hodina_TextChanged(object sender, TextChangedEventArgs e)
        {
            int hodina;
            string od_hodiny = textBox_od_hodina.Text;
            bool res = int.TryParse(od_hodiny, out hodina);

            if (checkHour(textBox_od_hodina.Text) && (textBox_do_hodina.Text == "" || checkHour(textBox_do_hodina.Text)))
            {
                btn_vyhledat_rezervaci.IsEnabled = true;
            }
            else
            {
                btn_vyhledat_rezervaci.IsEnabled = false;
            }
        }


        // do data
        private void TextBox_do_rok_TextChanged(object sender, TextChangedEventArgs e)
        {
            int rok;
            string do_roku = textBox_do_rok.Text;
            bool res = int.TryParse(do_roku, out rok);

            if (checkYear(textBox_do_rok.Text) && (textBox_od_rok.Text == "" || checkYear(textBox_od_rok.Text)))
            {
                textBox_do_mesic.IsEnabled = true;
                btn_vyhledat_rezervaci.IsEnabled = true;
            }
            else
            {
                textBox_do_mesic.IsEnabled = false;
                textBox_do_den.IsEnabled = false;
                textBox_do_hodina.IsEnabled = false;
                btn_vyhledat_rezervaci.IsEnabled = false;
            }
        }

        private void TextBox_do_mesic_TextChanged(object sender, TextChangedEventArgs e)
        {
            int mesic;
            string do_mesice = textBox_do_mesic.Text;
            bool res = int.TryParse(do_mesice, out mesic);

            if (checkMonth(textBox_do_mesic.Text) && (textBox_od_mesic.Text == "" || checkMonth(textBox_od_mesic.Text)))
            {
                textBox_do_den.IsEnabled = true;
                btn_vyhledat_rezervaci.IsEnabled = true;
            }
            else
            {
                textBox_do_den.IsEnabled = false;
                textBox_do_hodina.IsEnabled = false;
                btn_vyhledat_rezervaci.IsEnabled = false;
            }
        }

        private void TextBox_do_den_TextChanged(object sender, TextChangedEventArgs e)
        {
            int den;
            string do_dne = textBox_do_den.Text;
            bool res = int.TryParse(do_dne, out den);

            if (checkDay(textBox_do_den.Text) && (textBox_od_den.Text == "" || checkDay(textBox_od_den.Text)))
            {
                textBox_do_hodina.IsEnabled = true;
                btn_vyhledat_rezervaci.IsEnabled = true;
            }
            else
            {
                textBox_do_hodina.IsEnabled = false;
                btn_vyhledat_rezervaci.IsEnabled = false;
            }
        }

        private void TextBox_do_hodina_TextChanged(object sender, TextChangedEventArgs e)
        {
            int hodina;
            string do_hodiny = textBox_do_hodina.Text;
            bool res = int.TryParse(do_hodiny, out hodina);

            if (checkHour(textBox_do_hodina.Text) && (textBox_od_hodina.Text == "" || checkHour(textBox_od_hodina.Text)))
            {
                btn_vyhledat_rezervaci.IsEnabled = true;
            }
            else
            {
                btn_vyhledat_rezervaci.IsEnabled = false;
            }
        }

        private void Btn_zaevidovat_Click(object sender, RoutedEventArgs e)
        {
            RegisterVisitors();
        }

        private void Btn_vypis_vsech_rezervaci_Click(object sender, RoutedEventArgs e)
        {
            upravit_rezervaci_grid.Visibility = Visibility.Hidden;
            vytvorit_rezervaci_grid.Visibility = Visibility.Hidden;

            vypis_rezervaci_grid2.Visibility = vypis_rezervaci_grid2.IsVisible ? Visibility.Hidden : Visibility.Visible;

            btn_zrusit_rezervaci.IsEnabled = vypis_rezervaci_grid2.IsVisible ? true : false;
            btn_upravit_rezervaci.IsEnabled = vypis_rezervaci_grid2.IsVisible ? true : false;

            textBlock_info_o_zruseni_rezervace.Text = "";

            if(vypis_rezervaci_grid2.IsVisible)
                GetReservations();
        }

        private void Btn_zrusit_rezervaci_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedReservation();
        }

        private void Btn_upravit_rezervaci_Click(object sender, RoutedEventArgs e)
        {
            btn_zrusit_rezervaci.IsEnabled = false;
            btn_upravit_rezervaci.IsEnabled = false;

            vypis_rezervaci_grid2.Visibility = Visibility.Hidden;
            upravit_rezervaci_grid.Visibility = Visibility.Visible;

            textBlock_info_o_editaci_rezervace.Text = "";
            EditReservation();
        }

        private void Slider_hour_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(textBlock_set_hour != null)
                textBlock_set_hour.Text = slider_hour.Value.ToString();
        }

        private void Slider_minute_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (textBlock_set_minute != null)
                textBlock_set_minute.Text = slider_minute.Value.ToString();
        }

        private void Button_potvrdit_zmenu_rezervace_Click(object sender, RoutedEventArgs e)
        {
            ChangeReservation();
        }

        private void Btn_create_reservation_Click(object sender, RoutedEventArgs e)
        {
            CreateReservation();
        }

        private void Btn_vytvorit_rezervaci_Click(object sender, RoutedEventArgs e)
        {
            vytvorit_rezervaci_grid.Visibility = vytvorit_rezervaci_grid.IsVisible ? Visibility.Hidden : Visibility.Visible;
            upravit_rezervaci_grid.Visibility = Visibility.Hidden;
            vypis_rezervaci_grid2.Visibility = Visibility.Hidden;

            btn_upravit_rezervaci.IsEnabled = false;
            btn_zrusit_rezervaci.IsEnabled = false;

            FillData2();
        }

        private void Slider_hour_new_reservation_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (textBlock_set_hour_2 != null)
                textBlock_set_hour_2.Text = slider_hour_new_reservation.Value.ToString();
        }

        private void Slider_minute_new_reservation_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (textBlock_set_minute_2 != null)
                textBlock_set_minute_2.Text = slider_minute_new_reservation.Value.ToString();
        }
    }
}
