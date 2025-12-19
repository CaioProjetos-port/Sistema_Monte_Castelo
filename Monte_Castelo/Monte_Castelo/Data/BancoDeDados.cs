using Monte_Castelo.Config;
using System;
using Npgsql;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monte_Castelo.Data
{
    internal class BancoDeDados
    {
        public static void SalvarFesta(NpgsqlConnection conn, PageAgenda pagina)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT agendar_festa_completa(@cpf_cliente, " +
                    "@cpf_aniversariante, @nome, @sobrenome, @nascimento, " +
                    "@forma_pagamento, @entrada, @parcelas, @desconto, @valor_total, @valor_final" +
                    "@tipo, @tema, @pacote, @data, @hora, @convidados, @convidados_np, @criancas, @extras_desc, @extras_vlr)";

                cmd.Parameters.AddWithValue("@cliente", pagina.xaml_cpf_cliente.Text);
                cmd.Parameters.AddWithValue("@cpf_aniversariante", pagina.xaml_cpf_aniversariante.Text);
                cmd.Parameters.AddWithValue("@nome", pagina.xaml_nome_aniversariante.Text);
                cmd.Parameters.AddWithValue("@sobrenome", pagina.xaml_sobrenome_aniversariante);
                cmd.Parameters.AddWithValue("@nascimento", pagina.xaml_data_nascimento);
                cmd.Parameters.AddWithValue("@forma_pagamento", pagina.xaml_forma_pagamento);
                cmd.Parameters.AddWithValue("@entrada", pagina.xaml_valor_entrada);
                cmd.Parameters.AddWithValue("@parcelas", pagina.xaml_num_parcelas);
                cmd.Parameters.AddWithValue("@desconto", pagina.xaml_valor_desconto);
                cmd.Parameters.AddWithValue("@valor_total", pagina.xaml_valor_total);
                cmd.Parameters.AddWithValue("@valor_final", pagina.xaml_valor_final);
                cmd.Parameters.AddWithValue("@tipo", pagina.xaml_tipo_festa.Text);
                cmd.Parameters.AddWithValue("@tema", pagina.xaml_tema.Text);
                cmd.Parameters.AddWithValue("@pacote", pagina.xaml_pacote.Text);
                cmd.Parameters.AddWithValue("@data", pagina.xaml_data.Text);
                cmd.Parameters.AddWithValue("@hora", pagina.xaml_horario.Text);
                cmd.Parameters.AddWithValue("@convidados", pagina.xaml_qntd_convidados.Text);
                cmd.Parameters.AddWithValue("@convidados_np", pagina.xaml_qntd_convidados_np.Text);
                cmd.Parameters.AddWithValue("@criancas", pagina.xaml_criancas.Text);
                cmd.Parameters.AddWithValue("@extras_desc", pagina.xaml_extras_descricao.Text);
                cmd.Parameters.AddWithValue("@extras_vlr", pagina.xaml_extras_valor.Text);
                cmd.ExecuteNonQuery();
            }
        }

        public static void SalvarCliente(NpgsqlConnection conn, PageCliente pagina)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT cadastrar_cliente_completo(@cpf_cliente, @nome_cliente, @sobrenome_cliente, @celular_cliente, @email_cliente, @cep, @logradouro, @numero, @complemento, @bairro, @cidade)";

                cmd.Parameters.AddWithValue("@cpf_cliente", pagina.xaml_cpf_cliente.Text);
                cmd.Parameters.AddWithValue("@nome_cliente", pagina.xaml_nome_cliente.Text);
                cmd.Parameters.AddWithValue("@sobrenome_cliente", pagina.xaml_sobrenome_cliente.Text);
                cmd.Parameters.AddWithValue("@celular_cliente", pagina.xaml_celular_cliente.Text);
                cmd.Parameters.AddWithValue("@email_cliente", pagina.xaml_email_cliente.Text);
                cmd.Parameters.AddWithValue("@cep", pagina.xaml_cep.Text);
                cmd.Parameters.AddWithValue("@logradouro", pagina.xaml_logradouro.Text);
                cmd.Parameters.AddWithValue("@numero", pagina.xaml_numero.Text);
                cmd.Parameters.AddWithValue("@complemento", pagina.xaml_complemento.Text);
                cmd.Parameters.AddWithValue("@bairro", pagina.xaml_bairro.Text);
                cmd.Parameters.AddWithValue("@cidade", pagina.xaml_cidade.Text);
                cmd.ExecuteNonQuery();
            }
        }

        public static ObservableCollection<Festa> RetornarInformacoesDaListaDeFestas(ObservableCollection<Festa> Festas)
        {
            string conexao = ConexaoBD.StringConexao;

            using (var conn = new NpgsqlConnection(conexao))
            {
                conn.Open();

                using (NpgsqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM view_lista_festas ORDER BY data_festa DESC";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Festas.Add(new Festa { 
                                Cliente = reader["nome_cliente"].ToString(),
                                Aniversariante = reader["nome_aniversariante"].ToString(),
                                Data = reader["data_festa"].ToString(),
                                Hora = reader["hora_festa"].ToString(),
                                Tema = reader["tema"].ToString(),
                                Convidados = reader["convidados"].ToString(),
                                Pacote = reader["pacote"].ToString(),
                                ValorFinal = reader["valor_final"].ToString()
                            });
                        }
                    }
                }
            }

            return Festas;
        }

        public static ObservableCollection<Cliente> RetornarListaDeClientes(ObservableCollection<Cliente> Clientes)
        {
            Clientes.Clear();
            string conexao = ConexaoBD.StringConexao;

            using (var conn = new NpgsqlConnection(conexao))
            {
                conn.Open();

                using (NpgsqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM view_lista_clientes ORDER BY nome_cliente ASC";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Clientes.Add(new Cliente {
                                id = Convert.ToInt32(reader["id_cliente"]),
                                nome = reader["nome_cliente"].ToString() + " " + reader["sobrenome_cliente"].ToString(),
                                celular = reader["celular_cliente"].ToString(),
                                email = reader["email_cliente"].ToString(),
                                cpf = reader["cpf_cliente"].ToString()
                            });
                        }
                    }
                }
            }
            return Clientes;
        }
    }
}


