using IBEXBle.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace IBEXBle.Models
{
    public class ExcelModel : BaseModel
    {
        public bool IsSelected { get; set; }
        public string FileName { get; set; }
    }
}
