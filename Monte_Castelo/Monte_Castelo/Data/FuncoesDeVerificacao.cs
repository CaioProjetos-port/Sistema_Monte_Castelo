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
        public static string VerificarCamposDeDados(PageAgenda pagina)
        {
            string[] campos = {
                pagina.xaml_cpf_cliente.Text,
                pagina.xaml_cpf_aniversariante.Text,
                pagina.xaml_nome_aniversariante.Text,
                pagina.xaml_sobrenome_aniversariante.Text,
                pagina.xaml_data_nascimento.Text,
                pagina.xaml_tema.Text,
                pagina.xaml_data.Text,
                pagina.xaml_horario.Text,
                pagina.xaml_criancas.Text
            };

            Regex[] regex = {
                RegexER.regex_cpf,
                RegexER.regex_cpf,
                RegexER.regex_nome,
                RegexER.regex_nomes,
                RegexER.regex_data,
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
                string msg = ValidarRegex(campos[i], regex[i], i, "dados_festa");
                if (msg != null)
                    return msg;
            }

            if (tipoFesta == "Outro" && tipoFestaOutro == "")
                return "insira o tipo de festa";
            else if (tipoFesta == "Outro" && tipoFestaOutro != "")
            {
                string msg = ValidarRegex(tipoFestaOutro, regex[3], 0, "tipo_de_festa");
                if (msg != null)
                    return msg;
            }

            if (extrasDescricao == "" && extrasValor != "0")
                return "insira a descrição dos extras";
            else if (extrasDescricao != "" && extrasValor == "0")
                return "insira o valor dos extras";

            return null;
        }

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
                string msg = ValidarRegex(campos[i], regex[i], i, "valores_festa");
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

        public static string VerificarCamposDoCliente(PageCliente pagina)
        {
            string[] campos = {
                pagina.xaml_cpf_cliente.Text,
                pagina.xaml_nome_cliente.Text,
                pagina.xaml_sobrenome_cliente.Text,
                pagina.xaml_celular_cliente.Text,
                pagina.xaml_email_cliente.Text,
                pagina.xaml_cep.Text,
                pagina.xaml_cidade.Text,
                pagina.xaml_bairro.Text,
                pagina.xaml_logradouro.Text,
                pagina.xaml_numero.Text,
            };

            Regex[] regex = {
                RegexER.regex_cpf,
                RegexER.regex_nome,
                RegexER.regex_nomes,
                RegexER.regex_telefone,
                RegexER.regex_email,
                RegexER.regex_cep,
                RegexER.regex_descricao,
                RegexER.regex_descricao,
                RegexER.regex_descricao,
                RegexER.regex_descricao
            };

            for (int i = 0; i < campos.Length; i++)
            {
                string msg = ValidarRegex(campos[i], regex[i], i, "dados_cliente");
                if (msg != null)
                    return msg;
            }

            return null;
        }

        public static string formatarNumCelular(string celular, Key k)
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

        public static string formatarCpf(string cpf, Key k)
        {
            if (k == Key.Back)
                return cpf;
            
            if (cpf.Length > 3 && cpf[3] != '.')
                cpf = cpf.Insert(3, ".");

            if (cpf.Length > 7 && cpf[7] != '.')
                cpf = cpf.Insert(7, ".");

            if (cpf.Length > 11 && cpf[11] != '-')
                cpf = cpf.Insert(11, "-");

            if (cpf.Length > 14)
                return cpf.Substring(0, 14);

            return cpf;
        }

        public static string formatarCep(string cep, Key k)
        {
            if (k == Key.Back)
                return cep;

            if (cep.Length > 5 && cep[5] != '-')
                cep = cep.Insert(5, "-");

            if (cep.Length > 9)
                return cep.Substring(0, 9);

            return cep;
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

        private static string ValidarRegex(string campo, Regex regex, int index, string verificar)
        {
            string[] erros;
            if (verificar == "dados_festa")
                erros = new string[] {
                "CPF do cliente inválido",
                    "CPF do aniversariante inválido",
                    "Nome do aniversariante inválido",
                    "Sobrenome do aniversariante inválido",
                    "Idade inválida",
                    "Tema inválido",
                    "Data inválida",
                    "Hora inválida",
                    "Quantidade de crianças inválida", 
                };

            else if (verificar == "valores_festa")
                erros = new string[] {
                    "Quantidade de convidados inválida",
                    "Quantidade de convidados não pagantes inválida",
                    "Valor dos extras inválida",
                    "Valor da entrada inválida",
                    "Quantidade das parcelas inválida",
                    "Valor do desconto inválido"
                };


            else if (verificar == "dados_cliente")
                erros = new string[] {
                    "CPF inválido",
                    "Nome inválido",
                    "Sobrenome inválido",
                    "Celular inválido",
                    "Email inválido",
                    "CEP inválido",
                    "Cidade inválida",
                    "Bairro inválido",
                    "Logrado inválido",
                    "Número inválido",
                    "Complemento inválido"
                };

            else if (verificar == "tipo_de_festa")
                erros = new string[] {
                    "Preencha o tipo de festa"
                };

            else
                return "erro desconhecido";

                Match match = regex.Match(campo);
            if (!match.Success)
                return erros[index];

            return null;
        }
    }
}
