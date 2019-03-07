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
        private readonly Organizer organizer;
        private readonly IJobService deps;
        private bool runing;

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand StartService { get; private set; }
        public ICommand StopService { get; private set; }
        
        public MainViewModel()
        {
            organizer = new Organizer(this);         

            deps = DependencyService.Get<IJobService>();

            StartService = new Command(StartListener);
            StopService = new Command(StopListener);
        }

        private string _alertMessage;
        public string AlertMessage
        {
            get
            {
                return _alertMessage;
            }
            set
            {
                if (_alertMessage != value)
                {
                    _alertMessage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private void StopListener(object obj)
        {
            deps.StopJob();
            MessagingCenter.Unsubscribe<object, string>(this, "trigger");
            runing = false;
        }

        private void StartListener(object obj)
        {
            MessagingCenter
               .Subscribe<object, string>(this, "trigger", (s, e) =>
               {
                   Device.BeginInvokeOnMainThread(() =>
                   {
                       organizer.RunCycle();
                   });
               });

            deps.StartJob(5);

            runing = true;
        }
    }
}
