using IBEXBle.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace IBEXBle.Models
{
    public class IBexInfoModel
    {
        public byte Code { get; set; }
        /// <summary>
        /// 영점 직류전압
        /// </summary>
        public int ZeroBaseDCV { get; set; }
        /// <summary>
        /// 온도보상을 위한 임피던스 전류 기준
        /// </summary>
        public UInt16 ImpedanceStandard { get; set; }
        /// <summary>
        /// 케이블 임피던스 영점조절값
        /// </summary>
        public int CableImpedance { get; set; }
        /// <summary>
        /// 내부 상태 표시및 보정 계수
        /// </summary>
        public byte[] InternalStateAndcorrection { get; set; }
        /// <summary>
        /// 측정용 bank 번호
        /// </summary>
        public int CurrentBankNumber { get; set; }
        /// <summary>
        /// 측정용 cell 번호
        /// </summary>
        public int CurrentCellNumber { get; set; }
        /// <summary>
        /// Backlight 지속시간(분)
        /// </summary>
        public int BacklightTime { get; set; }
        /// <summary>
        /// 키나 측정이 없을떄 자동 전원차단시간(분)
        /// </summary>
        public int PowerConstant { get; set; }
        /// <summary>
        /// 0x00(dark)~ 0x40(bright)
        /// </summary>
        public byte Contrast { get; set; }
        /// <summary>
        /// 0x00 : C섭씨 , 0x01 : F(화씨)
        /// </summary>
        public byte Temperature { get; set; }
        /// <summary>
        /// 0x00: ON , 0x01 : OFF
        /// </summary>
        public byte Buzzer { get; set; }
        /// <summary>
        /// 0x00: Manual, 0x01 : Auto
        /// </summary>
        public byte Mode { get; set; }
        /// <summary>
        /// 0x00 ~ 0x03 : 4개 중에 선택
        /// </summary>
        public byte SelectedAlarm { get; set; }
        /// <summary>
        /// 내부 보정 계수
        /// </summary>
        public byte[] InterCorrection { get; set; }
        /// <summary>
        /// 현재알람
        /// </summary>
        public AlarmModel CurrentAlram { get; set; }

        public ObservableCollection<AlarmGroupModel> AlarmGroups { get; set; }

        public IBexInfoModel(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var binaryReader = new SBinaryReader(memoryStream))
                {
                    binaryReader.ReadBytes(4);
                    Code = binaryReader.ReadByte();
                    binaryReader.ReadByte();
                    ZeroBaseDCV = LittleEndian.ToInt32(binaryReader.ReadBytes(4));
                    ImpedanceStandard = LittleEndian.ToUint16(binaryReader.ReadBytes(2));
                    CableImpedance = LittleEndian.ToInt32(binaryReader.ReadBytes(4));
                    InternalStateAndcorrection = binaryReader.ReadBytes(11);
                    CurrentBankNumber = binaryReader.ReadByte();
                    CurrentCellNumber = LittleEndian.ToUint16(binaryReader.ReadBytes(2));
                    BacklightTime = binaryReader.ReadByte();
                    PowerConstant = binaryReader.ReadByte();
                    Contrast = binaryReader.ReadByte();
                    Temperature = binaryReader.ReadByte();
                    Buzzer = binaryReader.ReadByte();
                    Mode = binaryReader.ReadByte();
                    SelectedAlarm = binaryReader.ReadByte();
                    InterCorrection = binaryReader.ReadBytes(5);
                    CurrentAlram = new AlarmModel(binaryReader.ReadBytes(8));
                    binaryReader.ReadBytes(10);
                    AlarmGroups = new ObservableCollection<AlarmGroupModel>();
                    var alarmGroup1 = new AlarmGroupModel();
                    var alarmGroup2 = new AlarmGroupModel();
                    var alarmGroup3 = new AlarmGroupModel();
                    var alarmGroup4 = new AlarmGroupModel();
                    alarmGroup1.Alarms = new ObservableCollection<AlarmModel>();
                    alarmGroup2.Alarms = new ObservableCollection<AlarmModel>();
                    alarmGroup3.Alarms = new ObservableCollection<AlarmModel>();
                    alarmGroup4.Alarms = new ObservableCollection<AlarmModel>();
                    AlarmGroups.Add(alarmGroup1);
                    AlarmGroups.Add(alarmGroup2);
                    AlarmGroups.Add(alarmGroup3);
                    AlarmGroups.Add(alarmGroup4);
                    for (int i = 0; i < 4; i++)
                        AlarmGroups[i].Name = binaryReader.ReadString(10);
                    for (int i = 0; i < 4; i++)
                    {
                        var alarmGroup = AlarmGroups[i];
                        for (int j = 0; j < 20; j++)
                        {
                            var alarmName = binaryReader.ReadString(10);
                            var alarm = new AlarmModel(binaryReader.ReadBytes(8))
                            {
                                Name = alarmName
                            };
                            binaryReader.ReadBytes(2);
                            alarmGroup.Alarms.Add(alarm);
                        }
                    }
                }
            }
        }
    }
}