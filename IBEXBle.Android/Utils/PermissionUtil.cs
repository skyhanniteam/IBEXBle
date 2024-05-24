using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using IBEXBle.DependencyInterface;
using Xamarin.Forms;

[assembly: Dependency(typeof(IBEXBle.Droid.Utils.PermissionUtil))]
namespace IBEXBle.Droid.Utils
{
    public class PermissionUtil : ICheckPermisstion
    {
        public void CheckLocationPermission()
        {
            if ((int)Build.VERSION.SdkInt < 23)
                return;

            if (ContextCompat.CheckSelfPermission(MainActivity.Instance, Manifest.Permission.AccessFineLocation) != (int)Android.Content.PM.Permission.Granted)
                ActivityCompat.RequestPermissions(MainActivity.Instance, new string[] { Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation }, 101);
                        
        }
    }
}