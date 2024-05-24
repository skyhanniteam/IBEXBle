using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace IBEXBle.Converter
{
    public class TemperatureConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var doubleValue = value as double?;
            if (!doubleValue.HasValue)
                return null;

            if (culture.Name.Replace("-", string.Empty).Replace(".", string.Empty).ToLower() != "kokr" && culture.Name.ToLower() != "ko")
            {            //double f = (c + 40) * 1.8 - 40;
                doubleValue = Math.Round(doubleValue.Value, 1, MidpointRounding.AwayFromZero);
                return Math.Round((doubleValue.Value * 1.8) + 32);
            }

            return doubleValue;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
