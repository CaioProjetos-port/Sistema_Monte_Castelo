using System;
using System.Collections.Generic;
using Npgsql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monte_Castelo.Config
{
    internal class ConexaoBD
    {
        private static string servidor = "localhost";
        private static string porta = "5432";
        private static string usuario = "postgres";
        private static string senha = "Caio2423!";
        private static string nomeBanco = "monte_castelo";

        public static string StringConexao
        {
            get
            {
                return $"Host={servidor};Port={porta};Database={nomeBanco};Username={usuario};Password={senha}";
            }
        }
    }
}
