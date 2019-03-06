using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NamazVakti.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(NamazVakti.Droid.JobService))]
namespace NamazVakti.Droid
{
    public class JobService:IJobService
    {
        public JobService()
        {
            var currentContext = Android.App.Application.Context;
            var alarmIntent = new Intent(currentContext, typeof(BackgroundReceiver));
            pending = PendingIntent.GetBroadcast(currentContext, 0, alarmIntent, PendingIntentFlags.CancelCurrent);


            alarmManager = currentContext.GetSystemService(Context.AlarmService).JavaCast<AlarmManager>();
        }

        private PendingIntent pending;
        public AlarmManager alarmManager;

        public void StartJob()
        {
            alarmManager.SetRepeating(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + 3000, 2000, pending);
        }

        public void StopJob()
        {
            alarmManager.Cancel(pending);
        }
    }
}