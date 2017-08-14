using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace MousePhone
{
    [Activity(Label = "MousePhone", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            /*
             - princip sličan PopcornTimeRemoteu -> trebat će PC aplikacija
             - prazan zaslon, na početku te odma pita da li želiš uspostaviti konekciju
             - utipkaš IP koji ti komp pokaže, miš počne radit
             - cijeli zaslon je prazan, klik na lijevu stranu zaslon je LMB, i suprotno je RMB
             - gore/dolje/lijevo/desno = pomicanje kursora
             - gumbić na dnu/vrhu koji omogućava korištenje funkcije kotačića sa pomakom prsta gore/dolje
           */
        }
    }
}

