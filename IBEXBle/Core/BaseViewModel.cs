using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace IBEXBle.Core
{
    public class BaseViewModel : BaseModel
    {
        protected IBluetoothLE ble = CrossBluetoothLE.Current;
        protected IAdapter adapter = CrossBluetoothLE.Current.Adapter;

        public BaseViewModel()
        {   
            adapter.DeviceConnected += Adapter_DeviceConnected;
            adapter.DeviceDisconnected += Adapter_DeviceConnected;
            adapter.DeviceConnectionLost += Adapter_DeviceConnected;            
        }

        private void Adapter_DeviceConnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            SetBleConnected();
        }

        protected void SetBleConnected()
        {
            BleConnected = Bluetooth.Instance.IsConnected;
        }

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set { if (SetProperty(ref isBusy, value)) IsNotBusy = !isBusy; }
        }

        bool isNotBusy = true;
        public bool IsNotBusy
        {
            get => isNotBusy;
            set { if (SetProperty(ref isNotBusy, value)) IsBusy = !isNotBusy; }
        }

        private bool bleConnected;
        public bool BleConnected
        {
            get => bleConnected;
            set => SetProperty(ref bleConnected, value);
        }
    }
}