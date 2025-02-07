using System.Diagnostics;
using FlorianMezzo.Constants;
using FlorianMezzo.Controls;
using FlorianMezzo.Controls.db;

namespace FlorianMezzo.Pages;

public partial class HealthCheck : ContentPage
{

    private readonly HealthCheckService _healthCheckService;
    private AppSettings Settings = new AppSettings();

    public HealthCheck(HealthCheckService healthCheckService)
	{
        InitializeComponent();
        _healthCheckService = healthCheckService;
        Settings.LoadOrCreateSettings();
        //Debug.WriteLine("HealthChecker Page Initialized");
        initESDs();

        // Subscribe to required events
        _healthCheckService._statusChangeEvent += ServiceStatusHandler; // service status changed
        _healthCheckService._newdataEvent += NewDataHandler;
        Settings._newGroupIdEvent += NewGroupIdHandler;

        // Atempt to update UI
        UpdateServiceUI();
        UpdateStateDisplays(Settings.LastGroupId);
    }

    private async void initESDs()
    {
        await MainThread.InvokeOnMainThreadAsync(() => {
            tileSoftDependencyESD.MainStateDisplay = new StateDisplay("Soft Dependencies (Workspace Tiles)", "--", 0);
            coreSoftDependencyESD.MainStateDisplay = new StateDisplay("Soft Dependencies", "--", 0);
            resourceESD.MainStateDisplay = new StateDisplay("Hardware Resources", "--", 0);
        });
    }

    private async void ShowFetch()
    {
        await MainThread.InvokeOnMainThreadAsync(() => {
            tileSoftDependencyESD.MainStateDisplay = new StateDisplay("Soft Dependencies (Workspace Tiles)", "Fetching...", -2);
            coreSoftDependencyESD.MainStateDisplay = new StateDisplay("Soft Dependencies", "Fetching...", -2);
            resourceESD.MainStateDisplay = new StateDisplay("Hardware Resources", "Fetching...", -2);
        });
    }

    // Health Check Service interactions--------------------------------------
        //export health check to csv
    private async void ExportToCSV(object sender, EventArgs e)
    {
        string groupId = Settings.LastGroupId;
        if (groupId == "") { return; }

        // Define the file path
        string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        string filePath = Path.Combine(downloadsPath, $"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH-mm-ss")}_FlorianHealthCheck.csv");

        // Fetch batched data
        LocalDbService dbService = _healthCheckService.GetDbService();
        Dictionary<string, List<DbData>> statuses = await dbService.GetByGroupId(groupId);

        // Build the file
        var csvContent = "GroupId, SessionId, Title, Status, Feedback, DateTime, Averageable\n";
        foreach(string sectionTitle in statuses.Keys)
        {
            csvContent += $"\n{sectionTitle}\n";
            foreach (var dataEntry in statuses[sectionTitle])
            {
                csvContent += dataEntry.ToString() + "\n";
            }
        }

        // Write to the CSV file
        await File.WriteAllTextAsync(filePath, csvContent);

        await DisplayAlert("Success", $"Health Check exported to {filePath}", "OK");
    }

    private void ToggleService(object sender, EventArgs e)
    {
        if (_healthCheckService.GetRunningStatus() > 0)
        {
            _healthCheckService?.Stop();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                serviceStatusImage.Source = "xmark.png";  // Example: change image to X mark
                serviceStatus.BackgroundColor = Color.FromArgb("#F94620"); // Red

                toggleServiceBtn.Text = "Start Service";
            });
        }
        else
        {
            _healthCheckService?.Start();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                serviceStatusImage.Source = "check.png";  // Example: change image to a checkmark
                serviceStatus.BackgroundColor = Color.FromArgb("#66E44C"); // Green

                toggleServiceBtn.Text = "Stop Service";
            });
        }
        UpdateServiceUI();
    }

        // Fetch Methods
    private int GetIntHr()
    {
        return (int)(Settings.Interval / 3600);
    }
    private int GetIntMin()
    {
        return (int)((Settings.Interval % 3600) /60);
    }
    private int GetIntSec()
    {
        return (int)(Settings.Interval % 60);
    }
    private string GetFetchCountString()
    {
        return _healthCheckService.GetCount().ToString();
    }

    // Update UI based on latest info
    private void UpdateServiceUI()
    {
        Settings.LoadOrCreateSettings();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Update count
            fetchCountNum.Text = GetFetchCountString();

            var temp1 = GetIntHr();
            var temp2 = GetIntMin();
            var temp3 = GetIntSec();

            // Update Interval
            intervalHrNum.Text = GetIntHr().ToString();
            intervalMinNum.Text = GetIntMin().ToString();
            intervalSecNum.Text = GetIntSec().ToString();
        });
    }

        // handle service status changed
    private void ServiceStatusHandler(object sender, StatusChangeEvent newStatusEvent)
    {
        //Debug.WriteLine($"Health Checker Service status changed: {newStatusEvent.Status}");

        // On main thread, update UI
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Update service status image
            if (newStatusEvent.Status == -1)        // Stopping
            {
                serviceStatusImage.Source = "running.png";  // Example: change image to running icon
                serviceStatus.BackgroundColor = Color.FromArgb("#83858a"); // Grey
            }
            else if (newStatusEvent.Status == 0)    // Not Running
            {
                serviceStatusImage.Source = "xmark.png";  // Example: change image to X mark
                serviceStatus.BackgroundColor = Color.FromArgb("#F94620"); // Red
            }
            else if (newStatusEvent.Status == 1)    // Running
            {
                serviceStatusImage.Source = "check.png";  // Example: change image to a checkmark
                serviceStatus.BackgroundColor = Color.FromArgb("#66E44C"); // Green
            }
            else if (newStatusEvent.Status == 2)    // Fetching
            {
                serviceStatusImage.Source = "running.png";  // Example: change image to running icon
                serviceStatus.BackgroundColor = Color.FromArgb("#83858a"); // Grey
            }
            else
            {
                serviceStatusImage.Source = "xmark.png";  // Example: change image to X mark
                serviceStatus.BackgroundColor = Color.FromArgb("#F94620"); // Red
            }
        });
        UpdateStateDisplays(Settings.LastGroupId);
    }

    private async void UpdateStateDisplays(string groupId)
    {
        // if there is no valid group ID, exit
        if(groupId == "") { return; }

        // Fetch batched data
        LocalDbService dbService = _healthCheckService.GetDbService();
        Dictionary<string, List<DbData>> statuses = await dbService.GetByGroupId(groupId);

        // On main thread, update UI
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // state displays
            tileSoftDependencyESD.UpdateDropdownContent(statuses["tileSoftDependencies"]);
            coreSoftDependencyESD.UpdateDropdownContent(statuses["coreSoftDependencies"]);
            resourceESD.UpdateDropdownContent(statuses["hardwareResources"]);

        });
    }
    // -----------------------------------------------------------------------



    // handle new settings ---------------------------------------------------
    private void NewGroupIdHandler(object sender, NewGroupIdEvent newGroupIdEvent)
    {
        Debug.WriteLine($"New group ID recieved: {newGroupIdEvent.GroupId}");

        UpdateStateDisplays(newGroupIdEvent.GroupId);
        UpdateServiceUI();
    }
    private void NewDataHandler(object sender, NewDataEvent newDataEvent)
    {
        Debug.WriteLine($"New data recieved: {newDataEvent.GroupId}");

        UpdateStateDisplays(newDataEvent.GroupId);
    }
    // -----------------------------------------------------------------------



    // Navigation methods-----------------------------------------------------
    private async void redirectToMain(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }
    private async void redirectToCompatibility(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Compatibility));
    }
    private async void redirectToHealthCheck(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HealthCheck));
    }
    private async void redirectToMezzoAnalysis(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MezzoAnalysis));
    }
    private async void redirectToMore3AM(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(More3AM));
    }
    // -----------------------------------------------------------------------
}