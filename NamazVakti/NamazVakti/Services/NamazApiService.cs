using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamazVakti.Services
{
    public class NamazApiService
    {
        public NamazApiService()
        {
            AppSettings = CrossSettings.Current;
        }

        public ISettings AppSettings { get; }

        internal object GetMonthlyPrayerTimes(string ilceKod)
        {
            throw new NotImplementedException();
        }
    }
}
