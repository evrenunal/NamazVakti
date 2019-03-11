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
           // AppSettings = CrossSettings.Current;
            baseUrl = "https://ezanvakti.herokuapp.com";
        }

       // public ISettings AppSettings { get; }

        internal DailyTimeData[] GetMonthlyPrayerTimes(string ilceKod)
        {
            var prayerTimeList = baseUrl
                          .AppendPathSegment("vakitler")
                          .SetQueryParams(new { ilce = ilceKod })
                          .GetJsonAsync<DailyTimeData[]> ()
                          .Result;

            return prayerTimeList;
        }

        internal Ulke[] GetCountries()
        {
            var countryList = baseUrl
                           .AppendPathSegment("ulkeler")
                           .GetJsonAsync<Ulke[]>()
                           .Result;

            return countryList;

        }

        internal Sehir[] GetCities(string ulkeKod)
        {
            var SehirList = baseUrl
                           .AppendPathSegment("sehirler")
                            .SetQueryParams(new { ulke = ulkeKod })
                           .GetJsonAsync<Sehir[]>()
                           .Result;

            return SehirList;
        }

        internal Ilce[] GetTowns(string sehirKod)
        {
            var townList = baseUrl
                           .AppendPathSegment("ilceler")
                            .SetQueryParams(new { sehir = sehirKod })
                           .GetJsonAsync<Ilce[]>()
                           .Result;

            return townList;
        }
    }
}
