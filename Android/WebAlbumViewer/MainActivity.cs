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
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            /*
             - mogućnost gledanja slika i videa sa nekog mobitela putem HTTP servera na mobitelu koji sharea
             - modificat HTTP server i aplikaciju da sama nađe taj mobitel koji sharea kada se spoje na istu mrežu
             - mogućnost swipeanja lijevo/desno, kao najnormalniji album
             - od opcija: samo "Save to my phone" za slike, videa ništa
            */
        }
    }
}

