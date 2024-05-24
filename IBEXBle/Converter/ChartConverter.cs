using IBEXBle.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace IBEXBle.Converter
{
    public class ChartSelectedColorConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isVisible = value as bool?;
            return isVisible ?? false ? Color.FromHex("#9E9E9E") : Color.Default;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
