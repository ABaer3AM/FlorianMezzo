using System.Runtime.InteropServices;
using Microsoft.Maui.Devices;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using FlorianMezzo.Controls.db;

namespace FlorianMezzo.Controls
{
    public partial class ResourceChecker
    {

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private class MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;

            public MEMORYSTATUSEX()
            {
                this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);


        public ResourceChecker()
        {
        }

        public async Task<Tuple<int, string>> FetchBattery()
        {
            HardwareResourcesData datapacket = new();
            double battery = Math.Round(Battery.ChargeLevel * 100, 1);
            await Task.Yield();

            if (battery > 30.0)
            {                        // Good     = 31+  
                return Tuple.Create(1, battery.ToString() + "%");
            }
            else if (battery > 15)
            {                    // Warning  = 30-16
                return Tuple.Create(-1, battery.ToString() + "%");
            }
            else
            {                                      // Critical = 15-0
                return Tuple.Create(0, battery.ToString() + "%");
            }
        }

        public async Task<Tuple<int, string>> FetchDiskSpace()
        {
            await Task.Yield();
            try
            {
                // Get the drive information for the specified drive
                DriveInfo drive = new DriveInfo("C:");

                if (drive.IsReady)
                {
                    long availableSpace = drive.AvailableFreeSpace; // In bytes
                    long totalSpace = drive.TotalSize;

                    // Convert to GB for readability
                    double availableSpaceGB = availableSpace / (1024.0 * 1024 * 1024);
                    double totalSpaceGB = totalSpace / (1024.0 * 1024 * 1024);
                    double percentFree = (availableSpaceGB * 100) / totalSpaceGB;

                    int status = 0;

                    if (percentFree > 10)
                    {
                        return Tuple.Create(1, $"{availableSpaceGB:F2} GB  /  {totalSpaceGB:F2} GB  -> {percentFree:F1}%");
                    }
                    else if (percentFree < 5)
                    {
                        return Tuple.Create(-1, $"{availableSpaceGB:F2} GB  /  {totalSpaceGB:F2} GB  -> {percentFree:F1}% [>10% recommended]");
                    }
                    else
                    {
                        return Tuple.Create(-1, $"{availableSpaceGB:F2} GB  /  {totalSpaceGB:F2} GB  -> {percentFree:F1}% [>5% required]");
                    }
                }
                else
                {
                    return Tuple.Create(0, "Main is not ready.");
                }
            }
            catch (Exception ex)
            {
                return Tuple.Create(0, $"Error fetching disk space: {ex.Message}");
            }
        }

        public async Task<Tuple<int, string>> FetchRamSpace()
        {
            var memoryStatus = new MEMORYSTATUSEX();
            await Task.Yield();

            if (GlobalMemoryStatusEx(memoryStatus))
            {
                double availableRAMGB = memoryStatus.ullAvailPhys / (1024.0 * 1024 * 1024);
                double totalRAMGB = memoryStatus.ullTotalPhys / (1024.0 * 1024 * 1024);

                if (availableRAMGB > 1)
                {
                    return Tuple.Create(1, $"{availableRAMGB:F2} GB  available");
                }
                else if (availableRAMGB > .25)
                {
                    return Tuple.Create(-1, $"only {availableRAMGB:F2} GB  available  [>.7 GB recommended]");
                }
                else
                {
                    return Tuple.Create(0, $"only {availableRAMGB:F2} GB  available  [>.4 GB required]");
                }

            }
            else
            {
                return Tuple.Create(0, $"Error fetching RAM information.");
            }
        }
        public async Task<Tuple<int, string>> FetchOs()
        {
            string os = DeviceInfo.Platform.ToString(); // Windows, MacCatalyst, Android, iOS
            string version = DeviceInfo.VersionString;  // e.g., "10.0.22621"

            // Example logic using the OS version
            if (os == "Windows")
            {
                Version parsedVersion = new Version(version);
                if (parsedVersion.Build > 19044)
                    return Tuple.Create(1, $"{os} {version}");
                else if (parsedVersion.Build > 17763)
                    return Tuple.Create(-1, $"{os} {version} [at least Windows 10 21H2 recommended (10.0.19044)]");
                else
                    return Tuple.Create(0, $"{os} {version} [at least Windows 10 1809 required] (10.0.17763)");
            }
            await Task.Yield();
            return Tuple.Create(1, $"{os} {version}");
        }
        public async Task<Tuple<int, string>> FetchDownloadSpeed()
        {
            var watch = new Stopwatch();

            // Test Download
            byte[] data;
            double upSpeed;
            using (var client = new System.Net.WebClient())
            {
                watch.Start();
                data = client.DownloadData("https://testfiles.ah-apps.de/100MB.bin");
                watch.Stop();
            }
            upSpeed = ((data.LongLength / watch.Elapsed.TotalSeconds) / (1024.0 * 1024)) * 8; // instead of [Seconds] property

            if (upSpeed > 3)
            {
                return Tuple.Create(1, $"{upSpeed:F2} Mbps");
            }
            else if (upSpeed > 2)
            {
                return Tuple.Create(-1, $"{upSpeed:F2} Mbps");
            }
            else
            {
                return Tuple.Create(0, $"{upSpeed:F2} Mbps");
            }
        }

        public async Task<Tuple<int, string>> FetchUploadSpeed()
        {
            var watch = new Stopwatch();
            // Test Upload
            double downSpeed;
            long fileSize = 10 * 1024 * 1024; // 10 MB file size for testing
            byte[] uploadData = new byte[fileSize];
            new Random().NextBytes(uploadData); // Fill the array with random data
            using (var client = new System.Net.Http.HttpClient())
            {
                var content = new ByteArrayContent(uploadData);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                try
                {
                    watch.Start();
                    // Replace the URL below with an endpoint on your server to accept uploads
                    var response = await client.PostAsync("https://httpbin.org/post", content);
                    watch.Stop();

                    if (!response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine($"Upload failed. Status Code: {response.StatusCode}");
                        return Tuple.Create(0, $"Upload failed");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Upload failed. Error: {ex.Message}");
                    return Tuple.Create(0, $"Upload failed");
                }
            }
            downSpeed = ((fileSize / watch.Elapsed.TotalSeconds) / (1024.0 * 1024))*8; // Bytes -> Megabytes


            if(downSpeed > 5)
            {
                return Tuple.Create(1, $"{downSpeed:F2} Mbps");
            }else if (downSpeed > 3)
            {
                return Tuple.Create(-1, $"{downSpeed:F2} Mbps");
            }
            else
            {
                return Tuple.Create(0, $"{downSpeed:F2} Mbps");
            }
        }


        // Abstract Methods------------------------------------------------------------------------
            // Returns the CPU usage for the host device
        public partial Task<Tuple<int, string>> FetchCpuUsage();

            // Returns either 0 or 1 based on whether FLORIAN is open in the background
        public partial Task<Tuple<int, string>> IsFlorianRunning();

            // Will fetch all of the data reguarding location from the device
        public partial Task<Tuple<int, string>> FetchLocation();
        // ----------------------------------------------------------------------------------------

        public static (long bytesSent, long bytesReceived) GetNetworkUsage()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            long totalBytesSent = 0;
            long totalBytesReceived = 0;

            foreach (var networkInterface in interfaces)
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    var stats = networkInterface.GetIPv4Statistics();
                    totalBytesSent += stats.BytesSent;
                    totalBytesReceived += stats.BytesReceived;
                }
            }

            return (totalBytesSent, totalBytesReceived);
        }

        public virtual async Task<List<HardwareResourcesData>> testHardwareResources(string groupId, string sessionId)
        {
            List<HardwareResourcesData> hardDataEntries = new List<HardwareResourcesData>();
            var florianData = await IsFlorianRunning();

            // Build data for state displays
            Tuple<int, string>[] responses = new Tuple<int, string>[]{
                await FetchBattery(),
                await FetchDiskSpace(),
                await FetchRamSpace(),
                await FetchOs(), 
                await FetchUploadSpeed(), 
                await FetchDownloadSpeed(), 
                await FetchCpuUsage(),
                await FetchLocation(),
                florianData
            };
            string[] titles = new string[] {
                "Battery Percentage",
                "Available  Disk Space",
                "Available RAM",
                "Operating System",
                "Upload Speed",
                "Download Speed",
                "CPU Usage",
                "Location",
                "FLORIAN"
            };
            string[] thresholds = new string[] {
                "\n\tGood\t\t->\t30-100%\n\tWarning\t->\t15-30%\n\tCritical\t->\t0-15%",                                                                                    // Battery
                "\n\tGood\t\t->\t10+%\n\tWarning\t->\t5-10%\n\tCritical\t->\t5-%",                                                                                          // Disk Space
                "\n\tGood\t\t->\t1000+ MB\n\tWarning\t->\t400-700 MB\n\tCritical\t->\t400-MB",                                                                          // RAM
                "\n\tGood\t\t->\tWindows 21H2 LTSC (and newer)\n\tWarning\t->\tWindows 21H2 LTSC - Windows 1809 LTSC\n\tCritical\t->\tWindows 1809 LTSC (and older)",     // OS
                "\n\tGood\t\t->\t3+Mbps\n\tWarning\t->\t2-3 Mbps\n\tCritical\t->\t2- Mbps",                                                                             // Upload Speed
                "\n\tGood\t\t->\t5+Mbps\n\tWarning\t->\t3-5 Mbps\n\tCritical\t->\t3- Mbps",                                                                             // Download Speed
                "\n\tGood\t\t->\t100%(10- mins)\n\tWarning\t->\t100%(10-30 mins)\n\tCritical\t->\t100%(30+ mins)",                                                          // CPU Usage
                "\n\tGood\t\t->\tAccuracy of  50- ft\n\tWarning\t->\tAccuracy of  50-100 ft\n\tCritical\t->\tAccuracy of  100+ ft",                                                                             // Location accuracy
                "\n\tGood\t\t->\tinstalled and running\n\tWarning\t->\tinstalled but not running\n\tCritical\t->\tnot installed",                                         // Florian
            };

            // Plug data into state displays
            for (int i=0; i<responses.Length; i++)
            {
                if (i == 0 && IsCharging())
                {
                    hardDataEntries.Add(
                        new HardwareResourcesData(
                            groupId,
                            sessionId,
                            titles[i],
                            responses[i].Item1,
                            responses[i].Item2 + $"\n\n** Status Criteria **{thresholds[i]}",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            false,
                            (florianData.Item1 == 1) ? true : false));
                }
                else
                {
                    hardDataEntries.Add(
                        new HardwareResourcesData(
                            groupId,
                            sessionId,
                            titles[i],
                            responses[i].Item1,
                            responses[i].Item2 + $"\n\n** Status Criteria **{thresholds[i]}",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            (florianData.Item1 == 1) ? true : false));
                }
            }

            return hardDataEntries;
        }
    }
}