using NamazVakti.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NamazVakti.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage()
		{
            BindingContext = new SettingsViewModel();
			InitializeComponent();
		}
        

        private void CountriesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}