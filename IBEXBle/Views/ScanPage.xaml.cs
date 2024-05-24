using IBEXBle.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IBEXBle.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScanPage : ContentPage
	{
		public ScanPage ()
		{
			InitializeComponent ();
		}

        protected override void OnDisappearing()
        {
            try
            {
                var adapter = Plugin.BLE.CrossBluetoothLE.Current.Adapter;
                if (adapter.IsScanning)
                    adapter.StopScanningForDevicesAsync();
            }
            catch
            {
            }
            
            base.OnDisappearing();
        }
    }
}