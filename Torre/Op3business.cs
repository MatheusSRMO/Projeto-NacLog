using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Nacional.Torre.ConnectToApi;
using Projetinho;

namespace Nacional {
    class Op3business {
        BaseFont fonteBase = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, false);
        public List<int> todasNotas = new List<int>();
        public string mensagem = "";
        public string pesoTotal = "";
        public string valorTotal = "";
        public string volumeTotal = "";
        public double valor = 0;
        public double peso = 0;
        public int volume = 0;
        public void GeraPdf4(Pedidos[] notas, Dados[] dados) {
            //Configurar o documento PDF
            var pxPorMm = 72 / 25.2F;
            var pdf = new Document(PageSize.A4, 10 * pxPorMm, 10 * pxPorMm,
                15 * pxPorMm, 15 * pxPorMm);
            var nomeArquivo = $"romaneios/romaneio.{DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss")}.pdf";
            var arquivo = new FileStream(nomeArquivo, FileMode.Create);
            var writer = PdfWriter.GetInstance(pdf, arquivo);

            pdf.Open();

            //Titulo

            var fonteParagrafo = new iTextSharp.text.Font(fonteBase, 30,
                iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            var titulo = new Paragraph("Romaneio de Clientes\n", fonteParagrafo);
            titulo.Alignment = Element.ALIGN_LEFT;
            pdf.Add(titulo);

            var caminhoImagem = @"C:\Users\mathe\OneDrive\Área de Trabalho\nacional-logistica.png";

            if (File.Exists(caminhoImagem)) {
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(caminhoImagem);
                float razaoAlturaLargura = logo.Width / logo.Height;
                float alturaLogo = 32;
                float larguraLogo = alturaLogo * razaoAlturaLargura;
                logo.ScaleToFit(larguraLogo, alturaLogo);
                var margemEsquerda = pdf.PageSize.Width - pdf.RightMargin - larguraLogo;
                var margemTopo = pdf.PageSize.Height - pdf.TopMargin - 54;
                logo.SetAbsolutePosition(margemEsquerda, margemTopo);
                writer.DirectContent.AddImage(logo, false);
            }


            //Adição tabela
            var tabela = new PdfPTable(7);
            float[] larguraColunas = { 1f, 2.5f, 2f, 1f, 1.3f, 1.1f, 0.7f };
            tabela.SetWidths(larguraColunas);
            tabela.DefaultCell.BorderWidth = 0;
            tabela.WidthPercentage = 100;

            var fontelb1 = new iTextSharp.text.Font(fonteBase, 7.5f,
                iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
            var label1 = new Paragraph("\n\n\n", fontelb1);
            label1.Alignment = Element.ALIGN_LEFT;
            pdf.Add(label1);

            CriarCelulaTexto(tabela, "Nota Fiscal", PdfPCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "Cliente", PdfPCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "Bairro", PdfPCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "Cidade", PdfPCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "Valor (R$)", PdfPCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "Peso (KG)", PdfPCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "Vol", PdfPCell.ALIGN_CENTER, true);

            foreach (var pedido in notas) {
                CriarCelulaTexto(tabela, pedido.documento.ToString(), PdfPCell.ALIGN_CENTER);
                CriarCelulaTexto(tabela, pedido.cliente, PdfPCell.ALIGN_CENTER);
                CriarCelulaTexto(tabela, pedido.bairro, PdfPCell.ALIGN_CENTER);
                CriarCelulaTexto(tabela, pedido.cidade, PdfPCell.ALIGN_CENTER);
                CriarCelulaTexto(tabela, String.Format("{0:n}", Math.Round(pedido.valorNf, 2)), PdfPCell.ALIGN_CENTER);
                CriarCelulaTexto(tabela, String.Format("{0:n}", Math.Round(pedido.peso, 2)), PdfPCell.ALIGN_CENTER);
                CriarCelulaTexto(tabela, String.Format("{0:n0}", pedido.volume), PdfPCell.ALIGN_CENTER);
            }

            pdf.Add(tabela);

            foreach (var pedido in dados) {
                var fontelb = new iTextSharp.text.Font(fonteBase, 7.5f,
                    iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
                var label = new Paragraph("\n" + pedido.dado, fontelb);
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
                celula.BorderWidthBottom = 1;
                celula.FixedHeight = alturaCelula;
                tabela.AddCell(celula);
        }
      }
}
