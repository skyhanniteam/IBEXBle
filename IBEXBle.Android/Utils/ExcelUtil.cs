using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using IBEXBle.Core;
using IBEXBle.DependencyInterface;
using IBEXBle.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(IBEXBle.Droid.Utils.ExcelDownloadUtil))]
namespace IBEXBle.Droid.Utils
{
    public class ExcelDownloadUtil : IExcel
    {
        private string excelDirectoryPath = Path.Combine(Android.App.Application.Context.GetExternalFilesDir(null).ToString(), "ibex");

        private ResourceManager resourceManager = CoreUtils.ResourceManager;

        private string GetString(string key)
        {
            return resourceManager.GetString(key, CultureInfo.CurrentCulture);
        }

        public bool DownloadMSExcel()
        {
            var context = MainActivity.Instance;
            if (context == null)
                return false;

            var intent = new Intent(Intent.ActionView);
            intent.SetData(Android.Net.Uri.Parse("market://details?id=com.microsoft.office.excel"));
            try
            {
                context.StartActivity(intent);
                return true;
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() => Xamarin.Forms.Application.Current.MainPage.DisplayAlert(string.Empty, $"{GetString("InstallExcel")} - {ex.Message}", "ok"));
                return false;
            }
        }

        public bool Write(Stream stream, string fileName)
        {
            try
            {                
                if (!Directory.Exists(excelDirectoryPath))
                    Directory.CreateDirectory(excelDirectoryPath);

                File.Delete(Path.Combine(excelDirectoryPath, fileName));                
                using (var fileStream = new FileStream(Path.Combine(excelDirectoryPath, fileName), FileMode.CreateNew, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }
                Device.BeginInvokeOnMainThread(() => Xamarin.Forms.Application.Current.MainPage.DisplayAlert(string.Empty, GetString("Saved"), "ok"));
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() => CoreUtils.Toast($"faild {ex.Message}", Definitions.ToastLength.ShortLength));
                return false;
            }

            return true;
        }


        public bool Shared(List<string> fileNames)
        {
            if (fileNames == null || fileNames.Count == 0)
                return false;
            var context = MainActivity.Instance;
            if (context == null)
                return false;

            Intent intent = null;

            try
            {
                if (fileNames.Count > 1)
                {
                    intent = new Intent(Intent.ActionSendMultiple);
                    var uriList = new List<IParcelable>();
                    foreach (var item in fileNames)
                        uriList.Add(FileProvider.GetUriForFile(context, "com.waton.IBEXBle.fileProvider", new Java.IO.File(Path.Combine(excelDirectoryPath, item))));
                    intent.PutParcelableArrayListExtra(Intent.ExtraStream, uriList);
                }
                else
                {
                    var uri = FileProvider.GetUriForFile(context, "com.waton.IBEXBle.fileProvider", new Java.IO.File(Path.Combine(excelDirectoryPath, fileNames[0])));
                    intent = new Intent(Intent.ActionSend);
                    intent.PutExtra(Intent.ExtraStream, uri);
                }

                intent.SetType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                context.StartActivity(Intent.CreateChooser(intent, "Share files"));
                return true;
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() => CoreUtils.Toast($"Can't shared {ex.Message}", Definitions.ToastLength.ShortLength));
                return false;
            }
        }

        public List<ExcelModel> Select()
        {            
            if (!Directory.Exists(excelDirectoryPath))
                Directory.CreateDirectory(excelDirectoryPath);

            var fileNames = Directory.GetFiles(excelDirectoryPath).OrderByDescending(r => r);
            var excelModels = new List<ExcelModel>();
            foreach (var fileName in fileNames)
            {
                var fileInfo = new FileInfo(fileName);
                excelModels.Add(new ExcelModel
                {
                    FileName = fileInfo.Name
                });
            }
            return excelModels;
        }

        public bool Open(string fileName)
        {
            try
            {
                var context = MainActivity.Instance;
                if (context == null)
                    return false;
                
                var intent = new Intent(Intent.ActionView);                
                var uri = FileProvider.GetUriForFile(context, "com.waton.IBEXBle.fileProvider", new Java.IO.File(excelDirectoryPath, fileName));
                intent.SetDataAndType(uri, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                intent.SetFlags(ActivityFlags.GrantReadUriPermission);
                context.StartActivity(intent);
            }
            catch(Exception ex)
            {   
                DownloadMSExcel();
                return false;
            }
            return true;
        }

        public bool Delete(List<string> fileNames)
        {
            if (fileNames == null || fileNames.Count == 0)
                return false;            
            try
            {
                foreach (var fileName in fileNames)
                    File.Delete(Path.Combine(excelDirectoryPath, fileName));            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() => Xamarin.Forms.Application.Current.MainPage.DisplayAlert(string.Empty, $"Failed - {ex.Message}", "ok"));
                return false;
            }            
            return true;
        }
    }
}