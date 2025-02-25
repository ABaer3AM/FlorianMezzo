using System.Diagnostics;

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

        public async partial Task<Tuple<int, string>> IsFlorianRunning()
        {
            await Task.Delay(500);
            Debug.WriteLine("Checking on Mezzo Process");
            return Tuple.Create(0, "FLORIAN is not running");
        }

        public async partial Task<Tuple<int, string>> FetchLocation()
        {
            await Task.CompletedTask;
            return Tuple.Create(0, "Location not fetched");
        }
    }
}
