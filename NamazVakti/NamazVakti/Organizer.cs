using NamazVakti.Models;
using NamazVakti.Services;
using Newtonsoft.Json;
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
        private string defaultIlce = "9541";

        public Organizer()
        {
            strg = new StorageService();
            AppSettings = CrossSettings.Current;
            nmzApi = new NamazApiService();
        }

        //  CrossLocalNotifications.Current.Show("title", count++.ToString());
        internal void RunCycle()
        {
            DateTime currentDtTime = DateTime.Now;
            var thisMonthId = currentDtTime.ToString("yy-MM");

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
                var lastMonthId = currentDtTime.AddMonths(-1).ToString("yy-MM");
                strg.DeleteFile(lastMonthId);
            }

            var todaysData = dailyTimes.FirstOrDefault(dd => dd.Date == currentDtTime.Date);

        }
    }
}