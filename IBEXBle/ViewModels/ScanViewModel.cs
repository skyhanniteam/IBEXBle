using IBEXBle.Core;
using IBEXBle.Models;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace IBEXBle.ViewModels
{
    public class ScanViewModel : BaseViewModel
    {
        public ScanViewModel()
        {
            Title = "Scan Device";
            IsScanning = false;
            SetBleConnected();
            if (BleConnected)
                ConnectingInfo = $"Connected : {Bluetooth.Instance.DeviceName}";
            else
                ConnectingInfo = "Not connected";
            Scan.Execute(null);
        }

        private ObservableCollection<DeviceModel> deviceModel;
        public ObservableCollection<DeviceModel> DeviceModels
        {
            get => deviceModel;
            set => SetProperty(ref deviceModel, value);
        }

        private DeviceModel selectedDevice;
        public DeviceModel SelectedDevice
        {
            get => selectedDevice;
            set
            {
                try
                {
                    if (SetProperty(ref selectedDevice, value))
                        if (!string.IsNullOrWhiteSpace(selectedDevice.DeviceName))
                            ConnectDevice();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        string connectingInfo;
        public string ConnectingInfo
        {
            get => connectingInfo;
            set => SetProperty(ref connectingInfo, value);
        }

        bool isScanning;
        public bool IsScanning
        {
            get => isScanning;
            set => SetProperty(ref isScanning, value);
        }        

        private async void ConnectDevice()
        {
            IsBusy = true;
            try
            {
                Bluetooth.Instance.DisConnect();                
                await adapter.ConnectToDeviceAsync(selectedDevice.Device);
                CoreUtils.Toast($"Connet to {selectedDevice.DeviceName}", Definitions.ToastLength.ShortLength);
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                CoreUtils.Toast(ex.Message, Definitions.ToastLength.ShortLength);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ICommand Scan => new Command(async () =>
        {
            CoreUtils.CheckLocationPermission();
            IsScanning = true;

            DeviceModels = new ObservableCollection<DeviceModel>();

            adapter.DeviceDiscovered += (s, a) =>
            {
                if (a.Device != null && a.Device.Name != null && a.Device.Name.ToLower().Contains("ibex") && !DeviceModels.Any(r => r.Id == a.Device.Id))
                    DeviceModels.Add(new DeviceModel
                    {
                        Id = a.Device.Id,
                        Device = a.Device,
                        DeviceName = a.Device.Name
                    });
            };
            adapter.ScanTimeout = 10000;
            await adapter.StartScanningForDevicesAsync();

            IsScanning = false;
        });
    }
}
