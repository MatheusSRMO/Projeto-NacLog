using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Nacional.Torre.ConnectToApi;

namespace Projetinho {
    class CadastraPedidos {
        public int quantidade = 0;
        public List<Dados> dados = new List<Dados>();
        public CadastraPedidos(string path) {
            string result = ExtrairTexto(path);
            var r = result.Split("####################################################################################################################################\n");
            var cl = new Regex("Cliente:");
            var sub = new Regex("Folha.*");
            var sub1 = new Regex("SIGA.*");
            var sub2 = new Regex("Hora.*");
            var sub3 = new Regex(".*Placa.*");
            var sub4 = new Regex("\\n\\n\\n");
            var sub5 = new Regex(@"Pedido: (.*)");
            var orEntrega = new Regex(@"Ordem Entrega:\s*([\d]{3})");
            foreach (var i in r) {
                var sl = cl.Matches(i);
                if (sl.Count > 0) {
                    var sla = i.Split("Cliente:");
                    var pedido = sub5.Match(sla[0]).Groups[1].ToString().TrimEnd().TrimStart();
                    var limpo = sub.Replace(sla[sla.Length - 1], string.Empty);
                    var limpo1 = sub1.Replace(limpo, string.Empty);
                    var limpo2 = sub2.Replace(limpo1, string.Empty);
                    var limpo3 = sub3.Replace(limpo2, string.Empty);
                    var orEnt = orEntrega.Match(sla[0]);
                    var limpo4 = $"Pedido: {pedido}\nOrdem Entrega: {orEnt.Groups[1].Value}\n" + "Cliente:" + sub4.Replace(limpo3, string.Empty);

                    dados.Add(new Dados {
                        pedido = pedido,
                        dado = limpo4
                    });
                }
            }
        }
        public string ExtrairTexto(string path) {
            string result = "";
            PdfReader pdfReader = new PdfReader(path);
            PdfDocument pdfDocument = new PdfDocument(pdfReader);
            for (int page = 1; page <= pdfDocument.GetNumberOfPages(); page++) {
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                string content = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(page), strategy);
                result += content;
            }
            pdfDocument.Close();
            pdfReader.Close();
            return result;
        }
    }
}
