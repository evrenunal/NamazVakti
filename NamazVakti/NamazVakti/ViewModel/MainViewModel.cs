﻿using NamazVakti.Models;
using NamazVakti.Services;
using NamazVakti.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NamazVakti.ViewModel
{
    public class MainViewModel:INotifyPropertyChanged
    {
        internal readonly Organizer organizer;
        private readonly IJobService deps;
        public ICommand SettingsPage { get; private set; }

       
        private bool runing;

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public MainViewModel()
        {         
            organizer = new Organizer(this);         

            deps = DependencyService.Get<IJobService>();
            SettingsPage = new Command(async() => await OpenSettingsPage());


            ReStart();
         }

        internal void ReStart()
        {
            StopListener();
            var settings = LocalSettings.GetCurrent();

            StartListener(settings.Interval);         

            var ilceAdi = (settings.AbsolutePlace.Town.IlceAdi == settings.AbsolutePlace.City.SehirAdi
                ? "(Merkez)"
               : settings.AbsolutePlace.Town.IlceAdi);
            Location = settings.AbsolutePlace.City.SehirAdi + " > " + ilceAdi;
               
            AlertOpen = settings.AlertUser;
        }    

        private async Task OpenSettingsPage()
        {
            var ok =   await Organizer.CheckConnection2();
            if (!ok) return;

            var navigationPage = (NavigationPage)App.Current.MainPage;
            await navigationPage.PushAsync(new SettingsPage(this));
        }

        private string _location;
        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                if (_location != value)
                {
                    _location = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _kilindiSwEnabled;
        public bool KilindiSwEnabled
        {
            get
            {
                return _kilindiSwEnabled;
            }
            set
            {
                if (_kilindiSwEnabled != value)
                {
                    _kilindiSwEnabled = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public DateTime PrayerTimeEndline { get; set; }
        public PrayTimeKind PrayerTimeKind { get; set; }
        private string _prayerTimeText;
        public string PrayerTimeText
        {
            get
            {
                return _prayerTimeText;
            }
            set
            {
                if (_prayerTimeText != value)
                {
                    _prayerTimeText = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _remainingTime;
        public string RemainingTime
        {
            get
            {
                return _remainingTime;
            }
            set
            {
                if (_remainingTime != value)
                {
                    _remainingTime = value;
                    NotifyPropertyChanged();
                }
            }
        } 

        private bool _alertOpen;
        public bool AlertOpen
        {
            get
            {
                return _alertOpen;
            }
            set
            {
                if (_alertOpen != value)
                {
                    _alertOpen = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _prayerPerformed;
        public bool PrayerPerformed
        {
            get
            {
                return _prayerPerformed;
            }
            set
            {
                if (_prayerPerformed != value)
                {
                    _prayerPerformed = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private DailyTimes _timeTable;
        public DailyTimes TimeTable
        {
            get
            {
                return _timeTable;
            }
            set
            {
                if (_timeTable != value)
                {
                    _timeTable = value;
                    NotifyPropertyChanged();
                }
            }
        }

        internal void DeletePrayerTimes()
        {
            organizer.DeletePrayerTimes();
        }

        internal void StopListener()
        {
            deps.StopJob();
            MessagingCenter.Unsubscribe<object, string>(this, "trigger");
            runing = false;
        }

        internal void StartListener(int interval)
        {
            MessagingCenter
               .Subscribe<object, string>(this, "trigger", (s, e) =>
               {
                   Device.BeginInvokeOnMainThread(() =>
                   {
                       organizer.RunCycle();
                   });
               });

            deps.StartJob(interval);
           
            runing = true;
        }
    }
}
