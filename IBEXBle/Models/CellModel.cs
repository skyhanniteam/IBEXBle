using IBEXBle.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IBEXBle.Models
{
    public class CellModel
    {
        public int Id { get; set; }
        public double? Impedance { get; set; }
        public double? ImpedancePercentage { get; set; }
        public double? Voltage { get; set; }
        public double? Temperature { get; set; }
        public Definitions.Alarm.Status ImpedanceAlarm { get; set; }
        public Definitions.Alarm.Status VoltageAlarm { get; set; }
        public Definitions.Alarm.Status TemperatureAlarm { get; set; }                

        public CellModel()
        {
        }
        public CellModel(byte[] bytes, AlarmModel alarm)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var binaryReader = new BinaryReader(memoryStream))
                {
                    bool ExistFF(byte[] source)
                    {
                        foreach (var item in source)
                        {
                            if (item != 0xff)
                                return false;
                        }
                        return true;
                    }

                    var impedanceByte = new byte[4];
                    var b = binaryReader.ReadBytes(3);
                    if (ExistFF(b))
                        Impedance = null;
                    else
                    {
                        b.CopyTo(impedanceByte, 0);
                        Impedance = Math.Round(LittleEndian.ToUInt32(impedanceByte) / 10000.0, 3);
                    }

                    var voltageByte = binaryReader.ReadBytes(2);
                    if (ExistFF(voltageByte))
                        Voltage = null;
                    else
                        Voltage = Math.Round(LittleEndian.ToUint16(voltageByte) / 1000.0, 2);
                    var temperatureByte = binaryReader.ReadByte();
                    if (temperatureByte >= 0xC7)
                        Temperature = null;
                    else
                        Temperature = Math.Round(temperatureByte / 2.0 - 20.0, 1);

                    if (alarm != null)
                    {
                        if (Impedance.HasValue && alarm.ImpedanceStandard != 0)
                        {
                            ImpedancePercentage = Impedance.Value / alarm.ImpedanceStandard * 100;
                            ImpedancePercentage = Math.Round(ImpedancePercentage.Value, 2);
                            if (ImpedancePercentage > alarm.ImpedanceFail)
                                ImpedanceAlarm = Definitions.Alarm.Status.Fail;
                            else if (ImpedancePercentage > alarm.ImpedanceAlarm)
                                ImpedanceAlarm = Definitions.Alarm.Status.Waring;
                            else
                                ImpedanceAlarm = Definitions.Alarm.Status.Normal;
                        }
                        else
                            ImpedanceAlarm = Definitions.Alarm.Status.Normal;

                        if (Voltage.HasValue)
                        {
                            if (Voltage.Value >= alarm.OverVoltage || Voltage.Value <= alarm.UnderVoltage)
                                VoltageAlarm = Definitions.Alarm.Status.Fail;
                            else
                                VoltageAlarm = Definitions.Alarm.Status.Normal;
                        }
                        else
                            VoltageAlarm = Definitions.Alarm.Status.Normal;


                        if (Temperature.HasValue)
                        {
                            if (Temperature.Value >= alarm.Temperature)
                                TemperatureAlarm = Definitions.Alarm.Status.Fail;
                            else
                                TemperatureAlarm = Definitions.Alarm.Status.Normal;
                        }
                        else
                            TemperatureAlarm = Definitions.Alarm.Status.Normal;
                    }
                }
            }
        }
    }
}
