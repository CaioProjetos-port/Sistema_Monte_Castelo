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
        public static void CriarTabelasAgendamento(SQLiteConnection conn)   // REFAZER TABELAS E SEUS DADOS (, usar constraings e tiggers, usar BD PostgreSQL, criar mais telas no Agendamento de festa para separar as informações)
        {
            using (var cmd = conn.CreateCommand())
            {
                // Tabela do Cliente
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Cliente (
                                    id_cliente INTEGER PRIMARY KEY AUTOINCREMENT,
                                    cpf INTEGER NOT NULL,
                                    nome TEXT NOT NULL,
                                    sobreNome TEXT NOT NULL,
                                    celular INTEGER NOT NULL,
                                    email TEXT NOT NULL,
                                    cep INTEGER NOT NULL
                                    numCasa INTEGER
                                    complemento TEXT
                                    )";
                cmd.ExecuteNonQuery();

                // Tabela de Aniversariante
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Aniversariante (
                                  id_aniversariante INTEGER PRIMARY KEY AUTOINCREMENT,
                                  Nome TEXT NOT NULL,
                                  SobreNome TEXT NOT NULL,
                                  Nascimento TEXT NOT NULL,
                                  )";
                cmd.ExecuteNonQuery();

                // Tabela do Pagamento Previsto
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS PagamentoPrev (
                                    id_pagamento INTEGER PRIMARY KEY AUTOINCREMENT,
                                    formaPagamento TEXT NOT NULL,
                                    entrada REAL NOT NULL,
                                    parcelas INTEGER NOT NULL,
                                    desconto REAL NOT NULL,
                                    valorTotal REAL NOT NULL,
                                    valorFinal REAL NOT NULL
                                    )";
                cmd.ExecuteNonQuery();

                // Tabela da Festa
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Festa (
                                    id_festa INTEGER PRIMARY KEY AUTOINCREMENT,
                                    aniversariante TEXT NOT NULL,
                                    idade INTEGER NOT NULL,
                                    tipo TEXT NOT NULL,
                                    tema TEXT NOT NULL,
                                    pacote TEXT NOT NULL,
                                    data TEXT NOT NULL,
                                    hora TEXT NOT NULL,
                                    convidados INTEGER NOT NULL,
                                    convidadosNP INTEGER NOT NULL,
                                    criancas INTEGER NOT NULL,
                                    extrasDesc TEXT,
                                    extrasVlr REAL,
                                    cliente INTEGER NOT NULL,
                                    pagamentoPrev INTEGER NOT NULL,
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

        public static void SalvarCliente(SQLiteConnection conn, PageCliente pagina)
        {
            using (var cmd = conn.CreateCommand())
            {
                // Salvar Cliente
                cmd.CommandText = @"INSERT INTO Cliente (cpf, nome, sobreNome, celular, email, cep, numCasa, complemento)
                                    VALUES (@cpf, @nome, @sobreNome, @celular, @email, @cep, @numCasa, @complemento)";
                cmd.Parameters.AddWithValue("@cpf", pagina.xaml_cpf.Text);
                cmd.Parameters.AddWithValue("@nome", pagina.xaml_nome.Text);
                cmd.Parameters.AddWithValue("@sobreNome", pagina.xaml_sobreNome.Text);
                cmd.Parameters.AddWithValue("@celular", pagina.xaml_celular.Text);
                cmd.Parameters.AddWithValue("@email", pagina.xaml_email.Text);
                cmd.Parameters.AddWithValue("@cep", pagina.xaml_cep.Text);
                cmd.Parameters.AddWithValue("@numCasa", pagina.xaml_numCasa.Text);
                cmd.Parameters.AddWithValue("@complemento", pagina.xaml_complemento.Text);
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


