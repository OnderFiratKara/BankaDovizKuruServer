namespace BankaDovizKuruServer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            tabPage4 = new TabPage();
            tabPage5 = new TabPage();
            tabPage6 = new TabPage();
            btnAyarlar = new Button();
            txtSearch = new TextBox();
            richTextBoxAciklama = new RichTextBox();
            label1 = new Label();
            btnSendAllData = new Button();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage6);
            tabControl1.Location = new Point(19, 41);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(689, 453);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(681, 425);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Tüm Veriler";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(681, 425);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "İş Bankası";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(681, 425);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "DenizBank";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            tabPage4.Location = new Point(4, 24);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(681, 425);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "YapiKredi";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            tabPage5.Location = new Point(4, 24);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(681, 425);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Akbank";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            tabPage6.Location = new Point(4, 24);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(3);
            tabPage6.Size = new Size(681, 425);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "QNB FinansBank";
            tabPage6.UseVisualStyleBackColor = true;
            // 
            // btnAyarlar
            // 
            btnAyarlar.Location = new Point(602, 11);
            btnAyarlar.Name = "btnAyarlar";
            btnAyarlar.Size = new Size(102, 23);
            btnAyarlar.TabIndex = 1;
            btnAyarlar.Text = "Ayarlara Git";
            btnAyarlar.UseVisualStyleBackColor = true;
            btnAyarlar.Click += btnAyarlar_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(19, 12);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Aramak istediğiniz değeri giriniz...";
            txtSearch.Size = new Size(136, 23);
            txtSearch.TabIndex = 2;
            txtSearch.KeyDown += txtSearch_KeyDown;
            // 
            // richTextBoxAciklama
            // 
            richTextBoxAciklama.Location = new Point(23, 525);
            richTextBoxAciklama.Name = "richTextBoxAciklama";
            richTextBoxAciklama.Size = new Size(681, 243);
            richTextBoxAciklama.TabIndex = 3;
            richTextBoxAciklama.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11.25F, FontStyle.Italic, GraphicsUnit.Point, 162);
            label1.Location = new Point(23, 502);
            label1.Name = "label1";
            label1.Size = new Size(80, 20);
            label1.TabIndex = 4;
            label1.Text = "Açıklama : ";
            // 
            // btnSendAllData
            // 
            btnSendAllData.Location = new Point(435, 11);
            btnSendAllData.Name = "btnSendAllData";
            btnSendAllData.Size = new Size(140, 23);
            btnSendAllData.TabIndex = 5;
            btnSendAllData.Text = "Tüm Verileri Gönder";
            btnSendAllData.UseVisualStyleBackColor = true;
            btnSendAllData.Click += btnSendAllData_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(756, 780);
            Controls.Add(btnSendAllData);
            Controls.Add(label1);
            Controls.Add(richTextBoxAciklama);
            Controls.Add(txtSearch);
            Controls.Add(btnAyarlar);
            Controls.Add(tabControl1);
            Name = "Form1";
            Text = "Banka Döviz Kurları";
            Load += Form1_Load;
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
       
        private Button btnAyarlar;
        private TextBox txtSearch;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;
        public RichTextBox richTextBoxAciklama;
        private Label label1;
        private Button btnSendAllData;
    }
}
