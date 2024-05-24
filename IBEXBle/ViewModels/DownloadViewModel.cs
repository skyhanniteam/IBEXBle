using IBEXBle.Core;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IBEXBle.ViewModels
{
    public class DownloadViewModel : BaseViewModel
    {
        private bool isPopModal = false;
        private object lockPopModal = new object();
        private System.Timers.Timer timer;
        public DownloadViewModel()
        {
            Title = "Download Page";            
            Download();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
        }

        private int receiveDataLength;
        public int ReceiveDataLength
        {
            get => receiveDataLength;
            set => SetProperty(ref receiveDataLength, value);
        }


        private int totalDataLength;
        public int TotalDataLength
        {
            get => totalDataLength;
            set => SetProperty(ref totalDataLength, value);
        }


        private double progress;
        public double Progress
        {
            get => progress;
            set => SetProperty(ref progress, value);
        }

        private bool isCancel = false;

        private async void Download()
        {
            var startBytes = Encoding.ASCII.GetBytes("S*!t");

            try
            {
                var bluetooth = Bluetooth.Instance;
                var connectedDevices = bluetooth.ConnectedDevices;
                if (connectedDevices.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert(string.Empty, "disconneted", "ok"); ;
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                    return;
                }                
                
                var service = await connectedDevices.First().GetServiceAsync(Definitions.TISensorTagSmartKeysGuid);
                var characteristic = await service.GetCharacteristicAsync(Definitions.TISensorTagKeysDataGuid);

                var listByte = new List<byte>();
                ReceiveDataLength = 0;
                TotalDataLength = -1;
                bool isClose = false;
                timer = new System.Timers.Timer(3000);
                timer.Elapsed += (sender, e) =>
                {
                    isCancel = true;
                    isClose = true;
                    timer.Close();
                    Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.DisplayAlert(string.Empty, "Download failed.", "ok"));
                    Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.Navigation.PopModalAsync());
                    return;
                };
                timer.Start();
                characteristic.ValueUpdated += (o, args) =>
                {                    
                    if (timer != null)
                    {
                        timer.Stop();
                        timer.Start();
                    }
                    
                    if (isCancel)
                        characteristic.StopUpdatesAsync();
                    if (isCancel || isClose)
                    {                        
                        timer.Close();  
                        return;
                    }
                    var bytes = args.Characteristic.Value;
                    ReceiveDataLength += bytes.Length;
                    listByte.AddRange(bytes);
                    if (totalDataLength == -1)
                    {
                        Progress = 0;

                        if (listByte.Count >= 4)
                        {
                            var ibex = IBex.Instance;
                            switch (listByte[3])
                            {
                                case 0x41: ibex.BankNumber = 30; ibex.CellNumber = 48; break;
                                case 0x42: ibex.BankNumber = 28; ibex.CellNumber = 64; break;
                                case 0x43: ibex.BankNumber = 24; ibex.CellNumber = 80; break;
                                case 0x44: ibex.BankNumber = 22; ibex.CellNumber = 96; break;
                                case 0x45: ibex.BankNumber = 20; ibex.CellNumber = 128; break;
                                case 0x46: ibex.BankNumber = 18; ibex.CellNumber = 160; break;
                                case 0x47: ibex.BankNumber = 16; ibex.CellNumber = 192; break;
                                case 0x48: ibex.BankNumber = 14; ibex.CellNumber = 240; break;
                                case 0x49: ibex.BankNumber = 12; ibex.CellNumber = 288; break;
                                case 0x4A: ibex.BankNumber = 10; ibex.CellNumber = 384; break;
                                case 0x4B: ibex.BankNumber = 8; ibex.CellNumber = 512; break;
                                case 0x4C: ibex.BankNumber = 6; ibex.CellNumber = 720; break;
                                case 0x4D: ibex.BankNumber = 4; ibex.CellNumber = 1152; break;
                                case 0x4E: ibex.BankNumber = 3; ibex.CellNumber = 1568; break;
                                case 0x4F: ibex.BankNumber = 1; ibex.CellNumber = 4800; break;
                                default:
                                    isCancel = true;
                                    timer.Close();
                                    Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.DisplayAlert(string.Empty, "Download failed.", "ok"));
                                    Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.Navigation.PopModalAsync());
                                    return;
                            }
                            ibex.DeviceName = connectedDevices.First().Name;
                            TotalDataLength = 1700 + ibex.BankNumber * 90 + ibex.BankNumber * ibex.CellNumber * 6 ;
                        }                          
                    }
                    else
                        Progress = (double)receiveDataLength / (double)totalDataLength;
                    if (!isClose && totalDataLength != -1 && totalDataLength <= receiveDataLength)
                    {
                        isClose = true;
                        timer.Close();
                        characteristic.StopUpdatesAsync();                        

                        if (!IBex.Instance.Make(listByte.ToArray()))
                        {                            
                            Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.DisplayAlert(string.Empty, "Download failed.", "ok"));
                            bluetooth.DisConnect();
                            Application.Current.MainPage.Navigation.PopModalAsync();
                            return;
                        }
                        Application.Current.MainPage.Navigation.PopModalAsync();
                        lock (lockPopModal)
                        {
                            if (!isPopModal)
                            {
                                isPopModal = true;                                
                                MessagingCenter.Send<string>("", "NavigateBank");
                                Bluetooth.Instance.DisConnect();
                                CoreUtils.Vibrate(300);
                            }
                        }                        
                        return;
                    }
                };
                await characteristic.StartUpdatesAsync();
                await characteristic.WriteAsync(startBytes);
                
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(string.Empty, ex.Message, "ok");
            }
        }  

        public ICommand Cancel => new Command(async () =>
        {
            isCancel = true;
            if (timer != null)
                timer.Close();
            Bluetooth.Instance.DisConnect();
            await Application.Current.MainPage.Navigation.PopModalAsync();
        });        
    }    
}
