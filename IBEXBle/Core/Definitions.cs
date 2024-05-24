using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IBEXBle.Core
{
    public class Definitions
    {
        public enum ToastLength { LongLength, ShortLength };        
        public static bool IsDesignMode
        {
            get => Xamarin.Forms.Application.Current == null;
        }
        public static Guid TISensorTagSmartKeysGuid = new Guid("0000ffe0-0000-1000-8000-00805f9b34fb");
        public static Guid TISensorTagKeysDataGuid = new Guid("0000ffe1-0000-1000-8000-00805f9b34fb");

        public static class Alarm
        {
            public enum Status { Normal = 0, Waring = 1, Fail = 2 }
            public static string WarningColorHex = "#ff9800";
            public static string FailColorHex = "#f44336";
            public static Xamarin.Forms.Color Color(Status status)
            {
                return AlarmColor(status);
            }
        }

        public static Xamarin.Forms.Color AlarmColor(Alarm.Status status)
        {
            switch (status)
            {
                case Definitions.Alarm.Status.Normal:
                    return Xamarin.Forms.Color.Default;
                case Definitions.Alarm.Status.Waring:
                    return Xamarin.Forms.Color.FromHex(Alarm.WarningColorHex);
                case Definitions.Alarm.Status.Fail:
                    return Xamarin.Forms.Color.FromHex(Alarm.FailColorHex);
                default:
                    break;
            }
            return Xamarin.Forms.Color.Default;
        }
    }    
}
