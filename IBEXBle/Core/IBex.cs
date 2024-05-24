using IBEXBle.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace IBEXBle.Core
{
    public class IBex : BaseModel
    {
        private static IBex instance;
        private static object syncLock = new Object();

        public static IBex Instance
        {
            get
            {
                lock (syncLock)
                {
                    if (instance == null)
                        instance = new IBex();
                }

                return instance;
            }
        }

        public void Clear()
        {
            DeviceName = string.Empty;
            BankNumber = 0;
            CellNumber = 0;
        }

        private string deviceName;
        public string DeviceName
        {
            get => deviceName;
            set => SetProperty(ref deviceName, value);
        }

        private int bankNumber;
        public int BankNumber
        {
            get => bankNumber;
            set => SetProperty(ref bankNumber, value);
        }

        private int cellNumber;
        public int CellNumber
        {
            get => cellNumber;
            set => SetProperty(ref cellNumber, value);
        }


        private DateTime receivedTime;
        public DateTime ReceivedTime
        {
            get => receivedTime;
            set => SetProperty(ref receivedTime, value);
        }

        private IBexInfoModel iBexInfo;
        public IBexInfoModel IBexInfo
        {
            get => iBexInfo;
            set => SetProperty(ref iBexInfo, value);
        }

        private ObservableCollection<BankModel> banks;
        public ObservableCollection<BankModel> Banks
        {
            get => banks;
            set => SetProperty(ref banks, value);
        }

        public bool Make(byte[] bytes)
        {
            try
            {
                using (var memoryStream = new MemoryStream(bytes))
                {
                    using (var binaryReader = new BinaryReader(memoryStream))
                    {
                        IBexInfo = new IBexInfoModel(binaryReader.ReadBytes(1700));

                        Banks = new ObservableCollection<BankModel>();

                        var bankList = new List<BankModel>();

                        for (int i = 0; i < bankNumber; i++)
                        {
                            var bank = new BankModel(binaryReader.ReadBytes(90))
                            {
                                Id = i
                            };
                            bankList.Add(bank);
                        }

                        for (int i = 0; i < bankNumber; i++)
                        {
                            var bank = bankList.Single(r => r.Id == i);
                            bank.Cells = new ObservableCollection<CellModel>();

                            for (int j = 0; j < cellNumber; j++)
                            {
                                var cell = new CellModel(binaryReader.ReadBytes(6), bank.Alarm)
                                {
                                    Id = j + 1
                                };
                                if (cell.Impedance.HasValue)
                                    bank.Cells.Add(cell);
                            }
                            if (bank.Cells.Count == 0)
                                bankList.Remove(bank);
                            else
                            {
                                var alarmStaus = new List<Definitions.Alarm.Status>
                                {
                                    bank.Cells.Max(r => r.ImpedanceAlarm),
                                    bank.Cells.Max(r => r.VoltageAlarm),
                                    bank.Cells.Max(r => r.TemperatureAlarm)
                                };
                                bank.AlarmStatus = alarmStaus.Max();
                            }
                        }

                        Banks = new ObservableCollection<BankModel>(bankList);
                    }
                }

                ReceivedTime = DateTime.Now;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }            
        }

        public ObservableCollection<CellModel> SelectCell(int bankId)
        {
            var bank = banks.FirstOrDefault(r => r.Id == bankId);
            if (bank == null)
                return null;
            return bank.Cells;
        }        
    }
}
