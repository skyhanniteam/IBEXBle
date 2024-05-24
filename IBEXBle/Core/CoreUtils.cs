using IBEXBle.DependencyInterface;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using Xamarin.Forms;

namespace IBEXBle.Core
{
    public class CoreUtils
    {
        public static ResourceManager ResourceManager
        {
            get
            {
                ResourceManager resourceManager = null;

                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        resourceManager = Droid.AppResource.ResourceManager;
                        break;
                    default:
                        break;
                }
                return resourceManager;
            }

        }

        public static void Toast(string text, Definitions.ToastLength toastLength)
        {
            var toast = DependencyService.Get<IToast>();
            if (toast != null)
                toast.MakeText(text, toastLength);
        }

        public static void Vibrate(long milliSeconds)
        {
            var vibrator = DependencyService.Get<IVibrator>();
            if (vibrator != null)
                vibrator.Vibrate(milliSeconds);
        }

        public static void CheckLocationPermission()
        {
            var checkPermission = DependencyService.Get<ICheckPermisstion>();
            if (checkPermission != null)
                checkPermission.CheckLocationPermission();
        }
    }
}