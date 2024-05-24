using IBEXBle.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace IBEXBle.Models
{
    public class BankModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Place { get; set; }
        public string Maker { get; set; }
        public string Model { get; set; }
        public string Capa { get; set; }
        public string Memo { get; set; }
        public AlarmModel Alarm { get; set; }
        public ObservableCollection<CellModel> Cells { get; set; }
        public Definitions.Alarm.Status AlarmStatus { get; set; }

        public BankModel()
        {
        }

        public BankModel(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var binaryReader = new SBinaryReader(memoryStream))
                {
                    Name = binaryReader.ReadString(10);
                    DateTime temp;
                    if (DateTime.TryParse(binaryReader.ReadString(10), out temp))
                        Date = temp;
                    else
                        Date = DateTime.Now;
                    Place = binaryReader.ReadString(10);
                    Maker = binaryReader.ReadString(10);
                    Model = binaryReader.ReadString(10);
                    Capa = binaryReader.ReadString(10);
                    Memo = binaryReader.ReadString(10);
                    Alarm = new AlarmModel(binaryReader.ReadBytes(8));
                }
            }            
        }
    }
}
