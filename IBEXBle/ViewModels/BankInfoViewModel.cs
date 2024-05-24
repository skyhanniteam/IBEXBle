using IBEXBle.Core;
using IBEXBle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IBEXBle.ViewModels
{
    public class BankInfoViewModel : BaseViewModel
    {
        private BankModel bank;
        public BankModel Bank
        {
            get => bank;
            set => SetProperty(ref bank, value);
        }

        public ICommand Close => new Command(async () =>
        {
            IsBusy = true;
            await Application.Current.MainPage.Navigation.PopModalAsync();
            IsBusy = false;
        });
    }
}
