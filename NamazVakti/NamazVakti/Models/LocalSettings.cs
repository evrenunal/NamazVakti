using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NamazVakti.Models
{
    public class LocalSettings
    {
        public int RunIntervalInMinutes { get; internal set; }

       public static LocalSettings GetFromProperties(LocalSettings defaults)
        {

            var properties = Application.Current.Properties;

            int runIntervalInMinutes =  (properties.ContainsKey(nameof(LocalSettings.RunIntervalInMinutes)))
                ?(int)properties[nameof(LocalSettings.RunIntervalInMinutes)]
                : defaults.RunIntervalInMinutes;
            
            return new LocalSettings
            {
                RunIntervalInMinutes = runIntervalInMinutes
            };
        }

        public static void SaveSettings(LocalSettings settings)
        {
            Application.Current.Properties[nameof(settings.RunIntervalInMinutes)] = settings.RunIntervalInMinutes;
        }
    }



}
