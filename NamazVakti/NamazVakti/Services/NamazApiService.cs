using Flurl;
using Flurl.Http;
using NamazVakti.Models;
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

        internal MonthlyPrayerTimes GetMonthlyPrayerTimes(string ilceKod)
        {
            var person = baseUrl
                          .AppendPathSegment("person")
                          .SetQueryParams(new { a = 1, b = 2 })
                          .GetJsonAsync<MonthlyPrayerTimes>()
                          .Result;

            return person;
        }
    }
}
