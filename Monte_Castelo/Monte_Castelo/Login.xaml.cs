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
    /// <summary>
    /// Interação lógica para Login.xam
    /// </summary>
    public partial class Login : Page
    {
        private Frame _mainFrame;

        public Login(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            string senha = pswrd_box.Password;

            if (senha == "123")
            {
                _mainFrame.Navigate(new HomePage());
            }
            else
            {
                msg_erro_login.Visibility = Visibility.Visible;
            }
        }
    }
}
