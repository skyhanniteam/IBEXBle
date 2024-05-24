using IBEXBle.Core;
using IBEXBle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IBEXBle.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BankInfoPage : ContentPage
    {
		public BankInfoPage ()
		{
			InitializeComponent ();            
		}

        public BankInfoPage(BankModel bankModel) : this()
        {
            (this.BindingContext as ViewModels.BankInfoViewModel).Bank = bankModel;
        }
    }
}