using Monte_Castelo.Config;
using Monte_Castelo.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
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
        public ObservableCollection<Festa> Festas { get; set; }

        public PageAgenda()
        {
            InitializeComponent();
            Festas = new ObservableCollection<Festa>();
            DataContext = this;
        }

        private void ListaDeFestas_Loaded(object sender, RoutedEventArgs e)
        {
            AjustarColunas(xaml_lista_de_festas);
        }

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

            xaml_lista_de_festas.SizeChanged += (s, e) => AjustarColunas(xaml_lista_de_festas);

        }

        // Botão para calcular o valor da festa
        private void Button_Calcular( object sender, RoutedEventArgs e)
        {
            VerificarCalculo();
        }
        
        // Botão para salvar o evento
        private void Button_Salvar( object sender, RoutedEventArgs e)
        {
            if (VerificarCalculo() == 1)
                return;

            if (VerificarDados() == 1)
                return;

            SalvarEvento.Salvar(this);
        }

        // função que verifica se os campos numericos são validos
        private int VerificarCalculo()
        {
            // verifica se os campos estão vazios
            // se forem vazios, retorna true e fecha a função button_Calcular
            string msg_erro = FuncoesDeVerificacao.VerificarCamposNumericos(this);

            if (msg_erro != null)
            {
                MessageBox.Show(msg_erro);
                return 1;
            }

            CalcularValores();
            return 0;
        }

        // verifica se todos os campos de dados foram preenchidos
        private int VerificarDados()
        {
            string msg_erro = FuncoesDeVerificacao.VerificarCamposDeDados(this);

            if (msg_erro != null)
            {
                MessageBox.Show(msg_erro);
                return 1;
            }

            return 0;
        }

        // função para calcular os valores da festa
        private void CalcularValores()
        {
            string pacote_str = xaml_pacote.Text;
            string pagamento_str = xaml_forma_pagamento.Text;
            float entrada_vlr = float.Parse(xaml_valor_entrada.Text);
            float extras_vlr = float.Parse(xaml_extras_valor.Text);
            int parcelas_num = int.Parse(xaml_num_parcelas.Text);
            int convidados_qntd = int.Parse(xaml_qntd_convidados.Text) - int.Parse(xaml_qntd_convidados_np.Text);

            float pacote_vlr = 0;
            float valor_total = 0;
            float valor_final = 0;

            if (pacote_str == "Kids")
                pacote_vlr = FuncoesDeVerificacao.ValorDoPacoteKids(convidados_qntd);
            
            else if (pacote_str == "Sonho")
                pacote_vlr = FuncoesDeVerificacao.ValorDoPacoteSonho(convidados_qntd);
            
            else if (pacote_str == "Encanto")
                pacote_vlr = FuncoesDeVerificacao.ValorDoPacoteEncanto(convidados_qntd);

            else if (pacote_str == "Reino")
                pacote_vlr = FuncoesDeVerificacao.ValorDoPacoteReino(convidados_qntd);

            if (pagamento_str == "Crédito C/J")
            {
                float taxa = (pacote_vlr + extras_vlr - entrada_vlr) * ValoresDasFestas.taxa_cartao;
                valor_total = pacote_vlr + extras_vlr + taxa;
            }
            else
                valor_total = pacote_vlr + extras_vlr;
            

            float desconto_vlr;
            if (xaml_valor_desconto.Text.Contains("%"))
            {
                string desconto_str = xaml_valor_desconto.Text.Replace("%", "").Trim();
                desconto_vlr = float.Parse(desconto_str);

                valor_final = valor_total - ((desconto_vlr / 100) * valor_total);
            }
            else
            {
                desconto_vlr = float.Parse(xaml_valor_desconto.Text);
                valor_final = valor_total - desconto_vlr;
            }

            xaml_valor_total.Text = "R$ " + valor_total.ToString("F2");
            xaml_valor_final.Text = "R$ " + valor_final.ToString("F2");
        }
    }

    public class Festa
    {
        public string Cliente { get; set; }
        public string Aniversariante { get; set; }
        public string Data { get; set; }
        public string Hora { get; set; }
        public string Tema { get; set; }
        public string Convidados { get; set; }
        public string Pacote { get; set; }
    }

    public class SalvarEvento
    {
        public static void Salvar(PageAgenda pagina)
        {
            bool confirm;
            confirm = AdicionarAoBD(pagina);

            if (confirm)
                LimparCampos(pagina);
                IniciarListaDeFestas(pagina);
        }

        private static void LimparCampos(PageAgenda pagina)
        {
            pagina.xaml_cpf.Clear();
            pagina.xaml_aniversariante.Clear();
            pagina.xaml_idade.Clear();
            pagina.xaml_tdf_outro.Clear();
            pagina.xaml_tema.Clear();
            pagina.xaml_horario.Clear();
            pagina.xaml_qntd_convidados.Clear();
            pagina.xaml_qntd_convidados_np.Clear();
            pagina.xaml_criancas.Clear();
            pagina.xaml_extras_descricao.Clear();
            pagina.xaml_extras_valor.Clear();
            pagina.xaml_valor_entrada.Clear();
            pagina.xaml_num_parcelas.Clear();
            pagina.xaml_valor_desconto.Clear();
            pagina.xaml_valor_total.Text = "R$ 0,00";
            pagina.xaml_valor_final.Text = "R$ 0,00";
        }

        private static bool AdicionarAoBD(PageAgenda pagina)
        {
            using (SQLiteConnection conn = new SQLiteConnection(Acesso.conection))
            {
                conn.Open();

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        BancoDeDados.CriarTabelasAgendamento(conn);

                        bool cliente = BancoDeDados.VerificarSeClienteExiste(conn, pagina.xaml_cpf.Text);
                        if (!cliente)
                        {
                            MessageBox.Show("Cadastre o cliente antes de agendar uma festa.");
                            return false;
                        }

                        bool disponivel = BancoDeDados.VerificarDisponibilidaDeData(conn, pagina.xaml_data.Text);
                        if (!disponivel)
                        {
                            MessageBox.Show("Data indisponível.");
                            return false;
                        }

                        BancoDeDados.SalvarClientePagamentoFesta(conn, pagina);

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

        public static void IniciarListaDeFestas(PageAgenda pagina)
        {
            ObservableCollection<Festa> listaDeFestas = new ObservableCollection<Festa>();

            listaDeFestas = BancoDeDados.RetornarInformacoesDaListaDeFestas(listaDeFestas);

            pagina.xaml_lista_de_festas.ItemsSource = listaDeFestas;
        }
    }
}


