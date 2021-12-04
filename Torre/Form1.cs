using System;
using System.Windows.Forms;

namespace Projetinho {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            index1.Show();
            userControl121.Hide();
            userControl31.Hide();
            uC_AnexarXml2.Hide();
            retornos1.Hide();
        }

        private void button1_Click(object sender, EventArgs e) {
            // Esconder outros
            index1.Hide();
            retornos1.Hide();
            userControl121.Hide();
            userControl31.Hide();
            // Mostrar tela pretendida
            uC_AnexarXml2.Show();
            // Trazer tela pra frente
            uC_AnexarXml2.BringToFront();
        }

        private void button2_Click(object sender, EventArgs e) {
            index1.Hide();
            retornos1.Hide();
            uC_AnexarXml2.Hide();
            userControl31.Hide();
            userControl121.Show();
            userControl121.BringToFront();
        }

        private void button3_Click(object sender, EventArgs e) {
            index1.Hide();
            retornos1.Hide();
            uC_AnexarXml2.Hide();
            userControl121.Hide();
            userControl31.Show();
            userControl31.BringToFront();
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            retornos1.Hide();
            userControl121.Hide();
            userControl31.Hide();
            uC_AnexarXml2.Hide();
            index1.Show();
            index1.BringToFront();
        }

        private void button4_Click(object sender, EventArgs e) {
            index1.Hide();
            userControl121.Hide();
            userControl31.Hide();
            uC_AnexarXml2.Hide();
            retornos1.Show();
            retornos1.BringToFront();
        }
    }
}
