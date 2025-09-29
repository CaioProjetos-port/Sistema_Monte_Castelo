using Monte_Castelo.Data;
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
    /// Interação lógica para PageCliente.xam
    /// </summary>
    public partial class PageCliente : Page
    {
        private Key _ultimaTecla;

        public PageCliente()
        {
            InitializeComponent();
        }

        // Função para salvar a ultima tecla pressionada no campo celular
        private void KeyPress(object sender, KeyEventArgs e)
        {
            _ultimaTecla = e.Key;
        }

        // Função para formatar o campo celular em tempo real
        private void Formatacao_celular(object sender, TextChangedEventArgs e)
        {
            string numero = xaml_celular.Text;

            if (numero.Length > 0)
                xaml_celular.Text = FuncoesDeVerificacao.FormatarNumCelular(numero, _ultimaTecla);
            xaml_celular.CaretIndex = xaml_celular.Text.Length;
        }
    }
}
