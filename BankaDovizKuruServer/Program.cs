using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace BankaDovizKuruServer
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

      
    }
}
