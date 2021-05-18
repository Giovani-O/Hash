using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hash.Models;
using Hash.Utils;


namespace Hash.Controllers
{
    class UsuarioController
    {
        public static void Salvar(string nome, string senha, string descricao)
        {
            UsuarioModel usuario = new UsuarioModel();
            usuario.Nome = nome;
            usuario.Descricao = descricao;

            // Gerando e criptografando o Salt para a senha
            Random rand = new Random();
            double numeroAleatorio = rand.NextDouble();
            usuario.SenhaSalt = numeroAleatorio.ToString();
            usuario.SenhaSalt = Criptografia.GerarMD5(usuario.SenhaSalt);

            // Armazenando e criptografando a senha com salt
            usuario.Senha = Criptografia.GerarMD5(senha + usuario.SenhaSalt);

            if (UsuarioModel.Salvar(usuario))
            {
                MessageBox.Show("Usuário cadastrado com sucesso!");
            }
            else
            {
                MessageBox.Show("Erro: Usuário NÃO cadastrado!");
            }
        }

        public static void Autenticar(string nome, string senha)
        {
            List<string> retorno = UsuarioModel.Autenticar(nome);

            if (retorno.Count > 0)
            {
                string nomeDB = retorno[0];
                string senhaDB = retorno[1];
                string saltDB = retorno[2];

                if (Criptografia.GerarMD5(senha + saltDB) == senhaDB)
                {
                    MessageBox.Show("Credenciais autenticadas com sucesso!");
                }
                else
                {
                    MessageBox.Show("Falha ao autenticar!");
                }
            }
        }
    }
}
