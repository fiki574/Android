using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace WebAlbumViewer
{
    [Activity(Label = "WebAlbumViewer", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static MainActivityHandler handler;
        public Button Host;
        public Button Client;
        public TextView HostLog;
        public HttpServer server;
        public EditText IPAddress;
        public Button Connect;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            handler = new MainActivityHandler(this);
            Host = FindViewById<Button>(Resource.Id.HostButton);
            Client = FindViewById<Button>(Resource.Id.ClientButton);
            Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);

            Host.Click += delegate
            {
                SetContentView(Resource.Layout.Host);
                HostLog = FindViewById<TextView>(Resource.Id.LogText);
                HostLog.MovementMethod = new Android.Text.Method.ScrollingMovementMethod();
                server = new HttpServer();
                server.Start();
                HostLog.Text += "\nPeople who are currently either not close to you or not connected to the same network as you use this IP address when connecting through client interface:\n" + Utilities.GetPublicIP() + "\n";
                HostLog.Text += "\nPeople who are currently on the same network as you use this IP address when connecting through the client interface:\n" + Utilities.GetLocalIP() + "\n\nIf you want to stop the server or change the application mode, please close this application and start it again!\n\n";
            };

            Client.Click += delegate
            {
                SetContentView(Resource.Layout.Client);
                IPAddress = FindViewById<EditText>(Resource.Id.IPAddress);
                Connect = FindViewById<Button>(Resource.Id.Connect);

                Connect.Click += delegate
                {
                    //TODO: Album layout and requests logic
                };
            };
        }

        public void ShowAlert(string message, string title)
        {
            new AlertDialog.Builder(this).SetMessage(message).SetTitle(title).Show();
        }
    }
}

