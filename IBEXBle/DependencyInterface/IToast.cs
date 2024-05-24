using IBEXBle.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace IBEXBle.DependencyInterface
{
    public interface IToast
    {
        void MakeText(string text, Definitions.ToastLength length);
    }
}
