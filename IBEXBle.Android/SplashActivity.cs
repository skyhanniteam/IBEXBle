using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace IBEXBle.Droid
{
    [Activity(Label = "IBEX-BT", Icon = "@drawable/watonIcon", Theme = "@style/splashscreen", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(MainActivity));
        }
    }
}