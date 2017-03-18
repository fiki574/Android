using System;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;

namespace PopcornTimeRemoteServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Listener listener = new Listener(GetLocalIPAddress(), 7188);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            return "127.0.0.1";
        }

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public static void SendKeystroke(ushort k)
        {
            SendMessage(FindWindow(null, "Popcorn Time"), 0x100, ((IntPtr)k), (IntPtr)0);
        }
    }
}
