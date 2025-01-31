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
    }
}
