﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using NamazVakti.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(NamazVakti.iOS.JobService))]
namespace NamazVakti.iOS
{
    public class JobService : IJobService
    {
        public void StartJob(int jobIntervalInMinutes)
        {
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);
        }

        public void StopJob()
        {
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalNever);

        }
    }
}