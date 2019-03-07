using Flurl;
using Flurl.Http;
using NamazVakti.Models;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamazVakti.Services
{
    public class NamazApiService
    {
        private readonly string baseUrl;

        public NamazApiService()
        {
            AppSettings = CrossSettings.Current;
            baseUrl = "https://ezanvakti.herokuapp.com";
        }

        public ISettings AppSettings { get; }

        internal DailyTimeData[] GetMonthlyPrayerTimes(string ilceKod)
        {
            var prayerTimeList = baseUrl
                          .AppendPathSegment("vakitler")
                          .SetQueryParams(new { ilce = ilceKod })
                          .GetJsonAsync<DailyTimeData[]> ()
                          .Result;

            return prayerTimeList;
        }
    }
}
