using NamazVakti.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace NamazVakti
{
    public partial class App : Application
    {
        private MainPage mainPage;

        public App()
        {
            InitializeComponent();

             mainPage = new MainPage();
            MainPage = new NavigationPage(mainPage);
        }

        protected override void OnStart()
        {
            mainPage.viewModel.AlertMessage = "onstart";
        }

        protected override void OnSleep()
        {
            mainPage.viewModel.AlertMessage = "onsleep";
        }

        protected override void OnResume()
        {
            mainPage.viewModel.AlertMessage = "onresume";
        }
    }
}
