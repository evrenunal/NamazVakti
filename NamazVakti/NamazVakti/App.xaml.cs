using NamazVakti.Views;
using Plugin.LocalNotification;
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

            // Local Notification tap event listener
            MessagingCenter.Instance.Subscribe<LocalNotificationTappedEvent>(this,
                typeof(LocalNotificationTappedEvent).FullName, OnLocalNotificationTapped);

            mainPage = new MainPage();
            MainPage = new NavigationPage(mainPage);
        }

        private void OnLocalNotificationTapped(LocalNotificationTappedEvent obj)
        {
           
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
           
        }
    }
}
