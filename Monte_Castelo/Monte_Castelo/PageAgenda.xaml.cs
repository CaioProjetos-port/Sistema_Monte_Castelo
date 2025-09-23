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

            // SalvarEvento();
        }

        // função que verifica se os campos numericos são validos
        private int VerificarCalculo()
        {
            // verifica se os campos estão vazios
            // se forem vazios, retorna true e fecha a função button_Calcular
            string msg_erro = FuncoesDeVerificacao.VerificarCamposNumericos(
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
                float taxa = (pacote_vlr + extras_vlr - entrada_vlr) * ValoresPacotes.taxa_cartao;
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
            string msg_erro = FuncoesDeVerificacao.VerificarCamposDeDados(
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

        // valor da taxa do cartão de crédito
        public static float taxa_cartao = 0.05f;
    }

    public static class ExpressoesRegulares
    {
        public static Regex regex_nomes = new Regex(@"^[\p{L}]+\s[\p{L}]+(\s?[\p{L}]*)*$");
        public static Regex regex_nome = new Regex(@"^[\p{L}]+(\s?[\p{L}]*)*$");
        public static Regex regex_email = new Regex(@"^[\w\.-]+@[\w\.-]+\.\w{2,}$");
        public static Regex regex_endereco = new Regex(@"^[\p{L}\d\s,.\-ºª]+$");
        public static Regex regex_descricao = new Regex(@"^[\p{L}\d\s,.\-!?\(\)]{1,200}$");
        public static Regex regex_data = new Regex(@"^(0[1-9]|[12]\d|3[01])/(0[1-9]|1[0-2])/(19|20)\d{2}$");
        public static Regex regex_hora = new Regex(@"^([01]\d|2[0-3]):[0-5]\d$");
        public static Regex regex_numeros = new Regex(@"^\d+$");
        public static Regex regex_valores = new Regex(@"^\d+([,]\d+)?$");
        public static Regex regex_valores_percentuais = new Regex(@"^\d+([,]\d{1,2})?%?$");
    }

    public class FuncoesDeVerificacao
    {
        public static string VerificarCamposNumericos(string convidados, string convidados_np, string extras, string entrada, string parcelas, string desconto)
        {
            string[] campos = {
                convidados,
                convidados_np,
                extras,
                entrada,
                parcelas,
                desconto
            };

            Regex[] regex = {
                ExpressoesRegulares.regex_numeros,
                ExpressoesRegulares.regex_numeros,
                ExpressoesRegulares.regex_valores,
                ExpressoesRegulares.regex_valores,
                ExpressoesRegulares.regex_numeros,
                ExpressoesRegulares.regex_valores_percentuais,
            };

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

        public static string VerificarCamposDeDados(string cliente, string aniversariante, string email, string endereço, string tipoFesta, string tipoFestaOutro, string tema, string data, string horario, string criancas, string extrasDescricao, string extrasValor)
        {
            string[] campos = {
                cliente,
                aniversariante,
                email,
                endereço,
                //tipoFesta,
                //tipoFestaOutro,
                tema,
                data,
                horario,
                criancas,
                //extrasDescricao,
                //extrasValor
            };

            Regex[] regex = {
                ExpressoesRegulares.regex_nomes,
                ExpressoesRegulares.regex_nome,
                ExpressoesRegulares.regex_email,
                ExpressoesRegulares.regex_endereco,
                ExpressoesRegulares.regex_descricao,
                ExpressoesRegulares.regex_data,
                ExpressoesRegulares.regex_hora,
                ExpressoesRegulares.regex_numeros
            };

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

            if (extrasDescricao == "" && extrasValor != "")
                return "insira a descrição dos extras";
            else if (extrasDescricao != "" && extrasValor == "")
                return "insira o valor dos extras";

            return null;
        }

        public static float ValorDoPacoteKids(int convidados_qntd)
        {
            float pacote_vlr = 0;

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

            return pacote_vlr;
        }

        public static float ValorDoPacoteSonho(int convidados_qntd)
        {
            float pacote_vlr = 0;

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

            return pacote_vlr;
        }

        public static float ValorDoPacoteEncanto(int convidados_qntd)
        {
            float pacote_vlr = 0;

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

            return pacote_vlr;
        }

        public static float ValorDoPacoteReino(int convidados_qntd)
        {
            float pacote_vlr = 0;

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
                "Quantidade de convidados inválida", // indice 7, ultimo do loop campo de dados
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
}