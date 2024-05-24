using IBEXBle.Core;
using IBEXBle.Models;
using IBEXBle.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IBEXBle.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CellPage : ContentPage
	{     

        public CellPage ()
		{
			InitializeComponent ();
		}

        public CellPage(int bankdId, string bankName, ObservableCollection<CellModel> cells) : this()
        {
            var viewModel = this.BindingContext as CellViewModel;
            viewModel.Title = bankName;
            viewModel.BankId = bankdId;
            viewModel.Cells = cells;
        }                
    }
}