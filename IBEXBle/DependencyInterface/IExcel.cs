using IBEXBle.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IBEXBle.DependencyInterface
{
    public interface IExcel
    {
        bool DownloadMSExcel();
        bool Write(Stream stream, string fileName);
        List<ExcelModel> Select();
        bool Shared(List<string> fileNames);
        bool Open(string fileName);
        bool Delete(List<string> fileNames);        
    }
}
