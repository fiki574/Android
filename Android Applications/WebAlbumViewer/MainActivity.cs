﻿using Android.App;
using Android.Webkit;
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
        public WebView Browser;
        public Button Previous;
        public string URL;

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
                HostLog.Text += "\nPeople who are currently on the same network as you use this IP address when connecting through the client interface:\n" + Utilities.GetLocalIP() + "\n\nIf you want to stop the server or change the application mode, please close this application and start it again!\n";
                HostLog.Text += "\nYou can also access your Album files from PC by typing this in your URL bar:\nhttp://public_or_local_IP:8080/album.view\n\n";
            };

            Client.Click += delegate
            {
                SetContentView(Resource.Layout.Client);
                IPAddress = FindViewById<EditText>(Resource.Id.IPAddress);
                Connect = FindViewById<Button>(Resource.Id.Connect);

                Connect.Click += delegate
                {
                    URL = "http://" + IPAddress.Text + ":8080";
                    string album = URL + "/album.view";
                    SetContentView(Resource.Layout.Album);
                    Browser = FindViewById<WebView>(Resource.Id.Browser);
                    Browser.Settings.JavaScriptEnabled = true;
                    Browser.Settings.BuiltInZoomControls = true;
                    Browser.Settings.SetSupportZoom(true);
                    Browser.Settings.DisplayZoomControls = true;
                    Browser.SetWebViewClient(new WebViewClient());
                    Browser.LoadUrl(album);
                };
            };
        }
    }
}

