using NamazVakti.Models;
using NamazVakti.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NamazVakti.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
        private SettingsViewModel viewModel;
       

        public SettingsPage(MainViewModel mainViewModel)
		{
            viewModel = new SettingsViewModel(mainViewModel);
            BindingContext = viewModel;

            InitializeComponent();
        }        

        private void CountriesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            viewModel.CountryIndexChanged(e);
        }

        private void CitiesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            viewModel.CityIndexChanged(e);
        }        

        private static async Task CheckConnectionAsync()
        {
            while (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("No Internet", "Lutfen internet Baglantısını açın", "OK");
            }
        }

        private async void CountriesPicker_Focused(object sender, FocusEventArgs e)
        {
            //var picker = sender as Picker;
            //if (!picker.Items.Any())
            //{
            //  await  CheckConnectionAsync();
            //}
            //var settings = LocalSettings.GetCurrent();

            //viewModel.LoadCurrents(settings);
        }

        private async void CitiesPicker_Focused(object sender, FocusEventArgs e)
        {
            //var picker = sender as Picker;
            //if (!picker.Items.Any())
            //{
            //  await  CheckConnectionAsync();
            //}
        }

        private async void TownsPicker_Focused(object sender, FocusEventArgs e)
        {
            var picker = sender as Picker;
            if (!picker.Items.Any())
            {
                await CheckConnectionAsync();
            }
        }
    }
}