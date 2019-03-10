using NamazVakti.Models;
using NamazVakti.Services;
using Newtonsoft.Json;
using Plugin.LocalNotification;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NamazVakti
{
    internal class Organizer
    {
        private readonly StorageService strg;
        private readonly NamazApiService nmzApi;
        private readonly ILocalNotificationService notificationService;
        private readonly ViewModel.MainViewModel mainViewModel;
        private static ISettings AppSettings;
        private string defaultIlce = "9225";
        private PrayTimeKind prayTimeKind;
        const string fileName = "MonthlyPrayTimes";

        public Organizer(ViewModel.MainViewModel mainViewModel)
        {
            strg = new StorageService();
            AppSettings = CrossSettings.Current;
            nmzApi = new NamazApiService();
            notificationService = DependencyService.Get<ILocalNotificationService>();
            this.mainViewModel = mainViewModel;
        }

       
        internal void RunCycle()
        {
            var cycleStartTm = DateTime.Now;

            DailyTimes[] dailyTimes = null;
            var (success, file) = strg.GetFile(fileName);

            if (!success)
            {
                dailyTimes = PullMonthlyData();

            }
            else
            {
                dailyTimes = JsonConvert.DeserializeObject<DailyTimes[]>(file);

                if(!dailyTimes.Any(dt =>dt.Date> cycleStartTm.Date))
                    dailyTimes = PullMonthlyData();
            }

            var todaysData = dailyTimes.FirstOrDefault(dd => dd.Date == cycleStartTm.Date);
            var tomorrowData = dailyTimes.FirstOrDefault(dd => dd.Date == cycleStartTm.Date.AddDays(1));

            if (DateTime.Now.Date > cycleStartTm.Date) RunCycle();//if the day passed since the method started
             
            switch (DateTime.Now.TimeOfDay)
            {               
                case var now when todaysData.Imsak.TimeOfDay < now && now < todaysData.Gunes.TimeOfDay:
                    if (prayTimeKind != PrayTimeKind.Sabah)
                    {
                        mainViewModel.PrayerPerformed = false;
                        prayTimeKind = PrayTimeKind.Sabah;
                    }                   
                    mainViewModel.PrayerTimeEndline = todaysData.Gunes;
                    break;
                case var now when todaysData.Ogle.TimeOfDay < now && now < todaysData.Ikindi.TimeOfDay:
                    if (prayTimeKind != PrayTimeKind.Ogle)
                    {
                        mainViewModel.PrayerPerformed = false;
                        prayTimeKind = PrayTimeKind.Ogle;
                    }                    
                    mainViewModel.PrayerTimeEndline = todaysData.Ikindi;
                    break;                   
                case var now when todaysData.Ikindi.TimeOfDay < now && now < todaysData.Aksam.TimeOfDay:
                    if (prayTimeKind != PrayTimeKind.Ikindi)
                    {
                        mainViewModel.PrayerPerformed = false;
                        prayTimeKind = PrayTimeKind.Ikindi;
                    }                    
                    mainViewModel.PrayerTimeEndline = todaysData.Aksam;
                    break;
                case var now when todaysData.Aksam.TimeOfDay < now && now < todaysData.Yatsi.TimeOfDay:
                    if (prayTimeKind != PrayTimeKind.Aksam)
                    {
                        mainViewModel.PrayerPerformed = false;
                        prayTimeKind = PrayTimeKind.Aksam;
                    }                    
                    mainViewModel.PrayerTimeEndline = todaysData.Yatsi;
                    break;
                case var now when todaysData.Yatsi.TimeOfDay < now || now < tomorrowData.Imsak.TimeOfDay:
                    if (prayTimeKind != PrayTimeKind.Yatsi)
                    {
                        mainViewModel.PrayerPerformed = false;
                        prayTimeKind = PrayTimeKind.Yatsi;
                    }
                    mainViewModel.PrayerTimeEndline = tomorrowData.Imsak;
                    break;

                default: return;
            }

           var remainingTime = mainViewModel.PrayerTimeEndline - DateTime.Now;
            if (mainViewModel.AlertOpen && !mainViewModel.PrayerPerformed)
                AlertUser(prayTimeKind, remainingTime);

            mainViewModel.PrayerTimeKind = prayTimeKind.Stringify();            
        }

        private DailyTimes[] PullMonthlyData()
        {
            DailyTimes[] dailyTimes;
            var ilceKod = AppSettings.GetValueOrDefault(nameof(LocationParams.ilce), defaultIlce);
            var monthlyData = nmzApi.GetMonthlyPrayerTimes(ilceKod);
            dailyTimes = monthlyData.Select(s => s.Map()).ToArray();

            var fileToSave = JsonConvert.SerializeObject(dailyTimes);

            strg.SaveFile(fileToSave, fileName);
            return dailyTimes;
        }

        private void AlertUser(PrayTimeKind prayKind, TimeSpan remainingTime)
        {
            var message =
                  $"{ prayKind.Stringify()} namazi vaktinin çıkması için kalan süre: {remainingTime.Hours}:{remainingTime.Minutes}" +
                  $"{Environment.NewLine}Bildirim Zamanı: {DateTime.Now.ToString("HH:mm")}";
             
            var notification = new Plugin.LocalNotification.LocalNotification
            {
                NotificationId = 100,
                Title = "Namaz Vakti",
                Description = message,
                ReturningData = "Dummy data", // Returning data when tapped on notification.
              //  NotifyTime = DateTime.Now// Used for Scheduling local notification.
            };
            notificationService.Show(notification);
        }
    }
}