using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.LocalNotification.Platform.Droid;
using Android.Content;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;

namespace NamazVakti.Droid
{
    [Activity(Label = "NamazVakti", Icon = "@drawable/mosque", Theme = "@style/MainTheme", LaunchMode =LaunchMode.SingleInstance,  MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            //AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;            
            //TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            LocalNotificationService.NotificationIconId = Resource.Drawable.mosque;
            LocalNotificationService.NotifyNotificationTapped(Intent);
            LoadApplication(new App());
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Crashes.TrackError((Exception)e.ExceptionObject);
        }

        private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
           
            Crashes.TrackError(e.Exception);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnNewIntent(Intent intent)
        {
            LocalNotificationService.NotifyNotificationTapped(intent);
            base.OnNewIntent(intent);
        }
    }
}