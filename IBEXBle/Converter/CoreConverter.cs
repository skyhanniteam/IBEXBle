using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace IBEXBle.Converter
{
    public class SwapBoolConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = value as bool?;
            return boolValue.HasValue && boolValue.Value ? false : true;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = value as bool?;
            return boolValue.HasValue && boolValue.Value ? false : true;
        }
    }
}
