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
using Xamarin.Forms;

namespace NamazVakti.Droid
{
    [BroadcastReceiver]
    public class BackgroundReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            //PowerManager pm = (PowerManager)context.GetSystemService(Context.PowerService);
            //PowerManager.WakeLock wakeLock = pm.NewWakeLock(WakeLockFlags.Full, nameof(PeriodicService));
            //wakeLock.Acquire();

            MessagingCenter.Send<object, string>(this, "trigger", "Modify from Android BroadcastReceiver");
            
            //  wakeLock.Release();
        }
    }
}