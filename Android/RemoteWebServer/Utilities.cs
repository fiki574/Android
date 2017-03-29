using System.Net;
using System.Net.Sockets;

namespace RemoteWebServer
{
    public class Utilities
    {
        public static string GetLocalIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            return "127.0.0.1";
        }

        public static string GetPublicIP()
        {
            try
            {
                return (new WebClient()).DownloadString("http://bot.whatismyipaddress.com/");
            }
            catch
            {
                return "127.0.0.1";
            }
        }
    }
}