using IBEXBle.Core;
using IBEXBle.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IBEXBle.ViewModels
{
    public class CellChartViewModel : BaseViewModel
    {
        public CellChartViewModel()
        {
            VisibleResistance = true;
            VisibleVoltage = false;
            VisibleTemperature = false;
        }
        private BankModel bankModel;
        private int bankId;
        public int BankId
        {
            set
            {
                SetProperty(ref bankId, value);
                var ibex = IBex.Instance;
                if (ibex.Banks != null)
                {
                    bankModel = ibex.Banks.SingleOrDefault(bank => bank.Id == value);
                    if (bankModel != null)
                        Title = bankModel.Name;

                    AlarmResistanceFailValue = bankModel.Alarm.ImpedanceStandard * bankModel.Alarm.ImpedanceFail / 100;
                    AlarmResistanceWarningValue = bankModel.Alarm.ImpedanceStandard * bankModel.Alarm.ImpedanceAlarm / 100;
                    AlarmOverVoltageValue = bankModel.Alarm.OverVoltage;
                    AlarmUnderVoltageValue = bankModel.Alarm.UnderVoltage;
                    AlarmTemperatureValue = bankModel.Alarm.Temperature;
                }
                    
            }
        }

        private bool visibleResistance;
        public bool VisibleResistance
        {
            get => visibleResistance;
            set => SetProperty(ref visibleResistance, value);
        }

        private bool visibleVoltage;
        public bool VisibleVoltage
        {
            get => visibleVoltage;
            set => SetProperty(ref visibleVoltage, value);
        }

        private bool visibleTemperature;
        public bool VisibleTemperature
        {
            get => visibleTemperature;
            set => SetProperty(ref visibleTemperature, value);
        }

        private double alarmResistanceWarningValue;
        public double AlarmResistanceWarningValue
        {
            get => alarmResistanceWarningValue;
            set => SetProperty(ref alarmResistanceWarningValue, value);
        }


        private double alarmResistanceFailValue;
        public double AlarmResistanceFailValue
        {
            get => alarmResistanceFailValue;
            set => SetProperty(ref alarmResistanceFailValue, value);
        }


        private double alarmOverVoltageValue;
        public double AlarmOverVoltageValue
        {
            get => alarmOverVoltageValue;
            set => SetProperty(ref alarmOverVoltageValue, value);
        }

        private double alarmUnderVoltageValue;
        public double AlarmUnderVoltageValue
        {
            get => alarmUnderVoltageValue;
            set => SetProperty(ref alarmUnderVoltageValue, value);
        }


        private double alarmTemperatureValue;
        public double AlarmTemperatureValue
        {
            get => alarmTemperatureValue;
            set => SetProperty(ref alarmTemperatureValue, value);
        }


        private ObservableCollection<CellModel> cells;
        public ObservableCollection<CellModel> Cells
        {
            set
            {
                SetProperty(ref cells, value);
                MakeChartModel();
            }
        }

        private ObservableCollection<CellChartModel> resistanceCharts;
        public ObservableCollection<CellChartModel> ResistanceCharts
        {
            get => resistanceCharts;
            set => SetProperty(ref resistanceCharts, value);
        }


        private ObservableCollection<CellChartModel> voltageCharts;
        public ObservableCollection<CellChartModel> VoltageCharts
        {
            get => voltageCharts;
            set => SetProperty(ref voltageCharts, value);
        }


        private ObservableCollection<CellChartModel> temperatureCharts;
        public ObservableCollection<CellChartModel> TemperatureCharts
        {
            get => temperatureCharts;
            set => SetProperty(ref temperatureCharts, value);
        }

        private void MakeChartModel()
        {
            if (cells == null)
                return;

            var maxResistance = 0.0;
            var maxVoltage = 0.0;
            var maxTemperature = 0.0;

            maxResistance = cells.Max(r => r.Impedance).GetValueOrDefault(0);
            if (maxResistance < AlarmResistanceFailValue)
                maxResistance = AlarmResistanceFailValue;
            maxVoltage = cells.Max(r => r.Voltage).GetValueOrDefault(0);
            if (maxVoltage < AlarmOverVoltageValue)
                maxVoltage = AlarmOverVoltageValue;
            maxTemperature = cells.Max(r => r.Temperature).GetValueOrDefault(0);
            if (maxTemperature < AlarmTemperatureValue)
                maxTemperature = AlarmTemperatureValue;

            Color AlarmColor(Definitions.Alarm.Status status)
            {
                if (status == Definitions.Alarm.Status.Normal)
                    return Color.WhiteSmoke;
                return Definitions.AlarmColor(status);
            }

            var resistanceChartModels = new ObservableCollection<CellChartModel>
            {
                new CellChartModel
                {
                    Name = "Warn",
                    Progress = maxResistance > 0 ? AlarmResistanceFailValue / maxResistance : 0,
                    Value = AlarmResistanceFailValue,
                    Color = AlarmColor(Definitions.Alarm.Status.Fail)
                },
                new CellChartModel
                {
                    Name = "Fail",
                    Progress = maxResistance > 0 ? AlarmResistanceWarningValue / maxResistance : 0,
                    Value = AlarmResistanceWarningValue,
                    Color = AlarmColor(Definitions.Alarm.Status.Waring)
                },
                new CellChartModel
                {
                    IsSeparator = true
                }
            };

            var voltageChartModels = new ObservableCollection<CellChartModel>
            {
                new CellChartModel
                {
                    Name = "Max",
                    Progress = maxVoltage > 0  ? AlarmOverVoltageValue / maxVoltage : 0,
                    Value = AlarmOverVoltageValue,
                    Color = AlarmColor(Definitions.Alarm.Status.Fail)
                },
                new CellChartModel
                {
                    Name = "Min",
                    Progress = maxVoltage > 0  ? AlarmUnderVoltageValue / maxVoltage : 0,
                    Value = AlarmUnderVoltageValue,
                    Color = AlarmColor(Definitions.Alarm.Status.Fail)
                },
                new CellChartModel
                {
                    IsSeparator = true
                }
            };

            var temperatureChartModels = new ObservableCollection<CellChartModel>
            {
                new CellChartModel
                {
                    Name = "Fail",
                    Progress = maxTemperature > 0  ? AlarmTemperatureValue/ maxTemperature : 0,
                    Value = AlarmTemperatureValue,
                    Color = AlarmColor(Definitions.Alarm.Status.Fail)
                },
                new CellChartModel
                {
                    IsSeparator = true
                }
            };

            foreach (var cell in cells)
            {
                resistanceChartModels.Add(new CellChartModel
                {
                    Name = cell.Id.ToString(),
                    Progress = maxResistance > 0 ? cell.Impedance.GetValueOrDefault(0) / maxResistance : 0,
                    Value = cell.Impedance.GetValueOrDefault(0),
                    Color = AlarmColor(cell.ImpedanceAlarm)
                });


                voltageChartModels.Add(new CellChartModel
                {
                    Name = cell.Id.ToString(),
                    Progress = maxVoltage > 0 ? cell.Voltage.GetValueOrDefault(0) / maxVoltage : 0,
                    Value = cell.Voltage.GetValueOrDefault(0),
                    Color = AlarmColor(cell.VoltageAlarm)
                });


                temperatureChartModels.Add(new CellChartModel
                {
                    Name = cell.Id.ToString(),
                    Progress = maxTemperature > 0 ? cell.Temperature.GetValueOrDefault(0) / maxTemperature : 0,
                    Value = cell.Temperature.GetValueOrDefault(0),
                    Color = AlarmColor(cell.TemperatureAlarm)
                });                
            }

            ResistanceCharts = resistanceChartModels;
            VoltageCharts = voltageChartModels;
            TemperatureCharts = temperatureChartModels;

        }

        public ICommand VisibleButton => new Command((parameter) =>
        {
            VisibleResistance = VisibleVoltage = VisibleTemperature = false;
            switch (parameter.ToString())
            {
                case "R":
                    VisibleResistance = true;
                    break;
                case "V":
                    VisibleVoltage = true;
                    break;
                case "T":
                    VisibleTemperature = true;
                    break;
                default:
                    break;
            }
        });

        public ICommand Close => new Command(async () =>
        {
            IsBusy = true;
            await Application.Current.MainPage.Navigation.PopModalAsync();
            IsBusy = false;
        });
    }
}
