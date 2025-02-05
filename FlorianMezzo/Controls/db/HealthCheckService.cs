using System.Diagnostics;
using System.Runtime.CompilerServices;
using FlorianMezzo.Constants;

namespace FlorianMezzo.Controls.db
{
    public class HealthCheckService
    {
        private readonly LocalDbService _dbService;
        public event EventHandler<NewDataEvent> _newdataEvent; // Event to notify subscribers of new data
        public event EventHandler<StatusChangeEvent> _statusChangeEvent; // Event to notify subscribers of a change in running status
        private AppSettings Settings;
        private int interval;
        private int fetchCount = 0;
        private string latestGroupId = "";
        private int status;

        public HealthCheckService(LocalDbService dbService) {
            Debug.WriteLine("Health Check Service Instanciated");
            _dbService = dbService;
            // Fetch Interval
            Settings = new AppSettings();
            SetStatus(0);
        }

        // Fetch all status data and write it to its respective tables
        public void Start()
        {
            SetStatus(1);
            fetchCount = 0;

            Settings.LoadOrCreateSettings();
            interval = Settings.Interval;
            PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(interval));

            string sessionId = Guid.NewGuid().ToString();

            UrlChecker _urlChecker = new();
            ResourceChecker _resourceChecker = new();

            Task.Run(async () => {
                do
                {
                    string groupId = Guid.NewGuid().ToString();

                    // Fetch data
                    Debug.WriteLine($"Collecting status data at {DateTime.Now}...");

                    if (status != -1) { SetStatus(2); }

                    List<CoreSoftDependencyData> coreSoftDependencyEntries = await _urlChecker.testCoreSoftDependencies(groupId, sessionId);
                    List<TileSoftDependencyData> tileSoftDependencyEntries = await _urlChecker.testTileSoftDependencies(groupId, sessionId);
                    List<HardwareResourcesData> hardwareResourceEntries = await _resourceChecker.testHardwareResources(groupId, sessionId);
                    Debug.WriteLine($"COMPLETED collecting status data");

                    // Write data
                    await _dbService.WriteToDb(coreSoftDependencyEntries);
                    await _dbService.WriteToDb(tileSoftDependencyEntries);
                    await _dbService.WriteToDb(hardwareResourceEntries);
                    // increment count
                    fetchCount++;

                    if (status != -1) { SetStatus(1); }

                    // Broadcast new data has been written
                    BroadcastNewData(new NewDataEvent(groupId));
                    this.Settings.UpdateLastGroupId(groupId);
                    latestGroupId = groupId;
                } while (await timer.WaitForNextTickAsync() && status > 0);

                Debug.WriteLine("Health Check Service Terminated");
                SetStatus(0);
            });
        }

        public void Stop()
        {
            SetStatus(-1);
        }

        public int GetCount()
        {
            return fetchCount;
        }

        public string GetLatestGroupId()
        {
            return latestGroupId;
        }
        private void SetStatus(int statusIn)
        {
            status = statusIn;
            BroadcastStatusChange(new StatusChangeEvent(status));
        }

        public int GetRunningStatus()
        {
            return status;
        }

        public LocalDbService GetDbService()
        {
            return _dbService;
        }
        public int GetInterval()
        {
            return interval;
        }

        // Event Interactions ---------------------------------------------------------------------
        protected virtual void BroadcastNewData(NewDataEvent e)
        {
            _newdataEvent?.Invoke(this, e);
        }
        protected virtual void BroadcastStatusChange(StatusChangeEvent e)
        {
            _statusChangeEvent?.Invoke(this, e);
        }

        private void newGroupIdHandler(object sender, NewGroupIdEvent groupIdEvent)
        {
            Debug.WriteLine($"New groupId recieved from settings: {groupIdEvent.GroupId}");
        }

        // --------------------------------------------------------------------------------------------
    }


    // Event Classes ------------------------------------------------------------------------------
    public class NewDataEvent: EventArgs
    {
        public string GroupId { get; }

        // constructor
        public NewDataEvent(string groupId)
        {
            GroupId = groupId;
        }
    }

    public class StatusChangeEvent : EventArgs
    {
        public int Status { get; }

        // constructor
        public StatusChangeEvent(int statusIn)
        {
            Status = statusIn;
        }
    }


    // --------------------------------------------------------------------------------------------
}