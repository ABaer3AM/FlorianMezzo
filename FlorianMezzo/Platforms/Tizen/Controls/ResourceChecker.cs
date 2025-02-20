using System.Diagnostics;

namespace FlorianMezzo.Controls
{
    public partial class ResourceChecker
    {

        public partial async Task<Tuple<int, string>> FetchCpuUsage()
        {
            await Task.Delay(500);
            Debug.WriteLine("Fetching Miscellaneous CPU");
            return Tuple.Create(0, "No CPU Usage Fetching availible on Miscellaneous");
        }

        public static bool IsCharging()
        {
            return false;
        }

        public partial int IsFlorianRunning()
        {
            Debug.WriteLine("Checking on Mezzo Process");
            return 0;
        }

        public async partial Task<Tuple<int, string>> FetchLocation()
        {
            await Task.CompletedTask;
            return Tuple.Create(0, "Location not fetched");
        }
    }
}
