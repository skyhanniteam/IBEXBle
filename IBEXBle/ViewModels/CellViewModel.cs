using IBEXBle.Core;
using IBEXBle.Models;
using IBEXBle.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IBEXBle.ViewModels
{
    public class CellViewModel : BaseViewModel
    {
        private int bankId;
        public int BankId
        {
            get => bankId;
            set => SetProperty(ref bankId, value);
        }

        private ObservableCollection<CellModel> cells;
        public ObservableCollection<CellModel> Cells
        {
            get => cells;
            set => SetProperty(ref cells, value);
        }        

        public ICommand Close => new Command(async () =>
        {
            IsBusy = true;
            await Application.Current.MainPage.Navigation.PopModalAsync();
            IsBusy = false;
        });
    }
}
