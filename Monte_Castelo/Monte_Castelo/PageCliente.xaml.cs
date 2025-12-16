using Monte_Castelo.Config;
using Monte_Castelo.Data;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        // Função para completar os dados da ListView
        private void ListaDeFestas_Loaded(object sender, RoutedEventArgs e)
        {
            AjustarColunas(xaml_listaClientes);
        }

        // Função para ajustar as colunas da ListView
        private void AjustarColunas(ListView listView)
        {
            var gridView = listView.View as GridView;
            if (gridView == null) return;

            // desconta a largura da scrollbar vertical
            double totalWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;

            // se quiser todas iguais
            double colWidth = totalWidth / gridView.Columns.Count;

            foreach (var col in gridView.Columns)
            {
                col.Width = colWidth;
            }

            xaml_listaClientes.SizeChanged += (s, e) => AjustarColunas(xaml_listaClientes);

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

        private void Formatacao_cpf(object sender, TextChangedEventArgs e)
        {
            string cpf = xaml_cpf.Text;

            if (cpf.Length > 0)
            
                xaml_cpf.Text = FuncoesDeVerificacao.FormatarCpf(cpf, _ultimaTecla);
            xaml_cpf.CaretIndex = xaml_cpf.Text.Length;
            
        }

        // Função para salvar o cliente no banco de dados
        private void salvarCliente(object sender, RoutedEventArgs e)
        {

            SalvarCliente.Salvar(this);
        }

    }

    public class SalvarCliente
    {
        public static void Salvar(PageCliente pagina)
        {
            bool confirm;
            confirm = AdicionarAoBD(pagina);

            if (confirm)
                LimparCampos(pagina);
            IniciarListaDeClientes(pagina);
        }

        private static void LimparCampos(PageCliente pagina)
        {
            pagina.xaml_cpf.Clear();
            pagina.xaml_nome.Clear();
            pagina.xaml_sobreNome.Clear();
            pagina.xaml_celular.Clear();
            pagina.xaml_cep.Clear();
            pagina.xaml_numCasa.Clear();
            pagina.xaml_complemento.Clear();
            pagina.xaml_email.Clear();
        }

        private static bool AdicionarAoBD(PageCliente pagina)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(Acesso.conection))
            {
                conn.Open();

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        bool cliente = BancoDeDados.VerificarSeClienteExiste(conn, pagina.xaml_cpf.Text);
                        if (!cliente)
                        {
                            MessageBox.Show("Já há um cliente cadastrado com esse CPF.");
                            return false;
                        }

                        BancoDeDados.SalvarCliente(conn, pagina);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }

            return true;
        }

        public static void IniciarListaDeClientes(PageCliente pagina)
        {
            ObservableCollection<Festa> listaDeFestas = new ObservableCollection<Festa>();

            listaDeFestas = BancoDeDados.RetornarInformacoesDaListaDeFestas(listaDeFestas);

            pagina.xaml_listaClientes.ItemsSource = listaDeFestas;
        }
    }
}
