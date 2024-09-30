namespace BankaDovizKuruServer
{
    partial class FrmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBxSendPort = new TextBox();
            lblSendPort = new Label();
            btnSaveSettings = new Button();
            label2 = new Label();
            numericUpDownGetReqTimer = new NumericUpDown();
            checkBoxIsBank = new CheckBox();
            checkBoxDenizBank = new CheckBox();
            checkBoxYapiKredi = new CheckBox();
            checkBoxAkbank = new CheckBox();
            checkBoxQnbFinansBank = new CheckBox();
            checkBoxZiraat = new CheckBox();
            checkBoxVakif = new CheckBox();
            checkBoxHalkBank = new CheckBox();
            txtbxSenderIp = new TextBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)numericUpDownGetReqTimer).BeginInit();
            SuspendLayout();
            // 
            // textBxSendPort
            // 
            textBxSendPort.Location = new Point(141, 65);
            textBxSendPort.Name = "textBxSendPort";
            textBxSendPort.Size = new Size(85, 23);
            textBxSendPort.TabIndex = 0;
            // 
            // lblSendPort
            // 
            lblSendPort.AutoSize = true;
            lblSendPort.Location = new Point(15, 68);
            lblSendPort.Name = "lblSendPort";
            lblSendPort.Size = new Size(98, 15);
            lblSendPort.TabIndex = 1;
            lblSendPort.Text = "Gönderim Portu :";
            // 
            // btnSaveSettings
            // 
            btnSaveSettings.Location = new Point(14, 187);
            btnSaveSettings.Name = "btnSaveSettings";
            btnSaveSettings.Size = new Size(83, 40);
            btnSaveSettings.TabIndex = 3;
            btnSaveSettings.Text = "KAYDET";
            btnSaveSettings.UseVisualStyleBackColor = true;
            btnSaveSettings.Click += btnSaveSettings_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 114);
            label2.Name = "label2";
            label2.Size = new Size(127, 15);
            label2.TabIndex = 1;
            label2.Text = "Servis İstek Timer (dk) :";
            // 
            // numericUpDownGetReqTimer
            // 
            numericUpDownGetReqTimer.Location = new Point(141, 112);
            numericUpDownGetReqTimer.Name = "numericUpDownGetReqTimer";
            numericUpDownGetReqTimer.Size = new Size(85, 23);
            numericUpDownGetReqTimer.TabIndex = 2;
            // 
            // checkBoxIsBank
            // 
            checkBoxIsBank.AutoSize = true;
            checkBoxIsBank.Location = new Point(359, 24);
            checkBoxIsBank.Name = "checkBoxIsBank";
            checkBoxIsBank.Size = new Size(167, 19);
            checkBoxIsBank.TabIndex = 4;
            checkBoxIsBank.Text = "İş Bankası Verilerini Gönder";
            checkBoxIsBank.UseVisualStyleBackColor = true;
            // 
            // checkBoxDenizBank
            // 
            checkBoxDenizBank.AutoSize = true;
            checkBoxDenizBank.Location = new Point(359, 49);
            checkBoxDenizBank.Name = "checkBoxDenizBank";
            checkBoxDenizBank.Size = new Size(171, 19);
            checkBoxDenizBank.TabIndex = 4;
            checkBoxDenizBank.Text = "DenizBank Verilerini Gönder";
            checkBoxDenizBank.UseVisualStyleBackColor = true;
            // 
            // checkBoxYapiKredi
            // 
            checkBoxYapiKredi.AutoSize = true;
            checkBoxYapiKredi.Location = new Point(359, 74);
            checkBoxYapiKredi.Name = "checkBoxYapiKredi";
            checkBoxYapiKredi.Size = new Size(168, 19);
            checkBoxYapiKredi.TabIndex = 4;
            checkBoxYapiKredi.Text = "Yapı Kredi Verilerini Gönder";
            checkBoxYapiKredi.UseVisualStyleBackColor = true;
            // 
            // checkBoxAkbank
            // 
            checkBoxAkbank.AutoSize = true;
            checkBoxAkbank.Location = new Point(359, 99);
            checkBoxAkbank.Name = "checkBoxAkbank";
            checkBoxAkbank.Size = new Size(156, 19);
            checkBoxAkbank.TabIndex = 4;
            checkBoxAkbank.Text = "Akbank Verilerini Gönder";
            checkBoxAkbank.UseVisualStyleBackColor = true;
            // 
            // checkBoxQnbFinansBank
            // 
            checkBoxQnbFinansBank.AutoSize = true;
            checkBoxQnbFinansBank.Location = new Point(359, 124);
            checkBoxQnbFinansBank.Name = "checkBoxQnbFinansBank";
            checkBoxQnbFinansBank.Size = new Size(202, 19);
            checkBoxQnbFinansBank.TabIndex = 4;
            checkBoxQnbFinansBank.Text = "QnbFinans Bank Verilerini Gönder";
            checkBoxQnbFinansBank.UseVisualStyleBackColor = true;
            // 
            // checkBoxZiraat
            // 
            checkBoxZiraat.AutoSize = true;
            checkBoxZiraat.Location = new Point(359, 149);
            checkBoxZiraat.Name = "checkBoxZiraat";
            checkBoxZiraat.Size = new Size(189, 19);
            checkBoxZiraat.TabIndex = 4;
            checkBoxZiraat.Text = "Ziraat Bankası Verilerini Gönder";
            checkBoxZiraat.UseVisualStyleBackColor = true;
            // 
            // checkBoxVakif
            // 
            checkBoxVakif.AutoSize = true;
            checkBoxVakif.Location = new Point(359, 174);
            checkBoxVakif.Name = "checkBoxVakif";
            checkBoxVakif.Size = new Size(167, 19);
            checkBoxVakif.TabIndex = 4;
            checkBoxVakif.Text = "Vakıfbank Verilerini Gönder";
            checkBoxVakif.UseVisualStyleBackColor = true;
            // 
            // checkBoxHalkBank
            // 
            checkBoxHalkBank.AutoSize = true;
            checkBoxHalkBank.Location = new Point(359, 199);
            checkBoxHalkBank.Name = "checkBoxHalkBank";
            checkBoxHalkBank.Size = new Size(166, 19);
            checkBoxHalkBank.TabIndex = 4;
            checkBoxHalkBank.Text = "Halkbank Verilerini Gönder";
            checkBoxHalkBank.UseVisualStyleBackColor = true;
            // 
            // txtbxSenderIp
            // 
            txtbxSenderIp.Location = new Point(141, 25);
            txtbxSenderIp.Name = "txtbxSenderIp";
            txtbxSenderIp.Size = new Size(85, 23);
            txtbxSenderIp.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 28);
            label1.Name = "label1";
            label1.Size = new Size(79, 15);
            label1.TabIndex = 1;
            label1.Text = "Gönderim IP :";
            // 
            // FrmSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(582, 332);
            Controls.Add(checkBoxHalkBank);
            Controls.Add(checkBoxAkbank);
            Controls.Add(checkBoxVakif);
            Controls.Add(checkBoxYapiKredi);
            Controls.Add(checkBoxZiraat);
            Controls.Add(checkBoxDenizBank);
            Controls.Add(checkBoxQnbFinansBank);
            Controls.Add(checkBoxIsBank);
            Controls.Add(btnSaveSettings);
            Controls.Add(numericUpDownGetReqTimer);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtbxSenderIp);
            Controls.Add(lblSendPort);
            Controls.Add(textBxSendPort);
            Name = "FrmSettings";
            Text = "FrmSettings";
            ((System.ComponentModel.ISupportInitialize)numericUpDownGetReqTimer).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        public TextBox textBxSendPort;
        public Label lblSendPort;
        public Button btnSaveSettings;
        public Label label2;
        public NumericUpDown numericUpDownGetReqTimer;
        private CheckBox checkBoxIsBank;
        private CheckBox checkBoxDenizBank;
        private CheckBox checkBoxYapiKredi;
        private CheckBox checkBoxAkbank;
        private CheckBox checkBoxQnbFinansBank;
        private CheckBox checkBoxZiraat;
        private CheckBox checkBoxVakif;
        private CheckBox checkBoxHalkBank;
        public TextBox txtbxSenderIp;
        public Label label1;
    }
}