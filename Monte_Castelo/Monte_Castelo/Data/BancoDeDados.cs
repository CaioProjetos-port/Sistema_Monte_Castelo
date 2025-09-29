using Monte_Castelo.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monte_Castelo.Data
{
    internal class BancoDeDados
    {
        public static void CriarTabelasAgendamento(SQLiteConnection conn)
        {
            using (var cmd = conn.CreateCommand())
            {
                // Tabela do Cliente
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Cliente (
                                    CPF INTEGER PRIMARY KEY,
                                    Nome TEXT NOT NULL,
                                    Telefone TEXT NOT NULL,
                                    Email TEXT NOT NULL,
                                    Endereco TEXT NOT NULL
                                    )";
                cmd.ExecuteNonQuery();

                // Tabela do Pagamento Previsto
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS PagamentoPrev (
                                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                    FormaPagamento TEXT NOT NULL,
                                    Entrada REAL NOT NULL,
                                    Parcelas INTEGER NOT NULL,
                                    Desconto REAL NOT NULL,
                                    ValorTotal REAL NOT NULL,
                                    ValorFinal REAL NOT NULL
                                    )";
                cmd.ExecuteNonQuery();

                // Tabela da Festa
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Festa (
                                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Aniversariante TEXT NOT NULL,
                                    Idade INTEGER NOT NULL,
                                    Tipo TEXT NOT NULL,
                                    Tema TEXT NOT NULL,
                                    Pacote TEXT NOT NULL,
                                    Data TEXT NOT NULL,
                                    Hora TEXT NOT NULL,
                                    Convidados INTEGER NOT NULL,
                                    ConvidadosNP INTEGER NOT NULL,
                                    Criancas INTEGER NOT NULL,
                                    ExtrasDesc TEXT,
                                    ExtrasVlr REAL,
                                    Cliente INTEGER NOT NULL,
                                    PagamentoPrev INTEGER NOT NULL,
                                    FOREIGN KEY (Cliente) REFERENCES Cliente(CPF),
                                    FOREIGN KEY (PagamentoPrev) REFERENCES PagamentoPrev(ID)
                                    )";
                cmd.ExecuteNonQuery();
            }
        }

        public static bool VerificarSeClienteExiste(SQLiteConnection conn, string cpf)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM Cliente WHERE CPF = @cpf";
                cmd.Parameters.AddWithValue("@cpf", cpf);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return true;
                    else
                        return false;
                }
            }
        }

        public static bool VerificarDisponibilidaDeData(SQLiteConnection conn , string data)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM Festa WHERE Data = @data";
                cmd.Parameters.AddWithValue("@data", data);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return false;
                    else
                        return true;
                }
            }
        }

        public static void SalvarClientePagamentoFesta(SQLiteConnection conn, PageAgenda pagina)
        {
            int PagamentoID;

            using (var cmd = conn.CreateCommand())
            {
                // Salvar Pagamento Previsto
                cmd.CommandText = @"INSERT INTO PagamentoPrev (FormaPagamento, Entrada, Parcelas, Desconto, ValorTotal, ValorFinal)
                                    VALUES (@formaPagamento, @entrada, @parcelas, @desconto, @valorTotal, @valorFinal)";
                cmd.Parameters.AddWithValue("formaPagamento", pagina.xaml_forma_pagamento.Text);
                cmd.Parameters.AddWithValue("entrada", pagina.xaml_valor_entrada.Text);
                cmd.Parameters.AddWithValue("parcelas", pagina.xaml_num_parcelas.Text);
                cmd.Parameters.AddWithValue("desconto", pagina.xaml_valor_desconto.Text);
                cmd.Parameters.AddWithValue("valorTotal", pagina.xaml_valor_total.Text);
                cmd.Parameters.AddWithValue("valorFinal", pagina.xaml_valor_final.Text);
                cmd.ExecuteNonQuery();

                // Coletar o ID do Pagamento Previsto
                cmd.CommandText = @"SELECT last_insert_rowid()";
                PagamentoID = Convert.ToInt32(cmd.ExecuteScalar());

                // Salvar Festa
                cmd.CommandText = @"INSERT INTO Festa (Aniversariante, Idade, Tipo, Tema, Pacote, Data, Hora, Convidados, ConvidadosNP, Criancas, ExtrasDesc, ExtrasVlr, Cliente, PagamentoPrev)
                                    VALUES (@aniversariante, @idade, @tipo, @tema, @pacote, @data, @hora, @convidados, @convidadosNP, @criancas, @extrasDesc, @extrasVlr, @cliente, @pagamentoPrev)";
                cmd.Parameters.AddWithValue("@aniversariante", pagina.xaml_aniversariante.Text);
                cmd.Parameters.AddWithValue("@idade", pagina.xaml_idade.Text);
                cmd.Parameters.AddWithValue("@tipo", pagina.xaml_tipo_festa.Text);
                cmd.Parameters.AddWithValue("@tema", pagina.xaml_tema.Text);
                cmd.Parameters.AddWithValue("@pacote", pagina.xaml_pacote.Text);
                cmd.Parameters.AddWithValue("@data", pagina.xaml_data.Text);
                cmd.Parameters.AddWithValue("@hora", pagina.xaml_horario.Text);
                cmd.Parameters.AddWithValue("@convidados", pagina.xaml_qntd_convidados.Text);
                cmd.Parameters.AddWithValue("@convidadosNP", pagina.xaml_qntd_convidados_np.Text);
                cmd.Parameters.AddWithValue("@criancas", pagina.xaml_criancas.Text);
                cmd.Parameters.AddWithValue("@extrasDesc", pagina.xaml_extras_descricao.Text);
                cmd.Parameters.AddWithValue("@extrasVlr", pagina.xaml_extras_valor.Text);
                cmd.Parameters.AddWithValue("@cliente", pagina.xaml_cpf.Text);
                cmd.Parameters.AddWithValue("@pagamentoPrev", PagamentoID);
                cmd.ExecuteNonQuery();
            }
        }

        public static ObservableCollection<Festa> RetornarInformacoesDaListaDeFestas(ObservableCollection<Festa> Festas)
        {
            string conexao = Acesso.conection;

            using (var conn = new SQLiteConnection(conexao))
            {
                conn.Open();

                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * FROM Festa";

                    using(var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Festas.Add(new Festa { 
                                Cliente = reader["cpf"].ToString(),
                                Aniversariante = reader["aniversariante"].ToString(),
                                Data = reader["data"].ToString(),
                                Hora = reader["hora"].ToString(),
                                Tema = reader["tema"].ToString(),
                                Convidados = reader["convidados"].ToString(),
                                Pacote = reader["pacote"].ToString(),
                            });
                        }
                    }
                }
            }

            return Festas;
        }
    }
}
