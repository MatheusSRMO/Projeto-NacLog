using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;
using Nacional.Torre.ConnectToApi;
using System.Threading;

namespace Projetinho {
    public partial class UserControl12 : UserControl {
        Requests requests = new Requests();
        string caminho = @".\nacional-logistica.png";
        double pesoL = 0;
        public List<int> NotasSelecionadas = new List<int>();
        BaseFont fonteBase = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, false);
        public UserControl12() {
            InitializeComponent();
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            try {
                dataGridView1.Rows[e.RowIndex].Cells[0].Value = (bool)dataGridView1.Rows[e.RowIndex].Cells[0].Value == false ? true : false;
                int cont = 0;
                double pesoTotal = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++) {
                    bool check = (bool)dataGridView1.Rows[i].Cells[0].Value;
                    dataGridView1.Rows[i].DefaultCellStyle.SelectionBackColor = Color.White;
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    if (check) {
                        dataGridView1.Rows[i].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                        cont += 1;
                        pesoTotal += double.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString());
                    }
                }
                tbTotalNotas.Text = cont.ToString();
                tbTotalPeso.Text = Math.Round(pesoTotal, 2).ToString();
            }
            catch (Exception) { }
        }
        private async void button1_Click_1(object sender, EventArgs e) {
            if (cbMotorista.Text != "" && cbPlaca.Text != "") {
                for (int j = 0; j < dataGridView1.Rows.Count; j++) {
                    bool check = (bool)dataGridView1.Rows[j].Cells[0].Value;
                    if (check) {
                        int notaSelect = (int)dataGridView1.Rows[j].Cells[1].Value;
                        NotasSelecionadas.Add(notaSelect);
                    }
                }
                if (NotasSelecionadas.Count == 0) {
                    MessageBox.Show("Selecione ao menos uma nota!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else {
                    DialogResult dialogResultSSW = MessageBox.Show("Deseja gerar o romaneio no SSW?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResultSSW == DialogResult.Yes) {
                        // Gera Romaneio SSW \\
                        await requests.PostRomaneio(cbPlaca.Text, cbMotorista.Text, NotasSelecionadas);
                    }

                    DialogResult dialogResult = MessageBox.Show("Deseja emitir os Romaneios?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    
                    if (dialogResult == DialogResult.Yes) {
                        // Gera os relatorios \\
                        try {
                            await GeraPdf(cbPlaca.Text, cbMotorista.Text);
                            await GeraPdf2(cbPlaca.Text, cbMotorista.Text);
                            await GeraPdf3(cbPlaca.Text, cbMotorista.Text);
                        }
                        catch (Exception) {
                            MessageBox.Show("Erro ao emitir romaneios!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    try {
                        // Registra que foi gerado, muda o estado de disponivel para placa \\
                        await RegistraBanco(cbPlaca.Text);
                    }
                    catch (Exception) { }

                    // Limpa as notas selecionadas \\
                    NotasSelecionadas.Clear();

                    try {
                        // Atualiza o grid \\
                        await atualizaGrid();
                    }
                    catch (Exception) { }
                }
            }
            else if (cbPlaca.Text == "") {
                MessageBox.Show("Selecione a placa!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                MessageBox.Show("Selecione o motorista!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task RegistraBanco(string placa) {
            Requests requests = new Requests();
            List<Registra> registros = new List<Registra>();
            foreach (int nota in NotasSelecionadas) {
                registros.Add(new Registra {
                    senha = requests.ValidSenha,
                    placa = placa,
                    documento = nota
                });
            }
            await requests.PostRegistraPlaca(registros);
        }
        private void CriarCelulaTexto(PdfPTable tabela,
            string texto, int alinhamentoHorz = PdfPCell.ALIGN_LEFT,
            bool negrito = false, bool italico = false,
            int tamanhoFonte = 10, int alturaCelula = 25) {
            int estilo = iTextSharp.text.Font.NORMAL;
            if (negrito && italico) {
                estilo = iTextSharp.text.Font.BOLDITALIC;
            }
            else if (negrito) {
                estilo = iTextSharp.text.Font.BOLD;
            }
            else if (italico) {
                estilo = iTextSharp.text.Font.ITALIC;
            }
            var fonteCelula = new iTextSharp.text.Font(fonteBase, tamanhoFonte, estilo, BaseColor.BLACK);
            var celula = new PdfPCell(new Phrase(texto, fonteCelula));
            celula.HorizontalAlignment = alinhamentoHorz;
            celula.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            celula.Border = 0;
            celula.BorderWidthTop = 1;
            celula.FixedHeight = alturaCelula;
            tabela.AddCell(celula);
        }
        public async Task GeraPdf(string placa, string motorista) {
            //Configurar o documento PDF
            var pxPorMm = 72 / 25.2F;
            var pdf = new Document(PageSize.A4, 10 * pxPorMm, 10 * pxPorMm,
                15 * pxPorMm, 15 * pxPorMm);
            var nomeArquivo = $"./romaneios/romaneio.{DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss")}.pdf";
            var arquivo = new FileStream(nomeArquivo, FileMode.Create);
            var writer = PdfWriter.GetInstance(pdf, arquivo);

            pdf.Open();

            //Titulo

            var fonteParagrafo = new iTextSharp.text.Font(fonteBase, 25,
                iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            var titulo = new Paragraph("Romaneio de Clientes\n\n", fonteParagrafo);
            titulo.Alignment = Element.ALIGN_LEFT;
            pdf.Add(titulo);

            var caminhoImagem = caminho;

            if (File.Exists(caminhoImagem)) {
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(caminhoImagem);
                float razaoAlturaLargura = logo.Width / logo.Height;
                float alturaLogo = 50;
                float larguraLogo = alturaLogo * razaoAlturaLargura;
                logo.ScaleToFit(larguraLogo, alturaLogo);
                var margemEsquerda = pdf.PageSize.Width - pdf.RightMargin - larguraLogo;
                var margemTopo = pdf.PageSize.Height - pdf.TopMargin - 54;
                logo.SetAbsolutePosition(margemEsquerda, margemTopo);
                writer.DirectContent.AddImage(logo, false);
            }

            var fontelb = new iTextSharp.text.Font(fonteBase, 13,
                iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            string mens = $"Placa    : {(placa == "" ? "______________________" : placa)}\n" +
                $"Motorista: {(motorista == "" ? "______________________" : motorista)}\n" +
                $"Ajudante : ______________________\n\n";
            var label = new Paragraph(mens, fontelb);
            label.Alignment = Element.ALIGN_LEFT;
            pdf.Add(label);

            //Adição tabela
            var tabela = new PdfPTable(7);
            float[] larguraColunas = { 1f, 2.5f, 2f, 1f, 1.3f, 1.1f, 0.7f };
            tabela.SetWidths(larguraColunas);
            tabela.DefaultCell.BorderWidth = 0;
            tabela.WidthPercentage = 100;

            CriarCelulaTexto(tabela, "Nota Fiscal", PdfPCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "Cliente", PdfPCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "Bairro", PdfPCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "Cidade", PdfPCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "Valor (R$)", PdfPCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "Peso (KG)", PdfPCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "Vol", PdfPCell.ALIGN_CENTER, true);

            double peso = 0;
            double valor = 0;
            int volume = 0;

            await requests.PostBuscaNotas(NotasSelecionadas);

            foreach (var i in requests.notasDisp) {
                peso += i.peso;
                valor += i.valorNf;
                volume += i.volume;
                CriarCelulaTexto(tabela, i.documento.ToString(), PdfPCell.ALIGN_CENTER);
                CriarCelulaTexto(tabela, i.cliente, PdfPCell.ALIGN_CENTER);
                CriarCelulaTexto(tabela, i.bairro, PdfPCell.ALIGN_CENTER);
                CriarCelulaTexto(tabela, i.cidade, PdfPCell.ALIGN_CENTER);
                CriarCelulaTexto(tabela, String.Format("{0:n}", Math.Round(i.valorNf, 2)), PdfPCell.ALIGN_CENTER);
                CriarCelulaTexto(tabela, String.Format("{0:n}", Math.Round(i.peso, 2)), PdfPCell.ALIGN_CENTER);
                CriarCelulaTexto(tabela, String.Format("{0:n0}", i.volume), PdfPCell.ALIGN_CENTER);

                //Aqui entra um metodo para deixar indisponivel a nota
            }
            pesoL = peso;
            CriarCelulaTexto(tabela, $"Total:", PdfPCell.ALIGN_CENTER);
            CriarCelulaTexto(tabela, $"{requests.notasDisp.Length} notas", PdfPCell.ALIGN_CENTER);
            CriarCelulaTexto(tabela, "", PdfPCell.ALIGN_CENTER);
            CriarCelulaTexto(tabela, "", PdfPCell.ALIGN_CENTER);
            CriarCelulaTexto(tabela, String.Format("{0:n}", Math.Round(valor, 2)), PdfPCell.ALIGN_CENTER);
            CriarCelulaTexto(tabela, String.Format("{0:n}", Math.Round(peso, 2)), PdfPCell.ALIGN_CENTER);
            CriarCelulaTexto(tabela, String.Format("{0:n0}", volume), PdfPCell.ALIGN_CENTER);
            pdf.Add(tabela);


            pdf.Close();
            arquivo.Close();
            var caminhoPDF = AppDomain.CurrentDomain.BaseDirectory;

            if (File.Exists($@"{caminhoPDF}/{nomeArquivo}")) {
                Process.Start(new ProcessStartInfo() {
                    Arguments = $"/c start C:/\"Program Files\"/NacLog/{nomeArquivo}",
                    FileName = "cmd.exe",
                    CreateNoWindow = true
                });
            }
        }
        public async Task GeraPdf2(string placa, string motorista) {
            //Configurar o documento PDF
            var pxPorMm = 72 / 25.2F;
            var pdf = new Document(PageSize.A4, 10 * pxPorMm, 10 * pxPorMm,
                15 * pxPorMm, 15 * pxPorMm);
            var nomeArquivo = $"romaneios/romaneio.{DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss")}(1).pdf";
            var arquivo = new FileStream(nomeArquivo, FileMode.Create);
            var writer = PdfWriter.GetInstance(pdf, arquivo);

            pdf.Open();

            //Titulo

            var fonteParagrafo = new iTextSharp.text.Font(fonteBase, 25,
                iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            var titulo = new Paragraph("Romaneio de Separação\nPor Clientes\n\n", fonteParagrafo);
            titulo.Alignment = Element.ALIGN_LEFT;
            pdf.Add(titulo);

            var caminhoImagem = caminho;

            if (File.Exists(caminhoImagem)) {
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(caminhoImagem);
                float razaoAlturaLargura = logo.Width / logo.Height;
                float alturaLogo = 50;
                float larguraLogo = alturaLogo * razaoAlturaLargura;
                logo.ScaleToFit(larguraLogo, alturaLogo);
                var margemEsquerda = pdf.PageSize.Width - pdf.RightMargin - larguraLogo;
                var margemTopo = pdf.PageSize.Height - pdf.TopMargin - 54;
                logo.SetAbsolutePosition(margemEsquerda, margemTopo);
                writer.DirectContent.AddImage(logo, false);
            }
            var fontelb1 = new iTextSharp.text.Font(fonteBase, 13,
                iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            string mens = $"Placa    : {(placa == "" ? "______________________" : placa)}\n" +
                $"Motorista: {(motorista == "" ? "______________________" : motorista)}\n" +
                $"Ajudante : ______________________\n\n";
            var label1 = new Paragraph(mens, fontelb1);
            label1.Alignment = Element.ALIGN_LEFT;
            pdf.Add(label1);

            Requests requests = new Requests();
            await requests.PostBuscaDados(NotasSelecionadas);

            foreach (var dados in requests.notasDisp2) {
                var fontelb = new iTextSharp.text.Font(fonteBase, 7.6f,
                    iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
                var label = new Paragraph(dados.dado + "\n------------------------" +
                    "---------------------------------------------------------------" +
                    "------------------------------", fontelb);
                label.Alignment = Element.ALIGN_LEFT;
                pdf.Add(label);
            }
            pdf.Close();
            arquivo.Close();
            var caminhoPDF = AppDomain.CurrentDomain.BaseDirectory;

            if (File.Exists($@"{caminhoPDF}/{nomeArquivo}")) {
                Process.Start(new ProcessStartInfo() {
                    Arguments = $"/c start C:/\"Program Files\"/NacLog/{nomeArquivo}",
                    FileName = "cmd.exe",
                    CreateNoWindow = true
                });
            }
        }
        public async Task GeraPdf3(string placa, string motorista) {
            //Configurar o documento PDF
            var pxPorMm = 72 / 25.2F;
            var pdf = new Document(PageSize.A4, 10 * pxPorMm, 10 * pxPorMm,
                15 * pxPorMm, 15 * pxPorMm);
            var nomeArquivo = $"./romaneios/romaneio.{DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss")}(2).pdf";
            var arquivo = new FileStream(nomeArquivo, FileMode.Create);
            var writer = PdfWriter.GetInstance(pdf, arquivo);

            pdf.Open();

            //Titulo

            var fonteParagrafo = new iTextSharp.text.Font(fonteBase, 25,
                iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            var titulo = new Paragraph("Romaneio de Separação\n\n", fonteParagrafo);
            titulo.Alignment = Element.ALIGN_LEFT;
            pdf.Add(titulo);

            //Imagem
            var caminhoImagem = caminho;

            if (File.Exists(caminhoImagem)) {
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(caminhoImagem);
                float razaoAlturaLargura = logo.Width / logo.Height;
                float alturaLogo = 50;
                float larguraLogo = alturaLogo * razaoAlturaLargura;
                logo.ScaleToFit(larguraLogo, alturaLogo);
                var margemEsquerda = pdf.PageSize.Width - pdf.RightMargin - larguraLogo;
                var margemTopo = pdf.PageSize.Height - pdf.TopMargin - 54;
                logo.SetAbsolutePosition(margemEsquerda, margemTopo);
                writer.DirectContent.AddImage(logo, false);
            }

            var fontelb1 = new iTextSharp.text.Font(fonteBase, 13,
                iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            string mens = $"Placa    : {(placa == "" ? "______________________" : placa)}\n" +
                $"Motorista: {(motorista == "" ? "______________________" : motorista)}\n" +
                $"Ajudante : ______________________\n\n";
            var label1 = new Paragraph(mens, fontelb1);
            label1.Alignment = Element.ALIGN_LEFT;
            pdf.Add(label1);

            DataTable limpo = new DataTable();
            limpo.Columns.Add("Codigo", typeof(int));
            limpo.Columns.Add("Produto", typeof(string));
            limpo.Columns.Add("PesoLiquido", typeof(float));
            limpo.Columns.Add("Quantidade", typeof(int));
            limpo.Columns.Add("PesoBruto", typeof(float));
            limpo.Columns.Add("Unidade", typeof(string));
            limpo.Columns.Add("Dados", typeof(string));
            limpo.Columns.Add("Ordem", typeof(string));

            // CONTEUDO DO PDF \\
            var enc = new Regex(@"(\d{6}\s)(.{40})\s*([\d\,\.]*)[\sA-Z]*([\d\,\.]*)[\sA-Z]*([\d\,\.]*) ([A-Z]{2}).*");
            var ordemEntregaRex = new Regex(@"Ordem Entrega:(.*)");
            Requests requests = new Requests();
            await requests.PostBuscaDados(NotasSelecionadas);

            foreach (var dados in requests.notasDisp2) {
                var OrdemEntrega = ordemEntregaRex.Match(dados.dado).Groups[1].Value.Trim();
                var dados1 = enc.Replace(dados.dado, "######").Split("######");
                var format = enc.Matches(dados.dado);

                for (var j = 0; j < format.Count; j++) {

                    var codigo = format[j].Groups[1].Value;
                    var produto = format[j].Groups[2].Value.ToString().Trim();
                    var pesoLiquido = format[j].Groups[3].Value;
                    var quantidade = format[j].Groups[4].Value;
                    var pesoBruto = format[j].Groups[5].Value;
                    var unidade = format[j].Groups[6].Value;
                    var dados2 = dados1[j + 1].Replace("\n", "").Trim();

                    if (float.Parse(quantidade) == 1f) {
                        dados2 = pesoLiquido;
                    }

                    DataRow[] rows = limpo.Select("Codigo=" + codigo);

                    if (rows.Length == 0) {
                        DataRow linha = limpo.NewRow();
                        linha["Codigo"] = int.Parse(codigo);
                        linha["Produto"] = produto;
                        linha["PesoLiquido"] = float.Parse(pesoLiquido);
                        linha["Quantidade"] = (int)float.Parse(quantidade);
                        linha["PesoBruto"] = float.Parse(pesoBruto);
                        linha["Unidade"] = unidade;
                        linha["Dados"] = dados2 != ""? $"{OrdemEntrega}: {dados2}": dados2;
                        linha["Ordem"] = OrdemEntrega;
                        limpo.Rows.Add(linha);
                    }
                    else {
                        var PesoLiquidoOK = rows[0].ItemArray[2].ToString().Replace(".", ",");
                        var QuantidadeOK = rows[0].ItemArray[3].ToString();
                        var PesoBrutoOK = rows[0].ItemArray[4].ToString().Replace(".", ",");
                        var DadosOK = rows[0].ItemArray[6].ToString();
                        rows[0].BeginEdit();
                        rows[0]["PesoLiquido"] = float.Parse(PesoLiquidoOK) + float.Parse(pesoLiquido);
                        rows[0]["Quantidade"] = float.Parse(QuantidadeOK) + float.Parse(quantidade);
                        rows[0]["PesoBruto"] = float.Parse(PesoBrutoOK) + float.Parse(pesoBruto);
                        var cp = dados2 != "" ? $"{int.Parse(OrdemEntrega)}: {dados2}" : "";
                        rows[0]["Dados"] = DadosOK + "  " + cp;
                        rows[0].EndEdit();
                    }
                }
            }

            var quantidadeTotal = 0f;
            var pesoBrutoTotal = 0f;

            for (int i = 0; i < limpo.Rows.Count; i++) {
                var tabela = new PdfPTable(5);
                float[] larguraColunas = { 0.5f, 2.7f, 1f, 1f, 1.3f };
                tabela.SetWidths(larguraColunas);
                tabela.DefaultCell.BorderWidth = 0;
                tabela.WidthPercentage = 100;

                quantidadeTotal += int.Parse(limpo.Rows[i][3].ToString());
                pesoBrutoTotal += float.Parse(limpo.Rows[i][4].ToString());

                var codigo = limpo.Rows[i][0].ToString();
                var produto = limpo.Rows[i][1].ToString();
                var pesoLiquido = limpo.Rows[i][2].ToString();
                var quantidade = limpo.Rows[i][3].ToString();
                var pesoBruto = limpo.Rows[i][4].ToString();
                var unidade = limpo.Rows[i][5].ToString();
                var dadosOK = limpo.Rows[i][6].ToString();
                var Ordem = limpo.Rows[i][7].ToString();

                CriarCelulaTexto(tabela, codigo, PdfPCell.ALIGN_LEFT, tamanhoFonte: 9, italico: true);
                CriarCelulaTexto(tabela, produto, PdfPCell.ALIGN_LEFT, tamanhoFonte: 9, italico: true);
                CriarCelulaTexto(tabela, String.Format("{0:n3}", float.Parse(pesoLiquido)) + " " + unidade, PdfPCell.ALIGN_RIGHT, tamanhoFonte: 9, italico: true);
                CriarCelulaTexto(tabela, $"{(quantidade == "1" ? $"{Ordem}: " : "")}" + String.Format("{0:n3}", int.Parse(quantidade)) + " CX", PdfPCell.ALIGN_RIGHT, tamanhoFonte: 9, italico: true);
                CriarCelulaTexto(tabela, String.Format("{0:n3}", float.Parse(pesoBruto)) + " " + unidade + " BRUTO", PdfPCell.ALIGN_RIGHT, tamanhoFonte: 9, italico: true);
                pdf.Add(tabela);

                List<string> vs = new List<string>();
                int igual = 1;
                var formatE = new Regex(@"([\d\,\.]{5,20})");
                var dadosOKArray = formatE.Matches(dadosOK);
                foreach (var q in dadosOKArray) {
                    if (vs.Contains(q.ToString())) {
                        igual += 1;
                    }
                    else {
                        vs.Add(q.ToString());
                    }
                }
                if (dadosOKArray.Count != igual && dadosOK.Trim() != "") {
                    var dadosOK1 = new Regex(@"(\s{1,1000})");
                    dadosOK = dadosOK1.Replace(dadosOK, " ");
                    var fonteParagrafo1 = new iTextSharp.text.Font(fonteBase, 9,
                    iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    var titulo1 = new Paragraph($"{dadosOK}\n\n", fonteParagrafo1);
                    titulo1.Alignment = Element.ALIGN_LEFT;
                    pdf.Add(titulo1);
                }
            }
            var tabelaF = new PdfPTable(5);
            float[] larguraColunasF = { 0.5f, 2.7f, 1f, 1f, 1.3f };
            tabelaF.SetWidths(larguraColunasF);
            tabelaF.DefaultCell.BorderWidth = 0;
            tabelaF.WidthPercentage = 100;
            CriarCelulaTexto(tabelaF, "TOTAL:", PdfPCell.ALIGN_LEFT, tamanhoFonte: 9, italico: true);
            CriarCelulaTexto(tabelaF, "", PdfPCell.ALIGN_LEFT, tamanhoFonte: 9, italico: true);
            CriarCelulaTexto(tabelaF, $"", PdfPCell.ALIGN_RIGHT, tamanhoFonte: 9, italico: true);
            CriarCelulaTexto(tabelaF, String.Format("{0:n3}", quantidadeTotal) + " CX", PdfPCell.ALIGN_RIGHT, tamanhoFonte: 9, italico: true);
            CriarCelulaTexto(tabelaF, $"{String.Format("{0:n3}", pesoBrutoTotal)} KG BRUTO", PdfPCell.ALIGN_RIGHT, tamanhoFonte: 9, italico: true);

            pdf.Add(tabelaF);

            pdf.Close();
            arquivo.Close();
            var caminhoPDF = AppDomain.CurrentDomain.BaseDirectory;

            if (File.Exists($@"{caminhoPDF}/{nomeArquivo}")) {
                Process.Start(new ProcessStartInfo() {
                    Arguments = $"/c start C:/\"Program Files\"/NacLog/{nomeArquivo}",
                    FileName = "cmd.exe",
                    CreateNoWindow = true
                });
            }
        }
        private async Task atualizaGrid() {
            try {
                Requests requests = new Requests();
                await requests.PostVerPedidos("1");
                tbTotalPeso.Text = "0";
                tbTotalNotas.Text = "0";
                cbMotorista.Text = "";
                cbPlaca.Text = "";
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
            }
            catch (Exception) {}
        }
        private void UserControl12_Load(object sender, EventArgs e) {
            if (!this.DesignMode) {
                Thread t = new Thread(new ThreadStart(AlimentaPlaca));
                t.Start();
            }
        }
        private async void AlimentaPlaca() {
            while (true) {
                try {
                    cbPlaca.Invoke(new Action(() => {
                        cbPlaca.Items.Clear();
                    }));
                    cbMotorista.Invoke(new Action(() => {
                        cbMotorista.Items.Clear();
                    }));
                    
                    Requests requests = new Requests();
                    await requests.GetVeiculos("Veiculos");
                    await requests.GetMotoristas("Motoristas");
                    cbPlaca.Invoke(new Action(() => {
                        cbPlaca.Items.AddRange(requests.placas.ToArray());
                    }));
                    cbMotorista.Invoke(new Action(() => {
                        cbMotorista.Items.AddRange(requests.motoristas.ToArray());
                    }));
                    break;
                }
                catch (Exception) {
                    Thread.Sleep(2000);
                }
            }
        }
        private async void UserControl12_VisibleChanged(object sender, EventArgs e) {
            if (!this.DesignMode) {
                await atualizaGrid();
            }
        }
    }
}
