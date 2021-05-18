using MaterialSkin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hash.Controllers;

namespace Hash
{
    public partial class Usuario : MaterialSkin.Controls.MaterialForm
    {

        public Usuario()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Purple700, Primary.Purple800, Primary.Purple500, Accent.Purple200, TextShade.WHITE);
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (txbNome.Text != "" && txbSenha.Text != "" && txbDescricao.Text != ""){
                UsuarioController.Salvar(txbNome.Text, txbSenha.Text, txbDescricao.Text);
                txbNome.Text = "";
                txbSenha.Text = "";
                txbDescricao.Text = "";
            }
            else
            {
                MessageBox.Show("Campos não preenchidos!");
            }
        }

        private void btnAutenticar_Click(object sender, EventArgs e)
        {
            UsuarioController.Autenticar(txbNome.Text, txbSenha.Text);
        }
    }
}
