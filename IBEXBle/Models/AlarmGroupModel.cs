using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IBEXBle.Models
{
    public class AlarmGroupModel 
    {
        public string Name { get; set; }
        public ObservableCollection<AlarmModel> Alarms { get; set; }
    }
}
