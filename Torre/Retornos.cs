using Projetinho;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nacional.Torre.ConnectToApi;
using System.Threading;

namespace Nacional.Torre {
    public partial class Retornos : UserControl {
        Requests requests = new Requests(); 
        public Retornos() {
            InitializeComponent();
        }

        private async void dateTimePicker1_ValueChangedAsync(object sender, EventArgs e) {
            string data = dtpData.Value.Date.ToString("yyyy-MM-dd");
            await atualizaGrid(cbPlaca.Text, data);
        }

        private async void cbPlaca_SelectedIndexChanged(object sender, EventArgs e) {
            string data = dtpData.Value.Date.ToString("yyyy-MM-dd");
            await atualizaGrid(cbPlaca.Text, data);
        }

        private void Retornos_Load(object sender, EventArgs e) {
            cbPlaca.Items.Clear();
            Thread t = new Thread(new ThreadStart(AlimentaPlaca));
            t.Start();
        }
        private async void AlimentaPlaca() {
            while (true) {
                try {
                    cbPlaca.Invoke(new Action(() => {
                        cbPlaca.Items.Clear();
                    }));
                    Requests requests = new Requests();
                    await requests.GetVeiculos("Veiculos");
                    cbPlaca.Invoke(new Action(() => {
                        cbPlaca.Items.AddRange(requests.placas.ToArray());
                    }));
                    break;
                }
                catch (Exception) {
                    Thread.Sleep(2000);
                }
            }

        }
        private async Task atualizaGrid(string placa, string data) {
            try {
                await requests.PostVerPedidosPlacaData(placa, data);
                dataGridView1.Rows.Clear();
                foreach (var i in requests.notasDisp) {
                    dataGridView1.Rows.Add(false,
                        i.documento,
                        i.cliente,
                        i.bairro,
                        i.cidade,
                        i.cep,
                        i.peso);
                }
                btnApontar.Enabled = dataGridView1.Rows.Count > 0 ? true : false;
            }
            catch (Exception) { }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            try {
                dataGridView1.Rows[e.RowIndex].Cells[0].Value = (bool)dataGridView1.Rows[e.RowIndex].Cells[0].Value == false ? true : false;
               
                for (int i = 0; i < dataGridView1.Rows.Count; i++) {
                    bool check = (bool)dataGridView1.Rows[i].Cells[0].Value;
                    dataGridView1.Rows[i].DefaultCellStyle.SelectionBackColor = Color.White;
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    if (check) {
                        dataGridView1.Rows[i].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                    }
                }
            }
            catch (Exception) { }
        }

        private async void btnApontar_Click(object sender, EventArgs e) {
            List<int> notasSelecionadas = new List<int>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++) {
                bool check = (bool)dataGridView1.Rows[i].Cells[0].Value;
                if (check) {
                    notasSelecionadas.Add(int.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString()));
                }
            }
            Requests requests = new Requests();
            await requests.PosUpgradeData(notasSelecionadas);
            string data = dtpData.Value.Date.ToString("yyyy-MM-dd");
            await atualizaGrid(cbPlaca.Text, data);
            DialogResult dialogResult = MessageBox.Show("Deseja apontar reentrega no SSW?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes) {
                await requests.PostRetornos(notasSelecionadas);
            }
            notasSelecionadas.Clear();
        }

        private async void Retornos_VisibleChanged(object sender, EventArgs e) {
            string data = dtpData.Value.Date.ToString("yyyy-MM-dd");
            await atualizaGrid(cbPlaca.Text, data);
        }
    }
}
