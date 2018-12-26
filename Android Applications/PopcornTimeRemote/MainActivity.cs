using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;

namespace PopcornTimeRemote
{
    [Activity(Label = "PopcornTimeRemote", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static MainActivityHandler handler;
        public Client client;
        public EditText IP;
        public Button Connect;
        public Button Pause;
        public Button Screen;
        public TextView IPInfo;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            handler = new MainActivityHandler(this);

            IP = FindViewById<EditText>(Resource.Id.IP);
            Connect = FindViewById<Button>(Resource.Id.Connect);
            Pause = FindViewById<Button>(Resource.Id.Pause);
            Screen = FindViewById<Button>(Resource.Id.Screen);
            IPInfo = FindViewById<TextView>(Resource.Id.textView1);

            Components(false);

            Connect.Click += delegate
            {
                if (!string.IsNullOrEmpty(IP.Text))
                {
                    if (IsDigitsOnly(IP.Text, true))
                    {
                        try
                        {
                            client = new Client(IP.Text, 7188);
                            client.Send(new Command("connect"));
                        }
                        catch
                        {
                            ShowAlert("Server unreachable!", "ERROR");
                        }
                    }
                    else
                        ShowAlert("Wrong input!", "WARNING");
                }
                else
                    ShowAlert("Wrong input!", "WARNING");
            };

            Pause.Click += delegate
            {
                client.Send(new Command("pause"));
            };

            Screen.Click += delegate
            {
                client.Send(new Command("screen"));
            };
        }

        public void ShowAlert(string message, string title)
        {
            new AlertDialog.Builder(this).SetMessage(message).SetTitle(title).Show();
        }

        public void Components(bool show)
        {
            if (show)
            {
                IPInfo.Visibility = ViewStates.Invisible;
                IP.Visibility = ViewStates.Invisible;
                Connect.Visibility = ViewStates.Invisible;
                Pause.Visibility = ViewStates.Visible;
                Screen.Visibility = ViewStates.Visible;
            }
            else
            {
                IPInfo.Visibility = ViewStates.Visible;
                IP.Visibility = ViewStates.Visible;
                Connect.Visibility = ViewStates.Visible;
                Pause.Visibility = ViewStates.Invisible;
                Screen.Visibility = ViewStates.Invisible;
            }
        }

        public bool IsDigitsOnly(string str, bool allow_dot = false)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                {
                    if (!allow_dot) return false;
                    else if (allow_dot) if (c != '.') return false;
                }
            }
            return true;
        }
    }
}

