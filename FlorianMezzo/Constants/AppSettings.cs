using System.Diagnostics;
using System.Text.Json;


namespace FlorianMezzo.Constants
{
    public class AppSettings
    {
        public int Interval { get; set; }
        public string LastGroupId { get; set; }
        public event EventHandler<NewSettingEvent> _newSettingEvent; // Event to notify subscribers of new data

        public AppSettings()
        {
        }
        public AppSettings(int inInterval, string inId)
        {
            Interval = inInterval;
            LastGroupId = inId;
        }
        public AppSettings(AppSettings baseObject)
        {
            Interval = baseObject.Interval;
            LastGroupId = baseObject.LastGroupId;
            SaveSettings(baseObject);
        }

        public void LoadOrCreateSettings()
        {
            string filePath = Path.Combine(FileSystem.AppDataDirectory, "settings.json");
            //Debug.WriteLine($"Searching for files at {filePath}");

            if (File.Exists(filePath))
            {
                // File exists, read and deserialize it
                Debug.WriteLine($"Settings file found at {filePath}");
                var rawJson = File.ReadAllText(filePath);
                var currentSettings =  JsonSerializer.Deserialize<AppSettings>(rawJson);
                UpdateSettings(currentSettings);
            }
            else
            {
                // File does not exist, create it with default values
                Debug.WriteLine($"Settings file NOT found, creating one at {filePath}");
                var defaultSettings = new AppSettings(60, "");

                var defaultJson = JsonSerializer.Serialize(defaultSettings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, defaultJson);

                UpdateSettings(defaultSettings);
            }
        }

        public void SaveSettings(AppSettings settings)
        {
            string filePath = Path.Combine(FileSystem.AppDataDirectory, "settings.json");

            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
            Debug.WriteLine($"Wrote new settings to {filePath}");
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
            BroadcastNewSetting(new NewSettingEvent(this.LastGroupId, newInterval));
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
            BroadcastNewSetting(new NewSettingEvent(newId, this.Interval));
        }
        public void UpdateSettings(AppSettings newSettings)
        {
            Interval = newSettings.Interval;
            LastGroupId = newSettings.LastGroupId;
        }

        protected virtual void BroadcastNewSetting(NewSettingEvent e)
        {
            _newSettingEvent?.Invoke(this, e);
        }

    }

    public class NewSettingEvent : EventArgs
    {
        public int Interval;
        public string GroupId { get; }

        // constructor
        public NewSettingEvent(string groupId, int interval)
        {
            GroupId = groupId;
            Interval = interval;
        }
    }
}
