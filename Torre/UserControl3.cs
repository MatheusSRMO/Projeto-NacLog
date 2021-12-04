using Nacional;
using Nacional.Torre.ConnectToApi;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;


namespace Projetinho {
    public partial class UserControl3 : UserControl {
        
        public UserControl3() {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e) {
            try {
                string dataDe = dpDe.Value.Date.ToString("yyyy-MM-dd");
                string dataAte = dpAte.Value.Date.ToString("yyyy-MM-dd");
                List<int> listNotas = new List<int>();
                Requests requests = new Requests();
                await requests.PostBuscaEntreDatas(dataDe, dataAte);
                foreach (var i in requests.listNotas) {
                    listNotas.Add(i.documento);
                }
                UserControl12 userControl12 = new UserControl12();
                userControl12.NotasSelecionadas = listNotas;
                await userControl12.GeraPdf("", "");
                await userControl12.GeraPdf2("", "");
                await userControl12.GeraPdf3("", "");
                userControl12.NotasSelecionadas.Clear();
                listNotas.Clear();
            }
            catch (Exception) {}
        }

        private async void button2_Click(object sender, EventArgs e) {
            try {
                if (mtbPlaca.Text != "") {
                    Requests requests = new Requests();
                    await requests.PostVerPedidosPlacaData(mtbPlaca.Text, dpData.Value.Date.ToString("yyyy-MM-dd"));
                    List<int> nt = new List<int>();
                    foreach (var i in requests.notasDisp) {
                        nt.Add(i.documento);
                    }
                    UserControl12 userControl12 = new UserControl12();
                    userControl12.NotasSelecionadas = nt;
                    await userControl12.GeraPdf(mtbPlaca.Text, "");
                    await userControl12.GeraPdf2(mtbPlaca.Text, "");
                    await userControl12.GeraPdf3(mtbPlaca.Text, "");
                    userControl12.NotasSelecionadas.Clear();
                    nt.Clear();
                }
            }
            catch (Exception) { }
        }

        private async void button3_Click_1(object sender, EventArgs e) {
            if (mtbNf.Text != "" && mtbNf.Text.Length >= 7) {
                try {
                    Requests requests = new Requests();
                    var Listnota = new List<int>();
                    Listnota.Add(int.Parse(mtbNf.Text));
                    await requests.PostBuscaNotas(Listnota);
                    await requests.PostBuscaDados(Listnota);
                    new Op3business().GeraPdf4(requests.notasDisp, requests.notasDisp2);
                }
                catch (Exception) {
                    MessageBox.Show("Nota Fiscal não encontrada no banco de dados!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else {
                MessageBox.Show("Digite uma nota fiscal valida!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UserControl3_Load(object sender, EventArgs e) {
            if (!this.DesignMode) {
                Thread t = new Thread(new ThreadStart(AlimentaPlaca));
                t.Start();
            }
        }
        private async void AlimentaPlaca() {
            while (true) {
                try {
                    mtbPlaca.Invoke(new Action(() => {
                        mtbPlaca.Items.Clear();
                    }));
                    Requests requests = new Requests();
                    await requests.GetVeiculos("Veiculos");
                    mtbPlaca.Invoke(new Action(() => {
                        mtbPlaca.Items.AddRange(requests.placas.ToArray());
                    }));
                    break;
                }
                catch (Exception) {
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
