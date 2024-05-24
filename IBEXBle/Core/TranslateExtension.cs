using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IBEXBle.Core
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        const string ResourceId = "IBEXBle.Droid.AppResource";
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    return Droid.AppResource.ResourceManager.GetString(Text, CultureInfo.CurrentCulture);
                default:
                    break;
            }
            
            return string.Empty;            
        }
    }
}
