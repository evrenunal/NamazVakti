using Flurl;
using Flurl.Http;
using NamazVakti.Models;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace NamazVakti.Services
{
    public class NamazApiService
    {
        private readonly string baseUrl;

        public NamazApiService()
        {           
            baseUrl = "https://ezanvakti.herokuapp.com";
        }

        internal  DailyTimeData[] GetMonthlyPrayerTimes(string ilceKod)
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

        private async void CheckInternetConnection()
        {
            while (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage. DisplayAlert("No Internet", "Lutfen internet Baglantısını açın", "OK");

                System.Threading.Thread.SpinWait(3);
            }
        }
    }
}
