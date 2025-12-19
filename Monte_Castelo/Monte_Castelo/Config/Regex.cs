using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Monte_Castelo.Config
{
    internal class RegexER
    {
            public static Regex regex_cpf = new Regex(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$");
            public static Regex regex_cep= new Regex(@"^\d{5}-\d{3}$");
            public static Regex regex_nomes = new Regex(@"^[\p{L}]+\s[\p{L}]+(\s?[\p{L}]*)*$");
            public static Regex regex_nome = new Regex(@"^[\p{L}]+(\s?[\p{L}]*)*$");
            public static Regex regex_telefone = new Regex(@"^\(\d{2}\)\d\.\d{4}-\d{4}$");
            public static Regex regex_email = new Regex(@"^[\w\.-]+@[\w\.-]+\.\w{2,}$");
            public static Regex regex_endereco = new Regex(@"^[\p{L}\d\s,.\-ºª]+$");
            public static Regex regex_descricao = new Regex(@"^[\p{L}\d\s,.\-!?\(\)]{1,200}$");
            public static Regex regex_data = new Regex(@"^(0[1-9]|[12]\d|3[01])/(0[1-9]|1[0-2])/(19|20)\d{2}$");
            public static Regex regex_hora = new Regex(@"^([01]\d|2[0-3]):[0-5]\d$");
            public static Regex regex_numeros = new Regex(@"^\d+$");
            public static Regex regex_valores = new Regex(@"^\d+([,]\d+)?$");
            public static Regex regex_valores_percentuais = new Regex(@"^\d+([,]\d{1,2})?%?$");

    }
}
