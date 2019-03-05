using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NamazVakti
{
    public partial class MainPage : ContentPage
    {
        private readonly IJobService deps;

        public MainPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<object, string>(this, "trigger", (s, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                  //  CrossLocalNotifications.Current.Show("title", count++.ToString());
                });
            });

            deps = DependencyService.Get<IJobService>();
        }
    }
}
