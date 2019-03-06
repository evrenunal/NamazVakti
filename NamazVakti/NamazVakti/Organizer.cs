using NamazVakti.Models;
using NamazVakti.Services;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
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
           

            var thisMonthId = DateTime.Now.ToString("yy-MM");

            var (success,file) =   strg.GetFile(thisMonthId);

            if (success)
            {

            }
            else
            {
              var  ilceKod =  AppSettings.GetValueOrDefault(nameof(LocationParams.ilce), defaultIlce);

              var monthlyTimes = nmzApi.GetMonthlyPrayerTimes(ilceKod);
            }

        }
    }
}