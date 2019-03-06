using NamazVakti.Services;
using System;
using Xamarin.Essentials;

namespace NamazVakti
{
    internal class Organizer
    {
        private readonly StorageService strg;

        public Organizer()
        {
            strg = new StorageService();
        }

        //  CrossLocalNotifications.Current.Show("title", count++.ToString());
        internal void RunCycle()
        {
            var mainDir = FileSystem.AppDataDirectory;

            
        }
    }
}