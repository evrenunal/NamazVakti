using System;
using System.Collections.Generic;
using System.Text;

namespace NamazVakti.Models
{
    public class MonthlyPrayerTimes
    {
        public DailyTime[] DayPrayerTimes { get; set; }
    }


    //public class Rootobject
    //{
    //    public Class1[] Property1 { get; set; }
    //}

    public class DailyTime
    {
        public string Aksam { get; set; }
        public string AyinSekliURL { get; set; }
        public string Gunes { get; set; }
        public string GunesBatis { get; set; }
        public string GunesDogus { get; set; }
        public string HicriTarihKisa { get; set; }
        public string HicriTarihUzun { get; set; }
        public string Ikindi { get; set; }
        public string Imsak { get; set; }
        public string KibleSaati { get; set; }
        public string MiladiTarihKisa { get; set; }
        public string MiladiTarihKisaIso8601 { get; set; }
        public string MiladiTarihUzun { get; set; }
        public DateTime MiladiTarihUzunIso8601 { get; set; }
        public string Ogle { get; set; }
        public string Yatsi { get; set; }
    }

}
