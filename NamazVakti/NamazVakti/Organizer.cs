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
        private static ISettings AppSettings;
        private string defaultIlce = "9225";

        public Organizer()
        {
            strg = new StorageService();
            AppSettings = CrossSettings.Current;
            nmzApi = new NamazApiService();
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
                dailyTimes = monthlyData.DayPrayerTimes.Select(s => s.Map()).ToArray();

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
                    AlertUser("Öğle", todaysData.Gunes.TimeOfDay - now);
                    break;
                case var now when todaysData.Ikindi.TimeOfDay < now && now < todaysData.Aksam.TimeOfDay:
                    AlertUser("İkindi", todaysData.Ikindi.TimeOfDay - now);
                    break;
                case var now when todaysData.Aksam.TimeOfDay < now && now < todaysData.Yatsi.TimeOfDay:
                    AlertUser("Akşam", todaysData.Aksam.TimeOfDay - now);
                    break;
                case var now when todaysData.Yatsi.TimeOfDay < now && now < todaysData.Imsak.TimeOfDay:
                    AlertUser("Yatsı", todaysData.Yatsi.TimeOfDay - now);
                    break;
            }

        }

        private void AlertUser(string prayKind, TimeSpan remainingTime)
        {
              CrossLocalNotifications.Current.Show("Namaz Vakti",
                  $"{ prayKind} vaktine kalan süre:{Environment.NewLine}{remainingTime.Hours}:{remainingTime.Minutes}"+
                  $"{Environment.NewLine}Bildirim Zamanı: {DateTime.Now.ToString("HH:mm")}"
                  );
        }
    }
}