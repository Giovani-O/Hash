using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;

namespace Hash.Utils
{
    class DB
    {

        private MySqlConnection connection;
        private string host = "localhost";
        private string database = "hash";
        private string username = "root";
        private string password = "";

        /// <summary>
        /// Cria uma conexão com um banco de dados MySQL utilizando as informações padrões.
        /// </summary>
        public DB()
        {
            string connectionString =
                "SERVER=" + host + ";" +
                "DATABASE=" + database + ";" +
                "UID=" + username + ";" +
                "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Cria uma conexão com um banco de dados MySQL utilizando as informações passadas.
        /// </summary>
        /// <param name="host">Endereço do banco de dados</param>
        /// <param name="database">Nome do banco de dados</param>
        /// <param name="username">Nome de usuário para acessar o banco de dados</param>
        /// <param name="password">Senha do usuário para acessar o banco de dados</param>
        public DB(string host, string database, string username, string password)
        {
            string connectionString =
                "SERVER=" + host + ";" +
                "DATABASE=" + database + ";" +
                "UID=" + username + ";" +
                "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            // Inicializa uma conexão para realizar uma query.
            try
            {
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir conexão: " + ex.Message, "Erro");
                return false;
            }
        }
        private bool CloseConnection()
        {
            // Finaliza uma conexão após realizar uma query.
            try
            {
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao fechar conexão: " + ex.Message, "Erro");
                return false;
            }
        }

        /// <summary>
        /// Método para executar uma query de Select na conexão realizada anteriormente ao criar o objeto DB.
        /// 
        /// Pode lançar uma exceção interna e apresentar o erro por meio de um MessageBox.
        /// </summary>
        /// <param name="query">Query no formato SQL para ser executada no SGBD.</param>
        /// <returns>Retorna um DataTable caso exista a informação no Banco de Dados</returns>
        public DataTable Select(string query, List<MySqlParameter> parametros)
        {
            DataTable tbRetorno = null;

            try
            {
                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    if (parametros != null)
                        for (int i = 0; i < parametros.Count; i++)
                            cmd.Parameters.Add(parametros[i]);

                    // Objeto de resposta padrão de uma execução de query
                    MySqlDataReader data = cmd.ExecuteReader();
                    List<string> registros = new List<string>();

                    // Convertendo o DataReader para um DataTable (formato de visualização melhor)
                    DataTable tbEsquema = data.GetSchemaTable();
                    tbRetorno = new DataTable();
                    if (data != null && tbEsquema != null)
                    {
                        // Criando as colunas
                        foreach (DataRow linha in tbEsquema.Rows)
                        {
                            if (!tbRetorno.Columns.Contains(linha["ColumnName"].ToString()))
                            {
                                DataColumn col = new DataColumn()
                                {
                                    ColumnName = linha["ColumnName"].ToString(),
                                    Unique = Convert.ToBoolean(linha["IsUnique"]),
                                    AllowDBNull = Convert.ToBoolean(linha["AllowDBNull"]),
                                    ReadOnly = Convert.ToBoolean(linha["IsReadOnly"])
                                };
                                tbRetorno.Columns.Add(col);
                            }
                        }

                        // Lendo linha a linha os registros da tabela de retorno
                        while (data.Read())
                        {
                            //string registroAtual = "";
                            //for (int i = 0; i < data.FieldCount; i++)
                            //    registroAtual += data.GetValue(i) + " // ";
                            //registros.Add(registroAtual);
                            DataRow novaLinha = tbRetorno.NewRow();
                            // Populando as colunas de resposta com os valores do DB
                            for (int i = 0; i < tbRetorno.Columns.Count; i++)
                            {
                                novaLinha[i] = data.GetValue(i);
                            }
                            tbRetorno.Rows.Add(novaLinha);
                        }

                        data.Close();

                        for (int i = 0; i < registros.Count; i++)
                            Console.WriteLine(registros[i]);
                    }
                    this.CloseConnection();

                    // Tudo ocorreu corretamente e estamos retornando a tabela com as informações.
                    return tbRetorno;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro MySQL: " + ex.Message, "Erro");
            }

            // A query não possuía uma resposta, então não se retorna nada.
            return null;
        }
        public List<string> SelectData(string query, List<MySqlParameter> parametros, int responseSize)
        {
            List<string> datas = new List<string>();
            try
            {
                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    if (parametros != null)
                        for (int i = 0; i < parametros.Count; i++)
                            cmd.Parameters.Add(parametros[i]);

                    // Objeto de resposta padrão de uma execução de query
                    MySqlDataReader data = cmd.ExecuteReader();

                    // Lendo linha a linha os registros da tabela de retorno
                    if (data != null)
                        while (data.Read())
                            for (int i = 0; i < responseSize; i++)
                                datas.Add(data.GetValue(i).ToString());

                    data.Close();
                    this.CloseConnection();

                    // Tudo ocorreu corretamente e estamos retornando a tabela com as informações.
                    return datas;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro MySQL: " + ex.Message, "Erro");
            }

            // A query não possuía uma resposta, então não se retorna nada.
            return null;
        }

        /// <summary>
        /// Executa uma query de Insert no Banco de Dados MySQL com as configurações já
        /// realizadas.
        /// 
        /// Caso a query esteja em um formato incorreto, uma exceção interna é lançada e mostrada ao usuário por meio de um MessageBox.
        /// </summary>
        /// <param name="query">Query no formato SQL para ser executada no SGBD.</param>
        /// <returns>Retorna se a execução da query teve sucesso ou não.</returns>
        public bool Insert(string query, List<MySqlParameter> parametros)
        {
            try
            {
                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    if (parametros != null)
                        for (int i = 0; i < parametros.Count; i++)
                            cmd.Parameters.Add(parametros[i]);
                    int resposta = cmd.ExecuteNonQuery();
                    this.CloseConnection();
                    if (resposta != 0)
                        return true;
                    return false;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro MySQL: " + ex.Message, "Erro");
            }
            return false;
        }

        /// <summary>
        /// Executa uma query de Update no Banco de Dados MySQL com as configurações já
        /// realizadas.
        /// 
        /// Caso a query esteja em um formato incorreto, uma exceção interna é lançada e mostrada ao usuário por meio de um MessageBox.
        /// </summary>
        /// <param name="query">Query no formato SQL para ser executada no SGBD.</param>
        /// <returns>Retorna se a execução da query teve sucesso ou não.</returns>
        public bool Update(string query, List<MySqlParameter> parametros)
        {
            /* A execução das querys de Insert, Update e Delete são iguais.
            Por isso estamos chamando a mesma função já codificada. */
            return Insert(query, parametros);
        }

        /// <summary>
        /// Executa uma query de Delete no Banco de Dados MySQL com as configurações já
        /// realizadas.
        /// 
        /// Caso a query esteja em um formato incorreto, uma exceção interna é lançada e mostrada ao usuário por meio de um MessageBox.
        /// </summary>
        /// <param name="query">Query no formato SQL para ser executada no SGBD.</param>
        /// <returns>Retorna se a execução da query teve sucesso ou não.</returns>
        public bool Delete(string query, List<MySqlParameter> parametros)
        {
            /* A execução das querys de Insert, Update e Delete são iguais.
            Por isso estamos chamando a mesma função já codificada. */
            return Insert(query, parametros);
        }
    }
}
