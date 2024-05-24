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
	public partial class CellChartPage : ContentPage
	{
		public CellChartPage()
		{
			InitializeComponent ();
		}

        public CellChartPage(int bankId, ObservableCollection<CellModel> cells) : this()
        {
            var viewModel = this.BindingContext as CellChartViewModel;
            viewModel.BankId = bankId;
            viewModel.Cells = cells;
        }
    }
}