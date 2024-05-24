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
using IBEXBle.DependencyInterface;
using Xamarin.Forms;

[assembly: Dependency(typeof(IBEXBle.Droid.Utils.VibratorUtil))]
namespace IBEXBle.Droid.Utils
{
    public class VibratorUtil : IVibrator
    {
        public bool Vibrate(long milliSeconds)
        {
            try
            {
                var vibrator = (Vibrator) MainActivity.Instance.GetSystemService(Context.VibratorService);
                long[] vibrationPatern = new long[] { 100, 170 };
                vibrator.Vibrate(milliSeconds);
            }
            catch 
            {
                return false;
            }
            return true;
        }
    }
}