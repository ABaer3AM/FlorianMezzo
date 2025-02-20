using Windows.System.Power;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace FlorianMezzo.Controls
{
    public partial class ResourceChecker
    {

        public partial async Task<Tuple<int, string>> FetchCpuUsage()
        {
            //Debug.WriteLine("Fetching Windows CPU");
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

        public static bool IsCharging()
        {
            var chargingStatus = PowerManager.PowerSupplyStatus;
            return chargingStatus == PowerSupplyStatus.Adequate || chargingStatus == PowerSupplyStatus.Inadequate;
        }

        public partial Tuple<int, string> IsFlorianRunning()
        {
            var processList = Process.GetProcesses();
            var florianProcessList = Process.GetProcessesByName("Flare");
           if (Process.GetProcessesByName("Flare").Length > 0) {
                return new Tuple<int, string>(1, "FLORIAN is running");
            }
            else
            {
                return new Tuple<int, string>(0, "FLORIAN is not running");
            }
        }

        public async partial Task<Tuple<int, string>> FetchLocation()
        {
            try
            {   // attempt to access the location hardware on the device
                var accessStatus = await Geolocator.RequestAccessAsync();

                // validate the app has location permissions
                if (accessStatus != GeolocationAccessStatus.Allowed)
                {
                    throw new Exception("Location access denied.");
                }

                // retrieve location data
                var geolocator = new Geolocator { DesiredAccuracy = PositionAccuracy.Default };
                Geoposition position = await geolocator.GetGeopositionAsync();

                // if the location data exists, digest the data
                string locationData = "";
                if (position != null)
                {
                    var coord = position.Coordinate;

                    // Build basic location data
                    locationData += $"\tAccuracy: {coord.Accuracy}\n";
                    locationData = "Current Data\n";

                    locationData += $"\tLatitude:\t{coord.Point.Position.Latitude}\n";
                    locationData += $"\tLongitude:\t{coord.Point.Position.Longitude}\n";
                    locationData += $"\tAltitude:\t{coord.Point.Position.Altitude}\n";
                    locationData += $"\tAccuracy:\t{coord.Accuracy}\n";

                    // Build heading value
                    if (coord.Heading.HasValue)
                    {
                        locationData += $"\tHeading:\t{coord.Heading.HasValue}\n";
                    }
                    else
                    {
                        locationData += $"\tHeading:\tNot Availible\n";
                    }

                    // Build speed value
                    if (coord.Speed.HasValue)
                    {
                        locationData += $"\tSpeed:\t{coord.Speed.Value}\n";
                    }
                    else
                    {
                        locationData += $"\tSpeed:\tNot Availible\n";
                    }

                    // return status
                    if (coord.Accuracy <= 50)
                    {   // running
                        return new Tuple<int, string>(1, locationData);
                    }
                    else if(coord.Accuracy < 100)
                    {   // warning
                        return new Tuple<int, string>(-1, locationData);
                    }
                    else
                    {   // critical
                        return new Tuple<int, string>(0, locationData);
                    }
                }
                else
                {
                    locationData = "NULL";
                    return new Tuple<int, string>(0, $"Location data was NULL");
                }
            }
            catch (Exception ex)
            {   // if attempt fails, message it to the user
                Debug.WriteLine($"Error fetching location: {ex.Message}");

                return new Tuple<int, string>(0, $"Location could not be accessed: {ex.Message}");
            }
        }
    }
}
