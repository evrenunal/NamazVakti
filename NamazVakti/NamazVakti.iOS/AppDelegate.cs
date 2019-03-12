using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Microsoft.AppCenter.Crashes;
using ObjCRuntime;
using Plugin.LocalNotification.Platform.iOS;
using UIKit;
using Xamarin.Forms;

namespace NamazVakti.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            LocalNotificationService.Init();
            return base.FinishedLaunching(app, options);
        }

        private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Crashes.TrackError(e.Exception, new Dictionary<string, string> { ["location"] = "iOS" });
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Crashes.TrackError((Exception)e.ExceptionObject, new Dictionary<string, string> { ["location"] = "iOS" });
        }

        public override void PerformFetch(UIApplication application,  Action<UIBackgroundFetchResult> completionHandler)
        {
            //https://xamarinhelp.com/xamarin-background-tasks/

            // buraya interval suresi kontrol koy
            MessagingCenter.Send<object, string>(this, "trigger", "Modify from IOS");


            base.PerformFetch(application, completionHandler);
        }

        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {
            base.ReceivedLocalNotification(application, notification);

            if (UIApplication.SharedApplication.ApplicationState != UIApplicationState.Active)
            {
                LocalNotificationService.NotifyNotificationTapped(notification);
            }
        }
    }
}
