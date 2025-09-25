using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using Monte_Castelo.Config;
using System.Data.SQLite;

namespace Monte_Castelo
{
    /// <summary>
    /// Interação lógica para PageAgenda.xam
    /// </summary>
    public partial class PageAgenda : Page
    {
        public ObservableCollection<Festa> Festas { get; set; }
        private Key _ultimaTecla;

        public PageAgenda()
        {
            InitializeComponent();
            Festas = new ObservableCollection<Festa>();
            DataContext = this;
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
    }

    public class FuncoesDeVerificacao
    {
        public static string VerificarCamposNumericos(PageAgenda pagina)
        {
            string[] campos = {
                pagina.xaml_qntd_convidados.Text,
                pagina.xaml_qntd_convidados_np.Text,
                pagina.xaml_extras_valor.Text,
                pagina.xaml_valor_entrada.Text,
                pagina.xaml_num_parcelas.Text,
                pagina.xaml_valor_desconto.Text
            };

            Regex[] regex = {
                RegexER.regex_numeros,
                RegexER.regex_numeros,
                RegexER.regex_valores,
                RegexER.regex_valores,
                RegexER.regex_numeros,
                RegexER.regex_valores_percentuais,
            };

            string convidados = campos[0];
            string convidados_np = campos[1];
            string desconto = campos[5];

            for (int i = 0; i < campos.Length; i++)
            {
                string msg = ValidarRegex(campos[i], regex[i], i + 9);
                if (msg != null)
                    return msg;
            }

            int convidados_int = int.Parse(convidados);
            int convidados_np_int = int.Parse(convidados_np);
            if (desconto.Contains("%"))
            {
                desconto = desconto.Replace("%", "");
                float desconto_float = float.Parse(desconto);

                if (desconto_float < 0 || desconto_float > 100)
                    return "Valor do desconto inválido";

            }

            if (convidados_int < 50)
                return "Não há pacotes para menos de 50 convidados";

            if ((convidados_int - convidados_np_int) < 50)
                return "Não há pacotes para menos de 50 convidados";

            return null;
        }

        public static string VerificarCamposDeDados(PageAgenda pagina)
        {
            string[] campos = {
                pagina.xaml_cliente.Text,
                pagina.xaml_aniversariante.Text,
                pagina.xaml_email.Text,
                pagina.xaml_endereco.Text,
                pagina.xaml_tema.Text,
                pagina.xaml_data.Text,
                pagina.xaml_horario.Text,
                pagina.xaml_criancas.Text
            };

            Regex[] regex = {
                RegexER.regex_nomes,
                RegexER.regex_nome,
                RegexER.regex_email,
                RegexER.regex_endereco,
                RegexER.regex_descricao,
                RegexER.regex_data,
                RegexER.regex_hora,
                RegexER.regex_numeros
            };

            string tipoFesta = pagina.xaml_tipo_festa.Text;
            string tipoFestaOutro = pagina.xaml_tdf_outro.Text;
            string extrasDescricao = pagina.xaml_extras_descricao.Text;
            string extrasValor = pagina.xaml_extras_valor.Text;
            string celular = pagina.xaml_celular.Text;

            for (int i = 0; i < campos.Length; i++)
            {
                string msg = ValidarRegex(campos[i], regex[i], i);
                if (msg != null)
                    return msg;
            }

            if (tipoFesta == "Outro" && tipoFestaOutro == "")
                return "insira o tipo de festa";
            else
            {
                string msg = ValidarRegex(tipoFestaOutro, regex[4], 8);
                if (msg != null)
                    return msg;
            }

            if (extrasDescricao == "" && extrasValor != "0")
                return "insira a descrição dos extras";
            else if (extrasDescricao != "" && extrasValor == "0")
                return "insira o valor dos extras";

            if (celular.Length > 15 || celular.Length < 14)
                return "numero de celular inválido";

            return null;
        }

        public static string FormatarNumCelular(string celular, Key k)
        {
            if (celular.Length == 2 && k != Key.Back)
                celular = "(" + celular.Substring(0, 2) + ")";

            if (celular.Length == 4 && k != Key.Back)
            {
                celular = celular.Replace("(", "").Replace(")", "");
                celular = "(" + celular.Substring(0, 2) + ")";
            }

            if (celular.Length == 12)
            {
                celular = celular.Replace(".", "").Replace("-", "");
                celular = celular.Insert(8, "-");
            }

            if (celular.Length == 14 && k != Key.Back)
            {
                celular = celular.Replace(".", "").Replace("-", "");
                celular = celular.Insert(5, ".").Insert(10, "-");
            }

            if (celular.Length > 15)
                return celular.Substring(0, 14);

            return celular;
            // (88)9.8155-5424
        }

        public static float ValorDoPacoteKids(int convidados_qntd)
        {
            float pacote_vlr = 0;

            if (convidados_qntd < 60)   //  de 50 até 59
            {
                pacote_vlr = ValoresDasFestas.kids50;
                pacote_vlr += ValoresDasFestas.convidado_kids * (convidados_qntd % 50);
            }
            else if (convidados_qntd >= 60 && convidados_qntd < 70)     // de 60 até 69
            {
                pacote_vlr = ValoresDasFestas.kids60;
                pacote_vlr += ValoresDasFestas.convidado_kids * (convidados_qntd % 60);
            }
            else if (convidados_qntd >= 70 && convidados_qntd < 80)     // de 70 até 79
            {
                pacote_vlr = ValoresDasFestas.kids70;
                pacote_vlr += ValoresDasFestas.convidado_kids * (convidados_qntd % 70);
            }
            else if (convidados_qntd >= 80 && convidados_qntd < 90)     // de 80 até 89
            {
                pacote_vlr = ValoresDasFestas.kids80;
                pacote_vlr += ValoresDasFestas.convidado_kids * (convidados_qntd % 80);
            }
            else if (convidados_qntd >= 90 && convidados_qntd < 100)    // de 90 até 99
            {
                pacote_vlr = ValoresDasFestas.kids90;
                pacote_vlr += ValoresDasFestas.convidado_kids * (convidados_qntd % 90);
            }
            else if (convidados_qntd >= 100)                            // de 100 em diante
            {
                pacote_vlr = ValoresDasFestas.kids100;
                pacote_vlr += ValoresDasFestas.convidado_kids * (convidados_qntd % 100);
            }

            return pacote_vlr;
        }

        public static float ValorDoPacoteSonho(int convidados_qntd)
        {
            float pacote_vlr = 0;

            if (convidados_qntd < 60)   //  de 50 até 59
            {
                pacote_vlr = ValoresDasFestas.sonho50;
                pacote_vlr += ValoresDasFestas.convidado_kids * (convidados_qntd % 50);
            }
            else if (convidados_qntd >= 60 && convidados_qntd < 70)     // de 60 até 69
            {
                pacote_vlr = ValoresDasFestas.sonho60;
                pacote_vlr += ValoresDasFestas.convidado_sonho * (convidados_qntd % 60);
            }
            else if (convidados_qntd >= 70 && convidados_qntd < 80)     // de 70 até 79
            {
                pacote_vlr = ValoresDasFestas.sonho70;
                pacote_vlr += ValoresDasFestas.convidado_sonho * (convidados_qntd % 70);
            }
            else if (convidados_qntd >= 80 && convidados_qntd < 90)     // de 80 até 89
            {
                pacote_vlr = ValoresDasFestas.sonho80;
                pacote_vlr += ValoresDasFestas.convidado_sonho * (convidados_qntd % 80);
            }
            else if (convidados_qntd >= 90 && convidados_qntd < 100)    // de 90 até 99
            {
                pacote_vlr = ValoresDasFestas.sonho90;
                pacote_vlr += ValoresDasFestas.convidado_sonho * (convidados_qntd % 90);
            }
            else if (convidados_qntd >= 100)                            // de 100 em diante
            {
                pacote_vlr = ValoresDasFestas.sonho100;
                pacote_vlr += ValoresDasFestas.convidado_sonho * (convidados_qntd % 100);
            }

            return pacote_vlr;
        }

        public static float ValorDoPacoteEncanto(int convidados_qntd)
        {
            float pacote_vlr = 0;

            if (convidados_qntd < 60)   //  de 50 até 59
            {
                pacote_vlr = ValoresDasFestas.encanto50;
                pacote_vlr += ValoresDasFestas.convidado_encanto * (convidados_qntd % 50);
            }
            else if (convidados_qntd >= 60 && convidados_qntd < 70)     // de 60 até 69
            {
                pacote_vlr = ValoresDasFestas.encanto60;
                pacote_vlr += ValoresDasFestas.convidado_encanto * (convidados_qntd % 60);
            }
            else if (convidados_qntd >= 70 && convidados_qntd < 80)     // de 70 até 79
            {
                pacote_vlr = ValoresDasFestas.encanto70;
                pacote_vlr += ValoresDasFestas.convidado_encanto * (convidados_qntd % 70);
            }
            else if (convidados_qntd >= 80 && convidados_qntd < 90)     // de 80 até 89
            {
                pacote_vlr = ValoresDasFestas.encanto80;
                pacote_vlr += ValoresDasFestas.convidado_encanto * (convidados_qntd % 80);
            }
            else if (convidados_qntd >= 90 && convidados_qntd < 100)    // de 90 até 99
            {
                pacote_vlr = ValoresDasFestas.encanto90;
                pacote_vlr += ValoresDasFestas.convidado_encanto * (convidados_qntd % 90);
            }
            else if (convidados_qntd >= 100)                            // de 100 em diante
            {
                pacote_vlr = ValoresDasFestas.encanto100;
                pacote_vlr += ValoresDasFestas.convidado_encanto * (convidados_qntd % 100);
            }

            return pacote_vlr;
        }

        public static float ValorDoPacoteReino(int convidados_qntd)
        {
            float pacote_vlr = 0;

            if (convidados_qntd < 60)   //  de 50 até 59
            {
                pacote_vlr = ValoresDasFestas.reino50;
                pacote_vlr += ValoresDasFestas.convidado_reino * (convidados_qntd % 50);
            }
            else if (convidados_qntd >= 60 && convidados_qntd < 70)     // de 60 até 69
            {
                pacote_vlr = ValoresDasFestas.reino60;
                pacote_vlr += ValoresDasFestas.convidado_reino * (convidados_qntd % 60);
            }
            else if (convidados_qntd >= 70 && convidados_qntd < 80)     // de 70 até 79
            {
                pacote_vlr = ValoresDasFestas.reino70;
                pacote_vlr += ValoresDasFestas.convidado_reino * (convidados_qntd % 70);
            }
            else if (convidados_qntd >= 80 && convidados_qntd < 90)     // de 80 até 89
            {
                pacote_vlr = ValoresDasFestas.reino80;
                pacote_vlr += ValoresDasFestas.convidado_reino * (convidados_qntd % 80);
            }
            else if (convidados_qntd >= 90 && convidados_qntd < 100)    // de 90 até 99
            {
                pacote_vlr = ValoresDasFestas.reino90;
                pacote_vlr += ValoresDasFestas.convidado_reino * (convidados_qntd % 90);
            }
            else if (convidados_qntd >= 100)                            // de 100 em diante
            {
                pacote_vlr = ValoresDasFestas.reino100;
                pacote_vlr += ValoresDasFestas.convidado_reino * (convidados_qntd % 100);
            }

            return pacote_vlr;
        }

        private static string ValidarRegex(string campo, Regex regex, int index)
        {
            string[] erros = {
                "Nome do cliente inválido", // indice 0, primeiro do loop campo de dados
                "Nome do aniversariante inválido",
                "Email inválido",
                "Endereço inválido",
                "Tema inválido",
                "Data inválida",
                "Hora inválida",
                "Quantidade de crianças inválida", // indice 7, ultimo do loop campo de dados
                "Preencha o tipo de festa", // indice 8, campo de dados aparte
                "Quantidade de convidados inválida", // indice 9, primeiro do loop campo numerico
                "Quantidade de convidados não pagantes inválida",
                "Valor dos extras inválida",
                "Valor da entrada inválida",
                "Quantidade das parcelas inválida",
                "Valor do desconto inválido" // indice 14, ultimo do loop campo numerico
            };

            Match match = regex.Match(campo);
            if (!match.Success)
                return erros[index];

            return null;
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
            AdicionarALista(pagina);
            LimparCampos(pagina);
            //AdicionarAoBD(pagina);
        }

        private static void AdicionarALista(PageAgenda pagina)
        {
            pagina.Festas.Add(new Festa
            {
                Cliente = pagina.xaml_cliente.Text,
                Aniversariante = pagina.xaml_aniversariante.Text,
                Data = pagina.xaml_data.Text,
                Hora = pagina.xaml_horario.Text,
                Tema = pagina.xaml_tema.Text,
                Convidados = pagina.xaml_qntd_convidados.Text,
                Pacote = pagina.xaml_pacote.Text
            });
        }

        private static void LimparCampos(PageAgenda pagina)
        {
            pagina.xaml_cliente.Clear();
            pagina.xaml_aniversariante.Clear();
            pagina.xaml_celular.Clear();
            pagina.xaml_email.Clear();
            pagina.xaml_endereco.Clear();
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

        private static void AdicionarAoBD(PageAgenda pagina)
        {
            using (SQLiteConnection conn = new SQLiteConnection(DB.conection))
            {
                conn.Open();
                DB.CriarTabelasDeFestas();

            }

        }

        private static void LimparCampos()
        {

        }
    }
}