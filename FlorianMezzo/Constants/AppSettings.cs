using System.Diagnostics;
using System.Text.Json;


namespace FlorianMezzo.Constants
{
    public class AppSettings
    {
        public int Interval { get; set; }
        public string LastGroupId { get; set; }
        private readonly object lockObj = new object();

        private static readonly object settingsFileLock = new object();

        //public event EventHandler<NewGroupIdEvent> _newGroupIdEvent; // Event to notify subscribers of new data

        public AppSettings()
        {
        }
        public AppSettings(int inInterval, string inId)
        {
            Task.Run(() => {
                Interval = inInterval;
                LastGroupId = inId;
            });
        }
        public AppSettings(AppSettings baseObject)
        {
            Task.Run(() => {
                Interval = baseObject.Interval;
                LastGroupId = baseObject.LastGroupId;
                SaveSettings(baseObject);
            });
        }

        public async Task LoadOrCreateSettings()
        {
            await Task.Run(() =>
            {
                lock (lockObj)
                {
                    string filePath = Path.Combine(FileSystem.AppDataDirectory, "settings.json");
                    //Debug.WriteLine($"Searching for files at {filePath}");

                    if (File.Exists(filePath))
                    {
                        // File exists, read and deserialize it
                        //Debug.WriteLine($"Settings file found at\n\t{filePath}");
                        var rawJson = File.ReadAllText(filePath);
                        var currentSettings = JsonSerializer.Deserialize<AppSettings>(rawJson);
                        if (currentSettings != null) { UpdateSettings(currentSettings); };
                    }
                    else
                    {
                        // File does not exist, create it with default values
                        Debug.WriteLine($"Settings file NOT found, creating one at\n\t{filePath}");
                        var defaultSettings = new AppSettings(10, "");

                        var defaultJson = JsonSerializer.Serialize(defaultSettings, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(filePath, defaultJson);

                        UpdateSettings(defaultSettings);
                    }
                }
            });
        }

        public void SaveSettings(AppSettings settings)
        {
            string filePath = Path.Combine(FileSystem.AppDataDirectory, "settings.json");

            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            try
            {
                // Log where this method was called from
                var stackTrace = new System.Diagnostics.StackTrace(true);
                Debug.WriteLine("SaveSettings called from:");
                Debug.WriteLine(stackTrace.ToString());

                lock (settingsFileLock)
                {
                    File.WriteAllText(filePath, json);
                    Debug.WriteLine($"Wrote new settings to {filePath}");
                }
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"[IOException] {ex.Message}");
                Debug.WriteLine($"StackTrace: {Environment.StackTrace}");
            }
        }

        public void UpdateInterval(int newInterval)
        {
            LoadOrCreateSettings();
            AppSettings newSettings = new AppSettings
            {
                Interval = newInterval,
                LastGroupId = this.LastGroupId
            };
            SaveSettings(newSettings);
        }
        public void UpdateLastGroupId(string newId)
        {
            LoadOrCreateSettings();
            AppSettings newSettings = new AppSettings
            {
                Interval = this.Interval,
                LastGroupId = newId
            };
            SaveSettings(newSettings);
            //BroadcastNewGroupId(new NewGroupIdEvent(newId));
        }
        public void UpdateSettings(AppSettings newSettings)
        {
            Interval = newSettings.Interval;
            LastGroupId = newSettings.LastGroupId;
            Debug.WriteLine($"Current settings:\n\tInterval: {Interval}\n\tLastGroupId: {LastGroupId}\n\t");
        }

        /*
        protected virtual void BroadcastNewGroupId(NewGroupIdEvent e)
        {
            _newGroupIdEvent?.Invoke(this, e);
        }
        */
    }

    public class NewIntervalEvent : EventArgs
    {
        public int Interval;

        // constructor
        public NewIntervalEvent(int interval)
        {
            Interval = interval;
        }
    }
    public class NewGroupIdEvent : EventArgs
    {
        public string GroupId { get; }

        // constructor
        public NewGroupIdEvent(string groupId)
        {
            GroupId = groupId;
        }
    }
}
