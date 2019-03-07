using NamazVakti.Models;
using NamazVakti.Services;
using Newtonsoft.Json;
using Plugin.LocalNotifications;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Linq;
using Xamarin.Essentials;

namespace NamazVakti
{
    internal class Organizer
    {
        private readonly StorageService strg;
        private readonly NamazApiService nmzApi;
        private readonly ViewModel.MainViewModel mainViewModel;
        private static ISettings AppSettings;
        private string defaultIlce = "9225";

        public Organizer(ViewModel.MainViewModel mainViewModel)
        {
            strg = new StorageService();
            AppSettings = CrossSettings.Current;
            nmzApi = new NamazApiService();
            this.mainViewModel = mainViewModel;
        }

       
        internal void RunCycle()
        {
            var cyckleStartTm = DateTime.Now;

            var thisMonthId = cyckleStartTm.ToString("yy-MM");

            var (success, file) = strg.GetFile(thisMonthId);            
            DailyTimes[] dailyTimes = null;

            if (success)
            {
                dailyTimes = JsonConvert.DeserializeObject<DailyTimes[]>(file);
            }
            else
            {
                var ilceKod = AppSettings.GetValueOrDefault(nameof(LocationParams.ilce), defaultIlce);
                var monthlyData = nmzApi.GetMonthlyPrayerTimes(ilceKod);
                dailyTimes = monthlyData.Select(s => s.Map()).ToArray();

                var fileToSave = JsonConvert.SerializeObject(dailyTimes);

                strg.SaveFile(fileToSave, thisMonthId);
                var lastMonthId = cyckleStartTm.AddMonths(-1).ToString("yy-MM");
                strg.DeleteFile(lastMonthId);
            }

            var todaysData = dailyTimes.FirstOrDefault(dd => dd.Date == cyckleStartTm.Date);

            if (DateTime.Now.Date > cyckleStartTm) RunCycle();

            switch (DateTime.Now.TimeOfDay)
            {
               
                case var now when todaysData.Imsak.TimeOfDay < now && now < todaysData.Gunes.TimeOfDay:
                    AlertUser("Sabah", todaysData.Gunes.TimeOfDay - now);
                    break;
                case var now when todaysData.Ogle.TimeOfDay < now && now < todaysData.Ikindi.TimeOfDay:
                    AlertUser("Öğle", todaysData.Ikindi.TimeOfDay - now);
                    break;
                case var now when todaysData.Ikindi.TimeOfDay < now && now < todaysData.Aksam.TimeOfDay:
                    AlertUser("İkindi", todaysData.Aksam.TimeOfDay - now);
                    break;
                case var now when todaysData.Aksam.TimeOfDay < now && now < todaysData.Yatsi.TimeOfDay:
                    AlertUser("Akşam", todaysData.Yatsi.TimeOfDay - now);
                    break;
                case var now when todaysData.Yatsi.TimeOfDay < now || now < todaysData.Imsak.TimeOfDay: // this case will be fixed to take next days time
                    AlertUser("Yatsı", TimeSpan.FromHours(24) + todaysData.Imsak.TimeOfDay - now  );
                    break;

            }
        }

        private void AlertUser(string prayKind, TimeSpan remainingTime)
        {
            var message =
                  $"{ prayKind} namazi vaktinin çıkması için kalan süre: {remainingTime.Hours}:{remainingTime.Minutes}" +
                  $"{Environment.NewLine}Bildirim Zamanı: {DateTime.Now.ToString("HH:mm")}";

            CrossLocalNotifications.Current.Show("Namaz Vakti",message);

            mainViewModel.AlertMessage = message;
        }
    }
}