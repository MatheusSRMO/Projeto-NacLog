using Projetinho;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronPy {
    public partial class login : Form {
        Thread t;
        public login() {
            InitializeComponent();
        }
        private async void button1_Click(object sender, EventArgs e) {
            try {
                Verifica verifica = new Verifica();
                await verifica.PostLogin(tbUsuario.Text, tbSenha.Text);
                if (verifica.tem) {
                    abrirPrograma();
                }
                else {
                    MessageBox.Show("Credenciais invalidas!", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception) {
                MessageBox.Show("Erro de conexão!");
            }
            
        }
        private void abrirPrograma() {
            t = new Thread(ThreadProc);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            this.Close();
        }
        public static void ThreadProc(Object obj) {
            Application.Run(new Form1());
        }
    }
}
