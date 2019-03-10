using NamazVakti.Models;
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

        public void StartTimer()
        {
           
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                try
                {
                    var remainingTime=mainPage.viewModel.PrayerTimeEndline - DateTime.Now;
                    mainPage.viewModel.RemainingTime = remainingTime.ToString(@"hh\:mm"); 
                }
                catch 
                {                   
                    mainPage.viewModel.RemainingTime = TimeSpan.MaxValue.ToString(@"hh\:mm"); 
                }
                return true;
            });
        }

        protected override void OnStart()
        {
            // mainPage.viewModel.organizer.RunCycle();

            var defaultSettings = new LocalSettings
            {
                RunIntervalInMinutes=5
            };

            LocalSettings settings =  LocalSettings.GetFromProperties(defaultSettings);
            
            mainPage.viewModel.StartListener(settings.RunIntervalInMinutes);
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
