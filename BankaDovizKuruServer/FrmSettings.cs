using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankaDovizKuruServer
{
    public partial class FrmSettings : Form
    {
        public bool IsBankSelected { get; private set; }
        public bool DenizBankSelected { get; private set; }
        public bool YapiKrediSelected { get; private set; }
        public bool AkbankSelected { get; private set; }
        public bool QnbFinansSelected { get; private set; }


        public FrmSettings( int sendPort, int getTimerInterval)
        {
            InitializeComponent();
            //textBxSendAdress.Text = Form1.SendAddress;
            textBxSendPort.Text = Form1.SendPort.ToString();
            numericUpDownGetReqTimer.Value = Form1.GetTimerInterval;
            
            LoadSettingsFromJson();
        }

        private void FrmSettings_Load(object sender, EventArgs e)
        {
            LoadSettingsFromJson();
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            SaveSettingsToJson();

            Form1.SendPort = int.Parse(textBxSendPort.Text);
            Form1.GetTimerInterval = (int)numericUpDownGetReqTimer.Value;

            IsBankSelected = checkBoxIsBank.Checked;
            DenizBankSelected = checkBoxDenizBank.Checked;
            YapiKrediSelected = checkBoxYapiKredi.Checked;
            AkbankSelected = checkBoxAkbank.Checked;
            QnbFinansSelected = checkBoxQnbFinansBank.Checked;

            MessageBox.Show("Ayarlar başarıyla kaydedildi!", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //SaveSettingsToJson();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public void SaveSettingsToJson()
        {
            var settings = new Dictionary<string, object>
            {
                { "IsBankEnabled", checkBoxIsBank.Checked },
                { "DenizBankEnabled", checkBoxDenizBank.Checked },
                { "YapiKrediEnabled", checkBoxYapiKredi.Checked },
                { "AkbankEnabled", checkBoxAkbank.Checked },
                { "QnbFinansEnabled", checkBoxQnbFinansBank.Checked },
                { "SendPort", int.Parse(textBxSendPort.Text) },
                { "GetTimerInterval", (int)numericUpDownGetReqTimer.Value }

            };

            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

            File.WriteAllText(filePath, json);

            MessageBox.Show($"Ayarlar kaydedildi: {json}");
        }

        public void LoadSettingsFromJson()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var settings = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                checkBoxIsBank.Checked = Convert.ToBoolean(settings["IsBankEnabled"]);
                checkBoxDenizBank.Checked = Convert.ToBoolean(settings["DenizBankEnabled"]);
                checkBoxYapiKredi.Checked = Convert.ToBoolean(settings["YapiKrediEnabled"]);
                checkBoxAkbank.Checked = Convert.ToBoolean(settings["AkbankEnabled"]);
                checkBoxQnbFinansBank.Checked = Convert.ToBoolean(settings["QnbFinansEnabled"]);
                textBxSendPort.Text = settings["SendPort"].ToString();
                numericUpDownGetReqTimer.Value = Convert.ToDecimal(settings["GetTimerInterval"]);
                
                //MessageBox.Show($"IsBank: {settings["IsBankEnabled"]}, DenizBank: {settings["DenizBankEnabled"]}, YapiKredi: {settings["YapiKrediEnabled"]}, Akbank: {settings["AkbankEnabled"]}, QNB: {settings["QnbFinansEnabled"]}");
            }
            else
            {
                //dosya bulunamazsa veya başka bir hata olursa kullan
                checkBoxIsBank.Checked = true;
                checkBoxDenizBank.Checked = true;
                checkBoxYapiKredi.Checked = true;
                checkBoxAkbank.Checked = true;
                checkBoxQnbFinansBank.Checked = true;
                textBxSendPort.Text = "3545";
                numericUpDownGetReqTimer.Value = 5;
            }
        }

    }
}
