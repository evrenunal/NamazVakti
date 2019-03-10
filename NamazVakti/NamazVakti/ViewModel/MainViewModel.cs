using NamazVakti.Models;
using NamazVakti.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace NamazVakti.ViewModel
{
    public class MainViewModel:INotifyPropertyChanged
    {
        internal readonly Organizer organizer;
        private readonly IJobService deps;

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

          
          
        }

        private double _interval;
        public double Interval
        {
            get
            {
                return _interval;
            }
            set
            {
                if (_interval != value)
                {
                    _interval = value;
                    StopListener();
                    StartListener((int)_interval);
                    var currentSettings = LocalSettings.GetFromProperties(new LocalSettings());
                    if(currentSettings.RunIntervalInMinutes != (int)_interval)
                    {
                        currentSettings.RunIntervalInMinutes = (int)_interval;
                        LocalSettings.SaveSettings(currentSettings);
                    }
                    NotifyPropertyChanged();
                }
            }
        }

        private string _prayerTimeKind;
        public string PrayerTimeKind
        {
            get
            {
                return _prayerTimeKind;
            }
            set
            {
                if (_prayerTimeKind != value)
                {
                    _prayerTimeKind = value;
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
       
        public DateTime PrayerTimeEndline { get; set; }

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
