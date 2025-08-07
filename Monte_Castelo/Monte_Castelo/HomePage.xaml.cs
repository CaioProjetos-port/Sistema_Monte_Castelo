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

namespace Monte_Castelo
{
    public partial class HomePage : Page
    {
        // variaveis que ditam se os botões foram pressionados ou não
        bool btn1_on, btn2_on, btn3_on = false;

        public HomePage()
        {
            InitializeComponent();
        }

        // fuñções que verificam manipulam o estilo dos botões ao serem pressionados
        void btn1Pressed(object sender, RoutedEventArgs e)
        {
            if (btn1_on)
            {
                btn1_on = false;
                btn_menu_1.Style = (Style)FindResource("BotadoDeMenuLateral");
            }
            else
            {
                btn1_on = true;
                btn_menu_1.Style = (Style)FindResource("BotadoDeMenuLateralActive");
                PageFrame.Navigate(new PageAgenda());

                btn2_on = false;
                btn_menu_2.Style = (Style)FindResource("BotadoDeMenuLateral");

                btn3_on = false;
                btn_menu_3.Style = (Style)FindResource("BotadoDeMenuLateral");
            }
        }

        void btn2Pressed(object sender, RoutedEventArgs e)
        {
            if (btn2_on)
            {
                btn2_on = false;
                btn_menu_2.Style = (Style)FindResource("BotadoDeMenuLateral");
            }
            else
            {
                btn1_on = false;
                btn_menu_1.Style = (Style)FindResource("BotadoDeMenuLateral");

                btn2_on = true;
                btn_menu_2.Style = (Style)FindResource("BotadoDeMenuLateralActive");

                btn3_on = false;
                btn_menu_3.Style = (Style)FindResource("BotadoDeMenuLateral");

            }
        }

        void btn3Pressed(object sender, RoutedEventArgs e)
        {
            if (btn3_on)
            {
                btn3_on = false;
                btn_menu_3.Style = (Style)FindResource("BotadoDeMenuLateral");
            }
            else
            {
                btn1_on = false;
                btn_menu_1.Style = (Style)FindResource("BotadoDeMenuLateral");

                btn2_on = false;
                btn_menu_2.Style = (Style)FindResource("BotadoDeMenuLateral");

                btn3_on = true;
                btn_menu_3.Style = (Style)FindResource("BotadoDeMenuLateralActive");
            }
        }
    }
}
