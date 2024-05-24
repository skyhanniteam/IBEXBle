using IBEXBle.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace IBEXBle
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#455a64"),
                BarTextColor = Color.FromHex("#ffc400")
            };

            //junho
            Core.TestUtil.MakeTestIBex();
        }

		protected override void OnStart ()
		{
            // Handle when your app starts
            Core.CoreUtils.CheckLocationPermission();
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
