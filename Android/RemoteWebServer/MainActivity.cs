using Android.App;
using Android.Widget;
using Android.OS;

namespace RemoteWebServer
{
    [Activity(Label = "RemoteWebServer", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static MainActivityHandler handler;
        public int scroll;
        public Button Start;
        public TextView Help;
        public TextView Output;
        public HttpServer server;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            handler = new MainActivityHandler(this);
            Window.SetFlags(Android.Views.WindowManagerFlags.KeepScreenOn, Android.Views.WindowManagerFlags.KeepScreenOn);

            scroll = 0;
            Start = FindViewById<Button>(Resource.Id.StartServer);
            Help = FindViewById<TextView>(Resource.Id.Help);
            Output = FindViewById<TextView>(Resource.Id.Output);
            Help.MovementMethod = new Android.Text.Method.ScrollingMovementMethod();
            Output.MovementMethod = new Android.Text.Method.ScrollingMovementMethod();

            Help.Text += $"To make this application work properly, you must:\n";
            Help.Text += $"1. Be connected to your WiFi network\n";
            Help.Text += $"2. Port forward '8080' port for IP address '{Utilities.GetLocalIP()}'\n";
            Help.Text += $"3. Put website files in 'Download/Website' directory\n";
            Help.Text += $"4. Implement HTML/CSS images with external links\n\n";
            Help.Text += $"If any of the mentioned steps are not done correctly, application will either start and not work properly or give an error!\n\n";
            Help.Text += $"Recommended free DNS provider: http://www.noip.com/";

            Start.Click += delegate 
            {
                try
                {
                    if(Start.Text.Contains("START"))
                    {
                        AddText($"While opened, this application prevents your phone from locking/sleeping due to occasional network interruptions which are indeed caused by phone's sleep/lock mode!\n");
                        server = new HttpServer();
                        server.Start();
                        AddText($"Local URL: {Utilities.GetLocalIP()}:8080/index.html");
                        AddText($"Public URL: {Utilities.GetPublicIP()}:8080/index.html\n");
                        Start.Text = "STOP MY WEB SERVER";
                    }
                    else if(Start.Text.Contains("STOP"))
                    {
                        server.Stop();
                        Start.Text = "START MY WEB SERVER";
                        Output.Text = "";
                        scroll = 0;
                    }
                }
                catch
                {
                    ShowAlert("Something went wrong!", "ERROR");
                    Start.Text = "START MY WEB SERVER";
                    Output.Text = "";
                    scroll = 0;
                }
            };
        }

        public void AddText(string text)
        {
            Output.Append(text + "\n");
        }

        public void ShowAlert(string message, string title)
        {
            new AlertDialog.Builder(this).SetMessage(message).SetTitle(title).Show();
        }
    }
}

