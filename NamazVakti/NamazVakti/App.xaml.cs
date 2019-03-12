using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using NamazVakti.Models;
using NamazVakti.Views;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
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
            Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(1), () =>
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

        protected async override  void OnStart()
        {
            AppCenter.Start("ios=6e72f21c-f13e-4204-8a94-43f7c64aa766;" + "android=a78c0c5d-7cee-4102-8c21-83929ee9e60f", typeof(Analytics), typeof(Crashes));           

            Analytics.TrackEvent("App Started",
                new Dictionary<string, string>
                {
                    ["date"] = DateTime.Now.ToString()
                });
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
