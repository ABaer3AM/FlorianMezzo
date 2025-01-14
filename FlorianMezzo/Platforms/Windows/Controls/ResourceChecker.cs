using System.Diagnostics;
using FlorianMezzo.Controls;

namespace FlorianMezzo.Controls
{
    public partial class ResourceChecker
    {

        public partial async Task<Tuple<int, string>> FetchCpuUsage()
        {
            Debug.WriteLine("Fetching Windows CPU");
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue(); // First call returns 0, so we need a delay
            await Task.Delay(500); // Wait for half a second

            var cpuUsage = cpuCounter.NextValue();
            if (cpuUsage >= 100)  // 100% usage for 10+ minutes
            {
                return Tuple.Create(-1, $"{cpuUsage:F1}% usage");
            }
            else
            {
                return Tuple.Create(1, $"{cpuUsage:F1}% usage");
            }
        }
    }
}
