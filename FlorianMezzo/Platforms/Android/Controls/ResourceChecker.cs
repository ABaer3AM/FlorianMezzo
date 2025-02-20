using System.Diagnostics;

namespace FlorianMezzo.Controls
{
    public partial class ResourceChecker
    {

        public partial async Task<Tuple<int, string>> FetchCpuUsage()
        {
            await Task.Delay(500);
            Debug.WriteLine("Fetching Android CPU");
            return Tuple.Create(0, "No CPU Usage Fetching availible on Android");
        }

        public static bool IsCharging()
        {
            return false;
        }

        public partial Tuple<int, string> IsFlorianRunning()
        {
            Debug.WriteLine("Checking on Mezzo Process");
            return new Tuple<int, string>(0, "FLORIAN is not running");
        }

        public async partial Task<Tuple<int, string>> FetchLocation()
        {
            await Task.CompletedTask;
            return Tuple.Create(0, "Location not fetched");
        }
    }
}
