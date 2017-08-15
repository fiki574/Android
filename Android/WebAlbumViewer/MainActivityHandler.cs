namespace WebAlbumViewer
{
    public class MainActivityHandler
    {
        private MainActivity main;

        public MainActivityHandler(MainActivity main)
        {
            this.main = main;
        }

        public void AddText(string text)
        {
            main.RunOnUiThread(() => { main.HostLog.Text += text + '\n'; });
        }
    }
}