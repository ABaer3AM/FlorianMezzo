using System.Collections.ObjectModel;
using System.Diagnostics;
using FlorianMezzo.Controls;
using FlorianMezzo.Constants;
using FlorianMezzo.Controls.db;

namespace FlorianMezzo.Pages;

public partial class InstallGuide : ContentPage
{
    private AppSettings Settings = new AppSettings();

    public InstallGuide()
	{
		InitializeComponent();
        InitStateDisplays();
        Settings.LoadOrCreateSettings();
        UpdateStateDisplays();
    }



    private async void OpenFlorianInStore(object sender, EventArgs e)
    {
        // Microsoft Store URL for the app
        var storeUrl = "https://www.microsoft.com/store/apps/9p93s9wb325x";
        var storeAppUrl = "ms-windows-store://pdp/?ProductId=9p93s9wb325x";

        // Open the URL using the Launcher
        if (Uri.IsWellFormedUriString(storeUrl, UriKind.Absolute))
        {
            await Launcher.OpenAsync(new Uri(storeAppUrl));
        }
        else
        {
            if (Uri.IsWellFormedUriString(storeUrl, UriKind.Absolute))
            {
                await Launcher.OpenAsync(new Uri(storeUrl));
            }
            else
            {
                await DisplayAlert("Error", "Invalid Store URL", "OK");
            }
        }
    }

    private async void InitStateDisplays()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // state displays
            tileSoftDependencyESD.MainStateDisplay = new StateDisplay("Soft Dependencies (Workspace Tiles)", "Unfetched", 0);
            coreSoftDependencyESD.MainStateDisplay = new StateDisplay("Soft Dependencies", "Unfetched", 0);
            resourceESD.MainStateDisplay = new StateDisplay("Hardware Resources", "Unfetched", 0);

        });
    }

    // Methods to Fetch Status Data---------------------------------------------------------------------------------------------
    private async void getStatusOfSoftDependencies()
    {
        UrlChecker urlCheckerObj = new UrlChecker();

        // Update visual to show fetch is running
        Debug.WriteLine($"Started Task running at {DateTime.Now}");

        /*
        // run test
        Tuple<StateDisplay, List<StateDisplay>> sdStates = await urlCheckerObj.testSoftDependencies();

        await MainThread.InvokeOnMainThreadAsync(() => {
            softDependencyESD.MainStateDisplay = sdStates.Item1;
            softDependencyESD.StateDisplays = sdStates.Item2;
        });
        */
    }
    private async void getStatusOfHardwareResources()
    {
        ResourceChecker resourceCheckerObj = new ResourceChecker();

        // Update visual to show fetch is running
        Debug.WriteLine($"Started Task running at {DateTime.Now}");

        /*
        // run test
        Tuple<StateDisplay, List<StateDisplay>> sdStates = await resourceCheckerObj.testHardwareResources();

        await MainThread.InvokeOnMainThreadAsync(() => {
            resourceESD.MainStateDisplay = sdStates.Item1;
            resourceESD.StateDisplays = sdStates.Item2;
        });
        */
    }
    //-----------------------------------------------------------------------------------------------------------------------                       
    



    /* Overloaded method-
     * (sender,e) from button 
     * () called from another function
     */
    private async void UpdateStateDisplays(object sender, EventArgs e)
    {
        UpdateStateDisplays();
    }
    private async void UpdateStateDisplays()
    {
        string groupId = Settings.LastGroupId;

        // if there is no valid group ID, exit
        if (groupId == "") { return; }

        // Fetch batched data
        LocalDbService dbService =new LocalDbService();
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


    // Navigation methods-----------------------------------------------------
    private async void redirectToMain(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }
    private async void redirectToInstallGuide(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(InstallGuide));
    }
    private async void redirectToHealthCheck(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HealthCheck));
    }
    private async void redirectToITHandOff(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ItHandOff));
    }
    private async void redirectToFlorianBTS(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FlorianBTS));
    }
    private async void redirectToMore3AM(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(More3AM));
    }
    // -----------------------------------------------------------------------
}