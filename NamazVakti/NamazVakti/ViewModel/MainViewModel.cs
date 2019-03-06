using NamazVakti.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace NamazVakti.ViewModel
{
    public class MainViewModel
    {
        private readonly Organizer organizer;
        private readonly IJobService deps;
        private bool runing;

        public ICommand StartService { get; private set; }
        public ICommand StopService { get; private set; }

        public MainViewModel()
        {
            organizer = new Organizer();          

            deps = DependencyService.Get<IJobService>();

            StartService = new Command(StartListener);
            StopService = new Command(StopListener);
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

            deps.StartJob();

            runing = true;
        }
    }
}
