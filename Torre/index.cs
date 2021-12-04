using Nacional.Torre.ConnectToApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nacional.Torre {
    public partial class index : UserControl {
        Requests requests;
        string caminhoAudio = @"2567_mobile-rington..wav";
        public index() {
            InitializeComponent();
        }

        private void index_Load(object sender, EventArgs e) {
            timer1.Interval = 10;
            timer1.Start();
        }
        private async void AtualizaTable() {
            requests = new Requests();
            timer1.Interval = 1000;
            List<string> vs = new List<string>();
            bool firsttime = true;
            while (true) {
                try {
                    await requests.GetQuadroAviso();
                    bool equals = requests.QuadroAviso.Intersect(vs).Count() == requests.QuadroAviso.Union(vs).Count();
                    if (!equals || requests.QuadroAviso.Count != vs.Count) {
                        if (requests.QuadroAviso.Count > vs.Count && !firsttime) {
                            SoundPlayer sound = new SoundPlayer(caminhoAudio);
                            sound.Play();
                        }
                        flpIndex.Invoke(new Action(() => {
                            flpIndex.Controls.Clear();
                        }));
                        for (var o = requests.QuadroAviso.Count - 1; o > 0; o -= 2) {
                            flpIndex.Invoke(new Action(() => {
                                flpIndex.Controls.Add(GerarTextBox(requests.QuadroAviso[o - 1], int.Parse(requests.QuadroAviso[o])));
                            }));
                        }
                        vs.Clear();
                        vs.AddRange(requests.QuadroAviso);
                        firsttime = false;
                    }
                    if (requests.QuadroAviso.Count == 0) firsttime = false;
                    Thread.Sleep(2000);
                }
                catch (Exception) {
                    Thread.Sleep(2000);
                }
            }
        }
        private TableLayoutPanel GerarTextBox(string texto, int importancia) {
            var quantidade = texto.Length;
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.ColumnCount = 1;
            panel.RowCount = 1;
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));

            if (importancia == 1) {
                panel.BackColor = Color.YellowGreen;
            }
            else if (importancia == 3) {
                panel.BackColor = Color.Salmon;
            }
            else {
                panel.BackColor = Color.Yellow;
            }
            TextBox textBox = new TextBox();
            textBox.ScrollBars = ScrollBars.None;
            textBox.Anchor = ((AnchorStyles)((AnchorStyles.Left | AnchorStyles.Right)));
            textBox.Multiline = true;
            textBox.BorderStyle = BorderStyle.None;
            textBox.CharacterCasing = CharacterCasing.Upper;
            textBox.BackColor = panel.BackColor;
            textBox.Enabled = false;
            textBox.Text = texto;
            textBox.Font = new Font("Bookman Old Style", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            textBox.ReadOnly = true;
            Size tamanho = TextRenderer.MeasureText(textBox.Text, textBox.Font);
            textBox.Height = tamanho.Width/tamanho.Height + 17;
            textBox.TextAlign = HorizontalAlignment.Center;
            panel.Controls.Add(textBox);
            return panel;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            AtualizaTable();
        }

        private void timer1_Tick(object sender, EventArgs e) {
            if (!backgroundWorker1.IsBusy) {
                backgroundWorker1.RunWorkerAsync();
            }
        }
    }
}
