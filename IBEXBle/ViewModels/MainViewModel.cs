using IBEXBle.Core;
using IBEXBle.Views;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace IBEXBle.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            Title = "IBEX";
            adapter.DeviceConnected += Adapter_DeviceConnected;
            adapter.DeviceDisconnected += Adapter_DeviceConnected;
            adapter.DeviceConnectionLost += Adapter_DeviceConnected;
            Adapter_DeviceConnected(null, null);
            MessagingCenter.Subscribe<string>(this, "NavigateBank", (sender) =>
            {                
                Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.Navigation.PushAsync(new BankPage()));
            });
        }
        
        string connectingInfo;
        public string ConnectingInfo
        {
            get => connectingInfo;
            set => SetProperty(ref connectingInfo, value);
        }

        public bool isNotBusyAndBleConnected;
        public bool IsNotBusyAndBleConnected
        {            
            get => isNotBusyAndBleConnected;
            set => SetProperty(ref isNotBusyAndBleConnected, value);
        }

        private void SetBusyAndBleConnected()
        {
            IsNotBusyAndBleConnected = IsNotBusy && Bluetooth.Instance.IsConnected;
        }

        private void Adapter_DeviceConnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            var bluetooth = Bluetooth.Instance;
            ConnectingInfo = bluetooth.IsConnected ? $"{bluetooth.ConnectedDevices.First().Name} is Connected" : "NotConnected";            
            SetBusyAndBleConnected();
        }

        public ICommand DisConnect => new Command(() =>
        {
            Bluetooth.Instance.DisConnect();            
        });

        int receiveDataCount;
        public int ReceiveDataCount
        {
            get => receiveDataCount;
            set => SetProperty(ref receiveDataCount, value);
        }
        
        public ICommand DataDownload => new Command(async () =>
        {
            IsBusy = true;
            SetBusyAndBleConnected();
            await Application.Current.MainPage.Navigation.PushModalAsync(new DownloadPage());
            IsBusy = false;
            SetBusyAndBleConnected();
        });

        public ICommand Scan => new Command(async () =>
        {
            IsBusy = true;
            SetBusyAndBleConnected();
            await Application.Current.MainPage.Navigation.PushAsync(new ScanPage());
            IsBusy = false;
            SetBusyAndBleConnected();
        });

        public ICommand SelectExcel => new Command(async () =>
        {
            IsBusy = true;
            SetBusyAndBleConnected();
            await Application.Current.MainPage.Navigation.PushAsync(new SavedDataPage());
            IsBusy = false;
            SetBusyAndBleConnected();
        });

        public ICommand Bank => new Command(async () =>
        {
            IsBusy = true;
            SetBusyAndBleConnected();
            await Application.Current.MainPage.Navigation.PushAsync(new BankPage());
            IsBusy = false;
            SetBusyAndBleConnected();
        });
    }
}
