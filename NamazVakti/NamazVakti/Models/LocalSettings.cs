using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NamazVakti.Models
{
    public class LocalSettings
    {
        public AbsolutePlace AbsolutePlace { get;  set; }
        public bool AlertUser { get; set; }
        public int Interval { get;  set; }

        public static LocalSettings GetCurrent()
        {

            var properties = Application.Current.Properties.ContainsKey("settings")
                ? Application.Current.Properties["settings"] as LocalSettings
                : new LocalSettings
                {
                    AbsolutePlace = new AbsolutePlace
                    {
                        City = new Sehir
                        {
                            SehirAdi= "ANTALYA",
                            SehirID="507"
                        },
                        Country = new Ulke
                        {
                            UlkeAdi = "TÜRKİYE",
                           UlkeID="2"
                        },
                        Town = new Ilce
                        {
                            IlceAdi= "ANTALYA",
                            IlceID = "9225"
                        }
                    },
                    AlertUser = true,
                    Interval = 5
                };

            return properties;
        }

        public static void SaveSettings(LocalSettings settings)
        {
            Application.Current.Properties["settings"] = settings;
        }
    }
}
