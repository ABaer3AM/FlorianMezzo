﻿using System.Diagnostics;

namespace FlorianMezzo.Controls
{
    public partial class ResourceChecker
    {

        public partial async Task<Tuple<int, string>> FetchCpuUsage()
        {
            await Task.Delay(500);
            Debug.WriteLine("Fetching IOS CPU");
            return Tuple.Create(0, "No CPU Usage Fetching availible on IOS");
        }

        public static bool IsCharging()
        {
            return false;
        }
    }
}
