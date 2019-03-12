using NamazVakti.Models;
using NamazVakti.Views;
using Plugin.LocalNotification;
using System;
using Xamarin.Essentials;
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

        public void StartTimer()
        {           
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {

                if (mainPage.viewModel.PrayerTimeKind != PrayTimeKind.None)
                {
                    var remainingTime = mainPage.viewModel.PrayerTimeEndline - DateTime.Now;
                    mainPage.viewModel.RemainingTime = remainingTime.ToString(@"hh\:mm");
                    mainPage.viewModel.PrayerTimeText = mainPage.viewModel.PrayerTimeKind.Stringify()
                    + " vaktinin çıkmasına kalan süre:";
                }       

                return true;
            });
        }

        protected override void OnStart()
        {
            //LocalSettings settings =  LocalSettings.GetCurrent();
            
            //mainPage.viewModel.StartListener(settings.Interval);
            StartTimer();
        }
        

        protected override void OnSleep()
        {
            
        }

        protected override void OnResume()
        {
            
        }
    }
}
