using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FlorianMezzo.Controls;

namespace FlorianMezzo.Controls.db
{
    public class HealthCheckService
    {
        private readonly LocalDbService _dbService;
        private bool shouldEnd;

        public HealthCheckService(LocalDbService dbService) {
            _dbService = dbService;
            shouldEnd = false;
        }

        // Fetch all status data and write it to its respective tables
        public void Start()
        {
            UrlChecker _urlChecker = new();
            ResourceChecker _resourceChecker = new();

            Task.Run(async () => { 
                while (!shouldEnd)
                {
                    string groupId = Guid.NewGuid().ToString();

                    // Fetch data
                    Debug.WriteLine($"Collecting status data at {DateTime.Now}...");
                    List<SoftDependencyData> softDependencyEntries = await _urlChecker.testSoftDependencies(groupId);
                    Debug.WriteLine($"COMPLETED collecting status data");

                    // Write data
                    await _dbService.WriteManyToSoftwareDependency(softDependencyEntries);

                    // Wait during the interval
                    Task.Delay(5000).Wait();
                    if (shouldEnd)
                    {
                        Debug.WriteLine("Health Check Service Terminated");
                        return;
                    }
                }
            });
        }

        public void Stop()
        {
            shouldEnd = true;
        }
    }
}
