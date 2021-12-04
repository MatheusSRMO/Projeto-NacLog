using Nacional.Torre;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Nacional.Torre.ConnectToApi;

namespace Projetinho {
    public partial class UC_AnexarXml : UserControl {
        List<Pedidos> pedidos = new List<Pedidos>();
        public UC_AnexarXml() {
            InitializeComponent();
        }
        string rota;
        private async void btn_buscar_Click(object sender, EventArgs e) {
            using (var openDlg = new FolderBrowserDialog()) {
                openDlg.SelectedPath = "C:\\Downloads";
                if (openDlg.ShowDialog() == DialogResult.OK) {
                    rota = openDlg.SelectedPath;
                    foreach (var arquivo in Directory.GetFiles(rota)) {
                        lerXml(arquivo);
                    }
                    Requests requests = new Requests();
                    await requests.PostRegistroPedidos(pedidos);
                    MessageBox.Show($"{requests.quantidadeAdd} notas adicionadas ao banco de dados!");
                }
            }
        }
        private void lerXml(string path) {
            var destinatario = false;
            var totalA = 0;
            string Informação = "";
            var info = new List<string>();
            var produtos = new List<string>();
            using (XmlReader arquivo = XmlReader.Create(path)) {
                while (arquivo.Read()) {
                    if (arquivo.Name == "nNF") info.Add(arquivo.ReadElementContentAsString());
                    if (arquivo.Name == "dest") {
                        destinatario = true;
                    }
                    if (destinatario) {
                        if (arquivo.Name == "CNPJ") info.Add(arquivo.ReadElementContentAsString());
                        if (arquivo.Name == "xNome") info.Add(arquivo.ReadElementContentAsString());
                        if (arquivo.Name == "xBairro") info.Add(arquivo.ReadElementContentAsString());
                        if (arquivo.Name == "xMun") info.Add(arquivo.ReadElementContentAsString());
                        if (arquivo.Name == "CEP") info.Add(arquivo.ReadElementContentAsString());
                    }
                    if (arquivo.Name == "det") destinatario = false;
                    if (arquivo.Name == "vNF") info.Add(arquivo.ReadElementContentAsString());
                    if (arquivo.Name == "pesoB") info.Add(arquivo.ReadElementContentAsString());
                    if (arquivo.Name == "qVol") totalA = int.Parse(arquivo.ReadElementContentAsString());
                    if (arquivo.Name == "infCpl") Informação = arquivo.ReadElementContentAsString();
                }
            }
            var reg = new Regex(@"PEDIDO:(.*)\s*-\s*CLIENTE");
            var pedido = reg.Match(Informação).Groups[1].ToString().Trim();
            pedidos.Add(new Pedidos{
                documento = int.Parse(info[0]),
                cnpj = (int)Int64.Parse(info[1]),
                cliente = info[2],
                bairro = info[3],
                cidade = info[4],
                valorNf = float.Parse(info[6].Replace(".",",")),
                peso = float.Parse(info[7].Replace(".", ",")),
                volume = totalA,
                data = DateTime.Now.ToString("yyyy-MM-dd"),
                disponivel = "1",
                cep = info[5],
                pedido = pedido
            });
        }

        private async void button1_Click(object sender, EventArgs e) {
            var opendlg = new OpenFileDialog();
            opendlg.Filter = "*.pdf|";
            if (opendlg.ShowDialog() == DialogResult.OK) {
                var exst = new FileInfo(opendlg.FileName);
                var nome = exst.Extension;
                if (nome != ".pdf") MessageBox.Show("Selecione um PDF!");
                else {
                    CadastraPedidos cadastraPedidos = new CadastraPedidos(opendlg.FileName);
                    Requests requests = new Requests();
                    await requests.PostRegistroDados(cadastraPedidos.dados);
                    MessageBox.Show($"{requests.quantidadeAdd} Notas adicionadas!");
                }
            }
        }
    }
}
