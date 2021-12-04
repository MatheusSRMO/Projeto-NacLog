
namespace Projetinho {
    partial class Form1 {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.retornos1 = new Nacional.Torre.Retornos();
            this.index1 = new Nacional.Torre.index();
            this.userControl31 = new Projetinho.UserControl3();
            this.userControl121 = new Projetinho.UserControl12();
            this.uC_AnexarXml2 = new Projetinho.UC_AnexarXml();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(20);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.retornos1);
            this.splitContainer1.Panel2.Controls.Add(this.index1);
            this.splitContainer1.Panel2.Controls.Add(this.userControl31);
            this.splitContainer1.Panel2.Controls.Add(this.userControl121);
            this.splitContainer1.Panel2.Controls.Add(this.uC_AnexarXml2);
            this.splitContainer1.Size = new System.Drawing.Size(1104, 561);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(20, 94);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(226, 447);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.button2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.button3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.button4, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.MinimumSize = new System.Drawing.Size(226, 336);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(226, 447);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button1.Location = new System.Drawing.Point(0, 84);
            this.button1.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.button1.MaximumSize = new System.Drawing.Size(226, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(226, 36);
            this.button1.TabIndex = 0;
            this.button1.Text = "Anexar Arquivos";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button2.Location = new System.Drawing.Point(0, 158);
            this.button2.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.button2.MaximumSize = new System.Drawing.Size(226, 36);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(226, 36);
            this.button2.TabIndex = 1;
            this.button2.Text = "Montar Carga";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button3.Location = new System.Drawing.Point(0, 232);
            this.button3.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.button3.MaximumSize = new System.Drawing.Size(226, 36);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(226, 36);
            this.button3.TabIndex = 2;
            this.button3.Text = "Emitir Relatório";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button4.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button4.Location = new System.Drawing.Point(0, 306);
            this.button4.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.button4.MaximumSize = new System.Drawing.Size(226, 36);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(226, 36);
            this.button4.TabIndex = 3;
            this.button4.Text = "Retornos";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(20, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(226, 74);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // retornos1
            // 
            this.retornos1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.retornos1.Location = new System.Drawing.Point(0, 0);
            this.retornos1.Name = "retornos1";
            this.retornos1.Size = new System.Drawing.Size(834, 561);
            this.retornos1.TabIndex = 4;
            // 
            // index1
            // 
            this.index1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.index1.Location = new System.Drawing.Point(0, 0);
            this.index1.Name = "index1";
            this.index1.Size = new System.Drawing.Size(834, 561);
            this.index1.TabIndex = 1;
            // 
            // userControl31
            // 
            this.userControl31.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControl31.Location = new System.Drawing.Point(0, 0);
            this.userControl31.Name = "userControl31";
            this.userControl31.Padding = new System.Windows.Forms.Padding(15);
            this.userControl31.Size = new System.Drawing.Size(834, 561);
            this.userControl31.TabIndex = 1;
            // 
            // userControl121
            // 
            this.userControl121.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControl121.Location = new System.Drawing.Point(0, 0);
            this.userControl121.Name = "userControl121";
            this.userControl121.Size = new System.Drawing.Size(834, 561);
            this.userControl121.TabIndex = 3;
            // 
            // uC_AnexarXml2
            // 
            this.uC_AnexarXml2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_AnexarXml2.Location = new System.Drawing.Point(0, 0);
            this.uC_AnexarXml2.Name = "uC_AnexarXml2";
            this.uC_AnexarXml2.Size = new System.Drawing.Size(834, 561);
            this.uC_AnexarXml2.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 561);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1120, 600);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NacLog";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private UC_AnexarXml uC_AnexarXml2;
        private UserControl12 userControl121;
        private UserControl3 userControl31;
        private Nacional.Torre.index index1;
        private Nacional.Torre.Retornos retornos1;
    }
}

