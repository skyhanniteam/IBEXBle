using IBEXBle.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IBEXBle.Models
{
    public class AlarmModel
    {
        public string Name { get; set; }
        public double ImpedanceStandard { get; set; }
        public double ImpedanceFail { get; set; }
        public double ImpedanceAlarm { get; set; }
        public double OverVoltage { get; set; }
        public double UnderVoltage { get; set; }
        public double Temperature { get; set; }

        public AlarmModel()
        {

        }

        public AlarmModel(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var binaryReader = new BinaryReader(memoryStream))
                {
                    ImpedanceStandard = LittleEndian.ToUint16(binaryReader.ReadBytes(2)) / 100.0;
                    ImpedanceFail = binaryReader.ReadByte();
                    ImpedanceAlarm = binaryReader.ReadByte();
                    OverVoltage = binaryReader.ReadByte() / 10.0;
                    UnderVoltage = binaryReader.ReadByte() / 10.0;
                    Temperature = binaryReader.ReadByte() - 20.0;
                }
            }
        }
    }
}
