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
using IBEXBle.Core;
using IBEXBle.DependencyInterface;
using Xamarin.Forms;

[assembly: Dependency(typeof(IBEXBle.Droid.Utils.ToastUtil))]
namespace IBEXBle.Droid.Utils
{
    public class ToastUtil : IToast
    {
        public void MakeText(string text, Definitions.ToastLength length)
        {
            Toast.MakeText(MainActivity.Instance, text, length == Definitions.ToastLength.LongLength ? ToastLength.Long : ToastLength.Short).Show();
        }
    }
}