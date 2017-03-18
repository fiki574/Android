using Android.App;
using Android.Widget;
using Android.OS;
using System.Net;
using System.Net.Sockets;

namespace RemoteWebServer
{
    [Activity(Label = "RemoteWebServer", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static MainActivityHandler handler;
        public Button Start;
        public TextView Output;
        public HttpServer server;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            handler = new MainActivityHandler(this);

            Start = FindViewById<Button>(Resource.Id.StartServer);
            Output = FindViewById<TextView>(Resource.Id.Output);
            Output.MovementMethod = new Android.Text.Method.ScrollingMovementMethod();

            Start.Click += delegate 
            {
                try
                {
                    if(Start.Text.Contains("START"))
                    {
                        AddText($"If you're unable to reach the website by trying and using both local and/or public URL, you either haven't port forwarded properly or you're trying to access it from the mobile network (this app works on WiFi strictly)!\n");
                        AddText($"Due to some Android limitations, HTML's 'img' must have 'src' property set to external URL, meaning your images must be hosted somewhere!\n");
                        AddText($"The website is loaded from internal storage's 'Download/Website' folder, so make sure you put all your files there!\n");
                        AddText($"Recommended free DNS provider: http://www.noip.com/\n");
                        server = new HttpServer();
                        server.Start();
                        AddText($"Local URL: {GetLocalIP()}:8080/index.html");
                        AddText($"Public URL: {GetPublicIP()}:8080/index.html\n");
                        Start.Text = "STOP MY WEB SERVER";
                    }
                    else if(Start.Text.Contains("STOP"))
                    {
                        server.Stop();
                        Start.Text = "START MY WEB SERVER";
                        Output.Text = "";
                    }
                }
                catch
                {
                    ShowAlert("Something went wrong!", "ERROR");
                    Start.Text = "START MY WEB SERVER";
                    Output.Text = "";
                }
            };
        }

        public void AddText(string text)
        {
            Output.Text += text + "\n";
        }

        public void ShowAlert(string message, string title)
        {
            new AlertDialog.Builder(this).SetMessage(message).SetTitle(title).Show();
        }

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

        public static string FormatErrorHtml(string message)
        {
            return $"<!DOCTYPE HTML><HTML><HEAD></HEAD><BODY><p style=\"color: red;\">{ message }</p></BODY></HTML>";
        }
    }
}

