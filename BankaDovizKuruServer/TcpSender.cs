using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BankaDovizKuruServer
{
    public class TcpSender
    {
        private readonly string ipAddress;
        private readonly int port;
        private TcpClient client;
        private NetworkStream stream;

        public TcpSender(string ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
        }

        public bool Connect()
        {
            try
            {
                client = new TcpClient(ipAddress, port);
                stream = client.GetStream();
                Logger.Log($"Bağlantı {ipAddress}:{port} adresine başarıyla kuruldu.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log($"TCP bağlantısı kurulurken hata oluştu: {ex.Message}");
                return false;
            }
        }

        public async Task SendDataAsync(string data)
        {
            try
            {
                if (client == null || !client.Connected)
                {

                    Logger.Log("TCP bağlantısı kapalı.");
                    return;
                }

                byte[] byteData = Encoding.UTF8.GetBytes(data);
                await stream.WriteAsync(byteData, 0, byteData.Length); // Asenkron gönderim
                Logger.Log($"Veri başarıyla gönderildi: {data}");
            }
            catch (Exception ex)
            {
                Logger.Log($"Veri gönderimi sırasında hata oluştu: {ex.Message}");
            }
        }

        //public void Close()
        //{
        //    try
        //    {
        //        stream?.Close();
        //        client?.Close();
        //        Logger.Log("TCP bağlantısı kapatıldı.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log($"Bağlantı kapatılırken hata oluştu: {ex.Message}");
        //    }
        //}

        public void Disconnect()
        {
            try
            {
                if (stream != null)
                {
                    stream.Close();
                }
                if (client != null)
                {
                    client.Close();
                    client.Dispose();
                }
                Logger.Log("TCP bağlantısı başarıyla kapatıldı.");
            }
            catch (Exception ex)
            {
                Logger.Log($"TCP bağlantısı kapatılırken hata oluştu: {ex.Message}");
            }
        }

        public bool IsConnected()
        {
            try
            {
                return client != null && client.Connected;
            }
            catch
            {
                return false;
            }
        }

    }

    

}
