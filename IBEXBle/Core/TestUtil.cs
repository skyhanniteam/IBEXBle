using IBEXBle.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace IBEXBle.Core
{
    public class TestUtil
    {
        public static void MakeTestIBex()
        {
            var MakeAlarm = new Func<AlarmModel>(() =>
            {
                return new AlarmModel
                {
                    ImpedanceStandard = 10,
                    ImpedanceFail = 150,
                    ImpedanceAlarm = 130,
                    OverVoltage = 14,
                    UnderVoltage = 10.8,
                    Temperature = 55
                };
            });

            var iBex = IBex.Instance;
            iBex.BankNumber = 2;
            iBex.CellNumber = 10;
            iBex.ReceivedTime = DateTime.Now;

            var bank1 = new BankModel
            {
                Id = 0,
                Name = "bank1",
                Date = DateTime.Now,
                Place = "place1",
                Maker = "maker1",
                Model = "model1",
                Capa = "capa1",
                Memo = "memo1"
            };
            bank1.Alarm = MakeAlarm();
            var bank2 = new BankModel
            {
                Id = 1,
                Name = "bank2",
                Date = DateTime.Now,
                Place = "place2",
                Maker = "maker2",
                Model = "model2",
                Capa = "capa2",
                Memo = "memo2"
            };
            bank2.Alarm = MakeAlarm();
            iBex.Banks = new ObservableCollection<BankModel>
            {
                bank1,
                bank2
            };
            bank1.Cells = new ObservableCollection<CellModel>
            {
                new CellModel
                {
                    Id = 1,
                    Voltage = 13.234,
                    Impedance = 12.73234,                    
                    Temperature = 44.6343,
                    ImpedanceAlarm = Definitions.Alarm.Status.Normal,
                    VoltageAlarm = Definitions.Alarm.Status.Normal,
                    TemperatureAlarm = Definitions.Alarm.Status.Normal
                },
                new CellModel
                {
                    Id = 2,
                    Voltage = 13.3,
                    Impedance = 11.4,
                    Temperature = 35.6,
                    ImpedanceAlarm = Definitions.Alarm.Status.Normal,
                    VoltageAlarm = Definitions.Alarm.Status.Normal,
                    TemperatureAlarm = Definitions.Alarm.Status.Normal
                },
                new CellModel
                {
                    Id = 3,
                    Voltage = 12.532223,
                    Impedance = 15.734234,
                    Temperature = 43.623423,
                    ImpedanceAlarm = Definitions.Alarm.Status.Fail,
                    VoltageAlarm = Definitions.Alarm.Status.Normal,
                    TemperatureAlarm = Definitions.Alarm.Status.Normal
                },
                new CellModel
                {
                    Id = 4,
                    Voltage = 14.8322,
                    Impedance = 13.433,
                    Temperature = 62.6232,
                    ImpedanceAlarm = Definitions.Alarm.Status.Waring,
                    VoltageAlarm = Definitions.Alarm.Status.Fail,
                    TemperatureAlarm = Definitions.Alarm.Status.Fail
                },
                new CellModel
                {
                    Id = 5,
                    Voltage = 11.9,
                    Impedance = 9.1,
                    Temperature = 43.6,
                    ImpedanceAlarm = Definitions.Alarm.Status.Normal,
                    VoltageAlarm = Definitions.Alarm.Status.Normal,
                    TemperatureAlarm = Definitions.Alarm.Status.Normal
                },
                new CellModel
                {
                    Id = 6,
                    Voltage = 8.6234,
                    Impedance = 11.123,
                    Temperature = 23.62332,
                    ImpedanceAlarm = Definitions.Alarm.Status.Normal,
                    VoltageAlarm = Definitions.Alarm.Status.Fail,
                    TemperatureAlarm = Definitions.Alarm.Status.Normal
                }
            };

            bank2.Cells = new ObservableCollection<CellModel>();

            for (int i = 0; i < 119; i++)
            {
                bank2.Cells.Add(new CellModel
                {
                    Id = i + 1,
                    Voltage = 1.1,
                    Impedance = 2.5,
                    Temperature = 25.6
                });
            }

            for (int i = 3; i < 14; i++)
            {
                var bank = new BankModel
                {
                    Id = i,
                    Name = $"bank{i}",
                    Date = DateTime.Now,
                    Place = "place1",
                    Maker = "maker1",
                    Model = "model1",
                    Capa = "capa1",
                    Memo = "memo1"
                };

                bank.Alarm = MakeAlarm();

                bank.Cells = new ObservableCollection<CellModel>();

                for (int j = 0; j < 50; j++)
                {
                    bank.Cells.Add(new CellModel
                    {
                        Id = j + 1,
                        Voltage = 1.1,
                        Impedance = 2.5,
                        Temperature = 25.6
                    });
                }
                iBex.Banks.Add(bank);
            }

            foreach (var bank in iBex.Banks)
            {
                var alarmStaus = new List<Definitions.Alarm.Status>
                {
                    bank.Cells.Max(r => r.ImpedanceAlarm),
                    bank.Cells.Max(r => r.VoltageAlarm),
                    bank.Cells.Max(r => r.TemperatureAlarm)
                };
                bank.AlarmStatus = alarmStaus.Max();

                foreach (var cell in bank.Cells)                
                    cell.ImpedancePercentage = cell.Impedance.Value / bank1.Alarm.ImpedanceStandard * 100;                
            }            
        }         
    }
}
