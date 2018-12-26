namespace PopcornTimeRemote
{
    public class MainActivityHandler
    {
        private MainActivity main;

        public MainActivityHandler(MainActivity main)
        {
            this.main = main;
        }

        public void Components(bool show)
        {
            main.RunOnUiThread(() => { main.Components(show); });
        }

        public void ShowAlert(string message, string title)
        {
            main.RunOnUiThread(() => { main.ShowAlert(message, title); });
        }
    }
}