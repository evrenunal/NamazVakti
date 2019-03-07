using NamazVakti.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NamazVakti
{
    public static class Extensions
    {
        public static DailyTimes Map(this DailyTimeData timeData)
        {
            return new DailyTimes
            {
                Date = timeData.MiladiTarihUzunIso8601,
                Imsak = ParseTime(timeData.Imsak),
                Gunes = ParseTime(timeData.Gunes),
                Ogle = ParseTime(timeData.Ogle),
                Ikindi = ParseTime(timeData.Ikindi),
                Aksam = ParseTime(timeData.Aksam),
                Yatsi = ParseTime(timeData.Yatsi)
            };

            DateTime ParseTime(string tmData)
            {                
                var time = tmData.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                return timeData.MiladiTarihUzunIso8601.AddHours(time[0]).AddMinutes(time[1]);
            }
        }
    }
}
