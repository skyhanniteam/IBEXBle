using IBEXBle.Core;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace IBEXBle.Models
{
    public class DeviceModel 
    {
        public string DeviceName { get; set; }
        public Guid Id { get; set; }        
        public IDevice Device { get; set; }
    }
}
