using IBEXBle.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace IBEXBle.Converter
{
    public class AlarmColorConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var alarm = value as Definitions.Alarm.Status?;
            if (alarm == null)
                return Color.Default;
            return Definitions.Alarm.Color(alarm.Value);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
