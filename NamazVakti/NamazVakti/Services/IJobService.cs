using System;
using System.Collections.Generic;
using System.Text;

namespace NamazVakti.Services
{
    public interface IJobService
    {
        void StartJob(int jobIntervalInMinutes);
        void StopJob();
    }
}
