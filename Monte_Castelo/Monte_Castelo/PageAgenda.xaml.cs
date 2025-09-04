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
    /// Interação lógica para PageAgenda.xam
    /// </summary>
    public partial class PageAgenda : Page
    {
        public PageAgenda()
        {
            InitializeComponent();
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcularValorTotal();
        }

        private void CalcularValorTotal()
        {
            float valorTotal = 0;
            float valorpacote = 0;
            float valorDesconto = 0;
            float valorFinal = 0;
            int qntConvidados = int.Parse(xaml_convidados.Text) - int.Parse(xaml_convNpagantes.Text);

            switch (pacote_selecionado.SelectedIndex)
            {
                case 0:
                    valorpacote = 1000;
                    break;
                case 1:
                    valorpacote = 100;
                    break;
                case 2:
                    valorpacote = 200;
                    break;
                case 3:
                    valorpacote = 300;
                    break;
            }
        }
    }
}
