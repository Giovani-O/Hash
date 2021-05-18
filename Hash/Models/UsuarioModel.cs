using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hash.Utils;
using MySql.Data.MySqlClient;

namespace Hash.Models
{
    class UsuarioModel
    {
        private static string tabelaUsuarios = "usuarios";

        private string nome;
        private string senha;
        private string senhaSalt;
        private string descricao;

        public string Nome { get => nome; set => nome = value; }
        public string Senha { get => senha; set => senha = value; }
        public string SenhaSalt { get => senhaSalt; set => senhaSalt = value; }
        public string Descricao { get => descricao; set => descricao = value; }

        public static bool Salvar(UsuarioModel novoUsuario)
        {
            DB database = new DB();

            string query =  "INSERT INTO usuarios (nome, senha, senhaSalt, descricao) " +
                            "VALUES (@nome, @senha, @senhaSalt, @descricao)";

            // Definindo parametros da query
            List<MySqlParameter> parametros = new List<MySqlParameter>();
            parametros.Add(new MySqlParameter("@nome", MySqlDbType.String));
            parametros.Add(new MySqlParameter("@senha", MySqlDbType.String));
            parametros.Add(new MySqlParameter("@senhaSalt", MySqlDbType.String));
            parametros.Add(new MySqlParameter("@descricao", MySqlDbType.String));
            parametros[0].Value = novoUsuario.Nome;
            parametros[1].Value = novoUsuario.Senha;
            parametros[2].Value = novoUsuario.SenhaSalt;
            parametros[3].Value = novoUsuario.Descricao;

            try
            {
                database.Insert(query, parametros);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static List<string> Autenticar(string nome)
        {
            DB database = new DB();
            string query = "SELECT nome, senha, senhaSalt FROM usuarios WHERE nome = @nome;";

            List<MySqlParameter> parametros = new List<MySqlParameter>();
            parametros.Add(new MySqlParameter("@nome", MySqlDbType.String));
            parametros[0].Value = nome;

            List<string> retorno = database.SelectData(query, parametros, 3);

            if (retorno.Count > 0)
            {
                return retorno;
            }
            else
            {
                MessageBox.Show("Falha ao autenticar!");
                return retorno;
            }
            
        }
    }
}
