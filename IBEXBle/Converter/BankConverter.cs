using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace IBEXBle.Converter
{
    public class BankDateConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = value as DateTime?;
            if (!date.HasValue)
                return null;
            return date.Value.ToString("yyyy-MM-dd");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;                
        }
    }
}
