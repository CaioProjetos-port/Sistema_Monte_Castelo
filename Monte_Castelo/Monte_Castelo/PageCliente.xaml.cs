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
            CarregarDadosSemTravar();
        }

        private async void CarregarDadosSemTravar()
        {
            // Opcional: Mostre uma barrinha de carregamento ou mude o cursor
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                // Roda o acesso ao banco em uma Thread separada
                await Task.Run(() =>
                {
                    // O código pesado fica aqui
                    // CUIDADO: Você não pode mexer na UI (elementos xaml) aqui dentro diretamente
                    SalvarCliente.IniciarListaDeClientes(this);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar: " + ex.Message);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        // Função para completar os dados da ListView
        private void ListaDeClientes_Loaded(object sender, RoutedEventArgs e)
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

        // Função para salvar a ultima tecla pressionada no campo celular e cpf
        private void KeyPress(object sender, KeyEventArgs e)
        {
            _ultimaTecla = e.Key;
        }

        // Função para formatar o campo celular em tempo real
        private void formatarCelular(object sender, TextChangedEventArgs e)
        {
            string numero = xaml_celular_cliente.Text;

            if (numero.Length > 0)
                xaml_celular_cliente.Text = FuncoesDeVerificacao.formatarNumCelular(numero, _ultimaTecla);
            xaml_celular_cliente.CaretIndex = xaml_celular_cliente.Text.Length;
        }

        // Função para formatar o campo cpf em tempo real
        private void formatarCpf(object sender, TextChangedEventArgs e)
        {
            string cpf = xaml_cpf_cliente.Text;

            if (cpf.Length > 0)
            
                xaml_cpf_cliente.Text = FuncoesDeVerificacao.formatarCpf(cpf, _ultimaTecla);
            xaml_cpf_cliente.CaretIndex = xaml_cpf_cliente.Text.Length;
        }

        // Função para formatar o campo cpf em tempo real
        private void formatarCep(object sender, TextChangedEventArgs e)
        {
            string cpf = xaml_cep.Text;

            if (cpf.Length > 0)
            
                xaml_cep.Text = FuncoesDeVerificacao.formatarCep(cpf, _ultimaTecla);
            xaml_cep.CaretIndex = xaml_cep.Text.Length;
        }

        // Função para salvar o cliente no banco de dados
        private void salvarCliente(object sender, RoutedEventArgs e)
        {
            if (validarCliente() == 1)
                return;

            SalvarCliente.Salvar(this);
        }

        private int validarCliente()
        {
            string msg_erro = FuncoesDeVerificacao.VerificarCamposDoCliente(this);

            if (msg_erro != null)
            {
                MessageBox.Show(msg_erro);
                return 1;
            }

            return 0;
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
            pagina.xaml_cpf_cliente.Clear();
            pagina.xaml_nome_cliente.Clear();
            pagina.xaml_sobrenome_cliente.Clear();
            pagina.xaml_celular_cliente.Clear();
            pagina.xaml_email_cliente.Clear();
            pagina.xaml_cep.Clear();
            pagina.xaml_cidade.Clear();
            pagina.xaml_bairro.Clear();
            pagina.xaml_logradouro.Clear();
            pagina.xaml_numero.Clear();
            pagina.xaml_complemento.Clear();
        }

        private static bool AdicionarAoBD(PageCliente pagina)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConexaoBD.StringConexao))
            {
                conn.Open();
                BancoDeDados.SalvarCliente(conn, pagina);
            }
            return true;
        }

        public static void IniciarListaDeClientes(PageCliente pagina)
        {
            ObservableCollection<Cliente> listaDeClientes = new ObservableCollection<Cliente>();

            listaDeClientes = BancoDeDados.RetornarListaDeClientes(listaDeClientes);

            pagina.Dispatcher.Invoke(() =>
            {
                pagina.xaml_listaClientes.ItemsSource = listaDeClientes;
            });
        }
    }

    public class Cliente
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string celular { get; set; }
        public string email { get; set; }
        public string cpf { get; set; }
    }
}