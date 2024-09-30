using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using System.Text;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;

namespace BankaDovizKuruServer
{
    public partial class Form1 : Form
    {
        private DataGridView dataGridIsBankasi;
        private DataGridView dataGridDeniz;
        private DataGridView dataGridHalkBank;
        private DataGridView dataGridYapiKredi;
        private DataGridView dataGridAkbank;
        private DataGridView dataGridQnbFinans;
        private DataGridView dataGridAllBanks;

        public static int SendPort = 15099;
        public static string SendIPAddress = "10.1.1.54";
        public static int GetTimerInterval;

        private Timer SendTimer;
        
        private List<FxRateModel> fxRateIsBankasi;
        private List<FxRateModel> fxRateDenizbank;
        private List<FxRateModel> fxRateYapiKredi;
        private List<FxRateModel> fxRateAkbank;
        private List<FxRateModel> fxRateQnbFinansBank;

        private bool isBankEnabled = true;
        private bool denizBankEnabled = true;
        private bool yapiKrediEnabled = true;
        private bool akbankEnabled = true;
        private bool qnbFinanasEnabled = true;

        FrmSettings frmSettings = new FrmSettings( SendPort, GetTimerInterval);
        private Timer dataFetchTimer;
        private List<FxRateModel> previousRates = new List<FxRateModel>();
        private List<FxRateModel> allRates = new List<FxRateModel>();

        public static TcpSender tcpSender;

        public Form1()
        {
            InitializeComponent();
            InitializeDataGrids();
            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;
        }

        private void InitializeDataGrids()
        {

            dataGridIsBankasi = CreateDataGridView();
            tabPage2.Controls.Add(dataGridIsBankasi);  

            dataGridDeniz = CreateDataGridView();
            tabPage3.Controls.Add(dataGridDeniz);  

            dataGridYapiKredi = CreateDataGridView();
            tabPage4.Controls.Add(dataGridYapiKredi);

            dataGridAkbank = CreateDataGridView();
            tabPage5.Controls.Add(dataGridAkbank);

            dataGridQnbFinans = CreateDataGridView();
            tabPage6.Controls.Add(dataGridQnbFinans);


            dataGridAllBanks = CreateDataGridView(true);
            tabPage1.Controls.Add(dataGridAllBanks);  
        }

        private DataGridView CreateDataGridView(bool includeCheckBoxColumn = false)
        {
            try
            {
                var dataGridView = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
                };

                if (includeCheckBoxColumn)
                {
                    DataGridViewCheckBoxColumn sendColumn = new DataGridViewCheckBoxColumn();
                    sendColumn.HeaderText = "G�nder";
                    sendColumn.Name = "SendColumn";
                    sendColumn.TrueValue = true;
                    sendColumn.FalseValue = false;
                    sendColumn.ValueType = typeof(bool);
                    sendColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    sendColumn.Width = 60;
                    dataGridView.Columns.Add(sendColumn); 
                }

               // dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "IsActive", DataPropertyName ="IsActive" });
                dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Kod", Name = "Kod", DataPropertyName = "Code" });
                dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tan�m", DataPropertyName = "Description" });
                dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Al��", DataPropertyName = "Buy" });
                dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Sat��", DataPropertyName = "Sell" });
                dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Banka Ad�" , DataPropertyName = "BankName" });
                dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Son G�ncelleme" });


                return dataGridView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DataGridView olu�turulamad�" + ex.Message);
                return null;
            }
        }

        private Dictionary<string, bool> checkboxStates = new Dictionary<string, bool>();

        private Task SaveCheckboxStates(DataGridView dataGridView)
        {
            return Task.Run(() =>
            {
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    
                    if (row.Cells["Kod"]?.Value != null)
                    {
                        string code = row.Cells["Kod"].Value.ToString();

                    
                        //if (row.Cells["SendColumn"] != null && row.Cells["SendColumn"].Value != null)
                        if (row.Cells["SendColumn"]?.Value != null)
                        {
                            bool isChecked = Convert.ToBoolean(row.Cells["SendColumn"].Value);

                            if (checkboxStates.ContainsKey(code))
                                checkboxStates[code] = isChecked;
                            else
                                checkboxStates.Add(code, isChecked);
                        }
                    }
                }
            });
        }

        private Task RestoreCheckboxStates(DataGridView dataGridView)
        {
            return Task.Run(() =>
            {
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells["Kod"]?.Value != null)
                    {
                        string code = row.Cells["Kod"].Value.ToString();
                        if (checkboxStates.ContainsKey(code))
                        {
                            row.Cells["SendColumn"].Value = checkboxStates[code];
                        }
                    }
                }
            });
        }


        private void Form1_Load(object sender, EventArgs e)
        {
           
            LoadSettingsFromJson();
            UpdateTimers();
           // StartTcpListener();

            string isBankasiUrl = "https://www.isbank.com.tr/_vti_bin/DV.Isbank/PriceAndRate/PriceAndRateService.svc/GetFxRates?Lang=tr&fxRateType=IB&date=2024-8-29&time=1724936436985";
            string denizbankUrl = "https://www.denizbank.com/api/marketdata/exchanges";
            string yapiKrediUrl = "https://www.yapikredi.com.tr/_ajaxproxy/general.aspx/LoadMainCurrencies";
            string akbankUrl = "https://www.akbank.com/_vti_bin/AkbankServicesSecure/FrontEndServiceSecure.svc/GetCurrencyRates";
            string qnbFinansBankUrl = "https://www.qnbfinansbank.com/api/LoanCalculators/ExchangeParite?exChangeDate=2024-08-29&channelCode=148";

            FetchBankData(isBankasiUrl, denizbankUrl, yapiKrediUrl, akbankUrl, qnbFinansBankUrl);

            tcpSender = new TcpSender(SendIPAddress, SendPort);
            if (!tcpSender.Connect())
            {
                MessageBox.Show("TCP ba�lant�s� kurulamad�!");
            }
            //UpdateConnectionStatus();

        }

        private void FetchBankData(string isBankasiUrl, string denizbankUrl, string yapiKrediUrl, string akbankUrl, string qnbFinansBankUrl)
        {

            try
            {
                allRates.Clear();

                // �� Bankas� verilerini �ek
                if (isBankEnabled)
                {
                    var isBankasiRates = GetIsBankasiRates(isBankasiUrl);
                    if (isBankasiRates != null)
                    {
                        fxRateIsBankasi = ConvertIsBankasiRatesToFxRateModel(isBankasiRates.Data);
                        Logger.Log($"�� Bankas� verisi: {fxRateIsBankasi.Count} adet veri.");
                        richTextBoxAciklama.AppendText($"{DateTime.Now}: �� Bankas� verisi: {fxRateIsBankasi.Count} adet veri.\n");
                        ApplyCustomCodes(fxRateIsBankasi, "ISBNK");
                        DisplayFxRatesInGrid(fxRateIsBankasi, dataGridIsBankasi);
                    }
                }
                // DenizBank verilerini �ek
                if (denizBankEnabled)
                {
                    var denizBankRates = GetDenizBankRates(denizbankUrl);
                    fxRateDenizbank = denizBankRates != null ? ConvertDenizBankRatesToFxRateModel(denizBankRates) : new List<FxRateModel>();
                    Logger.Log($"DenizBank verisi: {fxRateDenizbank.Count} adet veri.");
                    richTextBoxAciklama.AppendText($"{DateTime.Now}: DenizBank verisi: {fxRateDenizbank.Count} adet veri.\n");
                    ApplyCustomCodes(fxRateDenizbank, "DNZ");
                    DisplayFxRatesInGrid(fxRateDenizbank, dataGridDeniz);
                }

                // Yap� Kredi verilerini �ek
                if (yapiKrediEnabled)
                {
                    var yapiKrediRates = GetYapiKrediRates(yapiKrediUrl);
                    if (yapiKrediRates?.d != null)
                    {
                        fxRateYapiKredi = ConvertYapiKrediRatesToFxRateModel(yapiKrediRates.d);
                        Logger.Log($"Yap� Kredi verisi: {fxRateYapiKredi.Count} adet veri.");
                        richTextBoxAciklama.AppendText($"{DateTime.Now}: YapiKredi verisi: {fxRateYapiKredi.Count} adet veri.\n");
                        ApplyCustomCodes(fxRateYapiKredi, "YKB");
                        DisplayFxRatesInGrid(fxRateYapiKredi, dataGridYapiKredi);
                    }
                    else
                    {
                        Logger.Log("Yap� Kredi verisi  null d�nd�.");
                        richTextBoxAciklama.AppendText($"{DateTime.Now}: Yap� Kredi verisi null d�nd�. \n");
                    }
                }

                // Akbank verilerini �ek
                if (akbankEnabled)
                {
                    var akbankRates = GetAkbankRates(akbankUrl);
                    if (akbankRates?.cur != null)
                    {
                        fxRateAkbank = ConvertAkbankRatesToFxRateModel(akbankRates.cur);
                        Logger.Log($"Akbank verisi: {fxRateAkbank.Count} adet veri.");
                        richTextBoxAciklama.AppendText($"{DateTime.Now}: Akbank verisi: {fxRateAkbank.Count} adet veri.\n");
                        ApplyCustomCodes(fxRateAkbank, "AKB");
                        DisplayFxRatesInGrid(fxRateAkbank, dataGridAkbank);
                    }
                    else
                    {
                        Logger.Log("Akbank verisi null d�nd�.");
                        richTextBoxAciklama.AppendText($"{DateTime.Now}: Akbank verisi null d�nd�. \n");
                    }
                }

                // QNB Finansbank verilerini �ek
                if (qnbFinanasEnabled)
                {
                    var qnbFinansBankRates = GetQnbFinansBankRates(qnbFinansBankUrl);
                    if (qnbFinansBankRates != null)
                    {
                        fxRateQnbFinansBank = ConvertQnbFinansRatesToFxRateModel(qnbFinansBankRates);
                        Logger.Log($"QNB Finansbank verisi: {fxRateQnbFinansBank.Count} adet veri.");
                        richTextBoxAciklama.AppendText($"{DateTime.Now}: QnbFinansBank verisi: {fxRateQnbFinansBank.Count} adet veri.\n");
                        ApplyCustomCodes(fxRateQnbFinansBank, "QNB");
                        DisplayFxRatesInGrid(fxRateQnbFinansBank, dataGridQnbFinans);
                    }
                    else
                    {
                        Logger.Log("QNB Finansbank verisi bo� veya null d�nd�.");
                        richTextBoxAciklama.AppendText($"{DateTime.Now}: QNB FinansBank verisi null d�nd�.\n");
                    }
                }

                // T�m bankalardan gelen verileri tek bir listeye ekle
                //var allRates = new List<FxRateModel>();

                if (fxRateIsBankasi != null) allRates.AddRange(fxRateIsBankasi);
                if (fxRateDenizbank != null) allRates.AddRange(fxRateDenizbank);
                if (fxRateYapiKredi != null) allRates.AddRange(fxRateYapiKredi);
                if (fxRateAkbank != null) allRates.AddRange(fxRateAkbank);
                if (fxRateQnbFinansBank != null) allRates.AddRange(fxRateQnbFinansBank);

                Logger.Log($"Toplam {allRates.Count} adet veri t�m bankalardan topland�.");
                richTextBoxAciklama.AppendText($"{DateTime.Now} : {allRates.Count} adet veri t�m bankalardan topland�.\n");
                DisplayFxRatesInGrid(allRates, dataGridAllBanks);
            }
            catch (Exception ex)
            {
                Logger.Log($"Veri �ekme i�lemi s�ras�nda hata: {ex.Message}");
                richTextBoxAciklama.AppendText($"{DateTime.Now}: Veri �ekme i�lemi s�ras�nda bir hata olu�tu.\n");
            }

        }

        // �� Bankas� verilerini genel modele d�n��t�rme
        private List<FxRateModel> ConvertIsBankasiRatesToFxRateModel(List<IsBankasiKurModel> rates)
        {
            try
            {
                return rates.Select(rate => new FxRateModel
                {
                    Code = rate.Code,
                    Description = rate.Description,
                    Buy = rate.FxRateBuy,
                    Sell = rate.FxRateSell
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("��Bankas� Verileri D�n��t�r�lemedi" + ex.Message);
                throw;
            }
        }

        // Denizbank verilerini genel modele d�n��t�rme
        private List<FxRateModel> ConvertDenizBankRatesToFxRateModel(List<DenizBankKurModel> rates)
        {
            try
            {
                return rates.Select(rate => new FxRateModel
                {
                    Code = rate.Code, 
                    Description = rate.Name,  
                    Buy = rate.BuyingPrice,
                    Sell = rate.SellingPrice
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("DenizBank Verileri D�n��t�r�lemedi" + ex.Message);
                return null;
            }
        }

        private List<FxRateModel> ConvertYapiKrediRatesToFxRateModel(List<YapiKrediKurModel> rates)
        {
            try
            {
                return rates.Select(rate => new FxRateModel
                {
                    Code = rate.code,
                    Description = "", // Yap� Kredi a��klama bilgisi sa�lam�yor
                    Buy = rate.buy,
                    Sell = rate.sell
                }).ToList();
            }
            catch (Exception ex)
            {
                Logger.Log($"Yap� Kredi verileri convert edilirken hata olu�tu: {ex.Message}");
                return null;
            }

        }

        Dictionary<string, string> CurrencyDescriptions = new Dictionary<string, string>
            {
                { "AED", "Birle�ik Arap Emirlikleri Dirhemi" },
                { "AUD", "Avustralya Dolar�" },
                { "CAD", "Kanada Dolar�" },
                { "CHF", "�svi�re Frang�" },
                { "CNY", "�in Yuan�" },
                { "DKK", "Danimarka Kronu" },
                { "EUR", "Euro" },
                { "GBP", "�ngiliz Sterlini" },
                { "JPY", "Japon Yeni" },
                { "KWD", "Kuveyt Dinar�" },
                { "NOK", "Norve� Kronu" },
                { "PLN", "Polonya Zlotisi" },
                { "RON", "Romanya Leyi" },
                { "RUB", "Rus Rublesi" },
                { "SAR", "Suudi Arabistan Riyali" },
                { "SEK", "�sve� Kronu" },
                { "USD", "Amerikan Dolar�" },
                { "XAG", "G�m�� (ons)" },
                { "XAU", "Alt�n (ons)" },
                { "XPT", "Platin (ons)" },
                { "ZAR", "G�ney Afrika Rand�" }
            };

        Dictionary<string, string> bankNameMapping = new Dictionary<string, string>
            {
                { "ISBNK", "ISBANK" },
                { "DNZ", "DENIZBANK" },
                { "QNB", "QNB FINANSBANK" },
                { "AKB", "AKBANK" }
            };

        private List<FxRateModel> ConvertAkbankRatesToFxRateModel(List<AkbankKurModel> rates)
        {
            return rates.Select(rate => new FxRateModel
            {
                Code = rate.Title,
                Description = CurrencyDescriptions.ContainsKey(rate.Title) ? CurrencyDescriptions[rate.Title] : "A��klama Bulunamad�",
                //Description = "bilinmiyor",
                Buy = rate.DovizAlis,
                Sell = rate.DovizSatis

            }).ToList();
        }

        private List<FxRateModel> ConvertQnbFinansRatesToFxRateModel(List<QnbFinansBankKurModel> rates)
        {
            return rates.Select(rate => new FxRateModel
            {
                Code = rate.CurrencyTypeField,
                Description = rate.CurrencyNameField,
                Buy = rate.BuyingExchangeRateField,
                Sell = rate.SellingExchangeRateField
            }).ToList();
        }

        public string ConvertFxRatesToJson(List<FxRateModel> allRates)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(allRates, Formatting.Indented);
                Logger.Log("Veriler JSON format�na d�n��t�r�ld�.");
                return jsonData;
            }
            catch (Exception ex)
            {
                Logger.Log($"Veriler JSON format�na d�n��t�r�l�rken hata olu�tu: {ex.Message}");
                return null;
            }
        }

        // D�viz kurlar�na �zel kodlar atama
        private void ApplyCustomCodes(List<FxRateModel> fxRates, string prefix)
        {

            try
            {
                foreach (var rate in fxRates)
                {
                    if (rate.Code == "USD") rate.Code = $"{prefix}USD";
                    else if (rate.Code == "EUR") rate.Code = $"{prefix}EUR";
                    else if (rate.Code == "GBP") rate.Code = $"{prefix}GBP";
                    else if (rate.Code == "XAU") rate.Code = $"{prefix}XAU";
                    else if (rate.Code == "AUD") rate.Code = $"{prefix}AUD";
                    else if (rate.Code == "DKK") rate.Code = $"{prefix}DKK";
                    else if (rate.Code == "SEK") rate.Code = $"{prefix}SEK";
                    else if (rate.Code == "CHF") rate.Code = $"{prefix}CHF";
                    else if (rate.Code == "JPY") rate.Code = $"{prefix}JPY";
                    else if (rate.Code == "CAD") rate.Code = $"{prefix}CAD";
                    else if (rate.Code == "KWD") rate.Code = $"{prefix}KWD";
                    else if (rate.Code == "NOK") rate.Code = $"{prefix}NOK";
                    else if (rate.Code == "SAR") rate.Code = $"{prefix}SAR";
                    else if (rate.Code == "RUB") rate.Code = $"{prefix}RUB";
                    else if (rate.Code == "ZAR") rate.Code = $"{prefix}ZAR";
                    else if (rate.Code == "XPD") rate.Code = $"{prefix}XPD";
                    else if (rate.Code == "BHD") rate.Code = $"{prefix}BHD";
                    else if (rate.Code == "IDR") rate.Code = $"{prefix}IDR";
                    else if (rate.Code == "MXN") rate.Code = $"{prefix}MXN";
                    else if (rate.Code == "KZT") rate.Code = $"{prefix}KZT";
                    else if (rate.Code == "CNH") rate.Code = $"{prefix}CNH";
                    else if (rate.Code == "QAR") rate.Code = $"{prefix}QAR";
                    else if (rate.Code == "AED") rate.Code = $"{prefix}AED";
                    else if (rate.Code == "NPR") rate.Code = $"{prefix}NPR";
                    else if (rate.Code == "RON") rate.Code = $"{prefix}RON";
                    else if (rate.Code == "BRL") rate.Code = $"{prefix}BRL";
                    else if (rate.Code == "CZK") rate.Code = $"{prefix}CZK";
                    else if (rate.Code == "HUF") rate.Code = $"{prefix}HUF";
                    else if (rate.Code == "NZD") rate.Code = $"{prefix}NZD";
                    else if (rate.Code == "PLN") rate.Code = $"{prefix}PLN";
                    else if (rate.Code == "THB") rate.Code = $"{prefix}THB";
                    else if (rate.Code == "XAG") rate.Code = $"{prefix}XAG";
                    else if (rate.Code == "INR") rate.Code = $"{prefix}INR";
                    else if (rate.Code == "XPT") rate.Code = $"{prefix}XPT";
                    else if (rate.Code == "CNY") rate.Code = $"{prefix}CNY";
                    else if (rate.Code == "HKD") rate.Code = $"{prefix}HKD";
                    else if (rate.Code == "SGD") rate.Code = $"{prefix}SGD";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kode bulunamad�" + ex.Message);

            }
        }

        // Verileri ilgili grid'de g�sterme
        public void DisplayFxRatesInGrid(List<FxRateModel> fxRates, DataGridView dataGridView)
        {
            if (dataGridView.InvokeRequired)
            {
                dataGridView.Invoke(new Action(() => DisplayFxRatesInGrid(fxRates, dataGridView)));
                return;
            }

            try
            {
                Logger.Log($"DisplayFxRatesInGrid: {fxRates.Count} adet veri g�sterilecek.");
                dataGridView.Rows.Clear();

                if (fxRates == null || !fxRates.Any())
                {
                    Logger.Log("Veri yok, DataGridView'e eklenemedi.");
                    return;
                }

                dataGridView.Rows.Clear();
                foreach (var rate in fxRates)
                {
                    string bankPrefix = FindBankPrefix(rate.Code);  // Prefix'i dinamik olarak ��kar
                    string bankName = bankNameMapping.ContainsKey(bankPrefix) ? bankNameMapping[bankPrefix] : "UNKNOWN";
                    if (dataGridView.Columns.Contains("SendColumn"))
                    {
                        dataGridView.Rows.Add(true, rate.Code, rate.Description, rate.Buy, rate.Sell, bankName, DateTime.Now.ToString("HH:mm:ss"));
                    }
                    else
                    {
                        dataGridView.Rows.Add(true, rate.Code, rate.Description, rate.Buy, rate.Sell, bankName, DateTime.Now.ToString("HH:mm:ss"));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Veriler tabloya eklenemedi: " + ex.Message);
            }
        }

        private string FindBankPrefix(string code)
        {
            // Prefix genellikle ilk 3 veya 4 karakter olabilir, buna g�re kontrol et
            if (code.StartsWith("ISBNK"))
                return "ISBNK";
            else if (code.StartsWith("DNZ"))
                return "DNZ";
            else if (code.StartsWith("QNB"))
                return "QNB";
            else if (code.StartsWith("AKB"))
                return "AKB";

            return "UNKNOWN";
        }

        // �� Bankas� API verilerini �ekme
        public IsBankasiResponse GetIsBankasiRates(string url)
        {
            try
            {
                //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                Logger.Log("�� Bankas� verilerini �ekme i�lemi ba�lat�ld�.");
                using (WebClient client = new WebClient())
                {
                    string jsonResponse = client.DownloadString(url);
                    Logger.Log("�� Bankas� verileri ba�ar�yla �ekildi.");
                    var fxRates = JsonConvert.DeserializeObject<IsBankasiResponse>(jsonResponse);
                    Logger.Log("�� Bankas� verileri ba�ar�yla parse edildi.");
                    return fxRates;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"�� Bankas� verileri al�n�rken hata olu�tu: {ex.Message}");
                MessageBox.Show($"�� Bankas� verileri al�n�rken hata olu�tu: {ex.Message}");
                return null;
            }
        }

        // Denizbank API verilerini �ekme
        public List<DenizBankKurModel> GetDenizBankRates(string url)
        {
            try
            {
                Logger.Log("DenizBank verilerini �ekme i�lemi ba�lat�ld�.");
                using (WebClient client = new WebClient())
                {
                    string jsonResponse = client.DownloadString(url);
                    Logger.Log($"DenizBank API cevab�: {jsonResponse}");
                    var fxRates = JsonConvert.DeserializeObject<List<DenizBankKurModel>>(jsonResponse);
                    Logger.Log("DenizBank verileri ba�ar�yla parse edildi.");
                    return fxRates;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"DenizBank verileri al�n�rken hata olu�tu: {ex.Message}");
                // MessageBox.Show($"Denizbank verileri al�n�rken hata olu�tu: {ex.Message}");
                return null;
            }
        }

        // Yap� Kredi verilerini web client ile �ekme
        public YapiKrediResponse GetYapiKrediRates(string url)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    Logger.Log("Yap� Kredi Bankas� verilerini �ekme i�lemi ba�lat�ld�.");
                    var jsonResponse = client.DownloadString(url);
                    Logger.Log("Yap� Kredi verileri ba�ar�yla �ekildi.");

                    var yapiKrediRates = JsonConvert.DeserializeObject<YapiKrediResponse>(jsonResponse);

                    if (yapiKrediRates?.d == null || !yapiKrediRates.d.Any())
                    {
                        Logger.Log("Yap� Kredi'den al�nan veri bo�.");
                        return null;
                    }

                    Logger.Log("Yap� Kredi verileri ba�ar�yla parse edildi.");
                    return yapiKrediRates;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Yap� Kredi verileri al�n�rken hata olu�tu: {ex.Message}");
                return null;
            }
        }

        // Akbank verilerini web client ile �ekme
        public AkbankResult GetAkbankRates(string url)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    Logger.Log("Akbank d�viz verilerini �ekme i�lemi ba�lat�ld�.");
                    var jsonResponse = client.DownloadString(url);

                    Logger.Log("Akbank verileri ba�ar� ile �ekildi.");
                    var akbankResponse = JsonConvert.DeserializeObject<AkbankResponse>(jsonResponse);
                    var akbankResult = JsonConvert.DeserializeObject<AkbankResult>(akbankResponse.GetCurrencyRatesResult);

                    Logger.Log("Akbank verileri ba�ar� ile parse edildi.");
                    return akbankResult;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Akbank verileri al�n�rken hata olu�tu: {ex.Message}");
                return null;
            }
        }

        // QNB Finansbank verilerini web client ile �ekme
        public List<QnbFinansBankKurModel> GetQnbFinansBankRates(string url)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    Logger.Log("QNB Finansbank verilerini �ekme i�lemi ba�lat�ld�.");
                    var jsonResponse = client.DownloadString(url);

                    Logger.Log("QNB Finansbank verileri ba�ar� ile �ekildi.");
                    var qnbFinansBankResponse = JsonConvert.DeserializeObject<QnbFinansBankResponse>(jsonResponse);

                    return qnbFinansBankResponse?.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"QNB Finansbank verileri al�n�rken hata olu�tu: {ex.Message}");
                return null;
            }
        }

        private void btnAyarlar_Click(object sender, EventArgs e)
        {

            using (FrmSettings frmSettings = new FrmSettings(SendPort, GetTimerInterval))
            {
                frmSettings.textBxSendPort.Text = SendPort.ToString();
                frmSettings.numericUpDownGetReqTimer.Value = GetTimerInterval;

                frmSettings.UpdateConnectionStatus(tcpSender != null && tcpSender.IsConnected());
                if (frmSettings.ShowDialog() == DialogResult.OK)
                {
                    // Kullan�c� ayarlar� kaydetti�inde bu de�erleri g�nceller
                    SendIPAddress = frmSettings.txtbxSenderIp.Text;
                    SendPort = int.Parse(frmSettings.textBxSendPort.Text);
                    GetTimerInterval = (int)frmSettings.numericUpDownGetReqTimer.Value;

                    isBankEnabled = frmSettings.IsBankSelected;
                    denizBankEnabled = frmSettings.DenizBankSelected;
                    yapiKrediEnabled = frmSettings.YapiKrediSelected;
                    akbankEnabled = frmSettings.AkbankSelected;
                    qnbFinanasEnabled = frmSettings.QnbFinansSelected;

                    UpdateTimers(); 
                    MessageBox.Show("Ayarlar ba�ar�yla g�ncellendi!");
                }
            }
        }

        private void UpdateTimers()
        {
            try
            {
                if (dataFetchTimer != null)
                {
                    dataFetchTimer.Stop();
                    dataFetchTimer.Dispose();
                }

                dataFetchTimer = new Timer();
                dataFetchTimer.Interval = GetTimerInterval * 60 * 1000;  // Dakikay� milisaniyeye �evrmek i�in
                dataFetchTimer.Tick += DataFetchTimer_Tick;
                dataFetchTimer.Start();
            }
            catch (Exception ex) { Logger.Log($"UpdateTimers metodunda hata var : {ex.Message}"); }
        }

        private async void DataFetchTimer_Tick(object sender, EventArgs e)
        {
            richTextBoxAciklama.AppendText($"{DateTime.Now}: Veriler otomatik olarak g�ncelleniyor...\n");

            await SaveCheckboxStates(dataGridAllBanks);  
            await FetchAndCompareDataAsync();  
            await RestoreCheckboxStates(dataGridAllBanks);  

            richTextBoxAciklama.AppendText($"{DateTime.Now}: Veriler ba�ar�yla g�ncellendi.\n");

        }

        private async Task FetchAndCompareDataAsync()
        {
            try
            {
               
                var newRates = new List<FxRateModel>();

                if (isBankEnabled && fxRateIsBankasi != null)
                {
                    newRates.AddRange(fxRateIsBankasi);
                }

                if (denizBankEnabled && fxRateDenizbank != null)
                {
                    newRates.AddRange(fxRateDenizbank);
                }

                if (yapiKrediEnabled && fxRateYapiKredi != null)
                {
                    newRates.AddRange(fxRateYapiKredi);
                }

                if (akbankEnabled && fxRateAkbank != null)
                {
                    newRates.AddRange(fxRateAkbank);
                }

                if (qnbFinanasEnabled && fxRateQnbFinansBank != null)
                {
                    newRates.AddRange(fxRateQnbFinansBank);
                }

                
                var updatedRates = CompareRates(previousRates, newRates);

                if (updatedRates.Any())
                {
                    await SendUpdatedRatesAsync(updatedRates); 
                }

                // Son �ekilen verileri sakla
                previousRates = newRates;

                DisplayFxRatesInGrid(newRates, dataGridAllBanks);
            }
            catch (Exception ex)
            {
                Logger.Log($"FetchAndCompareData metodunda hata var : {ex.Message}");
            }
        }

        private async Task SendUpdatedRatesAsync(List<FxRateModel> updatedRates)
        {
            try
            {
                int gonderilenVeriSayisi = 0;
                foreach (DataGridViewRow row in dataGridAllBanks.Rows)
                {
                    bool isSendChecked = Convert.ToBoolean(row.Cells["SendColumn"].Value);
                    if (isSendChecked)
                    {
                        FxRateModel rate = row.DataBoundItem as FxRateModel;
                        if (rate != null && updatedRates.Contains(rate))
                        {
                            string data = FormatData(rate);
                            await tcpSender.SendDataAsync(data); 
                            gonderilenVeriSayisi++;
                        }
                    }
                }
                richTextBoxAciklama.AppendText($"{DateTime.Now}: {gonderilenVeriSayisi} adet veri de�eri de�i�ti�i i�in g�ncellendi.G�ncellenmi� d�viz kuru TCP �zerinden ba�ar�yla g�nderildi.\n");
            }
            catch (Exception ex)
            {
                Logger.Log($"SendUpdatedRates metodunda hata var : {ex.Message}");
                richTextBoxAciklama.AppendText($"{DateTime.Now}: TCP �zerinden g�ncellenmi� veri g�nderimi s�ras�nda hata olu�tu.\n");
            }
        }


        private List<FxRateModel> CompareRates(List<FxRateModel> oldRates, List<FxRateModel> newRates)
        {
            var updatedRates = new List<FxRateModel>();

            foreach (var newRate in newRates)
            {
                var oldRate = oldRates.FirstOrDefault(r => r.Code == newRate.Code);
                if (oldRate == null || oldRate.Buy != newRate.Buy || oldRate.Sell != newRate.Sell)
                {
                    updatedRates.Add(newRate);
                }
            }

            return updatedRates;
        }

        private void FilterFxRatesInGrid(DataGridView dataGridView, string searchTerm)
        {
            try
            {
                
                dataGridView.Rows.Clear();

                if (allRates == null || !allRates.Any())
                {
                    MessageBox.Show("Veri bulunamad�.");
                    return;
                }

               
                var filteredRates = allRates
                    .Where(rate => rate.Code != null && rate.Description != null)
                    .Where(rate => rate.Code.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                   rate.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                
                if (!filteredRates.Any())
                {
                    MessageBox.Show("Arama teriminize uygun sonu� bulunamad�.");
                    return;
                }

                
                foreach (var rate in filteredRates)
                {
                    string bankPrefix = FindBankPrefix(rate.Code);
                    string bankName = bankNameMapping.ContainsKey(bankPrefix) ? bankNameMapping[bankPrefix] : "UNKNOWN";

                    dataGridView.Rows.Add(true, rate.Code, rate.Description, rate.Buy, rate.Sell, bankName, DateTime.Now.ToString("HH:mm:ss"));
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Verileri filtrelerken hata olu�tu: " + ex.Message);
            }
        }

        private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchTerm = txtSearch.Text.Trim().ToUpper();

                await SaveCheckboxStates(dataGridAllBanks);

                if (string.IsNullOrEmpty(searchTerm))
                {
                    DisplayFxRatesInGrid(allRates, dataGridAllBanks);
                }
                else
                {
                    FilterFxRatesInGrid(dataGridAllBanks, searchTerm);
                }

                await RestoreCheckboxStates(dataGridAllBanks);

                e.SuppressKeyPress = true; 
            }
        }

        public void LoadSettingsFromJson()
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    var settings = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                    isBankEnabled = Convert.ToBoolean(settings["IsBankEnabled"]);
                    denizBankEnabled = Convert.ToBoolean(settings["DenizBankEnabled"]);
                    yapiKrediEnabled = Convert.ToBoolean(settings["YapiKrediEnabled"]);
                    akbankEnabled = Convert.ToBoolean(settings["AkbankEnabled"]);
                    qnbFinanasEnabled = Convert.ToBoolean(settings["QnbFinansEnabled"]);


                    SendPort = Convert.ToInt32(settings["SendPort"]);
                    GetTimerInterval = Convert.ToInt32(settings["GetTimerInterval"]);

                    //MessageBox.Show("Ayarlar ba�ar�yla y�klendi.");
                }
                else
                {
                    // E�er ayar dosyas� yoksa bu ayarlar� kullan
                    isBankEnabled = true;
                    denizBankEnabled = true;
                    yapiKrediEnabled = true;
                    akbankEnabled = true;
                    qnbFinanasEnabled = true;
                    SendPort = 3545;
                    GetTimerInterval = 5;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"LoadSettingsFromJson metodunda hata var : {ex.Message}");
            }
        }

        private async void btnSendAllData_Click(object sender, EventArgs e)
        {
            try
            {
                List<FxRateModel> allRates = new List<FxRateModel>();

                if (fxRateIsBankasi != null) allRates.AddRange(fxRateIsBankasi);
                if (fxRateDenizbank != null) allRates.AddRange(fxRateDenizbank);
                if (fxRateYapiKredi != null) allRates.AddRange(fxRateYapiKredi);
                if (fxRateAkbank != null) allRates.AddRange(fxRateAkbank);
                if (fxRateQnbFinansBank != null) allRates.AddRange(fxRateQnbFinansBank);

                await SendAllRatesAsync(allRates);
            }
            catch (Exception ex)
            {
                Logger.Log($"btnSendAllData_Click metodunda hata var : {ex.Message}");
            }
        }

        private async Task SendAllRatesAsync(List<FxRateModel> allRates)
        {
            try
            {
                int gonderilenVeriSayisi = 0;
                foreach (DataGridViewRow row in dataGridAllBanks.Rows)
                {
                    bool isSelected = Convert.ToBoolean(row.Cells["SendColumn"].Value);
                    if (!isSelected) continue;  

                    string code = row.Cells["Kod"].Value.ToString();
                    FxRateModel rate = allRates.FirstOrDefault(r => r.Code == code);
                    if (rate != null)
                    {
                        string data = FormatData(rate);
                        await tcpSender.SendDataAsync(data);  
                        gonderilenVeriSayisi++;
                    }
                }
                richTextBoxAciklama.AppendText($"{DateTime.Now}: T�m verileri g�ndermek istediniz, {gonderilenVeriSayisi} d�viz kuru TCP �zerinden ba�ar�yla g�nderildi.\n");
            }
            catch (Exception ex)
            {
                Logger.Log($"SendAllRates metodunda hata var: {ex.Message}");
                richTextBoxAciklama.AppendText($"{DateTime.Now}: TCP �zerinden veri g�nderimi s�ras�nda hata olu�tu.\n");
            }
        }

        private string FormatData(FxRateModel rate)
        {
            string data = "?66001;" + rate.Code + ";C" + rate.Buy + ";B" + rate.Buy + ";A" + rate.Sell + ";" + (Char)3;
            data += (Char)8 + "j" + (Char)8 + "w" + (Char)8;
            return data;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // tcpSender.Close();

        }

    }
}
