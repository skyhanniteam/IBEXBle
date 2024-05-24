using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBEXBle.Core
{
    public class Bluetooth
    {
        private static Bluetooth instance;
        private static object syncLock = new Object();

        public static Bluetooth Instance
        {
            get
            {
                lock (syncLock)
                {
                    if (instance == null)
                        instance = new Bluetooth();
                }

                return instance;
            }
        }

        public bool IsConnected
        {
            get
            {
                try
                {
                    var connectedDevices = ConnectedDevices;
                    if (connectedDevices == null)
                        return false;
                    return connectedDevices.Count > 0;
                }
                catch
                {
                    return false;
                }
            }
        }

        public async void DisConnect()
        {
            try
            {
                if (!IsConnected)
                    return;
                var adapter = CrossBluetoothLE.Current.Adapter;
                foreach (var item in ConnectedDevices)
                    await adapter.DisconnectDeviceAsync(item);
            }
            catch (Exception ex)
            {
                CoreUtils.Toast(ex.Message, Definitions.ToastLength.ShortLength);
            }
        }

        public string DeviceName
        {
            get
            {
                if (!IsConnected)
                    return null;
                return ConnectedDevices.First().Name;
            }
        }

        public IList<IDevice> ConnectedDevices
        {
            get
            {
                var adapter = CrossBluetoothLE.Current.Adapter;
                if (adapter == null || adapter.ConnectedDevices == null)
                    return null;
                return adapter.ConnectedDevices.Where(r => r.Name.ToLower().Contains("ibex")).ToList();
            }
        }
    }
}
