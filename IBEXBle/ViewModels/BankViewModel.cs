using IBEXBle.Core;
using IBEXBle.DependencyInterface;
using IBEXBle.Models;
using IBEXBle.Views;
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
    public class BankViewModel : BaseViewModel
    {
        public BankViewModel()
        {            
            var iBex = IBex.Instance;            
            
            if (iBex.Banks == null || iBex.Banks.Count == 0)
            {                
                IsBusy = true;
                Title = "No Data";
                return;
            }
            

            Title = iBex.ReceivedTime.ToString("yyyy-MM-dd HH:mm:ss");

            Banks = new ObservableCollection<BankModel>();
            foreach (var bank in iBex.Banks)
            {
                Banks.Add(new BankModel
                {
                    Id = bank.Id,
                    Name = bank.Name,
                    AlarmStatus = bank.AlarmStatus
                });
            }
        }

        public ObservableCollection<BankModel> Banks { get; set; }

        private bool isWritingExcel;
        public bool IsWritingExcel
        {
            get => isWritingExcel;
            set => SetProperty(ref isWritingExcel, value);
        }

        public ICommand Excel => new Command(async() =>
        {
            IsBusy = IsWritingExcel = true;             
            await Task.Run(() =>
            {
                ExcelUtil.WriteIbex();
            });
            IsBusy = IsWritingExcel = false;
        });

        public ICommand BankInfo => new Command(async (bankModel) =>
        {
            IsBusy = true;

            var ibex = IBex.Instance;
            var parameterBankModel = new BankModel();
            if (ibex.Banks != null)            
                parameterBankModel = ibex.Banks.SingleOrDefault(bank => bank.Id == (bankModel as BankModel) .Id);
            await Application.Current.MainPage.Navigation.PushModalAsync(new BankInfoPage(parameterBankModel));
            IsBusy = false;
        });

        public ICommand CellInfo => new Command(async (bankModel) =>
        {
            IsBusy = true;
            var bankName = string.Empty;
            var cells = new ObservableCollection<CellModel>();
            var bankParameter = bankModel as BankModel;
            var bankId = -1;
            if (bankParameter != null)
            {
                bankName = bankParameter.Name;
                bankId = bankParameter.Id;
                cells = IBex.Instance.SelectCell(bankParameter.Id);
            }
            await Application.Current.MainPage.Navigation.PushModalAsync(new CellPage(bankId,bankName, cells));
            IsBusy = false;
        });

        public ICommand CellChart=> new Command(async (bankModel) =>
        {
            IsBusy = true;            
            var cells = new ObservableCollection<CellModel>();
            var bankParameter = bankModel as BankModel;
            var bankId = -1;
            if (bankParameter != null)
            {                
                bankId = bankParameter.Id;
                cells = IBex.Instance.SelectCell(bankParameter.Id);
            }
            await Application.Current.MainPage.Navigation.PushModalAsync(new CellChartPage(bankId, cells));
            IsBusy = false;
        });
    }
}