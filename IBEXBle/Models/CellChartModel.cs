using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IBEXBle.Models
{
    public class CellChartModel
    {
        public string Name { get; set; }

        public double Value { get; set; }
        public double Progress { get; set; }
        public Color Color { get; set; }
        public bool IsSeparator { get; set; }        
    }
}
