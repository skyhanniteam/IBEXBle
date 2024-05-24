using IBEXBle.Core;
using IBEXBle.DependencyInterface;
using IBEXBle.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IBEXBle.ViewModels
{
    public class SavedDataViewModel : BaseViewModel
    {
        public SavedDataViewModel()
        {
            Title = "Saved Data";
            SelectExcel();
        }

        private bool isSharing;
        public bool IsSharing
        {
            get => isSharing;
            set => SetProperty(ref isSharing, value);
        }

        private bool isDeleting;
        public bool IsDeleting
        {
            get => isDeleting;
            set => SetProperty(ref isDeleting, value);
        }

        private ObservableCollection<ExcelModel> excelModel;
        public ObservableCollection<ExcelModel> ExcelModels
        {
            get => excelModel;
            set => SetProperty(ref excelModel, value);
        }

        private void SelectExcel()
        {
            IsBusy = true;                        
            var excel = DependencyService.Get<IExcel>();
            if (excel != null)
                ExcelModels = new ObservableCollection<ExcelModel>(excel.Select());
            IsBusy = false;
        }

        public ICommand ExecuteExcel => new Command(async (excelModel) =>
        {
            IsBusy = true;
            await Task.Run(() =>
            {
                var excel = DependencyService.Get<IExcel>();
                if (excel != null)
                    excel.Open((excelModel as ExcelModel).FileName);
            });
            
            IsBusy = false;
        });

        public ICommand Shared => new Command(async () =>
        {
            IsSharing = true;
            IsBusy = true;
            await Task.Run(() =>
            {
                var fileNames = ExcelModels.Where(r => r.IsSelected).Select(r => r.FileName).ToList();

                if (fileNames.Count == 0)
                {
                    IsSharing = false;
                    IsBusy = false;
                    Device.BeginInvokeOnMainThread(() => CoreUtils.Toast("Select a data to share.", Definitions.ToastLength.ShortLength));
                    return;
                }

                var excel = DependencyService.Get<IExcel>();
                if (excel != null)
                    excel.Shared(fileNames);
            });
            IsSharing = false;
            IsBusy = false;
        });

        public ICommand Delete => new Command(async () =>
        {
            IsDeleting = true;
            IsBusy = true;

            var result = await Application.Current.MainPage.DisplayAlert(string.Empty, "Are you sure delete data?", "Ok", "Cancel");

            if(!result)
            {
                IsDeleting = false;
                IsBusy = false;
                return;
            }

            await Task.Run(() =>
            {
                var fileNames = ExcelModels.Where(r => r.IsSelected).Select(r => r.FileName).ToList();

                if (fileNames.Count == 0)
                {
                    Device.BeginInvokeOnMainThread(() => CoreUtils.Toast("Select a data to delete.", Definitions.ToastLength.ShortLength));
                    IsDeleting = false;
                    IsBusy = false;
                    return;
                }                

                var excel = DependencyService.Get<IExcel>();
                if (excel != null)
                    excel.Delete(fileNames);
                SelectExcel();
            });
            IsDeleting = false;
            IsBusy = false;
        });
    }
}
