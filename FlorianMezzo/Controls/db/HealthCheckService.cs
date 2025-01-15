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
        public event EventHandler<NewDataEvent> _newdataEvent; // Event to notify subscribers
        private int fetchCount = 0;
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


                    List<CoreSoftDependencyData> coreSoftDependencyEntries = await _urlChecker.testCoreSoftDependencies(groupId);
                    List<TileSoftDependencyData> tileSoftDependencyEntries = await _urlChecker.testTileSoftDependencies(groupId);
                    List<HardwareResourcesData> hardwareResourceEntries = await _resourceChecker.testHardwareResources(groupId);
                    Debug.WriteLine($"COMPLETED collecting status data");

                    // Write data
                    await _dbService.WriteToDb(coreSoftDependencyEntries);
                    await _dbService.WriteToDb(tileSoftDependencyEntries);
                    await _dbService.WriteToDb(hardwareResourceEntries);

                    // Broadcast new data has been written
                    BroadcastNewData(new NewDataEvent(groupId));

                    // Wait during the interval
                    Task.Delay(5000).Wait();
                    if (shouldEnd)
                    {
                        Debug.WriteLine("Health Check Service Terminated");
                        return;
                    }

                    // increment count
                    fetchCount++;
                }
            });
        }

        public void Stop()
        {
            shouldEnd = true;
        }

        protected virtual void BroadcastNewData(NewDataEvent e)
        {
            _newdataEvent?.Invoke(this, e);
        }

        public int GetCount()
        {
            return fetchCount;
        }

        public LocalDbService GetDbService()
        {
            return _dbService;
        }
    }

    public class NewDataEvent: EventArgs
    {
        public string GroupId { get; }

        // constructor
        public NewDataEvent(string groupId)
        {
            GroupId = groupId;
        }
    }
}