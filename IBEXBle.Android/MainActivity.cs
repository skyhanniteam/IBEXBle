using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.OS;

namespace IBEXBle.Droid
{
    [Activity(Label = "IBEX-BT", Icon = "@drawable/watonIcon", Theme = "@style/MainTheme", MainLauncher =false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            Instance = this;
            global::Xamarin.Forms.Forms.Init(this, bundle);
            RequestedOrientation = ScreenOrientation.Portrait;

            LoadApplication(new App());
        }
    }
}

