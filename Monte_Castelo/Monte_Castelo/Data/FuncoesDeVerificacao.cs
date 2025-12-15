using Monte_Castelo.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Monte_Castelo.Data
{
    internal class FuncoesDeVerificacao
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
                pagina.xaml_cpf.Text,
                pagina.xaml_aniversariante.Text,
                pagina.xaml_idade.Text,
                pagina.xaml_tema.Text,
                pagina.xaml_data.Text,
                pagina.xaml_horario.Text,
                pagina.xaml_criancas.Text
            };

            Regex[] regex = {
                RegexER.regex_cpf,
                RegexER.regex_nomes,
                RegexER.regex_numeros,
                RegexER.regex_descricao,
                RegexER.regex_data,
                RegexER.regex_hora,
                RegexER.regex_numeros
            };

            string tipoFesta = pagina.xaml_tipo_festa.Text;
            string tipoFestaOutro = pagina.xaml_tdf_outro.Text;
            string extrasDescricao = pagina.xaml_extras_descricao.Text;
            string extrasValor = pagina.xaml_extras_valor.Text;

            for (int i = 0; i < campos.Length; i++)
            {
                string msg = ValidarRegex(campos[i], regex[i], i);
                if (msg != null)
                    return msg;
            }

            if (tipoFesta == "Outro" && tipoFestaOutro == "")
                return "insira o tipo de festa";
            else if (tipoFesta == "Outro" && tipoFestaOutro != "")
            {
                string msg = ValidarRegex(tipoFestaOutro, regex[3], 8);
                if (msg != null)
                    return msg;
            }

            if (extrasDescricao == "" && extrasValor != "0")
                return "insira a descrição dos extras";
            else if (extrasDescricao != "" && extrasValor == "0")
                return "insira o valor dos extras";

            return null;
        }

        public static string FormatarNumCelular(string celular, Key k)
        {
            if (celular.Length == 2 && k != Key.Back)
                celular = "(" + celular.Substring(0, 2) + ")";

            else if (celular.Length == 4 && k != Key.Back)
            {
                celular = celular.Replace("(", "").Replace(")", "");
                celular = "(" + celular.Substring(0, 2) + ")";
            }

            else if (celular.Length == 12)
            {
                celular = celular.Replace(".", "").Replace("-", "");
                celular = celular.Insert(8, "-");
            }

            else if (celular.Length == 14 && k != Key.Back)
            {
                celular = celular.Replace(".", "").Replace("-", "");
                celular = celular.Insert(5, ".").Insert(10, "-");
            }

            else if (celular.Length > 15)
                return celular.Substring(0, 14);

            return celular;
            // (88)9.8155-5424
        }

        public static string FormatarCpf(string cpf, Key k)
        {
            if (cpf.Length == 3 && k != Key.Back)
                cpf = cpf + ".";

            else if (cpf.Length == 7 && k != Key.Back)
                cpf = cpf + ".";

            else if (cpf.Length == 11 && k != Key.Back)
                cpf = cpf + "-";

            if (cpf[3] != '.')
                cpf = cpf.Insert(3, ".");

            if (cpf[7] != '.')
                cpf = cpf.Insert(7, ".");

            if (cpf[11] != '-')
                cpf = cpf.Insert(11, "-");

            return cpf;
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
                "CPF do cliente inválido", // indice 0, primeiro do loop campo de dados
                "Nome do aniversariante inválido",
                "Idade inválida",
                "Tema inválido",
                "Data inválida",
                "Hora inválida",
                "Quantidade de crianças inválida", 
                "", // indice 7, ultimo do loop campo de dados
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
