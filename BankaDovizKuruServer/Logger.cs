using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankaDovizKuruServer
{
    public static class Logger
    {
        private static string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static string archiveDirectory = Path.Combine(logDirectory, "Arsiv");

        public static void Log(string message) 
        {
			try
			{
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
                if (!Directory.Exists(archiveDirectory)) 
                {
                    Directory.CreateDirectory(archiveDirectory);
                }

                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                string logFileName = $"{currentDate}_BankaDovizKuru_Log.txt";
                string logFilePath = Path.Combine(logDirectory, logFileName);

                GecmisLoguArsiveTasi(currentDate);

                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    sw.WriteLine($"{DateTime.Now}: {message}");
                }
            }
			catch (Exception ex)
			{
                MessageBox.Show($"Log yazarken hata oluştu: {ex.Message}");
				throw;
			}
        }

        private static void GecmisLoguArsiveTasi(string currentDate)
        {
            //Klasördeki tüm dosyaları kontrol et
            foreach (string file in Directory.GetFiles(logDirectory, "*_BankaDovizKuru_Log.txt"))
            {
                string fileName = Path.GetFileName(file);

                //Dosya bugünün dosyası değilse taşı
                if (!fileName.StartsWith(currentDate))
                { 
                    string archiveFilePath = Path.Combine(archiveDirectory, fileName);
                    if (File.Exists(archiveFilePath)) 
                    {
                        File.Move(file, archiveFilePath);
                    }
                }
            }
        }
    }
}
