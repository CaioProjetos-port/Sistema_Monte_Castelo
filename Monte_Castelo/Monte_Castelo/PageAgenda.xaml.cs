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
using System.Text.RegularExpressions;

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

        private void Button_Calcular( object sender, RoutedEventArgs e)
        {
            VerificarCalculo();
        }

        private void Button_Salvar( object sender, RoutedEventArgs e)
        {
            if (VerificarCalculo() == 1)
                return;

            if (VerificarDados() == 1)
                return;

            // SalvarEvento();
        }

        private int VerificarCalculo()
        {
            // verifica se os campos estão vazios
            // se forem vazios, retorna true e fecha a função button_Calcular
            string msg_erro = FuncoesDeVerificacao.VerificarCamposVazios(
                xaml_pacote.Text,
                xaml_qntd_convidados.Text,
                xaml_qntd_convidados_np.Text,
                xaml_extras_valor.Text,
                xaml_forma_pagamento.Text,
                xaml_valor_entrada.Text,
                xaml_num_parcelas.Text,
                xaml_valor_desconto.Text);

            if (msg_erro != null)
            {
                MessageBox.Show(msg_erro);
                return 1;
            }

            msg_erro = FuncoesDeVerificacao.VerificarValoresDiferentes(
                xaml_qntd_convidados.Text,
                xaml_qntd_convidados_np.Text,
                xaml_extras_valor.Text,
                xaml_valor_entrada.Text,
                xaml_num_parcelas.Text,
                xaml_valor_desconto.Text);

            if (msg_erro != null)
            {
                MessageBox.Show(msg_erro);
                return 1;
            }

            CalcularValores();
            return 0;
        }

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
            {
                if (convidados_qntd < 60)   //  de 50 até 59
                {
                    pacote_vlr = ValoresPacotes.kids50;
                    pacote_vlr += ValoresPacotes.convidado_kids * (convidados_qntd % 50);
                }
                else if (convidados_qntd >= 60 && convidados_qntd < 70)     // de 60 até 69
                {
                    pacote_vlr = ValoresPacotes.kids60;
                    pacote_vlr += ValoresPacotes.convidado_kids * (convidados_qntd % 60);
                }
                else if (convidados_qntd >= 70 && convidados_qntd < 80)     // de 70 até 79
                {
                    pacote_vlr = ValoresPacotes.kids70;
                    pacote_vlr += ValoresPacotes.convidado_kids * (convidados_qntd % 70);
                }
                else if (convidados_qntd >= 80 && convidados_qntd < 90)     // de 80 até 89
                {
                    pacote_vlr = ValoresPacotes.kids80;
                    pacote_vlr += ValoresPacotes.convidado_kids * (convidados_qntd % 80);
                }
                else if (convidados_qntd >= 90 && convidados_qntd < 100)    // de 90 até 99
                {
                    pacote_vlr = ValoresPacotes.kids90;
                    pacote_vlr += ValoresPacotes.convidado_kids * (convidados_qntd % 90);
                }
                else if (convidados_qntd >= 100)                            // de 100 em diante
                {
                    pacote_vlr = ValoresPacotes.kids100;
                    pacote_vlr += ValoresPacotes.convidado_kids * (convidados_qntd % 100);
                }
            }

            else if (pacote_str == "Sonho")
            {
                if (convidados_qntd < 60)   //  de 50 até 59
                {
                    pacote_vlr = ValoresPacotes.sonho50;
                    pacote_vlr += ValoresPacotes.convidado_kids * (convidados_qntd % 50);
                }
                else if (convidados_qntd >= 60 && convidados_qntd < 70)     // de 60 até 69
                {
                    pacote_vlr = ValoresPacotes.sonho60;
                    pacote_vlr += ValoresPacotes.convidado_sonho * (convidados_qntd % 60);
                }
                else if (convidados_qntd >= 70 && convidados_qntd < 80)     // de 70 até 79
                {
                    pacote_vlr = ValoresPacotes.sonho70;
                    pacote_vlr += ValoresPacotes.convidado_sonho * (convidados_qntd % 70);
                }
                else if (convidados_qntd >= 80 && convidados_qntd < 90)     // de 80 até 89
                {
                    pacote_vlr = ValoresPacotes.sonho80;
                    pacote_vlr += ValoresPacotes.convidado_sonho * (convidados_qntd % 80);
                }
                else if (convidados_qntd >= 90 && convidados_qntd < 100)    // de 90 até 99
                {
                    pacote_vlr = ValoresPacotes.sonho90;
                    pacote_vlr += ValoresPacotes.convidado_sonho * (convidados_qntd % 90);
                }
                else if (convidados_qntd >= 100)                            // de 100 em diante
                {
                    pacote_vlr = ValoresPacotes.sonho100;
                    pacote_vlr += ValoresPacotes.convidado_sonho * (convidados_qntd % 100);
                }
            }

            else if (pacote_str == "Encanto")
            {
                if (convidados_qntd < 60)   //  de 50 até 59
                {
                    pacote_vlr = ValoresPacotes.encanto50;
                    pacote_vlr += ValoresPacotes.convidado_encanto * (convidados_qntd % 50);
                }
                else if (convidados_qntd >= 60 && convidados_qntd < 70)     // de 60 até 69
                {
                    pacote_vlr = ValoresPacotes.encanto60;
                    pacote_vlr += ValoresPacotes.convidado_encanto * (convidados_qntd % 60);
                }
                else if (convidados_qntd >= 70 && convidados_qntd < 80)     // de 70 até 79
                {
                    pacote_vlr = ValoresPacotes.encanto70;
                    pacote_vlr += ValoresPacotes.convidado_encanto * (convidados_qntd % 70);
                }
                else if (convidados_qntd >= 80 && convidados_qntd < 90)     // de 80 até 89
                {
                    pacote_vlr = ValoresPacotes.encanto80;
                    pacote_vlr += ValoresPacotes.convidado_encanto * (convidados_qntd % 80);
                }
                else if (convidados_qntd >= 90 && convidados_qntd < 100)    // de 90 até 99
                {
                    pacote_vlr = ValoresPacotes.encanto90;
                    pacote_vlr += ValoresPacotes.convidado_encanto * (convidados_qntd % 90);
                }
                else if (convidados_qntd >= 100)                            // de 100 em diante
                {
                    pacote_vlr = ValoresPacotes.encanto100;
                    pacote_vlr += ValoresPacotes.convidado_encanto * (convidados_qntd % 100);
                }
            }

            else if (pacote_str == "Reino")
            {
                if (convidados_qntd < 60)   //  de 50 até 59
                {
                    pacote_vlr = ValoresPacotes.reino50;
                    pacote_vlr += ValoresPacotes.convidado_reino * (convidados_qntd % 50);
                }
                else if (convidados_qntd >= 60 && convidados_qntd < 70)     // de 60 até 69
                {
                    pacote_vlr = ValoresPacotes.reino60;
                    pacote_vlr += ValoresPacotes.convidado_reino * (convidados_qntd % 60);
                }
                else if (convidados_qntd >= 70 && convidados_qntd < 80)     // de 70 até 79
                {
                    pacote_vlr = ValoresPacotes.reino70;
                    pacote_vlr += ValoresPacotes.convidado_reino * (convidados_qntd % 70);
                }
                else if (convidados_qntd >= 80 && convidados_qntd < 90)     // de 80 até 89
                {
                    pacote_vlr = ValoresPacotes.reino80;
                    pacote_vlr += ValoresPacotes.convidado_reino * (convidados_qntd % 80);
                }
                else if (convidados_qntd >= 90 && convidados_qntd < 100)    // de 90 até 99
                {
                    pacote_vlr = ValoresPacotes.reino90;
                    pacote_vlr += ValoresPacotes.convidado_reino * (convidados_qntd % 90);
                }
                else if (convidados_qntd >= 100)                            // de 100 em diante
                {
                    pacote_vlr = ValoresPacotes.reino100;
                    pacote_vlr += ValoresPacotes.convidado_reino * (convidados_qntd % 100);
                }
            }



            if (pagamento_str == "Crédito C/J")
            {
                float taxa = (pacote_vlr + extras_vlr - entrada_vlr) * 0.05f;
                valor_total = pacote_vlr + extras_vlr + taxa;
            }
            else
            {
                valor_total = pacote_vlr + extras_vlr;
            }


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

        private int VerificarDados()
        {
            string msg_erro = FuncoesDeVerificacao.VerificarCamposDeDadosVazios(
                xaml_cliente.Text,
                xaml_aniversariante.Text,
                xaml_email.Text,
                xaml_endereco.Text,
                xaml_tipo_festa.Text,
                xaml_tdf_outro.Text,
                xaml_tema.Text,
                xaml_data.Text,
                xaml_horario.Text,
                xaml_criancas.Text,
                xaml_extras_descricao.Text,
                xaml_extras_valor.Text
                );

            if (msg_erro != null)
            {
                MessageBox.Show(msg_erro);
                return 1;
            }

            return 0;
        }
    }

    public static class ValoresPacotes
    {
        // valores do pacote kids
        public static float kids50 = 3598;
        public static float kids60 = 3790;
        public static float kids70 = 4125;
        public static float kids80 = 4350;
        public static float kids90 = 4575;
        public static float kids100 = 4930;
        public static float convidado_kids = 40;

        // valores do pacote sonho
        public static float sonho50 = 4589;
        public static float sonho60 = 4979;
        public static float sonho70 = 5369;
        public static float sonho80 = 5759;
        public static float sonho90 = 6149;
        public static float sonho100 = 6539;
        public static float convidado_sonho = 47;

        // valores do pacote encanto
        public static float encanto50 = 4971;
        public static float encanto60 = 5488;
        public static float encanto70 = 6115;
        public static float encanto80 = 6632;
        public static float encanto90 = 7149;
        public static float encanto100 = 7798;
        public static float convidado_encanto = 57;

        // valores do pacote reino
        public static float reino50 = 6459;
        public static float reino60 = 7270;
        public static float reino70 = 8194;
        public static float reino80 = 9008;
        public static float reino90 = 9822;
        public static float reino100 = 10768;
        public static float convidado_reino = 77;
    }

    public static class ExpressoesRegulares
    {
        public static Regex regex_nomes = new Regex(@"^[\p{L}]+\s[\p{L}]+(\s?[\p{L}]*)*$");
        public static Regex regex_nome = new Regex(@"^[\p{L}]+(\s?[\p{L}]*)*$");
        public static Regex regex_email = new Regex(@"^[\w\.-]+@[\w\.-]+\.\w{2,}$");
        public static Regex regex_endereco = new Regex(@"^$");
        public static Regex regex_descricao = new Regex(@"^$");
        public static Regex regex_data = new Regex(@"^$");
        public static Regex regex_hora = new Regex(@"^$");
        public static Regex regex_valores = new Regex(@"^$");
    }

    public class FuncoesDeVerificacao
    {
        public static string VerificarCamposVazios(string pacotes, string convidados, string convidados_np, string extras, string pagamento, string entrada, string parcelas, string desconto)
        {
            if (pacotes == "") 
                return "insira o pacote";
            else if (convidados == "")
                return "insira a quantidade de convidados";
            else if (convidados_np == "")
                return "insira a quantidade de convidados não pagantes";
            else if (extras == "")
                return "insira o valor dos extras";
            else if (pagamento == "")
                return "insira a forma de pagamento";
            else if (entrada == "")
                return "insira o valor da entrada";
            else if (parcelas == "")
                return "insira o valor das parcelas";
            else if (desconto == "")
                return "insira o valor do desconto";
            else return null;
        }

        public static string VerificarValoresDiferentes(string convidados, string convidados_np, string extras, string entrada, string parcelas, string desconto)
        {
            if (int.TryParse(convidados, out int convidados_vlr))
            {
                if (convidados_vlr < 50)
                    return "Não há pacotes para menos de 50 convidados";

                if (int.TryParse(convidados_np, out int convidados_np_vlr))
                {
                    if (convidados_np_vlr < 0)
                        return "Quantidade de convidados não pagantes inválida";

                    if ((convidados_vlr - convidados_np_vlr) < 50)
                        return "Não há pacotes para menos de 50 convidados";
                }
            }
            else return "Quantidade de convidados inválida";

            if (int.TryParse(extras, out int extras_vlr))
            {
                if (extras_vlr < 0)
                    return "Valor de extras inválido";
            }
            else return "Valor de extras inválido";

            if (int.TryParse(entrada, out int entrada_vlr))
            {
                if (entrada_vlr < 0)
                    return "Valor da entrada inválido";
            }
            else return "Valor da entrada inválido";

            if (int.TryParse(parcelas, out int parcelas_vlr))
            {
                if (parcelas_vlr < 0)
                    return "Valor das parcelas inválido";
            }
            else return "Valor das parcelas inválido";

            int desconto_vlr;
            if (desconto.Contains("%")){
                desconto = desconto.Replace("%", "").Trim();

                if (int.TryParse(desconto, out desconto_vlr))
                {
                    if (desconto_vlr < 0 || desconto_vlr > 100)
                        return "Valor do desconto inválido";
                }
                else return "Valor do desconto inválido";
            }
            else if (!int.TryParse(desconto, out desconto_vlr))
                return "Valor do desconto inválido";


            return null;
        }

        public static string VerificarCamposDeDadosVazios(string cliente, string aniversariante, string email, string endereço, string tipoFesta, string tipoFestaOutro, string tema, string data, string horario, string criancas, string extrasDescricao, string extrasValor)
        {
            Match match = ExpressoesRegulares.regex_nomes.Match(cliente);
            if (!match.Success)
                return "Nome do cliente ausente ou inválido";

            match = ExpressoesRegulares.regex_nome.Match(aniversariante);
            if (!match.Success)
                return "Nome do aniversariante ausente ou inválido";

            match = ExpressoesRegulares.regex_email.Match(email);
            if (!match.Success)
                return "E-mail inválido";

            return null;
        }
    }
}