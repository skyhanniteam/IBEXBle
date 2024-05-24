using IBEXBle.DependencyInterface;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using Xamarin.Forms;

namespace IBEXBle.Core
{
    public class ExcelUtil
    {
        public static bool WriteIbex()
        {
            var ibex = IBex.Instance;
            if (ibex.Banks == null || ibex.Banks.Count == 0)
                return false;

            try
            {
                using (var excelEngine = new ExcelEngine())
                {
                    var temperatureConverter = new Converter.TemperatureConverter();

                    double? ConvertTemperature(double? value)
                    {
                        if (!value.HasValue)
                            return null;
                        var culture = CultureInfo.CurrentCulture;

                        if (culture.Name.Replace("-", string.Empty).Replace(".", string.Empty).ToLower() != "kokr" && culture.Name.ToLower() != "ko")
                        {            //double f = (c + 40) * 1.8 - 40;
                            value = Math.Round(value.Value, 1, MidpointRounding.AwayFromZero);
                            return Math.Round((value.Value * 1.8) + 32);
                        }
                        return value;
                    }

                    excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2007;                    
                    
                    var workbook = excelEngine.Excel.Workbooks.Create(ibex.Banks.Count);
                    workbook.StandardFont = "맑은 고딕";
                    //Defining header style
                    var bodyStyle = workbook.Styles.Add("BodyStyle");
                    bodyStyle.BeginUpdate();                    
                    bodyStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                    bodyStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                    bodyStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                    bodyStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                    bodyStyle.EndUpdate();

                    for (int i = 0; i < ibex.Banks.Count; i++)
                    {
                        var bank = ibex.Banks[i];
                        var worksheet = workbook.Worksheets[i];

                        for (int j = 0; j < 6; j++)
                            worksheet.SetColumnWidth(j + 1, 13);

                        worksheet.Range["A13:F14"].CellStyle = bodyStyle;

                        if (bank.Cells != null && bank.Cells.Count > 0)
                            worksheet.Range[$"A17:F{bank.Cells.Count + 17}"].CellStyle = bodyStyle;


                        var resourceManager = CoreUtils.ResourceManager;
                        string GetString(string key)
                        {
                            return resourceManager.GetString(key, CultureInfo.CurrentCulture);
                        }

                        worksheet.Name = bank.Name;

                        worksheet["E1"].Text = "Date";
                        worksheet["F1"].Text = DateTime.Now.ToString("yyyy-MM-dd");
                        worksheet["A3"].Text = GetString("ExcelTitle");
                        worksheet["A3"].HorizontalAlignment = ExcelHAlign.HAlignCenter;                        
                        worksheet["A3"].CellStyle.Font.Size = 20;
                        worksheet["A3"].CellStyle.Font.Bold = true;
                        worksheet.Range["A3:F3"].Merge();

                        worksheet["A4"].Text = $"1. {GetString("BankInformation")}";
                        worksheet["A4"].CellStyle.Font.Size = 16;
                        worksheet["A4"].CellStyle.Font.Bold = true;
                        worksheet.Range["A4:F4"].Merge();

                        worksheet["A5"].Text = GetString("Site");
                        worksheet["A6"].Text = GetString("Name");
                        worksheet["B6"].Text = bank.Name;
                        worksheet["A7"].Text = GetString("Maker");
                        worksheet["B7"].Text = bank.Maker;
                        worksheet["A8"].Text = GetString("Model");
                        worksheet["B8"].Text = bank.Model;
                        worksheet["A9"].Text = GetString("Capa");
                        worksheet["B9"].Text = bank.Capa;
                        worksheet["A10"].Text = GetString("Date");
                        worksheet["B10"].Text = bank.Date.ToString("yyyy-MM-dd");

                        worksheet["A12"].Text = $"2. {GetString("CellAverageData")}";
                        worksheet["A12"].CellStyle.Font.Size = 16;
                        worksheet["A12"].CellStyle.Font.Bold = true;
                        worksheet.Range["A12:F12"].Merge();

                        worksheet["A13"].Text = GetString("Average");
                        worksheet["A13"].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet["A13"].VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range["A13:A14"].Merge();

                        worksheet["B13"].Text = GetString("Voltage"); 
                        worksheet["C13"].Text = GetString("Resistance");
                        worksheet["D13"].Text = GetString("Temperature");
                        worksheet["E13"].Text = GetString("Note");
                        worksheet.Range["B13:F13"].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range["E13:F13"].Merge();

                        if (bank.Cells.Count(r => r.Voltage.HasValue) > 0)
                            worksheet["B14"].Number = Math.Round(bank.Cells.Where(r => r.Voltage.HasValue).Average(r => r.Voltage.Value), 2);
                        if (bank.Cells.Count(r => r.Impedance.HasValue) > 0)
                            worksheet["C14"].Number = Math.Round(bank.Cells.Where(r => r.Impedance.HasValue).Average(r => r.Impedance.Value), 3);
                        if (bank.Cells.Count(r => r.Temperature.HasValue) > 0)
                            worksheet["D14"].Number = ConvertTemperature(Math.Round(bank.Cells.Where(r => r.Temperature.HasValue).Average(r => r.Temperature.Value), 1)) ?? 0;
                        worksheet.Range["E14:F14"].Merge();

                        worksheet["A16"].Text = $"3. {GetString("CellMeasurementData")}";
                        worksheet["A16"].CellStyle.Font.Size = 16;
                        worksheet["A16"].CellStyle.Font.Bold = true;
                        worksheet.Range["A16:F16"].Merge();

                        worksheet["A17"].Text = GetString("CellNumber");
                        worksheet["B17"].Text = GetString("Resistance");
                        worksheet["C17"].Text = GetString("ResistancePer");
                        worksheet["D17"].Text = GetString("Voltage");
                        worksheet["E17"].Text = GetString("Temperature");
                        worksheet["F17"].Text = GetString("Status");
                        worksheet.Range["A17:F17"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

                        var rowIndex = 18;

                        void WriteCell (string sheetName,double? value, Definitions.Alarm.Status alarmStatus, string format)
                        {
                            if (!value.HasValue)
                                return;
                            var excelCell = worksheet[sheetName];
                            excelCell.Number = value.Value;
                            if (!string.IsNullOrEmpty(format))
                                excelCell.NumberFormat = format;
                            switch (alarmStatus)
                            {
                                case Definitions.Alarm.Status.Normal:
                                    break;
                                case Definitions.Alarm.Status.Waring:
                                    excelCell.CellStyle.Font.RGBColor = Syncfusion.Drawing.ColorTranslator.FromHtml(Definitions.Alarm.WarningColorHex);
                                    break;
                                case Definitions.Alarm.Status.Fail:
                                    excelCell.CellStyle.Font.RGBColor = Syncfusion.Drawing.ColorTranslator.FromHtml(Definitions.Alarm.FailColorHex);
                                    break;
                                default:
                                    break;
                            }       
                        }

                        foreach (var cell in bank.Cells)
                        {
                            var cellAlarms = new List<Definitions.Alarm.Status>();
                            WriteCell($"A{rowIndex}", cell.Id, Definitions.Alarm.Status.Normal, string.Empty);                             
                            WriteCell($"B{rowIndex}", cell.Impedance, cell.ImpedanceAlarm, ".000");
                            WriteCell($"C{rowIndex}", cell.ImpedancePercentage, cell.ImpedanceAlarm, ".00");
                            WriteCell($"D{rowIndex}", cell.Voltage, cell.VoltageAlarm, ".00");
                            WriteCell($"E{rowIndex}", ConvertTemperature(cell.Temperature), cell.TemperatureAlarm, ".0");
                            cellAlarms.Add(cell.ImpedanceAlarm);
                            cellAlarms.Add(cell.VoltageAlarm);
                            cellAlarms.Add(cell.TemperatureAlarm);
                            var cellAlarm = cellAlarms.Max();
                            worksheet[$"F{rowIndex}"].Text = cellAlarm.ToString();
                            switch (cellAlarm)
                            {
                                case Definitions.Alarm.Status.Normal:
                                    break;
                                case Definitions.Alarm.Status.Waring:
                                    worksheet[$"F{rowIndex}"].CellStyle.Font.RGBColor = Syncfusion.Drawing.ColorTranslator.FromHtml(Definitions.Alarm.WarningColorHex);
                                    break;
                                case Definitions.Alarm.Status.Fail:
                                    worksheet[$"F{rowIndex}"].CellStyle.Font.RGBColor = Syncfusion.Drawing.ColorTranslator.FromHtml(Definitions.Alarm.FailColorHex);
                                    break;
                                default:
                                    break;
                            }
                            rowIndex++;
                        }
                    }
                    
                    var memoryStream = new MemoryStream();
                    workbook.SaveAs(memoryStream);
                    workbook.Close();
                    memoryStream.Position = 0;
                    var excel = DependencyService.Get<IExcel>();
                    if (excel != null)                        
                        excel.Write(memoryStream, $"{DateTime.Now.ToString("ibex-yyyyMMdd-HHmmss")}.xlsx");                        
                }
                return true;
            }
            catch (Exception ex)
            {
                CoreUtils.Toast($"Failed Make Excel {ex.Message}", Definitions.ToastLength.ShortLength);
                return false;
            }
            
        }
    }
}
